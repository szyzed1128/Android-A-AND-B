using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000736 RID: 1846
	internal class ScanContainer : List<ResultContainer>, INotifyPropertyChanged
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06003EB4 RID: 16052 RVA: 0x0032DAF8 File Offset: 0x0032BCF8
		// (remove) Token: 0x06003EB5 RID: 16053 RVA: 0x0032DB30 File Offset: 0x0032BD30
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

		// Token: 0x06003EB6 RID: 16054 RVA: 0x0032DB65 File Offset: 0x0032BD65
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x06003EB7 RID: 16055 RVA: 0x0032DB7E File Offset: 0x0032BD7E
		// (set) Token: 0x06003EB8 RID: 16056 RVA: 0x0032DB86 File Offset: 0x0032BD86
		public string Task
		{
			[CompilerGenerated]
			get
			{
				return this.<Task>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Task>k__BackingField = value;
			}
		}

		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06003EB9 RID: 16057 RVA: 0x0032DB8F File Offset: 0x0032BD8F
		// (set) Token: 0x06003EBA RID: 16058 RVA: 0x0032DB97 File Offset: 0x0032BD97
		public string Brand
		{
			get
			{
				return this.brand;
			}
			set
			{
				this.brand = value;
				this.OnPropertyChanged("Brand");
			}
		}

		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06003EBB RID: 16059 RVA: 0x0032DBAB File Offset: 0x0032BDAB
		// (set) Token: 0x06003EBC RID: 16060 RVA: 0x0032DBB3 File Offset: 0x0032BDB3
		public string Model
		{
			get
			{
				return this.model;
			}
			set
			{
				this.model = value;
				this.OnPropertyChanged("Model");
			}
		}

		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x06003EBD RID: 16061 RVA: 0x0032DBC7 File Offset: 0x0032BDC7
		// (set) Token: 0x06003EBE RID: 16062 RVA: 0x0032DBCF File Offset: 0x0032BDCF
		public string Year
		{
			get
			{
				return this.year;
			}
			set
			{
				this.year = value;
				this.OnPropertyChanged("Year");
			}
		}

		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x06003EBF RID: 16063 RVA: 0x0032DBE3 File Offset: 0x0032BDE3
		// (set) Token: 0x06003EC0 RID: 16064 RVA: 0x0032DBEB File Offset: 0x0032BDEB
		public string OtherInfo
		{
			get
			{
				return this.otherInfo;
			}
			set
			{
				this.otherInfo = value;
				this.OnPropertyChanged("OtherInfo");
			}
		}

		// Token: 0x06003EC1 RID: 16065 RVA: 0x0032DC00 File Offset: 0x0032BE00
		public string GetJson()
		{
			string text = JsonConvert.SerializeObject(this);
			return string.Concat(new string[]
			{
				"Scan task = ", this.Task, "\nBrand = ", this.Brand, "\nModel = ", this.Model, "\nYear = ", this.Year, "\nInfo = ", this.OtherInfo,
				"\r\n", text
			});
		}

		// Token: 0x06003EC2 RID: 16066 RVA: 0x0032DC86 File Offset: 0x0032BE86
		public ScanContainer()
		{
		}

		// Token: 0x0400267F RID: 9855
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002680 RID: 9856
		[CompilerGenerated]
		private string <Task>k__BackingField;

		// Token: 0x04002681 RID: 9857
		private string brand = "";

		// Token: 0x04002682 RID: 9858
		private string model = "";

		// Token: 0x04002683 RID: 9859
		private string year = "";

		// Token: 0x04002684 RID: 9860
		private string otherInfo = "";
	}
}
