using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x02000705 RID: 1797
	internal class NanExcluderFromBRC
	{
		// Token: 0x06003D1C RID: 15644 RVA: 0x00326928 File Offset: 0x00324B28
		public static void Exclude(string filename, IDataRecordContainer container)
		{
			foreach (DataRecord dataRecord in container.Records)
			{
				dataRecord.Elements.RemoveAll((DataRecordElement x) => !double.IsFinite(x.Value));
			}
			using (FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath(Path.GetFileNameWithoutExtension(filename) + "NoNAN.brc"), FileMode.Create, FileAccess.ReadWrite))
			{
				using (BufferedStream bufferedStream = new BufferedStream(fileStream, 16384))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(bufferedStream, Encoding.UTF8, true))
					{
						binaryWriter.Write("CARSCANNERRECORD");
						binaryWriter.Write(2);
						binaryWriter.Write(container.VIN);
						binaryWriter.Write(container.CarName);
						binaryWriter.Write(container.ConnectionProfile);
						binaryWriter.Write(container.Device);
						binaryWriter.Write(container.TimeStarted.Ticks);
						foreach (DataRecord dataRecord2 in container.Records)
						{
							if (binaryWriter != null)
							{
								binaryWriter.Write(13984144);
							}
							if (binaryWriter != null)
							{
								binaryWriter.Write(dataRecord2.PID_Id);
							}
							if (binaryWriter != null)
							{
								binaryWriter.Write(dataRecord2.Name ?? "");
							}
							if (binaryWriter != null)
							{
								binaryWriter.Write(dataRecord2.ShortName ?? "");
							}
							if (binaryWriter != null)
							{
								binaryWriter.Write((int)dataRecord2.Units);
							}
							foreach (DataRecordElement dataRecordElement in dataRecord2.Elements)
							{
								if (binaryWriter != null)
								{
									binaryWriter.Write(19350352);
								}
								if (binaryWriter != null)
								{
									binaryWriter.Write(dataRecordElement.Seconds);
								}
								if (binaryWriter != null)
								{
									binaryWriter.Write(dataRecord2.PID_Id);
								}
								if (binaryWriter != null)
								{
									binaryWriter.Write(dataRecordElement.Value);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00002050 File Offset: 0x00000250
		public NanExcluderFromBRC()
		{
		}

		// Token: 0x02000706 RID: 1798
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003D1E RID: 15646 RVA: 0x00326BD4 File Offset: 0x00324DD4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003D1F RID: 15647 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003D20 RID: 15648 RVA: 0x00315B68 File Offset: 0x00313D68
			internal bool <Exclude>b__0_0(DataRecordElement x)
			{
				return !double.IsFinite(x.Value);
			}

			// Token: 0x04002581 RID: 9601
			public static readonly NanExcluderFromBRC.<>c <>9 = new NanExcluderFromBRC.<>c();

			// Token: 0x04002582 RID: 9602
			public static Predicate<DataRecordElement> <>9__0_0;
		}
	}
}
