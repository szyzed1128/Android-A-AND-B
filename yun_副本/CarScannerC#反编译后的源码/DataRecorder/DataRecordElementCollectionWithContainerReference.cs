using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006DC RID: 1756
	public class DataRecordElementCollectionWithContainerReference : ObservableCollection<DataRecordElement>, IIsVisibleCollectionItem, INotifyPropertyChanged
	{
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06003BCB RID: 15307 RVA: 0x00315C4E File Offset: 0x00313E4E
		// (set) Token: 0x06003BCC RID: 15308 RVA: 0x00315C56 File Offset: 0x00313E56
		public DataRecord DataRecord
		{
			[CompilerGenerated]
			get
			{
				return this.<DataRecord>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DataRecord>k__BackingField = value;
			}
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00315C5F File Offset: 0x00313E5F
		public DataRecordElementCollectionWithContainerReference(DataRecord dataRecord, IEnumerable<DataRecordElement> elements)
			: base(elements)
		{
			this.DataRecord = dataRecord;
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00315C6F File Offset: 0x00313E6F
		public DataRecordElementCollectionWithContainerReference(DataRecord dataRecord)
		{
			this.DataRecord = dataRecord;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00315C80 File Offset: 0x00313E80
		public void UpdateForLastTime(double lastTime)
		{
			int num = this.DataRecord.Elements.FindLastIndex((DataRecordElement x) => x.Seconds <= lastTime);
			int num2 = base.Count - 1;
			if (num == -1)
			{
				base.Clear();
			}
			else if (num2 != num)
			{
				if (num2 < num)
				{
					IEnumerable<DataRecordElement> enumerable = this.DataRecord.Elements.Skip(base.Count).Take(num - base.Count + 1);
					try
					{
						foreach (DataRecordElement dataRecordElement in enumerable)
						{
							base.Add(dataRecordElement);
						}
						goto IL_00D7;
					}
					catch (Exception)
					{
						goto IL_00D7;
					}
				}
				if (num2 > num)
				{
					try
					{
						while (base.Count > 0 && base.Count > num + 1)
						{
							base.RemoveAt(base.Count - 1);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			IL_00D7:
			if (base.Count == 0)
			{
				this.LegendTitleWithValue = this.DataRecord.ShortName + " [" + UnitsHelper.GetCaption(this.DataRecord.Units) + "]";
				this.OnPropertyChanged(new PropertyChangedEventArgs("LegendTitleWithValue"));
				this.TextValue = "n/a";
				this.Value = double.NaN;
				this.IsVisible = false;
				this.OnPropertyChanged(new PropertyChangedEventArgs("LegendTitleWithValue"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("TextValue"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("Value"));
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
				return;
			}
			if (Math.Abs(base[base.Count - 1].Seconds - lastTime) > 5.0)
			{
				base.Items.Add(new DataRecordElement(double.NaN, lastTime));
			}
			double value = base[base.Count - 1].Value;
			this.Value = value;
			if (double.IsInfinity(value))
			{
				this.TextValue = "∞";
			}
			else if (double.IsNaN(value))
			{
				this.TextValue = "n/a";
				this.LegendTitleWithValue = this.DataRecord.ShortName + " [" + UnitsHelper.GetCaption(this.DataRecord.Units) + "]";
				this.OnPropertyChanged(new PropertyChangedEventArgs("LegendTitleWithValue"));
			}
			else
			{
				double value2 = UnitsHelper.GetValue(value, this.DataRecord.Units);
				this.LegendTitleWithValue = string.Concat(new string[]
				{
					this.DataRecord.ShortName,
					" [",
					value2.ToString(),
					" ",
					UnitsHelper.GetCaption(this.DataRecord.Units),
					"]"
				});
				this.TextValue = value.ToString("0.##", CultureInfo.InvariantCulture);
			}
			this.OnPropertyChanged(new PropertyChangedEventArgs("LegendTitleWithValue"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("TextValue"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Value"));
			if (double.IsNaN(this.Value))
			{
				this.IsVisible = false;
				return;
			}
			this.IsVisible = true;
		}

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06003BD0 RID: 15312 RVA: 0x00315FDC File Offset: 0x003141DC
		// (set) Token: 0x06003BD1 RID: 15313 RVA: 0x00315FE4 File Offset: 0x003141E4
		public string LegendTitleWithValue
		{
			[CompilerGenerated]
			get
			{
				return this.<LegendTitleWithValue>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<LegendTitleWithValue>k__BackingField = value;
			}
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06003BD2 RID: 15314 RVA: 0x00315FED File Offset: 0x003141ED
		public string AdaptedUnitsTitle
		{
			get
			{
				return UnitsHelper.GetCaption(this.DataRecord.Units);
			}
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06003BD3 RID: 15315 RVA: 0x00315FFF File Offset: 0x003141FF
		// (set) Token: 0x06003BD4 RID: 15316 RVA: 0x00316007 File Offset: 0x00314207
		public string TextValue
		{
			[CompilerGenerated]
			get
			{
				return this.<TextValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TextValue>k__BackingField = value;
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06003BD5 RID: 15317 RVA: 0x00316010 File Offset: 0x00314210
		// (set) Token: 0x06003BD6 RID: 15318 RVA: 0x00316018 File Offset: 0x00314218
		public double Value
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

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06003BD7 RID: 15319 RVA: 0x00316021 File Offset: 0x00314221
		// (set) Token: 0x06003BD8 RID: 15320 RVA: 0x00316029 File Offset: 0x00314229
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			private set
			{
				if (this._IsVisible != value)
				{
					this._IsVisible = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("IsVisible"));
				}
			}
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06003BD9 RID: 15321 RVA: 0x0031604B File Offset: 0x0031424B
		public string Name
		{
			get
			{
				return this.DataRecord.Name;
			}
		}

		// Token: 0x04002495 RID: 9365
		[CompilerGenerated]
		private DataRecord <DataRecord>k__BackingField;

		// Token: 0x04002496 RID: 9366
		private const double MAX_DIFFERENCE = 5.0;

		// Token: 0x04002497 RID: 9367
		[CompilerGenerated]
		private string <LegendTitleWithValue>k__BackingField;

		// Token: 0x04002498 RID: 9368
		[CompilerGenerated]
		private string <TextValue>k__BackingField;

		// Token: 0x04002499 RID: 9369
		[CompilerGenerated]
		private double <Value>k__BackingField;

		// Token: 0x0400249A RID: 9370
		private bool _IsVisible;

		// Token: 0x020006DD RID: 1757
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06003BDA RID: 15322 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06003BDB RID: 15323 RVA: 0x00316058 File Offset: 0x00314258
			internal bool <UpdateForLastTime>b__0(DataRecordElement x)
			{
				return x.Seconds <= this.lastTime;
			}

			// Token: 0x0400249B RID: 9371
			public double lastTime;
		}
	}
}
