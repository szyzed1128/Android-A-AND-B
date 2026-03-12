using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000880 RID: 2176
	internal interface ICodingContainerWithStringAddress : ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x170016B0 RID: 5808
		// (get) Token: 0x06004A26 RID: 18982
		// (set) Token: 0x06004A27 RID: 18983
		string Address { get; set; }
	}
}
