using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Backup;
using CarScannerXamarinForms.Coding;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E2 RID: 2018
	public static class InputDataAnalyzer
	{
		// Token: 0x060046F3 RID: 18163 RVA: 0x0036CAE4 File Offset: 0x0036ACE4
		public static InputDataAnalyzer.InputDataTypes GetDataTypeFromStream(Stream stream)
		{
			if (stream.Length > 4L)
			{
				stream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[4];
				stream.Read(array, 0, array.Length);
				stream.Seek(0L, SeekOrigin.Begin);
				if (array[0] == 80 && array[1] == 75 && ((array[2] == 3 && array[3] == 4) || (array[2] == 5 && array[3] == 6) || (array[2] == 7 && array[3] == 8)) && BackupManager.IsBackupValid(stream))
				{
					stream.Seek(0L, SeekOrigin.Begin);
					return InputDataAnalyzer.InputDataTypes.CBZ;
				}
			}
			if (InputDataAnalyzer.CheckIsCSP(stream))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				return InputDataAnalyzer.InputDataTypes.CSP;
			}
			if (InputDataAnalyzer.CheckIsCSC(stream))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				return InputDataAnalyzer.InputDataTypes.CSC;
			}
			if (InputDataAnalyzer.CheckIsBrc(stream))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				return InputDataAnalyzer.InputDataTypes.BRC;
			}
			if (InputDataAnalyzer.CheckIsCsv(stream))
			{
				stream.Seek(0L, SeekOrigin.Begin);
				return InputDataAnalyzer.InputDataTypes.CSV;
			}
			return InputDataAnalyzer.InputDataTypes.Unknown;
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x0036CBBC File Offset: 0x0036ADBC
		private static bool CheckIsCSP(Stream stream)
		{
			try
			{
				stream.Seek(0L, SeekOrigin.Begin);
				using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, false, 16384, true))
				{
					using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
					{
						if (new JsonSerializer().Deserialize<List<CustomPID>>(jsonTextReader).Count > 0)
						{
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x0036CC50 File Offset: 0x0036AE50
		private static bool CheckIsCSC(Stream stream)
		{
			try
			{
				stream.Seek(0L, SeekOrigin.Begin);
				using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, false, 16384, true))
				{
					using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
					{
						if (new JsonSerializer().Deserialize<List<CustomizableCodingTemplate>>(jsonTextReader).Count > 0)
						{
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x0036CCE4 File Offset: 0x0036AEE4
		private static bool CheckIsBrc(Stream stream)
		{
			bool flag;
			try
			{
				int version = (int)BRCHelper.GetVersion(stream);
				stream.Seek(0L, SeekOrigin.Begin);
				if (version == 1)
				{
					flag = true;
				}
				else
				{
					DataRecorder.LoadFromStream(stream, true);
					stream.Seek(0L, SeekOrigin.Begin);
					flag = true;
				}
			}
			catch (Exception)
			{
				stream.Seek(0L, SeekOrigin.Begin);
				flag = false;
			}
			return flag;
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x0036CD40 File Offset: 0x0036AF40
		private static bool CheckIsCsv(Stream stream)
		{
			bool flag = false;
			using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, false, 2048, true))
			{
				string text = streamReader.ReadLine();
				if (text.Count((char x) => x == ',') >= 4)
				{
					flag = true;
				}
				if (text.Count((char x) => x == ';') >= 4)
				{
					flag = true;
				}
			}
			stream.Seek(0L, SeekOrigin.Begin);
			return flag;
		}

		// Token: 0x020007E3 RID: 2019
		public enum InputDataTypes
		{
			// Token: 0x04002966 RID: 10598
			Unknown,
			// Token: 0x04002967 RID: 10599
			BRC,
			// Token: 0x04002968 RID: 10600
			CSV,
			// Token: 0x04002969 RID: 10601
			CBZ,
			// Token: 0x0400296A RID: 10602
			CSP,
			// Token: 0x0400296B RID: 10603
			CSC
		}

		// Token: 0x020007E4 RID: 2020
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060046F8 RID: 18168 RVA: 0x0036CDE4 File Offset: 0x0036AFE4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060046F9 RID: 18169 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060046FA RID: 18170 RVA: 0x0036B089 File Offset: 0x00369289
			internal bool <CheckIsCsv>b__5_0(char x)
			{
				return x == ',';
			}

			// Token: 0x060046FB RID: 18171 RVA: 0x0036B090 File Offset: 0x00369290
			internal bool <CheckIsCsv>b__5_1(char x)
			{
				return x == ';';
			}

			// Token: 0x0400296C RID: 10604
			public static readonly InputDataAnalyzer.<>c <>9 = new InputDataAnalyzer.<>c();

			// Token: 0x0400296D RID: 10605
			public static Func<char, bool> <>9__5_0;

			// Token: 0x0400296E RID: 10606
			public static Func<char, bool> <>9__5_1;
		}
	}
}
