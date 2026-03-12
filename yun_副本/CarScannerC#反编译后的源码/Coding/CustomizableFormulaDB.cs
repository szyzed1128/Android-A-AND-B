using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000876 RID: 2166
	internal static class CustomizableFormulaDB
	{
		// Token: 0x060049F9 RID: 18937 RVA: 0x0037BFE8 File Offset: 0x0037A1E8
		public static string INSERT_ZERO_BYTE(string inputHex, params string[] args)
		{
			List<byte> list = BitHelpers.ConvertHexToBytesX(inputHex).ToList<byte>();
			for (int i = 0; i < args.Length; i++)
			{
				int num = int.Parse(args[i], CultureInfo.InvariantCulture);
				list.Insert(num, 0);
			}
			return BitHelpers.ByteArrayToHexString(list.ToArray());
		}

		// Token: 0x060049FA RID: 18938 RVA: 0x0037C034 File Offset: 0x0037A234
		public static string INSERT_BYTES(string inputHex, params string[] args)
		{
			List<byte> list = BitHelpers.ConvertHexToBytesX(inputHex).ToList<byte>();
			for (int i = 0; i < args.Length; i += 2)
			{
				int num = int.Parse(args[i], CultureInfo.InvariantCulture);
				byte b = byte.Parse(args[i + 1], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
				list.Insert(num, b);
			}
			return BitHelpers.ByteArrayToHexString(list.ToArray());
		}
	}
}
