using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AD8 RID: 2776
	public class FPAControl : INotifyPropertyChanged
	{
		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600572C RID: 22316 RVA: 0x00419052 File Offset: 0x00417252
		public string[] ControlValues
		{
			get
			{
				return FPAControl.controlValues.Value;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600572D RID: 22317 RVA: 0x0041905E File Offset: 0x0041725E
		// (set) Token: 0x0600572E RID: 22318 RVA: 0x00419066 File Offset: 0x00417266
		public bool SaveOnRestart
		{
			get
			{
				return this._SaveOnRestart;
			}
			set
			{
				this._SaveOnRestart = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("SaveOnRestart"));
			}
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x0041908C File Offset: 0x0041728C
		public static List<FPAControl> GetControlsForProfile(byte[] data)
		{
			List<FPAControl> list = new List<FPAControl>(30);
			MemoryStream memoryStream = new MemoryStream(data);
			memoryStream.Seek(1229L, SeekOrigin.Begin);
			for (int i = 0; i < 30; i++)
			{
				list.Add(new FPAControl((byte)memoryStream.ReadByte()));
			}
			return list;
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x004190D8 File Offset: 0x004172D8
		public static List<FPAControl> GetControlsForSavingAfterRestart(byte[] data)
		{
			List<FPAControl> list = new List<FPAControl>(30);
			MemoryStream memoryStream = new MemoryStream(data);
			memoryStream.Seek(2539L, SeekOrigin.Begin);
			for (int i = 0; i < 30; i++)
			{
				list.Add(new FPAControl((byte)memoryStream.ReadByte()));
			}
			memoryStream.Seek(722L, SeekOrigin.Begin);
			for (int j = 0; j < list.Count; j++)
			{
				int num = memoryStream.ReadByte();
				int num2 = memoryStream.ReadByte();
				list[j].Value = num2 * 256 + num;
				if (list[j].Value == 65279)
				{
					list[j].SaveOnRestart = true;
				}
				else
				{
					list[j].SaveOnRestart = false;
				}
			}
			return list;
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x00419198 File Offset: 0x00417398
		public byte[] GetSaveOnRestartBytes()
		{
			if (this.SaveOnRestart)
			{
				return new byte[] { byte.MaxValue, 254 };
			}
			byte b = (byte)(this.Value / 256);
			byte b2 = (byte)(this.Value % 256);
			return new byte[] { b2, b };
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x004191F0 File Offset: 0x004173F0
		public override string ToString()
		{
			return this.Title + " = " + this.Value.ToString("X2");
		}

		// Token: 0x06005733 RID: 22323 RVA: 0x00419220 File Offset: 0x00417420
		private FPAControl(byte b)
		{
			this.ControlType = b;
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06005734 RID: 22324 RVA: 0x0041922F File Offset: 0x0041742F
		// (set) Token: 0x06005735 RID: 22325 RVA: 0x00419237 File Offset: 0x00417437
		public byte ControlType
		{
			[CompilerGenerated]
			get
			{
				return this.<ControlType>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ControlType>k__BackingField = value;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x06005736 RID: 22326 RVA: 0x00419240 File Offset: 0x00417440
		public string Title
		{
			get
			{
				switch (this.ControlType)
				{
				case 0:
					return "not set";
				case 1:
					return "Engine";
				case 2:
					return "Start - stop system";
				case 3:
					return "Gearbox";
				case 4:
					return "Rear differential Lock";
				case 5:
					return "Steering";
				case 6:
					return "Progressive Steering";
				case 7:
					return "DCC";
				case 8:
					return "Airco";
				case 9:
					return "ACC";
				case 10:
					return "Interior Engine Sound";
				case 11:
					return "Motorway Light";
				case 12:
					return "Background Lighting";
				case 13:
					return "Air suspension";
				case 14:
					return "Automatic Belt pre - tensioning";
				case 15:
					return "Seat Bolster Setting";
				case 16:
					return "Route Option";
				case 17:
					return "Navigation";
				case 18:
					return "DSG Coasting";
				case 19:
					return "Eco tips";
				case 20:
					return "Exterior engine sound";
				case 21:
					return "Front differential lock";
				case 22:
					return "Center differential lock";
				case 23:
					return "Four - wheel drive";
				case 24:
					return "Electronic torque vectoring(Audi - Text)";
				case 25:
					return "Anti - slip regulation";
				case 26:
					return "Headlight control";
				case 27:
					return "Rear spoiler";
				case 28:
					return "ESC System";
				case 29:
					return "Rear Axle Steering";
				case 30:
					return "Adaptive body roll compens";
				case 31:
					return "Road recognitiion";
				case 32:
					return "Hybrid drive";
				case 33:
					return "Drive";
				case 34:
					return "Chassis";
				case 35:
					return "Exhaust valves";
				case 36:
					return "Engine Sound";
				case 37:
					return "Passenger Compartment";
				case 38:
					return "Driver's seat";
				case 39:
					return "Tyre pressure monitoring syst.";
				case 40:
					return "Lane Assist";
				case 41:
					return "Aggregatelagerung(Audi - Text)";
				case 42:
					return "Magnetic ride(Audi-Text)";
				case 43:
					return "Sport Select chassis";
				case 44:
					return "Hill Descent Assist";
				case 45:
					return "Hill Hold Assist";
				case 46:
					return "Parking assist";
				case 47:
					return "Instrument cluster";
				case 48:
					return "Infotainment system";
				case 49:
					return "Eco driving tips";
				case 50:
					return "Speed adjustment";
				case 51:
					return "Electronic engine sound";
				case 52:
					return "Nothing displays, just Engine";
				case 53:
					return "Nothing displays, just Engine";
				case 54:
					return "ESC System";
				case 55:
					return "Nothing displays";
				case 76:
					return "Freilauf_DefaultON";
				}
				return "Unknown [" + this.Value.ToString("X2") + "]";
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06005737 RID: 22327 RVA: 0x0041950B File Offset: 0x0041770B
		// (set) Token: 0x06005738 RID: 22328 RVA: 0x00419513 File Offset: 0x00417713
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06005739 RID: 22329 RVA: 0x00419538 File Offset: 0x00417738
		// (remove) Token: 0x0600573A RID: 22330 RVA: 0x00419570 File Offset: 0x00417770
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

		// Token: 0x0600573B RID: 22331 RVA: 0x004195A5 File Offset: 0x004177A5
		// Note: this type is marked as 'beforefieldinit'.
		static FPAControl()
		{
		}

		// Token: 0x040035B1 RID: 13745
		private static Lazy<string[]> controlValues = new Lazy<string[]>(delegate
		{
			string[] array = new string[256];
			for (int i = 0; i < array.Length; i++)
			{
				string text;
				switch (i)
				{
				case 0:
					text = "not set";
					break;
				case 1:
					text = "Comfort";
					break;
				case 2:
					text = "Normal";
					break;
				case 3:
					text = "Sport";
					break;
				case 4:
					text = "Off - road";
					break;
				case 5:
					text = "Eco";
					break;
				case 6:
					text = "Race";
					break;
				case 7:
					text = "Individual";
					break;
				case 8:
					text = "Config 8";
					break;
				case 9:
					text = "Config 9";
					break;
				case 10:
					text = "Config 10";
					break;
				case 11:
					text = "Config 11";
					break;
				case 12:
					text = "Config 12";
					break;
				default:
					text = "Config " + i.ToString();
					break;
				}
				array[i] = text;
			}
			return array;
		});

		// Token: 0x040035B2 RID: 13746
		private bool _SaveOnRestart;

		// Token: 0x040035B3 RID: 13747
		[CompilerGenerated]
		private byte <ControlType>k__BackingField;

		// Token: 0x040035B4 RID: 13748
		private int _Value;

		// Token: 0x040035B5 RID: 13749
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x02000AD9 RID: 2777
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600573C RID: 22332 RVA: 0x004195C1 File Offset: 0x004177C1
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600573D RID: 22333 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600573E RID: 22334 RVA: 0x004195D0 File Offset: 0x004177D0
			internal string[] <.cctor>b__25_0()
			{
				string[] array = new string[256];
				for (int i = 0; i < array.Length; i++)
				{
					string text;
					switch (i)
					{
					case 0:
						text = "not set";
						break;
					case 1:
						text = "Comfort";
						break;
					case 2:
						text = "Normal";
						break;
					case 3:
						text = "Sport";
						break;
					case 4:
						text = "Off - road";
						break;
					case 5:
						text = "Eco";
						break;
					case 6:
						text = "Race";
						break;
					case 7:
						text = "Individual";
						break;
					case 8:
						text = "Config 8";
						break;
					case 9:
						text = "Config 9";
						break;
					case 10:
						text = "Config 10";
						break;
					case 11:
						text = "Config 11";
						break;
					case 12:
						text = "Config 12";
						break;
					default:
						text = "Config " + i.ToString();
						break;
					}
					array[i] = text;
				}
				return array;
			}

			// Token: 0x040035B6 RID: 13750
			public static readonly FPAControl.<>c <>9 = new FPAControl.<>c();
		}
	}
}
