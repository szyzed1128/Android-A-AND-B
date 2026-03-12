using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000713 RID: 1811
	public class DriveCycle
	{
		// Token: 0x17001414 RID: 5140
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x003282FE File Offset: 0x003264FE
		public static DriveCycle Current
		{
			get
			{
				return DriveCycle._Current;
			}
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x00328308 File Offset: 0x00326508
		public static void SaveAndReset()
		{
			object obj = DriveCycle.saveLock;
			lock (obj)
			{
				DriveCycle driveCycle = DriveCycle.Current;
				if (driveCycle != null)
				{
					driveCycle.Save(true);
				}
			}
			DriveCycle._Current = new DriveCycle();
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x0032835C File Offset: 0x0032655C
		private DriveCycle()
		{
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x003283CC File Offset: 0x003265CC
		public void RecordDistance(double distance, TimeSpan timeStamp)
		{
			object obj = this.distanceLock;
			lock (obj)
			{
				if (this.lastDistanceTicks != timeStamp.Ticks || this.lastDistance != distance)
				{
					this.lastDistanceTicks = timeStamp.Ticks;
					this.Distance += distance;
					if (!this.WasStarted)
					{
						this.SetupStartTime(timeStamp);
					}
					this.LastRecordTime = timeStamp;
				}
			}
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x00328454 File Offset: 0x00326654
		public void RecordFuel(double fuel, TimeSpan timeStamp)
		{
			object obj = this.fuelLock;
			lock (obj)
			{
				if (this.lastFuelTicks != timeStamp.Ticks || this.lastFuel != fuel)
				{
					this.lastFuelTicks = timeStamp.Ticks;
					this.FuelUsed += fuel;
					if (!this.WasStarted)
					{
						this.SetupStartTime(timeStamp);
					}
					this.LastRecordTime = timeStamp;
				}
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x003284DC File Offset: 0x003266DC
		private void SetupStartTime(TimeSpan timeStamp)
		{
			this.TimeStarted = DateTimeNowHelper.NowSafe;
			this.EllapsedSinceConnection = timeStamp;
			this.WasStarted = true;
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x003284F8 File Offset: 0x003266F8
		public void SetFinishTime()
		{
			this.TimeFinished = new DateTime(this.TimeStarted.Ticks + this.LastRecordTime.Ticks - this.EllapsedSinceConnection.Ticks);
		}

		// Token: 0x17001415 RID: 5141
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x00328536 File Offset: 0x00326736
		// (set) Token: 0x06003D7D RID: 15741 RVA: 0x0032853E File Offset: 0x0032673E
		public double Distance
		{
			[CompilerGenerated]
			get
			{
				return this.<Distance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Distance>k__BackingField = value;
			}
		}

		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x06003D7E RID: 15742 RVA: 0x00328547 File Offset: 0x00326747
		// (set) Token: 0x06003D7F RID: 15743 RVA: 0x0032854F File Offset: 0x0032674F
		public double FuelUsed
		{
			[CompilerGenerated]
			get
			{
				return this.<FuelUsed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FuelUsed>k__BackingField = value;
			}
		}

		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06003D80 RID: 15744 RVA: 0x00328558 File Offset: 0x00326758
		// (set) Token: 0x06003D81 RID: 15745 RVA: 0x00328560 File Offset: 0x00326760
		public double EnergyUsed
		{
			[CompilerGenerated]
			get
			{
				return this.<EnergyUsed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EnergyUsed>k__BackingField = value;
			}
		}

		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06003D82 RID: 15746 RVA: 0x00328569 File Offset: 0x00326769
		// (set) Token: 0x06003D83 RID: 15747 RVA: 0x00328571 File Offset: 0x00326771
		public decimal FuelPricePerL
		{
			[CompilerGenerated]
			get
			{
				return this.<FuelPricePerL>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FuelPricePerL>k__BackingField = value;
			}
		}

		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06003D84 RID: 15748 RVA: 0x0032857A File Offset: 0x0032677A
		// (set) Token: 0x06003D85 RID: 15749 RVA: 0x00328582 File Offset: 0x00326782
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

		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0032858B File Offset: 0x0032678B
		// (set) Token: 0x06003D87 RID: 15751 RVA: 0x00328593 File Offset: 0x00326793
		public DateTime TimeFinished
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeFinished>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeFinished>k__BackingField = value;
			}
		}

		// Token: 0x1700141B RID: 5147
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x0032859C File Offset: 0x0032679C
		[JsonIgnore]
		public double AvgFuelConsumption
		{
			get
			{
				double num = this.FuelUsed * 100.0 / this.Distance;
				if (double.IsInfinity(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x1700141C RID: 5148
		// (get) Token: 0x06003D89 RID: 15753 RVA: 0x003285D4 File Offset: 0x003267D4
		[JsonIgnore]
		public double AvgSpeed
		{
			get
			{
				double num = this.Distance / this.Duration.TotalHours;
				if (double.IsInfinity(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x1700141D RID: 5149
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0032860A File Offset: 0x0032680A
		[JsonIgnore]
		public double AvgSpeedUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.AvgSpeed, UnitsHelper.Units.kmh);
			}
		}

		// Token: 0x1700141E RID: 5150
		// (get) Token: 0x06003D8B RID: 15755 RVA: 0x00328618 File Offset: 0x00326818
		[JsonIgnore]
		public double AvgFuelConsumptionUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.AvgFuelConsumption, UnitsHelper.Units.liters100km);
			}
		}

		// Token: 0x1700141F RID: 5151
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x00328627 File Offset: 0x00326827
		[JsonIgnore]
		public double FuelUsedUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.FuelUsed, UnitsHelper.Units.liters);
			}
		}

		// Token: 0x17001420 RID: 5152
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x00328636 File Offset: 0x00326836
		[JsonIgnore]
		public double DistanceUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.Distance, UnitsHelper.Units.km);
			}
		}

		// Token: 0x17001421 RID: 5153
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x00328644 File Offset: 0x00326844
		[JsonIgnore]
		public decimal TotalFuelPrice
		{
			get
			{
				return (decimal)this.FuelUsed * this.FuelPricePerL;
			}
		}

		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x0032865C File Offset: 0x0032685C
		[JsonIgnore]
		public virtual TimeSpan Duration
		{
			get
			{
				return new TimeSpan(this.TimeFinished.Ticks - this.TimeStarted.Ticks);
			}
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x0026161F File Offset: 0x0025F81F
		public string DistanceUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.km);
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x00261627 File Offset: 0x0025F827
		public string FuelUsedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters);
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x00261630 File Offset: 0x0025F830
		public string FuelConsumptionsUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters100km);
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x00261639 File Offset: 0x0025F839
		public string SpeedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.kmh);
			}
		}

		// Token: 0x17001427 RID: 5159
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x00261641 File Offset: 0x0025F841
		public string FuelPriceUnits
		{
			get
			{
				return SharedSettings.Current.Currency;
			}
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x0032868C File Offset: 0x0032688C
		private static string GetFilename(DriveCycle dc)
		{
			return dc.TimeStarted.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture) + ".dc";
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x003286BC File Offset: 0x003268BC
		private void WriteBIN_V1(BinaryWriter writer)
		{
			writer.BaseStream.Position = writer.BaseStream.Length;
			writer.Write(254);
			writer.Write(1);
			writer.Write(this.Distance);
			writer.Write(this.FuelUsed);
			writer.Write(this.FuelPricePerL);
			writer.Write(this.TimeStarted.Ticks);
			writer.Write(this.TimeFinished.Ticks);
			writer.Write(byte.MaxValue);
		}

		// Token: 0x06003D97 RID: 15767 RVA: 0x00328748 File Offset: 0x00326948
		private void WriteBIN_V2(BinaryWriter writer)
		{
			writer.BaseStream.Position = writer.BaseStream.Length;
			writer.Write(254);
			writer.Write(2);
			writer.Write(this.Distance);
			writer.Write(this.FuelUsed);
			writer.Write(this.EnergyUsed);
			writer.Write(this.FuelPricePerL);
			writer.Write(this.TimeStarted.Ticks);
			writer.Write(this.TimeFinished.Ticks);
			writer.Write(byte.MaxValue);
		}

		// Token: 0x06003D98 RID: 15768 RVA: 0x003287E0 File Offset: 0x003269E0
		private static DriveCycle ReadBIN_V2(BinaryReader br)
		{
			DriveCycle driveCycle = new DriveCycle();
			driveCycle.Distance = br.ReadDouble();
			driveCycle.FuelUsed = br.ReadDouble();
			driveCycle.EnergyUsed = br.ReadDouble();
			driveCycle.FuelPricePerL = br.ReadDecimal();
			driveCycle.TimeStarted = new DateTime(br.ReadInt64());
			driveCycle.TimeFinished = new DateTime(br.ReadInt64());
			if (br.ReadByte() == 255)
			{
				return driveCycle;
			}
			return null;
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x00328858 File Offset: 0x00326A58
		private static DriveCycle ReadBIN_V1(BinaryReader br)
		{
			DriveCycle driveCycle = new DriveCycle();
			long position = br.BaseStream.Position;
			driveCycle.Distance = br.ReadDouble();
			driveCycle.FuelUsed = br.ReadDouble();
			driveCycle.FuelPricePerL = br.ReadDecimal();
			driveCycle.TimeStarted = new DateTime(br.ReadInt64());
			driveCycle.TimeFinished = new DateTime(br.ReadInt64());
			if (br.ReadByte() == 255)
			{
				return driveCycle;
			}
			br.BaseStream.Seek(position, SeekOrigin.Begin);
			return DriveCycle.ReadBIN_V2(br);
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x003288E4 File Offset: 0x00326AE4
		public static List<DriveCycle> LoadDriveCycles()
		{
			List<DriveCycle> list = new List<DriveCycle>(128);
			try
			{
				using (FileStream fileStream = File.OpenRead(FileSystemHelper.GetLocalFilePath("drivecycles.bin")))
				{
					using (MemoryStream memoryStream = new MemoryStream((int)fileStream.Length))
					{
						fileStream.CopyTo(memoryStream);
						memoryStream.Seek(0L, SeekOrigin.Begin);
						using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
						{
							while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
							{
								if (binaryReader.ReadByte() == 254)
								{
									int num = (int)binaryReader.ReadByte();
									if (num != 1 && num == 2)
									{
										DriveCycle driveCycle = DriveCycle.ReadBIN_V2(binaryReader);
										if (driveCycle != null)
										{
											int num2 = list.Count - 1;
											if (num2 >= 0 && list[num2].TimeStarted.Ticks == driveCycle.TimeStarted.Ticks)
											{
												list[num2] = driveCycle;
											}
											else
											{
												list.Add(driveCycle);
											}
										}
									}
									else
									{
										DriveCycle driveCycle2 = DriveCycle.ReadBIN_V1(binaryReader);
										if (driveCycle2 != null)
										{
											int num3 = list.Count - 1;
											if (num3 >= 0 && list[num3].TimeStarted.Ticks == driveCycle2.TimeStarted.Ticks)
											{
												list[num3] = driveCycle2;
											}
											else
											{
												list.Add(driveCycle2);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x00328ABC File Offset: 0x00326CBC
		public static async Task<IEnumerable<DriveCycle>> LoadDriveCyclesAsync()
		{
			return await Task.Run<List<DriveCycle>>(() => DriveCycle.LoadDriveCycles());
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x00328AF8 File Offset: 0x00326CF8
		private static async Task<bool> RestoreFromBackupFile()
		{
			bool flag;
			try
			{
				string localFilePath = FileSystemHelper.GetLocalFilePath("drivecycles.bin.bak");
				if (!File.Exists(localFilePath))
				{
					flag = false;
				}
				else
				{
					if (File.Exists(FileSystemHelper.GetLocalFilePath("drivecycles.bin")))
					{
						try
						{
							File.Delete(FileSystemHelper.GetLocalFilePath("drivecycles.bin"));
						}
						catch (Exception)
						{
						}
					}
					File.Move(localFilePath, FileSystemHelper.GetLocalFilePath("drivecycles.bin"));
					flag = true;
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x00328B34 File Offset: 0x00326D34
		private void Save(bool ShouldSetFinishTime)
		{
			if (App.OBDSimulator.IsActive)
			{
				return;
			}
			if (this.FuelUsed > 0.0 || this.EnergyUsed > 0.0 || (SharedSettings.Current.FuelHybridCar && this.Distance > 0.0))
			{
				if (ShouldSetFinishTime)
				{
					this.SetFinishTime();
				}
				this.FuelPricePerL = SharedSettings.Current.FuelPriceForLitre;
				DriveCycle.SaveBin(this);
				DriveCycleViewModel.Current.AddDriveCycle(DriveCycle.Current);
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00328BBC File Offset: 0x00326DBC
		private static void SaveBin(DriveCycle dc)
		{
			object obj = DriveCycle.saveLock;
			lock (obj)
			{
				try
				{
					if (dc.TimeFinished.Ticks == 0L)
					{
						dc.TimeFinished = DateTimeNowHelper.NowSafe;
					}
					using (FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath("drivecycles.bin"), FileMode.OpenOrCreate))
					{
						fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							dc.WriteBIN_V2(binaryWriter);
							fileStream.Flush();
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x00328C88 File Offset: 0x00326E88
		public static void SaveRange(IEnumerable<DriveCycle> dcs)
		{
			object obj = DriveCycle.saveLock;
			lock (obj)
			{
				try
				{
					using (FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath("drivecycles.bin"), FileMode.Create))
					{
						fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							foreach (DriveCycle driveCycle in dcs)
							{
								driveCycle.WriteBIN_V2(binaryWriter);
							}
							fileStream.Flush();
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00328D68 File Offset: 0x00326F68
		// Note: this type is marked as 'beforefieldinit'.
		static DriveCycle()
		{
		}

		// Token: 0x040025B1 RID: 9649
		private static DriveCycle _Current = new DriveCycle();

		// Token: 0x040025B2 RID: 9650
		private static object saveLock = new object();

		// Token: 0x040025B3 RID: 9651
		private bool WasStarted;

		// Token: 0x040025B4 RID: 9652
		private object distanceLock = new object();

		// Token: 0x040025B5 RID: 9653
		private long lastDistanceTicks = -1L;

		// Token: 0x040025B6 RID: 9654
		private double lastDistance = -1.0;

		// Token: 0x040025B7 RID: 9655
		private long lastFuelTicks = -1L;

		// Token: 0x040025B8 RID: 9656
		private double lastFuel = -1.0;

		// Token: 0x040025B9 RID: 9657
		private object fuelLock = new object();

		// Token: 0x040025BA RID: 9658
		private TimeSpan EllapsedSinceConnection = TimeSpan.Zero;

		// Token: 0x040025BB RID: 9659
		private TimeSpan LastRecordTime = TimeSpan.Zero;

		// Token: 0x040025BC RID: 9660
		[CompilerGenerated]
		private double <Distance>k__BackingField;

		// Token: 0x040025BD RID: 9661
		[CompilerGenerated]
		private double <FuelUsed>k__BackingField;

		// Token: 0x040025BE RID: 9662
		[CompilerGenerated]
		private double <EnergyUsed>k__BackingField;

		// Token: 0x040025BF RID: 9663
		[CompilerGenerated]
		private decimal <FuelPricePerL>k__BackingField;

		// Token: 0x040025C0 RID: 9664
		[CompilerGenerated]
		private DateTime <TimeStarted>k__BackingField;

		// Token: 0x040025C1 RID: 9665
		[CompilerGenerated]
		private DateTime <TimeFinished>k__BackingField;

		// Token: 0x040025C2 RID: 9666
		private const byte DriveCycleStartTag = 254;

		// Token: 0x040025C3 RID: 9667
		private const byte DriveCycleEndTag = 255;

		// Token: 0x02000714 RID: 1812
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003DA1 RID: 15777 RVA: 0x00328D7E File Offset: 0x00326F7E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003DA2 RID: 15778 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003DA3 RID: 15779 RVA: 0x00328D8A File Offset: 0x00326F8A
			internal List<DriveCycle> <LoadDriveCyclesAsync>b__77_0()
			{
				return DriveCycle.LoadDriveCycles();
			}

			// Token: 0x040025C4 RID: 9668
			public static readonly DriveCycle.<>c <>9 = new DriveCycle.<>c();

			// Token: 0x040025C5 RID: 9669
			public static Func<List<DriveCycle>> <>9__77_0;
		}

		// Token: 0x02000715 RID: 1813
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadDriveCyclesAsync>d__77 : IAsyncStateMachine
		{
			// Token: 0x06003DA4 RID: 15780 RVA: 0x00328D94 File Offset: 0x00326F94
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				IEnumerable<DriveCycle> result;
				try
				{
					TaskAwaiter<List<DriveCycle>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Task.Run<List<DriveCycle>>(() => DriveCycle.LoadDriveCycles()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<List<DriveCycle>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<DriveCycle>>, DriveCycle.<LoadDriveCyclesAsync>d__77>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<List<DriveCycle>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<DriveCycle>>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06003DA5 RID: 15781 RVA: 0x00328E60 File Offset: 0x00327060
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025C6 RID: 9670
			public int <>1__state;

			// Token: 0x040025C7 RID: 9671
			public AsyncTaskMethodBuilder<IEnumerable<DriveCycle>> <>t__builder;

			// Token: 0x040025C8 RID: 9672
			private TaskAwaiter<List<DriveCycle>> <>u__1;
		}

		// Token: 0x02000716 RID: 1814
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RestoreFromBackupFile>d__78 : IAsyncStateMachine
		{
			// Token: 0x06003DA6 RID: 15782 RVA: 0x00328E70 File Offset: 0x00327070
			void IAsyncStateMachine.MoveNext()
			{
				bool flag;
				try
				{
					try
					{
						string localFilePath = FileSystemHelper.GetLocalFilePath("drivecycles.bin.bak");
						if (!File.Exists(localFilePath))
						{
							flag = false;
						}
						else
						{
							if (File.Exists(FileSystemHelper.GetLocalFilePath("drivecycles.bin")))
							{
								try
								{
									File.Delete(FileSystemHelper.GetLocalFilePath("drivecycles.bin"));
								}
								catch (Exception)
								{
								}
							}
							File.Move(localFilePath, FileSystemHelper.GetLocalFilePath("drivecycles.bin"));
							flag = true;
						}
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06003DA7 RID: 15783 RVA: 0x00328F28 File Offset: 0x00327128
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025C9 RID: 9673
			public int <>1__state;

			// Token: 0x040025CA RID: 9674
			public AsyncTaskMethodBuilder<bool> <>t__builder;
		}
	}
}
