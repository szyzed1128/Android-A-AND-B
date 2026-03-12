using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using MoreLinq.Extensions;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000507 RID: 1287
	internal class Renault29bitCANECU : CAN29bitECU
	{
		// Token: 0x06003186 RID: 12678 RVA: 0x00222D5C File Offset: 0x00220F5C
		public Renault29bitCANECU(string name, string requestHeader, string responseHeader)
			: base(name, requestHeader, responseHeader)
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F18A", "Supplier code");
			base.IdentsASCII.TryAdd("22F194", "Soft version");
			base.IdentsASCII.TryAdd("22F195", "Version");
			base.IdentsASCII.TryAdd("22F1A0", "Diag. ver.");
			this.AddUDSIdents();
		}

		// Token: 0x06003187 RID: 12679 RVA: 0x00222E24 File Offset: 0x00221024
		public Renault29bitCANECU(string name, string requestHeader)
			: base(name, requestHeader.Substring(2), CAN29bitHelper.GetPossibleResponseHeader(requestHeader.Substring(2), SharedSettings.Current.BrandForDTC, null))
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F18A", "Supplier code");
			base.IdentsASCII.TryAdd("22F194", "Soft version");
			base.IdentsASCII.TryAdd("22F195", "Version");
			base.IdentsASCII.TryAdd("22F1A0", "Diag. ver.");
			this.AddUDSIdents();
		}

		// Token: 0x06003188 RID: 12680 RVA: 0x00222F08 File Offset: 0x00221108
		private static List<IECU> BuildList()
		{
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can29bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "17FF00", "1902AF", "1902AC" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF" }
			});
			list.Add(new Renault29bitCANECU(CAN11bitECU.ECU_ENGINE_NAME + " (29 bit)", "18DA15F1"));
			list.Add(new Renault29bitCANECU(CAN11bitECU.ECU_TRANSMISSION_NAME + " (29 bit)", "18DA18F1"));
			list.Add(new Renault29bitCANECU(CAN11bitECU.ECU_ABS_NAME + " (29 bit)", "18DA2DF1"));
			list.Add(new Renault29bitCANECU("HMD (29 bit)", "18DA60F1"));
			list.Add(new Renault29bitCANECU("Superviseur-DCDC  (29 bit)", "18DADDF1"));
			list.Add(new Renault29bitCANECU("ADB-L (29 bit)", "18DA72F2"));
			list.Add(new Renault29bitCANECU("ADB-R (29 bit)", "18DA73F2"));
			list.Add(new Renault29bitCANECU("ADB-LCU (29 bit)", "18DA71F2"));
			list.Add(new Renault29bitCANECU("BAT (29 bit)", "18DA5DF2"));
			list.Add(new Renault29bitCANECU("BLE VK (29 bit)", "18DAABF2"));
			list.Add(new Renault29bitCANECU("LIB 12V (29 bit)", "18DA0BF1"));
			list.Add(new Renault29bitCANECU("BMS Master (29 bit)", "18DA92F1"));
			list.Add(new Renault29bitCANECU("BSG 48v (29 bit)", "18DA55F1"));
			list.Add(new Renault29bitCANECU("SSG (29 bit)", "18DA14F2"));
			list.Add(new Renault29bitCANECU("CHADeMo (29 bit)", "18DAD7F2"));
			list.Add(new Renault29bitCANECU("GATEWAY (29 bit)", "18DAD0F1"));
			list.Add(new Renault29bitCANECU("S-GW3 (29 bit)", "18DAD2F1"));
			list.Add(new Renault29bitCANECU("S-GW4 (29 bit)", "18DAD4F1"));
			list.Add(new Renault29bitCANECU("BEV - SRVM (29 bit)", "18DA6CF2"));
			list.Add(new Renault29bitCANECU("SCCM (29 bit)", "18DAA2F2"));
			list.Add(new Renault29bitCANECU("E-ACT-EBA (29 bit)", "18DAE2F1"));
			list.Add(new Renault29bitCANECU("EVC-HCM-VCM (29 bit)", "18DADAF1"));
			list.Add(new Renault29bitCANECU("UDM (29 bit)", "18DAE1F1"));
			list.Add(new Renault29bitCANECU("HSG-LNG-SSG (29 bit)", "18DAE3F1"));
			list.Add(new Renault29bitCANECU("INV-ME (29 bit)", "18DADFF1"));
			list.Add(new Renault29bitCANECU("BCB-OBC (29 bit)", "18DADEF1"));
			list.Add(new Renault29bitCANECU("LBC2 (29 bit)", "18DADCF1"));
			list.Add(new Renault29bitCANECU("LBC (HEV) (29 bit)", "18DADBF1"));
			list.Add(new Renault29bitCANECU("HVAC 2 (29 bit)", "18DA98F1"));
			list.Add(new Renault29bitCANECU("INV-RE1 (29 bit)", "18DAE5F1"));
			list.Add(new Renault29bitCANECU("EVAP (29 bit)", "18DAE0F1"));
			list.Add(new Renault29bitCANECU("GATEWAY (29 bit)", "18DAD2F1"));
			list.Add(new Renault29bitCANECU("Signal Converter Unit (29 bit)", "18DAD6F2"));
			list.Add(new Renault29bitCANECU("DMC (29 bit)", "18DA69F2"));
			list.Add(new Renault29bitCANECU("eCALL (29 bit)", "18DAE5F1"));
			list.Add(new Renault29bitCANECU("SOW Front Left (29 bit)", "18DA23F2"));
			list.Add(new Renault29bitCANECU("SOW Front Right (29 bit)", "18DA24F2"));
			list.Add(new Renault29bitCANECU("SCCM (29 bit)", "18DA28F3"));
			list.Add(new Renault29bitCANECU("Controlographe (29 bit)", "18DAEEF2"));
			return DistinctByExtension.DistinctBy<IECU, string>(list, (IECU x) => x.RequestHeader).ToList<IECU>();
		}

		// Token: 0x170012EA RID: 4842
		// (get) Token: 0x06003189 RID: 12681 RVA: 0x00223346 File Offset: 0x00221546
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return Renault29bitCANECU.BuildList();
			}
		}

		// Token: 0x02000508 RID: 1288
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600318A RID: 12682 RVA: 0x0022334D File Offset: 0x0022154D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600318B RID: 12683 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600318C RID: 12684 RVA: 0x00223359 File Offset: 0x00221559
			internal string <BuildList>b__2_0(IECU x)
			{
				return x.RequestHeader;
			}

			// Token: 0x04001C9A RID: 7322
			public static readonly Renault29bitCANECU.<>c <>9 = new Renault29bitCANECU.<>c();

			// Token: 0x04001C9B RID: 7323
			public static Func<IECU, string> <>9__2_0;
		}
	}
}
