using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007F9 RID: 2041
	internal static class PackageFileReader
	{
		// Token: 0x06004740 RID: 18240 RVA: 0x0036D7D6 File Offset: 0x0036B9D6
		public static Stream OpenFileStream(string filename)
		{
			return typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0036D7FC File Offset: 0x0036B9FC
		public static string ReadFileToString(string filename)
		{
			Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename);
			string text = "";
			using (StreamReader streamReader = new StreamReader(manifestResourceStream))
			{
				text = streamReader.ReadToEnd();
			}
			return text;
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0036D860 File Offset: 0x0036BA60
		public static T BinaryDeserilzeFromEmbeddedFileUsingStream<T>(string filename, bool rootIsArray)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (BsonReader bsonReader = new BsonReader(manifestResourceStream))
				{
					bsonReader.ReadRootValueAsArray = rootIsArray;
					t = new JsonSerializer().Deserialize<T>(bsonReader);
				}
			}
			return t;
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0036D8E0 File Offset: 0x0036BAE0
		public static T DeserilzeFromEmbeddedFileUsingStream<T>(string filename)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
					{
						t = new JsonSerializer().Deserialize<T>(jsonTextReader);
					}
				}
			}
			return t;
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0036D978 File Offset: 0x0036BB78
		public static T DeserilzeFromEmbeddedFileUsingCryptoStream<T>(string filename)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (ShiftStream shiftStream = new ShiftStream(manifestResourceStream))
				{
					using (StreamReader streamReader = new StreamReader(shiftStream))
					{
						using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
						{
							try
							{
								t = new JsonSerializer().Deserialize<T>(jsonTextReader);
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

		// Token: 0x06004745 RID: 18245 RVA: 0x0036DA48 File Offset: 0x0036BC48
		public static T DeserilzeFromEmbeddedFileUsingCryptoStream2<T>(string filename)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (ShiftStream2 shiftStream = new ShiftStream2(manifestResourceStream))
				{
					using (StreamReader streamReader = new StreamReader(shiftStream))
					{
						using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
						{
							try
							{
								t = new JsonSerializer().Deserialize<T>(jsonTextReader);
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

		// Token: 0x06004746 RID: 18246 RVA: 0x0036DB18 File Offset: 0x0036BD18
		public static T BinaryDeserilzeFromEmbeddedFileUsingCryptoStream<T>(string filename, bool rootIsArray)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (ShiftStream shiftStream = new ShiftStream(manifestResourceStream))
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
			return t;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0036DBD0 File Offset: 0x0036BDD0
		public static T BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<T>(string filename, bool rootIsArray)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (ShiftStream2 shiftStream = new ShiftStream2(manifestResourceStream))
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
			return t;
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0036DC88 File Offset: 0x0036BE88
		public static T DeserilzeFromEmbeddedFile<T>(string filename)
		{
			return JsonConvert.DeserializeObject<T>(PackageFileReader.ReadFileToString(filename));
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0036DC95 File Offset: 0x0036BE95
		public static string[] GetManifestResourceNames()
		{
			return typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceNames();
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0036DCB0 File Offset: 0x0036BEB0
		public static string GetApplicationPath()
		{
			return Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x0036DCC0 File Offset: 0x0036BEC0
		public static string[] GetFilesInDirectory(string path)
		{
			IEnumerable<string> manifestResourceNames = PackageFileReader.GetManifestResourceNames();
			string package_and_path = PackageFileReader.GetPackageName() + path;
			if (!package_and_path.EndsWith('.'))
			{
				package_and_path += ".";
			}
			int cut_len = package_and_path.Length;
			return (from x in manifestResourceNames
				where x.StartsWith(package_and_path)
				select x.Substring(cut_len)).ToArray<string>();
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0036DD46 File Offset: 0x0036BF46
		public static string[] GetFoldersInDirectory(string path)
		{
			return Directory.GetDirectories(path);
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0036DD4E File Offset: 0x0036BF4E
		public static string GetPackageName()
		{
			return "CarScannerXamarinForms.";
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0036DD55 File Offset: 0x0036BF55
		public static Stream OpenFileCryptoStream2(string filename)
		{
			return new ShiftStream2(typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename));
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0036DD80 File Offset: 0x0036BF80
		public static T BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStream2<T>(string filename, bool rootIsArray)
		{
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
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

		// Token: 0x06004750 RID: 18256 RVA: 0x0036DE68 File Offset: 0x0036C068
		public static T BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStreamWithDiff<T>(string filename, bool rootIsArray, string diffFilePath)
		{
			if (string.IsNullOrEmpty(diffFilePath) || !File.Exists(diffFilePath))
			{
				return PackageFileReader.BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStream2<T>(filename, rootIsArray);
			}
			T t;
			using (Stream manifestResourceStream = typeof(PackageFileReader).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				byte[] array = Crc32.Calculate(manifestResourceStream);
				manifestResourceStream.Seek(0L, SeekOrigin.Begin);
				using (FileStream fileStream = File.OpenRead(diffFilePath))
				{
					using (ShiftStream3 shiftStream = new ShiftStream3(fileStream, array))
					{
						using (DiffDecodeStream diffDecodeStream = new DiffDecodeStream(manifestResourceStream, shiftStream))
						{
							diffDecodeStream.Init();
							using (MemoryStream memoryStream = new MemoryStream())
							{
								SevenZipHelper.Decompress(diffDecodeStream, memoryStream);
								memoryStream.Seek(0L, SeekOrigin.Begin);
								using (ShiftStream2 shiftStream2 = new ShiftStream2(memoryStream))
								{
									using (BsonReader bsonReader = new BsonReader(shiftStream2))
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
					}
				}
			}
			return t;
		}

		// Token: 0x020007FA RID: 2042
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x06004751 RID: 18257 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x06004752 RID: 18258 RVA: 0x0036DFEC File Offset: 0x0036C1EC
			internal bool <GetFilesInDirectory>b__0(string x)
			{
				return x.StartsWith(this.package_and_path);
			}

			// Token: 0x06004753 RID: 18259 RVA: 0x0036DFFA File Offset: 0x0036C1FA
			internal string <GetFilesInDirectory>b__1(string x)
			{
				return x.Substring(this.cut_len);
			}

			// Token: 0x0400298F RID: 10639
			public string package_and_path;

			// Token: 0x04002990 RID: 10640
			public int cut_len;
		}
	}
}
