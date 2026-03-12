using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarScannerXamarinForms.ECUModels;

namespace CarScannerXamarinForms.DTCv2
{
	// Token: 0x020005B1 RID: 1457
	internal interface ISupportedUnitsDetector
	{
		// Token: 0x060034D1 RID: 13521
		Task<List<IECU>> DetectSupported(IEnumerable<IECU> allUnits);
	}
}
