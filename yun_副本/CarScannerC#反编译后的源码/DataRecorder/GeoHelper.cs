using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006EB RID: 1771
	internal static class GeoHelper
	{
		// Token: 0x06003C4A RID: 15434 RVA: 0x003189E1 File Offset: 0x00316BE1
		public static Point ToPoint(this Position pos)
		{
			return new Point(pos.Latitude, pos.Longitude);
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x003189F6 File Offset: 0x00316BF6
		public static Position ToPosition(this Point point)
		{
			return new Position(point.X, point.Y);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x00318A0C File Offset: 0x00316C0C
		public static double GetDistanceMeters(Position pos1, Position pos2)
		{
			return Distance.BetweenPositions(pos1, pos2).Meters;
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x00318A28 File Offset: 0x00316C28
		public static double GetDistanceToPoint(this Point p1, Point p2)
		{
			return GeoHelper.GetDistanceMeters(p1.ToPosition(), p2.ToPosition());
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x00318A3B File Offset: 0x00316C3B
		public static double GetDistanceToPoint(this PointWithTime p1, PointWithTime p2)
		{
			return GeoHelper.GetDistanceMeters(p1.ToPosition(), p2.ToPosition());
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x00318A50 File Offset: 0x00316C50
		public static double GetDistanceToPoint(this PointWithTime p1, Point p2)
		{
			return GeoHelper.GetDistanceMeters(p1.ToPosition(), p2.ToPosition());
		}

		// Token: 0x06003C50 RID: 15440 RVA: 0x00318A64 File Offset: 0x00316C64
		public static double GetDistanceToPoint(this Point p1, PointWithTime p2)
		{
			return GeoHelper.GetDistanceMeters(p1.ToPosition(), p2.ToPosition());
		}

		// Token: 0x06003C51 RID: 15441 RVA: 0x00318A78 File Offset: 0x00316C78
		public static Position ToPosition(this PointWithTime point)
		{
			return new Position(point.X, point.Y);
		}
	}
}
