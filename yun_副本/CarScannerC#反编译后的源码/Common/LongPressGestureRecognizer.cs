using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007EF RID: 2031
	internal class LongPressGestureRecognizer : PanGestureRecognizer
	{
		// Token: 0x1400004D RID: 77
		// (add) Token: 0x06004727 RID: 18215 RVA: 0x0036D104 File Offset: 0x0036B304
		// (remove) Token: 0x06004728 RID: 18216 RVA: 0x0036D13C File Offset: 0x0036B33C
		public event EventHandler LongPressed
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.LongPressed;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.LongPressed, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.LongPressed;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.LongPressed, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06004729 RID: 18217 RVA: 0x0036D171 File Offset: 0x0036B371
		// (set) Token: 0x0600472A RID: 18218 RVA: 0x0036D179 File Offset: 0x0036B379
		public long LongPressDuration
		{
			[CompilerGenerated]
			get
			{
				return this.<LongPressDuration>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LongPressDuration>k__BackingField = value;
			}
		} = 1000L;

		// Token: 0x0600472B RID: 18219 RVA: 0x0036D182 File Offset: 0x0036B382
		public LongPressGestureRecognizer()
		{
			base.PanUpdated += this.LongPressGestureRecognizer_PanUpdated;
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0036D1BC File Offset: 0x0036B3BC
		private void LongPressGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
		{
			if (e.StatusType == null)
			{
				this.sw.Restart();
				this.gestureId = e.GestureId;
				return;
			}
			if (e.GestureId == this.gestureId && this.sw.IsRunning && e.StatusType == 1)
			{
				if (this.sw.ElapsedMilliseconds > this.LongPressDuration)
				{
					this.sw.Stop();
					this.gestureId = -1;
					EventHandler longPressed = this.LongPressed;
					if (longPressed == null)
					{
						return;
					}
					longPressed(sender, EventArgs.Empty);
					return;
				}
			}
			else
			{
				this.sw.Stop();
				this.gestureId = -1;
			}
		}

		// Token: 0x04002977 RID: 10615
		[CompilerGenerated]
		private EventHandler LongPressed;

		// Token: 0x04002978 RID: 10616
		[CompilerGenerated]
		private long <LongPressDuration>k__BackingField;

		// Token: 0x04002979 RID: 10617
		private Stopwatch sw = new Stopwatch();

		// Token: 0x0400297A RID: 10618
		private int gestureId = -1;
	}
}
