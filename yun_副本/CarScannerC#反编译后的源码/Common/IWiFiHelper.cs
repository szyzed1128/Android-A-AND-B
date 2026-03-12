using System;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E8 RID: 2024
	public interface IWiFiHelper
	{
		// Token: 0x06004711 RID: 18193
		string GetWiFiName();

		// Token: 0x06004712 RID: 18194
		Task<bool> ConnectToNetwork(string SSID);

		// Token: 0x06004713 RID: 18195
		bool HasLocationPermission();
	}
}
