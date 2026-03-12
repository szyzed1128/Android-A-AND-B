using System;
using System.IO;

namespace SevenZip.Compression.LZ
{
	// Token: 0x02000036 RID: 54
	internal interface IInWindowStream
	{
		// Token: 0x06000156 RID: 342
		void SetStream(Stream inStream);

		// Token: 0x06000157 RID: 343
		void Init();

		// Token: 0x06000158 RID: 344
		void ReleaseStream();

		// Token: 0x06000159 RID: 345
		byte GetIndexByte(int index);

		// Token: 0x0600015A RID: 346
		uint GetMatchLen(int index, uint distance, uint limit);

		// Token: 0x0600015B RID: 347
		uint GetNumAvailableBytes();
	}
}
