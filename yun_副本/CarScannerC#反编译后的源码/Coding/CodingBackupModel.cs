using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000861 RID: 2145
	public class CodingBackupModel : INotifyPropertyChanged
	{
		// Token: 0x0600494E RID: 18766 RVA: 0x003782E0 File Offset: 0x003764E0
		public CodingBackupModel()
		{
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00378300 File Offset: 0x00376500
		public async Task Load()
		{
			await Task.Run<List<CodingLogItem>>(() => this.AllItems = CodingLogItem.LoadLogItems());
			await this.UpdateFilter();
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x06004950 RID: 18768 RVA: 0x00378343 File Offset: 0x00376543
		public IReadOnlyList<CodingLogItem> RawItems
		{
			get
			{
				return this.AllItems;
			}
		}

		// Token: 0x17001673 RID: 5747
		// (get) Token: 0x06004951 RID: 18769 RVA: 0x0037834B File Offset: 0x0037654B
		// (set) Token: 0x06004952 RID: 18770 RVA: 0x00378353 File Offset: 0x00376553
		public bool ShowHidden
		{
			get
			{
				return this._ShowHidden;
			}
			set
			{
				if (this._ShowHidden != value)
				{
					this._ShowHidden = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs("ShowHidden"));
					}
					this.UpdateFilter();
				}
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06004953 RID: 18771 RVA: 0x00378388 File Offset: 0x00376588
		// (remove) Token: 0x06004954 RID: 18772 RVA: 0x003783C0 File Offset: 0x003765C0
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

		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x06004955 RID: 18773 RVA: 0x003783F5 File Offset: 0x003765F5
		// (set) Token: 0x06004956 RID: 18774 RVA: 0x003783FD File Offset: 0x003765FD
		public string Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				if (this._Filter != value)
				{
					this._Filter = value;
					this.UpdateFilter();
				}
			}
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x0037841C File Offset: 0x0037661C
		private async Task UpdateFilter()
		{
			string cachedFilter = this.Filter;
			await Task.Delay(1000);
			if (!(cachedFilter != this.Filter))
			{
				List<CodingLogItem> tempList;
				if (this.ShowHidden)
				{
					tempList = this.AllItems.ToList<CodingLogItem>();
				}
				else
				{
					tempList = this.AllItems.Where((CodingLogItem x) => x.IsVisible).ToList<CodingLogItem>();
				}
				await Task.Run(delegate
				{
					try
					{
						if (!string.IsNullOrEmpty(cachedFilter))
						{
							string[] array = cachedFilter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < array.Length; i++)
							{
								string word = array[i];
								tempList = tempList.Where((CodingLogItem x) => (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.VIN) && x.VIN.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Date) && x.Date.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<CodingLogItem>();
							}
						}
					}
					catch (Exception)
					{
					}
				});
				this.VisibleItems.Reset(tempList);
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x06004958 RID: 18776 RVA: 0x0037845F File Offset: 0x0037665F
		// (set) Token: 0x06004959 RID: 18777 RVA: 0x00378467 File Offset: 0x00376667
		public SmartCollection<CodingLogItem> VisibleItems
		{
			[CompilerGenerated]
			get
			{
				return this.<VisibleItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<VisibleItems>k__BackingField = value;
			}
		} = new SmartCollection<CodingLogItem>();

		// Token: 0x0600495A RID: 18778 RVA: 0x00378470 File Offset: 0x00376670
		public List<string> GetVINCodes()
		{
			List<string> list = this.AllItems.Select((CodingLogItem x) => x.VIN).Distinct<string>().ToList<string>();
			for (int i = 0; i < list.Count; i++)
			{
				if (string.IsNullOrEmpty(list[i]))
				{
					list[i] = "NO VIN";
				}
			}
			return list;
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x003784E0 File Offset: 0x003766E0
		public async Task CreateRestoreToStockItemsForVinCode(string vin)
		{
			List<CodingLogItemGroup> list = new List<CodingLogItemGroup>();
			using (List<CodingLogItem>.Enumerator enumerator = this.AllItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CodingLogItem item = enumerator.Current;
					CodingLogItemGroup codingLogItemGroup = list.FirstOrDefault((CodingLogItemGroup x) => x.CanAdd(item));
					if (codingLogItemGroup == null)
					{
						codingLogItemGroup = new CodingLogItemGroup(item);
						list.Add(codingLogItemGroup);
					}
					else
					{
						codingLogItemGroup.Add(item);
					}
				}
			}
			List<CodingLogItemGroup> list2 = list.Where((CodingLogItemGroup x) => x.VIN == vin).ToList<CodingLogItemGroup>();
			List<CodingLogItem> list3 = new List<CodingLogItem>();
			for (int i = 0; i < list2.Count; i++)
			{
				CodingLogItem itemForRestore = list2[i].GetItemForRestore();
				if (itemForRestore != null)
				{
					CodingLogItem codingLogItem = JsonConvert.DeserializeObject<CodingLogItem>(JsonConvert.SerializeObject(itemForRestore));
					codingLogItem.NewData = "";
					codingLogItem.Timestamp = DateTimeNowHelper.NowSafe.Ticks;
					list3.Add(codingLogItem);
				}
			}
			for (int j = 0; j < list3.Count; j++)
			{
				list3[j].Title = string.Format("{0}: {1}, ID={2}, {3}/{4}", new object[]
				{
					vin,
					Translate.GetString("coding_RestoreToInitialBtn"),
					list3[j].Address,
					j + 1,
					list3.Count
				});
				list3[j].UserFriendlyValue = "";
			}
			this.AllItems = list3.Concat(this.AllItems).ToList<CodingLogItem>();
			CodingLogItem.SaveLogItems(this.AllItems);
			foreach (CodingLogItem codingLogItem2 in this.AllItems)
			{
				codingLogItem2.Selected = false;
			}
			foreach (CodingLogItem codingLogItem3 in list3)
			{
				codingLogItem3.Selected = true;
			}
			await this.UpdateFilter();
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x0037852C File Offset: 0x0037672C
		[CompilerGenerated]
		private List<CodingLogItem> <Load>b__1_0()
		{
			return this.AllItems = CodingLogItem.LoadLogItems();
		}

		// Token: 0x04002A55 RID: 10837
		private List<CodingLogItem> AllItems;

		// Token: 0x04002A56 RID: 10838
		private bool _ShowHidden;

		// Token: 0x04002A57 RID: 10839
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002A58 RID: 10840
		private string _Filter = "";

		// Token: 0x04002A59 RID: 10841
		[CompilerGenerated]
		private SmartCollection<CodingLogItem> <VisibleItems>k__BackingField;

		// Token: 0x02000862 RID: 2146
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600495D RID: 18781 RVA: 0x00378547 File Offset: 0x00376747
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600495E RID: 18782 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600495F RID: 18783 RVA: 0x00378553 File Offset: 0x00376753
			internal bool <UpdateFilter>b__16_0(CodingLogItem x)
			{
				return x.IsVisible;
			}

			// Token: 0x06004960 RID: 18784 RVA: 0x0037855B File Offset: 0x0037675B
			internal string <GetVINCodes>b__21_0(CodingLogItem x)
			{
				return x.VIN;
			}

			// Token: 0x04002A5A RID: 10842
			public static readonly CodingBackupModel.<>c <>9 = new CodingBackupModel.<>c();

			// Token: 0x04002A5B RID: 10843
			public static Func<CodingLogItem, bool> <>9__16_0;

			// Token: 0x04002A5C RID: 10844
			public static Func<CodingLogItem, string> <>9__21_0;
		}

		// Token: 0x02000863 RID: 2147
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06004961 RID: 18785 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06004962 RID: 18786 RVA: 0x00378564 File Offset: 0x00376764
			internal void <UpdateFilter>b__1()
			{
				try
				{
					if (!string.IsNullOrEmpty(this.cachedFilter))
					{
						string[] array = this.cachedFilter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < array.Length; i++)
						{
							CodingBackupModel.<>c__DisplayClass16_1 CS$<>8__locals1 = new CodingBackupModel.<>c__DisplayClass16_1();
							CS$<>8__locals1.word = array[i];
							this.tempList = this.tempList.Where((CodingLogItem x) => (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.VIN) && x.VIN.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Date) && x.Date.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<CodingLogItem>();
						}
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04002A5D RID: 10845
			public string cachedFilter;

			// Token: 0x04002A5E RID: 10846
			public List<CodingLogItem> tempList;
		}

		// Token: 0x02000864 RID: 2148
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_1
		{
			// Token: 0x06004963 RID: 18787 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_1()
			{
			}

			// Token: 0x06004964 RID: 18788 RVA: 0x003785EC File Offset: 0x003767EC
			internal bool <UpdateFilter>b__2(CodingLogItem x)
			{
				return (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.VIN) && x.VIN.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Date) && x.Date.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002A5F RID: 10847
			public string word;
		}

		// Token: 0x02000865 RID: 2149
		[CompilerGenerated]
		private sealed class <>c__DisplayClass22_0
		{
			// Token: 0x06004965 RID: 18789 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass22_0()
			{
			}

			// Token: 0x06004966 RID: 18790 RVA: 0x00378666 File Offset: 0x00376866
			internal bool <CreateRestoreToStockItemsForVinCode>b__0(CodingLogItemGroup x)
			{
				return x.VIN == this.vin;
			}

			// Token: 0x04002A60 RID: 10848
			public string vin;
		}

		// Token: 0x02000866 RID: 2150
		[CompilerGenerated]
		private sealed class <>c__DisplayClass22_1
		{
			// Token: 0x06004967 RID: 18791 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass22_1()
			{
			}

			// Token: 0x06004968 RID: 18792 RVA: 0x00378679 File Offset: 0x00376879
			internal bool <CreateRestoreToStockItemsForVinCode>b__1(CodingLogItemGroup x)
			{
				return x.CanAdd(this.item);
			}

			// Token: 0x04002A61 RID: 10849
			public CodingLogItem item;
		}

		// Token: 0x02000867 RID: 2151
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CreateRestoreToStockItemsForVinCode>d__22 : IAsyncStateMachine
		{
			// Token: 0x06004969 RID: 18793 RVA: 0x00378688 File Offset: 0x00376888
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupModel codingBackupModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CodingBackupModel.<>c__DisplayClass22_0 CS$<>8__locals1 = new CodingBackupModel.<>c__DisplayClass22_0();
						CS$<>8__locals1.vin = vin;
						List<CodingLogItemGroup> list = new List<CodingLogItemGroup>();
						List<CodingLogItem>.Enumerator enumerator = codingBackupModel.AllItems.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CodingBackupModel.<>c__DisplayClass22_1 CS$<>8__locals2 = new CodingBackupModel.<>c__DisplayClass22_1();
								CS$<>8__locals2.item = enumerator.Current;
								CodingLogItemGroup codingLogItemGroup = list.FirstOrDefault((CodingLogItemGroup x) => x.CanAdd(CS$<>8__locals2.item));
								if (codingLogItemGroup == null)
								{
									codingLogItemGroup = new CodingLogItemGroup(CS$<>8__locals2.item);
									list.Add(codingLogItemGroup);
								}
								else
								{
									codingLogItemGroup.Add(CS$<>8__locals2.item);
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						List<CodingLogItemGroup> list2 = list.Where((CodingLogItemGroup x) => x.VIN == CS$<>8__locals1.vin).ToList<CodingLogItemGroup>();
						List<CodingLogItem> list3 = new List<CodingLogItem>();
						for (int i = 0; i < list2.Count; i++)
						{
							CodingLogItem itemForRestore = list2[i].GetItemForRestore();
							if (itemForRestore != null)
							{
								CodingLogItem codingLogItem = JsonConvert.DeserializeObject<CodingLogItem>(JsonConvert.SerializeObject(itemForRestore));
								codingLogItem.NewData = "";
								codingLogItem.Timestamp = DateTimeNowHelper.NowSafe.Ticks;
								list3.Add(codingLogItem);
							}
						}
						for (int j = 0; j < list3.Count; j++)
						{
							list3[j].Title = string.Format("{0}: {1}, ID={2}, {3}/{4}", new object[]
							{
								CS$<>8__locals1.vin,
								Translate.GetString("coding_RestoreToInitialBtn"),
								list3[j].Address,
								j + 1,
								list3.Count
							});
							list3[j].UserFriendlyValue = "";
						}
						codingBackupModel.AllItems = list3.Concat(codingBackupModel.AllItems).ToList<CodingLogItem>();
						CodingLogItem.SaveLogItems(codingBackupModel.AllItems);
						enumerator = codingBackupModel.AllItems.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CodingLogItem codingLogItem2 = enumerator.Current;
								codingLogItem2.Selected = false;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = list3.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CodingLogItem codingLogItem3 = enumerator.Current;
								codingLogItem3.Selected = true;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						taskAwaiter = codingBackupModel.UpdateFilter().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupModel.<CreateRestoreToStockItemsForVinCode>d__22>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600496A RID: 18794 RVA: 0x003789D0 File Offset: 0x00376BD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A62 RID: 10850
			public int <>1__state;

			// Token: 0x04002A63 RID: 10851
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002A64 RID: 10852
			public string vin;

			// Token: 0x04002A65 RID: 10853
			public CodingBackupModel <>4__this;

			// Token: 0x04002A66 RID: 10854
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000868 RID: 2152
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Load>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600496B RID: 18795 RVA: 0x003789E0 File Offset: 0x00376BE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupModel codingBackupModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<List<CodingLogItem>> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00D0;
						}
						taskAwaiter3 = Task.Run<List<CodingLogItem>>(() => codingBackupModel.AllItems = CodingLogItem.LoadLogItems()).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<List<CodingLogItem>> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CodingLogItem>>, CodingBackupModel.<Load>d__1>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<List<CodingLogItem>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<List<CodingLogItem>>);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = codingBackupModel.UpdateFilter().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupModel.<Load>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_00D0:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600496C RID: 18796 RVA: 0x00378B04 File Offset: 0x00376D04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A67 RID: 10855
			public int <>1__state;

			// Token: 0x04002A68 RID: 10856
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002A69 RID: 10857
			public CodingBackupModel <>4__this;

			// Token: 0x04002A6A RID: 10858
			private TaskAwaiter<List<CodingLogItem>> <>u__1;

			// Token: 0x04002A6B RID: 10859
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000869 RID: 2153
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateFilter>d__16 : IAsyncStateMachine
		{
			// Token: 0x0600496D RID: 18797 RVA: 0x00378B14 File Offset: 0x00376D14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupModel codingBackupModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_016B;
						}
						CS$<>8__locals1 = new CodingBackupModel.<>c__DisplayClass16_0();
						CS$<>8__locals1.cachedFilter = codingBackupModel.Filter;
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupModel.<UpdateFilter>d__16>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					if (CS$<>8__locals1.cachedFilter != codingBackupModel.Filter)
					{
						goto IL_01A8;
					}
					if (codingBackupModel.ShowHidden)
					{
						CS$<>8__locals1.tempList = codingBackupModel.AllItems.ToList<CodingLogItem>();
					}
					else
					{
						CS$<>8__locals1.tempList = codingBackupModel.AllItems.Where((CodingLogItem x) => x.IsVisible).ToList<CodingLogItem>();
					}
					taskAwaiter = Task.Run(delegate
					{
						try
						{
							CodingBackupModel.<>c__DisplayClass16_1 CS$<>8__locals1;
							if (!string.IsNullOrEmpty(CS$<>8__locals1.cachedFilter))
							{
								string[] array = CS$<>8__locals1.cachedFilter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								for (int i = 0; i < array.Length; i++)
								{
									CS$<>8__locals1 = new CodingBackupModel.<>c__DisplayClass16_1();
									CS$<>8__locals1.word = array[i];
									CS$<>8__locals1.tempList = CS$<>8__locals1.tempList.Where((CodingLogItem x) => (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.VIN) && x.VIN.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Date) && x.Date.IndexOf(CS$<>8__locals1.word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<CodingLogItem>();
								}
							}
						}
						catch (Exception)
						{
						}
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupModel.<UpdateFilter>d__16>(ref taskAwaiter, ref this);
						return;
					}
					IL_016B:
					taskAwaiter.GetResult();
					codingBackupModel.VisibleItems.Reset(CS$<>8__locals1.tempList);
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A8:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600496E RID: 18798 RVA: 0x00378D00 File Offset: 0x00376F00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A6C RID: 10860
			public int <>1__state;

			// Token: 0x04002A6D RID: 10861
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002A6E RID: 10862
			public CodingBackupModel <>4__this;

			// Token: 0x04002A6F RID: 10863
			private CodingBackupModel.<>c__DisplayClass16_0 <>8__1;

			// Token: 0x04002A70 RID: 10864
			private TaskAwaiter <>u__1;
		}
	}
}
