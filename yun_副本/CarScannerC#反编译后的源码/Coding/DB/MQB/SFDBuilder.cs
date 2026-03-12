using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ECUModels;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B55 RID: 2901
	internal static class SFDBuilder
	{
		// Token: 0x060059A9 RID: 22953 RVA: 0x0042C104 File Offset: 0x0042A304
		public static IEnumerable<ICodingContainer> BuildSFD()
		{
			List<IECU> list = VAGECU.ECUs.Where((IECU x) => !string.IsNullOrEmpty(x.RequestHeader) && (x is VAGECU || x is VAG29bitECU)).ToList<IECU>();
			List<CustomizableCodingTemplate> list2 = new List<CustomizableCodingTemplate>(list.Count * 3);
			foreach (IECU iecu in list)
			{
				try
				{
					iecu.Protocol.ToString();
					string text = iecu.RequestHeader;
					VAG29bitECU vag29bitECU = iecu as VAG29bitECU;
					if (vag29bitECU != null)
					{
						text = vag29bitECU.CANPriority + vag29bitECU.RequestHeader;
					}
					string name = iecu.Name;
					SFDActivationRequestReadVar2 sfdactivationRequestReadVar = new SFDActivationRequestReadVar2(name + ": Get activation request", text, iecu.ResponseHeader, iecu.Protocol.ToString());
					SFDActivationWriteToken sfdactivationWriteToken = new SFDActivationWriteToken(name + ": Write access token", text, iecu.ResponseHeader, iecu.Protocol.ToString());
					SFDActivationWriteToken sfdactivationWriteToken2 = new SFDActivationWriteToken(name + ": Lock unit", text, iecu.ResponseHeader, iecu.Protocol.ToString());
					list2.Add(sfdactivationRequestReadVar);
					list2.Add(sfdactivationWriteToken);
					list2.Add(sfdactivationWriteToken2);
				}
				catch (Exception)
				{
				}
			}
			return list2;
		}

		// Token: 0x02000B56 RID: 2902
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060059AA RID: 22954 RVA: 0x0042C26C File Offset: 0x0042A46C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060059AB RID: 22955 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060059AC RID: 22956 RVA: 0x0042C278 File Offset: 0x0042A478
			internal bool <BuildSFD>b__0_0(IECU x)
			{
				return !string.IsNullOrEmpty(x.RequestHeader) && (x is VAGECU || x is VAG29bitECU);
			}

			// Token: 0x04003817 RID: 14359
			public static readonly SFDBuilder.<>c <>9 = new SFDBuilder.<>c();

			// Token: 0x04003818 RID: 14360
			public static Func<IECU, bool> <>9__0_0;
		}
	}
}
