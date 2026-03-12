using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB
{
	// Token: 0x020009CA RID: 2506
	internal class VagUnitDetector
	{
		// Token: 0x0600511F RID: 20767 RVA: 0x003EE7CC File Offset: 0x003EC9CC
		public async Task Detect()
		{
			List<VagUnit> list = new string[]
			{
				"01", "03", "05", "08", "09", "10", "13", "17", "18", "19",
				"22", "36", "42", "44", "47", "52", "5F", "65", "6C", "6D",
				"75", "A5"
			}.Select((string x) => new VagUnit
			{
				UnitID = x,
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(x),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(x)
			}).ToList<VagUnit>();
			new List<OBDRequest>();
			foreach (VagUnit vagUnit in list)
			{
			}
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x00002050 File Offset: 0x00000250
		public VagUnitDetector()
		{
		}

		// Token: 0x020009CB RID: 2507
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005121 RID: 20769 RVA: 0x003EE807 File Offset: 0x003ECA07
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005122 RID: 20770 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005123 RID: 20771 RVA: 0x003EE813 File Offset: 0x003ECA13
			internal VagUnit <Detect>b__0_0(string x)
			{
				return new VagUnit
				{
					UnitID = x,
					RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(x),
					ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(x)
				};
			}

			// Token: 0x04003120 RID: 12576
			public static readonly VagUnitDetector.<>c <>9 = new VagUnitDetector.<>c();

			// Token: 0x04003121 RID: 12577
			public static Func<string, VagUnit> <>9__0_0;
		}

		// Token: 0x020009CC RID: 2508
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Detect>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005124 RID: 20772 RVA: 0x003EE83C File Offset: 0x003ECA3C
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				try
				{
					List<VagUnit> list = new string[]
					{
						"01", "03", "05", "08", "09", "10", "13", "17", "18", "19",
						"22", "36", "42", "44", "47", "52", "5F", "65", "6C", "6D",
						"75", "A5"
					}.Select((string x) => new VagUnit
					{
						UnitID = x,
						RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(x),
						ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(x)
					}).ToList<VagUnit>();
					new List<OBDRequest>();
					List<VagUnit>.Enumerator enumerator = list.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							VagUnit vagUnit = enumerator.Current;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005125 RID: 20773 RVA: 0x003EE9D0 File Offset: 0x003ECBD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003122 RID: 12578
			public int <>1__state;

			// Token: 0x04003123 RID: 12579
			public AsyncTaskMethodBuilder <>t__builder;
		}
	}
}
