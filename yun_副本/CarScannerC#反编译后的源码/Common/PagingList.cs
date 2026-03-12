using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007FC RID: 2044
	public class PagingList
	{
		// Token: 0x06004754 RID: 18260 RVA: 0x0036E008 File Offset: 0x0036C208
		public PagingList(List<KeyValuePair<string, int>> items, int maxPageCapacity, string prevPageTitle, string nextPageTitle)
		{
			this.internalList = new List<KeyValuePair<string, int>>(items);
			Queue<KeyValuePair<string, int>> queue = new Queue<KeyValuePair<string, int>>(items);
			if (items.Count < maxPageCapacity)
			{
				this.pages.Add(new List<KeyValuePair<string, int>>(items));
			}
			else
			{
				int num = maxPageCapacity - 1;
				List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(num + 1);
				for (int i = 0; i < num; i++)
				{
					list.Add(queue.Dequeue());
				}
				list.Add(new KeyValuePair<string, int>(nextPageTitle, int.MaxValue));
				this.pages.Add(list);
				while (queue.Count > 0)
				{
					if (queue.Count < maxPageCapacity - 1)
					{
						List<KeyValuePair<string, int>> list2 = new List<KeyValuePair<string, int>>(queue.Count + 1);
						list2.Add(new KeyValuePair<string, int>(prevPageTitle, int.MinValue));
						while (queue.Count > 0)
						{
							list2.Add(queue.Dequeue());
						}
						this.pages.Add(list2);
					}
					else
					{
						List<KeyValuePair<string, int>> list3 = new List<KeyValuePair<string, int>>(maxPageCapacity - 2);
						list3.Add(new KeyValuePair<string, int>(prevPageTitle, int.MinValue));
						for (int j = 0; j < maxPageCapacity - 2; j++)
						{
							list3.Add(queue.Dequeue());
						}
						list3.Add(new KeyValuePair<string, int>(nextPageTitle, int.MaxValue));
						this.pages.Add(list3);
					}
				}
			}
			this.CurrentPage = 0;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x0036E16C File Offset: 0x0036C36C
		public void NextPage()
		{
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage + 1;
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x0036E18C File Offset: 0x0036C38C
		public void PrevPage()
		{
			int currentPage = this.CurrentPage;
			this.CurrentPage = currentPage - 1;
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x0036E1A9 File Offset: 0x0036C3A9
		public List<KeyValuePair<string, int>> GetCurrentPage()
		{
			if (this.pages.Count == 0)
			{
				return new List<KeyValuePair<string, int>>(0);
			}
			return this.pages[this.CurrentPage];
		}

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06004758 RID: 18264 RVA: 0x0036E1D0 File Offset: 0x0036C3D0
		// (set) Token: 0x06004759 RID: 18265 RVA: 0x0036E1D8 File Offset: 0x0036C3D8
		public int MaxPageCapacity
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxPageCapacity>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MaxPageCapacity>k__BackingField = value;
			}
		} = 10;

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x0600475A RID: 18266 RVA: 0x0036E1E1 File Offset: 0x0036C3E1
		// (set) Token: 0x0600475B RID: 18267 RVA: 0x0036E1E9 File Offset: 0x0036C3E9
		public int CurrentPage
		{
			get
			{
				return this._CurrentPage;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value >= this.pages.Count)
				{
					value = this.pages.Count - 1;
				}
				this._CurrentPage = value;
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x0600475C RID: 18268 RVA: 0x0036E216 File Offset: 0x0036C416
		public int Count
		{
			get
			{
				return this.pages.Count;
			}
		}

		// Token: 0x04002991 RID: 10641
		private List<List<KeyValuePair<string, int>>> pages = new List<List<KeyValuePair<string, int>>>();

		// Token: 0x04002992 RID: 10642
		[CompilerGenerated]
		private int <MaxPageCapacity>k__BackingField;

		// Token: 0x04002993 RID: 10643
		private List<KeyValuePair<string, int>> internalList;

		// Token: 0x04002994 RID: 10644
		private int _CurrentPage;
	}
}
