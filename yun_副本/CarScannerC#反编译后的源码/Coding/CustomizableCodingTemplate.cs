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
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Coding.DB.Haval;
using CarScannerXamarinForms.Coding.DB.HyundaiKia;
using CarScannerXamarinForms.Coding.DB.Nissan;
using CarScannerXamarinForms.Coding.DB.PSA;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200086D RID: 2157
	public class CustomizableCodingTemplate : ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06004978 RID: 18808 RVA: 0x00378FB2 File Offset: 0x003771B2
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06004979 RID: 18809 RVA: 0x00378FCC File Offset: 0x003771CC
		// (remove) Token: 0x0600497A RID: 18810 RVA: 0x00379004 File Offset: 0x00377204
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

		// Token: 0x17001677 RID: 5751
		// (get) Token: 0x0600497B RID: 18811 RVA: 0x00379039 File Offset: 0x00377239
		// (set) Token: 0x0600497C RID: 18812 RVA: 0x00379041 File Offset: 0x00377241
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

		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x0600497D RID: 18813 RVA: 0x0037904A File Offset: 0x0037724A
		// (set) Token: 0x0600497E RID: 18814 RVA: 0x00379052 File Offset: 0x00377252
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

		// Token: 0x17001679 RID: 5753
		// (get) Token: 0x0600497F RID: 18815 RVA: 0x00379066 File Offset: 0x00377266
		// (set) Token: 0x06004980 RID: 18816 RVA: 0x0037906E File Offset: 0x0037726E
		public virtual CodingGroup Group
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

		// Token: 0x1700167A RID: 5754
		// (get) Token: 0x06004981 RID: 18817 RVA: 0x00379082 File Offset: 0x00377282
		// (set) Token: 0x06004982 RID: 18818 RVA: 0x00379099 File Offset: 0x00377299
		public virtual bool HasCurrentState
		{
			get
			{
				return !string.IsNullOrEmpty(this.ReadModeAndAddress) && this._HasCurrentState;
			}
			set
			{
				this._HasCurrentState = value;
				this.OnPropertyChanged("HasCurrentState");
			}
		}

		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06004983 RID: 18819 RVA: 0x003790AD File Offset: 0x003772AD
		// (set) Token: 0x06004984 RID: 18820 RVA: 0x003790B5 File Offset: 0x003772B5
		[JsonProperty("NM")]
		public string NameRaw
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.OnPropertyChanged("Name");
				this.OnPropertyChanged("NameRaw");
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x003790D4 File Offset: 0x003772D4
		// (set) Token: 0x06004986 RID: 18822 RVA: 0x00379148 File Offset: 0x00377348
		[JsonIgnore]
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

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06004987 RID: 18823 RVA: 0x0037915C File Offset: 0x0037735C
		// (set) Token: 0x06004988 RID: 18824 RVA: 0x00379164 File Offset: 0x00377364
		[JsonProperty("Description")]
		public string DescriptionRaw
		{
			get
			{
				return this._Description;
			}
			set
			{
				this._Description = value;
				this.OnPropertyChanged("Description");
				this.OnPropertyChanged("DescriptionRaw");
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x00379184 File Offset: 0x00377384
		// (set) Token: 0x0600498A RID: 18826 RVA: 0x003791F8 File Offset: 0x003773F8
		[JsonIgnore]
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

		// Token: 0x1700167F RID: 5759
		// (get) Token: 0x0600498B RID: 18827 RVA: 0x0037920C File Offset: 0x0037740C
		// (set) Token: 0x0600498C RID: 18828 RVA: 0x00379214 File Offset: 0x00377414
		[JsonProperty("InnerDescription")]
		public string InnerDescriptionRaw
		{
			get
			{
				return this._InnerDescription;
			}
			set
			{
				this._InnerDescription = value;
				this.OnPropertyChanged("InnerDescription");
				this.OnPropertyChanged("InnerDescriptionRaw");
			}
		}

		// Token: 0x17001680 RID: 5760
		// (get) Token: 0x0600498D RID: 18829 RVA: 0x00379234 File Offset: 0x00377434
		// (set) Token: 0x0600498E RID: 18830 RVA: 0x003792A8 File Offset: 0x003774A8
		[JsonIgnore]
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

		// Token: 0x17001681 RID: 5761
		// (get) Token: 0x0600498F RID: 18831 RVA: 0x003792BC File Offset: 0x003774BC
		// (set) Token: 0x06004990 RID: 18832 RVA: 0x003792C4 File Offset: 0x003774C4
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

		// Token: 0x17001682 RID: 5762
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x003792D8 File Offset: 0x003774D8
		// (set) Token: 0x06004992 RID: 18834 RVA: 0x003792E0 File Offset: 0x003774E0
		[JsonProperty("PWDVIS")]
		public virtual bool PasswordVisible
		{
			get
			{
				return this._PasswordVisible;
			}
			set
			{
				this._PasswordVisible = value;
				this.OnPropertyChanged("PasswordVisible");
			}
		}

		// Token: 0x17001683 RID: 5763
		// (get) Token: 0x06004993 RID: 18835 RVA: 0x003792F4 File Offset: 0x003774F4
		// (set) Token: 0x06004994 RID: 18836 RVA: 0x003792FC File Offset: 0x003774FC
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

		// Token: 0x17001684 RID: 5764
		// (get) Token: 0x06004995 RID: 18837 RVA: 0x00379310 File Offset: 0x00377510
		// (set) Token: 0x06004996 RID: 18838 RVA: 0x00379318 File Offset: 0x00377518
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

		// Token: 0x17001685 RID: 5765
		// (get) Token: 0x06004997 RID: 18839 RVA: 0x0037932C File Offset: 0x0037752C
		// (set) Token: 0x06004998 RID: 18840 RVA: 0x00379334 File Offset: 0x00377534
		[JsonProperty("OPTS")]
		public virtual ObservableCollection<MQBAdaptationOption> Options
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

		// Token: 0x17001686 RID: 5766
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x0037933D File Offset: 0x0037753D
		// (set) Token: 0x0600499A RID: 18842 RVA: 0x00379345 File Offset: 0x00377545
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

		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x0600499B RID: 18843 RVA: 0x00379359 File Offset: 0x00377559
		// (set) Token: 0x0600499C RID: 18844 RVA: 0x00379361 File Offset: 0x00377561
		[JsonProperty("VT")]
		public virtual AdaptationValueTypes ValueType
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

		// Token: 0x17001688 RID: 5768
		// (get) Token: 0x0600499D RID: 18845 RVA: 0x00379375 File Offset: 0x00377575
		// (set) Token: 0x0600499E RID: 18846 RVA: 0x0037937D File Offset: 0x0037757D
		[JsonProperty("RMA")]
		public virtual string ReadModeAndAddress
		{
			get
			{
				return this._ReadModeAndAddress;
			}
			set
			{
				this._ReadModeAndAddress = value;
				this.OnPropertyChanged("ReadModeAndAddress");
			}
		}

		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x0600499F RID: 18847 RVA: 0x00379391 File Offset: 0x00377591
		// (set) Token: 0x060049A0 RID: 18848 RVA: 0x00379399 File Offset: 0x00377599
		[JsonProperty("WMA")]
		public string WriteModeAndAddress
		{
			get
			{
				return this._WriteModeAndAddress;
			}
			set
			{
				this._WriteModeAndAddress = value;
				this.OnPropertyChanged("WriteModeAndAddress");
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x060049A1 RID: 18849 RVA: 0x003793AD File Offset: 0x003775AD
		// (set) Token: 0x060049A2 RID: 18850 RVA: 0x003793B5 File Offset: 0x003775B5
		[JsonProperty("MCTID")]
		public bool MakeChangesToInitialData
		{
			get
			{
				return this._MakeChangesToInitialData;
			}
			set
			{
				this._MakeChangesToInitialData = value;
				this.OnPropertyChanged("MakeChangesToInitialData");
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x060049A3 RID: 18851 RVA: 0x003793C9 File Offset: 0x003775C9
		// (set) Token: 0x060049A4 RID: 18852 RVA: 0x003793D1 File Offset: 0x003775D1
		public string ATST
		{
			get
			{
				return this._ATST;
			}
			set
			{
				this._ATST = value;
				this.OnPropertyChanged("ATST");
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x060049A5 RID: 18853 RVA: 0x003793E5 File Offset: 0x003775E5
		// (set) Token: 0x060049A6 RID: 18854 RVA: 0x003793ED File Offset: 0x003775ED
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

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x060049A7 RID: 18855 RVA: 0x003793F6 File Offset: 0x003775F6
		// (set) Token: 0x060049A8 RID: 18856 RVA: 0x003793FE File Offset: 0x003775FE
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

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x060049A9 RID: 18857 RVA: 0x00379407 File Offset: 0x00377607
		// (set) Token: 0x060049AA RID: 18858 RVA: 0x0037940F File Offset: 0x0037760F
		public string OpenSessionCommand
		{
			get
			{
				return this._OpenSessionCommand;
			}
			set
			{
				this._OpenSessionCommand = value;
				this.OnPropertyChanged("OpenSessionCommand");
			}
		}

		// Token: 0x1700168F RID: 5775
		// (get) Token: 0x060049AB RID: 18859 RVA: 0x00379423 File Offset: 0x00377623
		// (set) Token: 0x060049AC RID: 18860 RVA: 0x0037942B File Offset: 0x0037762B
		public string CloseSessionCommand
		{
			get
			{
				return this._CloseSessionCommand;
			}
			set
			{
				this._CloseSessionCommand = value;
				this.OnPropertyChanged("CloseSessionCommand");
			}
		}

		// Token: 0x17001690 RID: 5776
		// (get) Token: 0x060049AD RID: 18861 RVA: 0x0037943F File Offset: 0x0037763F
		// (set) Token: 0x060049AE RID: 18862 RVA: 0x00379447 File Offset: 0x00377647
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

		// Token: 0x17001691 RID: 5777
		// (get) Token: 0x060049AF RID: 18863 RVA: 0x0037945B File Offset: 0x0037765B
		// (set) Token: 0x060049B0 RID: 18864 RVA: 0x00379463 File Offset: 0x00377663
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

		// Token: 0x17001692 RID: 5778
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x00379477 File Offset: 0x00377677
		// (set) Token: 0x060049B2 RID: 18866 RVA: 0x0037947F File Offset: 0x0037767F
		[JsonProperty("EA")]
		public string ExtendedAddress
		{
			get
			{
				return this._ExtendedAddress;
			}
			set
			{
				this._ExtendedAddress = value;
				this.OnPropertyChanged("ExtendedAddress");
			}
		}

		// Token: 0x17001693 RID: 5779
		// (get) Token: 0x060049B3 RID: 18867 RVA: 0x00379493 File Offset: 0x00377693
		// (set) Token: 0x060049B4 RID: 18868 RVA: 0x0037949B File Offset: 0x0037769B
		[JsonProperty("TA")]
		public string TesterAddress
		{
			get
			{
				return this._TesterAddress;
			}
			set
			{
				this._TesterAddress = value;
				this.OnPropertyChanged("TesterAddress");
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x003794AF File Offset: 0x003776AF
		public string GetRequestHeaderForELM327()
		{
			if (this.RequestHeader.Length == 8)
			{
				return this.RequestHeader.Substring(2);
			}
			return this.RequestHeader;
		}

		// Token: 0x17001694 RID: 5780
		// (get) Token: 0x060049B6 RID: 18870 RVA: 0x003794D2 File Offset: 0x003776D2
		// (set) Token: 0x060049B7 RID: 18871 RVA: 0x003794DA File Offset: 0x003776DA
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

		// Token: 0x17001695 RID: 5781
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x003794EE File Offset: 0x003776EE
		// (set) Token: 0x060049B9 RID: 18873 RVA: 0x003794F6 File Offset: 0x003776F6
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

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x0037950A File Offset: 0x0037770A
		// (set) Token: 0x060049BB RID: 18875 RVA: 0x00379512 File Offset: 0x00377712
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

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x00379526 File Offset: 0x00377726
		// (set) Token: 0x060049BD RID: 18877 RVA: 0x0037952E File Offset: 0x0037772E
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

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x00379542 File Offset: 0x00377742
		// (set) Token: 0x060049BF RID: 18879 RVA: 0x0037954A File Offset: 0x0037774A
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

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x0037955E File Offset: 0x0037775E
		// (set) Token: 0x060049C1 RID: 18881 RVA: 0x00379566 File Offset: 0x00377766
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

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x0037957A File Offset: 0x0037777A
		// (set) Token: 0x060049C3 RID: 18883 RVA: 0x00379582 File Offset: 0x00377782
		[JsonProperty("PreRead")]
		public string PreReadCommands
		{
			get
			{
				return this._PreReadCommands;
			}
			set
			{
				this._PreReadCommands = value;
				this.OnPropertyChanged("PreReadCommands");
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00379596 File Offset: 0x00377796
		// (set) Token: 0x060049C5 RID: 18885 RVA: 0x0037959E File Offset: 0x0037779E
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

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x003795B2 File Offset: 0x003777B2
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x003795BA File Offset: 0x003777BA
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

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x003795CE File Offset: 0x003777CE
		// (set) Token: 0x060049C9 RID: 18889 RVA: 0x003795D6 File Offset: 0x003777D6
		[JsonProperty("Comment")]
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this.OnPropertyChanged("Comment");
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x003795EA File Offset: 0x003777EA
		// (set) Token: 0x060049CB RID: 18891 RVA: 0x003795F2 File Offset: 0x003777F2
		[JsonProperty("Protocol")]
		public string Protocol
		{
			get
			{
				return this._Protocol;
			}
			set
			{
				this._Protocol = value;
				this.OnPropertyChanged("Protocol");
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x00379608 File Offset: 0x00377808
		public virtual ELMFormat ELMFormat
		{
			get
			{
				if (this.RequestHeader == "000")
				{
					return ELMFormat.VwTp20;
				}
				if (string.IsNullOrEmpty(this.Protocol))
				{
					return App.OBDReader.CurrentELMFormat;
				}
				string protocol = this.Protocol;
				if (protocol == "6" || protocol == "8" || protocol == "B")
				{
					return ELMFormat.CAN11bit;
				}
				if (!(protocol == "7") && !(protocol == "9"))
				{
					return ELMFormat.Unknown;
				}
				return ELMFormat.CAN29bit;
			}
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x00379690 File Offset: 0x00377890
		protected virtual void BuildDefaultBeforeAndAfterCommands()
		{
			string text = (string.IsNullOrEmpty(this.Protocol) ? "" : ("ATSP" + this.Protocol + ";"));
			string text2 = ((this.RequestHeader.Length == 8) ? ("ATCP" + this.RequestHeader.Substring(0, 2) + ";") : "");
			if (string.IsNullOrEmpty(this.ExtendedAddress))
			{
				this.BeforeCommands = string.Concat(new string[] { text, text2, "ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", this.ResponseHeader, ";ATST", this.ATST });
				this.AfterCommands = string.Format("ATFCSM0;ATD;ATSP{0};ATE0;ATH1;ATS0;ATSTDEF", SharedSettings.Current.ProtocolNumber);
			}
			else
			{
				this.BeforeCommands = string.Concat(new string[]
				{
					text, text2, "ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", this.ResponseHeader, ";ATCEA", this.ExtendedAddress, ";ATTA", this.ExtendedAddress,
					";ATST", this.ATST
				});
				this.AfterCommands = string.Format("ATFCSM0;ATD;ATSP{0};ATE0;ATH1;ATS0;ATSTDEF", SharedSettings.Current.ProtocolNumber);
			}
			if (SharedSettings.Current.ShowExperimental && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (this.Protocol == "" || this.Protocol == "6"))
			{
				if (string.IsNullOrEmpty(this.ExtendedAddress))
				{
					this.AfterCommands = "ATFCSM0;ATCAF1;ATAR;ATSTDEF";
				}
				else
				{
					this.AfterCommands = "ATFCSM0;ATCAF1;ATAR;ATCEA;ATSTDEF";
				}
			}
			if (this.RequestHeader == "000")
			{
				this.BeforeCommands = "ATCAF0;ATV1;ATSP6;ATCM000";
				this.AfterCommands = "ATSPDEF;ATCAF1;ATV0";
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x00379894 File Offset: 0x00377A94
		public virtual async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string checked_value = "";
			if (this.MakeChangesToInitialData && originalData == null)
			{
				if (progress != null)
				{
					progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
				}
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData("");
				if (tuple.Item2 != CodingRequestResult.Success)
				{
					return tuple.Item2;
				}
				originalData = tuple.Item1;
			}
			if (string.IsNullOrEmpty(value))
			{
				checked_value = "";
			}
			else
			{
				if (this.MakeChangesToInitialData)
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
								goto IL_0244;
							}
							catch (Exception)
							{
								goto IL_0244;
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
						IL_0244:
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
					case AdaptationValueTypes.TPMS:
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
					case AdaptationValueTypes.InputFloatIEEE754:
					{
						byte[] bytes = BitConverter.GetBytes(float.Parse(value.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture));
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(bytes);
						}
						if (bytes.Length == 0)
						{
							return CodingRequestResult.WrongInputValue;
						}
						byte[] array11 = new byte[originalData.Length];
						try
						{
							Array.Copy(originalData, 0, array11, 0, array11.Length);
							Array.Copy(bytes, 0, array11, this.StartByteId, this.DataLength);
						}
						catch (Exception)
						{
							return CodingRequestResult.InitialDataIncorrect;
						}
						checked_value = BitHelpers.ByteArrayToHexString(array11);
						break;
					}
					}
					if (this.PreWriteDataProcessor == null)
					{
						goto IL_0612;
					}
					try
					{
						byte[] array12 = BitHelpers.ConvertHexToBytesX(checked_value);
						checked_value = BitHelpers.ByteArrayToHexString(this.PreWriteDataProcessor.ProcessData(array12));
						goto IL_0612;
					}
					catch (Exception)
					{
						goto IL_0612;
					}
				}
				AdaptationValueTypes valueType = this.ValueType;
				switch (valueType)
				{
				case AdaptationValueTypes.OptionType:
				case AdaptationValueTypes.InputHexDataType:
				case AdaptationValueTypes.ToyotaTPMSSensor:
					break;
				case AdaptationValueTypes.InputValueType:
				{
					byte[] array13 = this.ConvertInputValueToByteArray(value);
					if (array13.Length == 0)
					{
						return CodingRequestResult.WrongInputValue;
					}
					checked_value = BitHelpers.ByteArrayToHexString(array13);
					goto IL_0612;
				}
				case AdaptationValueTypes.MQBLightConfiguration:
				case AdaptationValueTypes.MQBColorList:
				case AdaptationValueTypes.MQBParametrizeDump:
				case AdaptationValueTypes.MQBFPAEditor:
					goto IL_0612;
				case AdaptationValueTypes.InputTextType:
					checked_value = BitHelpers.ByteArrayToHexString(Encoding.ASCII.GetBytes(value));
					goto IL_0612;
				default:
					if (valueType != AdaptationValueTypes.InputFloatIEEE754)
					{
						if (valueType != AdaptationValueTypes.TPMS)
						{
							goto IL_0612;
						}
					}
					else
					{
						byte[] bytes2 = BitConverter.GetBytes(float.Parse(value.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture));
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(bytes2);
						}
						if (bytes2.Length == 0)
						{
							return CodingRequestResult.WrongInputValue;
						}
						checked_value = BitHelpers.ByteArrayToHexString(bytes2);
						goto IL_0612;
					}
					break;
				}
				checked_value = value;
			}
			IL_0612:
			CodingRequestResult codingRequestResult;
			if (skipIfTheSameData && this.MakeChangesToInitialData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Protocol", this.Protocol);
				dictionary.Add("ExtendedAddress", this.ExtendedAddress);
				dictionary.Add("TesterAddress", this.TesterAddress);
				CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, this.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, this.RequestHeader, this.ResponseHeader, this.OpenSessionCommand, this.PreWriteCommands, this.PostWriteCommands, this.ATST, dictionary, this.PreReadCommands);
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
			}
			return codingRequestResult;
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0037990C File Offset: 0x00377B0C
		public virtual async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			string state = "";
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.ReadModeAndAddress))
			{
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					this.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else
				{
					switch (this.ValueType)
					{
					case AdaptationValueTypes.OptionType:
						state = this.ConvertRawDataToOptionType(item2);
						goto IL_01F6;
					case AdaptationValueTypes.InputValueType:
						state = this.ConvertRawDataToInputValue(item2);
						goto IL_01F6;
					case AdaptationValueTypes.InputHexDataType:
					case AdaptationValueTypes.MQBLightConfiguration:
					case AdaptationValueTypes.MQBColorList:
						state = BitHelpers.ByteArrayToHexString(item2);
						goto IL_01F6;
					case AdaptationValueTypes.MQBParametrizeDump:
					case AdaptationValueTypes.MQBFPAEditor:
					case AdaptationValueTypes.ToyotaTPMSSensor:
					case AdaptationValueTypes.MQBRKDSGenerator:
					case AdaptationValueTypes.MQBA5Customization:
						goto IL_01F6;
					case AdaptationValueTypes.InputTextType:
						try
						{
							if (this.DataLength == 0)
							{
								state = Encoding.ASCII.GetString(item2, this.StartByteId, item2.Length - this.StartByteId);
							}
							else
							{
								state = Encoding.ASCII.GetString(item2, this.StartByteId, this.DataLength);
							}
							goto IL_01F6;
						}
						catch (Exception)
						{
							state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							goto IL_01F6;
						}
						break;
					case AdaptationValueTypes.InputFloatIEEE754:
						break;
					default:
						goto IL_01F6;
					}
					if (item2.Length < this.StartByteId + this.DataLength)
					{
						state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					}
					else
					{
						byte[] array = new byte[4];
						Array.Copy(item2, this.StartByteId, array, 0, array.Length);
						if (BitConverter.IsLittleEndian)
						{
							Array.Reverse<byte>(array);
						}
						state = BitConverter.ToSingle(array, 0).ToString(CultureInfo.InvariantCulture);
					}
					IL_01F6:
					this.CurrentState = state;
					codingRequestResult = CodingRequestResult.Success;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x00379958 File Offset: 0x00377B58
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

		// Token: 0x060049D1 RID: 18897 RVA: 0x00379A00 File Offset: 0x00377C00
		private void SetRenaultOpenSessionFix(OBDRequest openSessionRequest)
		{
			if (openSessionRequest == null)
			{
				return;
			}
			if ((SharedSettings.Current.SelectedBrand == "Renault" || SharedSettings.Current.SelectedBrand == "Лада" || SharedSettings.Current.SelectedBrand == "Lada" || SharedSettings.Current.SelectedBrand == "Dacia" || SharedSettings.Current.SelectedBrand == "VAZ" || SharedSettings.Current.SelectedBrand == "ВАЗ") && openSessionRequest.Command == "10C0")
			{
				openSessionRequest.ResponseReceived += this.OpenSessionRequest_RenaulFix_ResponseReceived;
			}
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x00379ABC File Offset: 0x00377CBC
		private void OpenSessionRequest_RenaulFix_ResponseReceived(OBDRequest request, string data)
		{
			if (data != null && OBDDataReader.FilterHexAndNewLineOnly(data).Contains("7F1012"))
			{
				this.OpenSessionCommand = "1003";
				request.Command = "1003";
				request.ResponseMarker = "50";
				request.ResponseReceived -= this.OpenSessionRequest_RenaulFix_ResponseReceived;
				App.OBDReader.InsertRequestInQueue(request);
			}
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x00379B1C File Offset: 0x00377D1C
		public virtual async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest getStateReqest = new OBDRequest(this.ReadModeAndAddress, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			string text = SharedSettings.Current.GetATST();
			if (string.IsNullOrEmpty(text))
			{
				text = "32";
			}
			OBDRequest getStateReqestforNR78 = new OBDRequest(this.ReadModeAndAddress, this.GetRequestHeaderForELM327(), "ATAT0;ATSTFF;" + this.BeforeCommands, string.Concat(new string[]
			{
				"ATAT",
				SharedSettings.Current.AdaptiveTimings.ToString(),
				";ATST",
				text,
				";",
				this.AfterCommands
			}), false);
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest(this.OpenSessionCommand, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			this.SetRenaultOpenSessionFix(obdrequest);
			if (!SharedSettings.Current.ShowExperimental)
			{
				getStateReqest.ResponseReceived += delegate(OBDRequest request, string data)
				{
					string text2 = "7F" + request.Command.Substring(0, 2) + "78";
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if (data != null && data.IndexOf(text2) >= 0)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
						{
							getStateReqest.DoNotDecode = true;
							App.OBDReader.AddRequestToQueue(getStateReqestforNR78);
						}
					}
				};
			}
			else
			{
				getStateReqest.ResponseReceived += this.Request_ResponseReceivedCheckForNR78;
			}
			byte[] result = null;
			getStateReqest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					codingResult = CodingRequestResult.Success;
				}
				result = data;
				semaphore.Release();
			};
			getStateReqestforNR78.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					codingResult = CodingRequestResult.Success;
				}
				result = data;
				semaphore.Release();
			};
			getStateReqest.CheckLength = true;
			getStateReqestforNR78.CheckLength = true;
			List<OBDRequest> list = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(obdrequest.Command))
			{
				list.Add(obdrequest);
			}
			if (!string.IsNullOrEmpty(this.PreReadCommands))
			{
				if (VagUnitHelper.IsVag(SharedSettings.Current.SelectedBrand))
				{
					List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(this.PreReadCommands, password, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands);
					list.AddRange(requestsFromStringCommands);
				}
				else
				{
					OBDRequest[] array = (from x in this.PreReadCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					list.AddRange(array);
				}
			}
			list.Add(getStateReqest);
			foreach (OBDRequest obdrequest2 in list)
			{
				obdrequest2.ELMFormat = this.ELMFormat;
				obdrequest2.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
				string selectedBrand = SharedSettings.Current.SelectedBrand;
				if (selectedBrand == "Kia" || selectedBrand == "Hyundai" || selectedBrand == "Genesis")
				{
					obdrequest2.ForceManualFlowControl = false;
				}
				if (VagUnitHelper.IsVag(selectedBrand) && (obdrequest2.Header == "7E0" || obdrequest2.Header == "7E1"))
				{
					obdrequest2.ForceManualFlowControl = false;
				}
			}
			App.OBDReader.ReplaceQueue(list);
			await semaphore.WaitAsync();
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

		// Token: 0x060049D4 RID: 18900 RVA: 0x00379B68 File Offset: 0x00377D68
		public void Request_ResponseReceivedCheckForNR78(OBDRequest request, string data)
		{
			request.DoNotDecode = false;
			if (data == null)
			{
				return;
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			string text2 = request.Command;
			if (text2.StartsWith("VWTP:", StringComparison.OrdinalIgnoreCase))
			{
				int num = text2.IndexOf(':', 5);
				if (num > 0)
				{
					text2 = text2.Substring(num + 1);
				}
			}
			string text3 = text2.Substring(0, 2);
			"037F" + text3 + "11";
			string nr78_string = "037F" + text3 + "78";
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
					string text4 = SharedSettings.Current.GetATST();
					if (string.IsNullOrEmpty(text4))
					{
						text4 = "32";
					}
					array2[array2.Length - 1] = "ATST" + text4;
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
							string text5 = OBDDataReader.FilterHexAndNewLineOnly(response);
							string[] array3 = text5.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
							if (text5.Contains(nr78_string) && array3.Length == 1)
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

		// Token: 0x060049D5 RID: 18901 RVA: 0x00379DC4 File Offset: 0x00377FC4
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

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x00379E71 File Offset: 0x00378071
		// (set) Token: 0x060049D7 RID: 18903 RVA: 0x00379E79 File Offset: 0x00378079
		[JsonIgnore]
		internal IDataPreprocessor PreWriteDataProcessor
		{
			[CompilerGenerated]
			get
			{
				return this.<PreWriteDataProcessor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PreWriteDataProcessor>k__BackingField = value;
			}
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x00379E84 File Offset: 0x00378084
		protected virtual async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest(this.WriteModeAndAddress + checked_value, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest(this.OpenSessionCommand, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false);
			this.SetRenaultOpenSessionFix(obdrequest2);
			List<OBDRequest> list = new List<OBDRequest>();
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (!string.IsNullOrEmpty(obdrequest2.Command))
			{
				list.Add(obdrequest2);
			}
			if (!string.IsNullOrEmpty(this.PreWriteCommands))
			{
				if (VagUnitHelper.IsVag(selectedBrand))
				{
					List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(this.PreWriteCommands, password, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands);
					list.AddRange(requestsFromStringCommands);
				}
				else if (selectedBrand == "Haval" || selectedBrand == "Hover" || selectedBrand == "Great Wall")
				{
					OBDRequest[] array = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					OBDRequest[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						DashboardPasswordGenerator.SetPasswordDecodeFor2701(array2[i]);
					}
					list.AddRange(array);
				}
				else if (selectedBrand == "Nissan" && SharedSettings.Current.ProfileUpdateAlias == "7e2905bfa3eb488bad22f6fd955496f5")
				{
					OBDRequest[] array3 = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					OBDRequest[] array2 = array3;
					for (int i = 0; i < array2.Length; i++)
					{
						NissanLeafBatterySoHCountersReset.SetPasswordDecodeFor2701(array2[i]);
					}
					list.AddRange(array3);
				}
				else if (selectedBrand == "Hyundai" || selectedBrand == "Genesis" || selectedBrand == "Kia")
				{
					OBDRequest[] array4 = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					foreach (OBDRequest obdrequest3 in array4)
					{
						KiaUMCRDI27Generator.SetPasswordDecodeFor2701(obdrequest3);
						KiaDashboardSeed27Generator.SetPasswordDecodeFor2701(obdrequest3);
						KiaBCM27Generator.SetPasswordDecodeFor2711(obdrequest3);
					}
					list.AddRange(array4);
				}
				else if (selectedBrand == "Peugeot" || selectedBrand == "Citroen" || selectedBrand == "DS")
				{
					OBDRequest[] array5 = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					OBDRequest[] array2 = array5;
					for (int i = 0; i < array2.Length; i++)
					{
						PSA_SeedKeyGenerator.SetPasswordDecodeFor2703(array2[i], password);
					}
					list.AddRange(array5);
				}
				else
				{
					OBDRequest[] array6 = (from x in this.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>();
					list.AddRange(array6);
				}
			}
			list.Add(obdrequest);
			List<OBDRequest> postWriteRequests = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(this.PostWriteCommands))
			{
				if (VagUnitHelper.IsVag(selectedBrand))
				{
					List<OBDRequest> requestsFromStringCommands2 = MQBParametrizeBase.GetRequestsFromStringCommands(this.PostWriteCommands, password, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands);
					postWriteRequests.AddRange(requestsFromStringCommands2);
				}
				else
				{
					postWriteRequests.AddRange((from x in this.PostWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
						select new OBDRequest(x, this.GetRequestHeaderForELM327(), this.BeforeCommands, this.AfterCommands, false, null)
						{
							DoNotDecode = true
						}).ToArray<OBDRequest>());
				}
			}
			foreach (OBDRequest obdrequest4 in list)
			{
				obdrequest4.ELMFormat = this.ELMFormat;
			}
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				string text = request.Command;
				if (text == null)
				{
					return;
				}
				if (text.StartsWith("VWTP:", StringComparison.OrdinalIgnoreCase))
				{
					int num = text.IndexOf(':', 5);
					if (num > 0)
					{
						text = text.Substring(num + 1);
					}
				}
				string text2 = text.Substring(0, 2);
				string text3 = (int.Parse(text2, NumberStyles.HexNumber) + 64).ToString("X2");
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				string text4 = text3;
				if (this.WriteModeAndAddress != null && this.WriteModeAndAddress.Length >= 2)
				{
					text4 += this.WriteModeAndAddress.Substring(2);
				}
				if (data != null && (data.Contains(text4) || data.Contains("037F" + text2 + "78") || data.Contains(request.ResponseMarker) || MQBEasyCodingItem.MatchPattern(data, string.Concat(new string[] { "*", this.ResponseHeader, "*", text3, "*" }))))
				{
					IProgress<string> progress2 = progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					codingResult = CodingRequestResult.Success;
					if (postWriteRequests.Count > 0)
					{
						foreach (OBDRequest obdrequest6 in postWriteRequests)
						{
							obdrequest6.DoNotDecode = true;
						}
						App.OBDReader.ReplaceQueue(postWriteRequests);
						return;
					}
				}
				else
				{
					IProgress<string> progress3 = progress;
					if (progress3 != null)
					{
						progress3.Report(Translate.GetString("coding_progress_DataRejected"));
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
						if (data.Length >= 11 && data.Contains("7F" + text2))
						{
							int num2 = data.IndexOf("7F" + text2);
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
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			};
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			if (codingResult == CodingRequestResult.Success && this.MakeChangesToInitialData && !string.IsNullOrEmpty(this.ReadModeAndAddress) && this.WriteModeAndAddress.Substring(2) == this.ReadModeAndAddress.Substring(2))
			{
				if (this.PreWriteDataProcessor != null)
				{
					originalData = this.PreWriteDataProcessor.ProcessData(originalData);
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Protocol", this.Protocol);
				CodingLogItem.RecordToLog(this.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, this.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, this.RequestHeader, this.ResponseHeader, this.OpenSessionCommand, this.PreWriteCommands, this.PostWriteCommands, this.ATST, dictionary, this.PreReadCommands);
			}
			if (string.IsNullOrEmpty(this.ReadModeAndAddress) && postWriteRequests.Count > 0 && postWriteRequests[postWriteRequests.Count - 1] != null && postWriteRequests[postWriteRequests.Count - 1].Command.StartsWith("3E"))
			{
				OBDRequest obdrequest5 = postWriteRequests[postWriteRequests.Count - 1];
				obdrequest5.Repeat = true;
				App.OBDReader.ReplaceQueue(obdrequest5);
			}
			return codingResult;
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x00379EF4 File Offset: 0x003780F4
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

		// Token: 0x060049DA RID: 18906 RVA: 0x0037A114 File Offset: 0x00378314
		private string ConvertRawDataToOptionType(byte[] data)
		{
			foreach (MQBAdaptationOption mqbadaptationOption in this.Options)
			{
				string text = mqbadaptationOption.Value;
				if (this.MakeChangesToInitialData)
				{
					if (!string.IsNullOrEmpty(mqbadaptationOption.ReadOnlyValue))
					{
						text = mqbadaptationOption.ReadOnlyValue;
					}
				}
				else
				{
					text = mqbadaptationOption.ReadOnlyValue;
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (text.StartsWith("BIT:") || text.StartsWith("BITS:"))
					{
						try
						{
							string[] array = text.Substring(text.IndexOf(':') + 1).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
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
						byte[] array4 = BitHelpers.ConvertHexToBytesX(text);
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

		// Token: 0x060049DB RID: 18907 RVA: 0x0037A2E4 File Offset: 0x003784E4
		private string GetPositiveResponse(string mode)
		{
			int num = BitHelpers.ConvertHexToInt(mode);
			return (num + 64).ToString("X2");
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x0037A30C File Offset: 0x0037850C
		public CustomizableCodingTemplate()
		{
		}

		// Token: 0x04002A77 RID: 10871
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002A78 RID: 10872
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002A79 RID: 10873
		private int _UID;

		// Token: 0x04002A7A RID: 10874
		private CodingGroup _Group;

		// Token: 0x04002A7B RID: 10875
		private bool _HasCurrentState = true;

		// Token: 0x04002A7C RID: 10876
		private string _Name = "";

		// Token: 0x04002A7D RID: 10877
		private string _Description = "";

		// Token: 0x04002A7E RID: 10878
		private string _InnerDescription = "";

		// Token: 0x04002A7F RID: 10879
		private string _CurrentState = "";

		// Token: 0x04002A80 RID: 10880
		private bool _PasswordVisible = true;

		// Token: 0x04002A81 RID: 10881
		private string _Password = "";

		// Token: 0x04002A82 RID: 10882
		private string _PasswordHint = "";

		// Token: 0x04002A83 RID: 10883
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04002A84 RID: 10884
		private bool _RequiresPro;

		// Token: 0x04002A85 RID: 10885
		private AdaptationValueTypes _ValueType;

		// Token: 0x04002A86 RID: 10886
		private string _ReadModeAndAddress = "22";

		// Token: 0x04002A87 RID: 10887
		private string _WriteModeAndAddress = "2E";

		// Token: 0x04002A88 RID: 10888
		private bool _MakeChangesToInitialData = true;

		// Token: 0x04002A89 RID: 10889
		private string _ATST = "96";

		// Token: 0x04002A8A RID: 10890
		[CompilerGenerated]
		private string <BeforeCommands>k__BackingField;

		// Token: 0x04002A8B RID: 10891
		[CompilerGenerated]
		private string <AfterCommands>k__BackingField;

		// Token: 0x04002A8C RID: 10892
		private string _OpenSessionCommand = "";

		// Token: 0x04002A8D RID: 10893
		private string _CloseSessionCommand = "";

		// Token: 0x04002A8E RID: 10894
		private string _RequestHeader = "";

		// Token: 0x04002A8F RID: 10895
		private string _ResponseHeader = "";

		// Token: 0x04002A90 RID: 10896
		private string _ExtendedAddress = "";

		// Token: 0x04002A91 RID: 10897
		private string _TesterAddress = "";

		// Token: 0x04002A92 RID: 10898
		private int _StartByteId;

		// Token: 0x04002A93 RID: 10899
		private int _DataLength = 1;

		// Token: 0x04002A94 RID: 10900
		private double _Multiplier = 1.0;

		// Token: 0x04002A95 RID: 10901
		private bool _ReversedByteSet;

		// Token: 0x04002A96 RID: 10902
		private double _Offset;

		// Token: 0x04002A97 RID: 10903
		private bool _IsSigned;

		// Token: 0x04002A98 RID: 10904
		private string _PreReadCommands = "";

		// Token: 0x04002A99 RID: 10905
		private string _PreWriteCommands = "";

		// Token: 0x04002A9A RID: 10906
		private string _PostWriteCommands = "";

		// Token: 0x04002A9B RID: 10907
		private string _Comment = "";

		// Token: 0x04002A9C RID: 10908
		private string _Protocol = "";

		// Token: 0x04002A9D RID: 10909
		[CompilerGenerated]
		private IDataPreprocessor <PreWriteDataProcessor>k__BackingField;

		// Token: 0x0200086E RID: 2158
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060049DD RID: 18909 RVA: 0x0037A43C File Offset: 0x0037863C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060049DE RID: 18910 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060049DF RID: 18911 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Name>b__24_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x060049E0 RID: 18912 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__31_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x060049E1 RID: 18913 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_InnerDescription>b__38_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x04002A9E RID: 10910
			public static readonly CustomizableCodingTemplate.<>c <>9 = new CustomizableCodingTemplate.<>c();

			// Token: 0x04002A9F RID: 10911
			public static Func<TranslationItem, bool> <>9__24_0;

			// Token: 0x04002AA0 RID: 10912
			public static Func<TranslationItem, bool> <>9__31_0;

			// Token: 0x04002AA1 RID: 10913
			public static Func<TranslationItem, bool> <>9__38_0;
		}

		// Token: 0x0200086F RID: 2159
		[CompilerGenerated]
		private sealed class <>c__DisplayClass170_0
		{
			// Token: 0x060049E2 RID: 18914 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass170_0()
			{
			}

			// Token: 0x060049E3 RID: 18915 RVA: 0x0037A448 File Offset: 0x00378648
			internal void <GetCurrentStateRawData>b__0(OBDRequest request, string data)
			{
				string text = "7F" + request.Command.Substring(0, 2) + "78";
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				if (data != null && data.IndexOf(text) >= 0)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
					{
						this.getStateReqest.DoNotDecode = true;
						App.OBDReader.AddRequestToQueue(this.getStateReqestforNR78);
					}
				}
			}

			// Token: 0x060049E4 RID: 18916 RVA: 0x0037A4CA File Offset: 0x003786CA
			internal void <GetCurrentStateRawData>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					this.codingResult = CodingRequestResult.Success;
				}
				this.result = data;
				this.semaphore.Release();
			}

			// Token: 0x060049E5 RID: 18917 RVA: 0x0037A4CA File Offset: 0x003786CA
			internal void <GetCurrentStateRawData>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					this.codingResult = CodingRequestResult.Success;
				}
				this.result = data;
				this.semaphore.Release();
			}

			// Token: 0x060049E6 RID: 18918 RVA: 0x0037A4F7 File Offset: 0x003786F7
			internal OBDRequest <GetCurrentStateRawData>b__3(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x04002AA2 RID: 10914
			public OBDRequest getStateReqest;

			// Token: 0x04002AA3 RID: 10915
			public OBDRequest getStateReqestforNR78;

			// Token: 0x04002AA4 RID: 10916
			public CodingRequestResult codingResult;

			// Token: 0x04002AA5 RID: 10917
			public byte[] result;

			// Token: 0x04002AA6 RID: 10918
			public SemaphoreSlim semaphore;

			// Token: 0x04002AA7 RID: 10919
			public CustomizableCodingTemplate <>4__this;
		}

		// Token: 0x02000870 RID: 2160
		[CompilerGenerated]
		private sealed class <>c__DisplayClass171_0
		{
			// Token: 0x060049E7 RID: 18919 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass171_0()
			{
			}

			// Token: 0x060049E8 RID: 18920 RVA: 0x0037A52C File Offset: 0x0037872C
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

			// Token: 0x04002AA8 RID: 10920
			public OBDRequest request;

			// Token: 0x04002AA9 RID: 10921
			public string nr78_string;
		}

		// Token: 0x02000871 RID: 2161
		[CompilerGenerated]
		private sealed class <>c__DisplayClass177_0
		{
			// Token: 0x060049E9 RID: 18921 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass177_0()
			{
			}

			// Token: 0x060049EA RID: 18922 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__2(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049EB RID: 18923 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__3(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049EC RID: 18924 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__4(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049ED RID: 18925 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__5(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049EE RID: 18926 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__6(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049EF RID: 18927 RVA: 0x0037A60C File Offset: 0x0037880C
			internal OBDRequest <WriteDataToECU>b__0(string x)
			{
				return new OBDRequest(x, this.<>4__this.GetRequestHeaderForELM327(), this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x060049F0 RID: 18928 RVA: 0x0037A640 File Offset: 0x00378840
			internal void <WriteDataToECU>b__1(OBDRequest request, string data)
			{
				string text = request.Command;
				if (text == null)
				{
					return;
				}
				if (text.StartsWith("VWTP:", StringComparison.OrdinalIgnoreCase))
				{
					int num = text.IndexOf(':', 5);
					if (num > 0)
					{
						text = text.Substring(num + 1);
					}
				}
				string text2 = text.Substring(0, 2);
				string text3 = (int.Parse(text2, NumberStyles.HexNumber) + 64).ToString("X2");
				if (data != null)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
				}
				string text4 = text3;
				if (this.<>4__this.WriteModeAndAddress != null && this.<>4__this.WriteModeAndAddress.Length >= 2)
				{
					text4 += this.<>4__this.WriteModeAndAddress.Substring(2);
				}
				if (data != null && (data.Contains(text4) || data.Contains("037F" + text2 + "78") || data.Contains(request.ResponseMarker) || MQBEasyCodingItem.MatchPattern(data, string.Concat(new string[]
				{
					"*",
					this.<>4__this.ResponseHeader,
					"*",
					text3,
					"*"
				}))))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
					if (this.postWriteRequests.Count > 0)
					{
						foreach (OBDRequest obdrequest in this.postWriteRequests)
						{
							obdrequest.DoNotDecode = true;
						}
						App.OBDReader.ReplaceQueue(this.postWriteRequests);
						return;
					}
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
						if (data.Length >= 11 && data.Contains("7F" + text2))
						{
							int num2 = data.IndexOf("7F" + text2);
							int num3 = int.Parse(data.Substring(num2 + 4, 2), NumberStyles.HexNumber);
							if (num3 >= 128 || num3 == 34)
							{
								this.codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num3 == 51)
							{
								this.codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num3 == 49)
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
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x04002AAA RID: 10922
			public CustomizableCodingTemplate <>4__this;

			// Token: 0x04002AAB RID: 10923
			public IProgress<string> progress;

			// Token: 0x04002AAC RID: 10924
			public CodingRequestResult codingResult;

			// Token: 0x04002AAD RID: 10925
			public List<OBDRequest> postWriteRequests;
		}

		// Token: 0x02000872 RID: 2162
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__165 : IAsyncStateMachine
		{
			// Token: 0x060049F1 RID: 18929 RVA: 0x0037A8D4 File Offset: 0x00378AD4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomizableCodingTemplate customizableCodingTemplate = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					IProgress<string> progress;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_075F;
						}
						checked_value = "";
						if (!customizableCodingTemplate.MakeChangesToInitialData || originalData != null)
						{
							goto IL_00DB;
						}
						progress = progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
						}
						taskAwaiter3 = customizableCodingTemplate.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, CustomizableCodingTemplate.<Execute>d__165>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter3.GetResult();
					if (result.Item2 != CodingRequestResult.Success)
					{
						codingRequestResult = result.Item2;
						goto IL_0789;
					}
					originalData = result.Item1;
					IL_00DB:
					if (string.IsNullOrEmpty(value))
					{
						checked_value = "";
					}
					else
					{
						if (customizableCodingTemplate.MakeChangesToInitialData)
						{
							switch (customizableCodingTemplate.ValueType)
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
										goto IL_0244;
									}
									catch (Exception)
									{
										goto IL_0244;
									}
								}
								byte[] array5 = BitHelpers.ConvertHexToBytesX(value);
								try
								{
									Array.Copy(array, 0, array2, 0, array.Length);
									Array.Copy(array5, 0, array2, customizableCodingTemplate.StartByteId, array5.Length);
								}
								catch (Exception)
								{
									codingRequestResult = CodingRequestResult.InitialDataIncorrect;
									goto IL_0789;
								}
								IL_0244:
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
								byte[] array6 = customizableCodingTemplate.ConvertInputValueToByteArray(value);
								if (array6.Length == 0)
								{
									codingRequestResult = CodingRequestResult.WrongInputValue;
									goto IL_0789;
								}
								byte[] array7 = new byte[originalData.Length];
								try
								{
									Array.Copy(originalData, 0, array7, 0, array7.Length);
									Array.Copy(array6, 0, array7, customizableCodingTemplate.StartByteId, customizableCodingTemplate.DataLength);
								}
								catch (Exception)
								{
									codingRequestResult = CodingRequestResult.InitialDataIncorrect;
									goto IL_0789;
								}
								checked_value = BitHelpers.ByteArrayToHexString(array7);
								break;
							}
							case AdaptationValueTypes.InputHexDataType:
							case AdaptationValueTypes.MQBLightConfiguration:
							case AdaptationValueTypes.MQBColorList:
							case AdaptationValueTypes.TPMS:
							{
								string text2 = OBDDataReader.FilterHexAndNewLineOnly(value).Replace("\r", "").Replace("\n", "")
									.Trim();
								if (text2.Length % 2 != 0)
								{
									codingRequestResult = CodingRequestResult.WrongInputValue;
									goto IL_0789;
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
									goto IL_0789;
								}
								if (array8.Length > customizableCodingTemplate.DataLength)
								{
									array8 = array8.Take(customizableCodingTemplate.DataLength).ToArray<byte>();
								}
								else if (array8.Length < customizableCodingTemplate.DataLength)
								{
									byte[] array9 = new byte[customizableCodingTemplate.DataLength - array8.Length];
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
									Array.Copy(array8, 0, array10, customizableCodingTemplate.StartByteId, customizableCodingTemplate.DataLength);
								}
								catch (Exception)
								{
									codingRequestResult = CodingRequestResult.InitialDataIncorrect;
									goto IL_0789;
								}
								checked_value = BitHelpers.ByteArrayToHexString(array10);
								break;
							}
							case AdaptationValueTypes.InputFloatIEEE754:
							{
								byte[] bytes = BitConverter.GetBytes(float.Parse(value.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture));
								if (BitConverter.IsLittleEndian)
								{
									Array.Reverse<byte>(bytes);
								}
								if (bytes.Length == 0)
								{
									codingRequestResult = CodingRequestResult.WrongInputValue;
									goto IL_0789;
								}
								byte[] array11 = new byte[originalData.Length];
								try
								{
									Array.Copy(originalData, 0, array11, 0, array11.Length);
									Array.Copy(bytes, 0, array11, customizableCodingTemplate.StartByteId, customizableCodingTemplate.DataLength);
								}
								catch (Exception)
								{
									codingRequestResult = CodingRequestResult.InitialDataIncorrect;
									goto IL_0789;
								}
								checked_value = BitHelpers.ByteArrayToHexString(array11);
								break;
							}
							}
							if (customizableCodingTemplate.PreWriteDataProcessor == null)
							{
								goto IL_0612;
							}
							try
							{
								byte[] array12 = BitHelpers.ConvertHexToBytesX(checked_value);
								byte[] array13 = customizableCodingTemplate.PreWriteDataProcessor.ProcessData(array12);
								checked_value = BitHelpers.ByteArrayToHexString(array13);
								goto IL_0612;
							}
							catch (Exception)
							{
								goto IL_0612;
							}
						}
						AdaptationValueTypes valueType = customizableCodingTemplate.ValueType;
						switch (valueType)
						{
						case AdaptationValueTypes.OptionType:
						case AdaptationValueTypes.InputHexDataType:
						case AdaptationValueTypes.ToyotaTPMSSensor:
							break;
						case AdaptationValueTypes.InputValueType:
						{
							byte[] array14 = customizableCodingTemplate.ConvertInputValueToByteArray(value);
							if (array14.Length == 0)
							{
								codingRequestResult = CodingRequestResult.WrongInputValue;
								goto IL_0789;
							}
							checked_value = BitHelpers.ByteArrayToHexString(array14);
							goto IL_0612;
						}
						case AdaptationValueTypes.MQBLightConfiguration:
						case AdaptationValueTypes.MQBColorList:
						case AdaptationValueTypes.MQBParametrizeDump:
						case AdaptationValueTypes.MQBFPAEditor:
							goto IL_0612;
						case AdaptationValueTypes.InputTextType:
						{
							byte[] bytes2 = Encoding.ASCII.GetBytes(value);
							checked_value = BitHelpers.ByteArrayToHexString(bytes2);
							goto IL_0612;
						}
						default:
							if (valueType != AdaptationValueTypes.InputFloatIEEE754)
							{
								if (valueType != AdaptationValueTypes.TPMS)
								{
									goto IL_0612;
								}
							}
							else
							{
								byte[] bytes3 = BitConverter.GetBytes(float.Parse(value.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), CultureInfo.InvariantCulture));
								if (BitConverter.IsLittleEndian)
								{
									Array.Reverse<byte>(bytes3);
								}
								if (bytes3.Length == 0)
								{
									codingRequestResult = CodingRequestResult.WrongInputValue;
									goto IL_0789;
								}
								checked_value = BitHelpers.ByteArrayToHexString(bytes3);
								goto IL_0612;
							}
							break;
						}
						checked_value = value;
					}
					IL_0612:
					if (skipIfTheSameData && customizableCodingTemplate.MakeChangesToInitialData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Protocol", customizableCodingTemplate.Protocol);
						dictionary.Add("ExtendedAddress", customizableCodingTemplate.ExtendedAddress);
						dictionary.Add("TesterAddress", customizableCodingTemplate.TesterAddress);
						CodingLogItem.RecordToLog(customizableCodingTemplate.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, customizableCodingTemplate.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, customizableCodingTemplate.RequestHeader, customizableCodingTemplate.ResponseHeader, customizableCodingTemplate.OpenSessionCommand, customizableCodingTemplate.PreWriteCommands, customizableCodingTemplate.PostWriteCommands, customizableCodingTemplate.ATST, dictionary, customizableCodingTemplate.PreReadCommands);
						codingRequestResult = CodingRequestResult.Success;
						goto IL_0789;
					}
					taskAwaiter = customizableCodingTemplate.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CustomizableCodingTemplate.<Execute>d__165>(ref taskAwaiter, ref this);
						return;
					}
					IL_075F:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					checked_value = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0789:
				num2 = -2;
				checked_value = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060049F2 RID: 18930 RVA: 0x0037B134 File Offset: 0x00379334
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002AAE RID: 10926
			public int <>1__state;

			// Token: 0x04002AAF RID: 10927
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002AB0 RID: 10928
			public CustomizableCodingTemplate <>4__this;

			// Token: 0x04002AB1 RID: 10929
			public byte[] originalData;

			// Token: 0x04002AB2 RID: 10930
			public IProgress<string> progress;

			// Token: 0x04002AB3 RID: 10931
			public string value;

			// Token: 0x04002AB4 RID: 10932
			public bool skipIfTheSameData;

			// Token: 0x04002AB5 RID: 10933
			public string UserFriendlyValue;

			// Token: 0x04002AB6 RID: 10934
			public string password;

			// Token: 0x04002AB7 RID: 10935
			private string <checked_value>5__2;

			// Token: 0x04002AB8 RID: 10936
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;

			// Token: 0x04002AB9 RID: 10937
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000873 RID: 2163
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__170 : IAsyncStateMachine
		{
			// Token: 0x060049F3 RID: 18931 RVA: 0x0037B144 File Offset: 0x00379344
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomizableCodingTemplate customizableCodingTemplate = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new CustomizableCodingTemplate.<>c__DisplayClass170_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						customizableCodingTemplate.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.getStateReqest = new OBDRequest(customizableCodingTemplate.ReadModeAndAddress, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands, false);
						string text = SharedSettings.Current.GetATST();
						if (string.IsNullOrEmpty(text))
						{
							text = "32";
						}
						CS$<>8__locals1.getStateReqestforNR78 = new OBDRequest(customizableCodingTemplate.ReadModeAndAddress, customizableCodingTemplate.GetRequestHeaderForELM327(), "ATAT0;ATSTFF;" + customizableCodingTemplate.BeforeCommands, string.Concat(new string[]
						{
							"ATAT",
							SharedSettings.Current.AdaptiveTimings.ToString(),
							";ATST",
							text,
							";",
							customizableCodingTemplate.AfterCommands
						}), false);
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest(customizableCodingTemplate.OpenSessionCommand, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands, false);
						customizableCodingTemplate.SetRenaultOpenSessionFix(obdrequest);
						if (!SharedSettings.Current.ShowExperimental)
						{
							CS$<>8__locals1.getStateReqest.ResponseReceived += delegate(OBDRequest request, string data)
							{
								string text2 = "7F" + request.Command.Substring(0, 2) + "78";
								if (data != null)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
								}
								if (data != null && data.IndexOf(text2) >= 0)
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
									if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
									{
										CS$<>8__locals1.getStateReqest.DoNotDecode = true;
										App.OBDReader.AddRequestToQueue(CS$<>8__locals1.getStateReqestforNR78);
									}
								}
							};
						}
						else
						{
							CS$<>8__locals1.getStateReqest.ResponseReceived += customizableCodingTemplate.Request_ResponseReceivedCheckForNR78;
						}
						CS$<>8__locals1.result = null;
						CS$<>8__locals1.getStateReqest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							CS$<>8__locals1.result = data;
							CS$<>8__locals1.semaphore.Release();
						};
						CS$<>8__locals1.getStateReqestforNR78.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							CS$<>8__locals1.result = data;
							CS$<>8__locals1.semaphore.Release();
						};
						CS$<>8__locals1.getStateReqest.CheckLength = true;
						CS$<>8__locals1.getStateReqestforNR78.CheckLength = true;
						List<OBDRequest> list = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(obdrequest.Command))
						{
							list.Add(obdrequest);
						}
						if (!string.IsNullOrEmpty(customizableCodingTemplate.PreReadCommands))
						{
							if (VagUnitHelper.IsVag(SharedSettings.Current.SelectedBrand))
							{
								List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(customizableCodingTemplate.PreReadCommands, password, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands);
								list.AddRange(requestsFromStringCommands);
							}
							else
							{
								OBDRequest[] array = (from x in customizableCodingTemplate.PreReadCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								list.AddRange(array);
							}
						}
						list.Add(CS$<>8__locals1.getStateReqest);
						List<OBDRequest>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest2 = enumerator.Current;
								obdrequest2.ELMFormat = customizableCodingTemplate.ELMFormat;
								obdrequest2.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
								string selectedBrand = SharedSettings.Current.SelectedBrand;
								if (selectedBrand == "Kia" || selectedBrand == "Hyundai" || selectedBrand == "Genesis")
								{
									obdrequest2.ForceManualFlowControl = false;
								}
								if (VagUnitHelper.IsVag(selectedBrand) && (obdrequest2.Header == "7E0" || obdrequest2.Header == "7E1"))
								{
									obdrequest2.ForceManualFlowControl = false;
								}
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
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomizableCodingTemplate.<GetCurrentStateRawData>d__170>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.result.Length != 0 && customizableCodingTemplate.DataLength == 0)
					{
						customizableCodingTemplate.DataLength = CS$<>8__locals1.result.Length;
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

			// Token: 0x060049F4 RID: 18932 RVA: 0x0037B60C File Offset: 0x0037980C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002ABA RID: 10938
			public int <>1__state;

			// Token: 0x04002ABB RID: 10939
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04002ABC RID: 10940
			public CustomizableCodingTemplate <>4__this;

			// Token: 0x04002ABD RID: 10941
			public string password;

			// Token: 0x04002ABE RID: 10942
			private CustomizableCodingTemplate.<>c__DisplayClass170_0 <>8__1;

			// Token: 0x04002ABF RID: 10943
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000874 RID: 2164
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__166 : IAsyncStateMachine
		{
			// Token: 0x060049F5 RID: 18933 RVA: 0x0037B61C File Offset: 0x0037981C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomizableCodingTemplate customizableCodingTemplate = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						state = "";
						if (string.IsNullOrEmpty(customizableCodingTemplate.ReadModeAndAddress))
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_0226;
						}
						taskAwaiter = customizableCodingTemplate.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, CustomizableCodingTemplate.<UpdateCurrentState>d__166>(ref taskAwaiter, ref this);
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
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						customizableCodingTemplate.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						switch (customizableCodingTemplate.ValueType)
						{
						case AdaptationValueTypes.OptionType:
							state = customizableCodingTemplate.ConvertRawDataToOptionType(item2);
							goto IL_01F6;
						case AdaptationValueTypes.InputValueType:
							state = customizableCodingTemplate.ConvertRawDataToInputValue(item2);
							goto IL_01F6;
						case AdaptationValueTypes.InputHexDataType:
						case AdaptationValueTypes.MQBLightConfiguration:
						case AdaptationValueTypes.MQBColorList:
							state = BitHelpers.ByteArrayToHexString(item2);
							goto IL_01F6;
						case AdaptationValueTypes.MQBParametrizeDump:
						case AdaptationValueTypes.MQBFPAEditor:
						case AdaptationValueTypes.ToyotaTPMSSensor:
						case AdaptationValueTypes.MQBRKDSGenerator:
						case AdaptationValueTypes.MQBA5Customization:
							goto IL_01F6;
						case AdaptationValueTypes.InputTextType:
							try
							{
								if (customizableCodingTemplate.DataLength == 0)
								{
									state = Encoding.ASCII.GetString(item2, customizableCodingTemplate.StartByteId, item2.Length - customizableCodingTemplate.StartByteId);
								}
								else
								{
									state = Encoding.ASCII.GetString(item2, customizableCodingTemplate.StartByteId, customizableCodingTemplate.DataLength);
								}
								goto IL_01F6;
							}
							catch (Exception)
							{
								state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
								goto IL_01F6;
							}
							break;
						case AdaptationValueTypes.InputFloatIEEE754:
							break;
						default:
							goto IL_01F6;
						}
						if (item2.Length < customizableCodingTemplate.StartByteId + customizableCodingTemplate.DataLength)
						{
							state = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						}
						else
						{
							byte[] array = new byte[4];
							Array.Copy(item2, customizableCodingTemplate.StartByteId, array, 0, array.Length);
							if (BitConverter.IsLittleEndian)
							{
								Array.Reverse<byte>(array);
							}
							state = BitConverter.ToSingle(array, 0).ToString(CultureInfo.InvariantCulture);
						}
						IL_01F6:
						customizableCodingTemplate.CurrentState = state;
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
				IL_0226:
				num2 = -2;
				state = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060049F6 RID: 18934 RVA: 0x0037B8A0 File Offset: 0x00379AA0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002AC0 RID: 10944
			public int <>1__state;

			// Token: 0x04002AC1 RID: 10945
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002AC2 RID: 10946
			public CustomizableCodingTemplate <>4__this;

			// Token: 0x04002AC3 RID: 10947
			public string password;

			// Token: 0x04002AC4 RID: 10948
			private string <state>5__2;

			// Token: 0x04002AC5 RID: 10949
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}

		// Token: 0x02000875 RID: 2165
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__177 : IAsyncStateMachine
		{
			// Token: 0x060049F7 RID: 18935 RVA: 0x0037B8B0 File Offset: 0x00379AB0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomizableCodingTemplate customizableCodingTemplate = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new CustomizableCodingTemplate.<>c__DisplayClass177_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						customizableCodingTemplate.BuildDefaultBeforeAndAfterCommands();
						new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest(customizableCodingTemplate.WriteModeAndAddress + checked_value, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest(customizableCodingTemplate.OpenSessionCommand, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands, false);
						customizableCodingTemplate.SetRenaultOpenSessionFix(obdrequest2);
						List<OBDRequest> list = new List<OBDRequest>();
						string selectedBrand = SharedSettings.Current.SelectedBrand;
						if (!string.IsNullOrEmpty(obdrequest2.Command))
						{
							list.Add(obdrequest2);
						}
						if (!string.IsNullOrEmpty(customizableCodingTemplate.PreWriteCommands))
						{
							if (VagUnitHelper.IsVag(selectedBrand))
							{
								List<OBDRequest> requestsFromStringCommands = MQBParametrizeBase.GetRequestsFromStringCommands(customizableCodingTemplate.PreWriteCommands, password, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands);
								list.AddRange(requestsFromStringCommands);
							}
							else if (selectedBrand == "Haval" || selectedBrand == "Hover" || selectedBrand == "Great Wall")
							{
								OBDRequest[] array = (from x in customizableCodingTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								OBDRequest[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									DashboardPasswordGenerator.SetPasswordDecodeFor2701(array2[i]);
								}
								list.AddRange(array);
							}
							else if (selectedBrand == "Nissan" && SharedSettings.Current.ProfileUpdateAlias == "7e2905bfa3eb488bad22f6fd955496f5")
							{
								OBDRequest[] array3 = (from x in customizableCodingTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								OBDRequest[] array2 = array3;
								for (int i = 0; i < array2.Length; i++)
								{
									NissanLeafBatterySoHCountersReset.SetPasswordDecodeFor2701(array2[i]);
								}
								list.AddRange(array3);
							}
							else if (selectedBrand == "Hyundai" || selectedBrand == "Genesis" || selectedBrand == "Kia")
							{
								OBDRequest[] array4 = (from x in customizableCodingTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								foreach (OBDRequest obdrequest3 in array4)
								{
									KiaUMCRDI27Generator.SetPasswordDecodeFor2701(obdrequest3);
									KiaDashboardSeed27Generator.SetPasswordDecodeFor2701(obdrequest3);
									KiaBCM27Generator.SetPasswordDecodeFor2711(obdrequest3);
								}
								list.AddRange(array4);
							}
							else if (selectedBrand == "Peugeot" || selectedBrand == "Citroen" || selectedBrand == "DS")
							{
								OBDRequest[] array5 = (from x in customizableCodingTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								OBDRequest[] array2 = array5;
								for (int i = 0; i < array2.Length; i++)
								{
									PSA_SeedKeyGenerator.SetPasswordDecodeFor2703(array2[i], password);
								}
								list.AddRange(array5);
							}
							else
							{
								OBDRequest[] array6 = (from x in customizableCodingTemplate.PreWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>();
								list.AddRange(array6);
							}
						}
						list.Add(obdrequest);
						CS$<>8__locals1.postWriteRequests = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(customizableCodingTemplate.PostWriteCommands))
						{
							if (VagUnitHelper.IsVag(selectedBrand))
							{
								List<OBDRequest> requestsFromStringCommands2 = MQBParametrizeBase.GetRequestsFromStringCommands(customizableCodingTemplate.PostWriteCommands, password, customizableCodingTemplate.GetRequestHeaderForELM327(), customizableCodingTemplate.BeforeCommands, customizableCodingTemplate.AfterCommands);
								CS$<>8__locals1.postWriteRequests.AddRange(requestsFromStringCommands2);
							}
							else
							{
								CS$<>8__locals1.postWriteRequests.AddRange((from x in customizableCodingTemplate.PostWriteCommands.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(x, CS$<>8__locals1.<>4__this.GetRequestHeaderForELM327(), CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false, null)
									{
										DoNotDecode = true
									}).ToArray<OBDRequest>());
							}
						}
						List<OBDRequest>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest4 = enumerator.Current;
								obdrequest4.ELMFormat = customizableCodingTemplate.ELMFormat;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							string text = request.Command;
							if (text == null)
							{
								return;
							}
							if (text.StartsWith("VWTP:", StringComparison.OrdinalIgnoreCase))
							{
								int num3 = text.IndexOf(':', 5);
								if (num3 > 0)
								{
									text = text.Substring(num3 + 1);
								}
							}
							string text2 = text.Substring(0, 2);
							string text3 = (int.Parse(text2, NumberStyles.HexNumber) + 64).ToString("X2");
							if (data != null)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
							}
							string text4 = text3;
							if (CS$<>8__locals1.<>4__this.WriteModeAndAddress != null && CS$<>8__locals1.<>4__this.WriteModeAndAddress.Length >= 2)
							{
								text4 += CS$<>8__locals1.<>4__this.WriteModeAndAddress.Substring(2);
							}
							if (data != null && (data.Contains(text4) || data.Contains("037F" + text2 + "78") || data.Contains(request.ResponseMarker) || MQBEasyCodingItem.MatchPattern(data, string.Concat(new string[]
							{
								"*",
								CS$<>8__locals1.<>4__this.ResponseHeader,
								"*",
								text3,
								"*"
							}))))
							{
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
								if (CS$<>8__locals1.postWriteRequests.Count > 0)
								{
									foreach (OBDRequest obdrequest6 in CS$<>8__locals1.postWriteRequests)
									{
										obdrequest6.DoNotDecode = true;
									}
									App.OBDReader.ReplaceQueue(CS$<>8__locals1.postWriteRequests);
									return;
								}
							}
							else
							{
								IProgress<string> progress2 = CS$<>8__locals1.progress;
								if (progress2 != null)
								{
									progress2.Report(Translate.GetString("coding_progress_DataRejected"));
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
									if (data.Length >= 11 && data.Contains("7F" + text2))
									{
										int num4 = data.IndexOf("7F" + text2);
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
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
						};
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomizableCodingTemplate.<WriteDataToECU>d__177>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.codingResult == CodingRequestResult.Success && customizableCodingTemplate.MakeChangesToInitialData && !string.IsNullOrEmpty(customizableCodingTemplate.ReadModeAndAddress) && customizableCodingTemplate.WriteModeAndAddress.Substring(2) == customizableCodingTemplate.ReadModeAndAddress.Substring(2))
					{
						if (customizableCodingTemplate.PreWriteDataProcessor != null)
						{
							originalData = customizableCodingTemplate.PreWriteDataProcessor.ProcessData(originalData);
						}
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Protocol", customizableCodingTemplate.Protocol);
						CodingLogItem.RecordToLog(customizableCodingTemplate.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, customizableCodingTemplate.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, customizableCodingTemplate.RequestHeader, customizableCodingTemplate.ResponseHeader, customizableCodingTemplate.OpenSessionCommand, customizableCodingTemplate.PreWriteCommands, customizableCodingTemplate.PostWriteCommands, customizableCodingTemplate.ATST, dictionary, customizableCodingTemplate.PreReadCommands);
					}
					if (string.IsNullOrEmpty(customizableCodingTemplate.ReadModeAndAddress) && CS$<>8__locals1.postWriteRequests.Count > 0 && CS$<>8__locals1.postWriteRequests[CS$<>8__locals1.postWriteRequests.Count - 1] != null && CS$<>8__locals1.postWriteRequests[CS$<>8__locals1.postWriteRequests.Count - 1].Command.StartsWith("3E"))
					{
						OBDRequest obdrequest5 = CS$<>8__locals1.postWriteRequests[CS$<>8__locals1.postWriteRequests.Count - 1];
						obdrequest5.Repeat = true;
						App.OBDReader.ReplaceQueue(obdrequest5);
					}
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

			// Token: 0x060049F8 RID: 18936 RVA: 0x0037BFD8 File Offset: 0x0037A1D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002AC6 RID: 10950
			public int <>1__state;

			// Token: 0x04002AC7 RID: 10951
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002AC8 RID: 10952
			public CustomizableCodingTemplate <>4__this;

			// Token: 0x04002AC9 RID: 10953
			public IProgress<string> progress;

			// Token: 0x04002ACA RID: 10954
			public string checked_value;

			// Token: 0x04002ACB RID: 10955
			public string password;

			// Token: 0x04002ACC RID: 10956
			private CustomizableCodingTemplate.<>c__DisplayClass177_0 <>8__1;

			// Token: 0x04002ACD RID: 10957
			public byte[] originalData;

			// Token: 0x04002ACE RID: 10958
			public string UserFriendlyValue;

			// Token: 0x04002ACF RID: 10959
			private TaskAwaiter <>u__1;
		}
	}
}
