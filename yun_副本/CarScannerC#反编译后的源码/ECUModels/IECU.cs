using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004E1 RID: 1249
	public interface IECU : IEnumerable<DTCItemV2>, IEnumerable, INotifyPropertyChanged
	{
		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x060030B6 RID: 12470
		ELMFormat ELMFormat { get; }

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x060030B7 RID: 12471
		int Protocol { get; }

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x060030B8 RID: 12472
		// (set) Token: 0x060030B9 RID: 12473
		string Name { get; set; }

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x060030BA RID: 12474
		string ShortName { get; }

		// Token: 0x170012AC RID: 4780
		// (get) Token: 0x060030BB RID: 12475
		string RequestHeader { get; }

		// Token: 0x170012AD RID: 4781
		// (get) Token: 0x060030BC RID: 12476
		string ResponseHeader { get; }

		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x060030BD RID: 12477
		string ExtendedAddress { get; }

		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x060030BE RID: 12478
		string TesterAddress { get; }

		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x060030BF RID: 12479
		List<string> ReadDTCCommands { get; }

		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x060030C0 RID: 12480
		List<string> ClearDTCCommands { get; }

		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x060030C1 RID: 12481
		List<string> OpenSessionCommands { get; }

		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x060030C2 RID: 12482
		List<string> CloseSessionCommands { get; }

		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x060030C3 RID: 12483
		// (set) Token: 0x060030C4 RID: 12484
		bool Highlighted { get; set; }

		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x060030C5 RID: 12485
		bool ECUExists { get; }

		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x060030C6 RID: 12486
		// (set) Token: 0x060030C7 RID: 12487
		bool Expanded { get; set; }

		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x060030C8 RID: 12488
		string ExpandedStateImage { get; }

		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x060030C9 RID: 12489
		// (set) Token: 0x060030CA RID: 12490
		bool IsSelected { get; set; }

		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x060030CB RID: 12491
		List<DTCItemV2> DTCCollection { get; }

		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x060030CC RID: 12492
		bool RemoveOtherRequestsIfUDSReadResponded { get; }

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x060030CD RID: 12493
		// (set) Token: 0x060030CE RID: 12494
		bool TestELMDevice { get; set; }

		// Token: 0x060030CF RID: 12495
		void Reset();

		// Token: 0x060030D0 RID: 12496
		void UpdateCollection();

		// Token: 0x170012BC RID: 4796
		// (get) Token: 0x060030D1 RID: 12497
		bool CycleThroughOpenSessionCommands { get; }

		// Token: 0x060030D2 RID: 12498
		Task ReadDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken cancellationToken);

		// Token: 0x060030D3 RID: 12499
		Task ClearDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken cancellationToken);

		// Token: 0x060030D4 RID: 12500
		Task<string> GetECUInformationReportAsync(CancellationToken cancellationToken);

		// Token: 0x060030D5 RID: 12501
		OBDRequest GetRequestForCommand(string cmd);
	}
}
