using System;
using System.Collections.Generic;
using System.Linq;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Bluetooth2
{
	// Token: 0x02000BFB RID: 3067
	public static class WrongDeviceChecker
	{
		// Token: 0x06005C1C RID: 23580 RVA: 0x0043B704 File Offset: 0x00439904
		public static bool IsWrongDevice(string name)
		{
			if (name == null)
			{
				return false;
			}
			foreach (string text in WrongDeviceChecker.valid_names)
			{
				if (name.IndexOf(text, 0, StringComparison.InvariantCultureIgnoreCase) >= 0)
				{
					return false;
				}
			}
			if (new string[] { "VESTA", "SOLARIS", "MY-CAR", "SYNC", "MYCAR", "CAR BT", "CAR-KIT" }.Contains(name))
			{
				return true;
			}
			List<string> list = PackageFileReader.DeserilzeFromEmbeddedFile<List<string>>("brands.db");
			list.AddRange(new string[] { "KIA", "HYUNDAI" });
			try
			{
				list.Remove("WEI");
			}
			catch (Exception)
			{
			}
			foreach (string text2 in list)
			{
				if (name.Contains(text2, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x0043B818 File Offset: 0x00439A18
		// Note: this type is marked as 'beforefieldinit'.
		static WrongDeviceChecker()
		{
		}

		// Token: 0x04003A09 RID: 14857
		public static string[] valid_names = new string[]
		{
			"OBD", "obd", "obd2", "OBDI", "OBDII", "V-LINK", "obd-2", "Micro Mechanic", "Viecar", "KONNWEI",
			"Scan Tool Pro", "Android-Vlink", "TOPONE", "OBDII", "OBD II", "CHX", "VEEPEAK", "ANCEL", "Steren SCAN-010", "Vgate",
			"CBT.", "Carly Adapter", "ROADGID", "Carista", "KONNWEI OBDII", "NX3014", "OBDclick", "CarScanX", "CBT", "OBD2ECU",
			"OBDII", "OBDLink", "obd-ii", "Multilaser OBD", "OBDBOUTIK OBDII", "KONNWEIOBD", "Tacklife-OBD2-BT", "OBD-AUS", "CAN OBDII", "FIXD",
			"Elm327", "OBDII iOS", "NY-OM327 OBDII", "iCarsoft", "UniCarScan", "\u0002-LINK", "TECAR", "NexzScan", "Rokodil", "Scan Tool Pro",
			"Vlink", "BDII", "ANCEL", "CBT."
		};

		// Token: 0x04003A0A RID: 14858
		public static string[] BAD_ELM327_NAMES = new string[] { "Scan Tool Pro", "Micro Mechanic", "THINMI.COM" };
	}
}
