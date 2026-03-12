using System;

namespace CarScannerXamarinForms
{
	// Token: 0x0200004E RID: 78
	public static class DoubleExtensions
	{
		// Token: 0x060001DA RID: 474 RVA: 0x0001684C File Offset: 0x00014A4C
		public static bool Equals3DigitPrecision(this double left, double right)
		{
			return Math.Abs(left - right) < 0.001;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00016861 File Offset: 0x00014A61
		public static bool Equals4DigitPrecision(this double left, double right)
		{
			return Math.Abs(left - right) < 0.0001;
		}

		// Token: 0x0400018C RID: 396
		private const double _3 = 0.001;

		// Token: 0x0400018D RID: 397
		private const double _4 = 0.0001;

		// Token: 0x0400018E RID: 398
		private const double _5 = 1E-05;

		// Token: 0x0400018F RID: 399
		private const double _6 = 1E-06;

		// Token: 0x04000190 RID: 400
		private const double _7 = 1E-07;
	}
}
