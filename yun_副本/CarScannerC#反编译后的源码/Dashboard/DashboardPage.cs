using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard
{
	// Token: 0x02000771 RID: 1905
	public class DashboardPage : ContentView, IDashboardPage
	{
		// Token: 0x060040F5 RID: 16629 RVA: 0x00338A10 File Offset: 0x00336C10
		public DashboardPage()
		{
			this.Items.CollectionChanged += this.Items_CollectionChanged;
			base.HorizontalOptions = LayoutOptions.FillAndExpand;
			base.VerticalOptions = LayoutOptions.FillAndExpand;
			base.SizeChanged += this.OnSizeChanged;
			if (Device.RuntimePlatform == "Android")
			{
				this.grid = new Grid
				{
					RowSpacing = 0.0,
					ColumnSpacing = 0.0
				};
			}
			else
			{
				this.grid = new Grid
				{
					RowSpacing = 0.0,
					ColumnSpacing = 0.0
				};
			}
			base.Content = this.grid;
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x060040F6 RID: 16630 RVA: 0x00338AF0 File Offset: 0x00336CF0
		// (set) Token: 0x060040F7 RID: 16631 RVA: 0x00338AF8 File Offset: 0x00336CF8
		public virtual int ItemsCount
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemsCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ItemsCount>k__BackingField = value;
			}
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00338B04 File Offset: 0x00336D04
		private void OnSizeChanged(object sender, EventArgs e)
		{
			if (SharedSettings.Current.DashboardRearrangeOnRotation)
			{
				bool flag = base.Height >= base.Width;
				if (flag != this.isVertical)
				{
					this.isVertical = flag;
					if (this.isVertical)
					{
						this.RotateVertical();
						return;
					}
					this.RotateHorizontal();
				}
			}
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void RotateHorizontal()
		{
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void RotateVertical()
		{
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00338B54 File Offset: 0x00336D54
		public virtual void CreateGrid()
		{
			this.models = new ObservableCollection<LiveDataPIDModel>();
			this.Items.Clear();
			for (int i = 0; i < this.ItemsCount; i++)
			{
				DashboardItem dashboardItem = new DashboardItem();
				dashboardItem.Model = new LiveDataPIDModel
				{
					DoubleFormat = dashboardItem.ValueFormat
				};
				this.Items.Add(dashboardItem);
				this.grid.Children.Add(dashboardItem);
			}
			foreach (View view in this.grid.Children.Where((View x) => !this.Items.Contains(x)).ToArray<View>())
			{
				this.grid.Children.Remove(view);
			}
			if (base.Height >= base.Width)
			{
				this.RotateVertical();
				return;
			}
			this.RotateHorizontal();
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00338C2A File Offset: 0x00336E2A
		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SetItemsPlaces();
		}

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x060040FD RID: 16637 RVA: 0x00017A6F File Offset: 0x00015C6F
		public virtual string PreviewFile
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x060040FE RID: 16638 RVA: 0x00338C32 File Offset: 0x00336E32
		// (set) Token: 0x060040FF RID: 16639 RVA: 0x00338C3A File Offset: 0x00336E3A
		public virtual string Title
		{
			get
			{
				return this._Title;
			}
			set
			{
				this._Title = value;
				this.OnPropertyChanged("Title");
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06004100 RID: 16640 RVA: 0x00338C4E File Offset: 0x00336E4E
		public ObservableCollection<DashboardItem> Items
		{
			get
			{
				return this._Items;
			}
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00338C58 File Offset: 0x00336E58
		public void Clear()
		{
			foreach (DashboardItem dashboardItem in this.Items)
			{
				dashboardItem.PID_Id = PID.Empty.Id;
			}
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00338CAC File Offset: 0x00336EAC
		public void RebuildItems()
		{
			foreach (DashboardItem dashboardItem in this.Items)
			{
				dashboardItem.SelectAndAddControl();
			}
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00338CF8 File Offset: 0x00336EF8
		public async Task Start()
		{
			if (!App.OBDSimulator.IsActive && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				await App.OBDReader.ClearRequestQueue();
			}
			int i = 0;
			while (i < this.Items.Count)
			{
				int num = 0;
				try
				{
					this.Items[i].Start();
				}
				catch (Exception obj)
				{
					num = 1;
				}
				if (num == 1)
				{
					object obj;
					Exception ex = (Exception)obj;
					await App.OBDReader.DebugWrite(ex.ToString());
				}
				num = i++;
			}
			List<OBDRequest> list = new List<OBDRequest>(this.Items.Count);
			foreach (DashboardItem dashboardItem in this.Items)
			{
				dashboardItem.Model.GetRequests(list, null, "");
			}
			if (SharedSettings.Current.AlwaysRecordFuelConsumption)
			{
				PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
				if (pid != null)
				{
					LiveDataPIDModel.GetRequests(pid, list, null, "");
				}
			}
			foreach (DashboardPage dashboardPage in DashboardListViewModel.Current.Pages)
			{
				if (dashboardPage != null && dashboardPage != this && dashboardPage.UpdateInBackground)
				{
					foreach (DashboardItem dashboardItem2 in dashboardPage.Items)
					{
						dashboardItem2.Stop();
						dashboardItem2.Start();
						LiveDataPIDModel model = dashboardItem2.Model;
						if (model != null)
						{
							model.GetRequests(list, null, "");
						}
					}
				}
			}
			List<OBDRequest> requestsForDelegate = new List<OBDRequest>(list);
			MainAppRequestProducer.Delegate = delegate(List<OBDRequest> delegateRequests)
			{
				delegateRequests.AddRange(requestsForDelegate);
			};
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00338D3C File Offset: 0x00336F3C
		public virtual void Stop()
		{
			foreach (DashboardItem dashboardItem in this.Items)
			{
				dashboardItem.Stop();
			}
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x00338D88 File Offset: 0x00336F88
		// (set) Token: 0x06004106 RID: 16646 RVA: 0x00338D90 File Offset: 0x00336F90
		public virtual DashboardTypes DashboardType
		{
			[CompilerGenerated]
			get
			{
				return this.<DashboardType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DashboardType>k__BackingField = value;
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06004107 RID: 16647 RVA: 0x00338D99 File Offset: 0x00336F99
		// (set) Token: 0x06004108 RID: 16648 RVA: 0x00338DA1 File Offset: 0x00336FA1
		public bool UpdateInBackground
		{
			get
			{
				return this._UpdateInBackground;
			}
			set
			{
				if (this._UpdateInBackground == value)
				{
					return;
				}
				this._UpdateInBackground = value;
				this.OnPropertyChanged("UpdateInBackground");
			}
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x00338DC0 File Offset: 0x00336FC0
		public void SetItemsPlaces()
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].Place = i;
			}
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00338DF5 File Offset: 0x00336FF5
		public void SetItemPosition(int ItemIdx, int row, int col, int rowSpan, int colSpan)
		{
			if (ItemIdx < this.Items.Count)
			{
				DashboardItem dashboardItem = this.Items[ItemIdx];
				Grid.SetRow(dashboardItem, row);
				Grid.SetColumn(dashboardItem, col);
				Grid.SetRowSpan(dashboardItem, rowSpan);
				Grid.SetColumnSpan(dashboardItem, colSpan);
			}
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00338E2E File Offset: 0x0033702E
		public void SetItemPosition(int ItemIdx, int row, int col)
		{
			this.SetItemPosition(ItemIdx, row, col, 1, 1);
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x00338E3C File Offset: 0x0033703C
		protected void SetRowsAndColumns(int rows, int columns)
		{
			this.grid.ColumnDefinitions.Clear();
			this.grid.RowDefinitions.Clear();
			for (int i = 0; i < rows; i++)
			{
				RowDefinition rowDefinition = new RowDefinition();
				rowDefinition.Height = GridLength.Star;
				this.grid.RowDefinitions.Add(rowDefinition);
			}
			for (int j = 0; j < columns; j++)
			{
				ColumnDefinition columnDefinition = new ColumnDefinition();
				columnDefinition.Width = GridLength.Star;
				this.grid.ColumnDefinitions.Add(columnDefinition);
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00338EC8 File Offset: 0x003370C8
		public static void RegisterDashboardPageTypes()
		{
			StaticLists.DashboardPageTypes.Clear();
			StaticLists.RegisterDashboardType(new Dash_just1());
			StaticLists.RegisterDashboardType(new Dash_just2());
			StaticLists.RegisterDashboardType(new Dash_3vertical());
			StaticLists.RegisterDashboardType(new Dash_1big2small());
			StaticLists.RegisterDashboardType(new Dash_5_big_in_center());
			StaticLists.RegisterDashboardType(new Dash_5vertical());
			StaticLists.RegisterDashboardType(new Dash_1big4small_top());
			StaticLists.RegisterDashboardType(new Dash_4vertical());
			StaticLists.RegisterDashboardType(new Dash_4table());
			StaticLists.RegisterDashboardType(new Dash_6table());
			StaticLists.RegisterDashboardType(new Dash_8table());
			StaticLists.RegisterDashboardType(new Dash_3_1_1_3table());
			StaticLists.RegisterDashboardType(new Dash_3_1_3table());
			StaticLists.RegisterDashboardType(new Dash_3x4table());
			StaticLists.RegisterDashboardType(new Dash_2_1_1_2table());
			StaticLists.RegisterDashboardType(new Dash_6_2_1_2_1());
			StaticLists.RegisterDashboardType(new Dash_3_1_2_2());
			StaticLists.RegisterDashboardType(new Dash_2_1_2_2());
			StaticLists.RegisterDashboardType(new Dash_3_1_3_3());
			StaticLists.RegisterDashboardType(new Dash_3_1_2_3());
			StaticLists.RegisterDashboardType(new Dash_3x5table());
			StaticLists.RegisterDashboardType(new Dash_5x2());
			StaticLists.RegisterDashboardType(new Dash_3x3_w_vert_spaces());
			StaticLists.RegisterDashboardType(new Dash_3x3());
			StaticLists.RegisterDashboardType(new Dash_1_6());
			StaticLists.RegisterDashboardType(new Dash_8vertical());
			StaticLists.RegisterDashboardType(new Dash_3_3_1_3_3());
			StaticLists.RegisterDashboardType(new Dash_4square_6lines());
			StaticLists.RegisterDashboardType(new Dash_CustomPage());
			StaticLists.RegisterDashboardType(new Dash_4x4());
			StaticLists.RegisterDashboardType(new Dash_4x5());
			StaticLists.RegisterDashboardType(new Dash_4x6());
			StaticLists.RegisterDashboardType(new Dash_6x5());
			StaticLists.RegisterDashboardType(new Dash_5x5());
			StaticLists.RegisterDashboardType(new DashAbsLayout8_1());
			StaticLists.RegisterDashboardType(new DashAbsLayout8_2());
			StaticLists.RegisterDashboardType(new DashAbsLayout7_1());
			StaticLists.RegisterDashboardType(new DashAbsLayout7_2());
			StaticLists.RegisterDashboardType(new Dash_3_2_3());
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x00339068 File Offset: 0x00337268
		public static DashboardPage GetDashboardPageFromDashboardType(DashboardTypes dashType)
		{
			switch (dashType)
			{
			case DashboardTypes.ThreeInARow:
				return new Dash_3vertical();
			case DashboardTypes.OneAndTwo:
				return new Dash_1big2small();
			case DashboardTypes.FiveBigCenter:
				return new Dash_5_big_in_center();
			case DashboardTypes.FiveBigTop:
				return new Dash_1big4small_top();
			case DashboardTypes.FourVertical:
				return new Dash_4vertical();
			case DashboardTypes.FourTable:
				return new Dash_4table();
			case DashboardTypes.SixTable:
				return new Dash_6table();
			case DashboardTypes.EightTable:
				return new Dash_8table();
			case DashboardTypes.Custom:
				return new Dash_CustomPage();
			case DashboardTypes.Just1:
				return new Dash_just1();
			case DashboardTypes.Just2:
				return new Dash_just2();
			case DashboardTypes.Dash_3_1_1_3:
				return new Dash_3_1_1_3table();
			case DashboardTypes.Dash_3_1_3:
				return new Dash_3_1_3table();
			case DashboardTypes.Dash_3x4:
				return new Dash_3x4table();
			case DashboardTypes.Dash_2_1_1_2table:
				return new Dash_2_1_1_2table();
			case DashboardTypes.Dash_5Vertical:
				return new Dash_5vertical();
			case DashboardTypes.Dash_6_2_1_2_1:
				return new Dash_6_2_1_2_1();
			case DashboardTypes.Dash_3_1_2_2:
				return new Dash_3_1_2_2();
			case DashboardTypes.Dash_2_1_2_2:
				return new Dash_2_1_2_2();
			case DashboardTypes.Dash_3_1_3_3:
				return new Dash_3_1_3_3();
			case DashboardTypes.Dash_3_1_2_3:
				return new Dash_3_1_2_3();
			case DashboardTypes.Dash_3x5table:
				return new Dash_3x5table();
			case DashboardTypes.Dash_5x2:
				return new Dash_5x2();
			case DashboardTypes.Dash_3x3_w_vert_spaces:
				return new Dash_3x3_w_vert_spaces();
			case DashboardTypes.Dash_3x3:
				return new Dash_3x3();
			case DashboardTypes.Dash_1_6:
				return new Dash_1_6();
			case DashboardTypes.Dash_8vertical:
				return new Dash_8vertical();
			case DashboardTypes.Dash_3_3_1_3_3:
				return new Dash_3_3_1_3_3();
			case DashboardTypes.Dash_4square_6lines:
				return new Dash_4square_6lines();
			case DashboardTypes.Dash_4x4:
				return new Dash_4x4();
			case DashboardTypes.Dash_5x5:
				return new Dash_5x5();
			case DashboardTypes.Dash_4x5:
				return new Dash_4x5();
			case DashboardTypes.Dash_4x6:
				return new Dash_4x6();
			case DashboardTypes.Dash_6x5:
				return new Dash_6x5();
			case DashboardTypes.DashAbsLayout8_1:
				return new DashAbsLayout8_1();
			case DashboardTypes.DashAbsLayout8_2:
				return new DashAbsLayout8_2();
			case DashboardTypes.DashAbsLayout7_1:
				return new DashAbsLayout7_1();
			case DashboardTypes.DashAbsLayout7_2:
				return new DashAbsLayout7_2();
			case DashboardTypes.Dash_3_2_3:
				return new Dash_3_2_3();
			default:
				return new Dash_3vertical();
			}
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x0033920B File Offset: 0x0033740B
		[CompilerGenerated]
		private bool <CreateGrid>b__11_0(View x)
		{
			return !this.Items.Contains(x);
		}

		// Token: 0x0400280B RID: 10251
		[CompilerGenerated]
		private int <ItemsCount>k__BackingField;

		// Token: 0x0400280C RID: 10252
		protected Grid grid;

		// Token: 0x0400280D RID: 10253
		private bool isVertical = true;

		// Token: 0x0400280E RID: 10254
		public ObservableCollection<LiveDataPIDModel> models;

		// Token: 0x0400280F RID: 10255
		private string _Title = "";

		// Token: 0x04002810 RID: 10256
		private ObservableCollection<DashboardItem> _Items = new ObservableCollection<DashboardItem>();

		// Token: 0x04002811 RID: 10257
		[CompilerGenerated]
		private DashboardTypes <DashboardType>k__BackingField;

		// Token: 0x04002812 RID: 10258
		private bool _UpdateInBackground;

		// Token: 0x02000772 RID: 1906
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004110 RID: 16656 RVA: 0x0033921C File Offset: 0x0033741C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004111 RID: 16657 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004112 RID: 16658 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <Start>b__24_1(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x04002813 RID: 10259
			public static readonly DashboardPage.<>c <>9 = new DashboardPage.<>c();

			// Token: 0x04002814 RID: 10260
			public static Func<PID, bool> <>9__24_1;
		}

		// Token: 0x02000773 RID: 1907
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			// Token: 0x06004113 RID: 16659 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass24_0()
			{
			}

			// Token: 0x06004114 RID: 16660 RVA: 0x00339228 File Offset: 0x00337428
			internal void <Start>b__0(List<OBDRequest> delegateRequests)
			{
				delegateRequests.AddRange(this.requestsForDelegate);
			}

			// Token: 0x04002815 RID: 10261
			public List<OBDRequest> requestsForDelegate;
		}

		// Token: 0x02000774 RID: 1908
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__24 : IAsyncStateMachine
		{
			// Token: 0x06004115 RID: 16661 RVA: 0x00339238 File Offset: 0x00337438
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardPage dashboardPage = this;
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
							num = (num2 = -1);
							goto IL_0136;
						}
						CS$<>8__locals1 = new DashboardPage.<>c__DisplayClass24_0();
						if (App.OBDSimulator.IsActive || App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
						{
							goto IL_009B;
						}
						taskAwaiter = App.OBDReader.ClearRequestQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardPage.<Start>d__24>(ref taskAwaiter, ref this);
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
					IL_009B:
					i = 0;
					goto IL_014F;
					IL_0136:
					taskAwaiter.GetResult();
					IL_013D:
					int num3 = i;
					i = num3 + 1;
					IL_014F:
					if (i >= dashboardPage.Items.Count)
					{
						List<OBDRequest> list = new List<OBDRequest>(dashboardPage.Items.Count);
						IEnumerator<DashboardItem> enumerator = dashboardPage.Items.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DashboardItem dashboardItem = enumerator.Current;
								dashboardItem.Model.GetRequests(list, null, "");
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (SharedSettings.Current.AlwaysRecordFuelConsumption)
						{
							PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
							if (pid != null)
							{
								LiveDataPIDModel.GetRequests(pid, list, null, "");
							}
						}
						IEnumerator<DashboardPage> enumerator2 = DashboardListViewModel.Current.Pages.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								DashboardPage dashboardPage2 = enumerator2.Current;
								if (dashboardPage2 != null && dashboardPage2 != dashboardPage && dashboardPage2.UpdateInBackground)
								{
									enumerator = dashboardPage2.Items.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											DashboardItem dashboardItem2 = enumerator.Current;
											dashboardItem2.Stop();
											dashboardItem2.Start();
											LiveDataPIDModel model = dashboardItem2.Model;
											if (model != null)
											{
												model.GetRequests(list, null, "");
											}
										}
									}
									finally
									{
										if (num < 0 && enumerator != null)
										{
											enumerator.Dispose();
										}
									}
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						CS$<>8__locals1.requestsForDelegate = new List<OBDRequest>(list);
						MainAppRequestProducer.Delegate = delegate(List<OBDRequest> delegateRequests)
						{
							delegateRequests.AddRange(CS$<>8__locals1.requestsForDelegate);
						};
						RequestProducerStatic.UpdateOBDReaderRequests();
					}
					else
					{
						num3 = 0;
						try
						{
							dashboardPage.Items[i].Start();
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 != 1)
						{
							goto IL_013D;
						}
						object obj;
						Exception ex = (Exception)obj;
						taskAwaiter = App.OBDReader.DebugWrite(ex.ToString()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardPage.<Start>d__24>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0136;
					}
				}
				catch (Exception ex2)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004116 RID: 16662 RVA: 0x003395C8 File Offset: 0x003377C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002816 RID: 10262
			public int <>1__state;

			// Token: 0x04002817 RID: 10263
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002818 RID: 10264
			public DashboardPage <>4__this;

			// Token: 0x04002819 RID: 10265
			private DashboardPage.<>c__DisplayClass24_0 <>8__1;

			// Token: 0x0400281A RID: 10266
			private TaskAwaiter <>u__1;

			// Token: 0x0400281B RID: 10267
			private int <i>5__2;
		}
	}
}
