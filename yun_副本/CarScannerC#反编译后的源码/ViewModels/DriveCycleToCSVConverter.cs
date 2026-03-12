using System;
using System.Text;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000717 RID: 1815
	public static class DriveCycleToCSVConverter
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x00328F38 File Offset: 0x00327138
		public static string ConvertDriveCycleToCSV(DriveCycle dc)
		{
			StringBuilder stringBuilder = new StringBuilder(16);
			stringBuilder.Append(dc.TimeStarted.ToString("DD.MM.YYYY"));
			stringBuilder.Append(";");
			stringBuilder.Append(dc.TimeStarted.ToString("HH:mm:ss"));
			stringBuilder.Append(";");
			stringBuilder.Append(dc.TimeFinished.ToString("DD.MM.YYYY"));
			stringBuilder.Append(";");
			stringBuilder.Append(dc.TimeFinished.ToString("HH:mm:ss"));
			stringBuilder.Append(";");
			stringBuilder.Append(UnitsHelper.GetValue(dc.Distance, UnitsHelper.Units.km).ToString("#.##"));
			stringBuilder.Append(";");
			stringBuilder.Append(UnitsHelper.GetValue(dc.Distance, UnitsHelper.Units.km).ToString("#.##"));
			stringBuilder.Append(";");
			stringBuilder.Append(dc.FuelPricePerL.ToString("#.##"));
			stringBuilder.Append(";");
			stringBuilder.Append(dc.TotalFuelPrice.ToString("#.##"));
			stringBuilder.Append(";");
			stringBuilder.Append(UnitsHelper.GetValue(dc.AvgFuelConsumption, UnitsHelper.Units.liters100km).ToString("#.##"));
			stringBuilder.Append(";");
			stringBuilder.Append(UnitsHelper.GetValue(dc.AvgSpeed, UnitsHelper.Units.kmh).ToString("#.##"));
			stringBuilder.Append(";");
			return stringBuilder.ToString();
		}
	}
}
