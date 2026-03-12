using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace ECUSIM2000v2.Primitives
{
	// Token: 0x02000016 RID: 22
	public class CANFrameRaw
	{
		// Token: 0x0600009D RID: 157 RVA: 0x00005258 File Offset: 0x00003458
		public CANFrameRaw(string header, int dlc, string data)
		{
			if (header.Length == 3)
			{
				this.Format = CANFrameRaw.FrameFormat.Can11bit;
			}
			else
			{
				if (header.Length != 8)
				{
					throw new ArgumentException("Wrong header format");
				}
				this.Format = CANFrameRaw.FrameFormat.Can29bit;
			}
			this.DLC = dlc;
			data = data.Replace(" ", "");
			this.DataHex = data;
			this.Data = BitHelpers.ConvertHexToBytesX(data);
			this.CanIdHex = header;
			this.CanId = BitHelpers.ConvertHexToBytesX(this.CanIdHex);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000052E9 File Offset: 0x000034E9
		public CANFrameRaw(string header, int dlc, byte[] data)
			: this(header, dlc, BitHelpers.ByteArrayToHexString(data))
		{
		}

		// Token: 0x0600009F RID: 159 RVA: 0x000052F9 File Offset: 0x000034F9
		public CANFrameRaw(string header, byte[] data)
			: this(header, data.Length, BitHelpers.ByteArrayToHexString(data))
		{
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000530C File Offset: 0x0000350C
		public static CANFrameRaw FromCanHackerLogLine(string frame)
		{
			CANFrameRaw canframeRaw;
			try
			{
				string[] array = frame.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
				string text = array[3];
				int num = int.Parse(array[4]);
				string text2 = array[5].Replace(" ", "").Substring(0, num * 2);
				canframeRaw = new CANFrameRaw(text, num, text2);
			}
			catch (Exception)
			{
				canframeRaw = null;
			}
			return canframeRaw;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005374 File Offset: 0x00003574
		public static CANFrameRaw CreateFrameFromCanHackerMessage(string canHackerMessage)
		{
			CANFrameRaw canframeRaw;
			try
			{
				canframeRaw = new CANFrameRaw(canHackerMessage);
			}
			catch (Exception)
			{
				canframeRaw = null;
			}
			return canframeRaw;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000053A0 File Offset: 0x000035A0
		protected CANFrameRaw(string canHackerMessage)
		{
			try
			{
				char c = canHackerMessage[0];
				if (c != 'T')
				{
					if (c == 't')
					{
						this.Format = CANFrameRaw.FrameFormat.Can11bit;
						this.CanIdHex = canHackerMessage.Substring(2, 3);
						this.DLC = int.Parse(canHackerMessage[5].ToString(), NumberStyles.HexNumber);
						this.DataHex = canHackerMessage.Substring(6);
						this.DataHex = this.DataHex.Substring(0, this.DLC * 2);
					}
				}
				else
				{
					this.Format = CANFrameRaw.FrameFormat.Can29bit;
					this.CanIdHex = canHackerMessage.Substring(2, 8);
					this.DLC = int.Parse(canHackerMessage[10].ToString(), NumberStyles.HexNumber);
					this.DataHex = canHackerMessage.Substring(11);
					this.DataHex = this.DataHex.Substring(0, this.DLC * 2);
				}
				this.CanId = BitHelpers.ConvertHexToBytesX(this.CanIdHex);
				this.Data = BitHelpers.ConvertHexToBytesX(this.DataHex);
			}
			catch (Exception)
			{
				throw new ArgumentException();
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000054C8 File Offset: 0x000036C8
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x000054D0 File Offset: 0x000036D0
		public string CanIdHex
		{
			[CompilerGenerated]
			get
			{
				return this.<CanIdHex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CanIdHex>k__BackingField = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000054D9 File Offset: 0x000036D9
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x000054E1 File Offset: 0x000036E1
		public byte[] CanId
		{
			[CompilerGenerated]
			get
			{
				return this.<CanId>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CanId>k__BackingField = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000054EA File Offset: 0x000036EA
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x000054F2 File Offset: 0x000036F2
		public int DLC
		{
			[CompilerGenerated]
			get
			{
				return this.<DLC>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DLC>k__BackingField = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x000054FB File Offset: 0x000036FB
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00005503 File Offset: 0x00003703
		public byte[] Data
		{
			[CompilerGenerated]
			get
			{
				return this.<Data>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Data>k__BackingField = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000550C File Offset: 0x0000370C
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00005514 File Offset: 0x00003714
		public string DataHex
		{
			[CompilerGenerated]
			get
			{
				return this.<DataHex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DataHex>k__BackingField = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000551D File Offset: 0x0000371D
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00005525 File Offset: 0x00003725
		public int Delay
		{
			[CompilerGenerated]
			get
			{
				return this.<Delay>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Delay>k__BackingField = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005530 File Offset: 0x00003730
		public string TimeStr
		{
			get
			{
				return this.Timestamp.TotalSeconds.ToString("00.000");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00005558 File Offset: 0x00003758
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x00005560 File Offset: 0x00003760
		public TimeSpan Timestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<Timestamp>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Timestamp>k__BackingField = value;
			}
		} = TimeSpan.Zero;

		// Token: 0x060000B2 RID: 178 RVA: 0x0000556C File Offset: 0x0000376C
		public string ToCanHackerTransferMessage(bool fillTo8Bytes, string fillByteStr)
		{
			if (fillTo8Bytes && this.DataHex.Length < 16)
			{
				while (this.DataHex.Length < 16)
				{
					this.DataHex += fillByteStr;
				}
			}
			string text = ((this.Format == CANFrameRaw.FrameFormat.Can11bit) ? "t" : "T");
			return string.Concat(new string[]
			{
				text,
				"1",
				this.CanIdHex,
				this.DLC.ToString("X1"),
				this.DataHex
			});
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00005604 File Offset: 0x00003804
		public static List<CANFrameRaw> MessageToFrames(string canId, string dataHex, bool variableDLC, string extendedAddress)
		{
			if (string.IsNullOrEmpty(dataHex) || string.IsNullOrEmpty(canId))
			{
				return new List<CANFrameRaw>(0);
			}
			byte[] array = BitHelpers.ConvertHexToBytesX(dataHex);
			if (dataHex.StartsWith("81"))
			{
				byte[] array2 = BitHelpers.ConvertHexToBytesX(dataHex);
				CANFrameRaw canframeRaw = new CANFrameRaw(canId, array2);
				return new List<CANFrameRaw>(1) { canframeRaw };
			}
			int num = 7;
			int num2 = 6;
			if (!string.IsNullOrEmpty(extendedAddress))
			{
				num = 6;
				num2 = 5;
			}
			else
			{
				extendedAddress = "";
			}
			if (array.Length <= num)
			{
				int num3 = 8;
				string text = extendedAddress + array.Length.ToString("X2") + BitHelpers.ByteArrayToHexString(array);
				if (variableDLC)
				{
					num3 = text.Length / 2;
				}
				CANFrameRaw canframeRaw2 = new CANFrameRaw(canId, num3, text);
				return new List<CANFrameRaw> { canframeRaw2 };
			}
			string text2 = "1" + array.Length.ToString("X3");
			MemoryStream memoryStream = new MemoryStream(array);
			List<CANFrameRaw> list = new List<CANFrameRaw>();
			string text3 = extendedAddress + text2 + dataHex.Substring(0, num2 * 2);
			CANFrameRaw canframeRaw3 = new CANFrameRaw(canId, 8, text3);
			list.Add(canframeRaw3);
			memoryStream.Seek((long)num2, SeekOrigin.Begin);
			int num4 = 1;
			while (memoryStream.Position < memoryStream.Length)
			{
				byte[] array3 = new byte[num];
				long num5 = (long)memoryStream.Read(array3, 0, array3.Length);
				string text4 = BitHelpers.ByteArrayToHexString(array3);
				int num6 = 8;
				if (variableDLC)
				{
					text4 = text4.Substring(0, (int)num5 * 2);
					text4 = extendedAddress + "2" + num4.ToString("X1") + text4;
					num6 = text4.Length / 2;
				}
				else
				{
					text4 = extendedAddress + "2" + num4.ToString("X1") + text4;
				}
				CANFrameRaw canframeRaw4 = new CANFrameRaw(canId, num6, text4);
				list.Add(canframeRaw4);
				num4++;
				if (num4 > 15)
				{
					num4 = 0;
				}
			}
			return list;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000057E0 File Offset: 0x000039E0
		public override string ToString()
		{
			string text = string.Format("{0} {1} ", this.CanIdHex, this.DLC);
			string text2 = "";
			foreach (byte b in this.Data)
			{
				text2 = text2 + b.ToString("X2") + " ";
			}
			text2 = text2.Trim();
			return text + text2;
		}

		// Token: 0x04000060 RID: 96
		[CompilerGenerated]
		private string <CanIdHex>k__BackingField;

		// Token: 0x04000061 RID: 97
		[CompilerGenerated]
		private byte[] <CanId>k__BackingField;

		// Token: 0x04000062 RID: 98
		[CompilerGenerated]
		private int <DLC>k__BackingField;

		// Token: 0x04000063 RID: 99
		[CompilerGenerated]
		private byte[] <Data>k__BackingField;

		// Token: 0x04000064 RID: 100
		[CompilerGenerated]
		private string <DataHex>k__BackingField;

		// Token: 0x04000065 RID: 101
		public readonly CANFrameRaw.FrameFormat Format;

		// Token: 0x04000066 RID: 102
		[CompilerGenerated]
		private int <Delay>k__BackingField;

		// Token: 0x04000067 RID: 103
		[CompilerGenerated]
		private TimeSpan <Timestamp>k__BackingField;

		// Token: 0x02000017 RID: 23
		public enum FrameFormat
		{
			// Token: 0x04000069 RID: 105
			Can11bit,
			// Token: 0x0400006A RID: 106
			Can29bit
		}
	}
}
