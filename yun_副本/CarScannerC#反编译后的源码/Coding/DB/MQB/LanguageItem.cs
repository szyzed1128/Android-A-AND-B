using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AF5 RID: 2805
	internal class LanguageItem : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x060057DF RID: 22495 RVA: 0x0041EB60 File Offset: 0x0041CD60
		public LanguageItem(string code, string name)
		{
			this.Code = code;
			this.Name = name;
			this.IsChecked = false;
		}

		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x060057E0 RID: 22496 RVA: 0x0041EB93 File Offset: 0x0041CD93
		// (set) Token: 0x060057E1 RID: 22497 RVA: 0x0041EB9B File Offset: 0x0041CD9B
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x060057E2 RID: 22498 RVA: 0x0041EBBF File Offset: 0x0041CDBF
		// (set) Token: 0x060057E3 RID: 22499 RVA: 0x0041EBC7 File Offset: 0x0041CDC7
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				this._Code = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Code"));
			}
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x060057E4 RID: 22500 RVA: 0x0041EBEB File Offset: 0x0041CDEB
		// (set) Token: 0x060057E5 RID: 22501 RVA: 0x0041EBF3 File Offset: 0x0041CDF3
		public bool IsChecked
		{
			get
			{
				return this._IsChecked;
			}
			set
			{
				if (this._IsChecked != value)
				{
					this._IsChecked = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
				}
			}
		}

		// Token: 0x14000067 RID: 103
		// (add) Token: 0x060057E6 RID: 22502 RVA: 0x0041EC20 File Offset: 0x0041CE20
		// (remove) Token: 0x060057E7 RID: 22503 RVA: 0x0041EC58 File Offset: 0x0041CE58
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

		// Token: 0x060057E8 RID: 22504 RVA: 0x0041EC8D File Offset: 0x0041CE8D
		public void Dispose()
		{
			this.PropertyChanged = null;
		}

		// Token: 0x04003656 RID: 13910
		private string _Name = "";

		// Token: 0x04003657 RID: 13911
		private string _Code = "";

		// Token: 0x04003658 RID: 13912
		private bool _IsChecked;

		// Token: 0x04003659 RID: 13913
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
