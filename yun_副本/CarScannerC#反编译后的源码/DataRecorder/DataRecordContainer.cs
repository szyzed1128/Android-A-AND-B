using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006D8 RID: 1752
	internal class DataRecordContainer : IDataRecordContainer
	{
		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x003151F4 File Offset: 0x003133F4
		// (set) Token: 0x06003B97 RID: 15255 RVA: 0x003151FC File Offset: 0x003133FC
		public int Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x170013B0 RID: 5040
		// (get) Token: 0x06003B98 RID: 15256 RVA: 0x00315205 File Offset: 0x00313405
		// (set) Token: 0x06003B99 RID: 15257 RVA: 0x0031520D File Offset: 0x0031340D
		public string VIN
		{
			[CompilerGenerated]
			get
			{
				return this.<VIN>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<VIN>k__BackingField = value;
			}
		}

		// Token: 0x170013B1 RID: 5041
		// (get) Token: 0x06003B9A RID: 15258 RVA: 0x00315216 File Offset: 0x00313416
		// (set) Token: 0x06003B9B RID: 15259 RVA: 0x0031521E File Offset: 0x0031341E
		public string CarName
		{
			[CompilerGenerated]
			get
			{
				return this.<CarName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CarName>k__BackingField = value;
			}
		}

		// Token: 0x170013B2 RID: 5042
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x00315227 File Offset: 0x00313427
		// (set) Token: 0x06003B9D RID: 15261 RVA: 0x0031522F File Offset: 0x0031342F
		public string ConnectionProfile
		{
			[CompilerGenerated]
			get
			{
				return this.<ConnectionProfile>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ConnectionProfile>k__BackingField = value;
			}
		}

		// Token: 0x170013B3 RID: 5043
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x00315238 File Offset: 0x00313438
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x00315240 File Offset: 0x00313440
		public string Device
		{
			[CompilerGenerated]
			get
			{
				return this.<Device>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Device>k__BackingField = value;
			}
		}

		// Token: 0x170013B4 RID: 5044
		// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x00315249 File Offset: 0x00313449
		// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x00315251 File Offset: 0x00313451
		public DateTime TimeEnded
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeEnded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeEnded>k__BackingField = value;
			}
		}

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x0031525C File Offset: 0x0031345C
		// (set) Token: 0x06003BA3 RID: 15267 RVA: 0x00315277 File Offset: 0x00313477
		public long TimeStartedTicks
		{
			get
			{
				return this.TimeStarted.Ticks;
			}
			set
			{
				this.TimeStarted = new DateTime(value);
			}
		}

		// Token: 0x170013B6 RID: 5046
		// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x00315285 File Offset: 0x00313485
		// (set) Token: 0x06003BA5 RID: 15269 RVA: 0x0031528D File Offset: 0x0031348D
		public DateTime TimeStarted
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeStarted>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeStarted>k__BackingField = value;
			}
		}

		// Token: 0x170013B7 RID: 5047
		// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x00315296 File Offset: 0x00313496
		// (set) Token: 0x06003BA7 RID: 15271 RVA: 0x0031529E File Offset: 0x0031349E
		public List<DataRecord> Records
		{
			[CompilerGenerated]
			get
			{
				return this.<Records>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Records>k__BackingField = value;
			}
		} = new List<DataRecord>();

		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06003BA8 RID: 15272 RVA: 0x003152A7 File Offset: 0x003134A7
		// (set) Token: 0x06003BA9 RID: 15273 RVA: 0x003152AF File Offset: 0x003134AF
		public List<DTCItemV2> DTCs
		{
			[CompilerGenerated]
			get
			{
				return this.<DTCs>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DTCs>k__BackingField = value;
			}
		} = new List<DTCItemV2>();

		// Token: 0x170013B9 RID: 5049
		// (get) Token: 0x06003BAA RID: 15274 RVA: 0x003152B8 File Offset: 0x003134B8
		// (set) Token: 0x06003BAB RID: 15275 RVA: 0x003152C0 File Offset: 0x003134C0
		public List<PointWithTime> Positions
		{
			[CompilerGenerated]
			get
			{
				return this.<Positions>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Positions>k__BackingField = value;
			}
		} = new List<PointWithTime>();

		// Token: 0x170013BA RID: 5050
		// (get) Token: 0x06003BAC RID: 15276 RVA: 0x003152C9 File Offset: 0x003134C9
		// (set) Token: 0x06003BAD RID: 15277 RVA: 0x003152D1 File Offset: 0x003134D1
		public List<ProxyTest> SpeedTests
		{
			[CompilerGenerated]
			get
			{
				return this.<SpeedTests>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SpeedTests>k__BackingField = value;
			}
		} = new List<ProxyTest>();

		// Token: 0x170013BB RID: 5051
		// (get) Token: 0x06003BAE RID: 15278 RVA: 0x003152DA File Offset: 0x003134DA
		public bool HasDTC
		{
			get
			{
				return this.DTCs.Count > 0;
			}
		}

		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06003BAF RID: 15279 RVA: 0x003152EA File Offset: 0x003134EA
		public bool HasSpeedTests
		{
			get
			{
				return this.SpeedTests.Count > 0;
			}
		}

		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x003152FA File Offset: 0x003134FA
		// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x00315302 File Offset: 0x00313502
		public string Filename
		{
			[CompilerGenerated]
			get
			{
				return this.<Filename>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Filename>k__BackingField = value;
			}
		}

		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x0031530B File Offset: 0x0031350B
		public string Title
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.Filename);
			}
		}

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x00315318 File Offset: 0x00313518
		// (set) Token: 0x06003BB4 RID: 15284 RVA: 0x00315320 File Offset: 0x00313520
		public bool HasGeolocationData
		{
			[CompilerGenerated]
			get
			{
				return this.<HasGeolocationData>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HasGeolocationData>k__BackingField = value;
			}
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x0031532C File Offset: 0x0031352C
		public static DataRecordContainer LoadFromFile(string filepath, bool readOnlyTitle)
		{
			using (FileStream fileStream = File.Open(filepath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileStream, Encoding.UTF8))
				{
					if (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
					{
						if (binaryReader.ReadString() != "CARSCANNERRECORD")
						{
							throw new Exception("Wrong file format");
						}
						binaryReader.ReadInt32();
						DataRecordContainer dataRecordContainer = DataRecordContainer.ReadVersion2(binaryReader, readOnlyTitle);
						DataRecordContainer.SetTimeEnded(dataRecordContainer);
						dataRecordContainer.Filename = filepath;
						return dataRecordContainer;
					}
				}
			}
			throw new Exception("Error reading data record file");
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x003153E4 File Offset: 0x003135E4
		public static void RemoveNaNs(IDataRecordContainer drc)
		{
			if (SharedSettings.Current.AndroidChartRenderingSafeMode)
			{
				foreach (DataRecord dataRecord in drc.Records)
				{
					dataRecord.Elements.RemoveAll((DataRecordElement x) => !double.IsFinite(x.Value));
				}
			}
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x00315468 File Offset: 0x00313668
		private static void SetTimeEnded(DataRecordContainer drc)
		{
			double num = 0.0;
			foreach (DataRecord dataRecord in drc.Records)
			{
				if (dataRecord.Elements.Count > 0 && num < dataRecord.Elements[dataRecord.Elements.Count - 1].Seconds)
				{
					num = dataRecord.Elements[dataRecord.Elements.Count - 1].Seconds;
				}
			}
			if (!SharedSettings.Current.AndroidChartRenderingSafeMode)
			{
				foreach (DataRecord dataRecord2 in drc.Records)
				{
					dataRecord2.Elements.Add(new DataRecordElement(double.NaN, num));
				}
			}
			drc.TimeEnded = new DateTime((new TimeSpan(drc.TimeStarted.Ticks) + TimeSpan.FromSeconds(num)).Ticks);
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x0031559C File Offset: 0x0031379C
		private static DataRecordContainer ReadVersion2(BinaryReader br, bool readOnlyTitle)
		{
			DataRecordContainer dataRecordContainer = new DataRecordContainer();
			bool androidChartRenderingSafeMode = SharedSettings.Current.AndroidChartRenderingSafeMode;
			dataRecordContainer.Version = 2;
			dataRecordContainer.VIN = br.ReadString();
			dataRecordContainer.CarName = br.ReadString();
			dataRecordContainer.ConnectionProfile = br.ReadString();
			dataRecordContainer.Device = br.ReadString();
			dataRecordContainer.TimeStartedTicks = br.ReadInt64();
			dataRecordContainer.HasGeolocationData = false;
			PointWithTime pointWithTime = PointWithTime.Zero;
			if (readOnlyTitle)
			{
				return dataRecordContainer;
			}
			try
			{
				int num = SharedSettings.Current.DataRecordLineSplitterTime;
				if (num == 0)
				{
					num = int.MaxValue;
				}
				while (br.BaseStream.Position < br.BaseStream.Length)
				{
					int num2 = br.ReadInt32();
					if (num2 <= 19350352)
					{
						if (num2 != 13984144)
						{
							if (num2 == 19350352)
							{
								double num3 = br.ReadDouble();
								int pid_id = br.ReadInt32();
								double num4 = br.ReadDouble();
								if (!androidChartRenderingSafeMode || double.IsFinite(num4))
								{
									DataRecord dataRecord = dataRecordContainer.Records.Find((DataRecord x) => x.PID_Id == pid_id);
									TimeSpan timeSpan = dataRecordContainer.TimeStarted.TimeOfDay + TimeSpan.FromSeconds(num3);
									if (dataRecord != null)
									{
										DataRecordElement dataRecordElement = new DataRecordElement(num4, timeSpan.TotalSeconds);
										if (!pointWithTime.IsEmpty)
										{
											dataRecordElement.Position = pointWithTime.ToPoint();
										}
										if (dataRecord.Elements.Count > 1 && num > 0 && !SharedSettings.Current.AndroidChartRenderingSafeMode)
										{
											DataRecordElement dataRecordElement2 = dataRecord.Elements[dataRecord.Elements.Count - 1];
											DataRecordElement dataRecordElement3 = dataRecord.Elements[dataRecord.Elements.Count - 2];
											if (double.IsFinite(dataRecordElement.Value) && double.IsFinite(dataRecordElement2.Value) && dataRecordElement.Seconds - dataRecordElement2.Seconds > (double)num)
											{
												if (!double.IsFinite(dataRecordElement3.Value))
												{
													DataRecordElement dataRecordElement4 = new DataRecordElement(dataRecordElement2.Value, dataRecordElement2.Seconds - 0.01);
													dataRecord.Elements.Add(dataRecordElement4);
												}
												DataRecordElement dataRecordElement5 = new DataRecordElement(double.NaN, dataRecordElement2.Seconds + 0.0001);
												dataRecordElement5.Position = dataRecordElement2.Position;
												DataRecordElement dataRecordElement6 = new DataRecordElement(double.NaN, dataRecordElement.Seconds - 0.0001);
												dataRecordElement6.Position = dataRecordElement.Position;
												dataRecord.Elements.Add(dataRecordElement5);
												dataRecord.Elements.Add(dataRecordElement6);
											}
										}
										dataRecord.Elements.Add(dataRecordElement);
									}
								}
							}
						}
						else
						{
							DataRecord dataRecord2 = new DataRecord();
							dataRecord2.PID_Id = br.ReadInt32();
							dataRecord2.Name = br.ReadString();
							dataRecord2.ShortName = br.ReadString();
							dataRecord2.Units = (UnitsHelper.Units)br.ReadInt32();
							if (!SharedSettings.Current.AndroidChartRenderingSafeMode)
							{
								dataRecord2.Elements.Add(new DataRecordElement(double.NaN, dataRecordContainer.TimeStarted.TimeOfDay.TotalSeconds));
							}
							dataRecordContainer.Records.Add(dataRecord2);
						}
					}
					else
					{
						if (num2 != 59201502)
						{
							if (num2 != 383857192)
							{
								if (num2 != 527891292)
								{
									continue;
								}
							}
							else
							{
								br.ReadInt64();
								string text = br.ReadString();
								try
								{
									DTCItemV2 dtcitemV = JsonConvert.DeserializeObject<DTCItemV2>(text);
									dataRecordContainer.DTCs.Add(dtcitemV);
									continue;
								}
								catch (Exception)
								{
									continue;
								}
							}
							double num5 = br.ReadDouble();
							double num6 = br.ReadDouble();
							double num7 = br.ReadDouble();
							PointWithTime pointWithTime2 = new PointWithTime(num6, num7, dataRecordContainer.TimeStarted.TimeOfDay.TotalSeconds + num5);
							dataRecordContainer.Positions.Add(pointWithTime2);
							pointWithTime = pointWithTime2;
							Point point = pointWithTime.ToPoint();
							if (dataRecordContainer.HasGeolocationData)
							{
								continue;
							}
							dataRecordContainer.HasGeolocationData = true;
							using (List<DataRecord>.Enumerator enumerator = dataRecordContainer.Records.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									DataRecord dataRecord3 = enumerator.Current;
									foreach (DataRecordElement dataRecordElement7 in dataRecord3.Elements)
									{
										dataRecordElement7.Position = point;
									}
								}
								continue;
							}
						}
						br.ReadDouble();
						string text2 = br.ReadString();
						try
						{
							ProxyTest proxyTest = JsonConvert.DeserializeObject<ProxyTest>(text2);
							dataRecordContainer.SpeedTests.Add(proxyTest);
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (Exception)
			{
			}
			List<DataRecord> list = (from rec in dataRecordContainer.Records
				where rec.Elements.Any((DataRecordElement el) => double.IsFinite(el.Value))
				select rec into x
				orderby x.Name
				select x).ToList<DataRecord>();
			dataRecordContainer.Records = list;
			return dataRecordContainer;
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00315B28 File Offset: 0x00313D28
		public DataRecordContainer()
		{
		}

		// Token: 0x0400247F RID: 9343
		[CompilerGenerated]
		private int <Version>k__BackingField;

		// Token: 0x04002480 RID: 9344
		[CompilerGenerated]
		private string <VIN>k__BackingField;

		// Token: 0x04002481 RID: 9345
		[CompilerGenerated]
		private string <CarName>k__BackingField;

		// Token: 0x04002482 RID: 9346
		[CompilerGenerated]
		private string <ConnectionProfile>k__BackingField;

		// Token: 0x04002483 RID: 9347
		[CompilerGenerated]
		private string <Device>k__BackingField;

		// Token: 0x04002484 RID: 9348
		[CompilerGenerated]
		private DateTime <TimeEnded>k__BackingField;

		// Token: 0x04002485 RID: 9349
		[CompilerGenerated]
		private DateTime <TimeStarted>k__BackingField;

		// Token: 0x04002486 RID: 9350
		[CompilerGenerated]
		private List<DataRecord> <Records>k__BackingField;

		// Token: 0x04002487 RID: 9351
		[CompilerGenerated]
		private List<DTCItemV2> <DTCs>k__BackingField;

		// Token: 0x04002488 RID: 9352
		[CompilerGenerated]
		private List<PointWithTime> <Positions>k__BackingField;

		// Token: 0x04002489 RID: 9353
		[CompilerGenerated]
		private List<ProxyTest> <SpeedTests>k__BackingField;

		// Token: 0x0400248A RID: 9354
		[CompilerGenerated]
		private string <Filename>k__BackingField;

		// Token: 0x0400248B RID: 9355
		[CompilerGenerated]
		private bool <HasGeolocationData>k__BackingField;

		// Token: 0x020006D9 RID: 1753
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003BBA RID: 15290 RVA: 0x00315B5C File Offset: 0x00313D5C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003BBB RID: 15291 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003BBC RID: 15292 RVA: 0x00315B68 File Offset: 0x00313D68
			internal bool <RemoveNaNs>b__62_0(DataRecordElement x)
			{
				return !double.IsFinite(x.Value);
			}

			// Token: 0x06003BBD RID: 15293 RVA: 0x00315B78 File Offset: 0x00313D78
			internal bool <ReadVersion2>b__64_0(DataRecord rec)
			{
				return rec.Elements.Any((DataRecordElement el) => double.IsFinite(el.Value));
			}

			// Token: 0x06003BBE RID: 15294 RVA: 0x00315BA4 File Offset: 0x00313DA4
			internal bool <ReadVersion2>b__64_3(DataRecordElement el)
			{
				return double.IsFinite(el.Value);
			}

			// Token: 0x06003BBF RID: 15295 RVA: 0x00315BB1 File Offset: 0x00313DB1
			internal string <ReadVersion2>b__64_1(DataRecord x)
			{
				return x.Name;
			}

			// Token: 0x0400248C RID: 9356
			public static readonly DataRecordContainer.<>c <>9 = new DataRecordContainer.<>c();

			// Token: 0x0400248D RID: 9357
			public static Predicate<DataRecordElement> <>9__62_0;

			// Token: 0x0400248E RID: 9358
			public static Func<DataRecordElement, bool> <>9__64_3;

			// Token: 0x0400248F RID: 9359
			public static Func<DataRecord, bool> <>9__64_0;

			// Token: 0x04002490 RID: 9360
			public static Func<DataRecord, string> <>9__64_1;
		}

		// Token: 0x020006DA RID: 1754
		[CompilerGenerated]
		private sealed class <>c__DisplayClass64_0
		{
			// Token: 0x06003BC0 RID: 15296 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass64_0()
			{
			}

			// Token: 0x06003BC1 RID: 15297 RVA: 0x00315BB9 File Offset: 0x00313DB9
			internal bool <ReadVersion2>b__2(DataRecord x)
			{
				return x.PID_Id == this.pid_id;
			}

			// Token: 0x04002491 RID: 9361
			public int pid_id;
		}
	}
}
