using System;

namespace SevenZip.Compression.LZ
{
	// Token: 0x02000037 RID: 55
	internal interface IMatchFinder : IInWindowStream
	{
		// Token: 0x0600015C RID: 348
		void Create(uint historySize, uint keepAddBufferBefore, uint matchMaxLen, uint keepAddBufferAfter);

		// Token: 0x0600015D RID: 349
		uint GetMatches(uint[] distances);

		// Token: 0x0600015E RID: 350
		void Skip(uint num);
	}
}
