using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CarScannerXamarinForms
{
	// Token: 0x02000055 RID: 85
	public class SmartCollection<T> : ObservableCollection<T>
	{
		// Token: 0x060001FB RID: 507 RVA: 0x00016A6E File Offset: 0x00014C6E
		public SmartCollection()
		{
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00016A76 File Offset: 0x00014C76
		public SmartCollection(IEnumerable<T> collection)
			: base(collection)
		{
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00016A7F File Offset: 0x00014C7F
		public SmartCollection(List<T> list)
			: base(list)
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00016A88 File Offset: 0x00014C88
		public void AddRange(IEnumerable<T> range)
		{
			foreach (T t in range)
			{
				base.Items.Add(t);
			}
			this.NotifyCollectionReset();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00016ADC File Offset: 0x00014CDC
		public void Reset(IEnumerable<T> range)
		{
			base.Items.Clear();
			this.AddRange(range);
			this.NotifyCollectionReset();
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00016AF6 File Offset: 0x00014CF6
		public void ResetWithoutNotification(IEnumerable<T> range)
		{
			base.Items.Clear();
			this.AddRange(range);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00016B0A File Offset: 0x00014D0A
		public void NotifyCollectionReset()
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00016B38 File Offset: 0x00014D38
		internal void AddWithoutNotification(T item)
		{
			base.Items.Add(item);
		}
	}
}
