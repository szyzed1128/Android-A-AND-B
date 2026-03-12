using System;
using System.IO;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E0 RID: 2016
	public interface IJson
	{
		// Token: 0x060046EA RID: 18154
		string SerializeObject(object obj);

		// Token: 0x060046EB RID: 18155
		T DeserializeObject<T>(string value);

		// Token: 0x060046EC RID: 18156
		void SerializeToStream(Stream stream, object obj);

		// Token: 0x060046ED RID: 18157
		T DeserializeFromStream<T>(Stream stream);
	}
}
