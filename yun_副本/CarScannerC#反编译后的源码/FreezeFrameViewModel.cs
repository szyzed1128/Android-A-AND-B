using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms
{
	// Token: 0x020001B6 RID: 438
	public class FreezeFrameViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06001721 RID: 5921 RVA: 0x000AC210 File Offset: 0x000AA410
		// (remove) Token: 0x06001722 RID: 5922 RVA: 0x000AC248 File Offset: 0x000AA448
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

		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06001723 RID: 5923 RVA: 0x000AC27D File Offset: 0x000AA47D
		public ObservableCollection<PIDAdapter> FFPIDS
		{
			get
			{
				return this._FFPIDS;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x000AC285 File Offset: 0x000AA485
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x000AC28D File Offset: 0x000AA48D
		public int FreezeFrameNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<FreezeFrameNumber>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FreezeFrameNumber>k__BackingField = value;
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x000AC296 File Offset: 0x000AA496
		public FreezeFrameViewModel(OBDDataReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x000AC2B0 File Offset: 0x000AA4B0
		private void UpdateFromCarData()
		{
			IEnumerable<PID> enumerable = this.reader.CurrentCarData.Mode02PIDs.Where((PID x) => x.IsAvailable && !(x is PID_SupportedPids));
			this.FFPIDS.Clear();
			foreach (PID pid in enumerable)
			{
				PIDAdapter pidadapter = new PIDAdapter(pid);
				this.FFPIDS.Add(pidadapter);
			}
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x000AC340 File Offset: 0x000AA540
		private string FreezeFrameReport()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PIDAdapter pidadapter in this._FFPIDS)
			{
				stringBuilder.Append(pidadapter.Name);
				stringBuilder.Append(": ");
				stringBuilder.Append(pidadapter.Value);
				stringBuilder.Append(" ");
				stringBuilder.Append(pidadapter.Units);
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000AC3D8 File Offset: 0x000AA5D8
		private bool HasFreezeFrameData()
		{
			IEnumerable<PID> enumerable = this.reader.CurrentCarData.Mode02PIDs.Where((PID x) => x.IsAvailable && !(x is PID_SupportedPids));
			return enumerable != null && enumerable.Count<PID>() > 0;
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x000AC42C File Offset: 0x000AA62C
		public async Task<string> GetFreezeFrame()
		{
			this.FFPIDS.Clear();
			if (OBDReaderSimulator.Current.IsActive)
			{
				this.reader.CurrentCarData.CreateMode02PIDs();
				OBDReaderSimulator.Current.CreateFreezeFrame();
			}
			else
			{
				await this.reader.ReadFreezeFrame(this.FreezeFrameNumber);
			}
			string text;
			if (!this.HasFreezeFrameData())
			{
				text = "NO FREEZE FRAME DATA";
			}
			else
			{
				this.UpdateFromCarData();
				text = this.FreezeFrameReport();
			}
			return text;
		}

		// Token: 0x04000A0B RID: 2571
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04000A0C RID: 2572
		private ObservableCollection<PIDAdapter> _FFPIDS = new ObservableCollection<PIDAdapter>();

		// Token: 0x04000A0D RID: 2573
		private OBDDataReader reader;

		// Token: 0x04000A0E RID: 2574
		[CompilerGenerated]
		private int <FreezeFrameNumber>k__BackingField;

		// Token: 0x020001B7 RID: 439
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600172B RID: 5931 RVA: 0x000AC46F File Offset: 0x000AA66F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600172C RID: 5932 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600172D RID: 5933 RVA: 0x00056949 File Offset: 0x00054B49
			internal bool <UpdateFromCarData>b__12_0(PID x)
			{
				return x.IsAvailable && !(x is PID_SupportedPids);
			}

			// Token: 0x0600172E RID: 5934 RVA: 0x00056949 File Offset: 0x00054B49
			internal bool <HasFreezeFrameData>b__14_0(PID x)
			{
				return x.IsAvailable && !(x is PID_SupportedPids);
			}

			// Token: 0x04000A0F RID: 2575
			public static readonly FreezeFrameViewModel.<>c <>9 = new FreezeFrameViewModel.<>c();

			// Token: 0x04000A10 RID: 2576
			public static Func<PID, bool> <>9__12_0;

			// Token: 0x04000A11 RID: 2577
			public static Func<PID, bool> <>9__14_0;
		}

		// Token: 0x020001B8 RID: 440
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetFreezeFrame>d__15 : IAsyncStateMachine
		{
			// Token: 0x0600172F RID: 5935 RVA: 0x000AC47C File Offset: 0x000AA67C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FreezeFrameViewModel freezeFrameViewModel = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						freezeFrameViewModel.FFPIDS.Clear();
						if (OBDReaderSimulator.Current.IsActive)
						{
							freezeFrameViewModel.reader.CurrentCarData.CreateMode02PIDs();
							OBDReaderSimulator.Current.CreateFreezeFrame();
							goto IL_00A7;
						}
						taskAwaiter = freezeFrameViewModel.reader.ReadFreezeFrame(freezeFrameViewModel.FreezeFrameNumber).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FreezeFrameViewModel.<GetFreezeFrame>d__15>(ref taskAwaiter, ref this);
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
					IL_00A7:
					if (!freezeFrameViewModel.HasFreezeFrameData())
					{
						text = "NO FREEZE FRAME DATA";
					}
					else
					{
						freezeFrameViewModel.UpdateFromCarData();
						text = freezeFrameViewModel.FreezeFrameReport();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06001730 RID: 5936 RVA: 0x000AC58C File Offset: 0x000AA78C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A12 RID: 2578
			public int <>1__state;

			// Token: 0x04000A13 RID: 2579
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04000A14 RID: 2580
			public FreezeFrameViewModel <>4__this;

			// Token: 0x04000A15 RID: 2581
			private TaskAwaiter <>u__1;
		}
	}
}
