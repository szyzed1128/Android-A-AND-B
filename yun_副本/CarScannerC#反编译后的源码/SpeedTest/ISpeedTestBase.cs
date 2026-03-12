using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.SpeedTest
{
	// Token: 0x020001EF RID: 495
	public interface ISpeedTestBase : INotifyPropertyChanged
	{
		// Token: 0x060019F6 RID: 6646
		void Start();

		// Token: 0x060019F7 RID: 6647
		void Cancel();

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060019F8 RID: 6648
		// (remove) Token: 0x060019F9 RID: 6649
		event EventHandler TestCompleted;

		// Token: 0x17000F91 RID: 3985
		// (get) Token: 0x060019FA RID: 6650
		// (set) Token: 0x060019FB RID: 6651
		string Value { get; set; }

		// Token: 0x17000F92 RID: 3986
		// (get) Token: 0x060019FC RID: 6652
		// (set) Token: 0x060019FD RID: 6653
		string Name { get; set; }

		// Token: 0x17000F93 RID: 3987
		// (get) Token: 0x060019FE RID: 6654
		bool IsTestFinished { get; }

		// Token: 0x060019FF RID: 6655
		void RefreshSpeedPID();
	}
}
