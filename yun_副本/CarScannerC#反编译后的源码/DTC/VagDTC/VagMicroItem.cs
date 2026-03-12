using System;
using System.IO;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.DTC.VagDTC
{
	// Token: 0x0200058A RID: 1418
	internal struct VagMicroItem
	{
		// Token: 0x060033D6 RID: 13270 RVA: 0x00243F08 File Offset: 0x00242108
		[JsonConstructor]
		public VagMicroItem(int VagCode, int U, int RU, int DE, int EN, int S)
		{
			this.VagCode = VagCode;
			this.U = U;
			this.RU = RU;
			this.DE = DE;
			this.EN = EN;
			this.S = S;
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x00243F38 File Offset: 0x00242138
		public static VagMicroItem ReadFromStream(Stream stream)
		{
			byte[] array = new byte[18];
			stream.Read(array, 0, array.Length);
			return new VagMicroItem(VagMicroItem.IntFrom3Byte(array[0], array[1], array[2]), VagMicroItem.IntFrom3Byte(array[3], array[4], array[5]), VagMicroItem.IntFrom3Byte(array[6], array[7], array[8]), VagMicroItem.IntFrom3Byte(array[9], array[10], array[11]), VagMicroItem.IntFrom3Byte(array[12], array[13], array[14]), VagMicroItem.IntFrom3Byte(array[15], array[16], array[17]));
		}

		// Token: 0x060033D8 RID: 13272 RVA: 0x00243FBB File Offset: 0x002421BB
		public static int IntFrom3Byte(byte[] bytes)
		{
			return (int)bytes[2] * 65536 + (int)bytes[1] * 256 + (int)bytes[0];
		}

		// Token: 0x060033D9 RID: 13273 RVA: 0x00243FD4 File Offset: 0x002421D4
		internal static int IntFrom3Byte(byte b0, byte b1, byte b2)
		{
			return (int)b2 * 65536 + (int)b1 * 256 + (int)b0;
		}

		// Token: 0x04001E83 RID: 7811
		[JsonProperty("VC")]
		public int VagCode;

		// Token: 0x04001E84 RID: 7812
		public int U;

		// Token: 0x04001E85 RID: 7813
		public int RU;

		// Token: 0x04001E86 RID: 7814
		public int DE;

		// Token: 0x04001E87 RID: 7815
		public int EN;

		// Token: 0x04001E88 RID: 7816
		public int S;
	}
}
