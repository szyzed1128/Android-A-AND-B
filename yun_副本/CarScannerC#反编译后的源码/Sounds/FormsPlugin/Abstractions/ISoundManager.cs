using System;
using System.Threading.Tasks;

namespace Sounds.FormsPlugin.Abstractions
{
	// Token: 0x02000041 RID: 65
	public interface ISoundManager
	{
		// Token: 0x06000191 RID: 401
		Task Play();

		// Token: 0x06000192 RID: 402
		Task Stop();

		// Token: 0x06000193 RID: 403
		Task Pause();

		// Token: 0x06000194 RID: 404
		Task Volume(double volume);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000195 RID: 405
		// (set) Token: 0x06000196 RID: 406
		string Filename { get; set; }

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000197 RID: 407
		// (remove) Token: 0x06000198 RID: 408
		event EventHandler FinishedPlaying;

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000199 RID: 409
		bool IsPlaying { get; }
	}
}
