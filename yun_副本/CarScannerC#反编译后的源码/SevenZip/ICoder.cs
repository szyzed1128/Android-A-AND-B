using System;
using System.IO;

namespace SevenZip
{
	// Token: 0x0200001C RID: 28
	public interface ICoder
	{
		// Token: 0x060000C0 RID: 192
		void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress);
	}
}
