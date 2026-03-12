using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000709 RID: 1801
	public static class Brc2CsvConverter
	{
		// Token: 0x06003D38 RID: 15672 RVA: 0x00326F2C File Offset: 0x0032512C
		public static async void RemoveTemporaryCSV()
		{
			try
			{
				string localFilePath = FileSystemHelper.GetLocalFilePath("csv");
				if (Directory.Exists(localFilePath))
				{
					foreach (string text in Directory.GetFiles(localFilePath))
					{
						try
						{
							File.Delete(text);
						}
						catch
						{
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				string cacheFilePath = FileSystemHelper.GetCacheFilePath("exported_records.zip");
				if (File.Exists(cacheFilePath))
				{
					File.Delete(cacheFilePath);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00326F5C File Offset: 0x0032515C
		public static string Convert(IDataRecordContainer recorder, string input_name, IProgress<int> progress, int format_version)
		{
			string localFilePath = FileSystemHelper.GetLocalFilePath("csv");
			if (!Directory.Exists(localFilePath))
			{
				Directory.CreateDirectory(localFilePath);
			}
			string text = Path.Combine(localFilePath, input_name + ".csv");
			string text2;
			try
			{
				using (FileStream fileStream = File.Open(text, FileMode.Create))
				{
					using (BufferedStream bufferedStream = new BufferedStream(fileStream, 262144))
					{
						using (StreamWriter streamWriter = new StreamWriter(bufferedStream))
						{
							if (format_version == 1)
							{
								Brc2CsvConverter.WriteV1(recorder, streamWriter, progress);
							}
							else if (format_version == 2)
							{
								Brc2CsvConverter.WriteV2(recorder, streamWriter, progress);
							}
							bufferedStream.Flush();
							fileStream.Flush();
						}
					}
				}
				text2 = text;
			}
			catch (Exception ex)
			{
				text2 = "ERROR!\n" + ex.ToString();
			}
			return text2;
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00327054 File Offset: 0x00325254
		private static void WriteV1(IDataRecordContainer recorder, StreamWriter sw, IProgress<int> progress)
		{
			sw.Write(Brc2CsvConverter.GetQuotedString("SECONDS"));
			sw.Write(";");
			sw.Write(Brc2CsvConverter.GetQuotedString("PID"));
			sw.Write(";");
			sw.Write(Brc2CsvConverter.GetQuotedString("VALUE"));
			sw.Write(";");
			sw.Write(Brc2CsvConverter.GetQuotedString("UNITS"));
			sw.Write(";");
			if (recorder.HasGeolocationData)
			{
				sw.Write(Brc2CsvConverter.GetQuotedString("LATITUDE"));
				sw.Write(";");
				sw.Write(Brc2CsvConverter.GetQuotedString("LONGTITUDE"));
				sw.Write(";");
			}
			sw.WriteLine();
			List<Brc2CsvConverter.CSVRecord> csvrecordsFromRecorder = Brc2CsvConverter.GetCSVRecordsFromRecorder(recorder);
			long num = csvrecordsFromRecorder.LongCount<Brc2CsvConverter.CSVRecord>();
			long num2 = 0L;
			foreach (Brc2CsvConverter.CSVRecord csvrecord in csvrecordsFromRecorder)
			{
				num2 += 1L;
				if (double.IsFinite(csvrecord.Value))
				{
					sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Seconds.ToString(CultureInfo.InvariantCulture.NumberFormat)));
					sw.Write(";");
					sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Name));
					sw.Write(";");
					sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Value.ToString(CultureInfo.InvariantCulture.NumberFormat)));
					sw.Write(";");
					sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Units));
					sw.Write(";");
					if (recorder.HasGeolocationData)
					{
						sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Latitude.ToString(CultureInfo.InvariantCulture.NumberFormat)));
						sw.Write(";");
						sw.Write(Brc2CsvConverter.GetQuotedString(csvrecord.Longtitude.ToString(CultureInfo.InvariantCulture.NumberFormat)));
						sw.Write(";");
					}
					sw.Write('\r');
					sw.Write('\n');
					if (num2 % 500L == 0L)
					{
						long num3 = num2 * 100L / num;
						if (progress != null)
						{
							progress.Report((int)num3);
						}
					}
				}
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0032729C File Offset: 0x0032549C
		private static string GetQuotedString(string s)
		{
			return "\"" + s + "\"";
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x003272B0 File Offset: 0x003254B0
		private static List<Brc2CsvConverter.CSVRecord> GetCSVRecordsFromRecorder(IDataRecordContainer recorder)
		{
			List<Brc2CsvConverter.CSVRecord> list = new List<Brc2CsvConverter.CSVRecord>();
			foreach (DataRecord dataRecord in recorder.Records)
			{
				foreach (DataRecordElement dataRecordElement in dataRecord.Elements)
				{
					if (!double.IsNaN(dataRecordElement.Value) && !double.IsInfinity(dataRecordElement.Value))
					{
						Brc2CsvConverter.CSVRecord csvrecord = new Brc2CsvConverter.CSVRecord
						{
							Seconds = dataRecordElement.Seconds,
							Value = UnitsHelper.GetValue(dataRecordElement.Value, dataRecord.Units),
							Units = UnitsHelper.GetCaption(dataRecord.Units),
							Name = dataRecord.Name,
							Latitude = dataRecordElement.Position.X,
							Longtitude = dataRecordElement.Position.Y
						};
						list.Add(csvrecord);
					}
				}
			}
			list = list.OrderBy((Brc2CsvConverter.CSVRecord x) => x.Seconds).ToList<Brc2CsvConverter.CSVRecord>();
			return list;
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x00327414 File Offset: 0x00325614
		private static void WriteV2(IDataRecordContainer recorder, StreamWriter sw, IProgress<int> progress)
		{
			new Dictionary<int, int>();
			HashSet<double> hashSet = new HashSet<double>(2048);
			int num = recorder.Records.Count + 1;
			if (recorder.HasGeolocationData)
			{
				num = recorder.Records.Count + 3;
			}
			string[] array = new string[num];
			if (recorder.HasGeolocationData)
			{
				array[array.Length - 1] = "Longtitude";
				array[array.Length - 2] = "Latitude";
			}
			int row_longtitude = array.Length - 1;
			int row_latitude = array.Length - 2;
			array[0] = "time";
			for (int m = 0; m < recorder.Records.Count; m++)
			{
				array[m + 1] = string.Concat(new string[]
				{
					"\"",
					recorder.Records[m].Name,
					" (",
					UnitsHelper.GetCaption(recorder.Records[m].Units),
					")\""
				});
				foreach (DataRecordElement dataRecordElement in recorder.Records[m].Elements)
				{
					if (double.IsFinite(dataRecordElement.Value))
					{
						hashSet.Add(dataRecordElement.Seconds);
					}
				}
			}
			Brc2CsvConverter.WriteLineV2(array, sw);
			double[] array2 = hashSet.OrderBy((double x) => x).ToArray<double>();
			Dictionary<double, string> dictionary = new Dictionary<double, string>(array2.Length);
			foreach (double num2 in array2)
			{
				string text = recorder.TimeStarted.Date.AddSeconds(num2).ToString("HH:mm:ss.fff");
				dictionary.Add(num2, text);
			}
			Parallel.ForEach<DataRecord>(recorder.Records, delegate(DataRecord x)
			{
				for (int l = 0; l < x.Elements.Count; l++)
				{
					x.Elements[l].Value = UnitsHelper.GetValue(x.Elements[l].Value, x.Units);
				}
			});
			int[] skips = new int[recorder.Records.Count];
			for (int k = 0; k < array2.Length; k++)
			{
				double time = array2[k];
				string[] line = new string[num];
				line[0] = dictionary[time];
				Parallel.For(1, recorder.Records.Count + 1, delegate(int i)
				{
					DataRecord dataRecord = recorder.Records[i - 1];
					int num4 = Brc2CsvConverter.FastFindElement(dataRecord.Elements, skips[i - 1], time);
					if (num4 >= 0)
					{
						DataRecordElement dataRecordElement2 = dataRecord.Elements[num4];
						skips[i - 1] = num4;
						double value = dataRecordElement2.Value;
						if (!double.IsFinite(value))
						{
							line[i] = "";
							return;
						}
						line[i] = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
						if (recorder.HasGeolocationData)
						{
							if (!dataRecordElement2.Position.IsEmpty)
							{
								line[row_latitude] = dataRecordElement2.Position.X.ToString(CultureInfo.InvariantCulture.NumberFormat);
								line[row_longtitude] = dataRecordElement2.Position.Y.ToString(CultureInfo.InvariantCulture.NumberFormat);
								return;
							}
							line[row_latitude] = "";
							line[row_longtitude] = "";
							return;
						}
					}
					else
					{
						line[i] = "";
					}
				});
				Brc2CsvConverter.WriteLineV2(line, sw);
				if (k % 500 == 0)
				{
					int num3 = k * 100 / array2.Length;
					if (progress != null)
					{
						progress.Report(num3);
					}
				}
			}
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x00327744 File Offset: 0x00325944
		private static int FastFindElement(List<DataRecordElement> elements, int startIndex, double time)
		{
			if (startIndex < 0 || startIndex >= elements.Count)
			{
				return -1;
			}
			for (int i = startIndex; i < elements.Count; i++)
			{
				if (elements[i].Seconds == time)
				{
					return i;
				}
				if (elements[i].Seconds > time)
				{
					return -1;
				}
			}
			return -1;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x00327794 File Offset: 0x00325994
		private static void WriteLineV2(string[] line_contents, StreamWriter sw)
		{
			foreach (string text in line_contents)
			{
				if (text == null)
				{
					sw.Write("");
				}
				else
				{
					sw.Write(text);
				}
				sw.Write(",");
			}
			sw.WriteLine();
		}

		// Token: 0x0200070A RID: 1802
		private class CSVRecord
		{
			// Token: 0x06003D40 RID: 15680 RVA: 0x00002050 File Offset: 0x00000250
			public CSVRecord()
			{
			}

			// Token: 0x04002588 RID: 9608
			public double Seconds;

			// Token: 0x04002589 RID: 9609
			public string Name;

			// Token: 0x0400258A RID: 9610
			public double Value;

			// Token: 0x0400258B RID: 9611
			public string Units;

			// Token: 0x0400258C RID: 9612
			public double Latitude;

			// Token: 0x0400258D RID: 9613
			public double Longtitude;
		}

		// Token: 0x0200070B RID: 1803
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003D41 RID: 15681 RVA: 0x003277DD File Offset: 0x003259DD
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003D42 RID: 15682 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003D43 RID: 15683 RVA: 0x003277E9 File Offset: 0x003259E9
			internal double <GetCSVRecordsFromRecorder>b__4_0(Brc2CsvConverter.CSVRecord x)
			{
				return x.Seconds;
			}

			// Token: 0x06003D44 RID: 15684 RVA: 0x00016849 File Offset: 0x00014A49
			internal double <WriteV2>b__6_0(double x)
			{
				return x;
			}

			// Token: 0x06003D45 RID: 15685 RVA: 0x003277F4 File Offset: 0x003259F4
			internal void <WriteV2>b__6_1(DataRecord x)
			{
				for (int i = 0; i < x.Elements.Count; i++)
				{
					x.Elements[i].Value = UnitsHelper.GetValue(x.Elements[i].Value, x.Units);
				}
			}

			// Token: 0x0400258E RID: 9614
			public static readonly Brc2CsvConverter.<>c <>9 = new Brc2CsvConverter.<>c();

			// Token: 0x0400258F RID: 9615
			public static Func<Brc2CsvConverter.CSVRecord, double> <>9__4_0;

			// Token: 0x04002590 RID: 9616
			public static Func<double, double> <>9__6_0;

			// Token: 0x04002591 RID: 9617
			public static Action<DataRecord> <>9__6_1;
		}

		// Token: 0x0200070C RID: 1804
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06003D46 RID: 15686 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x04002592 RID: 9618
			public IDataRecordContainer recorder;

			// Token: 0x04002593 RID: 9619
			public int[] skips;

			// Token: 0x04002594 RID: 9620
			public int row_latitude;

			// Token: 0x04002595 RID: 9621
			public int row_longtitude;
		}

		// Token: 0x0200070D RID: 1805
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_1
		{
			// Token: 0x06003D47 RID: 15687 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_1()
			{
			}

			// Token: 0x06003D48 RID: 15688 RVA: 0x00327844 File Offset: 0x00325A44
			internal void <WriteV2>b__2(int i)
			{
				DataRecord dataRecord = this.CS$<>8__locals1.recorder.Records[i - 1];
				int num = Brc2CsvConverter.FastFindElement(dataRecord.Elements, this.CS$<>8__locals1.skips[i - 1], this.time);
				if (num >= 0)
				{
					DataRecordElement dataRecordElement = dataRecord.Elements[num];
					this.CS$<>8__locals1.skips[i - 1] = num;
					double value = dataRecordElement.Value;
					if (!double.IsFinite(value))
					{
						this.line[i] = "";
						return;
					}
					this.line[i] = value.ToString(CultureInfo.InvariantCulture.NumberFormat);
					if (this.CS$<>8__locals1.recorder.HasGeolocationData)
					{
						if (!dataRecordElement.Position.IsEmpty)
						{
							this.line[this.CS$<>8__locals1.row_latitude] = dataRecordElement.Position.X.ToString(CultureInfo.InvariantCulture.NumberFormat);
							this.line[this.CS$<>8__locals1.row_longtitude] = dataRecordElement.Position.Y.ToString(CultureInfo.InvariantCulture.NumberFormat);
							return;
						}
						this.line[this.CS$<>8__locals1.row_latitude] = "";
						this.line[this.CS$<>8__locals1.row_longtitude] = "";
						return;
					}
				}
				else
				{
					this.line[i] = "";
				}
			}

			// Token: 0x04002596 RID: 9622
			public double time;

			// Token: 0x04002597 RID: 9623
			public string[] line;

			// Token: 0x04002598 RID: 9624
			public Brc2CsvConverter.<>c__DisplayClass6_0 CS$<>8__locals1;
		}

		// Token: 0x0200070E RID: 1806
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RemoveTemporaryCSV>d__0 : IAsyncStateMachine
		{
			// Token: 0x06003D49 RID: 15689 RVA: 0x003279B4 File Offset: 0x00325BB4
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					try
					{
						string localFilePath = FileSystemHelper.GetLocalFilePath("csv");
						if (Directory.Exists(localFilePath))
						{
							foreach (string text in Directory.GetFiles(localFilePath))
							{
								try
								{
									File.Delete(text);
								}
								catch
								{
								}
							}
						}
					}
					catch
					{
					}
					try
					{
						string cacheFilePath = FileSystemHelper.GetCacheFilePath("exported_records.zip");
						if (File.Exists(cacheFilePath))
						{
							File.Delete(cacheFilePath);
						}
					}
					catch
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

			// Token: 0x06003D4A RID: 15690 RVA: 0x00327A84 File Offset: 0x00325C84
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002599 RID: 9625
			public int <>1__state;

			// Token: 0x0400259A RID: 9626
			public AsyncVoidMethodBuilder <>t__builder;
		}
	}
}
