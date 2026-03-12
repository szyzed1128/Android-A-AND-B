using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A77 RID: 2679
	internal class A5DatasetCustomizer : INotifyPropertyChanged
	{
		// Token: 0x06005498 RID: 21656 RVA: 0x00002050 File Offset: 0x00000250
		public A5DatasetCustomizer()
		{
		}

		// Token: 0x06005499 RID: 21657 RVA: 0x00403836 File Offset: 0x00401A36
		public A5DatasetCustomizer(string hex, A5DatasetCustomizer.CameraVersions hwVersion)
		{
			this.Load(hex, hwVersion);
		}

		// Token: 0x0600549A RID: 21658 RVA: 0x00403848 File Offset: 0x00401A48
		public void Load(byte[] data, A5DatasetCustomizer.CameraVersions cameraVersion)
		{
			this.originalData = data.ToArray<byte>();
			this.CameraVersion = cameraVersion;
			switch (cameraVersion)
			{
			case A5DatasetCustomizer.CameraVersions.H:
				this.LaneAssistTimerAddress = 13084;
				this.LaneAssistXorResultAddress = 13744;
				this.TJAAddress = 13748;
				this.LaneAssistTimerConfigurationAvailable = true;
				this.TJAConfigurationAvailable = true;
				if (SharedSettings.Current.ShowExperimental)
				{
					this.LaneAssistStartSpeedConfigurationAvailable = true;
					this.LaneAssistStartSpeedAddress = 13176;
					this.LaneAssistXorResultAddress = 13882;
					this.LightAssistSpeedConfigurationAvailable = true;
					this.LightAssistOnAddress1 = 3834;
					this.LightAssistOnAddress2 = 3922;
					this.LightAssistOffAddress = 3836;
				}
				break;
			case A5DatasetCustomizer.CameraVersions.L:
				this.LaneAssistTimerAddress = 13220;
				this.LaneAssistXorResultAddress = 13880;
				this.TJAAddress = 13884;
				this.LaneAssistTimerConfigurationAvailable = true;
				this.TJAConfigurationAvailable = true;
				if (SharedSettings.Current.ShowExperimental)
				{
					this.LaneAssistStartSpeedConfigurationAvailable = true;
					this.LaneAssistStartSpeedAddress = 13312;
					this.LaneAssistStartSpeedXorAddress = 13882;
					this.LightAssistSpeedConfigurationAvailable = true;
					this.LightAssistOnAddress1 = 3830;
					this.LightAssistOnAddress2 = 3918;
					this.LightAssistOffAddress = 3832;
				}
				break;
			case A5DatasetCustomizer.CameraVersions.F:
				this.LaneAssistTimerAddress = 13204;
				this.LaneAssistXorResultAddress = 13864;
				this.TJAAddress = 13868;
				this.LaneAssistTimerConfigurationAvailable = true;
				this.TJAConfigurationAvailable = true;
				this.LaneAssistStartSpeedConfigurationAvailable = false;
				this.LightAssistSpeedConfigurationAvailable = false;
				break;
			case A5DatasetCustomizer.CameraVersions.G:
				this.TJAAddress = 13744;
				this.TJAConfigurationAvailable = true;
				this.LaneAssistTimerConfigurationAvailable = false;
				this.LaneAssistStartSpeedConfigurationAvailable = false;
				this.LightAssistSpeedConfigurationAvailable = false;
				break;
			case A5DatasetCustomizer.CameraVersions.S:
				this.LaneAssistTimerConfigurationAvailable = true;
				this.LaneAssistTimerAddress = 13770;
				this.LaneAssistXorResultAddress = 13090;
				this.TJAConfigurationAvailable = true;
				this.TJAAddress = 13886;
				this.LaneAssistStartSpeedConfigurationAvailable = false;
				this.LightAssistSpeedConfigurationAvailable = false;
				break;
			case A5DatasetCustomizer.CameraVersions.M:
				this.TJAAddress = 13884;
				this.TJAConfigurationAvailable = true;
				this.LaneAssistTimerConfigurationAvailable = false;
				this.LaneAssistStartSpeedConfigurationAvailable = false;
				this.LightAssistSpeedConfigurationAvailable = false;
				if (SharedSettings.Current.ShowExperimental)
				{
					this.LaneAssistTimerConfigurationAvailable = true;
					this.LaneAssistTimerAddress = 13770;
					this.LaneAssistXorResultAddress = 13090;
				}
				break;
			}
			if (this.LaneAssistTimerConfigurationAvailable)
			{
				if (cameraVersion == A5DatasetCustomizer.CameraVersions.S || cameraVersion == A5DatasetCustomizer.CameraVersions.M)
				{
					byte[] array = new byte[]
					{
						this.originalData[this.LaneAssistTimerAddress],
						this.originalData[this.LaneAssistTimerAddress + 1],
						this.originalData[this.LaneAssistTimerAddress + 2],
						this.originalData[this.LaneAssistTimerAddress + 3]
					};
					if (!BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					float num = BitConverter.ToSingle(array, 0);
					this.LaneAssistTimer = (int)num;
					array = new byte[]
					{
						this.originalData[this.LaneAssistTimerAddress],
						this.originalData[this.LaneAssistTimerAddress + 1],
						this.originalData[this.LaneAssistTimerAddress + 2],
						this.originalData[this.LaneAssistTimerAddress + 3]
					};
					byte[] array2 = new byte[]
					{
						this.originalData[this.LaneAssistXorResultAddress],
						this.originalData[this.LaneAssistXorResultAddress + 1],
						this.originalData[this.LaneAssistXorResultAddress + 2],
						this.originalData[this.LaneAssistXorResultAddress + 3]
					};
					int num2 = (int)array[0] * 256 * 256 * 256 + (int)array[1] * 256 * 256 + (int)array[2] * 256 + (int)array[3];
					int num3 = (int)array2[0] * 256 * 256 * 256 + (int)array2[1] * 256 * 256 + (int)array2[2] * 256 + (int)array2[3];
					this.LaneAssistXor = num2 ^ num3;
				}
				else
				{
					this.LaneAssistTimer = (int)this.originalData[this.LaneAssistTimerAddress + 1] * 256 + (int)this.originalData[this.LaneAssistTimerAddress];
					int num4 = (int)this.originalData[this.LaneAssistTimerAddress] * 256 + (int)this.originalData[this.LaneAssistTimerAddress + 1];
					int num5 = (int)this.originalData[this.LaneAssistXorResultAddress] * 256 + (int)this.originalData[this.LaneAssistXorResultAddress + 1];
					this.LaneAssistXor = num4 ^ num5;
				}
			}
			if (this.LaneAssistStartSpeedConfigurationAvailable)
			{
				byte[] array3 = new byte[]
				{
					this.originalData[this.LaneAssistStartSpeedAddress],
					this.originalData[this.LaneAssistStartSpeedAddress + 1],
					this.originalData[this.LaneAssistStartSpeedAddress + 2],
					this.originalData[this.LaneAssistStartSpeedAddress + 3]
				};
				if (!BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(array3);
				}
				float num6 = BitConverter.ToSingle(array3, 0);
				this.LaneAssistStartSpeed = (int)num6;
				array3 = new byte[]
				{
					this.originalData[this.LaneAssistStartSpeedAddress],
					this.originalData[this.LaneAssistStartSpeedAddress + 1],
					this.originalData[this.LaneAssistStartSpeedAddress + 2],
					this.originalData[this.LaneAssistStartSpeedAddress + 3]
				};
				byte[] array4 = new byte[]
				{
					this.originalData[this.LaneAssistStartSpeedXorAddress],
					this.originalData[this.LaneAssistStartSpeedXorAddress + 1]
				};
				int num7 = (int)array3[2] * 256 + (int)array3[3];
				int num8 = (int)array4[0] * 256 + (int)array4[1];
				this.LaneAssistStartSpeedXor = num7 ^ num8;
			}
			if (this.LightAssistSpeedConfigurationAvailable)
			{
				this.LightAssistOnSpeed = (int)this.originalData[this.LightAssistOnAddress1];
				this.LightAssistOffSpeed = (int)this.originalData[this.LightAssistOffAddress];
			}
			if (this.TJAConfigurationAvailable)
			{
				if (this.originalData[this.TJAAddress] == 1)
				{
					this.TJA = true;
					return;
				}
				this.TJA = false;
			}
		}

		// Token: 0x0600549B RID: 21659 RVA: 0x00403E1B File Offset: 0x0040201B
		public void Load(string hex, A5DatasetCustomizer.CameraVersions hwVersion)
		{
			this.originalData = BitHelpers.ConvertHexToBytesX(hex);
			this.Load(this.originalData, hwVersion);
		}

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600549C RID: 21660 RVA: 0x00403E38 File Offset: 0x00402038
		// (remove) Token: 0x0600549D RID: 21661 RVA: 0x00403E70 File Offset: 0x00402070
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x0600549E RID: 21662 RVA: 0x00403EA5 File Offset: 0x004020A5
		// (set) Token: 0x0600549F RID: 21663 RVA: 0x00403EAD File Offset: 0x004020AD
		public bool LaneAssistTimerConfigurationAvailable
		{
			get
			{
				return this._LaneAssistTimerConfigurationAvailable;
			}
			private set
			{
				this._LaneAssistTimerConfigurationAvailable = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LaneAssistTimerConfigurationAvailable"));
			}
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x060054A0 RID: 21664 RVA: 0x00403ED1 File Offset: 0x004020D1
		// (set) Token: 0x060054A1 RID: 21665 RVA: 0x00403ED9 File Offset: 0x004020D9
		public bool TJAConfigurationAvailable
		{
			get
			{
				return this._TJAConfigurationAvailable;
			}
			private set
			{
				this._TJAConfigurationAvailable = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("TJAConfigurationAvailable"));
			}
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x060054A2 RID: 21666 RVA: 0x00403EFD File Offset: 0x004020FD
		// (set) Token: 0x060054A3 RID: 21667 RVA: 0x00403F05 File Offset: 0x00402105
		public bool TJA
		{
			get
			{
				return this._TJA;
			}
			set
			{
				this._TJA = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("TJA"));
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x060054A4 RID: 21668 RVA: 0x00403F29 File Offset: 0x00402129
		// (set) Token: 0x060054A5 RID: 21669 RVA: 0x00403F31 File Offset: 0x00402131
		public int LaneAssistTimer
		{
			get
			{
				return this._LaneAssistTimer;
			}
			set
			{
				if (value == -2)
				{
					return;
				}
				if (value >= 0 || value <= 65535)
				{
					this._LaneAssistTimer = value;
				}
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LaneAssistTimer"));
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x060054A6 RID: 21670 RVA: 0x00403F67 File Offset: 0x00402167
		// (set) Token: 0x060054A7 RID: 21671 RVA: 0x00403F6F File Offset: 0x0040216F
		public bool LaneAssistStartSpeedConfigurationAvailable
		{
			get
			{
				return this._LaneAssistStartSpeedConfigurationAvailable;
			}
			private set
			{
				this._LaneAssistStartSpeedConfigurationAvailable = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LaneAssistStartSpeedConfigurationAvailable"));
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x060054A8 RID: 21672 RVA: 0x00403F93 File Offset: 0x00402193
		// (set) Token: 0x060054A9 RID: 21673 RVA: 0x00403F9B File Offset: 0x0040219B
		public int LaneAssistStartSpeed
		{
			get
			{
				return this._LaneAssistStartSpeed;
			}
			set
			{
				if (value >= 0 || value <= 65535)
				{
					this._LaneAssistStartSpeed = value;
				}
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LaneAssistStartSpeed"));
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x060054AA RID: 21674 RVA: 0x00403FCB File Offset: 0x004021CB
		// (set) Token: 0x060054AB RID: 21675 RVA: 0x00403FD3 File Offset: 0x004021D3
		public bool LightAssistSpeedConfigurationAvailable
		{
			get
			{
				return this._LightAssistSpeedConfigurationAvailable;
			}
			private set
			{
				this._LightAssistSpeedConfigurationAvailable = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LightAssistSpeedConfigurationAvailable"));
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x060054AC RID: 21676 RVA: 0x00403FF7 File Offset: 0x004021F7
		// (set) Token: 0x060054AD RID: 21677 RVA: 0x00404000 File Offset: 0x00402200
		public int LightAssistOnSpeed
		{
			get
			{
				return this._LightAssistOnSpeed;
			}
			set
			{
				if (value > 255)
				{
					value = 255;
				}
				if (value < 0)
				{
					value = 0;
				}
				if (value >= 0 || value <= 255)
				{
					this._LightAssistOnSpeed = value;
				}
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LightAssistOnSpeed"));
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x060054AE RID: 21678 RVA: 0x00404051 File Offset: 0x00402251
		// (set) Token: 0x060054AF RID: 21679 RVA: 0x0040405C File Offset: 0x0040225C
		public int LightAssistOffSpeed
		{
			get
			{
				return this._LightAssistOffSpeed;
			}
			set
			{
				if (value > 255)
				{
					value = 255;
				}
				if (value < 0)
				{
					value = 0;
				}
				if (value >= 0 || value <= 255)
				{
					this._LightAssistOffSpeed = value;
				}
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LightAssistOffSpeed"));
			}
		}

		// Token: 0x060054B0 RID: 21680 RVA: 0x004040B0 File Offset: 0x004022B0
		public byte[] Build()
		{
			byte[] array = new byte[this.originalData.Length - 4];
			Array.Copy(this.originalData, array, array.Length);
			if (this.TJAConfigurationAvailable)
			{
				if (this.TJA)
				{
					array[this.TJAAddress] = 1;
				}
				else
				{
					array[this.TJAAddress] = 0;
				}
			}
			if (this.LaneAssistTimerConfigurationAvailable)
			{
				if (this.CameraVersion == A5DatasetCustomizer.CameraVersions.S)
				{
					byte[] bytes = BitConverter.GetBytes((float)this.LaneAssistTimer);
					if (!BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(bytes);
					}
					Array.Copy(bytes, 0, array, this.LaneAssistTimerAddress, bytes.Length);
					byte[] bytes2 = BitConverter.GetBytes(((int)bytes[0] * 256 * 256 * 256 + (int)bytes[1] * 256 * 256 + (int)bytes[2] * 256 + (int)bytes[3]) ^ this.LaneAssistXor);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(bytes2);
					}
					Array.Copy(bytes2, 0, array, this.LaneAssistXorResultAddress, bytes2.Length);
				}
				else
				{
					int num = (this.LaneAssistTimer >> 8) & 255;
					int num2 = this.LaneAssistTimer & 255;
					array[this.LaneAssistTimerAddress] = (byte)num2;
					array[this.LaneAssistTimerAddress + 1] = (byte)num;
					int num3 = (num2 * 256 + num) ^ this.LaneAssistXor;
					array[this.LaneAssistXorResultAddress] = (byte)((num3 >> 8) & 255);
					array[this.LaneAssistXorResultAddress + 1] = (byte)(num3 & 255);
				}
			}
			if (this.LaneAssistStartSpeedConfigurationAvailable)
			{
				byte[] bytes3 = BitConverter.GetBytes((float)this.LaneAssistStartSpeed);
				if (!BitConverter.IsLittleEndian)
				{
					Array.Reverse<byte>(bytes3);
				}
				Array.Copy(bytes3, 0, array, this.LaneAssistStartSpeedAddress, bytes3.Length);
				int num4 = ((int)bytes3[2] * 256 + (int)bytes3[3]) ^ this.LaneAssistStartSpeedXor;
				byte b = (byte)((num4 >> 8) & 255);
				byte b2 = (byte)(num4 & 255);
				byte[] array2 = new byte[] { b, b2 };
				Array.Copy(array2, 0, array, this.LaneAssistStartSpeedXorAddress, array2.Length);
			}
			if (this.LightAssistSpeedConfigurationAvailable)
			{
				array[this.LightAssistOnAddress1] = (byte)(this.LightAssistOnSpeed & 255);
				array[this.LightAssistOnAddress2] = (byte)(this.LightAssistOnSpeed & 255);
				array[this.LightAssistOffAddress] = (byte)(this.LightAssistOffSpeed & 255);
			}
			byte[] array3 = Crc32.Calculate(array);
			Array.Reverse<byte>(array3);
			return array.Concat(array3).ToArray<byte>();
		}

		// Token: 0x060054B1 RID: 21681 RVA: 0x004042FC File Offset: 0x004024FC
		public bool Test()
		{
			byte[] array = this.Build();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != this.originalData[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040033AD RID: 13229
		public A5DatasetCustomizer.CameraVersions CameraVersion;

		// Token: 0x040033AE RID: 13230
		private byte[] originalData;

		// Token: 0x040033AF RID: 13231
		private int TJAAddress;

		// Token: 0x040033B0 RID: 13232
		private int LaneAssistTimerAddress;

		// Token: 0x040033B1 RID: 13233
		private int LaneAssistXorResultAddress;

		// Token: 0x040033B2 RID: 13234
		private int LaneAssistXor;

		// Token: 0x040033B3 RID: 13235
		private int LaneAssistStartSpeedAddress;

		// Token: 0x040033B4 RID: 13236
		private int LaneAssistStartSpeedXorAddress;

		// Token: 0x040033B5 RID: 13237
		private int LaneAssistStartSpeedXor;

		// Token: 0x040033B6 RID: 13238
		private int LightAssistOnAddress1;

		// Token: 0x040033B7 RID: 13239
		private int LightAssistOnAddress2;

		// Token: 0x040033B8 RID: 13240
		private int LightAssistOffAddress;

		// Token: 0x040033B9 RID: 13241
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040033BA RID: 13242
		private bool _LaneAssistTimerConfigurationAvailable;

		// Token: 0x040033BB RID: 13243
		private bool _TJAConfigurationAvailable;

		// Token: 0x040033BC RID: 13244
		private bool _TJA;

		// Token: 0x040033BD RID: 13245
		private int _LaneAssistTimer;

		// Token: 0x040033BE RID: 13246
		private bool _LaneAssistStartSpeedConfigurationAvailable;

		// Token: 0x040033BF RID: 13247
		private int _LaneAssistStartSpeed;

		// Token: 0x040033C0 RID: 13248
		private bool _LightAssistSpeedConfigurationAvailable;

		// Token: 0x040033C1 RID: 13249
		private int _LightAssistOnSpeed;

		// Token: 0x040033C2 RID: 13250
		private int _LightAssistOffSpeed;

		// Token: 0x02000A78 RID: 2680
		public enum CameraVersions
		{
			// Token: 0x040033C4 RID: 13252
			Unknown,
			// Token: 0x040033C5 RID: 13253
			H,
			// Token: 0x040033C6 RID: 13254
			L,
			// Token: 0x040033C7 RID: 13255
			F,
			// Token: 0x040033C8 RID: 13256
			G,
			// Token: 0x040033C9 RID: 13257
			S,
			// Token: 0x040033CA RID: 13258
			M
		}
	}
}
