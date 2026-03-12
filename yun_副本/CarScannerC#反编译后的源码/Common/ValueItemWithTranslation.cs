using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x0200080D RID: 2061
	public class ValueItemWithTranslation : INotifyPropertyChanged
	{
		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x060047AC RID: 18348 RVA: 0x0036F1A7 File Offset: 0x0036D3A7
		// (set) Token: 0x060047AD RID: 18349 RVA: 0x0036F1AF File Offset: 0x0036D3AF
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
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x060047AE RID: 18350 RVA: 0x0036F1D4 File Offset: 0x0036D3D4
		// (set) Token: 0x060047AF RID: 18351 RVA: 0x0036F21C File Offset: 0x0036D41C
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
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Title"));
			}
		}

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x060047B0 RID: 18352 RVA: 0x0036F240 File Offset: 0x0036D440
		// (remove) Token: 0x060047B1 RID: 18353 RVA: 0x0036F278 File Offset: 0x0036D478
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

		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x060047B2 RID: 18354 RVA: 0x0036F2AD File Offset: 0x0036D4AD
		// (set) Token: 0x060047B3 RID: 18355 RVA: 0x0036F2B5 File Offset: 0x0036D4B5
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

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x060047B4 RID: 18356 RVA: 0x0036F2C0 File Offset: 0x0036D4C0
		// (set) Token: 0x060047B5 RID: 18357 RVA: 0x0036F308 File Offset: 0x0036D508
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
				return this._Description;
			}
			set
			{
				this._Description = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Description"));
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0036F32C File Offset: 0x0036D52C
		public ValueItemWithTranslation Clone()
		{
			ValueItemWithTranslation valueItemWithTranslation = new ValueItemWithTranslation();
			valueItemWithTranslation.Title = this.Title;
			valueItemWithTranslation.Description = this.Description;
			valueItemWithTranslation.Translations = new ObservableCollection<TranslationItem>(this.Translations.Select((TranslationItem x) => x.Clone()).ToList<TranslationItem>());
			return valueItemWithTranslation;
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0036F390 File Offset: 0x0036D590
		public ValueItemWithTranslation()
		{
		}

		// Token: 0x040029C6 RID: 10694
		private string _Value = "";

		// Token: 0x040029C7 RID: 10695
		private string _Title = "";

		// Token: 0x040029C8 RID: 10696
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040029C9 RID: 10697
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x040029CA RID: 10698
		private string _Description = "";

		// Token: 0x0200080E RID: 2062
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060047B8 RID: 18360 RVA: 0x0036F3C4 File Offset: 0x0036D5C4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060047B9 RID: 18361 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060047BA RID: 18362 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Title>b__5_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x060047BB RID: 18363 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <get_Description>b__16_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x060047BC RID: 18364 RVA: 0x0036F3D0 File Offset: 0x0036D5D0
			internal TranslationItem <Clone>b__19_0(TranslationItem x)
			{
				return x.Clone();
			}

			// Token: 0x040029CB RID: 10699
			public static readonly ValueItemWithTranslation.<>c <>9 = new ValueItemWithTranslation.<>c();

			// Token: 0x040029CC RID: 10700
			public static Func<TranslationItem, bool> <>9__5_0;

			// Token: 0x040029CD RID: 10701
			public static Func<TranslationItem, bool> <>9__16_0;

			// Token: 0x040029CE RID: 10702
			public static Func<TranslationItem, TranslationItem> <>9__19_0;
		}
	}
}
