using System;
using System.Collections.Generic;
using CarScannerXamarinForms.DTC;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006EF RID: 1775
	public interface IDataRecordContainer
	{
		// Token: 0x170013E8 RID: 5096
		// (get) Token: 0x06003C67 RID: 15463
		// (set) Token: 0x06003C68 RID: 15464
		string VIN { get; set; }

		// Token: 0x170013E9 RID: 5097
		// (get) Token: 0x06003C69 RID: 15465
		// (set) Token: 0x06003C6A RID: 15466
		DateTime TimeStarted { get; set; }

		// Token: 0x170013EA RID: 5098
		// (get) Token: 0x06003C6B RID: 15467
		// (set) Token: 0x06003C6C RID: 15468
		string ConnectionProfile { get; set; }

		// Token: 0x170013EB RID: 5099
		// (get) Token: 0x06003C6D RID: 15469
		// (set) Token: 0x06003C6E RID: 15470
		string Device { get; set; }

		// Token: 0x170013EC RID: 5100
		// (get) Token: 0x06003C6F RID: 15471
		// (set) Token: 0x06003C70 RID: 15472
		DateTime TimeEnded { get; set; }

		// Token: 0x170013ED RID: 5101
		// (get) Token: 0x06003C71 RID: 15473
		// (set) Token: 0x06003C72 RID: 15474
		string CarName { get; set; }

		// Token: 0x170013EE RID: 5102
		// (get) Token: 0x06003C73 RID: 15475
		List<DataRecord> Records { get; }

		// Token: 0x170013EF RID: 5103
		// (get) Token: 0x06003C74 RID: 15476
		List<ProxyTest> SpeedTests { get; }

		// Token: 0x170013F0 RID: 5104
		// (get) Token: 0x06003C75 RID: 15477
		bool HasSpeedTests { get; }

		// Token: 0x170013F1 RID: 5105
		// (get) Token: 0x06003C76 RID: 15478
		string Filename { get; }

		// Token: 0x170013F2 RID: 5106
		// (get) Token: 0x06003C77 RID: 15479
		string Title { get; }

		// Token: 0x170013F3 RID: 5107
		// (get) Token: 0x06003C78 RID: 15480
		List<DTCItemV2> DTCs { get; }

		// Token: 0x170013F4 RID: 5108
		// (get) Token: 0x06003C79 RID: 15481
		bool HasDTC { get; }

		// Token: 0x170013F5 RID: 5109
		// (get) Token: 0x06003C7A RID: 15482
		bool HasGeolocationData { get; }
	}
}
