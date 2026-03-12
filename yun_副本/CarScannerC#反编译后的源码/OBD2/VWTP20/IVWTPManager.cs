using System;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003A2 RID: 930
	internal interface IVWTPManager
	{
		// Token: 0x06002728 RID: 10024
		ValueTask<string> SendCommand(string cmd);

		// Token: 0x06002729 RID: 10025
		void Clear();

		// Token: 0x0600272A RID: 10026
		int GetFreeCRAChannel();
	}
}
