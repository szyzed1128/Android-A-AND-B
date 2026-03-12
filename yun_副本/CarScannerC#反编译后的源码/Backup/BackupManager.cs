using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Garage;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using ICSharpCode.SharpZipLib.Zip;

namespace CarScannerXamarinForms.Backup
{
	// Token: 0x02000BFC RID: 3068
	internal class BackupManager
	{
		// Token: 0x06005C1E RID: 23582 RVA: 0x0043BA34 File Offset: 0x00439C34
		public static async Task<bool> CreateBackup(bool includeRecords, IProgress<int> progress)
		{
			bool flag;
			try
			{
				new GarageModel().Initialize(true);
				string externalStoragePath = FileSystemHelper.ExternalStoragePath;
				List<string> list = Directory.GetFiles(FileSystemHelper.LocalStoragePath).ToList<string>();
				string externalStoragePath2 = FileSystemHelper.ExternalStoragePath;
				List<string> filtered_files = new List<string>();
				filtered_files.AddRange(list.Where((string x) => x.ToLowerInvariant().Contains("drivecycles.bin")));
				try
				{
					filtered_files.Add(list.First((string x) => x.EndsWith("garage.json")));
				}
				catch (Exception)
				{
				}
				try
				{
					filtered_files.Add(list.First((string x) => x.EndsWith("coding.bak")));
				}
				catch (Exception)
				{
				}
				if (includeRecords)
				{
					filtered_files.AddRange(list.Where((string x) => x.ToLowerInvariant().EndsWith(".brc")).ToArray<string>());
					if (externalStoragePath != null)
					{
						string[] array = (from x in Directory.GetFiles(Path.Combine(externalStoragePath, "records"))
							where x.ToLowerInvariant().EndsWith(".brc")
							select x).ToArray<string>();
						filtered_files.AddRange(array);
					}
				}
				string text = Path.Combine(FileSystemHelper.LocalStoragePath, "backup");
				if (externalStoragePath != null)
				{
					text = Path.Combine(externalStoragePath, "backup");
				}
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				DateTime nowSafe = DateTimeNowHelper.NowSafe;
				string text2 = string.Format("{0}-{1}-{2} {3}-{4}-{5}.cbz", new object[]
				{
					nowSafe.Year,
					nowSafe.Month.ToString("00"),
					nowSafe.Day.ToString("00"),
					nowSafe.Hour.ToString("00"),
					nowSafe.Minute.ToString("00"),
					nowSafe.Second.ToString("00")
				});
				string text3 = Path.Combine(text, text2);
				using (FileStream fstream = File.Open(text3, FileMode.Create))
				{
					using (ZipOutputStream zstream = new ZipOutputStream(fstream))
					{
						zstream.UseZip64 = 0;
						zstream.SetLevel(9);
						ZipEntry zipEntry = new ZipEntry("BACKUP");
						zstream.PutNextEntry(zipEntry);
						byte[] bytes = Encoding.UTF8.GetBytes(text2);
						zstream.Write(bytes, 0, bytes.Length);
						zstream.CloseEntry();
						BackupManager.WriteSettingsEntry(zstream);
						int num2;
						for (int i = 0; i < filtered_files.Count; i = num2 + 1)
						{
							string text4 = filtered_files[i];
							using (FileStream input_stream = File.Open(text4, FileMode.Open))
							{
								ZipEntry zipEntry2 = new ZipEntry(text4);
								zstream.PutNextEntry(zipEntry2);
								await input_stream.CopyToAsync(zstream);
								zstream.CloseEntry();
							}
							FileStream input_stream = null;
							int num = i * 100 / filtered_files.Count;
							progress.Report(num);
							num2 = i;
						}
						await zstream.FlushAsync();
						zstream.Finish();
					}
					ZipOutputStream zstream = null;
				}
				FileStream fstream = null;
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x0043BA80 File Offset: 0x00439C80
		private static void WriteSettingsEntry(ZipOutputStream zstream)
		{
			for (int i = 0; i < BackupManager.PropertiesList.Length; i++)
			{
				BackupManager.PutEntry(zstream, BackupManager.PropertiesList[i]);
			}
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x0043BAAC File Offset: 0x00439CAC
		private static void PutEntry(ZipOutputStream zstream, string name)
		{
			PropertyInfo property = typeof(SharedSettings).GetProperty(name);
			if (property == null)
			{
				return;
			}
			object value = property.GetValue(SharedSettings.Current);
			if (value == null)
			{
				return;
			}
			byte[] array = new byte[0];
			if (value is int || value is Enum)
			{
				array = BitConverter.GetBytes((int)value);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
			}
			else if (value is bool)
			{
				array = BitConverter.GetBytes((bool)value);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
			}
			else if (value is long)
			{
				array = BitConverter.GetBytes((long)value);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
			}
			else if (value is double)
			{
				array = BitConverter.GetBytes((double)value);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
			}
			else if (value is decimal)
			{
				string text = ((decimal)value).ToString(CultureInfo.InvariantCulture);
				array = Encoding.UTF8.GetBytes(text);
			}
			else
			{
				if (!(value is string))
				{
					throw new Exception("Parameter " + name + " is unknown type");
				}
				array = Encoding.UTF8.GetBytes((string)value);
			}
			ZipEntry zipEntry = new ZipEntry(name);
			zstream.PutNextEntry(zipEntry);
			zstream.Write(array, 0, array.Length);
			zstream.CloseEntry();
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x0043BC10 File Offset: 0x00439E10
		private static void ReadSettingsEntry(ZipInputStream zStream, ZipEntry zipEntry)
		{
			PropertyInfo property = typeof(SharedSettings).GetProperty(zipEntry.Name);
			if (property == null)
			{
				throw new Exception("Property " + zipEntry.Name + " not found in Settings!");
			}
			object value = property.GetValue(SharedSettings.Current);
			byte[] array = new byte[0];
			if (value is int)
			{
				array = new byte[4];
				zStream.Read(array, 0, array.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				int num = BitConverter.ToInt32(array, 0);
				property.SetValue(SharedSettings.Current, num);
				return;
			}
			if (value is bool)
			{
				array = new byte[1];
				zStream.Read(array, 0, array.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				bool flag = BitConverter.ToBoolean(array, 0);
				property.SetValue(SharedSettings.Current, flag);
				return;
			}
			if (value is long)
			{
				array = new byte[8];
				zStream.Read(array, 0, array.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				long num2 = BitConverter.ToInt64(array, 0);
				property.SetValue(SharedSettings.Current, num2);
				return;
			}
			if (value is string)
			{
				List<byte> list = new List<byte>(128);
				array = new byte[2048];
				for (;;)
				{
					int num3 = zStream.Read(array, 0, array.Length);
					if (num3 <= 0)
					{
						break;
					}
					list.AddRange(array.Take(num3));
				}
				string @string = Encoding.UTF8.GetString(list.ToArray());
				property.SetValue(SharedSettings.Current, @string);
				return;
			}
			if (value is Enum)
			{
				array = new byte[4];
				zStream.Read(array, 0, array.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				int num4 = BitConverter.ToInt32(array, 0);
				object obj = Enum.ToObject(property.PropertyType, num4);
				property.SetValue(SharedSettings.Current, obj);
				return;
			}
			if (value is decimal)
			{
				List<byte> list2 = new List<byte>(128);
				array = new byte[2048];
				for (;;)
				{
					int num5 = zStream.Read(array, 0, array.Length);
					if (num5 <= 0)
					{
						break;
					}
					list2.AddRange(array.Take(num5));
				}
				decimal num6;
				if (decimal.TryParse(Encoding.UTF8.GetString(list2.ToArray()), NumberStyles.Any, CultureInfo.InvariantCulture, out num6))
				{
					property.SetValue(SharedSettings.Current, num6);
					return;
				}
			}
			else if (value is double)
			{
				array = new byte[8];
				zStream.Read(array, 0, array.Length);
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array);
				}
				double num7 = BitConverter.ToDouble(array, 0);
				property.SetValue(SharedSettings.Current, num7);
			}
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x0043BEB0 File Offset: 0x0043A0B0
		public static List<FileSystemElement> GetFiles()
		{
			if (PlatformHelper.IsAndroid)
			{
				PlatformHelper.DroidService.FileMigrator_MigrateBackups(null);
			}
			string text = Path.Combine(FileSystemHelper.LocalStoragePath, "backup");
			List<FileSystemElement> list = new List<FileSystemElement>();
			list.AddRange(BackupManager.GetBackupFilesFromFolder(FileSystemHelper.LocalStoragePath));
			list.AddRange(BackupManager.GetBackupFilesFromFolder(text));
			string externalStoragePath = FileSystemHelper.ExternalStoragePath;
			if (externalStoragePath != null)
			{
				string text2 = Path.Combine(externalStoragePath, "backup");
				if (Directory.Exists(text2))
				{
					list.AddRange(BackupManager.GetBackupFilesFromFolder(text2));
				}
			}
			return list.OrderBy((FileSystemElement x) => x.Name).ToList<FileSystemElement>();
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x0043BF58 File Offset: 0x0043A158
		private static List<FileSystemElement> GetBackupFilesFromFolder(string backup_folder)
		{
			if (Directory.Exists(backup_folder))
			{
				return (from x in Directory.GetFiles(backup_folder)
					where x.ToLowerInvariant().EndsWith(".cbz")
					select new FileSystemElement(x, FileSystemElementType.File, false)).ToList<FileSystemElement>();
			}
			return new List<FileSystemElement>(0);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x0043BFC8 File Offset: 0x0043A1C8
		public static async Task<string> LoadFromBackup(string backup_file)
		{
			string text;
			try
			{
				using (FileStream fstream = File.Open(backup_file, FileMode.Open))
				{
					using (ZipInputStream zipstream = new ZipInputStream(fstream))
					{
						ZipEntry nextEntry;
						while ((nextEntry = zipstream.GetNextEntry()) != null)
						{
							string fileName = Path.GetFileName(nextEntry.Name);
							if (!string.IsNullOrEmpty(fileName))
							{
								if (BackupManager.PropertiesList.Contains(fileName))
								{
									BackupManager.ReadSettingsEntry(zipstream, nextEntry);
								}
								else if (fileName != "BACKUP")
								{
									await BackupManager.SaveAsFile(zipstream, fileName);
								}
							}
						}
					}
					ZipInputStream zipstream = null;
				}
				FileStream fstream = null;
				GarageModel garageModel = new GarageModel();
				if (PlatformHelper.IsAndroid)
				{
					PlatformHelper.DroidService.FileMigrator_MigrateBackups(null);
				}
				bool flag = garageModel.RestoreFromBackup();
				PIDOverrideDictionary.Instance.Load();
				if (flag)
				{
					text = SharedSettings.Current.CurrentCarName;
				}
				else
				{
					text = "Car not found";
				}
			}
			catch (Exception ex)
			{
				text = "Error!\n" + ex.Message;
			}
			return text;
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x0043C00C File Offset: 0x0043A20C
		private static async Task SaveAsFile(ZipInputStream zipstream, string fileName)
		{
			try
			{
				using (FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath(fileName), FileMode.Create))
				{
					byte[] array = new byte[2048];
					for (;;)
					{
						int num = zipstream.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						fileStream.Write(array, 0, num);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x0043C057 File Offset: 0x0043A257
		public static bool IsBackupValid(Stream fstream)
		{
			return BackupManager.ZipContainsEntry(fstream, "BACKUP");
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x0043C064 File Offset: 0x0043A264
		public static bool ZipContainsEntry(Stream fstream, string entry)
		{
			bool flag;
			try
			{
				using (ZipInputStream zipInputStream = new ZipInputStream(fstream))
				{
					zipInputStream.IsStreamOwner = false;
					ZipEntry nextEntry;
					while ((nextEntry = zipInputStream.GetNextEntry()) != null)
					{
						if (nextEntry.Name == entry)
						{
							fstream.Seek(0L, SeekOrigin.Begin);
							return true;
						}
					}
					fstream.Seek(0L, SeekOrigin.Begin);
					flag = false;
				}
			}
			catch (Exception)
			{
				fstream.Seek(0L, SeekOrigin.Begin);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x00002050 File Offset: 0x00000250
		public BackupManager()
		{
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x0043C0EC File Offset: 0x0043A2EC
		// Note: this type is marked as 'beforefieldinit'.
		static BackupManager()
		{
		}

		// Token: 0x04003A0B RID: 14859
		private static string[] PropertiesList = new string[]
		{
			"CustomPidLastId", "SpeedTestDB", "AndroidStartBackgroundService", "OpenDashboardOnLaunch", "ConnectOnLaunch", "IOTimeout", "SendDelay", "WiFiServer", "WiFiPort", "ShowBadELMWarning",
			"FuelConsumptionUnit", "UseLitersForVolume", "FirstTimeLaunch", "FirstConnectionAttempted", "UseL100ForFuel", "UseHoursePower", "UseNmForTorque", "AccelerationUseG", "Use_km", "Pressure_use_kpa",
			"Flow_use_grams_sec", "Use_celcium", "UseUSGallon", "FuelPriceForLitre", "Currency", "AlwaysRecordFuelConsumption", "ShowAirFuelBasedOnStoichiometric", "ChartsView", "PIDSortingMode", "ShowPing",
			"RecordData", "LiveDataShowTime", "ChartShowAverageValue", "SetChartMinMaxOnlyVisibleArea", "ChartsVisible", "LiveDataPIDId0", "LiveDataPIDId1", "LiveDataPIDId2", "LiveDataPIDId3", "SelectedProfileV2Name",
			"SelectedBrand", "ProfileUpdateAlias", "DTCReadingModeV2", "DTCReadingSequence", "DTCClearingModeV2", "DTCClearingSequence", "ForceOnlyOneProtocol", "ShouldCheckProfilePIDs", "DaihatsuKLine", "Mode01Prefix",
			"ResponseMarkerLength", "PIDOverridesData"
		};

		// Token: 0x02000BFD RID: 3069
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005C2A RID: 23594 RVA: 0x0043C2D0 File Offset: 0x0043A4D0
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005C2B RID: 23595 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005C2C RID: 23596 RVA: 0x0043C2DC File Offset: 0x0043A4DC
			internal bool <CreateBackup>b__0_0(string x)
			{
				return x.ToLowerInvariant().Contains("drivecycles.bin");
			}

			// Token: 0x06005C2D RID: 23597 RVA: 0x0043C2EE File Offset: 0x0043A4EE
			internal bool <CreateBackup>b__0_1(string x)
			{
				return x.EndsWith("garage.json");
			}

			// Token: 0x06005C2E RID: 23598 RVA: 0x0043C2FB File Offset: 0x0043A4FB
			internal bool <CreateBackup>b__0_2(string x)
			{
				return x.EndsWith("coding.bak");
			}

			// Token: 0x06005C2F RID: 23599 RVA: 0x0043C308 File Offset: 0x0043A508
			internal bool <CreateBackup>b__0_3(string x)
			{
				return x.ToLowerInvariant().EndsWith(".brc");
			}

			// Token: 0x06005C30 RID: 23600 RVA: 0x0043C308 File Offset: 0x0043A508
			internal bool <CreateBackup>b__0_4(string x)
			{
				return x.ToLowerInvariant().EndsWith(".brc");
			}

			// Token: 0x06005C31 RID: 23601 RVA: 0x0043C31A File Offset: 0x0043A51A
			internal string <GetFiles>b__5_0(FileSystemElement x)
			{
				return x.Name;
			}

			// Token: 0x06005C32 RID: 23602 RVA: 0x0043C322 File Offset: 0x0043A522
			internal bool <GetBackupFilesFromFolder>b__6_0(string x)
			{
				return x.ToLowerInvariant().EndsWith(".cbz");
			}

			// Token: 0x06005C33 RID: 23603 RVA: 0x0043C334 File Offset: 0x0043A534
			internal FileSystemElement <GetBackupFilesFromFolder>b__6_1(string x)
			{
				return new FileSystemElement(x, FileSystemElementType.File, false);
			}

			// Token: 0x04003A0C RID: 14860
			public static readonly BackupManager.<>c <>9 = new BackupManager.<>c();

			// Token: 0x04003A0D RID: 14861
			public static Func<string, bool> <>9__0_0;

			// Token: 0x04003A0E RID: 14862
			public static Func<string, bool> <>9__0_1;

			// Token: 0x04003A0F RID: 14863
			public static Func<string, bool> <>9__0_2;

			// Token: 0x04003A10 RID: 14864
			public static Func<string, bool> <>9__0_3;

			// Token: 0x04003A11 RID: 14865
			public static Func<string, bool> <>9__0_4;

			// Token: 0x04003A12 RID: 14866
			public static Func<FileSystemElement, string> <>9__5_0;

			// Token: 0x04003A13 RID: 14867
			public static Func<string, bool> <>9__6_0;

			// Token: 0x04003A14 RID: 14868
			public static Func<string, FileSystemElement> <>9__6_1;
		}

		// Token: 0x02000BFE RID: 3070
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CreateBackup>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005C34 RID: 23604 RVA: 0x0043C340 File Offset: 0x0043A540
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					try
					{
						string text2;
						if (num > 1)
						{
							new GarageModel().Initialize(true);
							string externalStoragePath = FileSystemHelper.ExternalStoragePath;
							List<string> list = Directory.GetFiles(FileSystemHelper.LocalStoragePath).ToList<string>();
							string externalStoragePath2 = FileSystemHelper.ExternalStoragePath;
							filtered_files = new List<string>();
							filtered_files.AddRange(list.Where((string x) => x.ToLowerInvariant().Contains("drivecycles.bin")));
							try
							{
								filtered_files.Add(list.First((string x) => x.EndsWith("garage.json")));
							}
							catch (Exception)
							{
							}
							try
							{
								filtered_files.Add(list.First((string x) => x.EndsWith("coding.bak")));
							}
							catch (Exception)
							{
							}
							if (includeRecords)
							{
								filtered_files.AddRange(list.Where((string x) => x.ToLowerInvariant().EndsWith(".brc")).ToArray<string>());
								if (externalStoragePath != null)
								{
									string[] array = (from x in Directory.GetFiles(Path.Combine(externalStoragePath, "records"))
										where x.ToLowerInvariant().EndsWith(".brc")
										select x).ToArray<string>();
									filtered_files.AddRange(array);
								}
							}
							string text = Path.Combine(FileSystemHelper.LocalStoragePath, "backup");
							if (externalStoragePath != null)
							{
								text = Path.Combine(externalStoragePath, "backup");
							}
							if (!Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
							}
							DateTime nowSafe = DateTimeNowHelper.NowSafe;
							text2 = string.Format("{0}-{1}-{2} {3}-{4}-{5}.cbz", new object[]
							{
								nowSafe.Year,
								nowSafe.Month.ToString("00"),
								nowSafe.Day.ToString("00"),
								nowSafe.Hour.ToString("00"),
								nowSafe.Minute.ToString("00"),
								nowSafe.Second.ToString("00")
							});
							string text3 = Path.Combine(text, text2);
							fstream = File.Open(text3, FileMode.Create);
						}
						try
						{
							if (num > 1)
							{
								zstream = new ZipOutputStream(fstream);
							}
							try
							{
								TaskAwaiter taskAwaiter2;
								TaskAwaiter taskAwaiter;
								if (num != 0)
								{
									if (num != 1)
									{
										zstream.UseZip64 = 0;
										zstream.SetLevel(9);
										ZipEntry zipEntry = new ZipEntry("BACKUP");
										zstream.PutNextEntry(zipEntry);
										byte[] bytes = Encoding.UTF8.GetBytes(text2);
										zstream.Write(bytes, 0, bytes.Length);
										zstream.CloseEntry();
										BackupManager.WriteSettingsEntry(zstream);
										i = 0;
										goto IL_03F4;
									}
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_0466;
								}
								IL_030F:
								try
								{
									if (num != 0)
									{
										string text4;
										ZipEntry zipEntry2 = new ZipEntry(text4);
										zstream.PutNextEntry(zipEntry2);
										taskAwaiter = input_stream.CopyToAsync(zstream).GetAwaiter();
										if (!taskAwaiter.IsCompleted)
										{
											num = (num2 = 0);
											taskAwaiter2 = taskAwaiter;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BackupManager.<CreateBackup>d__0>(ref taskAwaiter, ref this);
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
									zstream.CloseEntry();
								}
								finally
								{
									if (num < 0 && input_stream != null)
									{
										((IDisposable)input_stream).Dispose();
									}
								}
								input_stream = null;
								int num3 = i * 100 / filtered_files.Count;
								progress.Report(num3);
								int num4 = i;
								i = num4 + 1;
								IL_03F4:
								if (i < filtered_files.Count)
								{
									string text4 = filtered_files[i];
									input_stream = File.Open(text4, FileMode.Open);
									goto IL_030F;
								}
								taskAwaiter = zstream.FlushAsync().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BackupManager.<CreateBackup>d__0>(ref taskAwaiter, ref this);
									return;
								}
								IL_0466:
								taskAwaiter.GetResult();
								zstream.Finish();
							}
							finally
							{
								if (num < 0 && zstream != null)
								{
									zstream.Dispose();
								}
							}
							zstream = null;
						}
						finally
						{
							if (num < 0 && fstream != null)
							{
								((IDisposable)fstream).Dispose();
							}
						}
						fstream = null;
						flag = true;
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06005C35 RID: 23605 RVA: 0x0043C8EC File Offset: 0x0043AAEC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003A15 RID: 14869
			public int <>1__state;

			// Token: 0x04003A16 RID: 14870
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04003A17 RID: 14871
			public bool includeRecords;

			// Token: 0x04003A18 RID: 14872
			public IProgress<int> progress;

			// Token: 0x04003A19 RID: 14873
			private List<string> <filtered_files>5__2;

			// Token: 0x04003A1A RID: 14874
			private FileStream <fstream>5__3;

			// Token: 0x04003A1B RID: 14875
			private ZipOutputStream <zstream>5__4;

			// Token: 0x04003A1C RID: 14876
			private int <i>5__5;

			// Token: 0x04003A1D RID: 14877
			private FileStream <input_stream>5__6;

			// Token: 0x04003A1E RID: 14878
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BFF RID: 3071
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadFromBackup>d__7 : IAsyncStateMachine
		{
			// Token: 0x06005C36 RID: 23606 RVA: 0x0043C8FC File Offset: 0x0043AAFC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
				try
				{
					try
					{
						if (num != 0)
						{
							fstream = File.Open(backup_file, FileMode.Open);
						}
						try
						{
							if (num != 0)
							{
								zipstream = new ZipInputStream(fstream);
							}
							try
							{
								TaskAwaiter taskAwaiter;
								if (num == 0)
								{
									TaskAwaiter taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_00DC;
								}
								IL_00E3:
								ZipEntry nextEntry;
								while ((nextEntry = zipstream.GetNextEntry()) != null)
								{
									string fileName = Path.GetFileName(nextEntry.Name);
									if (!string.IsNullOrEmpty(fileName))
									{
										if (BackupManager.PropertiesList.Contains(fileName))
										{
											BackupManager.ReadSettingsEntry(zipstream, nextEntry);
										}
										else if (fileName != "BACKUP")
										{
											taskAwaiter = BackupManager.SaveAsFile(zipstream, fileName).GetAwaiter();
											if (!taskAwaiter.IsCompleted)
											{
												num = (num2 = 0);
												TaskAwaiter taskAwaiter2 = taskAwaiter;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BackupManager.<LoadFromBackup>d__7>(ref taskAwaiter, ref this);
												return;
											}
											goto IL_00DC;
										}
									}
								}
								goto IL_010F;
								IL_00DC:
								taskAwaiter.GetResult();
								goto IL_00E3;
							}
							finally
							{
								if (num < 0 && zipstream != null)
								{
									zipstream.Dispose();
								}
							}
							IL_010F:
							zipstream = null;
						}
						finally
						{
							if (num < 0 && fstream != null)
							{
								((IDisposable)fstream).Dispose();
							}
						}
						fstream = null;
						GarageModel garageModel = new GarageModel();
						if (PlatformHelper.IsAndroid)
						{
							PlatformHelper.DroidService.FileMigrator_MigrateBackups(null);
						}
						bool flag = garageModel.RestoreFromBackup();
						PIDOverrideDictionary.Instance.Load();
						if (flag)
						{
							text = SharedSettings.Current.CurrentCarName;
						}
						else
						{
							text = "Car not found";
						}
					}
					catch (Exception ex)
					{
						text = "Error!\n" + ex.Message;
					}
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06005C37 RID: 23607 RVA: 0x0043CB24 File Offset: 0x0043AD24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003A1F RID: 14879
			public int <>1__state;

			// Token: 0x04003A20 RID: 14880
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04003A21 RID: 14881
			public string backup_file;

			// Token: 0x04003A22 RID: 14882
			private FileStream <fstream>5__2;

			// Token: 0x04003A23 RID: 14883
			private ZipInputStream <zipstream>5__3;

			// Token: 0x04003A24 RID: 14884
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000C00 RID: 3072
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SaveAsFile>d__8 : IAsyncStateMachine
		{
			// Token: 0x06005C38 RID: 23608 RVA: 0x0043CB34 File Offset: 0x0043AD34
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				try
				{
					try
					{
						FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath(fileName), FileMode.Create);
						try
						{
							byte[] array = new byte[2048];
							for (;;)
							{
								int num2 = zipstream.Read(array, 0, array.Length);
								if (num2 <= 0)
								{
									break;
								}
								fileStream.Write(array, 0, num2);
							}
						}
						finally
						{
							if (num < 0 && fileStream != null)
							{
								((IDisposable)fileStream).Dispose();
							}
						}
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005C39 RID: 23609 RVA: 0x0043CBF8 File Offset: 0x0043ADF8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003A25 RID: 14885
			public int <>1__state;

			// Token: 0x04003A26 RID: 14886
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04003A27 RID: 14887
			public string fileName;

			// Token: 0x04003A28 RID: 14888
			public ZipInputStream zipstream;
		}
	}
}
