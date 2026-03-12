using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002F0 RID: 752
	internal class CANFrame
	{
		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x001B1B39 File Offset: 0x001AFD39
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x001B1B41 File Offset: 0x001AFD41
		public string Header
		{
			[CompilerGenerated]
			get
			{
				return this.<Header>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Header>k__BackingField = value;
			}
		}

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x001B1B4A File Offset: 0x001AFD4A
		// (set) Token: 0x06002375 RID: 9077 RVA: 0x001B1B52 File Offset: 0x001AFD52
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

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06002376 RID: 9078 RVA: 0x001B1B5B File Offset: 0x001AFD5B
		// (set) Token: 0x06002377 RID: 9079 RVA: 0x001B1B63 File Offset: 0x001AFD63
		public int ExpectedLength
		{
			[CompilerGenerated]
			get
			{
				return this.<ExpectedLength>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExpectedLength>k__BackingField = value;
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x001B1B6C File Offset: 0x001AFD6C
		// (set) Token: 0x06002379 RID: 9081 RVA: 0x001B1B74 File Offset: 0x001AFD74
		public string ExtendedAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<ExtendedAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExtendedAddress>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x0600237A RID: 9082 RVA: 0x001B1B7D File Offset: 0x001AFD7D
		// (set) Token: 0x0600237B RID: 9083 RVA: 0x001B1B85 File Offset: 0x001AFD85
		public bool HasExtendedAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<HasExtendedAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HasExtendedAddress>k__BackingField = value;
			}
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x0600237C RID: 9084 RVA: 0x001B1B8E File Offset: 0x001AFD8E
		// (set) Token: 0x0600237D RID: 9085 RVA: 0x001B1B96 File Offset: 0x001AFD96
		public CANFrame.CANFrameTypes Type
		{
			[CompilerGenerated]
			get
			{
				return this.<Type>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Type>k__BackingField = value;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x0600237E RID: 9086 RVA: 0x001B1B9F File Offset: 0x001AFD9F
		// (set) Token: 0x0600237F RID: 9087 RVA: 0x001B1BA7 File Offset: 0x001AFDA7
		public ELMFormat CANFormat
		{
			[CompilerGenerated]
			get
			{
				return this.<CANFormat>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CANFormat>k__BackingField = value;
			}
		}

		// Token: 0x06002380 RID: 9088 RVA: 0x001B1BB0 File Offset: 0x001AFDB0
		public CANFrame(string data_line, ELMFormat CANFormat, bool has_extended_address = false)
		{
			data_line = OBDDataReader.FilterHexAndNewLineOnly(data_line);
			this.RawHexData = data_line;
			if (data_line.Length < 7)
			{
				this.Type = CANFrame.CANFrameTypes.Error;
				return;
			}
			if (CANFormat == ELMFormat.CAN11bit)
			{
				this.Header = data_line.Substring(0, 3);
			}
			else
			{
				this.Header = data_line.Substring(0, 8);
			}
			string text = data_line.Substring(this.Header.Length);
			this.HasExtendedAddress = has_extended_address;
			if (has_extended_address)
			{
				this.ExtendedAddress = text.Substring(0, 2);
				text = text.Substring(2);
			}
			switch (text[0])
			{
			case '0':
			{
				this.Type = CANFrame.CANFrameTypes.SingleFrame;
				byte[] array = BitHelpers.ConvertHexToBytesX(text);
				this.ExpectedLength = (int)array[0];
				if (array.Length > this.ExpectedLength + 1)
				{
					this.Data = new byte[this.ExpectedLength];
					Array.Copy(array, 1, this.Data, 0, this.ExpectedLength);
					return;
				}
				this.Data = new byte[array.Length - 1];
				Array.Copy(array, 1, this.Data, 0, array.Length - 1);
				return;
			}
			case '1':
			{
				this.Type = CANFrame.CANFrameTypes.MultiFrameFirstFrame;
				byte[] array2 = BitHelpers.ConvertHexToBytesX(text);
				this.ExpectedLength = (int)(array2[0] & 15) * 256 + (int)array2[1];
				this.Data = new byte[array2.Length - 2];
				Array.Copy(array2, 2, this.Data, 0, array2.Length - 2);
				return;
			}
			case '2':
				this.Type = CANFrame.CANFrameTypes.MultiFrameContinuation;
				this.Type = CANFrame.CANFrameTypes.MultiFrameContinuation;
				this.Data = BitHelpers.ConvertHexToBytesX(text.Substring(2));
				this.ExpectedLength = this.Data.Length;
				return;
			case '3':
				this.Type = CANFrame.CANFrameTypes.FlowControl;
				this.Data = BitHelpers.ConvertHexToBytesX(text);
				this.ExpectedLength = 3;
				return;
			default:
				this.Type = CANFrame.CANFrameTypes.Other;
				this.Data = BitHelpers.ConvertHexToBytesX(text);
				this.ExpectedLength = this.Data.Length;
				return;
			}
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x001B1D89 File Offset: 0x001AFF89
		public override string ToString()
		{
			return this.RawHexData;
		}

		// Token: 0x0400113A RID: 4410
		[CompilerGenerated]
		private string <Header>k__BackingField;

		// Token: 0x0400113B RID: 4411
		[CompilerGenerated]
		private byte[] <Data>k__BackingField;

		// Token: 0x0400113C RID: 4412
		[CompilerGenerated]
		private int <ExpectedLength>k__BackingField;

		// Token: 0x0400113D RID: 4413
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x0400113E RID: 4414
		[CompilerGenerated]
		private bool <HasExtendedAddress>k__BackingField;

		// Token: 0x0400113F RID: 4415
		[CompilerGenerated]
		private CANFrame.CANFrameTypes <Type>k__BackingField;

		// Token: 0x04001140 RID: 4416
		[CompilerGenerated]
		private ELMFormat <CANFormat>k__BackingField;

		// Token: 0x04001141 RID: 4417
		private string RawHexData;

		// Token: 0x020002F1 RID: 753
		public enum CANFrameTypes
		{
			// Token: 0x04001143 RID: 4419
			SingleFrame,
			// Token: 0x04001144 RID: 4420
			MultiFrameFirstFrame,
			// Token: 0x04001145 RID: 4421
			MultiFrameContinuation,
			// Token: 0x04001146 RID: 4422
			FlowControl,
			// Token: 0x04001147 RID: 4423
			Other,
			// Token: 0x04001148 RID: 4424
			Error
		}
	}
}
