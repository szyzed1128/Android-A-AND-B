using System;
using System.Collections.Generic;
using System.Linq;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200043D RID: 1085
	internal class DoubleValueTimeLimitedList
	{
		// Token: 0x06002DE0 RID: 11744 RVA: 0x00200F68 File Offset: 0x001FF168
		public DoubleValueTimeLimitedList(TimeSpan timeLimit)
		{
			this.TimeLimit = timeLimit;
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x00200F90 File Offset: 0x001FF190
		public void AddLast(DoubleValueItem item)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.list.AddLast(item);
				this.UpdateForTimeLimit();
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x00200FE0 File Offset: 0x001FF1E0
		public DoubleValueItem[] GetItems()
		{
			return this.list.ToArray<DoubleValueItem>();
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x00200FF0 File Offset: 0x001FF1F0
		private void UpdateForTimeLimit()
		{
			TimeSpan timeSpan = App.OBDReader.stopwatch.Elapsed - this.TimeLimit;
			object obj = this.lockObj;
			lock (obj)
			{
				while (this.list.Count > 0 && this.list.First<DoubleValueItem>().TimeAdded < timeSpan)
				{
					this.list.RemoveFirst();
				}
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0020107C File Offset: 0x001FF27C
		public double GetAverage()
		{
			double num = 0.0;
			object obj = this.lockObj;
			lock (obj)
			{
				foreach (DoubleValueItem doubleValueItem in this.list)
				{
					num += doubleValueItem.Value;
				}
			}
			return num / (double)this.list.Count;
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00201114 File Offset: 0x001FF314
		public double GetMinimum()
		{
			if (this.list.Count == 0)
			{
				return double.NaN;
			}
			double num = double.NaN;
			object obj = this.lockObj;
			lock (obj)
			{
				num = this.list.First.Value.Value;
				foreach (DoubleValueItem doubleValueItem in this.list)
				{
					if (doubleValueItem.Value < num)
					{
						num = doubleValueItem.Value;
					}
				}
			}
			return num;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x002011D8 File Offset: 0x001FF3D8
		public double GetMaximum()
		{
			if (this.list.Count == 0)
			{
				return double.NaN;
			}
			double num = double.NaN;
			object obj = this.lockObj;
			lock (obj)
			{
				num = this.list.First.Value.Value;
				foreach (DoubleValueItem doubleValueItem in this.list)
				{
					if (doubleValueItem.Value > num)
					{
						num = doubleValueItem.Value;
					}
				}
			}
			return num;
		}

		// Token: 0x040019D0 RID: 6608
		public readonly TimeSpan TimeLimit;

		// Token: 0x040019D1 RID: 6609
		private LinkedList<DoubleValueItem> list = new LinkedList<DoubleValueItem>();

		// Token: 0x040019D2 RID: 6610
		private object lockObj = new object();
	}
}
