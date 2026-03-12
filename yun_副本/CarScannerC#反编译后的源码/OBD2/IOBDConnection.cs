using System;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000321 RID: 801
	public interface IOBDConnection
	{
		// Token: 0x1700115C RID: 4444
		// (get) Token: 0x0600247D RID: 9341
		// (set) Token: 0x0600247E RID: 9342
		bool KeepAlive { get; set; }

		// Token: 0x1700115D RID: 4445
		// (get) Token: 0x0600247F RID: 9343
		// (set) Token: 0x06002480 RID: 9344
		bool NoDelay { get; set; }

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x06002481 RID: 9345
		bool Connected { get; }

		// Token: 0x06002482 RID: 9346
		ValueTask<byte[]> ReadBytesAsync();

		// Token: 0x06002483 RID: 9347
		Task WriteBytesAsync(byte[] data);

		// Token: 0x06002484 RID: 9348
		Task FlushAsync();

		// Token: 0x06002485 RID: 9349
		Task<bool> ConnectAsync(string host_port, PCLDebugStream debugStream);

		// Token: 0x06002486 RID: 9350
		void Disconect();

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06002487 RID: 9351
		bool NeedFlush { get; }
	}
}
