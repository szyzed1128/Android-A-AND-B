using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x020001B1 RID: 433
	public class DashboardListViewModel
	{
		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x000AB1DE File Offset: 0x000A93DE
		// (set) Token: 0x06001704 RID: 5892 RVA: 0x000027D4 File Offset: 0x000009D4
		public static DashboardListViewModel Current
		{
			get
			{
				if (DashboardListViewModel._Current == null)
				{
					DashboardListViewModel._Current = new DashboardListViewModel();
				}
				return DashboardListViewModel._Current;
			}
			set
			{
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000AB1F6 File Offset: 0x000A93F6
		public DashboardListViewModel()
		{
			this._Pages = new ObservableCollection<DashboardPage>();
		}

		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x000AB214 File Offset: 0x000A9414
		// (set) Token: 0x06001707 RID: 5895 RVA: 0x000AB21C File Offset: 0x000A941C
		public bool IsReady
		{
			[CompilerGenerated]
			get
			{
				return this.<IsReady>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsReady>k__BackingField = value;
			}
		}

		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06001708 RID: 5896 RVA: 0x000AB225 File Offset: 0x000A9425
		public ObservableCollection<DashboardPage> Pages
		{
			get
			{
				return this._Pages;
			}
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000AB230 File Offset: 0x000A9430
		public void LoadDashboardFromSettings()
		{
			string dashboard = SharedSettings.Current.Dashboard;
			if (string.IsNullOrEmpty(dashboard))
			{
				App.OBDReader.DebugWrite("\r\n[Dashboard: Empty]\r\n");
				this.ResetDashboardToDefault();
				return;
			}
			try
			{
				if (this.Pages != null && this.Pages.Count > 0)
				{
					foreach (DashboardPage dashboardPage in this.Pages)
					{
						dashboardPage.Stop();
					}
				}
				this.Pages.Clear();
				List<ProxyPage> list = Json.DeserializeObject<List<ProxyPage>>(dashboard);
				if (list.Count > 0)
				{
					using (List<ProxyPage>.Enumerator enumerator2 = list.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ProxyPage proxyPage = enumerator2.Current;
							try
							{
								this.ApplyPatch(proxyPage);
								DashboardPage pageFromProxyPage = ProxyPage.GetPageFromProxyPage(proxyPage);
								if (pageFromProxyPage != null)
								{
									this.Pages.Add(pageFromProxyPage);
								}
							}
							catch (Exception ex)
							{
								App.OBDReader.DebugWrite(string.Concat(new string[]
								{
									"\r\n[Dashboard error exc2 [",
									proxyPage.Title ?? "null",
									"]: ",
									ex.ToString(),
									"]\r\n"
								}));
							}
						}
						goto IL_0133;
					}
				}
				this.ResetDashboardToDefault();
				IL_0133:;
			}
			catch (Exception ex2)
			{
				DashboardListViewModel.<>c__DisplayClass12_0 CS$<>8__locals1 = new DashboardListViewModel.<>c__DisplayClass12_0();
				PCLDebugStream.CurrentInstance.WriteLineAsync("\r\n[Dashboard loading error: " + ex2.ToString() + "]\r\n");
				CS$<>8__locals1.curPage = App.GetCurrentPage();
				CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
				CS$<>8__locals1.retry = false;
				MainThread.InvokeOnMainThreadAsync(delegate
				{
					DashboardListViewModel.<>c__DisplayClass12_0.<<LoadDashboardFromSettings>b__0>d <<LoadDashboardFromSettings>b__0>d;
					<<LoadDashboardFromSettings>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<LoadDashboardFromSettings>b__0>d.<>4__this = CS$<>8__locals1;
					<<LoadDashboardFromSettings>b__0>d.<>1__state = -1;
					<<LoadDashboardFromSettings>b__0>d.<>t__builder.Start<DashboardListViewModel.<>c__DisplayClass12_0.<<LoadDashboardFromSettings>b__0>d>(ref <<LoadDashboardFromSettings>b__0>d);
					return <<LoadDashboardFromSettings>b__0>d.<>t__builder.Task;
				});
				CS$<>8__locals1.semaphore.Wait();
				if (CS$<>8__locals1.retry)
				{
					this.LoadDashboardFromSettings();
				}
				else if (CS$<>8__locals1.curPage != null && CS$<>8__locals1.curPage is DashboardXamlPage)
				{
					CS$<>8__locals1.curPage.Navigation.PopAsync();
				}
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x000AB480 File Offset: 0x000A9680
		private void ApplyPatch(ProxyPage page)
		{
			foreach (ProxyItem proxyItem in page.Items)
			{
				if (proxyItem.PID_Id == 12935009)
				{
					proxyItem.PID_Id = 801;
				}
			}
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x000AB4E4 File Offset: 0x000A96E4
		public void SaveDashboardToSettings(List<ProxyPage> proxyPages)
		{
			string text = Json.SerializeObject(proxyPages);
			SharedSettings.Current.Dashboard = text;
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x000AB504 File Offset: 0x000A9704
		public void SaveDashboardToSettings()
		{
			List<ProxyPage> list = new List<ProxyPage>(this.Pages.Count);
			foreach (DashboardPage dashboardPage in this.Pages)
			{
				ProxyPage proxyPage = new ProxyPage(dashboardPage);
				list.Add(proxyPage);
			}
			if (list.Count == 0)
			{
				return;
			}
			string text = Json.SerializeObject(list);
			if (string.IsNullOrEmpty(text) || text == "[]")
			{
				return;
			}
			object obj = this.lockObject;
			lock (obj)
			{
				string dashboard = SharedSettings.Current.Dashboard;
				try
				{
					SharedSettings.Current.Dashboard = text;
				}
				catch (Exception)
				{
				}
				if (SharedSettings.Current.Dashboard != text && !string.IsNullOrEmpty(dashboard))
				{
					SharedSettings.Current.Dashboard = dashboard;
				}
			}
			CarPlayManager instance = CarPlayManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnDashboardConfigurationUpdated();
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000AB61C File Offset: 0x000A981C
		public void ClearDashboard()
		{
			this.Pages.Clear();
			DashboardListViewModel._Current = null;
			SharedSettings.Current.Dashboard = "";
			this.SaveDashboardToSettings();
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x000AB644 File Offset: 0x000A9844
		private void ResetDashboardToDefault()
		{
			bool flag = false;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				App.OBDSimulator.Start(false);
				LiveDataPIDModel.GetSupportedPIDsTEST(App.OBDReader);
				App.OBDReader.SetStatusForTest(OBDDataReaderStatus.ConnectedToECU);
				flag = true;
			}
			this.Pages.Clear();
			DashboardPage dashboardPage = new Dash_1big2small();
			dashboardPage.CreateGrid();
			PID pid = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM) as PID;
			if (pid != null)
			{
				dashboardPage.Items[0].PID_Id = pid.Id;
			}
			else
			{
				dashboardPage.Items[0].PID_Id = PID.Empty.Id;
			}
			dashboardPage.Items[0].ItemType = DashboardItemTypes.Gauge;
			dashboardPage.Items[0].Minimum = 0.0;
			dashboardPage.Items[0].Maximum = 7000.0;
			dashboardPage.Items[0].GaugeShowRedLine = true;
			dashboardPage.Items[0].GaugeRedLineStart = 6000.0;
			dashboardPage.Items[0].GaugeRedLineFinish = 7000.0;
			pid = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed) as PID;
			if (pid != null)
			{
				dashboardPage.Items[1].PID_Id = pid.Id;
			}
			dashboardPage.Items[1].ItemType = DashboardItemTypes.Text;
			dashboardPage.Items[1].ValueFontSize = 2.0 * Device.GetNamedSize(4, typeof(Label));
			pid = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Coolant) as PID;
			if (pid != null)
			{
				dashboardPage.Items[2].PID_Id = pid.Id;
			}
			dashboardPage.Items[2].ItemType = DashboardItemTypes.Gauge;
			dashboardPage.Items[2].Minimum = -20.0;
			dashboardPage.Items[2].Maximum = 130.0;
			dashboardPage.Items[2].GaugeShowRedLine = true;
			dashboardPage.Items[2].GaugeRedLineStart = 105.0;
			dashboardPage.Items[2].GaugeRedLineFinish = 130.0;
			if (!SharedSettings.Current.Use_celcium)
			{
				dashboardPage.Items[2].Minimum = 0.0;
				dashboardPage.Items[2].Maximum = 260.0;
				dashboardPage.Items[2].GaugeRedLineStart = 230.0;
				dashboardPage.Items[2].GaugeRedLineFinish = 260.0;
			}
			dashboardPage.Title = Translate.GetString("ios_DashboardPage") + " 1";
			this.Pages.Add(dashboardPage);
			DashboardPage dashboardPage2 = new Dash_CustomPage();
			this.Pages.Add(dashboardPage2);
			CalculatedPIDV2[] array = (from x in LiveDataPIDModel._PIDCollection
				where x is CalculatedPIDV2
				select x as CalculatedPIDV2).ToArray<CalculatedPIDV2>();
			if (array.Any((CalculatedPIDV2 x) => x is PID_CalculatedInstantFuelRate))
			{
				int num = 0;
				List<PID> list = new List<PID>();
				PID pid2 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedInstantFuelRate);
				if (pid2 != null)
				{
					list.Add(pid2);
					num++;
				}
				PID pid3 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedAVGFuelConsumption);
				if (pid3 != null)
				{
					list.Add(pid3);
					num++;
				}
				PID pid4 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedInstantFuelConsumption);
				if (pid4 != null)
				{
					list.Add(pid4);
					num++;
				}
				PID pid5 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed) as PID;
				if (pid5 == null)
				{
					pid5 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.kmh);
				}
				if (pid5 != null)
				{
					list.Add(pid5);
					num++;
				}
				PID pid6 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedAvgSpeed);
				if (pid6 != null)
				{
					list.Add(pid6);
					num++;
				}
				PID pid7 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM) as PID;
				if (pid7 != null)
				{
					list.Add(pid7);
					num++;
				}
				PID pid8 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_TotalDistance);
				if (pid8 != null)
				{
					list.Add(pid8);
					num++;
				}
				PID pid9 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_TotalFuelUsed);
				if (pid8 != null)
				{
					list.Add(pid9);
					num++;
				}
				DashboardPage dashboardPage3;
				switch (num)
				{
				case 0:
					dashboardPage3 = null;
					goto IL_05CD;
				case 1:
				case 2:
				case 3:
					dashboardPage3 = new Dash_3vertical();
					goto IL_05CD;
				case 4:
					dashboardPage3 = new Dash_4table();
					goto IL_05CD;
				case 5:
					dashboardPage3 = new Dash_5_big_in_center();
					goto IL_05CD;
				case 6:
					dashboardPage3 = new Dash_6table();
					goto IL_05CD;
				case 7:
					dashboardPage3 = new Dash_3_1_3table();
					goto IL_05CD;
				}
				dashboardPage3 = new Dash_8table();
				IL_05CD:
				if (dashboardPage3 != null)
				{
					dashboardPage3.CreateGrid();
					dashboardPage3.Title = Translate.GetString("ios_FuelConsumption");
					if (dashboardPage3.Title != null)
					{
						dashboardPage3.Title = dashboardPage3.Title.Replace(": ", "").Replace(":", "");
					}
					for (int i = 0; i < num; i++)
					{
						dashboardPage3.Items[i].PID_Id = list[i].Id;
						dashboardPage3.Items[i].ItemType = DashboardItemTypes.Text;
						dashboardPage3.Items[i].ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0;
						Roles role = list[i].Role;
						if (role == Roles.RPM || role == Roles.Speed)
						{
							dashboardPage3.Items[i].ValueFormat = 1;
						}
						else
						{
							dashboardPage3.Items[i].ValueFormat = 5;
						}
					}
					if (num == 7)
					{
						dashboardPage3.Items[3].ItemType = DashboardItemTypes.Chart;
						dashboardPage3.Items[3].ValueFontSize = Device.GetNamedSize(4, typeof(Label));
					}
					if (num == 5)
					{
						dashboardPage3.Items[2].ItemType = DashboardItemTypes.Chart;
						dashboardPage3.Items[2].ValueFontSize = Device.GetNamedSize(4, typeof(Label));
					}
					this.Pages.Add(dashboardPage3);
				}
			}
			foreach (DashboardPage dashboardPage4 in this.Pages)
			{
				using (IEnumerator<DashboardItem> enumerator2 = dashboardPage4.Items.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						DashboardItem item = enumerator2.Current;
						if (Math.Abs(item.Minimum - item.Maximum) < 0.001)
						{
							PID pid10 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == item.PID_Id);
							if (pid10 != null)
							{
								item.Minimum = pid10.Minimum;
								item.Maximum = pid10.Maximum;
							}
						}
					}
				}
			}
			foreach (DashboardPage dashboardPage5 in this.Pages)
			{
				foreach (DashboardItem dashboardItem in dashboardPage5.Items)
				{
					DashboardItem.ApplyThemeToItem(dashboardItem);
					dashboardItem.SelectAndAddControl();
				}
			}
			if (this.Pages.Count == 3)
			{
				foreach (DashboardItem dashboardItem2 in this.Pages[2].Items)
				{
					dashboardItem2.CornerRadius = 12.0;
					dashboardItem2.FrameSize = 2.0;
					dashboardItem2.FrameColor = Color.Silver;
				}
			}
			this.SaveDashboardToSettings();
			if (flag)
			{
				App.OBDReader.SetStatusForTest(OBDDataReaderStatus.Disconnected);
				App.OBDSimulator.Stop();
				LiveDataPIDModel._PIDCollection.Clear();
			}
		}

		// Token: 0x040009F4 RID: 2548
		private static DashboardListViewModel _Current;

		// Token: 0x040009F5 RID: 2549
		[CompilerGenerated]
		private bool <IsReady>k__BackingField;

		// Token: 0x040009F6 RID: 2550
		private ObservableCollection<DashboardPage> _Pages;

		// Token: 0x040009F7 RID: 2551
		private object lockObject = new object();

		// Token: 0x020001B2 RID: 434
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600170F RID: 5903 RVA: 0x000ABFD8 File Offset: 0x000AA1D8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001710 RID: 5904 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001711 RID: 5905 RVA: 0x000ABFE4 File Offset: 0x000AA1E4
			internal bool <ResetDashboardToDefault>b__18_0(PID x)
			{
				return x is CalculatedPIDV2;
			}

			// Token: 0x06001712 RID: 5906 RVA: 0x000ABFEF File Offset: 0x000AA1EF
			internal CalculatedPIDV2 <ResetDashboardToDefault>b__18_1(PID x)
			{
				return x as CalculatedPIDV2;
			}

			// Token: 0x06001713 RID: 5907 RVA: 0x000ABFF7 File Offset: 0x000AA1F7
			internal bool <ResetDashboardToDefault>b__18_2(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedInstantFuelRate;
			}

			// Token: 0x06001714 RID: 5908 RVA: 0x000ABFF7 File Offset: 0x000AA1F7
			internal bool <ResetDashboardToDefault>b__18_3(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedInstantFuelRate;
			}

			// Token: 0x06001715 RID: 5909 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <ResetDashboardToDefault>b__18_4(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x06001716 RID: 5910 RVA: 0x000AC00D File Offset: 0x000AA20D
			internal bool <ResetDashboardToDefault>b__18_5(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedInstantFuelConsumption;
			}

			// Token: 0x06001717 RID: 5911 RVA: 0x000AC018 File Offset: 0x000AA218
			internal bool <ResetDashboardToDefault>b__18_6(PID x)
			{
				return x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.kmh;
			}

			// Token: 0x06001718 RID: 5912 RVA: 0x000AC032 File Offset: 0x000AA232
			internal bool <ResetDashboardToDefault>b__18_7(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedAvgSpeed;
			}

			// Token: 0x06001719 RID: 5913 RVA: 0x000AC03D File Offset: 0x000AA23D
			internal bool <ResetDashboardToDefault>b__18_8(CalculatedPIDV2 x)
			{
				return x is PID_TotalDistance;
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x000AC048 File Offset: 0x000AA248
			internal bool <ResetDashboardToDefault>b__18_9(CalculatedPIDV2 x)
			{
				return x is PID_TotalFuelUsed;
			}

			// Token: 0x040009F8 RID: 2552
			public static readonly DashboardListViewModel.<>c <>9 = new DashboardListViewModel.<>c();

			// Token: 0x040009F9 RID: 2553
			public static Func<PID, bool> <>9__18_0;

			// Token: 0x040009FA RID: 2554
			public static Func<PID, CalculatedPIDV2> <>9__18_1;

			// Token: 0x040009FB RID: 2555
			public static Func<CalculatedPIDV2, bool> <>9__18_2;

			// Token: 0x040009FC RID: 2556
			public static Func<CalculatedPIDV2, bool> <>9__18_3;

			// Token: 0x040009FD RID: 2557
			public static Func<CalculatedPIDV2, bool> <>9__18_4;

			// Token: 0x040009FE RID: 2558
			public static Func<CalculatedPIDV2, bool> <>9__18_5;

			// Token: 0x040009FF RID: 2559
			public static Func<PID, bool> <>9__18_6;

			// Token: 0x04000A00 RID: 2560
			public static Func<CalculatedPIDV2, bool> <>9__18_7;

			// Token: 0x04000A01 RID: 2561
			public static Func<CalculatedPIDV2, bool> <>9__18_8;

			// Token: 0x04000A02 RID: 2562
			public static Func<CalculatedPIDV2, bool> <>9__18_9;
		}

		// Token: 0x020001B3 RID: 435
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600171B RID: 5915 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x000AC054 File Offset: 0x000AA254
			internal async Task <LoadDashboardFromSettings>b__0()
			{
				bool flag = await this.curPage.DisplayAlert(Translate.GetString("ios_MainPage_TileDashboard") + ": " + Translate.GetString("dash_LoadingErrorTitle"), string.Concat(new string[]
				{
					Translate.GetString("dash_loadingErrorText"),
					"\n\n",
					Translate.GetString("ios_MainPage_Settings"),
					" -> ",
					Translate.GetString("ios_MainPage_TileDashboard"),
					" -> ",
					Translate.GetString("Settings_Control_btnDashReset.Content")
				}), "OK", Translate.GetString("btnCancel.Content"));
				this.retry = flag;
				this.semaphore.Release();
			}

			// Token: 0x04000A03 RID: 2563
			public Page curPage;

			// Token: 0x04000A04 RID: 2564
			public bool retry;

			// Token: 0x04000A05 RID: 2565
			public SemaphoreSlim semaphore;

			// Token: 0x020001B4 RID: 436
			[StructLayout(LayoutKind.Auto)]
			private struct <<LoadDashboardFromSettings>b__0>d : IAsyncStateMachine
			{
				// Token: 0x0600171D RID: 5917 RVA: 0x000AC098 File Offset: 0x000AA298
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					DashboardListViewModel.<>c__DisplayClass12_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = CS$<>8__locals1.curPage.DisplayAlert(Translate.GetString("ios_MainPage_TileDashboard") + ": " + Translate.GetString("dash_LoadingErrorTitle"), string.Concat(new string[]
							{
								Translate.GetString("dash_loadingErrorText"),
								"\n\n",
								Translate.GetString("ios_MainPage_Settings"),
								" -> ",
								Translate.GetString("ios_MainPage_TileDashboard"),
								" -> ",
								Translate.GetString("Settings_Control_btnDashReset.Content")
							}), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardListViewModel.<>c__DisplayClass12_0.<<LoadDashboardFromSettings>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.retry = result;
						CS$<>8__locals1.semaphore.Release();
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

				// Token: 0x0600171E RID: 5918 RVA: 0x000AC1EC File Offset: 0x000AA3EC
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000A06 RID: 2566
				public int <>1__state;

				// Token: 0x04000A07 RID: 2567
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000A08 RID: 2568
				public DashboardListViewModel.<>c__DisplayClass12_0 <>4__this;

				// Token: 0x04000A09 RID: 2569
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x020001B5 RID: 437
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x0600171F RID: 5919 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06001720 RID: 5920 RVA: 0x000AC1FA File Offset: 0x000AA3FA
			internal bool <ResetDashboardToDefault>b__10(PID x)
			{
				return x.Id == this.item.PID_Id;
			}

			// Token: 0x04000A0A RID: 2570
			public DashboardItem item;
		}
	}
}
