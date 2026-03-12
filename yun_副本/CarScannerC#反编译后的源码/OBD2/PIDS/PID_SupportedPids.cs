using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000421 RID: 1057
	public class PID_SupportedPids : PID
	{
		// Token: 0x06002D28 RID: 11560 RVA: 0x001FE79F File Offset: 0x001FC99F
		public PID_SupportedPids(string Command, IEnumerable<PID> PIDList, string Header = "")
			: base("Supported PIDS", Command)
		{
			this.UpdateInterval();
			this.Header = Header;
			this.PIDList = PIDList;
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x001FE7C8 File Offset: 0x001FC9C8
		public void UpdateInterval()
		{
			this.Value = new bool[32];
			if (SharedSettings.Current.DaihatsuKLine)
			{
				this.start_value = BitHelpers.ConvertHexToInt(this.Command) + 256;
				this.end_value = this.start_value + 7936;
				return;
			}
			this.start_value = BitHelpers.ConvertHexToInt(this.Command) + 1;
			this.end_value = this.start_value + 31;
		}

		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x001FE83A File Offset: 0x001FCA3A
		// (set) Token: 0x06002D2B RID: 11563 RVA: 0x001FE842 File Offset: 0x001FCA42
		public new string Header
		{
			[CompilerGenerated]
			get
			{
				return this.<Header>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Header>k__BackingField = value;
			}
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x001FE84C File Offset: 0x001FCA4C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(string.Concat(new string[]
			{
				base.Name,
				" [",
				this.Command.Substring(0, 2),
				this.start_value.ToString("X2", CultureInfo.InvariantCulture),
				"..",
				this.Command.Substring(0, 2),
				this.end_value.ToString("X2", CultureInfo.InvariantCulture),
				"]\n"
			}));
			bool[] value = this.Value;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i])
				{
					stringBuilder.Append("1");
				}
				else
				{
					stringBuilder.Append("0");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06002D2D RID: 11565 RVA: 0x001FE915 File Offset: 0x001FCB15
		// (set) Token: 0x06002D2E RID: 11566 RVA: 0x001FE91D File Offset: 0x001FCB1D
		public bool[] Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x001FE928 File Offset: 0x001FCB28
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			if (data.Length >= 4)
			{
				this.IsAvailable = true;
			}
			bool[] array = new bool[32];
			for (int l = 0; l < 4; l++)
			{
				BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[l] }));
				bool[] array2 = new bool[8];
				for (int j = 0; j < 8; j++)
				{
					array2[j] = bitArrayReverse[j];
				}
				Array.Copy(array2, 0, array, l * 8, 8);
			}
			this.Value = array;
			int num = 0;
			if (SharedSettings.Current.DaihatsuKLine)
			{
				int m;
				for (m = this.start_value; m <= this.end_value; m += 256)
				{
					foreach (PID pid in this.PIDList.Where((PID x) => x.intCommand == m && ((x is CustomPID && (x as CustomPID).Header == this.Header) || !(x is CustomPID))))
					{
						if (!(pid is CustomPID))
						{
							pid.IsAvailable = array[num];
						}
						else if (pid.Id <= 1000 || !this.ShouldIgnoreCustomPIDs)
						{
							pid.IsAvailable = array[num];
						}
					}
					num++;
				}
			}
			else
			{
				string id = App.OBDReader.ECUHeaders[App.OBDReader.SelectedECU].Id;
				bool flag = (response_header == id) > false;
				int k;
				int i;
				for (i = this.start_value; i <= this.end_value; i = k + 1)
				{
					foreach (PID pid2 in this.PIDList.Where(delegate(PID x)
					{
						if (x.intCommand != i)
						{
							return false;
						}
						if (x is CustomPID)
						{
							CustomPID customPID = x as CustomPID;
							return (string.IsNullOrEmpty(customPID.Header) && string.IsNullOrEmpty(this.Header)) || customPID.Header == this.Header;
						}
						return true;
					}).ToArray<PID>())
					{
						if (!(pid2 is CustomPID))
						{
							if (array[num])
							{
								pid2.IsAvailable = true;
								if (flag)
								{
									OBDRequestQueueOptimizer.MainECUSupportedItems.Add(pid2.Command);
								}
							}
						}
						else if ((pid2.Id <= 1000 || !this.ShouldIgnoreCustomPIDs) && array[num])
						{
							pid2.IsAvailable = true;
							if (flag && array[num])
							{
								OBDRequestQueueOptimizer.MainECUSupportedItems.Add(pid2.Command);
							}
						}
					}
					num++;
					k = i;
				}
			}
			this.OnValueChanged();
		}

		// Token: 0x0400192C RID: 6444
		[CompilerGenerated]
		private string <Header>k__BackingField;

		// Token: 0x0400192D RID: 6445
		public bool ShouldIgnoreCustomPIDs = true;

		// Token: 0x0400192E RID: 6446
		private int start_value;

		// Token: 0x0400192F RID: 6447
		private int end_value;

		// Token: 0x04001930 RID: 6448
		private IEnumerable<PID> PIDList;

		// Token: 0x04001931 RID: 6449
		[CompilerGenerated]
		private bool[] <Value>k__BackingField;

		// Token: 0x02000422 RID: 1058
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x06002D30 RID: 11568 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x06002D31 RID: 11569 RVA: 0x001FEBBC File Offset: 0x001FCDBC
			internal bool <Decode>b__0(PID x)
			{
				return x.intCommand == this.i && ((x is CustomPID && (x as CustomPID).Header == this.<>4__this.Header) || !(x is CustomPID));
			}

			// Token: 0x04001932 RID: 6450
			public int i;

			// Token: 0x04001933 RID: 6451
			public PID_SupportedPids <>4__this;
		}

		// Token: 0x02000423 RID: 1059
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_1
		{
			// Token: 0x06002D32 RID: 11570 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_1()
			{
			}

			// Token: 0x06002D33 RID: 11571 RVA: 0x001FEC0C File Offset: 0x001FCE0C
			internal bool <Decode>b__1(PID x)
			{
				if (x.intCommand != this.i)
				{
					return false;
				}
				if (x is CustomPID)
				{
					CustomPID customPID = x as CustomPID;
					return (string.IsNullOrEmpty(customPID.Header) && string.IsNullOrEmpty(this.<>4__this.Header)) || customPID.Header == this.<>4__this.Header;
				}
				return true;
			}

			// Token: 0x04001934 RID: 6452
			public int i;

			// Token: 0x04001935 RID: 6453
			public PID_SupportedPids <>4__this;
		}
	}
}
