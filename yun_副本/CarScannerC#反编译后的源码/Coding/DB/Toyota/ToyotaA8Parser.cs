using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009DD RID: 2525
	internal static class ToyotaA8Parser
	{
		// Token: 0x0600516C RID: 20844 RVA: 0x003F1850 File Offset: 0x003EFA50
		internal static List<ToyotaA8Item> ParseResponse(OBDRequest request, string extendedAddress, string message)
		{
			List<ToyotaA8Item> list = new List<ToyotaA8Item>();
			if (message == null)
			{
				return list;
			}
			if (message.Contains("NO DATA") || message.Contains("ERROR"))
			{
				return list;
			}
			try
			{
				string responseHeader = CAN11bitHelper.GetPossibleResponseHeader(request.Header, "Toyota", request);
				string[] array = (from x in OBDDataReader.FilterHexAndNewLineOnly(message).Split(new char[] { '\r', '\n' })
					where !string.IsNullOrEmpty(x)
					where x.StartsWith(responseHeader)
					select x).ToArray<string>();
				int num = 3;
				if (!string.IsNullOrEmpty(extendedAddress))
				{
					num = 5;
				}
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Substring(num);
				}
				List<string> list2 = new List<string>();
				List<string> list3 = array.ToList<string>();
				while (list3.Count > 0)
				{
					string text = list3[0];
					if (ToyotaA8Parser.IsSingleFrame(text))
					{
						int lengthFromFrame = ToyotaA8Parser.GetLengthFromFrame(text);
						text = text.Substring(2, lengthFromFrame * 2);
						if (text.StartsWith("7F" + request.Command.Substring(0, 2)))
						{
							list3.RemoveAt(0);
						}
						else
						{
							list2.Add(text);
							list3.RemoveAt(0);
						}
					}
					else if (ToyotaA8Parser.IsFirstFrame(text))
					{
						string[] array2 = list3.Take(1).Concat(list3.Skip(1).TakeWhile((string x) => x.Length >= 4 && !ToyotaA8Parser.IsFirstFrame(x) && !ToyotaA8Parser.IsSingleFrame(x))).ToArray<string>();
						list3 = list3.Skip(array2.Length).ToList<string>();
						StringBuilder stringBuilder = new StringBuilder(array2.Length);
						for (int j = 0; j < array2.Length; j++)
						{
							if (j == 0)
							{
								stringBuilder.Append(array2[j]);
							}
							else
							{
								stringBuilder.Append(array2[j].Substring(2));
							}
						}
						string text2 = stringBuilder.ToString();
						int lengthFromFrame2 = ToyotaA8Parser.GetLengthFromFrame(text2);
						string text3 = text2.Substring(4);
						int num2 = lengthFromFrame2 * 2;
						text3 = text3.Substring(0, num2);
						list2.Add(text3);
					}
					else
					{
						list3.RemoveAt(0);
					}
				}
				foreach (string text4 in list2)
				{
					byte[] array3 = BitHelpers.ConvertHexToBytesX(text4.Substring(4));
					while (array3.Length != 0)
					{
						try
						{
							int num3 = (int)array3[1];
							byte[] array4 = array3.Take(2 + num3).ToArray<byte>();
							array3 = array3.Skip(2 + num3).ToArray<byte>();
							ToyotaA8Item toyotaA8Item = new ToyotaA8Item(request.Command, request.Header, extendedAddress, array4);
							list.Add(toyotaA8Item);
						}
						catch (Exception)
						{
							break;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x0600516D RID: 20845 RVA: 0x003F1B70 File Offset: 0x003EFD70
		private static bool IsFirstFrame(string dataframe)
		{
			return dataframe[0] == '1';
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x003F1B80 File Offset: 0x003EFD80
		private static bool IsSingleFrame(string dataframe)
		{
			return dataframe[0] == '0';
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x003F1B90 File Offset: 0x003EFD90
		private static int GetLengthFromFrame(string frame)
		{
			string text = "00";
			if (ToyotaA8Parser.IsSingleFrame(frame))
			{
				text = frame.Substring(0, 2);
			}
			else if (ToyotaA8Parser.IsFirstFrame(frame))
			{
				text = "0" + frame.Substring(1, 3);
			}
			return BitHelpers.ConvertHexToInt(text);
		}

		// Token: 0x020009DE RID: 2526
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005170 RID: 20848 RVA: 0x003F1BD7 File Offset: 0x003EFDD7
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005171 RID: 20849 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005172 RID: 20850 RVA: 0x000650BC File Offset: 0x000632BC
			internal bool <ParseResponse>b__0_0(string x)
			{
				return !string.IsNullOrEmpty(x);
			}

			// Token: 0x06005173 RID: 20851 RVA: 0x003F1BE3 File Offset: 0x003EFDE3
			internal bool <ParseResponse>b__0_2(string x)
			{
				return x.Length >= 4 && !ToyotaA8Parser.IsFirstFrame(x) && !ToyotaA8Parser.IsSingleFrame(x);
			}

			// Token: 0x04003163 RID: 12643
			public static readonly ToyotaA8Parser.<>c <>9 = new ToyotaA8Parser.<>c();

			// Token: 0x04003164 RID: 12644
			public static Func<string, bool> <>9__0_0;

			// Token: 0x04003165 RID: 12645
			public static Func<string, bool> <>9__0_2;
		}

		// Token: 0x020009DF RID: 2527
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005174 RID: 20852 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005175 RID: 20853 RVA: 0x003F1C01 File Offset: 0x003EFE01
			internal bool <ParseResponse>b__1(string x)
			{
				return x.StartsWith(this.responseHeader);
			}

			// Token: 0x04003166 RID: 12646
			public string responseHeader;
		}
	}
}
