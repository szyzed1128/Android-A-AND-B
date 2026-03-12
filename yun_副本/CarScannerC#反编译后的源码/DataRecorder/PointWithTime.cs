using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x02000707 RID: 1799
	public struct PointWithTime
	{
		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x06003D21 RID: 15649 RVA: 0x00326BE0 File Offset: 0x00324DE0
		// (set) Token: 0x06003D22 RID: 15650 RVA: 0x00326BE8 File Offset: 0x00324DE8
		public double X
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<X>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<X>k__BackingField = value;
			}
		}

		// Token: 0x17001401 RID: 5121
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00326BF1 File Offset: 0x00324DF1
		// (set) Token: 0x06003D24 RID: 15652 RVA: 0x00326BF9 File Offset: 0x00324DF9
		public double Y
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<Y>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Y>k__BackingField = value;
			}
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00326C02 File Offset: 0x00324E02
		// (set) Token: 0x06003D26 RID: 15654 RVA: 0x00326C0A File Offset: 0x00324E0A
		public double TimeSeconds
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<TimeSeconds>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeSeconds>k__BackingField = value;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06003D27 RID: 15655 RVA: 0x00326C13 File Offset: 0x00324E13
		// (set) Token: 0x06003D28 RID: 15656 RVA: 0x00326C1B File Offset: 0x00324E1B
		public int Index
		{
			[CompilerGenerated]
			readonly get
			{
				return this.<Index>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Index>k__BackingField = value;
			}
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x00326C24 File Offset: 0x00324E24
		public Point ToPoint()
		{
			return new Point(this.X, this.Y);
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x00326C37 File Offset: 0x00324E37
		public Position ToPosition()
		{
			return new Position(this.X, this.Y);
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x00326C4A File Offset: 0x00324E4A
		public PointWithTime(double x, double y, double time)
		{
			this.X = x;
			this.Y = y;
			this.TimeSeconds = time;
			this.Index = -1;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x00326C68 File Offset: 0x00324E68
		public PointWithTime(Point p, double time)
		{
			this.X = p.X;
			this.Y = p.Y;
			this.TimeSeconds = time;
			this.Index = -1;
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x06003D2D RID: 15661 RVA: 0x00326C92 File Offset: 0x00324E92
		public bool IsEmpty
		{
			get
			{
				return this.X == 0.0 && this.Y == 0.0 && this.TimeSeconds == 0.0;
			}
		}

		// Token: 0x17001405 RID: 5125
		// (get) Token: 0x06003D2E RID: 15662 RVA: 0x00326CCA File Offset: 0x00324ECA
		public static PointWithTime Zero
		{
			get
			{
				return new PointWithTime(0.0, 0.0, 0.0);
			}
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00326CEC File Offset: 0x00324EEC
		public bool EqualsToPoint(Point p)
		{
			return this.X == p.X && this.Y == p.Y;
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00326D10 File Offset: 0x00324F10
		public override bool Equals(object obj)
		{
			if (obj is PointWithTime)
			{
				PointWithTime pointWithTime = (PointWithTime)obj;
				return this.Equals(pointWithTime);
			}
			return false;
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00326D35 File Offset: 0x00324F35
		public bool Equals(PointWithTime p)
		{
			return this.X == p.X && this.Y == p.Y && this.TimeSeconds == p.TimeSeconds;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00326D66 File Offset: 0x00324F66
		public static bool operator ==(PointWithTime lhs, PointWithTime rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00326D70 File Offset: 0x00324F70
		public static bool operator !=(PointWithTime lhs, PointWithTime rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00326D7C File Offset: 0x00324F7C
		public override int GetHashCode()
		{
			return new ValueTuple<double, double, double>(this.X, this.Y, this.TimeSeconds).GetHashCode();
		}

		// Token: 0x04002583 RID: 9603
		[CompilerGenerated]
		private double <X>k__BackingField;

		// Token: 0x04002584 RID: 9604
		[CompilerGenerated]
		private double <Y>k__BackingField;

		// Token: 0x04002585 RID: 9605
		[CompilerGenerated]
		private double <TimeSeconds>k__BackingField;

		// Token: 0x04002586 RID: 9606
		[CompilerGenerated]
		private int <Index>k__BackingField;
	}
}
