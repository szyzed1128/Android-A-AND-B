using System;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007C0 RID: 1984
	internal class Crc16Ccitt
	{
		// Token: 0x0600467D RID: 18045 RVA: 0x0036A314 File Offset: 0x00368514
		public ushort ComputeChecksum(byte[] bytes)
		{
			ushort num = this.initialValue;
			for (int i = 0; i < bytes.Length; i++)
			{
				num = (ushort)(((int)num << 8) ^ (int)this.table[(num >> 8) ^ (int)(byte.MaxValue & bytes[i])]);
			}
			return num;
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0036A354 File Offset: 0x00368554
		public byte[] ComputeChecksumBytes(byte[] bytes)
		{
			byte[] bytes2 = BitConverter.GetBytes(this.ComputeChecksum(bytes));
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes2);
			}
			return bytes2;
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x0036A37C File Offset: 0x0036857C
		public Crc16Ccitt(InitialCrcValue initialValue)
		{
			this.initialValue = (ushort)initialValue;
			for (int i = 0; i < this.table.Length; i++)
			{
				ushort num = 0;
				ushort num2 = (ushort)(i << 8);
				for (int j = 0; j < 8; j++)
				{
					if (((num ^ num2) & 32768) != 0)
					{
						num = (ushort)(((int)num << 1) ^ 4129);
					}
					else
					{
						num = (ushort)(num << 1);
					}
					num2 = (ushort)(num2 << 1);
				}
				this.table[i] = num;
			}
		}

		// Token: 0x040028F6 RID: 10486
		private const ushort poly = 4129;

		// Token: 0x040028F7 RID: 10487
		private ushort[] table = new ushort[256];

		// Token: 0x040028F8 RID: 10488
		private ushort initialValue;
	}
}
