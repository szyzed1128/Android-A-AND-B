using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.ECUModels;

namespace CarScannerXamarinForms.DTCv2
{
	// Token: 0x0200058E RID: 1422
	internal sealed class BaseSupportedUnitsDetector : ISupportedUnitsDetector
	{
		// Token: 0x060033EB RID: 13291 RVA: 0x0024448C File Offset: 0x0024268C
		public async Task<List<IECU>> DetectSupported(IEnumerable<IECU> allUnits)
		{
			return new List<IECU>(allUnits);
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x00002050 File Offset: 0x00000250
		public BaseSupportedUnitsDetector()
		{
		}

		// Token: 0x0200058F RID: 1423
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DetectSupported>d__0 : IAsyncStateMachine
		{
			// Token: 0x060033ED RID: 13293 RVA: 0x002444D0 File Offset: 0x002426D0
			void IAsyncStateMachine.MoveNext()
			{
				List<IECU> list;
				try
				{
					list = new List<IECU>(allUnits);
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(list);
			}

			// Token: 0x060033EE RID: 13294 RVA: 0x00244528 File Offset: 0x00242728
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001E8E RID: 7822
			public int <>1__state;

			// Token: 0x04001E8F RID: 7823
			public AsyncTaskMethodBuilder<List<IECU>> <>t__builder;

			// Token: 0x04001E90 RID: 7824
			public IEnumerable<IECU> allUnits;
		}
	}
}
