using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x0200095F RID: 2399
	internal class LongCodingModel : INotifyPropertyChanged
	{
		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06004EC6 RID: 20166 RVA: 0x003BEE3C File Offset: 0x003BD03C
		// (remove) Token: 0x06004EC7 RID: 20167 RVA: 0x003BEE74 File Offset: 0x003BD074
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

		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x003BEEA9 File Offset: 0x003BD0A9
		// (set) Token: 0x06004EC9 RID: 20169 RVA: 0x003BEEB1 File Offset: 0x003BD0B1
		public bool UseCustomAddress
		{
			get
			{
				return this._UseCustomAddress;
			}
			set
			{
				this._UseCustomAddress = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("UseCustomAddress"));
			}
		}

		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x003BEED5 File Offset: 0x003BD0D5
		// (set) Token: 0x06004ECB RID: 20171 RVA: 0x003BEEDD File Offset: 0x003BD0DD
		public string CustomAddress
		{
			get
			{
				return this._CustomAddress;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._CustomAddress = value.Replace(" ", "").Trim();
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CustomAddress"));
			}
		}

		// Token: 0x17001792 RID: 6034
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x003BEF19 File Offset: 0x003BD119
		// (set) Token: 0x06004ECD RID: 20173 RVA: 0x003BEF21 File Offset: 0x003BD121
		public HexStringObject Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Data"));
			}
		}

		// Token: 0x17001793 RID: 6035
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x003BEF45 File Offset: 0x003BD145
		// (set) Token: 0x06004ECF RID: 20175 RVA: 0x003BEF4D File Offset: 0x003BD14D
		public ICodingContainer Coding
		{
			[CompilerGenerated]
			get
			{
				return this.<Coding>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Coding>k__BackingField = value;
			}
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x003BEF56 File Offset: 0x003BD156
		public LongCodingModel(ICodingContainer coding)
		{
			this.Coding = coding;
		}

		// Token: 0x04002F72 RID: 12146
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002F73 RID: 12147
		private bool _UseCustomAddress;

		// Token: 0x04002F74 RID: 12148
		private string _CustomAddress = "";

		// Token: 0x04002F75 RID: 12149
		private HexStringObject _Data = new HexStringObject("");

		// Token: 0x04002F76 RID: 12150
		[CompilerGenerated]
		private ICodingContainer <Coding>k__BackingField;
	}
}
