using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007BC RID: 1980
	internal class AutoUpdatingListForHidingItems<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable where T : IIsVisibleCollectionItem
	{
		// Token: 0x0600464F RID: 17999 RVA: 0x0036979B File Offset: 0x0036799B
		public AutoUpdatingListForHidingItems()
		{
			this.Initialize();
		}

		// Token: 0x17001604 RID: 5636
		// (get) Token: 0x06004650 RID: 18000 RVA: 0x003697BF File Offset: 0x003679BF
		public IReadOnlyList<T> FullList
		{
			get
			{
				return this.fullList;
			}
		}

		// Token: 0x17001605 RID: 5637
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x003697C7 File Offset: 0x003679C7
		public IReadOnlyList<T> VisibleList
		{
			get
			{
				return this.visibleList;
			}
		}

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x06004652 RID: 18002 RVA: 0x003697D0 File Offset: 0x003679D0
		// (remove) Token: 0x06004653 RID: 18003 RVA: 0x00369808 File Offset: 0x00367A08
		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			[CompilerGenerated]
			add
			{
				NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = this.CollectionChanged;
				NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler2;
				do
				{
					notifyCollectionChangedEventHandler2 = notifyCollectionChangedEventHandler;
					NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler3 = (NotifyCollectionChangedEventHandler)Delegate.Combine(notifyCollectionChangedEventHandler2, value);
					notifyCollectionChangedEventHandler = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, notifyCollectionChangedEventHandler3, notifyCollectionChangedEventHandler2);
				}
				while (notifyCollectionChangedEventHandler != notifyCollectionChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = this.CollectionChanged;
				NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler2;
				do
				{
					notifyCollectionChangedEventHandler2 = notifyCollectionChangedEventHandler;
					NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler3 = (NotifyCollectionChangedEventHandler)Delegate.Remove(notifyCollectionChangedEventHandler2, value);
					notifyCollectionChangedEventHandler = Interlocked.CompareExchange<NotifyCollectionChangedEventHandler>(ref this.CollectionChanged, notifyCollectionChangedEventHandler3, notifyCollectionChangedEventHandler2);
				}
				while (notifyCollectionChangedEventHandler != notifyCollectionChangedEventHandler2);
			}
		}

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x06004654 RID: 18004 RVA: 0x00369840 File Offset: 0x00367A40
		// (remove) Token: 0x06004655 RID: 18005 RVA: 0x00369878 File Offset: 0x00367A78
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

		// Token: 0x06004656 RID: 18006 RVA: 0x003698B0 File Offset: 0x00367AB0
		private void Initialize()
		{
			if (this.Count > 0)
			{
				foreach (T t in this)
				{
					ref T ptr = ref t;
					if (default(T) == null)
					{
						T t2 = t;
						ptr = ref t2;
					}
					ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
					ref T ptr2 = ref t;
					if (default(T) == null)
					{
						T t2 = t;
						ptr2 = ref t2;
					}
					ptr2.PropertyChanged += this.AutoUpdatingListForHidingItems_PropertyChanged;
				}
			}
		}

		// Token: 0x06004657 RID: 18007 RVA: 0x00369960 File Offset: 0x00367B60
		private void AutoUpdatingListForHidingItems_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsVisible" && sender is T)
			{
				T t = (T)((object)sender);
				if (t.IsVisible)
				{
					this.visibleList.Add(t);
					NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
					if (collectionChanged == null)
					{
						return;
					}
					collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, t));
					return;
				}
				else
				{
					this.visibleList.Remove(t);
					NotifyCollectionChangedEventHandler collectionChanged2 = this.CollectionChanged;
					if (collectionChanged2 == null)
					{
						return;
					}
					collectionChanged2(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, t));
				}
			}
		}

		// Token: 0x06004658 RID: 18008 RVA: 0x003699F1 File Offset: 0x00367BF1
		public int IndexOf(T item)
		{
			return this.fullList.IndexOf(item);
		}

		// Token: 0x06004659 RID: 18009 RVA: 0x00369A00 File Offset: 0x00367C00
		public void Insert(int index, T item)
		{
			this.fullList.Insert(index, item);
			if (item.IsVisible)
			{
				if (index > this.visibleList.Count)
				{
					this.visibleList.Insert(this.visibleList.Count, item);
				}
				else
				{
					this.visibleList.Insert(index, item);
				}
				NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
				if (collectionChanged != null)
				{
					collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, this.visibleList.Skip(index + 1).ToList<T>()));
				}
			}
			ref T ptr = ref item;
			if (default(T) == null)
			{
				T t = item;
				ptr = ref t;
			}
			ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			ref T ptr2 = ref item;
			if (default(T) == null)
			{
				T t = item;
				ptr2 = ref t;
			}
			ptr2.PropertyChanged += this.AutoUpdatingListForHidingItems_PropertyChanged;
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x00369AF0 File Offset: 0x00367CF0
		public void RemoveAt(int index)
		{
			T t = this.visibleList[index];
			ref T ptr = ref t;
			if (default(T) == null)
			{
				T t2 = t;
				ptr = ref t2;
			}
			ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			this.visibleList.Remove(t);
			this.fullList.Remove(t);
			NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
			if (collectionChanged == null)
			{
				return;
			}
			collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, t));
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x00369B74 File Offset: 0x00367D74
		public void Add(T item)
		{
			this.fullList.Add(item);
			if (item.IsVisible)
			{
				this.visibleList.Add(item);
				NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
				if (collectionChanged != null)
				{
					collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
				}
			}
			ref T ptr = ref item;
			if (default(T) == null)
			{
				T t = item;
				ptr = ref t;
			}
			ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			ref T ptr2 = ref item;
			if (default(T) == null)
			{
				T t = item;
				ptr2 = ref t;
			}
			ptr2.PropertyChanged += this.AutoUpdatingListForHidingItems_PropertyChanged;
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x00369C28 File Offset: 0x00367E28
		public void Clear()
		{
			foreach (T t in this.fullList)
			{
				ref T ptr = ref t;
				if (default(T) == null)
				{
					T t2 = t;
					ptr = ref t2;
				}
				ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			}
			this.fullList.Clear();
			this.visibleList.Clear();
			NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
			if (collectionChanged == null)
			{
				return;
			}
			collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x00369CD4 File Offset: 0x00367ED4
		public bool Contains(T item)
		{
			return this.fullList.Contains(item);
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x00369CE2 File Offset: 0x00367EE2
		public void CopyTo(T[] array, int arrayIndex)
		{
			this.fullList.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x00369CF4 File Offset: 0x00367EF4
		public bool Remove(T item)
		{
			ref T ptr = ref item;
			if (default(T) == null)
			{
				T t = item;
				ptr = ref t;
			}
			ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			if (this.visibleList.Contains(item))
			{
				this.visibleList.Remove(item);
				NotifyCollectionChangedEventHandler collectionChanged = this.CollectionChanged;
				if (collectionChanged != null)
				{
					collectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
				}
			}
			return this.fullList.Remove(item);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x00369D77 File Offset: 0x00367F77
		public IEnumerator<T> GetEnumerator()
		{
			return this.visibleList.GetEnumerator();
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x00369D77 File Offset: 0x00367F77
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.visibleList.GetEnumerator();
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x00369D8C File Offset: 0x00367F8C
		public void Dispose()
		{
			foreach (T t in this.fullList)
			{
				ref T ptr = ref t;
				if (default(T) == null)
				{
					T t2 = t;
					ptr = ref t2;
				}
				ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
			}
			this.fullList.Clear();
			this.visibleList.Clear();
		}

		// Token: 0x17001606 RID: 5638
		// (get) Token: 0x06004663 RID: 18019 RVA: 0x00369E20 File Offset: 0x00368020
		public int Count
		{
			get
			{
				return this.visibleList.Count;
			}
		}

		// Token: 0x17001607 RID: 5639
		// (get) Token: 0x06004664 RID: 18020 RVA: 0x00369E2D File Offset: 0x0036802D
		public int FullCount
		{
			get
			{
				return this.fullList.Count;
			}
		}

		// Token: 0x17001608 RID: 5640
		// (get) Token: 0x06004665 RID: 18021 RVA: 0x00002076 File Offset: 0x00000276
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001609 RID: 5641
		public T this[int index]
		{
			get
			{
				return this.visibleList[index];
			}
			set
			{
				T t = this.visibleList[index];
				int num = this.fullList.IndexOf(t);
				ref T ptr = ref t;
				if (default(T) == null)
				{
					T t2 = t;
					ptr = ref t2;
				}
				ptr.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
				this.visibleList[index] = value;
				this.fullList[num] = value;
				ref T ptr2 = ref value;
				if (default(T) == null)
				{
					T t2 = value;
					ptr2 = ref t2;
				}
				ptr2.PropertyChanged -= this.AutoUpdatingListForHidingItems_PropertyChanged;
				ref T ptr3 = ref value;
				if (default(T) == null)
				{
					T t2 = value;
					ptr3 = ref t2;
				}
				ptr3.PropertyChanged += this.AutoUpdatingListForHidingItems_PropertyChanged;
			}
		}

		// Token: 0x040028EC RID: 10476
		private List<T> fullList = new List<T>();

		// Token: 0x040028ED RID: 10477
		private List<T> visibleList = new List<T>();

		// Token: 0x040028EE RID: 10478
		[CompilerGenerated]
		private NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x040028EF RID: 10479
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
