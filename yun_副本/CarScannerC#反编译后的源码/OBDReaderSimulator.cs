using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms
{
	// Token: 0x020000A2 RID: 162
	public class OBDReaderSimulator
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0001DAD8 File Offset: 0x0001BCD8
		public static OBDReaderSimulator Current
		{
			get
			{
				if (OBDReaderSimulator._Current == null)
				{
					OBDReaderSimulator._Current = new OBDReaderSimulator();
				}
				return OBDReaderSimulator._Current;
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0001DAF0 File Offset: 0x0001BCF0
		public static void Reload()
		{
			OBDReaderSimulator.Current.Stop();
			OBDReaderSimulator._Current = null;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0001DB02 File Offset: 0x0001BD02
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0001DB0A File Offset: 0x0001BD0A
		public bool IsActive
		{
			[CompilerGenerated]
			get
			{
				return this.<IsActive>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsActive>k__BackingField = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0001DB13 File Offset: 0x0001BD13
		private OBDDataReader reader
		{
			get
			{
				return App.OBDReader;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0001DB1A File Offset: 0x0001BD1A
		private OBDReaderSimulator()
		{
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0001DB34 File Offset: 0x0001BD34
		public void CreateFreezeFrame()
		{
			App.OBDReader.CurrentCarData.FreezeFrameNumber.ToString("X2", CultureInfo.InvariantCulture);
			Random random = new Random();
			byte[] array = new byte[80];
			if (PlatformHelper.IsAndroid)
			{
				random.NextBytes(array);
			}
			foreach (PID pid in App.OBDReader.CurrentCarData.Mode02PIDs)
			{
				try
				{
					OBDReaderSimulator.RandomizePid(random, pid, array);
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0001DBE4 File Offset: 0x0001BDE4
		private static void RandomizePidV2(PID pid, byte[] data)
		{
			if (pid is IPIDFloatValue)
			{
				IPIDFloatValue ipidfloatValue = (IPIDFloatValue)pid;
				double num = ipidfloatValue.Maximum;
				double num2 = ipidfloatValue.Minimum;
				if (num2 > num)
				{
					double num3 = num2;
					num2 = num;
					num = num3;
				}
				ipidfloatValue.SetValue((double)OBDReaderSimulator.rnd.Next((int)num2, (int)num + 1));
				return;
			}
			byte[] array;
			if (pid.Command == "0902")
			{
				array = Encoding.UTF8.GetBytes(" 2C4GJ453XYR693123");
			}
			else if (pid.Command == "0904")
			{
				array = Encoding.UTF8.GetBytes(" 012034A");
			}
			else if (pid.Command == "090A")
			{
				array = Encoding.UTF8.GetBytes(" Engine.Control");
			}
			else
			{
				array = data;
			}
			pid.Decode(array, App.OBDReader.stopwatch.Elapsed, "7E8");
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0001DCB8 File Offset: 0x0001BEB8
		private static void RandomizePid(Random rnd, PID pid, byte[] data)
		{
			byte[] array;
			if (pid.Command == "0902")
			{
				array = Encoding.UTF8.GetBytes(" 2C4GJ453XYR693123");
			}
			else if (pid.Command == "0904")
			{
				array = Encoding.UTF8.GetBytes(" 012034A");
			}
			else if (pid.Command == "090A")
			{
				array = Encoding.UTF8.GetBytes(" Engine.Control");
			}
			else if (PlatformHelper.IsiOS)
			{
				if (pid is CustomPID)
				{
					array = new byte[64];
				}
				else
				{
					array = new byte[24];
				}
				rnd.NextBytes(array);
			}
			else
			{
				array = data;
			}
			pid.Decode(array, App.OBDReader.stopwatch.Elapsed, "7E8");
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0001DD78 File Offset: 0x0001BF78
		public async Task Start(bool loadSavedSensors)
		{
			this.PIDsChanging.Clear();
			if (loadSavedSensors)
			{
				foreach (PID pid in this.reader.CurrentCarData.LiveDataPIDs)
				{
					pid.IsAvailable = false;
				}
				try
				{
					string[] array = SharedSettings.Current.LastCarAvailableSensors.Split(new char[] { ';' });
					for (int i = 0; i < array.Length; i++)
					{
						int id = 0;
						int.TryParse(array[i], out id);
						PID pid2 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == id);
						if (pid2 != null)
						{
							this.PIDsChanging.Add(pid2);
							pid2.IsAvailable = true;
						}
					}
					goto IL_01A6;
				}
				catch
				{
					this.Start(false);
					return;
				}
			}
			if (SharedSettings.Current.UseOBD2)
			{
				foreach (PID pid3 in this.reader.CurrentCarData.LiveDataPIDs)
				{
					if (!(pid3 is PID014F_MaxValues) && !(pid3 is PID0150_MAFMaxValues) && !(pid3 is PID_SupportedPids) && !(pid3.Command == "0908") && !(pid3.Command == "090B"))
					{
						pid3.IsAvailable = true;
						this.PIDsChanging.Add(pid3);
					}
				}
			}
			IL_01A6:
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentProfile.PidCollection)
			{
				if (customPID.IsAvailable)
				{
					this.PIDsChanging.Add(customPID);
				}
			}
			foreach (CustomPID customPID2 in CustomPIDViewModel.CurrentCustom.PidCollection)
			{
				customPID2.IsAvailable = true;
				if (customPID2.IsAvailable)
				{
					this.PIDsChanging.Add(customPID2);
				}
			}
			App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
			this.IsActive = true;
			App.OBDReader.CurrentELMFormat = ELMFormat.CAN11bit;
			new Thread(async delegate
			{
				await this.Worker();
			})
			{
				Priority = ThreadPriority.Lowest
			}.Start();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001DDC4 File Offset: 0x0001BFC4
		private async Task Worker()
		{
			while (this.IsActive)
			{
				try
				{
					List<PID> list = new List<PID>(this.PIDsChanging);
					byte[] array = new byte[80];
					OBDReaderSimulator.rnd.NextBytes(array);
					foreach (PID pid in list)
					{
						OBDReaderSimulator.RandomizePidV2(pid, array);
						if (!this.IsActive)
						{
							break;
						}
					}
				}
				catch (Exception)
				{
				}
				if (PlatformHelper.IsAndroid)
				{
					await Task.Delay(600);
				}
				else
				{
					await Task.Delay(600);
				}
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0001DE07 File Offset: 0x0001C007
		public void Stop()
		{
			this.IsActive = false;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0001DE10 File Offset: 0x0001C010
		// Note: this type is marked as 'beforefieldinit'.
		static OBDReaderSimulator()
		{
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001DE34 File Offset: 0x0001C034
		[CompilerGenerated]
		private async void <Start>b__17_0()
		{
			await this.Worker();
		}

		// Token: 0x0400020F RID: 527
		private static Random rnd = new Random(4623870);

		// Token: 0x04000210 RID: 528
		[CompilerGenerated]
		private bool <IsActive>k__BackingField;

		// Token: 0x04000211 RID: 529
		private static OBDReaderSimulator _Current = null;

		// Token: 0x04000212 RID: 530
		private List<PID> PIDsChanging = new List<PID>(300);

		// Token: 0x04000213 RID: 531
		private static byte temp_b = 200;

		// Token: 0x020000A3 RID: 163
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<Start>b__17_0>d : IAsyncStateMachine
		{
			// Token: 0x06000342 RID: 834 RVA: 0x0001DE6C File Offset: 0x0001C06C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDReaderSimulator obdreaderSimulator = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = obdreaderSimulator.Worker().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDReaderSimulator.<<Start>b__17_0>d>(ref taskAwaiter, ref this);
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

			// Token: 0x06000343 RID: 835 RVA: 0x0001DF20 File Offset: 0x0001C120
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000214 RID: 532
			public int <>1__state;

			// Token: 0x04000215 RID: 533
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000216 RID: 534
			public OBDReaderSimulator <>4__this;

			// Token: 0x04000217 RID: 535
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000A4 RID: 164
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x06000344 RID: 836 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x06000345 RID: 837 RVA: 0x0001DF2E File Offset: 0x0001C12E
			internal bool <Start>b__1(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x04000218 RID: 536
			public int id;
		}

		// Token: 0x020000A5 RID: 165
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__17 : IAsyncStateMachine
		{
			// Token: 0x06000346 RID: 838 RVA: 0x0001DF40 File Offset: 0x0001C140
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				OBDReaderSimulator obdreaderSimulator = this;
				try
				{
					obdreaderSimulator.PIDsChanging.Clear();
					if (loadSavedSensors)
					{
						List<PID>.Enumerator enumerator = obdreaderSimulator.reader.CurrentCarData.LiveDataPIDs.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PID pid = enumerator.Current;
								pid.IsAvailable = false;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						try
						{
							string[] array = SharedSettings.Current.LastCarAvailableSensors.Split(new char[] { ';' });
							for (int i = 0; i < array.Length; i++)
							{
								OBDReaderSimulator.<>c__DisplayClass17_0 CS$<>8__locals1 = new OBDReaderSimulator.<>c__DisplayClass17_0();
								CS$<>8__locals1.id = 0;
								int.TryParse(array[i], out CS$<>8__locals1.id);
								PID pid2 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == CS$<>8__locals1.id);
								if (pid2 != null)
								{
									obdreaderSimulator.PIDsChanging.Add(pid2);
									pid2.IsAvailable = true;
								}
							}
							goto IL_01A6;
						}
						catch
						{
							obdreaderSimulator.Start(false);
							goto IL_02A1;
						}
					}
					if (SharedSettings.Current.UseOBD2)
					{
						List<PID>.Enumerator enumerator = obdreaderSimulator.reader.CurrentCarData.LiveDataPIDs.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PID pid3 = enumerator.Current;
								if (!(pid3 is PID014F_MaxValues) && !(pid3 is PID0150_MAFMaxValues) && !(pid3 is PID_SupportedPids) && !(pid3.Command == "0908") && !(pid3.Command == "090B"))
								{
									pid3.IsAvailable = true;
									obdreaderSimulator.PIDsChanging.Add(pid3);
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
					}
					IL_01A6:
					IEnumerator<CustomPID> enumerator2 = CustomPIDViewModel.CurrentProfile.PidCollection.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							CustomPID customPID = enumerator2.Current;
							if (customPID.IsAvailable)
							{
								obdreaderSimulator.PIDsChanging.Add(customPID);
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
					enumerator2 = CustomPIDViewModel.CurrentCustom.PidCollection.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							CustomPID customPID2 = enumerator2.Current;
							customPID2.IsAvailable = true;
							if (customPID2.IsAvailable)
							{
								obdreaderSimulator.PIDsChanging.Add(customPID2);
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
					App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
					obdreaderSimulator.IsActive = true;
					App.OBDReader.CurrentELMFormat = ELMFormat.CAN11bit;
					new Thread(delegate
					{
						OBDReaderSimulator.<<Start>b__17_0>d <<Start>b__17_0>d;
						<<Start>b__17_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<Start>b__17_0>d.<>4__this = obdreaderSimulator;
						<<Start>b__17_0>d.<>1__state = -1;
						<<Start>b__17_0>d.<>t__builder.Start<OBDReaderSimulator.<<Start>b__17_0>d>(ref <<Start>b__17_0>d);
					})
					{
						Priority = ThreadPriority.Lowest
					}.Start();
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02A1:
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000347 RID: 839 RVA: 0x0001E298 File Offset: 0x0001C498
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000219 RID: 537
			public int <>1__state;

			// Token: 0x0400021A RID: 538
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400021B RID: 539
			public OBDReaderSimulator <>4__this;

			// Token: 0x0400021C RID: 540
			public bool loadSavedSensors;
		}

		// Token: 0x020000A6 RID: 166
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Worker>d__18 : IAsyncStateMachine
		{
			// Token: 0x06000348 RID: 840 RVA: 0x0001E2A8 File Offset: 0x0001C4A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDReaderSimulator obdreaderSimulator = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num != 1)
						{
							goto IL_0148;
						}
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0141;
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					IL_00E0:
					taskAwaiter.GetResult();
					goto IL_0148;
					IL_0141:
					taskAwaiter.GetResult();
					IL_0148:
					if (obdreaderSimulator.IsActive)
					{
						try
						{
							List<PID> list = new List<PID>(obdreaderSimulator.PIDsChanging);
							byte[] array = new byte[80];
							OBDReaderSimulator.rnd.NextBytes(array);
							List<PID>.Enumerator enumerator = list.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									PID pid = enumerator.Current;
									OBDReaderSimulator.RandomizePidV2(pid, array);
									if (!obdreaderSimulator.IsActive)
									{
										break;
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
						}
						catch (Exception)
						{
						}
						if (PlatformHelper.IsAndroid)
						{
							taskAwaiter = Task.Delay(600).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDReaderSimulator.<Worker>d__18>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00E0;
						}
						else
						{
							taskAwaiter = Task.Delay(600).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDReaderSimulator.<Worker>d__18>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0141;
						}
					}
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

			// Token: 0x06000349 RID: 841 RVA: 0x0001E484 File Offset: 0x0001C684
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400021D RID: 541
			public int <>1__state;

			// Token: 0x0400021E RID: 542
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400021F RID: 543
			public OBDReaderSimulator <>4__this;

			// Token: 0x04000220 RID: 544
			private TaskAwaiter <>u__1;
		}
	}
}
