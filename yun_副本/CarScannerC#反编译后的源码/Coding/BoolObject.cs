using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200084A RID: 2122
	public class BoolObject : INotifyPropertyChanged
	{
		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06004891 RID: 18577 RVA: 0x00370F54 File Offset: 0x0036F154
		// (remove) Token: 0x06004892 RID: 18578 RVA: 0x00370F8C File Offset: 0x0036F18C
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

		// Token: 0x1700164C RID: 5708
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00370FC1 File Offset: 0x0036F1C1
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00370FC9 File Offset: 0x0036F1C9
		public bool Value
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

		// Token: 0x06004895 RID: 18581 RVA: 0x00002050 File Offset: 0x00000250
		public BoolObject()
		{
		}

		// Token: 0x040029E2 RID: 10722
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040029E3 RID: 10723
		private bool _Value;
	}
}
