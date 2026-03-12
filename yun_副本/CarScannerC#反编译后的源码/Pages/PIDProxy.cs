using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000641 RID: 1601
	public class PIDProxy : INotifyPropertyChanged
	{
		// Token: 0x060037A2 RID: 14242 RVA: 0x0029C5EE File Offset: 0x0029A7EE
		public PIDProxy(PID pid)
		{
			this.Pid = pid;
			this.IsVisible = false;
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x0029C604 File Offset: 0x0029A804
		// (set) Token: 0x060037A4 RID: 14244 RVA: 0x0029C60C File Offset: 0x0029A80C
		public PID Pid
		{
			[CompilerGenerated]
			get
			{
				return this.<Pid>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Pid>k__BackingField = value;
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060037A5 RID: 14245 RVA: 0x0029C618 File Offset: 0x0029A818
		// (remove) Token: 0x060037A6 RID: 14246 RVA: 0x0029C650 File Offset: 0x0029A850
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

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0029C685 File Offset: 0x0029A885
		// (set) Token: 0x060037A8 RID: 14248 RVA: 0x0029C690 File Offset: 0x0029A890
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			set
			{
				this._IsVisible = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
				}
			}
		}

		// Token: 0x040021B0 RID: 8624
		[CompilerGenerated]
		private PID <Pid>k__BackingField;

		// Token: 0x040021B1 RID: 8625
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040021B2 RID: 8626
		private bool _IsVisible;
	}
}
