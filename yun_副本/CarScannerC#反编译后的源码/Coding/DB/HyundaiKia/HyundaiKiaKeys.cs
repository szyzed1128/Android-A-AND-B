using System;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BA5 RID: 2981
	internal static class HyundaiKiaKeys
	{
		// Token: 0x06005AB3 RID: 23219 RVA: 0x00434824 File Offset: 0x00432A24
		internal static byte[] AbsSportageQL(byte[] seed)
		{
			if (seed == null || seed.Length != 2)
			{
				return new byte[0];
			}
			int num = seed[1] >> 4;
			num--;
			int num2 = ((int)seed[0] * 256 + (int)seed[1]) * 8 + num;
			return new byte[]
			{
				(byte)(num2 >> 8),
				(byte)(num2 & 255)
			};
		}
	}
}
