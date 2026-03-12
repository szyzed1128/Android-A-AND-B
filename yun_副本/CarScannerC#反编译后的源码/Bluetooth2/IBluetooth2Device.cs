using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Bluetooth2
{
	// Token: 0x02000BF6 RID: 3062
	public interface IBluetooth2Device : INotifyPropertyChanged
	{
		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x06005BFE RID: 23550
		string Name { get; }

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x06005BFF RID: 23551
		string Id { get; }

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x06005C00 RID: 23552
		bool Paired { get; }

		// Token: 0x06005C01 RID: 23553
		Task Pair();

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x06005C02 RID: 23554
		bool IsValid { get; }
	}
}
