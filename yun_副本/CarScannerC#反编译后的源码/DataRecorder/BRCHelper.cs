using System;
using System.IO;
using System.Text;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006D5 RID: 1749
	internal class BRCHelper
	{
		// Token: 0x06003B82 RID: 15234 RVA: 0x00314F3C File Offset: 0x0031313C
		internal static BRCHelper.BRCType GetVersion(string filePath)
		{
			BRCHelper.BRCType version;
			using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			{
				version = BRCHelper.GetVersion(fileStream);
			}
			return version;
		}

		// Token: 0x06003B83 RID: 15235 RVA: 0x00314F78 File Offset: 0x00313178
		internal static BRCHelper.BRCType GetVersion(Stream stream)
		{
			BRCHelper.BRCType brctype;
			using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8, true))
			{
				try
				{
					if (binaryReader.ReadString() == "CARSCANNERRECORD")
					{
						brctype = BRCHelper.BRCType.V2;
					}
					else
					{
						brctype = BRCHelper.BRCType.V1;
					}
				}
				catch (Exception)
				{
					brctype = BRCHelper.BRCType.V1;
				}
			}
			return brctype;
		}

		// Token: 0x06003B84 RID: 15236 RVA: 0x00002050 File Offset: 0x00000250
		public BRCHelper()
		{
		}

		// Token: 0x020006D6 RID: 1750
		internal enum BRCType
		{
			// Token: 0x04002476 RID: 9334
			V1,
			// Token: 0x04002477 RID: 9335
			V2
		}
	}
}
