using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000666 RID: 1638
	public class RootNavigationPage : ContentPage
	{
		// Token: 0x06003850 RID: 14416 RVA: 0x002A9180 File Offset: 0x002A7380
		public RootNavigationPage()
		{
			base.Content = new StackLayout
			{
				Children = 
				{
					new Label
					{
						Text = Translate.GetString("general_Loading"),
						HorizontalOptions = LayoutOptions.Center,
						VerticalOptions = LayoutOptions.Center,
						HorizontalTextAlignment = 1
					}
				}
			};
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x002A91DC File Offset: 0x002A73DC
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await this._ChangeRootPage(this.RealRootPage);
			while (this._changePageEntered)
			{
				await Task.Delay(1000);
			}
			if (base.Navigation.NavigationStack.Count > 0 && base.Navigation.NavigationStack.LastOrDefault<Page>() == this)
			{
				await this._ChangeRootPage(this.RealRootPage);
			}
		}

		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x002A9213 File Offset: 0x002A7413
		// (set) Token: 0x06003853 RID: 14419 RVA: 0x002A921B File Offset: 0x002A741B
		public ContentPage RealRootPage
		{
			[CompilerGenerated]
			get
			{
				return this.<RealRootPage>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RealRootPage>k__BackingField = value;
			}
		}

		// Token: 0x06003854 RID: 14420 RVA: 0x002A9224 File Offset: 0x002A7424
		public static void ChangeRootPage(ContentPage page)
		{
			App.Instance.RootPage.RealRootPage = page;
			App.Instance.RootPage.Navigation.PopToRootAsync();
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x002A924C File Offset: 0x002A744C
		private async Task _ChangeRootPage(ContentPage page)
		{
			if (page != null)
			{
				if (!this._changePageEntered)
				{
					this._changePageEntered = true;
					while (!base.Navigation.NavigationStack.Contains(page))
					{
						await Task.Delay(250);
						await base.Navigation.PushAsync(page, false);
						await Task.Delay(250);
					}
					this._changePageEntered = false;
				}
			}
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x002A9297 File Offset: 0x002A7497
		[CompilerGenerated]
		[DebuggerHidden]
		private void <>n__0()
		{
			base.OnAppearing();
		}

		// Token: 0x04002239 RID: 8761
		private volatile bool _changePageEntered;

		// Token: 0x0400223A RID: 8762
		[CompilerGenerated]
		private ContentPage <RealRootPage>k__BackingField;

		// Token: 0x02000667 RID: 1639
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnAppearing>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003857 RID: 14423 RVA: 0x002A92A0 File Offset: 0x002A74A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RootNavigationPage rootNavigationPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_00E1;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_016F;
					}
					default:
						rootNavigationPage.<>n__0();
						taskAwaiter = rootNavigationPage._ChangeRootPage(rootNavigationPage.RealRootPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<OnAppearing>d__1>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					goto IL_00E8;
					IL_00E1:
					taskAwaiter.GetResult();
					IL_00E8:
					if (!rootNavigationPage._changePageEntered)
					{
						if (rootNavigationPage.Navigation.NavigationStack.Count <= 0 || rootNavigationPage.Navigation.NavigationStack.LastOrDefault<Page>() != rootNavigationPage)
						{
							goto IL_0176;
						}
						taskAwaiter = rootNavigationPage._ChangeRootPage(rootNavigationPage.RealRootPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<OnAppearing>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<OnAppearing>d__1>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_00E1;
					}
					IL_016F:
					taskAwaiter.GetResult();
					IL_0176:;
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

			// Token: 0x06003858 RID: 14424 RVA: 0x002A946C File Offset: 0x002A766C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400223B RID: 8763
			public int <>1__state;

			// Token: 0x0400223C RID: 8764
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400223D RID: 8765
			public RootNavigationPage <>4__this;

			// Token: 0x0400223E RID: 8766
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000668 RID: 1640
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <_ChangeRootPage>d__8 : IAsyncStateMachine
		{
			// Token: 0x06003859 RID: 14425 RVA: 0x002A947C File Offset: 0x002A767C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RootNavigationPage rootNavigationPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0109;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0165;
					}
					default:
						if (page == null)
						{
							goto IL_01A9;
						}
						if (rootNavigationPage._changePageEntered)
						{
							goto IL_01A9;
						}
						rootNavigationPage._changePageEntered = true;
						goto IL_016C;
					}
					IL_00A2:
					taskAwaiter.GetResult();
					taskAwaiter = rootNavigationPage.Navigation.PushAsync(page, false).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<_ChangeRootPage>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_0109:
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(250).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<_ChangeRootPage>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_0165:
					taskAwaiter.GetResult();
					IL_016C:
					if (rootNavigationPage.Navigation.NavigationStack.Contains(page))
					{
						rootNavigationPage._changePageEntered = false;
					}
					else
					{
						taskAwaiter = Task.Delay(250).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RootNavigationPage.<_ChangeRootPage>d__8>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_00A2;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A9:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600385A RID: 14426 RVA: 0x002A9664 File Offset: 0x002A7864
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400223F RID: 8767
			public int <>1__state;

			// Token: 0x04002240 RID: 8768
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002241 RID: 8769
			public ContentPage page;

			// Token: 0x04002242 RID: 8770
			public RootNavigationPage <>4__this;

			// Token: 0x04002243 RID: 8771
			private TaskAwaiter <>u__1;
		}
	}
}
