using System;
using System.Collections;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007BD RID: 1981
	internal class BitArrayReverse
	{
		// Token: 0x06004668 RID: 18024 RVA: 0x00369F1C File Offset: 0x0036811C
		public BitArrayReverse(BitArray ba)
		{
			this._ba = ba;
		}

		// Token: 0x06004669 RID: 18025 RVA: 0x00369F2B File Offset: 0x0036812B
		public BitArrayReverse(byte[] data)
		{
			this._ba = new BitArray(data);
		}

		// Token: 0x1700160A RID: 5642
		public bool this[int index]
		{
			get
			{
				return this._ba[this._ba.Length - 1 - index];
			}
			set
			{
				this._ba[this._ba.Length - 1 - index] = value;
			}
		}

		// Token: 0x040028F0 RID: 10480
		private BitArray _ba;
	}
}
