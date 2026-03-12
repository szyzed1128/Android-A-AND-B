using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006E6 RID: 1766
	internal class DataRecordForMapsWithGeoposition : List<DataRecordElement>
	{
		// Token: 0x170013DB RID: 5083
		// (get) Token: 0x06003C2D RID: 15405 RVA: 0x00318706 File Offset: 0x00316906
		// (set) Token: 0x06003C2E RID: 15406 RVA: 0x0031870E File Offset: 0x0031690E
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x170013DC RID: 5084
		// (get) Token: 0x06003C2F RID: 15407 RVA: 0x00318717 File Offset: 0x00316917
		// (set) Token: 0x06003C30 RID: 15408 RVA: 0x0031871F File Offset: 0x0031691F
		public Point Position
		{
			[CompilerGenerated]
			get
			{
				return this.<Position>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Position>k__BackingField = value;
			}
		}

		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06003C31 RID: 15409 RVA: 0x00318728 File Offset: 0x00316928
		// (set) Token: 0x06003C32 RID: 15410 RVA: 0x00318730 File Offset: 0x00316930
		public double MinValue
		{
			[CompilerGenerated]
			get
			{
				return this.<MinValue>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MinValue>k__BackingField = value;
			}
		}

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06003C33 RID: 15411 RVA: 0x00318739 File Offset: 0x00316939
		// (set) Token: 0x06003C34 RID: 15412 RVA: 0x00318741 File Offset: 0x00316941
		public double MaxValue
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxValue>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MaxValue>k__BackingField = value;
			}
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06003C35 RID: 15413 RVA: 0x0031874A File Offset: 0x0031694A
		// (set) Token: 0x06003C36 RID: 15414 RVA: 0x00318752 File Offset: 0x00316952
		public double AvgValue
		{
			[CompilerGenerated]
			get
			{
				return this.<AvgValue>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AvgValue>k__BackingField = value;
			}
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x0031875C File Offset: 0x0031695C
		public void UpdateValue()
		{
			if (base.Count == 1)
			{
				this.MinValue = base[0].Value;
				this.MaxValue = this.MinValue;
				this.AvgValue = this.MinValue;
			}
			this.MinValue = this.Min((DataRecordElement x) => x.Value);
			this.MaxValue = this.Max((DataRecordElement x) => x.Value);
			double num = 0.0;
			int num2 = 0;
			foreach (DataRecordElement dataRecordElement in this)
			{
				if (double.IsFinite(dataRecordElement.Value))
				{
					num += dataRecordElement.Value;
					num2++;
				}
			}
			this.AvgValue = num / (double)num2;
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0031885C File Offset: 0x00316A5C
		public DataRecordForMapsWithGeoposition()
		{
		}

		// Token: 0x040024D8 RID: 9432
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x040024D9 RID: 9433
		[CompilerGenerated]
		private Point <Position>k__BackingField;

		// Token: 0x040024DA RID: 9434
		[CompilerGenerated]
		private double <MinValue>k__BackingField;

		// Token: 0x040024DB RID: 9435
		[CompilerGenerated]
		private double <MaxValue>k__BackingField;

		// Token: 0x040024DC RID: 9436
		[CompilerGenerated]
		private double <AvgValue>k__BackingField;

		// Token: 0x020006E7 RID: 1767
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003C39 RID: 15417 RVA: 0x00318864 File Offset: 0x00316A64
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003C3A RID: 15418 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003C3B RID: 15419 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <UpdateValue>b__20_0(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x06003C3C RID: 15420 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <UpdateValue>b__20_1(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x040024DD RID: 9437
			public static readonly DataRecordForMapsWithGeoposition.<>c <>9 = new DataRecordForMapsWithGeoposition.<>c();

			// Token: 0x040024DE RID: 9438
			public static Func<DataRecordElement, double> <>9__20_0;

			// Token: 0x040024DF RID: 9439
			public static Func<DataRecordElement, double> <>9__20_1;
		}
	}
}
