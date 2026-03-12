using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000889 RID: 2185
	public class MQBAdaptationOption : INotifyPropertyChanged
	{
		// Token: 0x06004A4E RID: 19022 RVA: 0x0037D4E8 File Offset: 0x0037B6E8
		public MQBAdaptationOption(string Title, string Value)
		{
			this.Title = Title;
			this.Value = Value;
			if (this.Title == "RES_ENABLE" || this.Title == "RES_ENABLED")
			{
				this.Title = Translate.GetString("coding_EnableOption");
				return;
			}
			if (this.Title == "RES_DISABLE" || this.Title == "RES_DISABLED")
			{
				this.Title = Translate.GetString("coding_DisableOption");
				return;
			}
			if (this.Title.StartsWith("RES:"))
			{
				this.Title = Translate.GetString(Title.Substring(4));
			}
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0037D5C4 File Offset: 0x0037B7C4
		public MQBAdaptationOption(string Title, string Value, params TranslationItem[] translations)
		{
			this.Title = Title;
			this.Value = Value;
			if (translations != null)
			{
				foreach (TranslationItem translationItem in translations)
				{
					this.Translations.Add(translationItem);
				}
			}
			if (this.Title == "RES_ENABLE" || this.Title == "RES_ENABLED")
			{
				this.Title = Translate.GetString("coding_EnableOption");
				return;
			}
			if (this.Title == "RES_DISABLE" || this.Title == "RES_DISABLED")
			{
				this.Title = Translate.GetString("coding_DisableOption");
				return;
			}
			if (this.Title.StartsWith("RES:"))
			{
				this.Title = Translate.GetString(Title.Substring(4));
			}
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x0037D6C4 File Offset: 0x0037B8C4
		[JsonConstructor]
		public MQBAdaptationOption(string Title, string Value, string ReadOnlyValue, params TranslationItem[] translations)
		{
			this.Title = Title;
			this.Value = Value;
			if (ReadOnlyValue != null)
			{
				this.ReadOnlyValue = ReadOnlyValue;
			}
			if (translations != null)
			{
				foreach (TranslationItem translationItem in translations)
				{
					this.Translations.Add(translationItem);
				}
			}
			if (this.Title == "RES_ENABLE" || this.Title == "RES_ENABLED")
			{
				this.Title = Translate.GetString("coding_EnableOption");
				return;
			}
			if (this.Title == "RES_DISABLE" || this.Title == "RES_DISABLED")
			{
				this.Title = Translate.GetString("coding_DisableOption");
				return;
			}
			if (this.Title != null && this.Title.StartsWith("RES:"))
			{
				this.Title = Translate.GetString(Title.Substring(4));
			}
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06004A51 RID: 19025 RVA: 0x0037D7D6 File Offset: 0x0037B9D6
		// (set) Token: 0x06004A52 RID: 19026 RVA: 0x0037D7DE File Offset: 0x0037B9DE
		[JsonIgnore]
		public string TitleRaw
		{
			get
			{
				return this._Title;
			}
			set
			{
				this._Title = value;
				this.OnPropertyChanged("Title");
				this.OnPropertyChanged("TitleRaw");
			}
		}

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06004A53 RID: 19027 RVA: 0x0037D800 File Offset: 0x0037BA00
		// (set) Token: 0x06004A54 RID: 19028 RVA: 0x0037D848 File Offset: 0x0037BA48
		[JsonProperty("TTL")]
		public string Title
		{
			get
			{
				TranslationItem translationItem = this.Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					return translationItem.Name;
				}
				return this._Title;
			}
			set
			{
				this._Title = value;
				this.OnPropertyChanged("Title");
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06004A55 RID: 19029 RVA: 0x0037D85C File Offset: 0x0037BA5C
		// (set) Token: 0x06004A56 RID: 19030 RVA: 0x0037D864 File Offset: 0x0037BA64
		[JsonProperty("VL")]
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				this.OnPropertyChanged("Value");
			}
		}

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06004A57 RID: 19031 RVA: 0x0037D878 File Offset: 0x0037BA78
		// (set) Token: 0x06004A58 RID: 19032 RVA: 0x0037D880 File Offset: 0x0037BA80
		[JsonProperty("ROVL")]
		public string ReadOnlyValue
		{
			get
			{
				return this._ReadOnlyValue;
			}
			set
			{
				this._ReadOnlyValue = value;
				this.OnPropertyChanged("ReadOnlyValue");
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06004A59 RID: 19033 RVA: 0x0037D894 File Offset: 0x0037BA94
		// (set) Token: 0x06004A5A RID: 19034 RVA: 0x0037D89C File Offset: 0x0037BA9C
		public ObservableCollection<TranslationItem> Translations
		{
			[CompilerGenerated]
			get
			{
				return this.<Translations>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Translations>k__BackingField = value;
			}
		} = new ObservableCollection<TranslationItem>();

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06004A5B RID: 19035 RVA: 0x0037D8A8 File Offset: 0x0037BAA8
		// (remove) Token: 0x06004A5C RID: 19036 RVA: 0x0037D8E0 File Offset: 0x0037BAE0
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

		// Token: 0x06004A5D RID: 19037 RVA: 0x0037D915 File Offset: 0x0037BB15
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0037D930 File Offset: 0x0037BB30
		public MQBAdaptationOption Clone()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(this.Title, this.Value);
			mqbadaptationOption.Translations = new ObservableCollection<TranslationItem>(this.Translations.Select((TranslationItem x) => x.Clone()).ToList<TranslationItem>());
			return mqbadaptationOption;
		}

		// Token: 0x04002B2D RID: 11053
		private string _Title = "";

		// Token: 0x04002B2E RID: 11054
		private string _Value = "";

		// Token: 0x04002B2F RID: 11055
		private string _ReadOnlyValue = "";

		// Token: 0x04002B30 RID: 11056
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04002B31 RID: 11057
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0200088A RID: 2186
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004A5F RID: 19039 RVA: 0x0037D988 File Offset: 0x0037BB88
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004A60 RID: 19040 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004A61 RID: 19041 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Title>b__7_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x06004A62 RID: 19042 RVA: 0x0036F3D0 File Offset: 0x0036D5D0
			internal TranslationItem <Clone>b__26_0(TranslationItem x)
			{
				return x.Clone();
			}

			// Token: 0x04002B32 RID: 11058
			public static readonly MQBAdaptationOption.<>c <>9 = new MQBAdaptationOption.<>c();

			// Token: 0x04002B33 RID: 11059
			public static Func<TranslationItem, bool> <>9__7_0;

			// Token: 0x04002B34 RID: 11060
			public static Func<TranslationItem, TranslationItem> <>9__26_0;
		}
	}
}
