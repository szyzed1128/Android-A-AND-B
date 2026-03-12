using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006D4 RID: 1748
	internal class ProxyPidForSelector : INotifyPropertyChanged
	{
		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x06003B7B RID: 15227 RVA: 0x00314E70 File Offset: 0x00313070
		// (set) Token: 0x06003B7C RID: 15228 RVA: 0x00314E78 File Offset: 0x00313078
		public bool IsSelected
		{
			get
			{
				return this._IsSelected;
			}
			set
			{
				if (value != this._IsSelected)
				{
					this._IsSelected = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
				}
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06003B7D RID: 15229 RVA: 0x00314EA8 File Offset: 0x003130A8
		// (remove) Token: 0x06003B7E RID: 15230 RVA: 0x00314EE0 File Offset: 0x003130E0
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

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x06003B7F RID: 15231 RVA: 0x00314F15 File Offset: 0x00313115
		// (set) Token: 0x06003B80 RID: 15232 RVA: 0x00314F1D File Offset: 0x0031311D
		public IPID Pid
		{
			[CompilerGenerated]
			get
			{
				return this.<Pid>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Pid>k__BackingField = value;
			}
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x00314F26 File Offset: 0x00313126
		public ProxyPidForSelector(IPID pid, bool isSelected = false)
		{
			this.Pid = pid;
			this.IsSelected = isSelected;
		}

		// Token: 0x04002472 RID: 9330
		private bool _IsSelected;

		// Token: 0x04002473 RID: 9331
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002474 RID: 9332
		[CompilerGenerated]
		private IPID <Pid>k__BackingField;
	}
}
