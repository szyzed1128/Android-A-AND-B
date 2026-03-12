using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AD6 RID: 2774
	public class FPAProfile : INotifyPropertyChanged
	{
		// Token: 0x06005718 RID: 22296 RVA: 0x00418A03 File Offset: 0x00416C03
		public override string ToString()
		{
			return string.Format("[{0}] {1}", this.Index, this.Title);
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x00418A20 File Offset: 0x00416C20
		public void ApplyToBytes(byte[] data)
		{
			data[825 + this.Index] = this.ReturnAfterRestart;
			int num = 869 + this.Index * 30;
			for (int i = 0; i < this.Controls.Count; i++)
			{
				data[num + i] = (byte)this.Controls[i].Value;
			}
		}

		// Token: 0x14000064 RID: 100
		// (add) Token: 0x0600571A RID: 22298 RVA: 0x00418A80 File Offset: 0x00416C80
		// (remove) Token: 0x0600571B RID: 22299 RVA: 0x00418AB8 File Offset: 0x00416CB8
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

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x0600571C RID: 22300 RVA: 0x00418AED File Offset: 0x00416CED
		// (set) Token: 0x0600571D RID: 22301 RVA: 0x00418AF5 File Offset: 0x00416CF5
		public int Index
		{
			[CompilerGenerated]
			get
			{
				return this.<Index>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Index>k__BackingField = value;
			}
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x00418B00 File Offset: 0x00416D00
		public FPAProfile(int idx, byte[] data)
		{
			this.Index = idx;
			this.Value = data[813 + idx];
			this.ReturnAfterRestart = data[825 + idx];
			this.Controls = FPAControl.GetControlsForProfile(data);
			int num = 869 + idx * 30;
			byte[] array = data.Skip(num).Take(30).ToArray<byte>();
			for (int i = 0; i < 30; i++)
			{
				this.Controls[i].Value = (int)array[i];
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x0600571F RID: 22303 RVA: 0x00418B84 File Offset: 0x00416D84
		// (set) Token: 0x06005720 RID: 22304 RVA: 0x00418B8C File Offset: 0x00416D8C
		public byte Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06005721 RID: 22305 RVA: 0x00418B98 File Offset: 0x00416D98
		public string Title
		{
			get
			{
				string text = this.Value.ToString("X2");
				byte value = this.Value;
				string text2;
				switch (value)
				{
				case 0:
					text2 = "not set";
					goto IL_01ED;
				case 1:
					text2 = "Comfort";
					goto IL_01ED;
				case 2:
					text2 = "Auto / Normal";
					goto IL_01ED;
				case 3:
					text2 = "Dynamic / Sport";
					goto IL_01ED;
				case 4:
					text2 = "Offroad";
					goto IL_01ED;
				case 5:
					text2 = "Eco / Economy";
					goto IL_01ED;
				case 6:
					text2 = "Race / Cupra";
					goto IL_01ED;
				case 7:
					text2 = "Individual";
					goto IL_01ED;
				case 8:
					text2 = "Range / PHEV 1";
					goto IL_01ED;
				case 9:
					text2 = "Lift / PHEV 2";
					goto IL_01ED;
				case 10:
					text2 = "Offroad Snow/ PHEV 3 / EV On";
					goto IL_01ED;
				case 11:
					text2 = "Offroad individual/ PHEV 4 / Hybrid auto";
					goto IL_01ED;
				case 12:
					text2 = "Offroad 4 / Hybrid hold";
					goto IL_01ED;
				case 13:
					text2 = "Offroad 5 / Hybrid charge";
					goto IL_01ED;
				case 14:
					text2 = "Offroad 6 / Hybrid area";
					goto IL_01ED;
				case 15:
					text2 = "EV Off";
					goto IL_01ED;
				case 16:
					text2 = "Second hold/ Torque Vectoring";
					goto IL_01ED;
				case 17:
					text2 = "Racetrack / Hybrid Charge Off";
					goto IL_01ED;
				case 18:
					text2 = "Offroad level 2 / Adaptive";
					goto IL_01ED;
				case 19:
					text2 = "Offroad level 3 / Traction";
					goto IL_01ED;
				case 20:
					text2 = "Offroad level 4";
					goto IL_01ED;
				case 21:
					text2 = "Hybrid sport";
					goto IL_01ED;
				case 22:
					text2 = "Second auto";
					goto IL_01ED;
				case 23:
					text2 = "Unknown profile";
					goto IL_01ED;
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
					break;
				case 40:
					text2 = "GTE off";
					goto IL_01ED;
				case 41:
					text2 = "GTE off 2";
					goto IL_01ED;
				case 42:
					text2 = "Unknown profile 2";
					goto IL_01ED;
				default:
					if (value == 255)
					{
						text2 = "none";
						goto IL_01ED;
					}
					break;
				}
				text2 = "Unknown";
				IL_01ED:
				return text2 + " [0x" + text + "]";
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06005722 RID: 22306 RVA: 0x00418DA3 File Offset: 0x00416FA3
		public string[] ReturnToValues
		{
			get
			{
				return FPAProfile.returnToValues.Value;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06005723 RID: 22307 RVA: 0x00418DAF File Offset: 0x00416FAF
		public string IndexAndTitle
		{
			get
			{
				return string.Format("{0}) {1}", this.Index + 1, this.Title);
			}
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x06005724 RID: 22308 RVA: 0x00418DCE File Offset: 0x00416FCE
		// (set) Token: 0x06005725 RID: 22309 RVA: 0x00418DD6 File Offset: 0x00416FD6
		public byte ReturnAfterRestart
		{
			get
			{
				return this._ReturnAfterRestart;
			}
			set
			{
				this._ReturnAfterRestart = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("ReturnAfterRestart"));
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x06005726 RID: 22310 RVA: 0x00418DFA File Offset: 0x00416FFA
		// (set) Token: 0x06005727 RID: 22311 RVA: 0x00418E02 File Offset: 0x00417002
		public List<FPAControl> Controls
		{
			[CompilerGenerated]
			get
			{
				return this.<Controls>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Controls>k__BackingField = value;
			}
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x00418E0B File Offset: 0x0041700B
		// Note: this type is marked as 'beforefieldinit'.
		static FPAProfile()
		{
		}

		// Token: 0x040035AA RID: 13738
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040035AB RID: 13739
		[CompilerGenerated]
		private int <Index>k__BackingField;

		// Token: 0x040035AC RID: 13740
		[CompilerGenerated]
		private byte <Value>k__BackingField;

		// Token: 0x040035AD RID: 13741
		private static Lazy<string[]> returnToValues = new Lazy<string[]>(delegate
		{
			string[] array = new string[256];
			int i = 0;
			while (i < array.Length)
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
					text = "Auto / Normal";
					break;
				case 3:
					text = "Dynamic / Sport";
					break;
				case 4:
					text = "Offroad";
					break;
				case 5:
					text = "Eco / Economy";
					break;
				case 6:
					text = "Race / Cupra";
					break;
				case 7:
					text = "Individual";
					break;
				case 8:
					text = "Range / PHEV 1";
					break;
				case 9:
					text = "Lift / PHEV 2";
					break;
				case 10:
					text = "Offroad Snow/ PHEV 3 / EV On";
					break;
				case 11:
					text = "Offroad individual/ PHEV 4 / Hybrid auto";
					break;
				case 12:
					text = "Offroad 4 / Hybrid hold";
					break;
				case 13:
					text = "Offroad 5 / Hybrid charge";
					break;
				case 14:
					text = "Offroad 6 / Hybrid area";
					break;
				case 15:
					text = "EV Off";
					break;
				case 16:
					text = "Second hold/ Torque Vectoring";
					break;
				case 17:
					text = "Racetrack / Hybrid Charge Off";
					break;
				case 18:
					text = "Offroad level 2 / Adaptive";
					break;
				case 19:
					text = "Offroad level 3 / Traction";
					break;
				case 20:
					text = "Offroad level 4";
					break;
				case 21:
					text = "Hybrid sport";
					break;
				case 22:
					text = "Second auto";
					break;
				case 23:
					text = "Unknown profile";
					break;
				case 24:
				case 25:
				case 26:
				case 27:
				case 28:
				case 29:
				case 30:
				case 31:
				case 32:
				case 33:
				case 34:
				case 35:
				case 36:
				case 37:
				case 38:
				case 39:
					goto IL_01DE;
				case 40:
					text = "GTE off";
					break;
				case 41:
					text = "GTE off 2";
					break;
				case 42:
					text = "Unknown profile 2";
					break;
				default:
					if (i != 255)
					{
						goto IL_01DE;
					}
					text = "none";
					break;
				}
				IL_01E4:
				array[i] = "[" + i.ToString("X2") + "] " + text;
				i++;
				continue;
				IL_01DE:
				text = "Unknown";
				goto IL_01E4;
			}
			return array;
		});

		// Token: 0x040035AE RID: 13742
		private byte _ReturnAfterRestart;

		// Token: 0x040035AF RID: 13743
		[CompilerGenerated]
		private List<FPAControl> <Controls>k__BackingField;

		// Token: 0x02000AD7 RID: 2775
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005729 RID: 22313 RVA: 0x00418E27 File Offset: 0x00417027
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600572A RID: 22314 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600572B RID: 22315 RVA: 0x00418E34 File Offset: 0x00417034
			internal string[] <.cctor>b__29_0()
			{
				string[] array = new string[256];
				int i = 0;
				while (i < array.Length)
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
						text = "Auto / Normal";
						break;
					case 3:
						text = "Dynamic / Sport";
						break;
					case 4:
						text = "Offroad";
						break;
					case 5:
						text = "Eco / Economy";
						break;
					case 6:
						text = "Race / Cupra";
						break;
					case 7:
						text = "Individual";
						break;
					case 8:
						text = "Range / PHEV 1";
						break;
					case 9:
						text = "Lift / PHEV 2";
						break;
					case 10:
						text = "Offroad Snow/ PHEV 3 / EV On";
						break;
					case 11:
						text = "Offroad individual/ PHEV 4 / Hybrid auto";
						break;
					case 12:
						text = "Offroad 4 / Hybrid hold";
						break;
					case 13:
						text = "Offroad 5 / Hybrid charge";
						break;
					case 14:
						text = "Offroad 6 / Hybrid area";
						break;
					case 15:
						text = "EV Off";
						break;
					case 16:
						text = "Second hold/ Torque Vectoring";
						break;
					case 17:
						text = "Racetrack / Hybrid Charge Off";
						break;
					case 18:
						text = "Offroad level 2 / Adaptive";
						break;
					case 19:
						text = "Offroad level 3 / Traction";
						break;
					case 20:
						text = "Offroad level 4";
						break;
					case 21:
						text = "Hybrid sport";
						break;
					case 22:
						text = "Second auto";
						break;
					case 23:
						text = "Unknown profile";
						break;
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
					case 30:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
						goto IL_01DE;
					case 40:
						text = "GTE off";
						break;
					case 41:
						text = "GTE off 2";
						break;
					case 42:
						text = "Unknown profile 2";
						break;
					default:
						if (i != 255)
						{
							goto IL_01DE;
						}
						text = "none";
						break;
					}
					IL_01E4:
					array[i] = "[" + i.ToString("X2") + "] " + text;
					i++;
					continue;
					IL_01DE:
					text = "Unknown";
					goto IL_01E4;
				}
				return array;
			}

			// Token: 0x040035B0 RID: 13744
			public static readonly FPAProfile.<>c <>9 = new FPAProfile.<>c();
		}
	}
}
