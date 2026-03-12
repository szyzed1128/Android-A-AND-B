using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200074A RID: 1866
	internal class MultiplePIDViewModel : LiveDataPIDModel
	{
		// Token: 0x06003F8E RID: 16270 RVA: 0x00331138 File Offset: 0x0032F338
		public MultiplePIDViewModel()
		{
			base.Values = new SmartCollection<DoubleValueItem>();
		}

		// Token: 0x170014B4 RID: 5300
		// (get) Token: 0x06003F8F RID: 16271 RVA: 0x00331161 File Offset: 0x0032F361
		// (set) Token: 0x06003F90 RID: 16272 RVA: 0x00331169 File Offset: 0x0032F369
		public ObservableCollection<int> SelectedPIDs
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedPIDs>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedPIDs>k__BackingField = value;
			}
		} = new ObservableCollection<int>();

		// Token: 0x170014B5 RID: 5301
		// (get) Token: 0x06003F91 RID: 16273 RVA: 0x00331172 File Offset: 0x0032F372
		// (set) Token: 0x06003F92 RID: 16274 RVA: 0x0033117A File Offset: 0x0032F37A
		public ObservableCollection<LiveDataPIDModel> InternalModels
		{
			[CompilerGenerated]
			get
			{
				return this.<InternalModels>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InternalModels>k__BackingField = value;
			}
		} = new ObservableCollection<LiveDataPIDModel>();

		// Token: 0x06003F93 RID: 16275 RVA: 0x00331184 File Offset: 0x0032F384
		public void SetPIDs(List<int> ids)
		{
			this.SelectedPIDs.Clear();
			foreach (LiveDataPIDModel liveDataPIDModel in this.InternalModels)
			{
				liveDataPIDModel.Unsubscribe();
				liveDataPIDModel.SelectedPID = PID.Empty;
			}
			this.InternalModels.Clear();
			for (int i = 0; i < ids.Count; i++)
			{
				int id = ids[i];
				IPID ipid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x.Id == id);
				if (ipid == null)
				{
					ipid = PID.Empty;
				}
				else
				{
					this.SelectedPIDs.Add(id);
					LiveDataPIDModel liveDataPIDModel2 = new LiveDataPIDModel();
					liveDataPIDModel2.SelectedPID = ipid;
					liveDataPIDModel2.PropertyChanged += this.Model_PropertyChanged;
					liveDataPIDModel2.Mode = LiveDataModes.Dashboard;
					this.InternalModels.Add(liveDataPIDModel2);
					base.Values.Add(new DoubleValueItem(double.NaN, default(TimeSpan)));
				}
			}
			if (this.InternalModels.Count > 0)
			{
				string firstUnit = this.InternalModels[0].Units;
				if (this.InternalModels.Skip(1).All((LiveDataPIDModel x) => x.Units == firstUnit))
				{
					base.Units = firstUnit;
					return;
				}
				base.Units = "";
			}
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0033130C File Offset: 0x0032F50C
		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FloatValue")
			{
				LiveDataPIDModel liveDataPIDModel = (LiveDataPIDModel)sender;
				int num = this.InternalModels.IndexOf(liveDataPIDModel);
				if (liveDataPIDModel.Values.Count == 0)
				{
					return;
				}
				DoubleValueItem doubleValueItem = new DoubleValueItem(liveDataPIDModel.Values[liveDataPIDModel.Values.Count - 1].Value, TimeSpan.FromSeconds((double)num));
				base.Values[num] = doubleValueItem;
				this.UpdateMinMaxAvg();
			}
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x00331390 File Offset: 0x0032F590
		private void UpdateMinMaxAvg()
		{
			double num = double.NaN;
			double num2 = double.NaN;
			double num3 = 0.0;
			int num4 = 0;
			for (int i = 0; i < base.Values.Count; i++)
			{
				double value = base.Values[i].Value;
				if (double.IsFinite(value))
				{
					if (double.IsNaN(num))
					{
						num = value;
					}
					else if (value < num)
					{
						num = value;
					}
					if (double.IsNaN(num2))
					{
						num2 = value;
					}
					else if (value > num2)
					{
						num2 = value;
					}
					num3 += value;
					num4++;
				}
			}
			base.AverageTextValue = (num3 / (double)num4).ToString(StaticLists.DoubleFormats[base.DoubleFormat]);
			base.MinimumAchieved = num;
			base.MaximumAchieved = num2;
		}

		// Token: 0x170014B6 RID: 5302
		// (get) Token: 0x06003F96 RID: 16278 RVA: 0x0033145C File Offset: 0x0032F65C
		// (set) Token: 0x06003F97 RID: 16279 RVA: 0x00331482 File Offset: 0x0032F682
		public override IPID SelectedPID
		{
			get
			{
				if (this.InternalModels.Count == 0)
				{
					return PID.Empty;
				}
				return this.InternalModels[0].SelectedPID;
			}
			set
			{
				this.Unsubscribe();
				this.Subscribe();
			}
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00331490 File Offset: 0x0032F690
		public override void GetRequests(List<OBDRequest> requestsQueue, string addKey = null, string addValue = "")
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.InternalModels)
			{
				liveDataPIDModel.GetRequests(requestsQueue, null, "");
			}
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x003314E4 File Offset: 0x0032F6E4
		public override void Unsubscribe()
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.InternalModels)
			{
				liveDataPIDModel.Unsubscribe();
			}
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x00331530 File Offset: 0x0032F730
		public override void Subscribe()
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.InternalModels)
			{
				liveDataPIDModel.Subscribe();
			}
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0033157C File Offset: 0x0032F77C
		public override void Dispose()
		{
			foreach (LiveDataPIDModel liveDataPIDModel in this.InternalModels)
			{
				liveDataPIDModel.PropertyChanged -= this.Model_PropertyChanged;
				liveDataPIDModel.Dispose();
			}
			this.InternalModels.Clear();
		}

		// Token: 0x040026E3 RID: 9955
		[CompilerGenerated]
		private ObservableCollection<int> <SelectedPIDs>k__BackingField;

		// Token: 0x040026E4 RID: 9956
		[CompilerGenerated]
		private ObservableCollection<LiveDataPIDModel> <InternalModels>k__BackingField;

		// Token: 0x0200074B RID: 1867
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06003F9C RID: 16284 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06003F9D RID: 16285 RVA: 0x003315E4 File Offset: 0x0032F7E4
			internal bool <SetPIDs>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040026E5 RID: 9957
			public int id;
		}

		// Token: 0x0200074C RID: 1868
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_1
		{
			// Token: 0x06003F9E RID: 16286 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_1()
			{
			}

			// Token: 0x06003F9F RID: 16287 RVA: 0x003315F4 File Offset: 0x0032F7F4
			internal bool <SetPIDs>b__1(LiveDataPIDModel x)
			{
				return x.Units == this.firstUnit;
			}

			// Token: 0x040026E6 RID: 9958
			public string firstUnit;
		}
	}
}
