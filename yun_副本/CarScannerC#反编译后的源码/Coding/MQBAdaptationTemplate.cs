using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200088C RID: 2188
	public class MQBAdaptationTemplate : ICodingContainerWithStringAddress, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06004A63 RID: 19043 RVA: 0x0037D994 File Offset: 0x0037BB94
		// (set) Token: 0x06004A64 RID: 19044 RVA: 0x0037D99C File Offset: 0x0037BB9C
		protected virtual string READ_MODE
		{
			[CompilerGenerated]
			get
			{
				return this.<READ_MODE>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<READ_MODE>k__BackingField = value;
			}
		} = "22";

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x06004A65 RID: 19045 RVA: 0x0037D9A5 File Offset: 0x0037BBA5
		// (set) Token: 0x06004A66 RID: 19046 RVA: 0x0037D9AD File Offset: 0x0037BBAD
		protected virtual string WRITE_MODE
		{
			[CompilerGenerated]
			get
			{
				return this.<WRITE_MODE>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WRITE_MODE>k__BackingField = value;
			}
		} = "2E";

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x06004A67 RID: 19047 RVA: 0x0037D9B6 File Offset: 0x0037BBB6
		// (set) Token: 0x06004A68 RID: 19048 RVA: 0x0037D9BE File Offset: 0x0037BBBE
		protected virtual string READ_POSTIVE_RESPONSE
		{
			[CompilerGenerated]
			get
			{
				return this.<READ_POSTIVE_RESPONSE>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<READ_POSTIVE_RESPONSE>k__BackingField = value;
			}
		} = "62";

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x06004A69 RID: 19049 RVA: 0x0037D9C7 File Offset: 0x0037BBC7
		// (set) Token: 0x06004A6A RID: 19050 RVA: 0x0037D9CF File Offset: 0x0037BBCF
		protected virtual string WRITE_POSTIVE_RESPONSE
		{
			[CompilerGenerated]
			get
			{
				return this.<WRITE_POSTIVE_RESPONSE>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WRITE_POSTIVE_RESPONSE>k__BackingField = value;
			}
		} = "6E";

		// Token: 0x170016C4 RID: 5828
		// (get) Token: 0x06004A6B RID: 19051 RVA: 0x0037D9D8 File Offset: 0x0037BBD8
		// (set) Token: 0x06004A6C RID: 19052 RVA: 0x0037D9E0 File Offset: 0x0037BBE0
		[JsonProperty("OPTS")]
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x06004A6D RID: 19053 RVA: 0x0037D9EC File Offset: 0x0037BBEC
		public MQBAdaptationTemplate()
		{
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x0037DAF4 File Offset: 0x0037BCF4
		[JsonConstructor]
		public MQBAdaptationTemplate(int UID, string Name, string Description, string Password, string PasswordHint, IEnumerable<TranslationItem> Translations, AdaptationValueTypes ValueType, string Address, string RequestHeader, string ResponseHeader, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned, bool ReversedByteSet, bool RequiresPro, IEnumerable<MQBAdaptationOption> Options)
		{
			this.Name = Name;
			this.UID = UID;
			this.Description = Description;
			this.Password = Password;
			this.PasswordHint = PasswordHint;
			this.Translations = new ObservableCollection<TranslationItem>(Translations);
			this.ValueType = ValueType;
			this.Address = Address;
			this.RequestHeader = RequestHeader;
			this.ResponseHeader = ResponseHeader;
			this.StartByteId = StartByteId;
			this.DataLength = DataLength;
			this.Multiplier = Multiplier;
			this.Offset = Offset;
			this.IsSigned = IsSigned;
			this.ReversedByteSet = ReversedByteSet;
			this.RequiresPro = RequiresPro;
			this.Options = new ObservableCollection<MQBAdaptationOption>(Options);
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x0037DC93 File Offset: 0x0037BE93
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06004A70 RID: 19056 RVA: 0x0037DCAC File Offset: 0x0037BEAC
		// (remove) Token: 0x06004A71 RID: 19057 RVA: 0x0037DCE4 File Offset: 0x0037BEE4
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

		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06004A72 RID: 19058 RVA: 0x0037DD19 File Offset: 0x0037BF19
		// (set) Token: 0x06004A73 RID: 19059 RVA: 0x0037DD21 File Offset: 0x0037BF21
		[JsonProperty("UID")]
		public int UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				this._UID = value;
				this.OnPropertyChanged("UID");
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x06004A74 RID: 19060 RVA: 0x0037DD38 File Offset: 0x0037BF38
		// (set) Token: 0x06004A75 RID: 19061 RVA: 0x0037DDAC File Offset: 0x0037BFAC
		[JsonProperty("NM")]
		public string Name
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.Name;
				}
				if (this._Name != null && this._Name.StartsWith("RES:"))
				{
					return Translate.GetString(this._Name.Substring(4));
				}
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x06004A76 RID: 19062 RVA: 0x0037DDC0 File Offset: 0x0037BFC0
		// (set) Token: 0x06004A77 RID: 19063 RVA: 0x0037DE34 File Offset: 0x0037C034
		[JsonProperty("DSC")]
		public string Description
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.ShortName;
				}
				if (this._Description != null && this._Description.StartsWith("RES:"))
				{
					return Translate.GetString(this._Description.Substring(4));
				}
				return this._Description;
			}
			set
			{
				this._Description = value;
				this.OnPropertyChanged("Description");
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x06004A78 RID: 19064 RVA: 0x0037DE48 File Offset: 0x0037C048
		// (set) Token: 0x06004A79 RID: 19065 RVA: 0x0037DEBC File Offset: 0x0037C0BC
		public string InnerDescription
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.AdditionalText;
				}
				if (this._InnerDescription != null && this._InnerDescription.StartsWith("RES:"))
				{
					return Translate.GetString(this._InnerDescription.Substring(4));
				}
				return this._InnerDescription;
			}
			set
			{
				this._InnerDescription = value;
				this.OnPropertyChanged("InnerDescription");
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x06004A7A RID: 19066 RVA: 0x0037DED0 File Offset: 0x0037C0D0
		// (set) Token: 0x06004A7B RID: 19067 RVA: 0x0037DED8 File Offset: 0x0037C0D8
		[JsonProperty("PWD")]
		public virtual string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				this.OnPropertyChanged("Password");
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x06004A7C RID: 19068 RVA: 0x0037DEEC File Offset: 0x0037C0EC
		// (set) Token: 0x06004A7D RID: 19069 RVA: 0x0037DEF4 File Offset: 0x0037C0F4
		[JsonProperty("PWH")]
		public string PasswordHint
		{
			get
			{
				return this._PasswordHint;
			}
			set
			{
				this._PasswordHint = value;
				this.OnPropertyChanged("PasswordHint");
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x0037DF08 File Offset: 0x0037C108
		// (set) Token: 0x06004A7F RID: 19071 RVA: 0x0037DF10 File Offset: 0x0037C110
		[JsonProperty("TRNS")]
		public ObservableCollection<TranslationItem> Translations
		{
			[CompilerGenerated]
			get
			{
				return this.<Translations>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Translations>k__BackingField = value;
			}
		} = new ObservableCollection<TranslationItem>();

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x06004A80 RID: 19072 RVA: 0x0037DF19 File Offset: 0x0037C119
		// (set) Token: 0x06004A81 RID: 19073 RVA: 0x0037DF21 File Offset: 0x0037C121
		[JsonProperty("VT")]
		public AdaptationValueTypes ValueType
		{
			get
			{
				return this._ValueType;
			}
			set
			{
				this._ValueType = value;
				this.OnPropertyChanged("ValueType");
			}
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x06004A82 RID: 19074 RVA: 0x0037DF35 File Offset: 0x0037C135
		// (set) Token: 0x06004A83 RID: 19075 RVA: 0x0037DF3D File Offset: 0x0037C13D
		[JsonProperty("AD")]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				this._Address = value;
				this.OnPropertyChanged("Address");
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x06004A84 RID: 19076 RVA: 0x0037DF51 File Offset: 0x0037C151
		// (set) Token: 0x06004A85 RID: 19077 RVA: 0x0037DF59 File Offset: 0x0037C159
		[JsonProperty("RQH")]
		public string RequestHeader
		{
			get
			{
				return this._RequestHeader;
			}
			set
			{
				this._RequestHeader = value;
				this.OnPropertyChanged("RequestHeader");
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06004A86 RID: 19078 RVA: 0x0037DF6D File Offset: 0x0037C16D
		// (set) Token: 0x06004A87 RID: 19079 RVA: 0x0037DF75 File Offset: 0x0037C175
		[JsonProperty("RSH")]
		public string ResponseHeader
		{
			get
			{
				return this._ResponseHeader;
			}
			set
			{
				this._ResponseHeader = value;
				this.OnPropertyChanged("ResponseHeader");
			}
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06004A88 RID: 19080 RVA: 0x0037DF89 File Offset: 0x0037C189
		// (set) Token: 0x06004A89 RID: 19081 RVA: 0x0037DF91 File Offset: 0x0037C191
		[JsonIgnore]
		public string CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			set
			{
				this._CurrentState = value;
				this.OnPropertyChanged("CurrentState");
			}
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06004A8A RID: 19082 RVA: 0x0037DFA5 File Offset: 0x0037C1A5
		// (set) Token: 0x06004A8B RID: 19083 RVA: 0x0037DFAD File Offset: 0x0037C1AD
		[JsonIgnore]
		protected virtual string BeforeCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<BeforeCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BeforeCommands>k__BackingField = value;
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06004A8C RID: 19084 RVA: 0x0037DFB6 File Offset: 0x0037C1B6
		// (set) Token: 0x06004A8D RID: 19085 RVA: 0x0037DFBE File Offset: 0x0037C1BE
		[JsonIgnore]
		protected virtual string AfterCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<AfterCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AfterCommands>k__BackingField = value;
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06004A8E RID: 19086 RVA: 0x0037DFC7 File Offset: 0x0037C1C7
		// (set) Token: 0x06004A8F RID: 19087 RVA: 0x0037DFCF File Offset: 0x0037C1CF
		public virtual string Protocol
		{
			[CompilerGenerated]
			get
			{
				return this.<Protocol>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Protocol>k__BackingField = value;
			}
		} = "6";

		// Token: 0x06004A90 RID: 19088 RVA: 0x0037DFD8 File Offset: 0x0037C1D8
		protected virtual void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = string.Concat(new string[] { "ATSP", this.Protocol, ";ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", this.ResponseHeader });
			string text = "";
			if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
			{
				text = "ATSP6";
			}
			else if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
			{
				text = "ATSP7";
			}
			this.AfterCommands = "ATFCSM0;ATD;" + text + ";ATE0;ATH1;ATS0";
			if (this.RequestHeader == "70A")
			{
				this.BeforeCommands += ";ATSTFF";
				this.AfterCommands += ";ATSTDEF";
			}
		}

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x06004A91 RID: 19089 RVA: 0x0037E0AB File Offset: 0x0037C2AB
		// (set) Token: 0x06004A92 RID: 19090 RVA: 0x0037E0B3 File Offset: 0x0037C2B3
		[JsonProperty("SBI")]
		public int StartByteId
		{
			get
			{
				return this._StartByteId;
			}
			set
			{
				this._StartByteId = value;
				this.OnPropertyChanged("StartByteId");
			}
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x06004A93 RID: 19091 RVA: 0x0037E0C7 File Offset: 0x0037C2C7
		// (set) Token: 0x06004A94 RID: 19092 RVA: 0x0037E0CF File Offset: 0x0037C2CF
		[JsonProperty("DL")]
		public int DataLength
		{
			get
			{
				return this._DataLength;
			}
			set
			{
				this._DataLength = value;
				this.OnPropertyChanged("DataLength");
			}
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x06004A95 RID: 19093 RVA: 0x0037E0E3 File Offset: 0x0037C2E3
		// (set) Token: 0x06004A96 RID: 19094 RVA: 0x0037E0EB File Offset: 0x0037C2EB
		[JsonProperty("MUL")]
		public double Multiplier
		{
			get
			{
				return this._Multiplier;
			}
			set
			{
				this._Multiplier = value;
				this.OnPropertyChanged("Multiplier");
			}
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x06004A97 RID: 19095 RVA: 0x0037E0FF File Offset: 0x0037C2FF
		// (set) Token: 0x06004A98 RID: 19096 RVA: 0x0037E107 File Offset: 0x0037C307
		[JsonProperty("RBS")]
		public bool ReversedByteSet
		{
			get
			{
				return this._ReversedByteSet;
			}
			set
			{
				this._ReversedByteSet = value;
				this.OnPropertyChanged("ReversedByteSet");
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06004A99 RID: 19097 RVA: 0x0037E11B File Offset: 0x0037C31B
		// (set) Token: 0x06004A9A RID: 19098 RVA: 0x0037E123 File Offset: 0x0037C323
		[JsonProperty("OFS")]
		public double Offset
		{
			get
			{
				return this._Offset;
			}
			set
			{
				this._Offset = value;
				this.OnPropertyChanged("Offset");
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x0037E137 File Offset: 0x0037C337
		// (set) Token: 0x06004A9C RID: 19100 RVA: 0x0037E13F File Offset: 0x0037C33F
		[JsonProperty("SIG")]
		public bool IsSigned
		{
			get
			{
				return this._IsSigned;
			}
			set
			{
				this._IsSigned = value;
				this.OnPropertyChanged("IsSigned");
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x06004A9D RID: 19101 RVA: 0x0037E153 File Offset: 0x0037C353
		// (set) Token: 0x06004A9E RID: 19102 RVA: 0x0037E15B File Offset: 0x0037C35B
		[JsonIgnore]
		public virtual bool PasswordVisible
		{
			[CompilerGenerated]
			get
			{
				return this.<PasswordVisible>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PasswordVisible>k__BackingField = value;
			}
		} = true;

		// Token: 0x170016DB RID: 5851
		// (get) Token: 0x06004A9F RID: 19103 RVA: 0x0037E164 File Offset: 0x0037C364
		// (set) Token: 0x06004AA0 RID: 19104 RVA: 0x0037E16C File Offset: 0x0037C36C
		[JsonProperty("RP")]
		public virtual bool RequiresPro
		{
			get
			{
				return this._RequiresPro;
			}
			set
			{
				this._RequiresPro = value;
				this.OnPropertyChanged("RequiresPro");
			}
		}

		// Token: 0x170016DC RID: 5852
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x0037E180 File Offset: 0x0037C380
		// (set) Token: 0x06004AA2 RID: 19106 RVA: 0x0037E188 File Offset: 0x0037C388
		public CodingGroup Group
		{
			get
			{
				return this._Group;
			}
			set
			{
				this._Group = value;
				this.OnPropertyChanged("Group");
			}
		}

		// Token: 0x170016DD RID: 5853
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x0037E19C File Offset: 0x0037C39C
		// (set) Token: 0x06004AA4 RID: 19108 RVA: 0x0037E1A4 File Offset: 0x0037C3A4
		public virtual bool HasCurrentState
		{
			get
			{
				return this._HasCurrentState;
			}
			set
			{
				this._HasCurrentState = value;
				this.OnPropertyChanged("HasCurrentState");
			}
		}

		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x06004AA5 RID: 19109 RVA: 0x0037E1B8 File Offset: 0x0037C3B8
		// (set) Token: 0x06004AA6 RID: 19110 RVA: 0x0037E1C0 File Offset: 0x0037C3C0
		[JsonProperty("PreWrite")]
		public string PreWriteCommands
		{
			get
			{
				return this._PreWriteCommands;
			}
			set
			{
				this._PreWriteCommands = value;
				this.OnPropertyChanged("PreWriteCommands");
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06004AA7 RID: 19111 RVA: 0x0037E1D4 File Offset: 0x0037C3D4
		// (set) Token: 0x06004AA8 RID: 19112 RVA: 0x0037E1DC File Offset: 0x0037C3DC
		[JsonProperty("PostWrite")]
		public string PostWriteCommands
		{
			get
			{
				return this._PostWriteCommands;
			}
			set
			{
				this._PostWriteCommands = value;
				this.OnPropertyChanged("PostWriteCommands");
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x0037E1F0 File Offset: 0x0037C3F0
		public static string UNKNOWN_TITLE
		{
			get
			{
				return Translate.GetString("coding_StateUnknown");
			}
		}

		// Token: 0x170016E1 RID: 5857
		// (get) Token: 0x06004AAA RID: 19114 RVA: 0x0037E1FC File Offset: 0x0037C3FC
		public static string UNSUPPORTED_TITLE
		{
			get
			{
				return Translate.GetString("coding_NotSupported");
			}
		}

		// Token: 0x170016E2 RID: 5858
		// (get) Token: 0x06004AAB RID: 19115 RVA: 0x0037E208 File Offset: 0x0037C408
		public static string WRONG_PASSWORD
		{
			get
			{
				return Translate.GetString("coding_WrongPassword");
			}
		}

		// Token: 0x170016E3 RID: 5859
		// (get) Token: 0x06004AAC RID: 19116 RVA: 0x0037E214 File Offset: 0x0037C414
		public static string CODITIONS_NOT_CORRECT
		{
			get
			{
				return Translate.GetString("coding_ConditionsNotCorrect");
			}
		}

		// Token: 0x170016E4 RID: 5860
		// (get) Token: 0x06004AAD RID: 19117 RVA: 0x0037E220 File Offset: 0x0037C420
		// (set) Token: 0x06004AAE RID: 19118 RVA: 0x0037E228 File Offset: 0x0037C428
		public string OpenDiagnosticSessionCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<OpenDiagnosticSessionCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OpenDiagnosticSessionCommand>k__BackingField = value;
			}
		} = "1003";

		// Token: 0x06004AAF RID: 19119 RVA: 0x0037E234 File Offset: 0x0037C434
		public virtual async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			byte[] result = null;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest(this.READ_MODE + this.Address, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			string.IsNullOrEmpty(SharedSettings.Current.GetATST());
			OBDRequest obdrequest2 = new OBDRequest(this.OpenDiagnosticSessionCommand, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest3 = null;
			OBDRequest req_sendKey = null;
			uint uint_pass = 0U;
			if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out uint_pass))
			{
				req_sendKey = new OBDRequest("2704", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest3 = new OBDRequest("2703", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest3.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
				{
					try
					{
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(getSeedData);
						}
						string text = (BitConverter.ToUInt32(getSeedData, 0) + uint_pass).ToString("X8");
						req_sendKey.Command = "2704" + text;
					}
					catch (Exception)
					{
					}
				};
				req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data == null || !data.Contains("6704"))
					{
						codingResult = CodingRequestResult.WrongAccessKey;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
				};
			}
			obdrequest.CheckLength = true;
			obdrequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
			if (obdrequest.Header == "7E0" || obdrequest.Header == "7E1")
			{
				obdrequest.ForceManualFlowControl = false;
			}
			obdrequest.ELMFormat = ELMFormat.CAN11bit;
			obdrequest.ResponseReceived += delegate(OBDRequest getStateReqest2, string response)
			{
				int negativeCodeFromResponse = this.GetNegativeCodeFromResponse(getStateReqest2, response);
				if (negativeCodeFromResponse > 0 && negativeCodeFromResponse != 120)
				{
					if (negativeCodeFromResponse <= 49)
					{
						if (negativeCodeFromResponse - 17 > 1)
						{
							if (negativeCodeFromResponse == 34)
							{
								goto IL_00C1;
							}
							if (negativeCodeFromResponse != 49)
							{
								return;
							}
						}
					}
					else
					{
						if (negativeCodeFromResponse == 51)
						{
							codingResult = CodingRequestResult.WrongAccessKey;
							return;
						}
						switch (negativeCodeFromResponse)
						{
						case 126:
						case 127:
							break;
						case 128:
						case 142:
						case 144:
							return;
						case 129:
						case 130:
						case 131:
						case 132:
						case 133:
						case 134:
						case 135:
						case 136:
						case 137:
						case 138:
						case 139:
						case 140:
						case 141:
						case 143:
						case 145:
						case 146:
						case 147:
						case 148:
							goto IL_00C1;
						default:
							if (negativeCodeFromResponse - 240 > 14)
							{
								return;
							}
							goto IL_00C1;
						}
					}
					codingResult = CodingRequestResult.NotSupported;
					return;
					IL_00C1:
					codingResult = CodingRequestResult.WrongConditions;
				}
			};
			obdrequest.ResponseReceived += this.Request_ResponseReceivedCheckForNR78;
			obdrequest.ResponseDecoded += delegate(OBDRequest getStateReqest3, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					if (codingResult == CodingRequestResult.UnknownError)
					{
						codingResult = CodingRequestResult.NotSupported;
					}
				}
				else
				{
					codingResult = CodingRequestResult.Success;
				}
				result = data;
			};
			List<OBDRequest> list = new List<OBDRequest>();
			list.Add(obdrequest2);
			if (obdrequest3 != null && req_sendKey != null)
			{
				list.Add(obdrequest3);
				list.Add(req_sendKey);
			}
			list.Add(obdrequest);
			foreach (OBDRequest obdrequest4 in list)
			{
				obdrequest4.ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			if (result == null)
			{
				result = new byte[0];
			}
			if (result.Length != 0 && this.DataLength == 0)
			{
				this.DataLength = result.Length;
			}
			return new Tuple<byte[], CodingRequestResult>(result, codingResult);
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0037E280 File Offset: 0x0037C480
		public int GetNegativeCodeFromResponse(OBDRequest request, string data)
		{
			if (data == null)
			{
				return -1;
			}
			string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			string text = request.Command.Substring(0, 2);
			string text2 = "037F" + text;
			ELMFormat elmformat = App.OBDReader.CurrentELMFormat;
			int num = 3;
			if (request.ELMFormat != ELMFormat.Unknown && request.ELMFormat != elmformat)
			{
				elmformat = request.ELMFormat;
			}
			if (elmformat != ELMFormat.CAN11bit)
			{
				if (elmformat == ELMFormat.CAN29bit)
				{
					num = 8;
				}
			}
			else
			{
				num = 3;
			}
			for (int i = array.Length - 1; i >= 0; i--)
			{
				string text3 = array[i];
				int num2 = text3.IndexOf(text2, num);
				if (num2 >= 0 && text3.Length >= num2 + text2.Length + 2)
				{
					try
					{
						return (int)BitHelpers.ConvertHexToBytesX(text3.Substring(num2 + text2.Length, 2))[0];
					}
					catch (Exception)
					{
						return -2;
					}
				}
			}
			return 0;
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0037E378 File Offset: 0x0037C578
		public void Request_ResponseReceivedCheckForNR78(OBDRequest request, string data)
		{
			request.DoNotDecode = false;
			if (data == null)
			{
				return;
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			string text2 = request.Command.Substring(0, 2);
			"037F" + text2 + "11";
			string nr78_string = "037F" + text2 + "78";
			if (text.IndexOf(nr78_string) >= 0)
			{
				data = OBDDataReader.FilterHexAndNewLineOnly(data);
				if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
				{
					App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					request.DoNotDecode = true;
					string[] array = new string[request.BeforeCommands.Length + 2];
					Array.Copy(request.BeforeCommands, array, request.BeforeCommands.Length);
					array[array.Length - 2] = "ATAT0";
					array[array.Length - 1] = "ATSTFF";
					string[] array2 = new string[request.AfterCommands.Length + 2];
					Array.Copy(request.AfterCommands, array2, request.AfterCommands.Length);
					array2[array2.Length - 2] = "ATAT" + SharedSettings.Current.AdaptiveTimings.ToString();
					string text3 = SharedSettings.Current.GetATST();
					if (string.IsNullOrEmpty(text3))
					{
						text3 = "32";
					}
					array2[array2.Length - 1] = "ATST" + text3;
					request.BeforeCommands = array;
					request.AfterCommands = array2;
					request.ResponseReceived -= this.Request_ResponseReceivedCheckForNR78;
					request.ResponseReceived += delegate(OBDRequest request2, string response)
					{
						App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
						request.DoNotDecode = false;
						if (response == null || request2.NR78RepeatCounter > 3)
						{
							return;
						}
						bool flag = false;
						bool flag2 = false;
						if (response != null && response.Contains("NO DATA"))
						{
							flag = true;
						}
						if (!flag)
						{
							string text4 = OBDDataReader.FilterHexAndNewLineOnly(response);
							string[] array3 = text4.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
							if (text4.Contains(nr78_string) && array3.Length == 1)
							{
								flag2 = true;
							}
						}
						if (flag || flag2)
						{
							request.DoNotDecode = true;
							int nr78RepeatCounter = request2.NR78RepeatCounter;
							request2.NR78RepeatCounter = nr78RepeatCounter + 1;
							App.OBDReader.InsertRequestInQueue(request2);
						}
					};
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Insert(0, request);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0037E5A0 File Offset: 0x0037C7A0
		public virtual async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			string state = "";
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			if (tuple.Item2 != CodingRequestResult.Success && string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(this.Password) && tuple.Item2 == CodingRequestResult.WrongAccessKey)
			{
				tuple = await this.GetCurrentStateRawData(this.Password);
			}
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				this.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				switch (this.ValueType)
				{
				case AdaptationValueTypes.OptionType:
					state = this.ConvertRawDataToOptionType(item2);
					break;
				case AdaptationValueTypes.InputValueType:
					state = this.ConvertRawDataToInputValue(item2);
					break;
				case AdaptationValueTypes.InputHexDataType:
				case AdaptationValueTypes.MQBLightConfiguration:
				case AdaptationValueTypes.MQBColorList:
					state = BitHelpers.ByteArrayToHexString(item2);
					break;
				case AdaptationValueTypes.InputTextType:
					try
					{
						state = Encoding.ASCII.GetString(item2, this.StartByteId, this.DataLength);
					}
					catch (Exception)
					{
						state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					break;
				}
				this.CurrentState = state;
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0037E5EC File Offset: 0x0037C7EC
		public static string CodingRequestResultToString(CodingRequestResult codingResult)
		{
			switch (codingResult)
			{
			case CodingRequestResult.WrongAccessKey:
				return Translate.GetString("coding_WrongPassword");
			case CodingRequestResult.WrongConditions:
				return Translate.GetString("coding_ConditionsNotCorrect");
			case CodingRequestResult.WrongDate:
				return Translate.GetString("coding_WrongDate");
			case CodingRequestResult.InitialDataIncorrect:
			case CodingRequestResult.InitialDataEmpty:
				return Translate.GetString("coding_InitialDataIncorrect");
			case CodingRequestResult.WrongDevice:
				return Translate.GetString("coding_WrongDevice");
			case CodingRequestResult.WrongInputValue:
				return Translate.GetString("coding_WrongInputData");
			case CodingRequestResult.NotSupported:
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			case CodingRequestResult.NoData:
				return "No response from ECU";
			}
			return codingResult.ToString();
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0037E694 File Offset: 0x0037C894
		internal string ConvertRawDataToOptionType(byte[] data)
		{
			foreach (MQBAdaptationOption mqbadaptationOption in this.Options)
			{
				if (!string.IsNullOrEmpty(mqbadaptationOption.Value))
				{
					if (mqbadaptationOption.Value.StartsWith("BIT:") || mqbadaptationOption.Value.StartsWith("BITS:"))
					{
						try
						{
							string[] array = mqbadaptationOption.Value.Substring(mqbadaptationOption.Value.IndexOf(':') + 1).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
							bool flag = true;
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string[] array3 = array2[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
								int num = int.Parse(array3[0]);
								int num2 = int.Parse(array3[1]);
								bool flag2 = int.Parse(array3[2]) == 1;
								if (BitHelpers.GetBit_0_7(data[num], num2) != flag2)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								return mqbadaptationOption.Title;
							}
							continue;
						}
						catch (Exception)
						{
							continue;
						}
					}
					try
					{
						byte[] array4 = BitHelpers.ConvertHexToBytesX(mqbadaptationOption.Value);
						byte[] array5 = new byte[data.Length];
						Array.Copy(data, array5, data.Length);
						Array.Copy(array4, 0, array5, this.StartByteId, array4.Length);
						if (ArrayHelpers.ArrayEquals<byte>(array5, data))
						{
							return mqbadaptationOption.Title;
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return MQBAdaptationTemplate.UNKNOWN_TITLE;
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0037E850 File Offset: 0x0037CA50
		protected string ConvertRawDataToInputValue(byte[] data)
		{
			if (data.Length < this.StartByteId + this.DataLength)
			{
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			}
			double num = double.NaN;
			int num2 = this.DataLength;
			if (num2 == 0)
			{
				num2 = data.Length - this.StartByteId;
			}
			if (num2 > 4)
			{
				num2 = 4;
			}
			if (this.IsSigned)
			{
				switch (num2)
				{
				case 1:
					num = (double)((sbyte)data[this.StartByteId]);
					break;
				case 2:
					num = (double)((short)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]));
					break;
				case 3:
					num = (double)((int)((sbyte)data[this.StartByteId]) * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					int startByteId = this.StartByteId;
					num = (double)(((int)data[startByteId++] << 24) | ((int)data[startByteId++] << 16) | ((int)data[startByteId++] << 8) | (int)data[startByteId++]);
					break;
				}
				}
			}
			else
			{
				switch (num2)
				{
				case 1:
					num = (double)data[this.StartByteId];
					break;
				case 2:
					num = (double)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]);
					break;
				case 3:
					num = (double)((int)data[this.StartByteId] * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					byte[] array = new byte[4];
					Array.Copy(data, this.StartByteId, array, 0, 4);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					num = BitConverter.ToUInt32(array, 0);
					break;
				}
				default:
					num = (double)((int)data[this.StartByteId] * 256 * 256 * 256 + (int)data[this.StartByteId + 1] * 256 * 256 + (int)data[this.StartByteId + 2] * 256 + (int)data[this.StartByteId + 3]);
					break;
				}
			}
			num *= this.Multiplier;
			return (num + this.Offset).ToString("0.####");
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0037EA70 File Offset: 0x0037CC70
		protected byte[] ConvertInputValueToByteArray(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return new byte[0];
			}
			double num;
			if (!double.TryParse(input.Trim().Replace(" ", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				return new byte[0];
			}
			byte[] bytes = BitConverter.GetBytes((int)((num - this.Offset) / this.Multiplier));
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			if (this.DataLength == 4)
			{
				return bytes;
			}
			byte[] array = new byte[this.DataLength];
			Array.Copy(bytes, 4 - this.DataLength, array, 0, array.Length);
			return array;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0037EB1D File Offset: 0x0037CD1D
		private byte[] ConvertInputHexStringToByteArray(string hex)
		{
			return BitHelpers.ConvertHexToBytesX(OBDDataReader.FilterHexAndNewLineOnly(hex).Replace("\r", "").Replace("\n", "")
				.Trim());
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0037EB50 File Offset: 0x0037CD50
		public static void RequestRetryRequestResponseDelegate(OBDRequest request, string data)
		{
			int num = 0;
			string text;
			if (request.Keys.TryGetValue("RepeatCounter", out text))
			{
				num = int.Parse(text, CultureInfo.InvariantCulture);
			}
			if (num > 3)
			{
				return;
			}
			if (data.Contains("NO DATA"))
			{
				num++;
				request.Keys["RepeatCounter"] = num.ToString();
				List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
				queueCopy.Insert(0, request);
				App.OBDReader.ReplaceQueue(queueCopy);
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0037EBCC File Offset: 0x0037CDCC
		public virtual async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string checked_value = "";
			if (originalData == null)
			{
				if (progress != null)
				{
					progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
				}
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData("");
				if (tuple.Item2 != CodingRequestResult.Success)
				{
					if (string.IsNullOrEmpty(password) || tuple.Item2 != CodingRequestResult.WrongAccessKey)
					{
						return tuple.Item2;
					}
					tuple = await this.GetCurrentStateRawData(password);
				}
				originalData = tuple.Item1;
			}
			if (string.IsNullOrEmpty(value))
			{
				checked_value = "";
			}
			else
			{
				switch (this.ValueType)
				{
				case AdaptationValueTypes.OptionType:
				{
					byte[] array = originalData;
					byte[] array2 = new byte[array.Length];
					Array.Copy(array, 0, array2, 0, array.Length);
					if (value.StartsWith("BIT:"))
					{
						try
						{
							string[] array3 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < array3.Length; i++)
							{
								string[] array4 = array3[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
								BitHelpers.SwitchBitInByte(array2, int.Parse(array4[0]), int.Parse(array4[1]), int.Parse(array4[2]) == 1);
							}
							goto IL_0297;
						}
						catch (Exception)
						{
							goto IL_0297;
						}
					}
					byte[] array5 = BitHelpers.ConvertHexToBytesX(value);
					try
					{
						Array.Copy(array, 0, array2, 0, array.Length);
						Array.Copy(array5, 0, array2, this.StartByteId, array5.Length);
					}
					catch (Exception)
					{
						return CodingRequestResult.InitialDataIncorrect;
					}
					IL_0297:
					string text = BitHelpers.ByteArrayToHexString(array2);
					if (text.Length % 2 != 0)
					{
						text = "0" + text;
					}
					checked_value = text;
					break;
				}
				case AdaptationValueTypes.InputValueType:
				{
					byte[] array6 = this.ConvertInputValueToByteArray(value);
					if (array6.Length == 0)
					{
						return CodingRequestResult.WrongInputValue;
					}
					byte[] array7 = new byte[originalData.Length];
					try
					{
						Array.Copy(originalData, 0, array7, 0, array7.Length);
						Array.Copy(array6, 0, array7, this.StartByteId, this.DataLength);
					}
					catch (Exception)
					{
						return CodingRequestResult.InitialDataIncorrect;
					}
					checked_value = BitHelpers.ByteArrayToHexString(array7);
					break;
				}
				case AdaptationValueTypes.InputHexDataType:
				case AdaptationValueTypes.MQBLightConfiguration:
				case AdaptationValueTypes.MQBColorList:
				{
					string text2 = OBDDataReader.FilterHexAndNewLineOnly(value).Replace("\r", "").Replace("\n", "")
						.Trim();
					if (text2.Length % 2 != 0)
					{
						return CodingRequestResult.WrongInputValue;
					}
					checked_value = text2;
					break;
				}
				case AdaptationValueTypes.InputTextType:
				{
					byte[] array8 = Encoding.ASCII.GetBytes(value);
					if (array8.Length == 0)
					{
						return CodingRequestResult.WrongInputValue;
					}
					if (array8.Length > this.DataLength)
					{
						array8 = array8.Take(this.DataLength).ToArray<byte>();
					}
					else if (array8.Length < this.DataLength)
					{
						byte[] array9 = new byte[this.DataLength - array8.Length];
						for (int j = 0; j < array9.Length; j++)
						{
							array9[j] = 0;
						}
						array8 = array8.Concat(array9).ToArray<byte>();
					}
					byte[] array10 = new byte[originalData.Length];
					try
					{
						Array.Copy(originalData, 0, array10, 0, array10.Length);
						Array.Copy(array8, 0, array10, this.StartByteId, this.DataLength);
					}
					catch (Exception)
					{
						return CodingRequestResult.InitialDataIncorrect;
					}
					checked_value = BitHelpers.ByteArrayToHexString(array10);
					break;
				}
				}
			}
			CodingRequestResult codingRequestResult;
			if (skipIfTheSameData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
			{
				CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQB, this.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, this.RequestHeader, this.ResponseHeader, "", "", "", "96", null, "");
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
			}
			return codingRequestResult;
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0037EC44 File Offset: 0x0037CE44
		protected virtual async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null)
				{
					data.Contains("6EF199");
				}
			};
			OBDRequest obdrequest = new OBDRequest("22F199", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
			};
			obdrequest.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
			{
				if (getDateData != null && getDateData.Length >= 3)
				{
					string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
					req_setDate.Command = text;
				}
			};
			OBDRequest req_setCodingSequence = new OBDRequest("2EF198", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && data.Contains("6EF198"))
				{
					IProgress<string> progress2 = progress;
					if (progress2 == null)
					{
						return;
					}
					progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
				else
				{
					App.OBDReader.DebugWrite("\n2EF198_error\n");
					IProgress<string> progress3 = progress;
					if (progress3 == null)
					{
						return;
					}
					progress3.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("22F1A5", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations,
				CheckLength = true
			};
			obdrequest2.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
			{
				if (getCodingData != null && getCodingData.Length != 0)
				{
					string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
					if (getCodingData.All((byte x) => x == 0))
					{
						text2 = "0181C8F63039";
					}
					if (text2.Length == 6 && req_getCoding2.ForceManualFlowControl)
					{
						req_getCoding2.ForceManualFlowControl = false;
						App.OBDReader.InsertRequestInQueue(req_getCoding2);
						text2 = "0181C8F63039";
					}
					if (text2.Length == 6)
					{
						text2 = "0181C8F63039";
					}
					string text3 = "2EF198" + text2;
					req_setCodingSequence.Command = text3;
					return;
				}
				string text4 = "2EF1980181C8F63039";
				req_setCodingSequence.Command = text4;
				if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
				{
					req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
					App.OBDReader.InsertRequestInQueue(req_getCoding2);
				}
			};
			OBDRequest req_set22F1A0 = new OBDRequest("3E", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest3 = new OBDRequest("22F1A0", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				CheckLength = true,
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
			};
			obdrequest3.ResponseDecoded += delegate(OBDRequest req_get22F1A0_2, byte[] req_get22F1A0Data, bool req_get22F1A0DecodeResult, string req_get22F1A0ResponseHeader)
			{
				if (req_get22F1A0Data != null && req_get22F1A0Data.Length != 0)
				{
					string text5 = BitHelpers.ByteArrayToHexString(req_get22F1A0Data);
					req_set22F1A0.Command = "2EF1A0" + text5;
				}
			};
			OBDRequest req_set22F1A1 = new OBDRequest("3E", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest4 = new OBDRequest("22F1A1", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				CheckLength = true,
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
			};
			obdrequest4.ResponseDecoded += delegate(OBDRequest req_get22F1A1_2, byte[] req_get22F1A1Data, bool req_get22F1A1DecodeResult, string req_get22F1A1ResponseHeader)
			{
				if (req_get22F1A1Data != null && req_get22F1A1Data.Length != 0)
				{
					string text6 = BitHelpers.ByteArrayToHexString(req_get22F1A1Data);
					req_set22F1A1.Command = "2EF1A1" + text6;
				}
			};
			bool zero_seed = false;
			OBDRequest obdrequest5 = null;
			OBDRequest req_sendKey = null;
			uint uint_pass = 0U;
			if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out uint_pass))
			{
				req_sendKey = new OBDRequest("2704", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest5 = new OBDRequest("2703", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
				obdrequest5.ResponseReceived += delegate(OBDRequest req_getSeed2, string msg)
				{
					if (msg != null && msg.Contains("NO DATA") && !req_getSeed2.Keys.ContainsKey("SECOND_TRY"))
					{
						req_getSeed2.Keys.Add("SECOND_TRY", "1");
						App.OBDReader.InsertRequestInQueue(req_getSeed2);
					}
				};
				obdrequest5.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
				{
					try
					{
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(getSeedData);
						}
						uint num = BitConverter.ToUInt32(getSeedData, 0);
						if (num == 0U)
						{
							zero_seed = true;
						}
						if (!zero_seed)
						{
							string text7 = (num + uint_pass).ToString("X8");
							req_sendKey.Command = "2704" + text7;
							IProgress<string> progress4 = progress;
							if (progress4 != null)
							{
								progress4.Report(Translate.GetString("coding_progress_SendingPassword"));
							}
						}
						else
						{
							req_sendKey.Command = "3E";
						}
					}
					catch (Exception)
					{
						App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
						req_sendKey.Command = "3E";
					}
				};
				req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (request.Command != "3E")
					{
						if (data != null)
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
						}
						if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
						{
							IProgress<string> progress5 = progress;
							if (progress5 == null)
							{
								return;
							}
							progress5.Report(Translate.GetString("coding_progress_SuccessPassword"));
							return;
						}
						else if (!(SharedSettings.Current.IgnoreCodingErrors | zero_seed))
						{
							codingResult = CodingRequestResult.WrongAccessKey;
							IProgress<string> progress6 = progress;
							if (progress6 != null)
							{
								progress6.Report(Translate.GetString("coding_progress_WrongPassword"));
							}
							App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							semaphore.Release();
						}
					}
				};
			}
			OBDRequest obdrequest6 = new OBDRequest(this.WRITE_MODE + this.Address + checked_value, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest6.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && (data.Contains(this.WRITE_POSTIVE_RESPONSE + this.Address) || data.Contains("037F2E78")))
				{
					IProgress<string> progress7 = progress;
					if (progress7 != null)
					{
						progress7.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress8 = progress;
					if (progress8 != null)
					{
						progress8.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					else if (data.Contains("NO DATA"))
					{
						codingResult = CodingRequestResult.NoData;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 11 && data.Contains("7F2E"))
						{
							int num2 = data.IndexOf("7F2E");
							int num3 = int.Parse(data.Substring(num2 + 4, 2), NumberStyles.HexNumber);
							if (num3 >= 128 || num3 == 34)
							{
								codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num3 == 51)
							{
								codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num3 == 49)
							{
								codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				semaphore.Release();
			};
			OBDRequest obdrequest7 = new OBDRequest(this.OpenDiagnosticSessionCommand, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest8 = new OBDRequest("1040", this.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest9 = new OBDRequest("1003", "710", "ATFCSH710;ATFCSD300000;ATFCSM1;ATAL;ATCRA77A", this.AfterCommands, false)
			{
				DoNotDecode = true
			};
			OBDRequest obdrequest10 = new OBDRequest("3E80", "700", this.BeforeCommands, this.AfterCommands, false)
			{
				DoNotDecode = true
			};
			List<OBDRequest> list = new List<OBDRequest>();
			list.Add(obdrequest9);
			list.Add(obdrequest7);
			if (this.RequestHeader == "773")
			{
				list.Add(obdrequest8);
				if (SharedSettings.Current.SendTesterPresentWhileLongUploadTimeVag5f)
				{
					obdrequest6.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + this.RequestHeader);
				}
			}
			if (!string.IsNullOrEmpty(this.PreWriteCommands))
			{
				OBDRequest[] array = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false, null)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				list.AddRange(array);
			}
			list.Add(obdrequest);
			obdrequest.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			obdrequest.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			list.Add(obdrequest10);
			list.Add(obdrequest2);
			obdrequest2.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			obdrequest2.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
			list.Add(obdrequest10);
			if (obdrequest5 != null && req_sendKey != null)
			{
				list.Add(obdrequest5);
				list.Add(req_sendKey);
			}
			list.Add(req_setDate);
			list.Add(req_setCodingSequence);
			if (this.RequestHeader == "773")
			{
				list.Add(obdrequest3);
				obdrequest3.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				obdrequest3.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				list.Add(obdrequest10);
				list.Add(obdrequest4);
				obdrequest4.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				obdrequest4.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				list.Add(obdrequest10);
				list.Add(req_set22F1A0);
				req_set22F1A0.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				req_set22F1A0.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				list.Add(obdrequest10);
				list.Add(req_set22F1A1);
				req_set22F1A1.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				req_set22F1A1.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
				list.Add(obdrequest10);
			}
			list.Add(obdrequest6);
			if (this.RequestHeader == "7E0" || this.RequestHeader == "7E1")
			{
				obdrequest3.ForceManualFlowControl = false;
				obdrequest4.ForceManualFlowControl = false;
				obdrequest.ForceManualFlowControl = false;
				obdrequest2.ForceManualFlowControl = false;
				obdrequest6.ForceManualFlowControl = false;
			}
			SemaphoreSlim postWriteSemaphore = null;
			if (!string.IsNullOrEmpty(this.PostWriteCommands))
			{
				OBDRequest[] array2 = (from x in this.PostWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false, null)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				if (array2.Length != 0)
				{
					postWriteSemaphore = new SemaphoreSlim(0, 1);
					array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest postreq, string postdata)
					{
						postWriteSemaphore.Release();
					};
				}
				foreach (OBDRequest obdrequest11 in array2)
				{
					list.Add(obdrequest10);
					list.Add(obdrequest11);
					if (obdrequest11.Command.StartsWith("22") || obdrequest11.Command.StartsWith("2E"))
					{
						obdrequest11.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
						obdrequest11.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
					}
				}
			}
			foreach (OBDRequest obdrequest12 in list)
			{
				obdrequest12.ELMFormat = ELMFormat.CAN11bit;
			}
			CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQB, this.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, this.RequestHeader, this.ResponseHeader, "", "", "", "96", null, "");
			App.OBDReader.ReplaceQueue(list);
			if (postWriteSemaphore == null)
			{
				await semaphore.WaitAsync();
			}
			else
			{
				await Task.WhenAll(new Task[]
				{
					semaphore.WaitAsync(),
					postWriteSemaphore.WaitAsync()
				});
			}
			return codingResult;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0037ECB4 File Offset: 0x0037CEB4
		protected virtual bool CheckForSuccessfullResponse(OBDRequest req, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return false;
			}
			if (data.Contains("NO DATA"))
			{
				return false;
			}
			string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' });
			string nr78pattern = "037F" + req.Command.Substring(0, 2);
			string responseHeader = CAN11bitHelper.GetPossibleResponseHeader(req.Header, SharedSettings.Current.SelectedBrand, req);
			return array.Any((string line) => line.StartsWith(responseHeader) && line.IndexOf(req.ResponseMarker, 3) >= 0) || array.Any((string x) => x.Contains(nr78pattern));
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x0037ED78 File Offset: 0x0037CF78
		// Note: this type is marked as 'beforefieldinit'.
		static MQBAdaptationTemplate()
		{
		}

		// Token: 0x04002B44 RID: 11076
		[CompilerGenerated]
		private string <READ_MODE>k__BackingField;

		// Token: 0x04002B45 RID: 11077
		[CompilerGenerated]
		private string <WRITE_MODE>k__BackingField;

		// Token: 0x04002B46 RID: 11078
		[CompilerGenerated]
		private string <READ_POSTIVE_RESPONSE>k__BackingField;

		// Token: 0x04002B47 RID: 11079
		[CompilerGenerated]
		private string <WRITE_POSTIVE_RESPONSE>k__BackingField;

		// Token: 0x04002B48 RID: 11080
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04002B49 RID: 11081
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002B4A RID: 11082
		private int _UID;

		// Token: 0x04002B4B RID: 11083
		private string _Name = "";

		// Token: 0x04002B4C RID: 11084
		private string _Description = "";

		// Token: 0x04002B4D RID: 11085
		private string _InnerDescription = "";

		// Token: 0x04002B4E RID: 11086
		private string _Password = "";

		// Token: 0x04002B4F RID: 11087
		private string _PasswordHint = "";

		// Token: 0x04002B50 RID: 11088
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002B51 RID: 11089
		private AdaptationValueTypes _ValueType;

		// Token: 0x04002B52 RID: 11090
		private string _Address = "";

		// Token: 0x04002B53 RID: 11091
		private string _RequestHeader = "";

		// Token: 0x04002B54 RID: 11092
		private string _ResponseHeader = "";

		// Token: 0x04002B55 RID: 11093
		private string _CurrentState = "";

		// Token: 0x04002B56 RID: 11094
		[CompilerGenerated]
		private string <BeforeCommands>k__BackingField;

		// Token: 0x04002B57 RID: 11095
		[CompilerGenerated]
		private string <AfterCommands>k__BackingField;

		// Token: 0x04002B58 RID: 11096
		[CompilerGenerated]
		private string <Protocol>k__BackingField;

		// Token: 0x04002B59 RID: 11097
		private int _StartByteId;

		// Token: 0x04002B5A RID: 11098
		private int _DataLength = 1;

		// Token: 0x04002B5B RID: 11099
		private double _Multiplier = 1.0;

		// Token: 0x04002B5C RID: 11100
		private bool _ReversedByteSet;

		// Token: 0x04002B5D RID: 11101
		private double _Offset;

		// Token: 0x04002B5E RID: 11102
		private bool _IsSigned;

		// Token: 0x04002B5F RID: 11103
		[CompilerGenerated]
		private bool <PasswordVisible>k__BackingField;

		// Token: 0x04002B60 RID: 11104
		private bool _RequiresPro;

		// Token: 0x04002B61 RID: 11105
		private CodingGroup _Group;

		// Token: 0x04002B62 RID: 11106
		private bool _HasCurrentState = true;

		// Token: 0x04002B63 RID: 11107
		private string _PreWriteCommands = "";

		// Token: 0x04002B64 RID: 11108
		private string _PostWriteCommands = "";

		// Token: 0x04002B65 RID: 11109
		public static readonly MQBAdaptationOption EnableOption = new MQBAdaptationOption(Translate.GetString("coding_EnableOption"), "01");

		// Token: 0x04002B66 RID: 11110
		public static readonly MQBAdaptationOption DisableOption = new MQBAdaptationOption(Translate.GetString("coding_DisableOption"), "00");

		// Token: 0x04002B67 RID: 11111
		private const string OPTION_ENABLE_VALUE = "01";

		// Token: 0x04002B68 RID: 11112
		private const string OPTION_DISABLE_VALUE = "00";

		// Token: 0x04002B69 RID: 11113
		[CompilerGenerated]
		private string <OpenDiagnosticSessionCommand>k__BackingField;

		// Token: 0x0200088D RID: 2189
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004ABD RID: 19133 RVA: 0x0037EDAC File Offset: 0x0037CFAC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004ABE RID: 19134 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004ABF RID: 19135 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Name>b__31_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004AC0 RID: 19136 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__35_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004AC1 RID: 19137 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_InnerDescription>b__39_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004AC2 RID: 19138 RVA: 0x0037EDB8 File Offset: 0x0037CFB8
			internal void <WriteDataToECU>b__162_0(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null)
				{
					data.Contains("6EF199");
				}
			}

			// Token: 0x06004AC3 RID: 19139 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <WriteDataToECU>b__162_10(byte x)
			{
				return x == 0;
			}

			// Token: 0x06004AC4 RID: 19140 RVA: 0x0037EDDC File Offset: 0x0037CFDC
			internal void <WriteDataToECU>b__162_6(OBDRequest req_getSeed2, string msg)
			{
				if (msg != null && msg.Contains("NO DATA") && !req_getSeed2.Keys.ContainsKey("SECOND_TRY"))
				{
					req_getSeed2.Keys.Add("SECOND_TRY", "1");
					App.OBDReader.InsertRequestInQueue(req_getSeed2);
				}
			}

			// Token: 0x04002B6A RID: 11114
			public static readonly MQBAdaptationTemplate.<>c <>9 = new MQBAdaptationTemplate.<>c();

			// Token: 0x04002B6B RID: 11115
			public static Func<TranslationItem, bool> <>9__31_0;

			// Token: 0x04002B6C RID: 11116
			public static Func<TranslationItem, bool> <>9__35_0;

			// Token: 0x04002B6D RID: 11117
			public static Func<TranslationItem, bool> <>9__39_0;

			// Token: 0x04002B6E RID: 11118
			public static ResponseReceivedDelegate <>9__162_0;

			// Token: 0x04002B6F RID: 11119
			public static Func<byte, bool> <>9__162_10;

			// Token: 0x04002B70 RID: 11120
			public static ResponseReceivedDelegate <>9__162_6;
		}

		// Token: 0x0200088E RID: 2190
		[CompilerGenerated]
		private sealed class <>c__DisplayClass151_0
		{
			// Token: 0x06004AC5 RID: 19141 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass151_0()
			{
			}

			// Token: 0x06004AC6 RID: 19142 RVA: 0x0037EE2C File Offset: 0x0037D02C
			internal void <GetCurrentStateRawData>b__0(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					string text = (BitConverter.ToUInt32(getSeedData, 0) + this.uint_pass).ToString("X8");
					this.req_sendKey.Command = "2704" + text;
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06004AC7 RID: 19143 RVA: 0x0037EE90 File Offset: 0x0037D090
			internal void <GetCurrentStateRawData>b__1(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data == null || !data.Contains("6704"))
				{
					this.codingResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x06004AC8 RID: 19144 RVA: 0x0037EEC4 File Offset: 0x0037D0C4
			internal void <GetCurrentStateRawData>b__2(OBDRequest getStateReqest2, string response)
			{
				int negativeCodeFromResponse = this.<>4__this.GetNegativeCodeFromResponse(getStateReqest2, response);
				if (negativeCodeFromResponse > 0 && negativeCodeFromResponse != 120)
				{
					if (negativeCodeFromResponse <= 49)
					{
						if (negativeCodeFromResponse - 17 > 1)
						{
							if (negativeCodeFromResponse == 34)
							{
								goto IL_00C1;
							}
							if (negativeCodeFromResponse != 49)
							{
								return;
							}
						}
					}
					else
					{
						if (negativeCodeFromResponse == 51)
						{
							this.codingResult = CodingRequestResult.WrongAccessKey;
							return;
						}
						switch (negativeCodeFromResponse)
						{
						case 126:
						case 127:
							break;
						case 128:
						case 142:
						case 144:
							return;
						case 129:
						case 130:
						case 131:
						case 132:
						case 133:
						case 134:
						case 135:
						case 136:
						case 137:
						case 138:
						case 139:
						case 140:
						case 141:
						case 143:
						case 145:
						case 146:
						case 147:
						case 148:
							goto IL_00C1;
						default:
							if (negativeCodeFromResponse - 240 > 14)
							{
								return;
							}
							goto IL_00C1;
						}
					}
					this.codingResult = CodingRequestResult.NotSupported;
					return;
					IL_00C1:
					this.codingResult = CodingRequestResult.WrongConditions;
				}
			}

			// Token: 0x06004AC9 RID: 19145 RVA: 0x0037EF99 File Offset: 0x0037D199
			internal void <GetCurrentStateRawData>b__3(OBDRequest getStateReqest3, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					if (this.codingResult == CodingRequestResult.UnknownError)
					{
						this.codingResult = CodingRequestResult.NotSupported;
					}
				}
				else
				{
					this.codingResult = CodingRequestResult.Success;
				}
				this.result = data;
			}

			// Token: 0x04002B71 RID: 11121
			public uint uint_pass;

			// Token: 0x04002B72 RID: 11122
			public OBDRequest req_sendKey;

			// Token: 0x04002B73 RID: 11123
			public CodingRequestResult codingResult;

			// Token: 0x04002B74 RID: 11124
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002B75 RID: 11125
			public byte[] result;
		}

		// Token: 0x0200088F RID: 2191
		[CompilerGenerated]
		private sealed class <>c__DisplayClass153_0
		{
			// Token: 0x06004ACA RID: 19146 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass153_0()
			{
			}

			// Token: 0x06004ACB RID: 19147 RVA: 0x0037EFC4 File Offset: 0x0037D1C4
			internal void <Request_ResponseReceivedCheckForNR78>b__0(OBDRequest request2, string response)
			{
				App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
				this.request.DoNotDecode = false;
				if (response == null || request2.NR78RepeatCounter > 3)
				{
					return;
				}
				bool flag = false;
				bool flag2 = false;
				if (response != null && response.Contains("NO DATA"))
				{
					flag = true;
				}
				if (!flag)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(response);
					string[] array = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					if (text.Contains(this.nr78_string) && array.Length == 1)
					{
						flag2 = true;
					}
				}
				if (flag || flag2)
				{
					this.request.DoNotDecode = true;
					int nr78RepeatCounter = request2.NR78RepeatCounter;
					request2.NR78RepeatCounter = nr78RepeatCounter + 1;
					App.OBDReader.InsertRequestInQueue(request2);
				}
			}

			// Token: 0x04002B76 RID: 11126
			public OBDRequest request;

			// Token: 0x04002B77 RID: 11127
			public string nr78_string;
		}

		// Token: 0x02000890 RID: 2192
		[CompilerGenerated]
		private sealed class <>c__DisplayClass162_0
		{
			// Token: 0x06004ACC RID: 19148 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass162_0()
			{
			}

			// Token: 0x06004ACD RID: 19149 RVA: 0x0037F0A4 File Offset: 0x0037D2A4
			internal void <WriteDataToECU>b__1(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
			{
				if (getDateData != null && getDateData.Length >= 3)
				{
					string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
					this.req_setDate.Command = text;
				}
			}

			// Token: 0x06004ACE RID: 19150 RVA: 0x0037F104 File Offset: 0x0037D304
			internal void <WriteDataToECU>b__2(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && data.Contains("6EF198"))
				{
					IProgress<string> progress = this.progress;
					if (progress == null)
					{
						return;
					}
					progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
				else
				{
					App.OBDReader.DebugWrite("\n2EF198_error\n");
					IProgress<string> progress2 = this.progress;
					if (progress2 == null)
					{
						return;
					}
					progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
					return;
				}
			}

			// Token: 0x06004ACF RID: 19151 RVA: 0x0037F174 File Offset: 0x0037D374
			internal void <WriteDataToECU>b__3(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
			{
				if (getCodingData != null && getCodingData.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(getCodingData);
					if (getCodingData.All((byte x) => x == 0))
					{
						text = "0181C8F63039";
					}
					if (text.Length == 6 && req_getCoding2.ForceManualFlowControl)
					{
						req_getCoding2.ForceManualFlowControl = false;
						App.OBDReader.InsertRequestInQueue(req_getCoding2);
						text = "0181C8F63039";
					}
					if (text.Length == 6)
					{
						text = "0181C8F63039";
					}
					string text2 = "2EF198" + text;
					this.req_setCodingSequence.Command = text2;
					return;
				}
				string text3 = "2EF1980181C8F63039";
				this.req_setCodingSequence.Command = text3;
				if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
				{
					req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
					App.OBDReader.InsertRequestInQueue(req_getCoding2);
				}
			}

			// Token: 0x06004AD0 RID: 19152 RVA: 0x0037F258 File Offset: 0x0037D458
			internal void <WriteDataToECU>b__4(OBDRequest req_get22F1A0_2, byte[] req_get22F1A0Data, bool req_get22F1A0DecodeResult, string req_get22F1A0ResponseHeader)
			{
				if (req_get22F1A0Data != null && req_get22F1A0Data.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(req_get22F1A0Data);
					this.req_set22F1A0.Command = "2EF1A0" + text;
				}
			}

			// Token: 0x06004AD1 RID: 19153 RVA: 0x0037F28C File Offset: 0x0037D48C
			internal void <WriteDataToECU>b__5(OBDRequest req_get22F1A1_2, byte[] req_get22F1A1Data, bool req_get22F1A1DecodeResult, string req_get22F1A1ResponseHeader)
			{
				if (req_get22F1A1Data != null && req_get22F1A1Data.Length != 0)
				{
					string text = BitHelpers.ByteArrayToHexString(req_get22F1A1Data);
					this.req_set22F1A1.Command = "2EF1A1" + text;
				}
			}

			// Token: 0x06004AD2 RID: 19154 RVA: 0x0037F2C0 File Offset: 0x0037D4C0
			internal void <WriteDataToECU>b__7(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					uint num = BitConverter.ToUInt32(getSeedData, 0);
					if (num == 0U)
					{
						this.zero_seed = true;
					}
					if (!this.zero_seed)
					{
						string text = (num + this.uint_pass).ToString("X8");
						this.req_sendKey.Command = "2704" + text;
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						this.req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					this.req_sendKey.Command = "3E";
				}
			}

			// Token: 0x06004AD3 RID: 19155 RVA: 0x0037F384 File Offset: 0x0037D584
			internal void <WriteDataToECU>b__8(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress = this.progress;
						if (progress == null)
						{
							return;
						}
						progress.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else if (!(SharedSettings.Current.IgnoreCodingErrors | this.zero_seed))
					{
						this.codingResult = CodingRequestResult.WrongAccessKey;
						IProgress<string> progress2 = this.progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_WrongPassword"));
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						this.semaphore.Release();
					}
				}
			}

			// Token: 0x06004AD4 RID: 19156 RVA: 0x0037F43C File Offset: 0x0037D63C
			internal void <WriteDataToECU>b__9(OBDRequest request, string data)
			{
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && (data.Contains(this.<>4__this.WRITE_POSTIVE_RESPONSE + this.<>4__this.Address) || data.Contains("037F2E78")))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					else if (data.Contains("NO DATA"))
					{
						this.codingResult = CodingRequestResult.NoData;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 11 && data.Contains("7F2E"))
						{
							int num = data.IndexOf("7F2E");
							int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								this.codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								this.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								this.codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							this.codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x06004AD5 RID: 19157 RVA: 0x0037F581 File Offset: 0x0037D781
			internal OBDRequest <WriteDataToECU>b__11(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004AD6 RID: 19158 RVA: 0x0037F581 File Offset: 0x0037D781
			internal OBDRequest <WriteDataToECU>b__12(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004AD7 RID: 19159 RVA: 0x0037F5B3 File Offset: 0x0037D7B3
			internal void <WriteDataToECU>b__13(OBDRequest postreq, string postdata)
			{
				this.postWriteSemaphore.Release();
			}

			// Token: 0x04002B78 RID: 11128
			public OBDRequest req_setDate;

			// Token: 0x04002B79 RID: 11129
			public IProgress<string> progress;

			// Token: 0x04002B7A RID: 11130
			public OBDRequest req_setCodingSequence;

			// Token: 0x04002B7B RID: 11131
			public OBDRequest req_set22F1A0;

			// Token: 0x04002B7C RID: 11132
			public OBDRequest req_set22F1A1;

			// Token: 0x04002B7D RID: 11133
			public bool zero_seed;

			// Token: 0x04002B7E RID: 11134
			public uint uint_pass;

			// Token: 0x04002B7F RID: 11135
			public OBDRequest req_sendKey;

			// Token: 0x04002B80 RID: 11136
			public CodingRequestResult codingResult;

			// Token: 0x04002B81 RID: 11137
			public SemaphoreSlim semaphore;

			// Token: 0x04002B82 RID: 11138
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002B83 RID: 11139
			public SemaphoreSlim postWriteSemaphore;
		}

		// Token: 0x02000891 RID: 2193
		[CompilerGenerated]
		private sealed class <>c__DisplayClass163_0
		{
			// Token: 0x06004AD8 RID: 19160 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass163_0()
			{
			}

			// Token: 0x06004AD9 RID: 19161 RVA: 0x0037F5C1 File Offset: 0x0037D7C1
			internal bool <CheckForSuccessfullResponse>b__0(string line)
			{
				return line.StartsWith(this.responseHeader) && line.IndexOf(this.req.ResponseMarker, 3) >= 0;
			}

			// Token: 0x06004ADA RID: 19162 RVA: 0x0037F5EB File Offset: 0x0037D7EB
			internal bool <CheckForSuccessfullResponse>b__1(string x)
			{
				return x.Contains(this.nr78pattern);
			}

			// Token: 0x04002B84 RID: 11140
			public string responseHeader;

			// Token: 0x04002B85 RID: 11141
			public OBDRequest req;

			// Token: 0x04002B86 RID: 11142
			public string nr78pattern;
		}

		// Token: 0x02000892 RID: 2194
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__161 : IAsyncStateMachine
		{
			// Token: 0x06004ADB RID: 19163 RVA: 0x0037F5FC File Offset: 0x0037D7FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAdaptationTemplate mqbadaptationTemplate = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					IProgress<string> progress;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						goto IL_0133;
					}
					case 2:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0552;
					}
					default:
						checked_value = "";
						if (originalData != null)
						{
							goto IL_0155;
						}
						progress = progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
						}
						taskAwaiter = mqbadaptationTemplate.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAdaptationTemplate.<Execute>d__161>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					Tuple<byte[], CodingRequestResult> tuple = taskAwaiter.GetResult();
					if (tuple.Item2 == CodingRequestResult.Success)
					{
						goto IL_0149;
					}
					if (string.IsNullOrEmpty(password) || tuple.Item2 != CodingRequestResult.WrongAccessKey)
					{
						codingRequestResult = tuple.Item2;
						goto IL_057C;
					}
					taskAwaiter = mqbadaptationTemplate.GetCurrentStateRawData(password).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAdaptationTemplate.<Execute>d__161>(ref taskAwaiter, ref this);
						return;
					}
					IL_0133:
					tuple = taskAwaiter.GetResult();
					IL_0149:
					originalData = tuple.Item1;
					IL_0155:
					if (string.IsNullOrEmpty(value))
					{
						checked_value = "";
					}
					else
					{
						switch (mqbadaptationTemplate.ValueType)
						{
						case AdaptationValueTypes.OptionType:
						{
							byte[] array = originalData;
							byte[] array2 = new byte[array.Length];
							Array.Copy(array, 0, array2, 0, array.Length);
							if (value.StartsWith("BIT:"))
							{
								try
								{
									string[] array3 = value.Substring(4).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
									for (int i = 0; i < array3.Length; i++)
									{
										string[] array4 = array3[i].Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
										int num3 = int.Parse(array4[0]);
										int num4 = int.Parse(array4[1]);
										bool flag = int.Parse(array4[2]) == 1;
										BitHelpers.SwitchBitInByte(array2, num3, num4, flag);
									}
									goto IL_0297;
								}
								catch (Exception)
								{
									goto IL_0297;
								}
							}
							byte[] array5 = BitHelpers.ConvertHexToBytesX(value);
							try
							{
								Array.Copy(array, 0, array2, 0, array.Length);
								Array.Copy(array5, 0, array2, mqbadaptationTemplate.StartByteId, array5.Length);
							}
							catch (Exception)
							{
								codingRequestResult = CodingRequestResult.InitialDataIncorrect;
								goto IL_057C;
							}
							IL_0297:
							string text = BitHelpers.ByteArrayToHexString(array2);
							if (text.Length % 2 != 0)
							{
								text = "0" + text;
							}
							checked_value = text;
							break;
						}
						case AdaptationValueTypes.InputValueType:
						{
							byte[] array6 = mqbadaptationTemplate.ConvertInputValueToByteArray(value);
							if (array6.Length == 0)
							{
								codingRequestResult = CodingRequestResult.WrongInputValue;
								goto IL_057C;
							}
							byte[] array7 = new byte[originalData.Length];
							try
							{
								Array.Copy(originalData, 0, array7, 0, array7.Length);
								Array.Copy(array6, 0, array7, mqbadaptationTemplate.StartByteId, mqbadaptationTemplate.DataLength);
							}
							catch (Exception)
							{
								codingRequestResult = CodingRequestResult.InitialDataIncorrect;
								goto IL_057C;
							}
							checked_value = BitHelpers.ByteArrayToHexString(array7);
							break;
						}
						case AdaptationValueTypes.InputHexDataType:
						case AdaptationValueTypes.MQBLightConfiguration:
						case AdaptationValueTypes.MQBColorList:
						{
							string text2 = OBDDataReader.FilterHexAndNewLineOnly(value).Replace("\r", "").Replace("\n", "")
								.Trim();
							if (text2.Length % 2 != 0)
							{
								codingRequestResult = CodingRequestResult.WrongInputValue;
								goto IL_057C;
							}
							checked_value = text2;
							break;
						}
						case AdaptationValueTypes.InputTextType:
						{
							byte[] array8 = Encoding.ASCII.GetBytes(value);
							if (array8.Length == 0)
							{
								codingRequestResult = CodingRequestResult.WrongInputValue;
								goto IL_057C;
							}
							if (array8.Length > mqbadaptationTemplate.DataLength)
							{
								array8 = array8.Take(mqbadaptationTemplate.DataLength).ToArray<byte>();
							}
							else if (array8.Length < mqbadaptationTemplate.DataLength)
							{
								byte[] array9 = new byte[mqbadaptationTemplate.DataLength - array8.Length];
								for (int j = 0; j < array9.Length; j++)
								{
									array9[j] = 0;
								}
								array8 = array8.Concat(array9).ToArray<byte>();
							}
							byte[] array10 = new byte[originalData.Length];
							try
							{
								Array.Copy(originalData, 0, array10, 0, array10.Length);
								Array.Copy(array8, 0, array10, mqbadaptationTemplate.StartByteId, mqbadaptationTemplate.DataLength);
							}
							catch (Exception)
							{
								codingRequestResult = CodingRequestResult.InitialDataIncorrect;
								goto IL_057C;
							}
							checked_value = BitHelpers.ByteArrayToHexString(array10);
							break;
						}
						}
					}
					if (skipIfTheSameData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
					{
						CodingLogItem.RecordToLog(mqbadaptationTemplate.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQB, mqbadaptationTemplate.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.ResponseHeader, "", "", "", "96", null, "");
						codingRequestResult = CodingRequestResult.Success;
						goto IL_057C;
					}
					taskAwaiter3 = mqbadaptationTemplate.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBAdaptationTemplate.<Execute>d__161>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0552:
					codingRequestResult = taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					checked_value = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_057C:
				num2 = -2;
				checked_value = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004ADC RID: 19164 RVA: 0x0037FC1C File Offset: 0x0037DE1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B87 RID: 11143
			public int <>1__state;

			// Token: 0x04002B88 RID: 11144
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002B89 RID: 11145
			public byte[] originalData;

			// Token: 0x04002B8A RID: 11146
			public IProgress<string> progress;

			// Token: 0x04002B8B RID: 11147
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002B8C RID: 11148
			public string password;

			// Token: 0x04002B8D RID: 11149
			public string value;

			// Token: 0x04002B8E RID: 11150
			public bool skipIfTheSameData;

			// Token: 0x04002B8F RID: 11151
			public string UserFriendlyValue;

			// Token: 0x04002B90 RID: 11152
			private string <checked_value>5__2;

			// Token: 0x04002B91 RID: 11153
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;

			// Token: 0x04002B92 RID: 11154
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000893 RID: 2195
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__151 : IAsyncStateMachine
		{
			// Token: 0x06004ADD RID: 19165 RVA: 0x0037FC2C File Offset: 0x0037DE2C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAdaptationTemplate mqbadaptationTemplate = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBAdaptationTemplate.<>c__DisplayClass151_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.result = null;
						mqbadaptationTemplate.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest(mqbadaptationTemplate.READ_MODE + mqbadaptationTemplate.Address, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
						string.IsNullOrEmpty(SharedSettings.Current.GetATST());
						OBDRequest obdrequest2 = new OBDRequest(mqbadaptationTemplate.OpenDiagnosticSessionCommand, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
						OBDRequest obdrequest3 = null;
						CS$<>8__locals1.req_sendKey = null;
						CS$<>8__locals1.uint_pass = 0U;
						if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out CS$<>8__locals1.uint_pass))
						{
							CS$<>8__locals1.req_sendKey = new OBDRequest("2704", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							obdrequest3 = new OBDRequest("2703", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							obdrequest3.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
							{
								try
								{
									if (BitConverter.IsLittleEndian)
									{
										Array.Reverse<byte>(getSeedData);
									}
									string text = (BitConverter.ToUInt32(getSeedData, 0) + CS$<>8__locals1.uint_pass).ToString("X8");
									CS$<>8__locals1.req_sendKey.Command = "2704" + text;
								}
								catch (Exception)
								{
								}
							};
							CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data == null || !data.Contains("6704"))
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								}
							};
						}
						obdrequest.CheckLength = true;
						obdrequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						if (obdrequest.Header == "7E0" || obdrequest.Header == "7E1")
						{
							obdrequest.ForceManualFlowControl = false;
						}
						obdrequest.ELMFormat = ELMFormat.CAN11bit;
						obdrequest.ResponseReceived += delegate(OBDRequest getStateReqest2, string response)
						{
							int negativeCodeFromResponse = CS$<>8__locals1.<>4__this.GetNegativeCodeFromResponse(getStateReqest2, response);
							if (negativeCodeFromResponse > 0 && negativeCodeFromResponse != 120)
							{
								if (negativeCodeFromResponse <= 49)
								{
									if (negativeCodeFromResponse - 17 > 1)
									{
										if (negativeCodeFromResponse == 34)
										{
											goto IL_00C1;
										}
										if (negativeCodeFromResponse != 49)
										{
											return;
										}
									}
								}
								else
								{
									if (negativeCodeFromResponse == 51)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
										return;
									}
									switch (negativeCodeFromResponse)
									{
									case 126:
									case 127:
										break;
									case 128:
									case 142:
									case 144:
										return;
									case 129:
									case 130:
									case 131:
									case 132:
									case 133:
									case 134:
									case 135:
									case 136:
									case 137:
									case 138:
									case 139:
									case 140:
									case 141:
									case 143:
									case 145:
									case 146:
									case 147:
									case 148:
										goto IL_00C1;
									default:
										if (negativeCodeFromResponse - 240 > 14)
										{
											return;
										}
										goto IL_00C1;
									}
								}
								CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								return;
								IL_00C1:
								CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
							}
						};
						obdrequest.ResponseReceived += mqbadaptationTemplate.Request_ResponseReceivedCheckForNR78;
						obdrequest.ResponseDecoded += delegate(OBDRequest getStateReqest3, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								if (CS$<>8__locals1.codingResult == CodingRequestResult.UnknownError)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
								}
							}
							else
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							CS$<>8__locals1.result = data;
						};
						List<OBDRequest> list = new List<OBDRequest>();
						list.Add(obdrequest2);
						if (obdrequest3 != null && CS$<>8__locals1.req_sendKey != null)
						{
							list.Add(obdrequest3);
							list.Add(CS$<>8__locals1.req_sendKey);
						}
						list.Add(obdrequest);
						List<OBDRequest>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest4 = enumerator.Current;
								obdrequest4.ELMFormat = ELMFormat.CAN11bit;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAdaptationTemplate.<GetCurrentStateRawData>d__151>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					if (CS$<>8__locals1.result == null)
					{
						CS$<>8__locals1.result = new byte[0];
					}
					if (CS$<>8__locals1.result.Length != 0 && mqbadaptationTemplate.DataLength == 0)
					{
						mqbadaptationTemplate.DataLength = CS$<>8__locals1.result.Length;
					}
					tuple = new Tuple<byte[], CodingRequestResult>(CS$<>8__locals1.result, CS$<>8__locals1.codingResult);
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06004ADE RID: 19166 RVA: 0x0037FFFC File Offset: 0x0037E1FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B93 RID: 11155
			public int <>1__state;

			// Token: 0x04002B94 RID: 11156
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04002B95 RID: 11157
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002B96 RID: 11158
			public string password;

			// Token: 0x04002B97 RID: 11159
			private MQBAdaptationTemplate.<>c__DisplayClass151_0 <>8__1;

			// Token: 0x04002B98 RID: 11160
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000894 RID: 2196
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__154 : IAsyncStateMachine
		{
			// Token: 0x06004ADF RID: 19167 RVA: 0x0038000C File Offset: 0x0037E20C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAdaptationTemplate mqbadaptationTemplate = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							goto IL_0113;
						}
						state = "";
						taskAwaiter = mqbadaptationTemplate.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAdaptationTemplate.<UpdateCurrentState>d__154>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> tuple = taskAwaiter.GetResult();
					if (tuple.Item2 == CodingRequestResult.Success || !string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mqbadaptationTemplate.Password) || tuple.Item2 != CodingRequestResult.WrongAccessKey)
					{
						goto IL_011B;
					}
					taskAwaiter = mqbadaptationTemplate.GetCurrentStateRawData(mqbadaptationTemplate.Password).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAdaptationTemplate.<UpdateCurrentState>d__154>(ref taskAwaiter, ref this);
						return;
					}
					IL_0113:
					tuple = taskAwaiter.GetResult();
					IL_011B:
					CodingRequestResult item = tuple.Item2;
					byte[] item2 = tuple.Item1;
					if (item != CodingRequestResult.Success)
					{
						mqbadaptationTemplate.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						switch (mqbadaptationTemplate.ValueType)
						{
						case AdaptationValueTypes.OptionType:
							state = mqbadaptationTemplate.ConvertRawDataToOptionType(item2);
							break;
						case AdaptationValueTypes.InputValueType:
							state = mqbadaptationTemplate.ConvertRawDataToInputValue(item2);
							break;
						case AdaptationValueTypes.InputHexDataType:
						case AdaptationValueTypes.MQBLightConfiguration:
						case AdaptationValueTypes.MQBColorList:
							state = BitHelpers.ByteArrayToHexString(item2);
							break;
						case AdaptationValueTypes.InputTextType:
							try
							{
								state = Encoding.ASCII.GetString(item2, mqbadaptationTemplate.StartByteId, mqbadaptationTemplate.DataLength);
							}
							catch (Exception)
							{
								state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							}
							break;
						}
						mqbadaptationTemplate.CurrentState = state;
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					state = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				state = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004AE0 RID: 19168 RVA: 0x00380268 File Offset: 0x0037E468
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B99 RID: 11161
			public int <>1__state;

			// Token: 0x04002B9A RID: 11162
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002B9B RID: 11163
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002B9C RID: 11164
			public string password;

			// Token: 0x04002B9D RID: 11165
			private string <state>5__2;

			// Token: 0x04002B9E RID: 11166
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}

		// Token: 0x02000895 RID: 2197
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__162 : IAsyncStateMachine
		{
			// Token: 0x06004AE1 RID: 19169 RVA: 0x00380278 File Offset: 0x0037E478
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAdaptationTemplate mqbadaptationTemplate = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							CS$<>8__locals1 = new MQBAdaptationTemplate.<>c__DisplayClass162_0();
							CS$<>8__locals1.progress = progress;
							CS$<>8__locals1.<>4__this = this;
							CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
							mqbadaptationTemplate.BuildDefaultBeforeAndAfterCommands();
							CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
							CS$<>8__locals1.req_setDate = new OBDRequest(string.Format("2EF199{0}{1}{2}", DateTimeNowHelper.NowSafe.Year - 2000, DateTimeNowHelper.NowSafe.Month.ToString("00"), DateTimeNowHelper.NowSafe.Day.ToString("00")), mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							CS$<>8__locals1.req_setDate.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data != null)
								{
									data.Contains("6EF199");
								}
							};
							OBDRequest obdrequest = new OBDRequest("22F199", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false)
							{
								ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
							};
							obdrequest.ResponseDecoded += delegate(OBDRequest req_getDate2, byte[] getDateData, bool getDateDecodeResult, string responseHeader)
							{
								if (getDateData != null && getDateData.Length >= 3)
								{
									string text = "2EF199" + getDateData[0].ToString("X2") + getDateData[1].ToString("X2") + getDateData[2].ToString("X2");
									CS$<>8__locals1.req_setDate.Command = text;
								}
							};
							CS$<>8__locals1.req_setCodingSequence = new OBDRequest("2EF198", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							CS$<>8__locals1.req_setCodingSequence.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data != null && data.Contains("6EF198"))
								{
									IProgress<string> progress = CS$<>8__locals1.progress;
									if (progress == null)
									{
										return;
									}
									progress.Report(Translate.GetString("coding_progress_ApplyingNewData"));
									return;
								}
								else
								{
									App.OBDReader.DebugWrite("\n2EF198_error\n");
									IProgress<string> progress2 = CS$<>8__locals1.progress;
									if (progress2 == null)
									{
										return;
									}
									progress2.Report(Translate.GetString("coding_progress_ApplyingNewData"));
									return;
								}
							};
							OBDRequest obdrequest2 = new OBDRequest("22F1A5", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false)
							{
								ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations,
								CheckLength = true
							};
							obdrequest2.ResponseDecoded += delegate(OBDRequest req_getCoding2, byte[] getCodingData, bool getCodingDataResult, string responseHeader)
							{
								if (getCodingData != null && getCodingData.Length != 0)
								{
									string text2 = BitHelpers.ByteArrayToHexString(getCodingData);
									if (getCodingData.All((byte x) => x == 0))
									{
										text2 = "0181C8F63039";
									}
									if (text2.Length == 6 && req_getCoding2.ForceManualFlowControl)
									{
										req_getCoding2.ForceManualFlowControl = false;
										App.OBDReader.InsertRequestInQueue(req_getCoding2);
										text2 = "0181C8F63039";
									}
									if (text2.Length == 6)
									{
										text2 = "0181C8F63039";
									}
									string text3 = "2EF198" + text2;
									CS$<>8__locals1.req_setCodingSequence.Command = text3;
									return;
								}
								string text4 = "2EF1980181C8F63039";
								CS$<>8__locals1.req_setCodingSequence.Command = text4;
								if (req_getCoding2.ForceManualFlowControl == SharedSettings.Current.ForceUseManualFlowControlForCodingOperations)
								{
									req_getCoding2.ForceManualFlowControl = !SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
									App.OBDReader.InsertRequestInQueue(req_getCoding2);
								}
							};
							CS$<>8__locals1.req_set22F1A0 = new OBDRequest("3E", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							OBDRequest obdrequest3 = new OBDRequest("22F1A0", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false)
							{
								CheckLength = true,
								ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
							};
							obdrequest3.ResponseDecoded += delegate(OBDRequest req_get22F1A0_2, byte[] req_get22F1A0Data, bool req_get22F1A0DecodeResult, string req_get22F1A0ResponseHeader)
							{
								if (req_get22F1A0Data != null && req_get22F1A0Data.Length != 0)
								{
									string text5 = BitHelpers.ByteArrayToHexString(req_get22F1A0Data);
									CS$<>8__locals1.req_set22F1A0.Command = "2EF1A0" + text5;
								}
							};
							CS$<>8__locals1.req_set22F1A1 = new OBDRequest("3E", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							OBDRequest obdrequest4 = new OBDRequest("22F1A1", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false)
							{
								CheckLength = true,
								ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
							};
							obdrequest4.ResponseDecoded += delegate(OBDRequest req_get22F1A1_2, byte[] req_get22F1A1Data, bool req_get22F1A1DecodeResult, string req_get22F1A1ResponseHeader)
							{
								if (req_get22F1A1Data != null && req_get22F1A1Data.Length != 0)
								{
									string text6 = BitHelpers.ByteArrayToHexString(req_get22F1A1Data);
									CS$<>8__locals1.req_set22F1A1.Command = "2EF1A1" + text6;
								}
							};
							CS$<>8__locals1.zero_seed = false;
							OBDRequest obdrequest5 = null;
							CS$<>8__locals1.req_sendKey = null;
							CS$<>8__locals1.uint_pass = 0U;
							if (!string.IsNullOrEmpty(password) && uint.TryParse(password.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out CS$<>8__locals1.uint_pass))
							{
								CS$<>8__locals1.req_sendKey = new OBDRequest("2704", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
								obdrequest5 = new OBDRequest("2703", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
								obdrequest5.ResponseReceived += delegate(OBDRequest req_getSeed2, string msg)
								{
									if (msg != null && msg.Contains("NO DATA") && !req_getSeed2.Keys.ContainsKey("SECOND_TRY"))
									{
										req_getSeed2.Keys.Add("SECOND_TRY", "1");
										App.OBDReader.InsertRequestInQueue(req_getSeed2);
									}
								};
								obdrequest5.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
								{
									try
									{
										if (BitConverter.IsLittleEndian)
										{
											Array.Reverse<byte>(getSeedData);
										}
										uint num3 = BitConverter.ToUInt32(getSeedData, 0);
										if (num3 == 0U)
										{
											CS$<>8__locals1.zero_seed = true;
										}
										if (!CS$<>8__locals1.zero_seed)
										{
											string text7 = (num3 + CS$<>8__locals1.uint_pass).ToString("X8");
											CS$<>8__locals1.req_sendKey.Command = "2704" + text7;
											IProgress<string> progress3 = CS$<>8__locals1.progress;
											if (progress3 != null)
											{
												progress3.Report(Translate.GetString("coding_progress_SendingPassword"));
											}
										}
										else
										{
											CS$<>8__locals1.req_sendKey.Command = "3E";
										}
									}
									catch (Exception)
									{
										App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
										CS$<>8__locals1.req_sendKey.Command = "3E";
									}
								};
								CS$<>8__locals1.req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
								{
									if (request.Command != "3E")
									{
										if (data != null)
										{
											data = OBDDataReader.FilterHexAndNewLineOnly(data);
										}
										if ((data != null && data.Contains("6704")) || SharedSettings.Current.IgnoreCodingErrors)
										{
											IProgress<string> progress4 = CS$<>8__locals1.progress;
											if (progress4 == null)
											{
												return;
											}
											progress4.Report(Translate.GetString("coding_progress_SuccessPassword"));
											return;
										}
										else if (!(SharedSettings.Current.IgnoreCodingErrors | CS$<>8__locals1.zero_seed))
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
											IProgress<string> progress5 = CS$<>8__locals1.progress;
											if (progress5 != null)
											{
												progress5.Report(Translate.GetString("coding_progress_WrongPassword"));
											}
											App.OBDReader.ReplaceQueue(new OBDRequest[0]);
											CS$<>8__locals1.semaphore.Release();
										}
									}
								};
							}
							OBDRequest obdrequest6 = new OBDRequest(mqbadaptationTemplate.WRITE_MODE + mqbadaptationTemplate.Address + checked_value, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							obdrequest6.ResponseReceived += delegate(OBDRequest request, string data)
							{
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data != null && (data.Contains(CS$<>8__locals1.<>4__this.WRITE_POSTIVE_RESPONSE + CS$<>8__locals1.<>4__this.Address) || data.Contains("037F2E78")))
								{
									IProgress<string> progress6 = CS$<>8__locals1.progress;
									if (progress6 != null)
									{
										progress6.Report(Translate.GetString("coding_progress_DataAccepted"));
									}
									CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								}
								else
								{
									IProgress<string> progress7 = CS$<>8__locals1.progress;
									if (progress7 != null)
									{
										progress7.Report(Translate.GetString("coding_progress_DataRejected"));
									}
									if (data == null)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									}
									else if (data.Contains("NO DATA"))
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.NoData;
									}
									else
									{
										data = OBDDataReader.FilterHexAndNewLineOnly(data);
										if (data.Length >= 11 && data.Contains("7F2E"))
										{
											int num4 = data.IndexOf("7F2E");
											int num5 = int.Parse(data.Substring(num4 + 4, 2), NumberStyles.HexNumber);
											if (num5 >= 128 || num5 == 34)
											{
												CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
											}
											else if (num5 == 51)
											{
												CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
											}
											else if (num5 == 49)
											{
												CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
											}
											else
											{
												CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
											}
										}
										else
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
										}
									}
								}
								CS$<>8__locals1.semaphore.Release();
							};
							OBDRequest obdrequest7 = new OBDRequest(mqbadaptationTemplate.OpenDiagnosticSessionCommand, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							OBDRequest obdrequest8 = new OBDRequest("1040", mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false);
							OBDRequest obdrequest9 = new OBDRequest("1003", "710", "ATFCSH710;ATFCSD300000;ATFCSM1;ATAL;ATCRA77A", mqbadaptationTemplate.AfterCommands, false)
							{
								DoNotDecode = true
							};
							OBDRequest obdrequest10 = new OBDRequest("3E80", "700", mqbadaptationTemplate.BeforeCommands, mqbadaptationTemplate.AfterCommands, false)
							{
								DoNotDecode = true
							};
							List<OBDRequest> list = new List<OBDRequest>();
							list.Add(obdrequest9);
							list.Add(obdrequest7);
							if (mqbadaptationTemplate.RequestHeader == "773")
							{
								list.Add(obdrequest8);
								if (SharedSettings.Current.SendTesterPresentWhileLongUploadTimeVag5f)
								{
									obdrequest6.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + mqbadaptationTemplate.RequestHeader);
								}
							}
							if (!string.IsNullOrEmpty(mqbadaptationTemplate.PreWriteCommands))
							{
								OBDRequest[] array = (from x in mqbadaptationTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								list.AddRange(array);
							}
							list.Add(obdrequest);
							obdrequest.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
							obdrequest.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
							list.Add(obdrequest10);
							list.Add(obdrequest2);
							obdrequest2.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
							obdrequest2.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
							list.Add(obdrequest10);
							if (obdrequest5 != null && CS$<>8__locals1.req_sendKey != null)
							{
								list.Add(obdrequest5);
								list.Add(CS$<>8__locals1.req_sendKey);
							}
							list.Add(CS$<>8__locals1.req_setDate);
							list.Add(CS$<>8__locals1.req_setCodingSequence);
							if (mqbadaptationTemplate.RequestHeader == "773")
							{
								list.Add(obdrequest3);
								obdrequest3.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								obdrequest3.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								list.Add(obdrequest10);
								list.Add(obdrequest4);
								obdrequest4.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								obdrequest4.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								list.Add(obdrequest10);
								list.Add(CS$<>8__locals1.req_set22F1A0);
								CS$<>8__locals1.req_set22F1A0.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								CS$<>8__locals1.req_set22F1A0.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								list.Add(obdrequest10);
								list.Add(CS$<>8__locals1.req_set22F1A1);
								CS$<>8__locals1.req_set22F1A1.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								CS$<>8__locals1.req_set22F1A1.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
								list.Add(obdrequest10);
							}
							list.Add(obdrequest6);
							if (mqbadaptationTemplate.RequestHeader == "7E0" || mqbadaptationTemplate.RequestHeader == "7E1")
							{
								obdrequest3.ForceManualFlowControl = false;
								obdrequest4.ForceManualFlowControl = false;
								obdrequest.ForceManualFlowControl = false;
								obdrequest2.ForceManualFlowControl = false;
								obdrequest6.ForceManualFlowControl = false;
							}
							CS$<>8__locals1.postWriteSemaphore = null;
							if (!string.IsNullOrEmpty(mqbadaptationTemplate.PostWriteCommands))
							{
								OBDRequest[] array2 = (from x in mqbadaptationTemplate.PostWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								if (array2.Length != 0)
								{
									CS$<>8__locals1.postWriteSemaphore = new SemaphoreSlim(0, 1);
									array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest postreq, string postdata)
									{
										CS$<>8__locals1.postWriteSemaphore.Release();
									};
								}
								foreach (OBDRequest obdrequest11 in array2)
								{
									list.Add(obdrequest10);
									list.Add(obdrequest11);
									if (obdrequest11.Command.StartsWith("22") || obdrequest11.Command.StartsWith("2E"))
									{
										obdrequest11.ResponseReceived -= MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
										obdrequest11.ResponseReceived += MQBAdaptationTemplate.RequestRetryRequestResponseDelegate;
									}
								}
							}
							List<OBDRequest>.Enumerator enumerator = list.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									OBDRequest obdrequest12 = enumerator.Current;
									obdrequest12.ELMFormat = ELMFormat.CAN11bit;
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							CodingLogItem.RecordToLog(mqbadaptationTemplate.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQB, mqbadaptationTemplate.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, mqbadaptationTemplate.RequestHeader, mqbadaptationTemplate.ResponseHeader, "", "", "", "96", null, "");
							App.OBDReader.ReplaceQueue(list);
							if (CS$<>8__locals1.postWriteSemaphore == null)
							{
								taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAdaptationTemplate.<WriteDataToECU>d__162>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_09A7;
							}
							else
							{
								taskAwaiter = Task.WhenAll(new Task[]
								{
									CS$<>8__locals1.semaphore.WaitAsync(),
									CS$<>8__locals1.postWriteSemaphore.WaitAsync()
								}).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAdaptationTemplate.<WriteDataToECU>d__162>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
						goto IL_0A39;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num = (num2 = -1);
					IL_09A7:
					taskAwaiter.GetResult();
					IL_0A39:
					codingResult = CS$<>8__locals1.codingResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x06004AE2 RID: 19170 RVA: 0x00380D3C File Offset: 0x0037EF3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B9F RID: 11167
			public int <>1__state;

			// Token: 0x04002BA0 RID: 11168
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002BA1 RID: 11169
			public IProgress<string> progress;

			// Token: 0x04002BA2 RID: 11170
			public MQBAdaptationTemplate <>4__this;

			// Token: 0x04002BA3 RID: 11171
			public string password;

			// Token: 0x04002BA4 RID: 11172
			public string checked_value;

			// Token: 0x04002BA5 RID: 11173
			public string UserFriendlyValue;

			// Token: 0x04002BA6 RID: 11174
			public byte[] originalData;

			// Token: 0x04002BA7 RID: 11175
			private MQBAdaptationTemplate.<>c__DisplayClass162_0 <>8__1;

			// Token: 0x04002BA8 RID: 11176
			private TaskAwaiter <>u__1;
		}
	}
}
