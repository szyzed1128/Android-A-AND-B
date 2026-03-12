using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007EA RID: 2026
	public abstract class JsonColorConverterBase : JsonConverter
	{
		// Token: 0x06004719 RID: 18201 RVA: 0x0036CF54 File Offset: 0x0036B154
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Color);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0036CF6B File Offset: 0x0036B16B
		protected JsonColorConverterBase()
		{
		}
	}
}
