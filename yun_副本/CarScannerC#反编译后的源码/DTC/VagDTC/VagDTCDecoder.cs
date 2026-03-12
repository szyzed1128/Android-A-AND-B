using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using MoreLinq;

namespace CarScannerXamarinForms.DTC.VagDTC
{
	// Token: 0x0200057F RID: 1407
	internal class VagDTCDecoder
	{
		// Token: 0x060033A4 RID: 13220 RVA: 0x00242E68 File Offset: 0x00241068
		public static List<string> GetDescriptions(int VagCode, string asam, string asamversion, string vin)
		{
			List<string> list = new List<string>(1);
			if (string.IsNullOrEmpty(vin) || string.IsNullOrEmpty(asam))
			{
				return list;
			}
			asam = asam.Trim().Replace("\0", "");
			string modelCode = VINDecoder.GetVagModelCodeFromVIN(vin);
			int modelYear = VINDecoder.GetYearFromVin(vin, null);
			if (VagDTCDecoder.projects == null)
			{
				VagDTCDecoder.LoadProjects();
			}
			List<VagProject> list2 = VagDTCDecoder.projects.Where((VagProject x) => x.FitsModelCode(modelCode)).ToList<VagProject>();
			list2 = list2.OrderByDescending((VagProject x) => x.FitsModelYear(modelYear)).ToList<VagProject>();
			if (VagDTCDecoder.microContainers == null)
			{
				try
				{
					VagDTCDecoder.LoadMicroContainers();
				}
				catch (Exception)
				{
				}
			}
			using (List<VagProject>.Enumerator enumerator = list2.GetEnumerator())
			{
				Func<string, bool> <>9__5;
				Func<VagMicroItem, bool> <>9__6;
				Func<VagMicroItem, bool> <>9__7;
				while (enumerator.MoveNext())
				{
					VagProject project = enumerator.Current;
					Func<string, bool> <>9__4;
					List<MicroDBContainer> list3 = (from x in VagDTCDecoder.microContainers.Where(delegate(MicroDBContainer x)
						{
							IEnumerable<string> enumerable = x.Projects;
							Func<string, bool> func2;
							if ((func2 = <>9__4) == null)
							{
								func2 = (<>9__4 = (string p) => p == project.Name);
							}
							if (enumerable.Any(func2))
							{
								IEnumerable<string> asamcollection = x.ASAMCollection;
								Func<string, bool> func3;
								if ((func3 = <>9__5) == null)
								{
									func3 = (<>9__5 = (string a) => a == asam);
								}
								if (asamcollection.Any(func3))
								{
									IEnumerable<VagMicroItem> eventsCollection2 = x.EventsCollection;
									Func<VagMicroItem, bool> func4;
									if ((func4 = <>9__6) == null)
									{
										func4 = (<>9__6 = (VagMicroItem ev) => ev.VagCode == VagCode);
									}
									return eventsCollection2.Any(func4);
								}
							}
							return false;
						})
						orderby x.IsBaseVariant
						select x).ToList<MicroDBContainer>();
					if (list3.Count > 0)
					{
						foreach (MicroDBContainer microDBContainer in list3)
						{
							IEnumerable<VagMicroItem> eventsCollection = microDBContainer.EventsCollection;
							Func<VagMicroItem, bool> func;
							if ((func = <>9__7) == null)
							{
								func = (<>9__7 = (VagMicroItem x) => x.VagCode == VagCode);
							}
							foreach (VagMicroItem vagMicroItem in MoreEnumerable.DistinctBy<VagMicroItem, int>(eventsCollection.Where(func), (VagMicroItem x) => x.S).ToList<VagMicroItem>())
							{
								string description2 = VagDTCDecoder.GetDescriptionFromMicroDTC(vagMicroItem);
								if (!string.IsNullOrEmpty(description2) && !list.Any((string x) => x.EqualsWithoutSpacesAndPunctuationTo(description2)))
								{
									list.Add(description2);
								}
							}
						}
					}
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			if (VagDTCDecoder.projects.Count > 0)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (VagProject vagProject in VagDTCDecoder.projects)
				{
					dictionary[vagProject.Name] = asam;
				}
			}
			if (VagDTCDecoder.ProvideAnyOtherFoundProject)
			{
				Func<VagMicroItem, bool> <>9__12;
				MicroDBContainer microDBContainer2 = VagDTCDecoder.microContainers.FirstOrDefault((MicroDBContainer x) => x.ASAMCollection.Any(delegate(string a)
				{
					if (a == asam)
					{
						IEnumerable<VagMicroItem> eventsCollection3 = x.EventsCollection;
						Func<VagMicroItem, bool> func5;
						if ((func5 = <>9__12) == null)
						{
							func5 = (<>9__12 = (VagMicroItem ev) => ev.VagCode == VagCode);
						}
						return eventsCollection3.Any(func5);
					}
					return false;
				}));
				if (microDBContainer2 != null)
				{
					foreach (VagMicroItem vagMicroItem2 in microDBContainer2.EventsCollection.Where((VagMicroItem x) => x.VagCode == VagCode).ToList<VagMicroItem>())
					{
						string description = VagDTCDecoder.GetDescriptionFromMicroDTC(vagMicroItem2);
						if (!string.IsNullOrEmpty(description) && !list.Any((string x) => x.EqualsWithoutSpacesAndPunctuationTo(description)))
						{
							list.Add(description);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x00243284 File Offset: 0x00241484
		public static string GetDescription(int VagCode, string asam, string asamversion, string vin)
		{
			if (string.IsNullOrEmpty(vin) || string.IsNullOrEmpty(asam))
			{
				return "";
			}
			asam = asam.Trim().Replace("\0", "");
			string modelCode = VINDecoder.GetVagModelCodeFromVIN(vin);
			int modelYear = VINDecoder.GetYearFromVin(vin, null);
			if (VagDTCDecoder.projects == null)
			{
				VagDTCDecoder.LoadProjects();
			}
			List<VagProject> list = VagDTCDecoder.projects.Where((VagProject x) => x.FitsModelCode(modelCode)).ToList<VagProject>();
			list = list.OrderByDescending((VagProject x) => x.FitsModelYear(modelYear)).ToList<VagProject>();
			if (list.Count == 0 && vin != null)
			{
				int length = vin.Length;
			}
			if (VagDTCDecoder.microContainers == null)
			{
				try
				{
					VagDTCDecoder.LoadMicroContainers();
				}
				catch (Exception)
				{
				}
			}
			using (List<VagProject>.Enumerator enumerator = list.GetEnumerator())
			{
				Func<string, bool> <>9__5;
				Func<VagMicroItem, bool> <>9__6;
				while (enumerator.MoveNext())
				{
					VagProject project = enumerator.Current;
					Func<string, bool> <>9__4;
					List<MicroDBContainer> list2 = (from x in VagDTCDecoder.microContainers.Where(delegate(MicroDBContainer x)
						{
							IEnumerable<string> enumerable = x.Projects;
							Func<string, bool> func;
							if ((func = <>9__4) == null)
							{
								func = (<>9__4 = (string p) => p == project.Name);
							}
							if (enumerable.Any(func))
							{
								IEnumerable<string> asamcollection = x.ASAMCollection;
								Func<string, bool> func2;
								if ((func2 = <>9__5) == null)
								{
									func2 = (<>9__5 = (string a) => a == asam);
								}
								if (asamcollection.Any(func2))
								{
									IEnumerable<VagMicroItem> eventsCollection = x.EventsCollection;
									Func<VagMicroItem, bool> func3;
									if ((func3 = <>9__6) == null)
									{
										func3 = (<>9__6 = (VagMicroItem ev) => ev.VagCode == VagCode);
									}
									return eventsCollection.Any(func3);
								}
							}
							return false;
						})
						orderby x.IsBaseVariant
						select x).ToList<MicroDBContainer>();
					if (list2.Count > 0)
					{
						foreach (MicroDBContainer microDBContainer in list2)
						{
							string descriptionFromMicroContainer = VagDTCDecoder.GetDescriptionFromMicroContainer(VagCode, microDBContainer);
							if (!string.IsNullOrEmpty(descriptionFromMicroContainer))
							{
								return descriptionFromMicroContainer;
							}
						}
					}
				}
			}
			if (VagDTCDecoder.projects.Count > 0)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (VagProject vagProject in VagDTCDecoder.projects)
				{
					dictionary[vagProject.Name] = asam;
				}
			}
			if (VagDTCDecoder.ProvideAnyOtherFoundProject)
			{
				Func<VagMicroItem, bool> <>9__9;
				MicroDBContainer microDBContainer2 = VagDTCDecoder.microContainers.FirstOrDefault((MicroDBContainer x) => x.ASAMCollection.Any(delegate(string a)
				{
					if (a == asam)
					{
						IEnumerable<VagMicroItem> eventsCollection2 = x.EventsCollection;
						Func<VagMicroItem, bool> func4;
						if ((func4 = <>9__9) == null)
						{
							func4 = (<>9__9 = (VagMicroItem ev) => ev.VagCode == VagCode);
						}
						return eventsCollection2.Any(func4);
					}
					return false;
				}));
				if (microDBContainer2 != null)
				{
					return VagDTCDecoder.GetDescriptionFromMicroContainer(VagCode, microDBContainer2);
				}
			}
			return "";
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x00243510 File Offset: 0x00241710
		private static string GetDescriptionFromMicroContainer(int VagCode, MicroDBContainer container)
		{
			return VagDTCDecoder.GetDescriptionFromMicroDTC(container.EventsCollection.FirstOrDefault((VagMicroItem x) => x.VagCode == VagCode));
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x00243548 File Offset: 0x00241748
		private static string GetDescriptionFromMicroDTC(VagMicroItem microDtc)
		{
			string text = App.CurrentLanguageCode.ToLower();
			string text2;
			if (!(text == "ru"))
			{
				if (!(text == "de"))
				{
					if (!(text == "en"))
					{
					}
					text2 = VagDTCDecoder.LoadFromStrings(microDtc.EN);
				}
				else
				{
					text2 = VagDTCDecoder.LoadFromStrings(microDtc.DE);
				}
			}
			else
			{
				text2 = VagDTCDecoder.LoadFromStrings(microDtc.RU);
			}
			string text3 = VagDTCDecoder.LoadFromStrings(microDtc.U);
			string text4 = VagDTCDecoder.LoadFromStrings(microDtc.S);
			string text5 = "";
			if (text2 == text3)
			{
				text5 = text2;
			}
			else
			{
				if (!string.IsNullOrEmpty(text2))
				{
					text5 = text2;
				}
				if (!string.IsNullOrEmpty(text3))
				{
					if (string.IsNullOrEmpty(text5))
					{
						text5 = text3;
					}
					else
					{
						text5 = text5 + "\n" + text3;
					}
				}
			}
			if (!string.IsNullOrEmpty(text4))
			{
				text5 = text4 + ": " + text5;
			}
			return text5;
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x00243630 File Offset: 0x00241830
		public static string LoadFromStrings(int a)
		{
			if (a < 0)
			{
				return "";
			}
			string text;
			try
			{
				using (FileStream fileStream = File.OpenRead(FileSystemHelper.GetCacheFilePath("vstr.db")))
				{
					using (ShiftStream2 shiftStream = new ShiftStream2(fileStream))
					{
						using (BinaryReader binaryReader = new BinaryReader(shiftStream, Encoding.UTF8))
						{
							fileStream.Seek((long)a, SeekOrigin.Begin);
							text = binaryReader.ReadString();
						}
					}
				}
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x002436DC File Offset: 0x002418DC
		private static string LoadFromStrings(int a, BinaryReader br)
		{
			br.BaseStream.Seek((long)a, SeekOrigin.Begin);
			return br.ReadString();
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x002436F4 File Offset: 0x002418F4
		public static void UnpackContainers(bool forceUnpack = false)
		{
			string text = FileSystemHelper.GetCacheFilePath("vstr.db");
			if (!File.Exists(text) || forceUnpack)
			{
				using (Stream stream = PackageFileReader.OpenFileStream("vag.strings.vag"))
				{
					using (FileStream fileStream = File.Create(text))
					{
						SevenZipHelper.Decompress(stream, fileStream);
					}
				}
			}
			text = FileSystemHelper.GetCacheFilePath("vmc.db");
			if (!File.Exists(text) || forceUnpack)
			{
				using (Stream stream2 = PackageFileReader.OpenFileStream("vag.micro.vag"))
				{
					using (FileStream fileStream2 = File.Create(text))
					{
						SevenZipHelper.Decompress(stream2, fileStream2);
					}
				}
			}
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x002437C4 File Offset: 0x002419C4
		public static void DeleteContainers()
		{
			try
			{
				string cacheFilePath = FileSystemHelper.GetCacheFilePath("vstr.db");
				if (File.Exists(cacheFilePath))
				{
					File.Delete(cacheFilePath);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				string cacheFilePath2 = FileSystemHelper.GetCacheFilePath("vmc.db");
				if (File.Exists(cacheFilePath2))
				{
					File.Delete(cacheFilePath2);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x0024382C File Offset: 0x00241A2C
		public static void LoadMicroContainers()
		{
			bool flag = false;
			for (;;)
			{
				new Stopwatch().Start();
				VagDTCDecoder.UnpackContainers(false);
				try
				{
					using (FileStream fileStream = File.OpenRead(FileSystemHelper.GetCacheFilePath("vstr.db")))
					{
						using (ShiftStream2 shiftStream = new ShiftStream2(fileStream))
						{
							using (BinaryReader binaryReader = new BinaryReader(shiftStream, Encoding.UTF8))
							{
								using (FileStream fileStream2 = File.OpenRead(FileSystemHelper.GetCacheFilePath("vmc.db")))
								{
									using (ShiftStream2 shiftStream2 = new ShiftStream2(fileStream2))
									{
										byte[] array = new byte[3];
										shiftStream2.Read(array, 0, 3);
										int num = VagMicroItem.IntFrom3Byte(array);
										VagDTCDecoder.microContainers = new List<MicroDBContainer>(num);
										for (int i = 0; i < num; i++)
										{
											MicroDBContainer microDBContainer = new MicroDBContainer();
											if ((byte)shiftStream2.ReadByte() == 0)
											{
												microDBContainer.IsBaseVariant = false;
											}
											else
											{
												microDBContainer.IsBaseVariant = true;
											}
											shiftStream2.Read(array, 0, 3);
											int num2 = VagMicroItem.IntFrom3Byte(array);
											microDBContainer.Projects.Capacity = num2;
											byte[] array2 = new byte[num2 * 3];
											shiftStream2.Read(array2, 0, array2.Length);
											for (int j = 0; j < num2; j++)
											{
												string text = VagDTCDecoder.LoadFromStrings(VagMicroItem.IntFrom3Byte(array2[j * 3], array2[j * 3 + 1], array2[j * 3 + 2]), binaryReader);
												microDBContainer.Projects.Add(text);
											}
											shiftStream2.Read(array, 0, 3);
											int num3 = VagMicroItem.IntFrom3Byte(array);
											microDBContainer.ASAMCollection.Capacity = num3;
											byte[] array3 = new byte[num3 * 3];
											shiftStream2.Read(array3, 0, array3.Length);
											for (int k = 0; k < num3; k++)
											{
												string text2 = VagDTCDecoder.LoadFromStrings(VagMicroItem.IntFrom3Byte(array3[k * 3], array3[k * 3 + 1], array3[k * 3 + 2]), binaryReader);
												microDBContainer.ASAMCollection.Add(text2);
											}
											shiftStream2.Read(array, 0, 3);
											int num4 = VagMicroItem.IntFrom3Byte(array);
											microDBContainer.EventsCollection.Capacity = num4;
											for (int l = 0; l < num4; l++)
											{
												VagMicroItem vagMicroItem = VagMicroItem.ReadFromStream(shiftStream2);
												microDBContainer.EventsCollection.Add(vagMicroItem);
											}
											VagDTCDecoder.microContainers.Add(microDBContainer);
										}
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
					if (!flag)
					{
						VagDTCDecoder.UnpackContainers(true);
						flag = true;
						continue;
					}
				}
				break;
			}
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x00243B30 File Offset: 0x00241D30
		private static void LoadProjects()
		{
			VagDTCDecoder.projects = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<VagProject>>("vag.vagmodelcodes_crypto.db", true);
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x00243B42 File Offset: 0x00241D42
		public static void ClearResources()
		{
			if (VagDTCDecoder.microContainers != null)
			{
				VagDTCDecoder.microContainers.Clear();
				VagDTCDecoder.microContainers = null;
			}
			if (VagDTCDecoder.projects != null)
			{
				VagDTCDecoder.projects.Clear();
				VagDTCDecoder.projects = null;
			}
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x00002050 File Offset: 0x00000250
		public VagDTCDecoder()
		{
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x00243B72 File Offset: 0x00241D72
		// Note: this type is marked as 'beforefieldinit'.
		static VagDTCDecoder()
		{
		}

		// Token: 0x04001E60 RID: 7776
		private static bool ProvideAnyOtherFoundProject = true;

		// Token: 0x04001E61 RID: 7777
		private static List<MicroDBContainer> microContainers = null;

		// Token: 0x04001E62 RID: 7778
		private static List<VagProject> projects = null;

		// Token: 0x02000580 RID: 1408
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060033B1 RID: 13233 RVA: 0x00243B86 File Offset: 0x00241D86
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060033B2 RID: 13234 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060033B3 RID: 13235 RVA: 0x00243B92 File Offset: 0x00241D92
			internal bool <GetDescriptions>b__1_3(MicroDBContainer x)
			{
				return x.IsBaseVariant;
			}

			// Token: 0x060033B4 RID: 13236 RVA: 0x00243B9A File Offset: 0x00241D9A
			internal int <GetDescriptions>b__1_8(VagMicroItem x)
			{
				return x.S;
			}

			// Token: 0x060033B5 RID: 13237 RVA: 0x00243B92 File Offset: 0x00241D92
			internal bool <GetDescription>b__2_3(MicroDBContainer x)
			{
				return x.IsBaseVariant;
			}

			// Token: 0x04001E63 RID: 7779
			public static readonly VagDTCDecoder.<>c <>9 = new VagDTCDecoder.<>c();

			// Token: 0x04001E64 RID: 7780
			public static Func<MicroDBContainer, bool> <>9__1_3;

			// Token: 0x04001E65 RID: 7781
			public static Func<VagMicroItem, int> <>9__1_8;

			// Token: 0x04001E66 RID: 7782
			public static Func<MicroDBContainer, bool> <>9__2_3;
		}

		// Token: 0x02000581 RID: 1409
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060033B6 RID: 13238 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060033B7 RID: 13239 RVA: 0x00243BA2 File Offset: 0x00241DA2
			internal bool <GetDescriptions>b__0(VagProject x)
			{
				return x.FitsModelCode(this.modelCode);
			}

			// Token: 0x060033B8 RID: 13240 RVA: 0x00243BB0 File Offset: 0x00241DB0
			internal bool <GetDescriptions>b__1(VagProject x)
			{
				return x.FitsModelYear(this.modelYear);
			}

			// Token: 0x060033B9 RID: 13241 RVA: 0x00243BBE File Offset: 0x00241DBE
			internal bool <GetDescriptions>b__5(string a)
			{
				return a == this.asam;
			}

			// Token: 0x060033BA RID: 13242 RVA: 0x00243BCC File Offset: 0x00241DCC
			internal bool <GetDescriptions>b__6(VagMicroItem ev)
			{
				return ev.VagCode == this.VagCode;
			}

			// Token: 0x060033BB RID: 13243 RVA: 0x00243BCC File Offset: 0x00241DCC
			internal bool <GetDescriptions>b__7(VagMicroItem x)
			{
				return x.VagCode == this.VagCode;
			}

			// Token: 0x060033BC RID: 13244 RVA: 0x00243BDC File Offset: 0x00241DDC
			internal bool <GetDescriptions>b__10(MicroDBContainer x)
			{
				VagDTCDecoder.<>c__DisplayClass1_3 CS$<>8__locals1 = new VagDTCDecoder.<>c__DisplayClass1_3();
				CS$<>8__locals1.CS$<>8__locals2 = this;
				CS$<>8__locals1.x = x;
				return CS$<>8__locals1.x.ASAMCollection.Any(delegate(string a)
				{
					if (a == CS$<>8__locals1.CS$<>8__locals2.asam)
					{
						IEnumerable<VagMicroItem> eventsCollection = CS$<>8__locals1.x.EventsCollection;
						Func<VagMicroItem, bool> func;
						if ((func = CS$<>8__locals1.CS$<>8__locals2.<>9__12) == null)
						{
							func = (CS$<>8__locals1.CS$<>8__locals2.<>9__12 = (VagMicroItem ev) => ev.VagCode == CS$<>8__locals1.CS$<>8__locals2.VagCode);
						}
						return eventsCollection.Any(func);
					}
					return false;
				});
			}

			// Token: 0x060033BD RID: 13245 RVA: 0x00243BCC File Offset: 0x00241DCC
			internal bool <GetDescriptions>b__12(VagMicroItem ev)
			{
				return ev.VagCode == this.VagCode;
			}

			// Token: 0x060033BE RID: 13246 RVA: 0x00243BCC File Offset: 0x00241DCC
			internal bool <GetDescriptions>b__13(VagMicroItem x)
			{
				return x.VagCode == this.VagCode;
			}

			// Token: 0x04001E67 RID: 7783
			public string modelCode;

			// Token: 0x04001E68 RID: 7784
			public int modelYear;

			// Token: 0x04001E69 RID: 7785
			public string asam;

			// Token: 0x04001E6A RID: 7786
			public int VagCode;

			// Token: 0x04001E6B RID: 7787
			public Func<string, bool> <>9__5;

			// Token: 0x04001E6C RID: 7788
			public Func<VagMicroItem, bool> <>9__6;

			// Token: 0x04001E6D RID: 7789
			public Func<VagMicroItem, bool> <>9__7;

			// Token: 0x04001E6E RID: 7790
			public Func<VagMicroItem, bool> <>9__12;
		}

		// Token: 0x02000582 RID: 1410
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_1
		{
			// Token: 0x060033BF RID: 13247 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_1()
			{
			}

			// Token: 0x060033C0 RID: 13248 RVA: 0x00243C1C File Offset: 0x00241E1C
			internal bool <GetDescriptions>b__2(MicroDBContainer x)
			{
				IEnumerable<string> projects = x.Projects;
				Func<string, bool> func;
				if ((func = this.<>9__4) == null)
				{
					func = (this.<>9__4 = (string p) => p == this.project.Name);
				}
				if (projects.Any(func))
				{
					IEnumerable<string> asamcollection = x.ASAMCollection;
					Func<string, bool> func2;
					if ((func2 = this.CS$<>8__locals1.<>9__5) == null)
					{
						func2 = (this.CS$<>8__locals1.<>9__5 = (string a) => a == this.CS$<>8__locals1.asam);
					}
					if (asamcollection.Any(func2))
					{
						IEnumerable<VagMicroItem> eventsCollection = x.EventsCollection;
						Func<VagMicroItem, bool> func3;
						if ((func3 = this.CS$<>8__locals1.<>9__6) == null)
						{
							func3 = (this.CS$<>8__locals1.<>9__6 = (VagMicroItem ev) => ev.VagCode == this.CS$<>8__locals1.VagCode);
						}
						return eventsCollection.Any(func3);
					}
				}
				return false;
			}

			// Token: 0x060033C1 RID: 13249 RVA: 0x00243CCB File Offset: 0x00241ECB
			internal bool <GetDescriptions>b__4(string p)
			{
				return p == this.project.Name;
			}

			// Token: 0x04001E6F RID: 7791
			public VagProject project;

			// Token: 0x04001E70 RID: 7792
			public VagDTCDecoder.<>c__DisplayClass1_0 CS$<>8__locals1;

			// Token: 0x04001E71 RID: 7793
			public Func<string, bool> <>9__4;
		}

		// Token: 0x02000583 RID: 1411
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_2
		{
			// Token: 0x060033C2 RID: 13250 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_2()
			{
			}

			// Token: 0x060033C3 RID: 13251 RVA: 0x00243CDE File Offset: 0x00241EDE
			internal bool <GetDescriptions>b__9(string x)
			{
				return x.EqualsWithoutSpacesAndPunctuationTo(this.description);
			}

			// Token: 0x04001E72 RID: 7794
			public string description;
		}

		// Token: 0x02000584 RID: 1412
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_3
		{
			// Token: 0x060033C4 RID: 13252 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_3()
			{
			}

			// Token: 0x060033C5 RID: 13253 RVA: 0x00243CEC File Offset: 0x00241EEC
			internal bool <GetDescriptions>b__11(string a)
			{
				if (a == this.CS$<>8__locals2.asam)
				{
					IEnumerable<VagMicroItem> eventsCollection = this.x.EventsCollection;
					Func<VagMicroItem, bool> func;
					if ((func = this.CS$<>8__locals2.<>9__12) == null)
					{
						func = (this.CS$<>8__locals2.<>9__12 = (VagMicroItem ev) => ev.VagCode == this.CS$<>8__locals2.VagCode);
					}
					return eventsCollection.Any(func);
				}
				return false;
			}

			// Token: 0x04001E73 RID: 7795
			public MicroDBContainer x;

			// Token: 0x04001E74 RID: 7796
			public VagDTCDecoder.<>c__DisplayClass1_0 CS$<>8__locals2;
		}

		// Token: 0x02000585 RID: 1413
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_4
		{
			// Token: 0x060033C6 RID: 13254 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_4()
			{
			}

			// Token: 0x060033C7 RID: 13255 RVA: 0x00243D4C File Offset: 0x00241F4C
			internal bool <GetDescriptions>b__14(string x)
			{
				return x.EqualsWithoutSpacesAndPunctuationTo(this.description);
			}

			// Token: 0x04001E75 RID: 7797
			public string description;
		}

		// Token: 0x02000586 RID: 1414
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060033C8 RID: 13256 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060033C9 RID: 13257 RVA: 0x00243D5A File Offset: 0x00241F5A
			internal bool <GetDescription>b__0(VagProject x)
			{
				return x.FitsModelCode(this.modelCode);
			}

			// Token: 0x060033CA RID: 13258 RVA: 0x00243D68 File Offset: 0x00241F68
			internal bool <GetDescription>b__1(VagProject x)
			{
				return x.FitsModelYear(this.modelYear);
			}

			// Token: 0x060033CB RID: 13259 RVA: 0x00243D76 File Offset: 0x00241F76
			internal bool <GetDescription>b__5(string a)
			{
				return a == this.asam;
			}

			// Token: 0x060033CC RID: 13260 RVA: 0x00243D84 File Offset: 0x00241F84
			internal bool <GetDescription>b__6(VagMicroItem ev)
			{
				return ev.VagCode == this.VagCode;
			}

			// Token: 0x060033CD RID: 13261 RVA: 0x00243D94 File Offset: 0x00241F94
			internal bool <GetDescription>b__7(MicroDBContainer x)
			{
				VagDTCDecoder.<>c__DisplayClass2_2 CS$<>8__locals1 = new VagDTCDecoder.<>c__DisplayClass2_2();
				CS$<>8__locals1.CS$<>8__locals2 = this;
				CS$<>8__locals1.x = x;
				return CS$<>8__locals1.x.ASAMCollection.Any(delegate(string a)
				{
					if (a == CS$<>8__locals1.CS$<>8__locals2.asam)
					{
						IEnumerable<VagMicroItem> eventsCollection = CS$<>8__locals1.x.EventsCollection;
						Func<VagMicroItem, bool> func;
						if ((func = CS$<>8__locals1.CS$<>8__locals2.<>9__9) == null)
						{
							func = (CS$<>8__locals1.CS$<>8__locals2.<>9__9 = (VagMicroItem ev) => ev.VagCode == CS$<>8__locals1.CS$<>8__locals2.VagCode);
						}
						return eventsCollection.Any(func);
					}
					return false;
				});
			}

			// Token: 0x060033CE RID: 13262 RVA: 0x00243D84 File Offset: 0x00241F84
			internal bool <GetDescription>b__9(VagMicroItem ev)
			{
				return ev.VagCode == this.VagCode;
			}

			// Token: 0x04001E76 RID: 7798
			public string modelCode;

			// Token: 0x04001E77 RID: 7799
			public int modelYear;

			// Token: 0x04001E78 RID: 7800
			public string asam;

			// Token: 0x04001E79 RID: 7801
			public int VagCode;

			// Token: 0x04001E7A RID: 7802
			public Func<string, bool> <>9__5;

			// Token: 0x04001E7B RID: 7803
			public Func<VagMicroItem, bool> <>9__6;

			// Token: 0x04001E7C RID: 7804
			public Func<VagMicroItem, bool> <>9__9;
		}

		// Token: 0x02000587 RID: 1415
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x060033CF RID: 13263 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x060033D0 RID: 13264 RVA: 0x00243DD4 File Offset: 0x00241FD4
			internal bool <GetDescription>b__2(MicroDBContainer x)
			{
				IEnumerable<string> projects = x.Projects;
				Func<string, bool> func;
				if ((func = this.<>9__4) == null)
				{
					func = (this.<>9__4 = (string p) => p == this.project.Name);
				}
				if (projects.Any(func))
				{
					IEnumerable<string> asamcollection = x.ASAMCollection;
					Func<string, bool> func2;
					if ((func2 = this.CS$<>8__locals1.<>9__5) == null)
					{
						func2 = (this.CS$<>8__locals1.<>9__5 = (string a) => a == this.CS$<>8__locals1.asam);
					}
					if (asamcollection.Any(func2))
					{
						IEnumerable<VagMicroItem> eventsCollection = x.EventsCollection;
						Func<VagMicroItem, bool> func3;
						if ((func3 = this.CS$<>8__locals1.<>9__6) == null)
						{
							func3 = (this.CS$<>8__locals1.<>9__6 = (VagMicroItem ev) => ev.VagCode == this.CS$<>8__locals1.VagCode);
						}
						return eventsCollection.Any(func3);
					}
				}
				return false;
			}

			// Token: 0x060033D1 RID: 13265 RVA: 0x00243E83 File Offset: 0x00242083
			internal bool <GetDescription>b__4(string p)
			{
				return p == this.project.Name;
			}

			// Token: 0x04001E7D RID: 7805
			public VagProject project;

			// Token: 0x04001E7E RID: 7806
			public VagDTCDecoder.<>c__DisplayClass2_0 CS$<>8__locals1;

			// Token: 0x04001E7F RID: 7807
			public Func<string, bool> <>9__4;
		}

		// Token: 0x02000588 RID: 1416
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_2
		{
			// Token: 0x060033D2 RID: 13266 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_2()
			{
			}

			// Token: 0x060033D3 RID: 13267 RVA: 0x00243E98 File Offset: 0x00242098
			internal bool <GetDescription>b__8(string a)
			{
				if (a == this.CS$<>8__locals2.asam)
				{
					IEnumerable<VagMicroItem> eventsCollection = this.x.EventsCollection;
					Func<VagMicroItem, bool> func;
					if ((func = this.CS$<>8__locals2.<>9__9) == null)
					{
						func = (this.CS$<>8__locals2.<>9__9 = (VagMicroItem ev) => ev.VagCode == this.CS$<>8__locals2.VagCode);
					}
					return eventsCollection.Any(func);
				}
				return false;
			}

			// Token: 0x04001E80 RID: 7808
			public MicroDBContainer x;

			// Token: 0x04001E81 RID: 7809
			public VagDTCDecoder.<>c__DisplayClass2_0 CS$<>8__locals2;
		}

		// Token: 0x02000589 RID: 1417
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060033D4 RID: 13268 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060033D5 RID: 13269 RVA: 0x00243EF8 File Offset: 0x002420F8
			internal bool <GetDescriptionFromMicroContainer>b__0(VagMicroItem x)
			{
				return x.VagCode == this.VagCode;
			}

			// Token: 0x04001E82 RID: 7810
			public int VagCode;
		}
	}
}
