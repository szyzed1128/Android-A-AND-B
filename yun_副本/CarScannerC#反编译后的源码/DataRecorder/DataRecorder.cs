using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006DE RID: 1758
	public class DataRecorder : IDataRecordContainer
	{
		// Token: 0x06003BDC RID: 15324 RVA: 0x0031606B File Offset: 0x0031426B
		public DataRecorder()
		{
			this._Records = new ConcurrentBag<DataRecord>();
			this.Records = new List<DataRecord>();
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00316095 File Offset: 0x00314295
		public void Synchronize()
		{
			this._Records = new ConcurrentBag<DataRecord>(this.Records);
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x003160A8 File Offset: 0x003142A8
		public string Title
		{
			get
			{
				return Path.GetFileNameWithoutExtension(this.Filename);
			}
		}

		// Token: 0x170013CB RID: 5067
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x003160B5 File Offset: 0x003142B5
		// (set) Token: 0x06003BE0 RID: 15328 RVA: 0x003160BD File Offset: 0x003142BD
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

		// Token: 0x170013CC RID: 5068
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x003160C6 File Offset: 0x003142C6
		// (set) Token: 0x06003BE2 RID: 15330 RVA: 0x003160CE File Offset: 0x003142CE
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

		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06003BE3 RID: 15331 RVA: 0x003160D7 File Offset: 0x003142D7
		// (set) Token: 0x06003BE4 RID: 15332 RVA: 0x003160DF File Offset: 0x003142DF
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

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x003160E8 File Offset: 0x003142E8
		// (set) Token: 0x06003BE6 RID: 15334 RVA: 0x003160F0 File Offset: 0x003142F0
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

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06003BE7 RID: 15335 RVA: 0x003160F9 File Offset: 0x003142F9
		// (set) Token: 0x06003BE8 RID: 15336 RVA: 0x00316101 File Offset: 0x00314301
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

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06003BE9 RID: 15337 RVA: 0x0031610A File Offset: 0x0031430A
		// (set) Token: 0x06003BEA RID: 15338 RVA: 0x00316112 File Offset: 0x00314312
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

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06003BEB RID: 15339 RVA: 0x0031611B File Offset: 0x0031431B
		// (set) Token: 0x06003BEC RID: 15340 RVA: 0x00316123 File Offset: 0x00314323
		private ConcurrentBag<DataRecord> _Records
		{
			[CompilerGenerated]
			get
			{
				return this.<_Records>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<_Records>k__BackingField = value;
			}
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06003BED RID: 15341 RVA: 0x0031612C File Offset: 0x0031432C
		// (set) Token: 0x06003BEE RID: 15342 RVA: 0x00316134 File Offset: 0x00314334
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
		} = new List<DTCItemV2>(0);

		// Token: 0x170013D3 RID: 5075
		// (get) Token: 0x06003BEF RID: 15343 RVA: 0x0031613D File Offset: 0x0031433D
		// (set) Token: 0x06003BF0 RID: 15344 RVA: 0x00316145 File Offset: 0x00314345
		public List<DataRecord> Records
		{
			[CompilerGenerated]
			get
			{
				return this.<Records>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Records>k__BackingField = value;
			}
		}

		// Token: 0x170013D4 RID: 5076
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00316150 File Offset: 0x00314350
		[JsonIgnore]
		public string DateTimeString
		{
			get
			{
				return string.Format("{0} {1} - {2} {3}", new object[]
				{
					this.TimeStarted.ToShortDateString(),
					this.TimeStarted.ToShortTimeString(),
					this.TimeEnded.ToShortDateString(),
					this.TimeEnded.ToShortTimeString()
				});
			}
		}

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06003BF2 RID: 15346 RVA: 0x003161B1 File Offset: 0x003143B1
		// (set) Token: 0x06003BF3 RID: 15347 RVA: 0x003161B9 File Offset: 0x003143B9
		[JsonIgnore]
		public string Filename
		{
			[CompilerGenerated]
			get
			{
				return this.<Filename>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Filename>k__BackingField = value;
			}
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x003161C4 File Offset: 0x003143C4
		private string GetDefaultFilename()
		{
			return this.TimeStarted.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture);
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x00002076 File Offset: 0x00000276
		public bool HasDTC
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x06003BF6 RID: 15350 RVA: 0x003161E9 File Offset: 0x003143E9
		public List<ProxyTest> SpeedTests
		{
			get
			{
				return new List<ProxyTest>(0);
			}
		}

		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x06003BF7 RID: 15351 RVA: 0x00002076 File Offset: 0x00000276
		public bool HasSpeedTests
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013D9 RID: 5081
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x00002076 File Offset: 0x00000276
		public bool HasGeolocationData
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00017A6F File Offset: 0x00015C6F
		public void Load()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x003161F4 File Offset: 0x003143F4
		public void Record(IPIDFloatValue pid, double TimeStampSeconds)
		{
			if (this.isRecording)
			{
				double value = pid.Value;
				int id = pid.Id;
				if (!double.IsInfinity(value))
				{
					DataRecord dataRecord = null;
					foreach (DataRecord dataRecord2 in this._Records)
					{
						if (dataRecord2.PID_Id == id)
						{
							dataRecord = dataRecord2;
							break;
						}
					}
					if (dataRecord == null)
					{
						dataRecord = new DataRecord
						{
							Name = pid.Name,
							ShortName = pid.ShortName,
							PID_Id = pid.Id,
							Units = pid.Units
						};
						if (!SharedSettings.Current.AndroidChartRenderingSafeMode)
						{
							dataRecord.AddElement(double.NaN, 0.0);
						}
						this._Records.Add(dataRecord);
					}
					dataRecord.AddElement(pid.Value, TimeStampSeconds);
				}
			}
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x003162E4 File Offset: 0x003144E4
		private bool HasData()
		{
			if (this._Records != null && this._Records.Count == 0)
			{
				using (IEnumerator<DataRecord> enumerator = this._Records.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Elements.Count > 0)
						{
							return true;
						}
					}
				}
			}
			return (this.DTCs != null && this.DTCs.Count > 0) || (this.SpeedTests != null && this.SpeedTests.Count > 0);
		}

		// Token: 0x06003BFC RID: 15356 RVA: 0x00316384 File Offset: 0x00314584
		public void Save()
		{
			if (!this.HasData())
			{
				return;
			}
			double totalSeconds = App.OBDReader.stopwatch.Elapsed.TotalSeconds;
			foreach (DataRecord dataRecord in this._Records)
			{
				dataRecord.AddElement(double.NaN, totalSeconds);
			}
			this.SaveBin();
		}

		// Token: 0x06003BFD RID: 15357 RVA: 0x00316400 File Offset: 0x00314600
		private void SaveBin()
		{
			try
			{
				using (FileStream fileStream = File.Open(Path.Combine(FileSystemHelper.LocalStoragePath, this.Filename + ".brc"), FileMode.Create))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
					{
						DataRecorder.WriteRecorder(this, binaryWriter);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003BFE RID: 15358 RVA: 0x00316488 File Offset: 0x00314688
		private async Task SaveJson()
		{
			using (FileStream fileStream = File.Open(Path.Combine(FileSystemHelper.LocalStoragePath, this.Filename + ".rec"), FileMode.Create))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
					{
						jsonTextWriter.Culture = CultureInfo.InvariantCulture;
						this.WriteToStreamDataRecorder(jsonTextWriter);
					}
				}
			}
		}

		// Token: 0x06003BFF RID: 15359 RVA: 0x003164CC File Offset: 0x003146CC
		public void Start(string VIN)
		{
			this.Filename = this.GetDefaultFilename();
			this.TimeStarted = DateTimeNowHelper.NowSafe;
			this.VIN = VIN;
			this.ConnectionProfile = SharedSettings.Current.BrandAndProfile;
			ConnectionTypes connectionType = SharedSettings.Current.ConnectionType;
			if (connectionType != ConnectionTypes.WiFi)
			{
				if (connectionType == ConnectionTypes.BluetoothLE)
				{
					this.Device = "BTLE " + SharedSettings.Current.BTLEDeviceName;
				}
			}
			else
			{
				this.Device = "Wi-Fi " + SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
			}
			this.isRecording = true;
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00316567 File Offset: 0x00314767
		public void Stop()
		{
			this.TimeEnded = DateTimeNowHelper.NowSafe;
			this.isRecording = false;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0031657C File Offset: 0x0031477C
		public static void AdaptToNewUnits(IDataRecordContainer recorder)
		{
			for (int i = 0; i < recorder.Records.Count; i++)
			{
				UnitsHelper.Units units = recorder.Records[i].Units;
				int pid_Id = recorder.Records[i].PID_Id;
				PIDOverride pidoverride = null;
				UnitsHelper.Units units2 = UnitsHelper.Units.None;
				if (PIDOverrideDictionary.Instance.TryGetValue(pid_Id, out pidoverride) && pidoverride.Unit != UnitsHelper.Units.None)
				{
					units2 = pidoverride.Unit;
					recorder.Records[i].Units = units2;
				}
				for (int j = 0; j < recorder.Records[i].Elements.Count; j++)
				{
					double value = recorder.Records[i].Elements[j].Value;
					if (!double.IsInfinity(value))
					{
						double num;
						if (units2 == UnitsHelper.Units.None)
						{
							num = UnitsHelper.GetValue(value, units);
						}
						else
						{
							num = UnitsHelper.Convert(value, units, units2);
						}
						if (double.IsInfinity(num))
						{
							recorder.Records[i].Elements[j].Value = double.NaN;
						}
						else
						{
							recorder.Records[i].Elements[j].Value = num;
						}
					}
					else
					{
						recorder.Records[i].Elements[j].Value = double.NaN;
					}
				}
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x003166E8 File Offset: 0x003148E8
		public void WriteToStreamDataRecorder(JsonTextWriter writer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("VIN");
			writer.WriteValue(this.VIN);
			writer.WritePropertyName("TimeStarted");
			writer.WriteValue(this.TimeStarted);
			writer.WritePropertyName("ConnectionProfile");
			writer.WriteValue(this.ConnectionProfile);
			writer.WritePropertyName("Device");
			writer.WriteValue(this.Device);
			writer.WritePropertyName("TimeEnded");
			writer.WriteValue(this.TimeEnded);
			writer.WritePropertyName("Records");
			writer.WriteStartArray();
			foreach (DataRecord dataRecord in this._Records)
			{
				dataRecord.WriteToStreamDataRecord(writer);
			}
			writer.WriteEndArray();
			writer.WriteEndObject();
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x003167CC File Offset: 0x003149CC
		private static void WriteRecorder(DataRecorder recorder, BinaryWriter writer)
		{
			DataRecorder.Write(writer, DataRecorder.DataIds.BINVersion, 1);
			writer.Write(1);
			DataRecorder.Write(writer, DataRecorder.DataIds.VIN, recorder.VIN);
			DataRecorder.Write(writer, DataRecorder.DataIds.TimeStarted, recorder.TimeStarted.Ticks);
			DataRecorder.Write(writer, DataRecorder.DataIds.TimeEnded, recorder.TimeEnded.Ticks);
			DataRecorder.Write(writer, DataRecorder.DataIds.ConnectionProfile, recorder.ConnectionProfile);
			DataRecorder.Write(writer, DataRecorder.DataIds.Device, recorder.Device);
			DataRecorder.Write(writer, DataRecorder.DataIds.RecordsStart, recorder._Records.Count);
			foreach (DataRecord dataRecord in recorder._Records)
			{
				DataRecorder.WriteRecord(dataRecord, writer);
			}
			writer.Write(9);
			writer.Write(2);
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x0031689C File Offset: 0x00314A9C
		private static void WriteRecord(DataRecord rec, BinaryWriter writer)
		{
			writer.Write(10);
			DataRecorder.Write(writer, DataRecorder.DataIds.PID_Id, rec.PID_Id);
			DataRecorder.Write(writer, DataRecorder.DataIds.Name, rec.Name);
			DataRecorder.Write(writer, DataRecorder.DataIds.ShortName, rec.ShortName);
			DataRecorder.Write(writer, DataRecorder.DataIds.Units, rec.Units);
			DataRecorder.Write(writer, DataRecorder.DataIds.ElementsStart, rec.Elements.Count);
			foreach (DataRecordElement dataRecordElement in rec.Elements)
			{
				DataRecorder.WriteElement(dataRecordElement, writer);
			}
			writer.Write(17);
			writer.Write(11);
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00316950 File Offset: 0x00314B50
		private static void WriteElement(DataRecordElement element, BinaryWriter writer)
		{
			writer.Write(element.Value);
			writer.Write(element.Seconds);
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x0031696C File Offset: 0x00314B6C
		public static DataRecorder LoadFromFile(string fileName)
		{
			DataRecorder dataRecorder2;
			try
			{
				using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
				{
					DataRecorder dataRecorder = DataRecorder.LoadFromStream(fileStream, false);
					dataRecorder.Filename = fileName;
					dataRecorder2 = dataRecorder;
				}
			}
			catch (Exception)
			{
				dataRecorder2 = null;
			}
			return dataRecorder2;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x003169C0 File Offset: 0x00314BC0
		public static DataRecorder LoadFromStream(Stream fstream, bool keepOpen = false)
		{
			DataRecorder dataRecorder;
			using (BinaryReader binaryReader = new BinaryReader(fstream, Encoding.UTF8, keepOpen))
			{
				if (binaryReader.ReadByte() == 22)
				{
					binaryReader.ReadInt32();
					dataRecorder = DataRecorder.ReadRecorder(binaryReader);
				}
				else
				{
					dataRecorder = null;
				}
			}
			return dataRecorder;
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x00316A18 File Offset: 0x00314C18
		private static DataRecorder ReadRecorder(BinaryReader br)
		{
			DataRecorder dataRecorder = new DataRecorder();
			DataRecorder.DataIds dataIds;
			do
			{
				dataIds = (DataRecorder.DataIds)br.ReadByte();
				switch (dataIds)
				{
				case DataRecorder.DataIds.VIN:
					dataRecorder.VIN = br.ReadString();
					break;
				case DataRecorder.DataIds.TimeStarted:
					dataRecorder.TimeStarted = new DateTime(br.ReadInt64());
					break;
				case DataRecorder.DataIds.TimeEnded:
					dataRecorder.TimeEnded = new DateTime(br.ReadInt64());
					break;
				case DataRecorder.DataIds.ConnectionProfile:
					dataRecorder.ConnectionProfile = br.ReadString();
					break;
				case DataRecorder.DataIds.Device:
					dataRecorder.Device = br.ReadString();
					break;
				case DataRecorder.DataIds.RecordsStart:
				{
					br.ReadInt32();
					DataRecorder.DataIds dataIds2;
					do
					{
						dataIds2 = (DataRecorder.DataIds)br.ReadByte();
						if (dataIds2 != DataRecorder.DataIds.RecordStart)
						{
							break;
						}
						DataRecord dataRecord = DataRecorder.ReadRecord(br);
						if (dataRecord.Elements.Count != dataRecord.Elements.Count((DataRecordElement x) => !double.IsFinite(x.Value)))
						{
							dataRecorder.Records.Add(dataRecord);
						}
					}
					while (dataIds2 == DataRecorder.DataIds.RecordStart);
					break;
				}
				}
			}
			while (dataIds != DataRecorder.DataIds.RecorderEnd || br.BaseStream.Position < br.BaseStream.Length);
			dataRecorder.Records = dataRecorder.Records.OrderBy((DataRecord x) => x.Name).ToList<DataRecord>();
			dataRecorder.Synchronize();
			double totalSeconds = dataRecorder.TimeStarted.TimeOfDay.TotalSeconds;
			foreach (DataRecord dataRecord2 in dataRecorder.Records)
			{
				foreach (DataRecordElement dataRecordElement in dataRecord2.Elements)
				{
					dataRecordElement.Seconds += totalSeconds;
				}
			}
			return dataRecorder;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00316C24 File Offset: 0x00314E24
		private static DataRecord ReadRecord(BinaryReader br)
		{
			bool androidChartRenderingSafeMode = SharedSettings.Current.AndroidChartRenderingSafeMode;
			DataRecord dataRecord = new DataRecord();
			DataRecorder.DataIds dataIds;
			do
			{
				dataIds = (DataRecorder.DataIds)br.ReadByte();
				switch (dataIds)
				{
				case DataRecorder.DataIds.PID_Id:
					dataRecord.PID_Id = br.ReadInt32();
					break;
				case DataRecorder.DataIds.Name:
					dataRecord.Name = br.ReadString();
					break;
				case DataRecorder.DataIds.ShortName:
					dataRecord.ShortName = br.ReadString();
					break;
				case DataRecorder.DataIds.Units:
					dataRecord.Units = (UnitsHelper.Units)br.ReadInt32();
					break;
				case DataRecorder.DataIds.ElementsStart:
				{
					int num = br.ReadInt32();
					for (int i = 0; i < num; i++)
					{
						DataRecordElement dataRecordElement = DataRecorder.ReadElement(br);
						if ((!androidChartRenderingSafeMode || double.IsFinite(dataRecordElement.Value)) && !double.IsInfinity(dataRecordElement.Value))
						{
							dataRecord.Elements.Add(dataRecordElement);
						}
					}
					break;
				}
				}
			}
			while (dataIds != DataRecorder.DataIds.RecordEnd || br.BaseStream.Position == br.BaseStream.Length);
			return dataRecord;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00316D18 File Offset: 0x00314F18
		private static DataRecordElement ReadElement(BinaryReader br)
		{
			return new DataRecordElement
			{
				Value = br.ReadDouble(),
				Seconds = br.ReadDouble()
			};
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00316D37 File Offset: 0x00314F37
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, long value)
		{
			writer.Write((byte)id);
			writer.Write(value);
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x00316D47 File Offset: 0x00314F47
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, double value)
		{
			writer.Write((byte)id);
			writer.Write(value);
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00316D57 File Offset: 0x00314F57
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, int value)
		{
			writer.Write((byte)id);
			writer.Write(value);
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x00316D67 File Offset: 0x00314F67
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, string value)
		{
			writer.Write((byte)id);
			if (string.IsNullOrEmpty(value))
			{
				writer.Write("");
				return;
			}
			writer.Write(value);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x00316D8B File Offset: 0x00314F8B
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, byte value)
		{
			writer.Write((byte)id);
			writer.Write(value);
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x00316D57 File Offset: 0x00314F57
		private static void Write(BinaryWriter writer, DataRecorder.DataIds id, UnitsHelper.Units value)
		{
			writer.Write((byte)id);
			writer.Write((int)value);
		}

		// Token: 0x0400249C RID: 9372
		private bool isRecording;

		// Token: 0x0400249D RID: 9373
		[CompilerGenerated]
		private string <VIN>k__BackingField;

		// Token: 0x0400249E RID: 9374
		[CompilerGenerated]
		private DateTime <TimeStarted>k__BackingField;

		// Token: 0x0400249F RID: 9375
		[CompilerGenerated]
		private string <ConnectionProfile>k__BackingField;

		// Token: 0x040024A0 RID: 9376
		[CompilerGenerated]
		private string <CarName>k__BackingField;

		// Token: 0x040024A1 RID: 9377
		[CompilerGenerated]
		private string <Device>k__BackingField;

		// Token: 0x040024A2 RID: 9378
		[CompilerGenerated]
		private DateTime <TimeEnded>k__BackingField;

		// Token: 0x040024A3 RID: 9379
		[CompilerGenerated]
		private ConcurrentBag<DataRecord> <_Records>k__BackingField;

		// Token: 0x040024A4 RID: 9380
		[CompilerGenerated]
		private List<DTCItemV2> <DTCs>k__BackingField;

		// Token: 0x040024A5 RID: 9381
		[CompilerGenerated]
		private List<DataRecord> <Records>k__BackingField;

		// Token: 0x040024A6 RID: 9382
		[CompilerGenerated]
		private string <Filename>k__BackingField;

		// Token: 0x020006DF RID: 1759
		private enum DataIds : byte
		{
			// Token: 0x040024A8 RID: 9384
			None,
			// Token: 0x040024A9 RID: 9385
			RecorderStart,
			// Token: 0x040024AA RID: 9386
			RecorderEnd,
			// Token: 0x040024AB RID: 9387
			VIN,
			// Token: 0x040024AC RID: 9388
			TimeStarted,
			// Token: 0x040024AD RID: 9389
			TimeEnded,
			// Token: 0x040024AE RID: 9390
			ConnectionProfile,
			// Token: 0x040024AF RID: 9391
			Device,
			// Token: 0x040024B0 RID: 9392
			RecordsStart,
			// Token: 0x040024B1 RID: 9393
			RecordsEnd,
			// Token: 0x040024B2 RID: 9394
			RecordStart,
			// Token: 0x040024B3 RID: 9395
			RecordEnd,
			// Token: 0x040024B4 RID: 9396
			PID_Id,
			// Token: 0x040024B5 RID: 9397
			Name,
			// Token: 0x040024B6 RID: 9398
			ShortName,
			// Token: 0x040024B7 RID: 9399
			Units,
			// Token: 0x040024B8 RID: 9400
			ElementsStart,
			// Token: 0x040024B9 RID: 9401
			ElementsEnd,
			// Token: 0x040024BA RID: 9402
			ElementStart,
			// Token: 0x040024BB RID: 9403
			ElementEnd,
			// Token: 0x040024BC RID: 9404
			Value,
			// Token: 0x040024BD RID: 9405
			Seconds,
			// Token: 0x040024BE RID: 9406
			BINVersion
		}

		// Token: 0x020006E0 RID: 1760
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003C11 RID: 15377 RVA: 0x00316D9B File Offset: 0x00314F9B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003C12 RID: 15378 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003C13 RID: 15379 RVA: 0x00315B68 File Offset: 0x00313D68
			internal bool <ReadRecorder>b__72_1(DataRecordElement x)
			{
				return !double.IsFinite(x.Value);
			}

			// Token: 0x06003C14 RID: 15380 RVA: 0x00315BB1 File Offset: 0x00313DB1
			internal string <ReadRecorder>b__72_0(DataRecord x)
			{
				return x.Name;
			}

			// Token: 0x040024BF RID: 9407
			public static readonly DataRecorder.<>c <>9 = new DataRecorder.<>c();

			// Token: 0x040024C0 RID: 9408
			public static Func<DataRecordElement, bool> <>9__72_1;

			// Token: 0x040024C1 RID: 9409
			public static Func<DataRecord, string> <>9__72_0;
		}

		// Token: 0x020006E1 RID: 1761
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SaveJson>d__61 : IAsyncStateMachine
		{
			// Token: 0x06003C15 RID: 15381 RVA: 0x00316DA8 File Offset: 0x00314FA8
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				DataRecorder dataRecorder = this;
				try
				{
					FileStream fileStream = File.Open(Path.Combine(FileSystemHelper.LocalStoragePath, dataRecorder.Filename + ".rec"), FileMode.Create);
					try
					{
						StreamWriter streamWriter = new StreamWriter(fileStream);
						try
						{
							JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter);
							try
							{
								jsonTextWriter.Culture = CultureInfo.InvariantCulture;
								dataRecorder.WriteToStreamDataRecorder(jsonTextWriter);
							}
							finally
							{
								if (num < 0 && jsonTextWriter != null)
								{
									jsonTextWriter.Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && streamWriter != null)
							{
								((IDisposable)streamWriter).Dispose();
							}
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
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003C16 RID: 15382 RVA: 0x00316E9C File Offset: 0x0031509C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040024C2 RID: 9410
			public int <>1__state;

			// Token: 0x040024C3 RID: 9411
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040024C4 RID: 9412
			public DataRecorder <>4__this;
		}
	}
}
