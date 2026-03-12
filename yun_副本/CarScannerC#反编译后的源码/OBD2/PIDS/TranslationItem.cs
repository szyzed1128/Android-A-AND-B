using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000425 RID: 1061
	public class TranslationItem : INotifyPropertyChanged
	{
		// Token: 0x06002D44 RID: 11588 RVA: 0x001FED0E File Offset: 0x001FCF0E
		public TranslationItem()
		{
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x001FED44 File Offset: 0x001FCF44
		public TranslationItem(string Language, string Name, string ShortName = "", string AdditionalText = "")
		{
			this.Language = Language;
			this.Name = Name;
			this.ShortName = ShortName;
			this.AdditionalText = AdditionalText;
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06002D46 RID: 11590 RVA: 0x001FEDA0 File Offset: 0x001FCFA0
		// (set) Token: 0x06002D47 RID: 11591 RVA: 0x001FEDA8 File Offset: 0x001FCFA8
		[JsonProperty("LNG")]
		public string Language
		{
			get
			{
				return this._Language;
			}
			set
			{
				this._Language = value;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06002D48 RID: 11592 RVA: 0x001FEDB1 File Offset: 0x001FCFB1
		// (set) Token: 0x06002D49 RID: 11593 RVA: 0x001FEDB9 File Offset: 0x001FCFB9
		[JsonProperty("NM")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x001FEDC2 File Offset: 0x001FCFC2
		// (set) Token: 0x06002D4B RID: 11595 RVA: 0x001FEDCA File Offset: 0x001FCFCA
		[JsonProperty("SN")]
		public string ShortName
		{
			get
			{
				return this._ShortName;
			}
			set
			{
				this._ShortName = value;
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06002D4C RID: 11596 RVA: 0x001FEDD3 File Offset: 0x001FCFD3
		// (set) Token: 0x06002D4D RID: 11597 RVA: 0x001FEDDB File Offset: 0x001FCFDB
		[JsonProperty("ADT")]
		public string AdditionalText
		{
			get
			{
				return this._AdditionalText;
			}
			set
			{
				this._AdditionalText = value;
			}
		}

		// Token: 0x1400002A RID: 42
		// (add) Token: 0x06002D4E RID: 11598 RVA: 0x001FEDE4 File Offset: 0x001FCFE4
		// (remove) Token: 0x06002D4F RID: 11599 RVA: 0x001FEE1C File Offset: 0x001FD01C
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

		// Token: 0x06002D50 RID: 11600 RVA: 0x001FEE51 File Offset: 0x001FD051
		public TranslationItem Clone()
		{
			return new TranslationItem
			{
				Language = this.Language,
				Name = this.Name,
				ShortName = this.ShortName
			};
		}

		// Token: 0x04001939 RID: 6457
		private string _Language = "";

		// Token: 0x0400193A RID: 6458
		private string _Name = "";

		// Token: 0x0400193B RID: 6459
		private string _ShortName = "";

		// Token: 0x0400193C RID: 6460
		private string _AdditionalText = "";

		// Token: 0x0400193D RID: 6461
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
