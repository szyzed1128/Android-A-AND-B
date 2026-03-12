using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008E0 RID: 2272
	internal class MQB_LightConfiguration : INotifyPropertyChanged
	{
		// Token: 0x06004CBE RID: 19646 RVA: 0x0038A552 File Offset: 0x00388752
		[JsonConstructor]
		public MQB_LightConfiguration()
		{
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0038A567 File Offset: 0x00388767
		public MQB_LightConfiguration(byte[] data)
			: this()
		{
			this.LoadFromData(data);
		}

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0038A576 File Offset: 0x00388776
		public MQB_LightConfiguration(string hex_data)
			: this()
		{
			this.LoadFromData(hex_data);
		}

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06004CC1 RID: 19649 RVA: 0x0038A588 File Offset: 0x00388788
		// (remove) Token: 0x06004CC2 RID: 19650 RVA: 0x0038A5C0 File Offset: 0x003887C0
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

		// Token: 0x06004CC3 RID: 19651 RVA: 0x0038A5F5 File Offset: 0x003887F5
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0038A60E File Offset: 0x0038880E
		public static string[] LightControlList
		{
			get
			{
				return new string[]
				{
					Translate.GetString("coding_LightAlways"),
					Translate.GetString("coding_IfBootIsClosed")
				};
			}
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0038A630 File Offset: 0x00388830
		public static string[] DimmingDirectionList
		{
			get
			{
				return new string[]
				{
					Translate.GetString("coding_Maximize"),
					Translate.GetString("coding_Minimize")
				};
			}
		}

		// Token: 0x17001754 RID: 5972
		// (get) Token: 0x06004CC6 RID: 19654 RVA: 0x0038A652 File Offset: 0x00388852
		// (set) Token: 0x06004CC7 RID: 19655 RVA: 0x0038A65A File Offset: 0x0038885A
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x17001755 RID: 5973
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x0038A663 File Offset: 0x00388863
		// (set) Token: 0x06004CC9 RID: 19657 RVA: 0x0038A66B File Offset: 0x0038886B
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x06004CCA RID: 19658 RVA: 0x0038A674 File Offset: 0x00388874
		// (set) Token: 0x06004CCB RID: 19659 RVA: 0x0038A67C File Offset: 0x0038887C
		public MQB_LampType LampType
		{
			get
			{
				return this._LampType;
			}
			set
			{
				this._LampType = value;
				this.OnPropertyChanged("LampType");
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x06004CCC RID: 19660 RVA: 0x0038A690 File Offset: 0x00388890
		// (set) Token: 0x06004CCD RID: 19661 RVA: 0x0038A698 File Offset: 0x00388898
		public MQB_LightFunction FunctionA
		{
			get
			{
				return this._FunctionA;
			}
			set
			{
				this._FunctionA = value;
				this.OnPropertyChanged("FunctionA");
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x06004CCE RID: 19662 RVA: 0x0038A6AC File Offset: 0x003888AC
		// (set) Token: 0x06004CCF RID: 19663 RVA: 0x0038A6B4 File Offset: 0x003888B4
		public MQB_LightFunction FunctionB
		{
			get
			{
				return this._FunctionB;
			}
			set
			{
				this._FunctionB = value;
				this.OnPropertyChanged("FunctionB");
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x06004CD0 RID: 19664 RVA: 0x0038A6C8 File Offset: 0x003888C8
		// (set) Token: 0x06004CD1 RID: 19665 RVA: 0x0038A6D0 File Offset: 0x003888D0
		public int DimmwertAB
		{
			get
			{
				return this._DimmwertAB;
			}
			set
			{
				if (value > 127)
				{
					value = 127;
				}
				if (value < 0)
				{
					value = 0;
				}
				this._DimmwertAB = value;
				this.OnPropertyChanged("DimmwertAB");
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x06004CD2 RID: 19666 RVA: 0x0038A6F4 File Offset: 0x003888F4
		// (set) Token: 0x06004CD3 RID: 19667 RVA: 0x0038A6FC File Offset: 0x003888FC
		public MQB_LightConfiguration.LightControl LightControlAB
		{
			get
			{
				return this._LightControlAB;
			}
			set
			{
				this._LightControlAB = value;
				this.OnPropertyChanged("LightControlAB");
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x06004CD4 RID: 19668 RVA: 0x0038A710 File Offset: 0x00388910
		// (set) Token: 0x06004CD5 RID: 19669 RVA: 0x0038A718 File Offset: 0x00388918
		public MQB_LightFunction FunctionC
		{
			get
			{
				return this._FunctionC;
			}
			set
			{
				this._FunctionC = value;
				this.OnPropertyChanged("FunctionC");
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06004CD6 RID: 19670 RVA: 0x0038A72C File Offset: 0x0038892C
		// (set) Token: 0x06004CD7 RID: 19671 RVA: 0x0038A734 File Offset: 0x00388934
		public MQB_LightFunction FunctionD
		{
			get
			{
				return this._FunctionD;
			}
			set
			{
				this._FunctionD = value;
				this.OnPropertyChanged("FunctionD");
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06004CD8 RID: 19672 RVA: 0x0038A748 File Offset: 0x00388948
		// (set) Token: 0x06004CD9 RID: 19673 RVA: 0x0038A750 File Offset: 0x00388950
		public int DimmwertCD
		{
			get
			{
				return this._DimmwertCD;
			}
			set
			{
				if (value > 127)
				{
					value = 127;
				}
				if (value < 0)
				{
					value = 0;
				}
				this._DimmwertCD = value;
				this.OnPropertyChanged("DimmwertCD");
			}
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06004CDA RID: 19674 RVA: 0x0038A774 File Offset: 0x00388974
		// (set) Token: 0x06004CDB RID: 19675 RVA: 0x0038A77C File Offset: 0x0038897C
		public MQB_LightConfiguration.DimmingDirection DimmingDirectionCD
		{
			get
			{
				return this._DimmingDirectionCD;
			}
			set
			{
				this._DimmingDirectionCD = value;
				this.OnPropertyChanged("DimmingDirectionCD");
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x06004CDC RID: 19676 RVA: 0x0038A790 File Offset: 0x00388990
		// (set) Token: 0x06004CDD RID: 19677 RVA: 0x0038A798 File Offset: 0x00388998
		public MQB_LightFunction FunctionE
		{
			get
			{
				return this._FunctionE;
			}
			set
			{
				this._FunctionE = value;
				this.OnPropertyChanged("FunctionE");
			}
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06004CDE RID: 19678 RVA: 0x0038A7AC File Offset: 0x003889AC
		// (set) Token: 0x06004CDF RID: 19679 RVA: 0x0038A7B4 File Offset: 0x003889B4
		public MQB_LightFunction FunctionF
		{
			get
			{
				return this._FunctionF;
			}
			set
			{
				this._FunctionF = value;
				this.OnPropertyChanged("FunctionF");
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x06004CE0 RID: 19680 RVA: 0x0038A7C8 File Offset: 0x003889C8
		// (set) Token: 0x06004CE1 RID: 19681 RVA: 0x0038A7D0 File Offset: 0x003889D0
		public int DimmwertEF
		{
			get
			{
				return this._DimmwertEF;
			}
			set
			{
				if (value > 127)
				{
					value = 127;
				}
				if (value < 0)
				{
					value = 0;
				}
				this._DimmwertEF = value;
				this.OnPropertyChanged("DimmwertEF");
			}
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x06004CE2 RID: 19682 RVA: 0x0038A7F4 File Offset: 0x003889F4
		// (set) Token: 0x06004CE3 RID: 19683 RVA: 0x0038A7FC File Offset: 0x003889FC
		public MQB_LightConfiguration.DimmingDirection DimmingDirectionEF
		{
			get
			{
				return this._DimmingDirectionEF;
			}
			set
			{
				this._DimmingDirectionEF = value;
				this.OnPropertyChanged("DimmingDirectionCD");
			}
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x0038A810 File Offset: 0x00388A10
		// (set) Token: 0x06004CE5 RID: 19685 RVA: 0x0038A818 File Offset: 0x00388A18
		public MQB_LightFunction FunctionG
		{
			get
			{
				return this._FunctionG;
			}
			set
			{
				this._FunctionG = value;
				this.OnPropertyChanged("FunctionG");
			}
		}

		// Token: 0x17001764 RID: 5988
		// (get) Token: 0x06004CE6 RID: 19686 RVA: 0x0038A82C File Offset: 0x00388A2C
		// (set) Token: 0x06004CE7 RID: 19687 RVA: 0x0038A834 File Offset: 0x00388A34
		public MQB_LightFunction FunctionH
		{
			get
			{
				return this._FunctionH;
			}
			set
			{
				this._FunctionH = value;
				this.OnPropertyChanged("FunctionH");
			}
		}

		// Token: 0x17001765 RID: 5989
		// (get) Token: 0x06004CE8 RID: 19688 RVA: 0x0038A848 File Offset: 0x00388A48
		// (set) Token: 0x06004CE9 RID: 19689 RVA: 0x0038A850 File Offset: 0x00388A50
		public int DimmwertGH
		{
			get
			{
				return this._DimmwertGH;
			}
			set
			{
				if (value > 127)
				{
					value = 127;
				}
				if (value < 0)
				{
					value = 0;
				}
				this._DimmwertGH = value;
				this.OnPropertyChanged("DimmwertGH");
			}
		}

		// Token: 0x17001766 RID: 5990
		// (get) Token: 0x06004CEA RID: 19690 RVA: 0x0038A874 File Offset: 0x00388A74
		// (set) Token: 0x06004CEB RID: 19691 RVA: 0x0038A87C File Offset: 0x00388A7C
		public MQB_LightConfiguration.DimmingDirection DimmingDirectionGH
		{
			get
			{
				return this._DimmingDirectionGH;
			}
			set
			{
				this._DimmingDirectionGH = value;
				this.OnPropertyChanged("DimmingDirectionGH");
			}
		}

		// Token: 0x17001767 RID: 5991
		// (get) Token: 0x06004CEC RID: 19692 RVA: 0x0038A890 File Offset: 0x00388A90
		// (set) Token: 0x06004CED RID: 19693 RVA: 0x0038A898 File Offset: 0x00388A98
		public byte LampenDefectBitPosition
		{
			[CompilerGenerated]
			get
			{
				return this.<LampenDefectBitPosition>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LampenDefectBitPosition>k__BackingField = value;
			}
		}

		// Token: 0x17001768 RID: 5992
		// (get) Token: 0x06004CEE RID: 19694 RVA: 0x0038A8A1 File Offset: 0x00388AA1
		// (set) Token: 0x06004CEF RID: 19695 RVA: 0x0038A8A9 File Offset: 0x00388AA9
		public byte FehlerortMittleresByteDTC_DFCC
		{
			[CompilerGenerated]
			get
			{
				return this.<FehlerortMittleresByteDTC_DFCC>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FehlerortMittleresByteDTC_DFCC>k__BackingField = value;
			}
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x0038A8B4 File Offset: 0x00388AB4
		public void LoadFromData(string hex_data)
		{
			byte[] array = BitHelpers.ConvertHexToBytesX(hex_data);
			this.LoadFromData(array);
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x0038A8D0 File Offset: 0x00388AD0
		public void LoadFromData(byte[] data)
		{
			this.LampType = MQB_LampType.FromValue(data[0]);
			this.LampenDefectBitPosition = data[1];
			this.FehlerortMittleresByteDTC_DFCC = data[3];
			this.FunctionA = MQB_LightFunction.FromValue(data[4]);
			this.FunctionB = MQB_LightFunction.FromValue(data[5]);
			if (BitHelpers.GetBit_0_7(data[6], 7))
			{
				this.LightControlAB = MQB_LightConfiguration.LightControl.IfBootIsClosed;
			}
			else
			{
				this.LightControlAB = MQB_LightConfiguration.LightControl.Always;
			}
			this.DimmwertAB = (int)(data[6] & 127);
			this.FunctionC = MQB_LightFunction.FromValue(data[7]);
			this.FunctionD = MQB_LightFunction.FromValue(data[8]);
			if (BitHelpers.GetBit_0_7(data[9], 7))
			{
				this.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Minimize;
			}
			else
			{
				this.DimmingDirectionCD = MQB_LightConfiguration.DimmingDirection.Maximize;
			}
			this.DimmwertCD = (int)(data[9] & 127);
			this.FunctionE = MQB_LightFunction.FromValue(data[10]);
			this.FunctionF = MQB_LightFunction.FromValue(data[11]);
			if (BitHelpers.GetBit_0_7(data[12], 7))
			{
				this.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Minimize;
			}
			else
			{
				this.DimmingDirectionEF = MQB_LightConfiguration.DimmingDirection.Maximize;
			}
			this.DimmwertEF = (int)(data[12] & 127);
			this.FunctionG = MQB_LightFunction.FromValue(data[13]);
			this.FunctionH = MQB_LightFunction.FromValue(data[14]);
			if (BitHelpers.GetBit_0_7(data[15], 7))
			{
				this.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Minimize;
			}
			else
			{
				this.DimmingDirectionGH = MQB_LightConfiguration.DimmingDirection.Maximize;
			}
			this.DimmwertGH = (int)(data[15] & 127);
			this.OriginalData = data;
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x0038AA1C File Offset: 0x00388C1C
		public byte[] ApplyToData(byte[] data)
		{
			byte[] array;
			try
			{
				if (data == null)
				{
					array = null;
				}
				else
				{
					byte[] array2 = new byte[data.Length];
					Array.Copy(data, array2, data.Length);
					array2[0] = this.LampType.GetByteValue();
					array2[1] = this.LampenDefectBitPosition;
					array2[3] = this.FehlerortMittleresByteDTC_DFCC;
					array2[4] = this.FunctionA.ApplyValueToByte(array2[4]);
					array2[5] = this.FunctionB.ApplyValueToByte(array2[5]);
					array2[6] = BitHelpers.SetBit((byte)this.DimmwertAB, 7, this.LightControlAB > MQB_LightConfiguration.LightControl.Always);
					array2[7] = this.FunctionC.ApplyValueToByte(array2[7]);
					array2[8] = this.FunctionD.ApplyValueToByte(array2[8]);
					array2[9] = BitHelpers.SetBit((byte)this.DimmwertCD, 7, this.DimmingDirectionCD > MQB_LightConfiguration.DimmingDirection.Maximize);
					array2[10] = this.FunctionE.ApplyValueToByte(array2[10]);
					array2[11] = this.FunctionF.ApplyValueToByte(array2[11]);
					array2[12] = BitHelpers.SetBit((byte)this.DimmwertEF, 7, this.DimmingDirectionEF > MQB_LightConfiguration.DimmingDirection.Maximize);
					array2[13] = this.FunctionG.ApplyValueToByte(array2[13]);
					array2[14] = this.FunctionH.ApplyValueToByte(array2[14]);
					array2[15] = BitHelpers.SetBit((byte)this.DimmwertGH, 7, this.DimmingDirectionGH > MQB_LightConfiguration.DimmingDirection.Maximize);
					array = array2;
				}
			}
			catch (NullReferenceException)
			{
				array = data;
			}
			return array;
		}

		// Token: 0x06004CF3 RID: 19699 RVA: 0x0038AB84 File Offset: 0x00388D84
		public byte[] ApplyToData()
		{
			return this.ApplyToData(this.OriginalData);
		}

		// Token: 0x06004CF4 RID: 19700 RVA: 0x0038AB94 File Offset: 0x00388D94
		public static string GetTextDifference(string address, string oldData, string newData)
		{
			byte[] array = BitHelpers.ConvertHexToBytesX(oldData);
			byte[] array2 = BitHelpers.ConvertHexToBytesX(newData);
			return MQB_LightConfiguration.GetTextDifference(address, array, array2);
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x0038ABB8 File Offset: 0x00388DB8
		public static string GetTextDifference(string address, byte[] oldData, byte[] newData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "";
			string text2 = SharedSettings.Current.CodingLastPlatformSelected;
			if (!(text2 == "PQ26NEWRAPID2020") && !(text2 == "PQ26") && !(text2 == "FABIANJ"))
			{
				if (text2 == "MQB")
				{
					MQB_LightConfigurationCoding mqb_LightConfigurationCoding = MQB_LightConfigurationCoding.GetLightConfigurationMQB().FirstOrDefault((MQB_LightConfigurationCoding x) => x.Address == address);
					if (mqb_LightConfigurationCoding != null)
					{
						text = mqb_LightConfigurationCoding.Name;
					}
				}
			}
			else
			{
				MQB_LightConfigurationCoding mqb_LightConfigurationCoding2 = MQB_LightConfigurationCoding.GetLightConfigurationPQ26().FirstOrDefault((MQB_LightConfigurationCoding x) => x.Address == address);
				if (mqb_LightConfigurationCoding2 != null)
				{
					text = mqb_LightConfigurationCoding2.Name;
				}
			}
			stringBuilder.AppendLine(text);
			MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(oldData);
			MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(newData);
			IEnumerable<string> enumerable = new string[]
			{
				"FunctionA", "FunctionB", "DimmwertAB", "LightControlAB", "FunctionC", "FunctionD", "DimmwertCD", "DimmingDirectionCD", "FunctionE", "FunctionF",
				"DimmwertEF", "DimmingDirectionEF", "FunctionG", "FunctionH", "DimmwertGH", "DimmingDirectionGH"
			};
			Type t = mqb_LightConfiguration.GetType();
			foreach (PropertyInfo propertyInfo in enumerable.Select((string x) => t.GetProperty(x)).ToArray<PropertyInfo>())
			{
				object value = propertyInfo.GetValue(mqb_LightConfiguration);
				object value2 = propertyInfo.GetValue(mqb_LightConfiguration2);
				text2 = propertyInfo.Name;
				if (text2 != null)
				{
					int length = text2.Length;
					if (length <= 10)
					{
						if (length != 9)
						{
							if (length == 10)
							{
								switch (text2[8])
								{
								case 'A':
									if (!(text2 == "DimmwertAB"))
									{
										goto IL_0506;
									}
									break;
								case 'B':
								case 'D':
								case 'F':
									goto IL_0506;
								case 'C':
									if (!(text2 == "DimmwertCD"))
									{
										goto IL_0506;
									}
									break;
								case 'E':
									if (!(text2 == "DimmwertEF"))
									{
										goto IL_0506;
									}
									break;
								case 'G':
									if (!(text2 == "DimmwertGH"))
									{
										goto IL_0506;
									}
									break;
								default:
									goto IL_0506;
								}
								int num = (int)value;
								int num2 = (int)value2;
								if (num != num2)
								{
									stringBuilder.AppendLine(string.Format("{0}: {1} -> {2}", propertyInfo.Name, num, num2));
								}
							}
						}
						else
						{
							switch (text2[8])
							{
							case 'A':
								if (!(text2 == "FunctionA"))
								{
									goto IL_0506;
								}
								break;
							case 'B':
								if (!(text2 == "FunctionB"))
								{
									goto IL_0506;
								}
								break;
							case 'C':
								if (!(text2 == "FunctionC"))
								{
									goto IL_0506;
								}
								break;
							case 'D':
								if (!(text2 == "FunctionD"))
								{
									goto IL_0506;
								}
								break;
							case 'E':
								if (!(text2 == "FunctionE"))
								{
									goto IL_0506;
								}
								break;
							case 'F':
								if (!(text2 == "FunctionF"))
								{
									goto IL_0506;
								}
								break;
							case 'G':
								if (!(text2 == "FunctionG"))
								{
									goto IL_0506;
								}
								break;
							case 'H':
								if (!(text2 == "FunctionH"))
								{
									goto IL_0506;
								}
								break;
							default:
								goto IL_0506;
							}
							MQB_LightFunction mqb_LightFunction = (MQB_LightFunction)value;
							MQB_LightFunction mqb_LightFunction2 = (MQB_LightFunction)value2;
							if (mqb_LightFunction.Value != mqb_LightFunction2.Value)
							{
								stringBuilder.AppendLine(string.Concat(new string[] { propertyInfo.Name, ": ", mqb_LightFunction.Title, " -> ", mqb_LightFunction2.Title }));
							}
						}
					}
					else if (length != 14)
					{
						if (length == 18)
						{
							switch (text2[16])
							{
							case 'C':
								if (!(text2 == "DimmingDirectionCD"))
								{
									goto IL_0506;
								}
								break;
							case 'D':
							case 'F':
								goto IL_0506;
							case 'E':
								if (!(text2 == "DimmingDirectionEF"))
								{
									goto IL_0506;
								}
								break;
							case 'G':
								if (!(text2 == "DimmingDirectionGH"))
								{
									goto IL_0506;
								}
								break;
							default:
								goto IL_0506;
							}
							MQB_LightConfiguration.DimmingDirection dimmingDirection = (MQB_LightConfiguration.DimmingDirection)value;
							MQB_LightConfiguration.DimmingDirection dimmingDirection2 = (MQB_LightConfiguration.DimmingDirection)value2;
							if (dimmingDirection != dimmingDirection2)
							{
								stringBuilder.AppendLine(string.Format("{0}: {1} -> {2}", propertyInfo.Name, dimmingDirection, dimmingDirection2));
							}
						}
					}
					else if (text2 == "LightControlAB")
					{
						MQB_LightConfiguration.LightControl lightControl = (MQB_LightConfiguration.LightControl)value;
						MQB_LightConfiguration.LightControl lightControl2 = (MQB_LightConfiguration.LightControl)value2;
						if (lightControl != lightControl2)
						{
							stringBuilder.AppendLine(string.Format("{0}: {1} -> {2}", propertyInfo.Name, lightControl, lightControl2));
						}
					}
				}
				IL_0506:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002D54 RID: 11604
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002D55 RID: 11605
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x04002D56 RID: 11606
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x04002D57 RID: 11607
		private MQB_LampType _LampType;

		// Token: 0x04002D58 RID: 11608
		public MQB_LightFunction _FunctionA;

		// Token: 0x04002D59 RID: 11609
		public MQB_LightFunction _FunctionB;

		// Token: 0x04002D5A RID: 11610
		private int _DimmwertAB;

		// Token: 0x04002D5B RID: 11611
		private MQB_LightConfiguration.LightControl _LightControlAB;

		// Token: 0x04002D5C RID: 11612
		public MQB_LightFunction _FunctionC;

		// Token: 0x04002D5D RID: 11613
		public MQB_LightFunction _FunctionD;

		// Token: 0x04002D5E RID: 11614
		private int _DimmwertCD;

		// Token: 0x04002D5F RID: 11615
		private MQB_LightConfiguration.DimmingDirection _DimmingDirectionCD;

		// Token: 0x04002D60 RID: 11616
		public MQB_LightFunction _FunctionE;

		// Token: 0x04002D61 RID: 11617
		public MQB_LightFunction _FunctionF;

		// Token: 0x04002D62 RID: 11618
		private int _DimmwertEF;

		// Token: 0x04002D63 RID: 11619
		private MQB_LightConfiguration.DimmingDirection _DimmingDirectionEF;

		// Token: 0x04002D64 RID: 11620
		public MQB_LightFunction _FunctionG;

		// Token: 0x04002D65 RID: 11621
		public MQB_LightFunction _FunctionH;

		// Token: 0x04002D66 RID: 11622
		private int _DimmwertGH;

		// Token: 0x04002D67 RID: 11623
		private MQB_LightConfiguration.DimmingDirection _DimmingDirectionGH;

		// Token: 0x04002D68 RID: 11624
		[CompilerGenerated]
		private byte <LampenDefectBitPosition>k__BackingField;

		// Token: 0x04002D69 RID: 11625
		[CompilerGenerated]
		private byte <FehlerortMittleresByteDTC_DFCC>k__BackingField;

		// Token: 0x04002D6A RID: 11626
		private byte[] OriginalData = new byte[16];

		// Token: 0x020008E1 RID: 2273
		public enum LightControl
		{
			// Token: 0x04002D6C RID: 11628
			Always,
			// Token: 0x04002D6D RID: 11629
			IfBootIsClosed
		}

		// Token: 0x020008E2 RID: 2274
		public enum DimmingDirection
		{
			// Token: 0x04002D6F RID: 11631
			Maximize,
			// Token: 0x04002D70 RID: 11632
			Minimize
		}

		// Token: 0x020008E3 RID: 2275
		[CompilerGenerated]
		private sealed class <>c__DisplayClass103_0
		{
			// Token: 0x06004CF6 RID: 19702 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass103_0()
			{
			}

			// Token: 0x06004CF7 RID: 19703 RVA: 0x0038B0E2 File Offset: 0x003892E2
			internal bool <GetTextDifference>b__1(MQB_LightConfigurationCoding x)
			{
				return x.Address == this.address;
			}

			// Token: 0x06004CF8 RID: 19704 RVA: 0x0038B0E2 File Offset: 0x003892E2
			internal bool <GetTextDifference>b__2(MQB_LightConfigurationCoding x)
			{
				return x.Address == this.address;
			}

			// Token: 0x06004CF9 RID: 19705 RVA: 0x0038B0F5 File Offset: 0x003892F5
			internal PropertyInfo <GetTextDifference>b__0(string x)
			{
				return this.t.GetProperty(x);
			}

			// Token: 0x04002D71 RID: 11633
			public string address;

			// Token: 0x04002D72 RID: 11634
			public Type t;
		}
	}
}
