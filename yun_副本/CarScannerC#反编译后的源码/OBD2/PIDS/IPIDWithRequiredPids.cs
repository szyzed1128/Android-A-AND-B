using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FD RID: 1021
	internal interface IPIDWithRequiredPids
	{
		// Token: 0x170011EA RID: 4586
		// (get) Token: 0x06002908 RID: 10504
		[JsonIgnore]
		IReadOnlyList<IPID> RequiredPIDs { get; }
	}
}
