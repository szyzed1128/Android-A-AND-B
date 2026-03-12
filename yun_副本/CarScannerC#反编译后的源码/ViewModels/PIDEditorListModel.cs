using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200075A RID: 1882
	internal class PIDEditorListModel : SmartCollection<IPID>, INotifyPropertyChanged
	{
		// Token: 0x06003FCB RID: 16331 RVA: 0x0033463C File Offset: 0x0033283C
		public PIDEditorListModel()
		{
			this._View = SharedSettings.Current.PidListLastViewByType;
		}

		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x06003FCC RID: 16332 RVA: 0x00334675 File Offset: 0x00332875
		// (set) Token: 0x06003FCD RID: 16333 RVA: 0x0033467D File Offset: 0x0033287D
		public PIDEditorListModel.ViewByType View
		{
			get
			{
				return this._View;
			}
			set
			{
				if (this._View != value)
				{
					this._View = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("View"));
					SharedSettings.Current.PidListLastViewByType = value;
					this.UpdateBaseList(true);
				}
			}
		}

		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x06003FCE RID: 16334 RVA: 0x003346B2 File Offset: 0x003328B2
		// (set) Token: 0x06003FCF RID: 16335 RVA: 0x003346BA File Offset: 0x003328BA
		public string Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				if (value != this._Filter)
				{
					this._Filter = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Filter"));
					this.UpdateFilter(true);
				}
			}
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x003346EC File Offset: 0x003328EC
		private async Task UpdateFilter(bool updateWithDelay)
		{
			string remember = this._Filter;
			if (updateWithDelay)
			{
				await Task.Delay(500);
			}
			if (this._Filter == remember)
			{
				if (string.IsNullOrEmpty(this._Filter))
				{
					base.Reset(this.BaseList);
				}
				else
				{
					List<IPID> list = new List<IPID>(this.BaseList);
					string[] array = this._Filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string word = array[i];
						list = list.Where((IPID x) => (x.OriginalName != null && x.OriginalName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.OriginalShortName != null && x.OriginalShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.CustomName != null && x.CustomName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.CustomShortName != null && x.CustomShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<IPID>();
					}
					base.Reset(list);
				}
			}
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00334738 File Offset: 0x00332938
		public async Task UpdateBaseList(bool withDelay)
		{
			PIDEditorListModel.ViewByType selectedView = this.View;
			if (PlatformHelper.IsiOS && withDelay)
			{
				await Task.Delay(500);
			}
			if (this.View == selectedView)
			{
				this.IsSelectProfileButtonVisible = false;
				switch (this.View)
				{
				case PIDEditorListModel.ViewByType.All:
					this.BaseList.Clear();
					this.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A")));
					this.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
					this.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
					this.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
					break;
				case PIDEditorListModel.ViewByType.OBDII:
					this.BaseList.Clear();
					this.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A")));
					break;
				case PIDEditorListModel.ViewByType.ProfilePids:
					this.BaseList.Clear();
					this.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
					this.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
					if (this.BaseList.Count == 0)
					{
						this.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
						this.IsSelectProfileButtonVisible = true;
					}
					break;
				case PIDEditorListModel.ViewByType.ActiveProfilePids:
					this.BaseList.Clear();
					this.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
					if (this.BaseList.Count == 0)
					{
						this.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
						this.IsSelectProfileButtonVisible = true;
					}
					break;
				case PIDEditorListModel.ViewByType.DisabledProfilePids:
					this.BaseList.Clear();
					this.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
					if (this.BaseList.Count == 0)
					{
						this.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
						this.IsSelectProfileButtonVisible = true;
					}
					break;
				case PIDEditorListModel.ViewByType.UserPids:
					this.BaseList.Clear();
					this.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
					if (this.BaseList.Count == 0)
					{
						this.EmptyListText = Translate.GetString("ios_NothingYetHereSensors");
					}
					break;
				case PIDEditorListModel.ViewByType.LastCarSupported:
					this.BaseList.Clear();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						List<PID> list = LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status).ToList<PID>();
						this.BaseList.AddRange(list);
					}
					else
					{
						if (!string.IsNullOrEmpty(SharedSettings.Current.LastCarAvailableSensors))
						{
							IEnumerable<int> enumerable = from x in SharedSettings.Current.LastCarAvailableSensors.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select int.Parse(x);
							List<IPID> list2 = new List<IPID>();
							list2.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status));
							list2.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
							list2.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
							list2.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
							using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									int id = enumerator.Current;
									IPID ipid = list2.FirstOrDefault((IPID x) => x.Id == id);
									if (ipid != null)
									{
										this.BaseList.Add(ipid);
									}
								}
								break;
							}
						}
						this.EmptyListText = Translate.GetString("ios_PleaseConnectFirst");
					}
					break;
				case PIDEditorListModel.ViewByType.WithRoles:
					this.BaseList.Clear();
					this.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A") && x.Role > Roles.None));
					this.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
					this.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
					this.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
					break;
				}
				if (this.BaseList.Count > 0)
				{
					this.EmptyListText = "";
				}
				await this.UpdateFilter(false);
			}
		}

		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x00334783 File Offset: 0x00332983
		// (set) Token: 0x06003FD3 RID: 16339 RVA: 0x0033478B File Offset: 0x0033298B
		public string EmptyListText
		{
			get
			{
				return this._EmptyListText;
			}
			set
			{
				this._EmptyListText = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("EmptyListText"));
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x06003FD4 RID: 16340 RVA: 0x003347A4 File Offset: 0x003329A4
		// (set) Token: 0x06003FD5 RID: 16341 RVA: 0x003347AC File Offset: 0x003329AC
		public bool IsSelectProfileButtonVisible
		{
			get
			{
				return this._IsSelectProfileButtonVisible;
			}
			set
			{
				this._IsSelectProfileButtonVisible = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("IsSelectProfileButtonVisible"));
			}
		}

		// Token: 0x04002723 RID: 10019
		private List<IPID> BaseList = new List<IPID>();

		// Token: 0x04002724 RID: 10020
		private PIDEditorListModel.ViewByType _View;

		// Token: 0x04002725 RID: 10021
		private string _Filter = "";

		// Token: 0x04002726 RID: 10022
		private string _EmptyListText = "";

		// Token: 0x04002727 RID: 10023
		private bool _IsSelectProfileButtonVisible;

		// Token: 0x0200075B RID: 1883
		public enum ViewByType
		{
			// Token: 0x04002729 RID: 10025
			All,
			// Token: 0x0400272A RID: 10026
			OBDII,
			// Token: 0x0400272B RID: 10027
			ProfilePids,
			// Token: 0x0400272C RID: 10028
			ActiveProfilePids,
			// Token: 0x0400272D RID: 10029
			DisabledProfilePids,
			// Token: 0x0400272E RID: 10030
			UserPids,
			// Token: 0x0400272F RID: 10031
			LastCarSupported,
			// Token: 0x04002730 RID: 10032
			WithRoles
		}

		// Token: 0x0200075C RID: 1884
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003FD6 RID: 16342 RVA: 0x003347C5 File Offset: 0x003329C5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003FD7 RID: 16343 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003FD8 RID: 16344 RVA: 0x003347D4 File Offset: 0x003329D4
			internal bool <UpdateBaseList>b__12_0(PID x)
			{
				return (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A");
			}

			// Token: 0x06003FD9 RID: 16345 RVA: 0x00334828 File Offset: 0x00332A28
			internal bool <UpdateBaseList>b__12_1(PID x)
			{
				return (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A");
			}

			// Token: 0x06003FDA RID: 16346 RVA: 0x001355C1 File Offset: 0x001337C1
			internal bool <UpdateBaseList>b__12_6(PID x)
			{
				return x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status;
			}

			// Token: 0x06003FDB RID: 16347 RVA: 0x0033487C File Offset: 0x00332A7C
			internal int <UpdateBaseList>b__12_7(string x)
			{
				return int.Parse(x);
			}

			// Token: 0x06003FDC RID: 16348 RVA: 0x001355C1 File Offset: 0x001337C1
			internal bool <UpdateBaseList>b__12_8(PID x)
			{
				return x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status;
			}

			// Token: 0x06003FDD RID: 16349 RVA: 0x00334884 File Offset: 0x00332A84
			internal bool <UpdateBaseList>b__12_2(PID x)
			{
				return (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A") && x.Role > Roles.None;
			}

			// Token: 0x06003FDE RID: 16350 RVA: 0x003348E0 File Offset: 0x00332AE0
			internal bool <UpdateBaseList>b__12_3(CustomPID x)
			{
				return x.Role > Roles.None;
			}

			// Token: 0x06003FDF RID: 16351 RVA: 0x003348E0 File Offset: 0x00332AE0
			internal bool <UpdateBaseList>b__12_4(CustomPID x)
			{
				return x.Role > Roles.None;
			}

			// Token: 0x06003FE0 RID: 16352 RVA: 0x003348E0 File Offset: 0x00332AE0
			internal bool <UpdateBaseList>b__12_5(CustomPID x)
			{
				return x.Role > Roles.None;
			}

			// Token: 0x04002731 RID: 10033
			public static readonly PIDEditorListModel.<>c <>9 = new PIDEditorListModel.<>c();

			// Token: 0x04002732 RID: 10034
			public static Func<PID, bool> <>9__12_0;

			// Token: 0x04002733 RID: 10035
			public static Func<PID, bool> <>9__12_1;

			// Token: 0x04002734 RID: 10036
			public static Func<PID, bool> <>9__12_6;

			// Token: 0x04002735 RID: 10037
			public static Func<string, int> <>9__12_7;

			// Token: 0x04002736 RID: 10038
			public static Func<PID, bool> <>9__12_8;

			// Token: 0x04002737 RID: 10039
			public static Func<PID, bool> <>9__12_2;

			// Token: 0x04002738 RID: 10040
			public static Func<CustomPID, bool> <>9__12_3;

			// Token: 0x04002739 RID: 10041
			public static Func<CustomPID, bool> <>9__12_4;

			// Token: 0x0400273A RID: 10042
			public static Func<CustomPID, bool> <>9__12_5;
		}

		// Token: 0x0200075D RID: 1885
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x06003FE1 RID: 16353 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x06003FE2 RID: 16354 RVA: 0x003348EC File Offset: 0x00332AEC
			internal bool <UpdateFilter>b__0(IPID x)
			{
				return (x.OriginalName != null && x.OriginalName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.OriginalShortName != null && x.OriginalShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.CustomName != null && x.CustomName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (x.CustomShortName != null && x.CustomShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x0400273B RID: 10043
			public string word;
		}

		// Token: 0x0200075E RID: 1886
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06003FE3 RID: 16355 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06003FE4 RID: 16356 RVA: 0x00334974 File Offset: 0x00332B74
			internal bool <UpdateBaseList>b__9(IPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x0400273C RID: 10044
			public int id;
		}

		// Token: 0x0200075F RID: 1887
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateBaseList>d__12 : IAsyncStateMachine
		{
			// Token: 0x06003FE5 RID: 16357 RVA: 0x00334984 File Offset: 0x00332B84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDEditorListModel pideditorListModel = this;
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
							goto IL_05ED;
						}
						selectedView = pideditorListModel.View;
						if (!PlatformHelper.IsiOS || !withDelay)
						{
							goto IL_0092;
						}
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDEditorListModel.<UpdateBaseList>d__12>(ref taskAwaiter, ref this);
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
					IL_0092:
					if (pideditorListModel.View != selectedView)
					{
						goto IL_05F4;
					}
					pideditorListModel.IsSelectProfileButtonVisible = false;
					switch (pideditorListModel.View)
					{
					case PIDEditorListModel.ViewByType.All:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A")));
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
						break;
					case PIDEditorListModel.ViewByType.OBDII:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A")));
						break;
					case PIDEditorListModel.ViewByType.ProfilePids:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
						if (pideditorListModel.BaseList.Count == 0)
						{
							pideditorListModel.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
							pideditorListModel.IsSelectProfileButtonVisible = true;
						}
						break;
					case PIDEditorListModel.ViewByType.ActiveProfilePids:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
						if (pideditorListModel.BaseList.Count == 0)
						{
							pideditorListModel.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
							pideditorListModel.IsSelectProfileButtonVisible = true;
						}
						break;
					case PIDEditorListModel.ViewByType.DisabledProfilePids:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
						if (pideditorListModel.BaseList.Count == 0)
						{
							pideditorListModel.EmptyListText = Translate.GetString("ios_SelectedProfileDoesntHaveSensors");
							pideditorListModel.IsSelectProfileButtonVisible = true;
						}
						break;
					case PIDEditorListModel.ViewByType.UserPids:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
						if (pideditorListModel.BaseList.Count == 0)
						{
							pideditorListModel.EmptyListText = Translate.GetString("ios_NothingYetHereSensors");
						}
						break;
					case PIDEditorListModel.ViewByType.LastCarSupported:
						pideditorListModel.BaseList.Clear();
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
						{
							List<PID> list = LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status).ToList<PID>();
							pideditorListModel.BaseList.AddRange(list);
						}
						else
						{
							if (!string.IsNullOrEmpty(SharedSettings.Current.LastCarAvailableSensors))
							{
								IEnumerable<int> enumerable = from x in SharedSettings.Current.LastCarAvailableSensors.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select int.Parse(x);
								List<IPID> list2 = new List<IPID>();
								list2.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status));
								list2.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
								list2.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
								list2.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
								IEnumerator<int> enumerator = enumerable.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										int num3 = enumerator.Current;
										IPID ipid = list2.FirstOrDefault(new Func<IPID, bool>(new PIDEditorListModel.<>c__DisplayClass12_0
										{
											id = num3
										}.<UpdateBaseList>b__9));
										if (ipid != null)
										{
											pideditorListModel.BaseList.Add(ipid);
										}
									}
									break;
								}
								finally
								{
									if (num < 0 && enumerator != null)
									{
										enumerator.Dispose();
									}
								}
							}
							pideditorListModel.EmptyListText = Translate.GetString("ios_PleaseConnectFirst");
						}
						break;
					case PIDEditorListModel.ViewByType.WithRoles:
						pideditorListModel.BaseList.Clear();
						pideditorListModel.BaseList.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => (x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status) && !x.Command.StartsWith("09") && !x.Command.StartsWith("1A") && x.Role > Roles.None));
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
						pideditorListModel.BaseList.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.Role > Roles.None));
						break;
					}
					if (pideditorListModel.BaseList.Count > 0)
					{
						pideditorListModel.EmptyListText = "";
					}
					taskAwaiter = pideditorListModel.UpdateFilter(false).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDEditorListModel.<UpdateBaseList>d__12>(ref taskAwaiter, ref this);
						return;
					}
					IL_05ED:
					taskAwaiter.GetResult();
					IL_05F4:;
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

			// Token: 0x06003FE6 RID: 16358 RVA: 0x00334FE8 File Offset: 0x003331E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400273D RID: 10045
			public int <>1__state;

			// Token: 0x0400273E RID: 10046
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400273F RID: 10047
			public PIDEditorListModel <>4__this;

			// Token: 0x04002740 RID: 10048
			public bool withDelay;

			// Token: 0x04002741 RID: 10049
			private PIDEditorListModel.ViewByType <selectedView>5__2;

			// Token: 0x04002742 RID: 10050
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000760 RID: 1888
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateFilter>d__11 : IAsyncStateMachine
		{
			// Token: 0x06003FE7 RID: 16359 RVA: 0x00334FF8 File Offset: 0x003331F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDEditorListModel pideditorListModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						remember = pideditorListModel._Filter;
						if (!updateWithDelay)
						{
							goto IL_0084;
						}
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDEditorListModel.<UpdateFilter>d__11>(ref taskAwaiter, ref this);
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
					IL_0084:
					if (pideditorListModel._Filter == remember)
					{
						if (string.IsNullOrEmpty(pideditorListModel._Filter))
						{
							pideditorListModel.Reset(pideditorListModel.BaseList);
						}
						else
						{
							List<IPID> list = new List<IPID>(pideditorListModel.BaseList);
							string[] array = pideditorListModel._Filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < array.Length; i++)
							{
								list = list.Where(new Func<IPID, bool>(new PIDEditorListModel.<>c__DisplayClass11_0
								{
									word = array[i]
								}.<UpdateFilter>b__0)).ToList<IPID>();
							}
							pideditorListModel.Reset(list);
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					remember = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				remember = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003FE8 RID: 16360 RVA: 0x00335180 File Offset: 0x00333380
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002743 RID: 10051
			public int <>1__state;

			// Token: 0x04002744 RID: 10052
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002745 RID: 10053
			public PIDEditorListModel <>4__this;

			// Token: 0x04002746 RID: 10054
			public bool updateWithDelay;

			// Token: 0x04002747 RID: 10055
			private string <remember>5__2;

			// Token: 0x04002748 RID: 10056
			private TaskAwaiter <>u__1;
		}
	}
}
