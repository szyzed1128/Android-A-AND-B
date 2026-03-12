using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000767 RID: 1895
	public class SupportedPID
	{
		// Token: 0x0600400D RID: 16397 RVA: 0x0033577E File Offset: 0x0033397E
		public SupportedPID(string Command)
		{
			this.Command = Command;
			this.UpdateInterval();
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x0600400E RID: 16398 RVA: 0x00335793 File Offset: 0x00333993
		// (set) Token: 0x0600400F RID: 16399 RVA: 0x0033579B File Offset: 0x0033399B
		public string Command
		{
			[CompilerGenerated]
			get
			{
				return this.<Command>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Command>k__BackingField = value;
			}
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x003357A4 File Offset: 0x003339A4
		public void UpdateInterval()
		{
			this.Value = new bool[32];
			this.start_value = BitHelpers.ConvertHexToInt(this.Command) + 1;
			this.end_value = this.start_value + 31;
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x003357D8 File Offset: 0x003339D8
		public void PrintSupported()
		{
			for (int i = 0; i < this.Value.Length; i++)
			{
				if (this.Value[i])
				{
					Console.WriteLine((this.start_value + i).ToString("X4"));
				}
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0033581C File Offset: 0x00333A1C
		public List<string> GetSupported()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Value.Length; i++)
			{
				if (this.Value[i])
				{
					list.Add((this.start_value + i).ToString("X4"));
				}
			}
			return list;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x00335868 File Offset: 0x00333A68
		public List<string> GetSupported(string data)
		{
			this.Decode(data);
			return this.GetSupported();
		}

		// Token: 0x06004014 RID: 16404 RVA: 0x00335877 File Offset: 0x00333A77
		public List<string> GetSupported(byte[] data)
		{
			this.Decode(data);
			return this.GetSupported();
		}

		// Token: 0x06004015 RID: 16405 RVA: 0x00335888 File Offset: 0x00333A88
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(string.Concat(new string[]
			{
				" [",
				this.start_value.ToString("X4", CultureInfo.InvariantCulture),
				"..",
				this.end_value.ToString("X4", CultureInfo.InvariantCulture),
				"]\n"
			}));
			bool[] value = this.Value;
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i])
				{
					stringBuilder.Append("1");
				}
				else
				{
					stringBuilder.Append("0");
				}
			}
			for (int j = 0; j < this.Value.Length; j++)
			{
				int num = this.start_value + j;
				stringBuilder.Append("\n");
				stringBuilder.Append(num.ToString("X4"));
				stringBuilder.Append(":");
				stringBuilder.Append(this.Value[j].ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x06004016 RID: 16406 RVA: 0x00335988 File Offset: 0x00333B88
		// (set) Token: 0x06004017 RID: 16407 RVA: 0x00335990 File Offset: 0x00333B90
		public bool[] Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0033599C File Offset: 0x00333B9C
		public void Decode(string data)
		{
			data = data.Replace(" ", "");
			byte[] array = BitHelpers.ConvertHexToBytesX(data);
			this.Decode(array);
		}

		// Token: 0x06004019 RID: 16409 RVA: 0x003359CC File Offset: 0x00333BCC
		public void Decode(byte[] data)
		{
			bool[] array = new bool[32];
			for (int i = 0; i < 4; i++)
			{
				BitArrayReverse bitArrayReverse = new BitArrayReverse(new BitArray(new byte[] { data[i] }));
				bool[] array2 = new bool[8];
				for (int j = 0; j < 8; j++)
				{
					array2[j] = bitArrayReverse[j];
				}
				Array.Copy(array2, 0, array, i * 8, 8);
			}
			this.Value = array;
		}

		// Token: 0x04002766 RID: 10086
		[CompilerGenerated]
		private string <Command>k__BackingField;

		// Token: 0x04002767 RID: 10087
		private int start_value;

		// Token: 0x04002768 RID: 10088
		private int end_value;

		// Token: 0x04002769 RID: 10089
		[CompilerGenerated]
		private bool[] <Value>k__BackingField;
	}
}
