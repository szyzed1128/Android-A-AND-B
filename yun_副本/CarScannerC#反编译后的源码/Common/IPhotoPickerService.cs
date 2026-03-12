using System;
using System.IO;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E5 RID: 2021
	public interface IPhotoPickerService
	{
		// Token: 0x060046FC RID: 18172
		Task<Stream> GetImageStreamAsync();
	}
}
