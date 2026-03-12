using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FE RID: 1022
	public interface IPIDWithStringValue : IPID, INotifyPropertyChanged
	{
		// Token: 0x170011EB RID: 4587
		// (get) Token: 0x06002909 RID: 10505
		string Value { get; }

		// Token: 0x170011EC RID: 4588
		// (get) Token: 0x0600290A RID: 10506
		int IntValue { get; }
	}
}
