using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x0200000A RID: 10
	public abstract class Dialog : Grid
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000027C3 File Offset: 0x000009C3
		// (set) Token: 0x0600002B RID: 43 RVA: 0x000027CB File Offset: 0x000009CB
		protected DialogSettings Settings
		{
			[CompilerGenerated]
			get
			{
				return this.<Settings>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Settings>k__BackingField = value;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void OnShow()
		{
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000027D6 File Offset: 0x000009D6
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000027DE File Offset: 0x000009DE
		protected Layout<View> MainConteiner
		{
			[CompilerGenerated]
			get
			{
				return this.<MainConteiner>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MainConteiner>k__BackingField = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000027E7 File Offset: 0x000009E7
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000027EF File Offset: 0x000009EF
		protected Frame MainFrame
		{
			[CompilerGenerated]
			get
			{
				return this.<MainFrame>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MainFrame>k__BackingField = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027F8 File Offset: 0x000009F8
		// (set) Token: 0x06000032 RID: 50 RVA: 0x00002800 File Offset: 0x00000A00
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x06000033 RID: 51
		protected abstract Task<View> BuildContent();

		// Token: 0x06000034 RID: 52 RVA: 0x0000280C File Offset: 0x00000A0C
		protected async Task<bool> ShowDialog(Layout<View> parent)
		{
			if (this.Settings == null)
			{
				this.Settings = new DialogSettings();
			}
			base.Parent = null;
			base.MinimumWidthRequest = parent.Width;
			Grid grid = parent as Grid;
			if (grid != null)
			{
				if (grid.RowDefinitions.Count > 1)
				{
					Grid.SetRowSpan(this, grid.RowDefinitions.Count);
				}
				if (grid.ColumnDefinitions.Count > 1)
				{
					Grid.SetColumnSpan(this, grid.ColumnDefinitions.Count);
				}
			}
			uint animLength = 400U;
			if (this.Settings.DialogAnimation)
			{
				base.Opacity = 0.0;
				this.MainFrame.Scale = 0.75;
				parent.Children.Add(this);
				ViewExtensions.FadeTo(this, 1.0, animLength, Easing.SinOut);
				await ViewExtensions.ScaleTo(this.MainFrame, 1.0, animLength, Easing.SinOut);
			}
			else
			{
				parent.Children.Add(this);
			}
			string result = await this.WaitForClick();
			if (this.Settings.DialogAnimation)
			{
				ViewExtensions.FadeTo(this, 0.0, animLength, Easing.SinIn);
				await ViewExtensions.ScaleTo(this.MainFrame, 0.75, animLength, Easing.SinIn);
			}
			parent.Children.Remove(this);
			string text = result;
			Button button = this.btnOk;
			return text == ((button != null) ? button.Text : null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002858 File Offset: 0x00000A58
		protected async Task Initialize()
		{
			base.Children.Clear();
			base.HorizontalOptions = LayoutOptions.Fill;
			base.VerticalOptions = LayoutOptions.Fill;
			base.BackgroundColor = this.Settings.BackgroundColor;
			base.Margin = new Thickness(-20.0);
			base.Padding = new Thickness(20.0);
			this.MainFrame = new Frame
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = this.Settings.DialogColor,
				Padding = new Thickness(16.0),
				HasShadow = true
			};
			TargetIdiom idiom = Device.Idiom;
			if (idiom - 2 <= 1)
			{
				this.MainFrame.WidthRequest = 300.0;
			}
			base.Children.Add(this.MainFrame);
			this.MainConteiner = new StackLayout
			{
				Orientation = 0
			};
			this.MainFrame.Content = this.MainConteiner;
			if (!string.IsNullOrEmpty(this.Title))
			{
				this.stlTitle = new StackLayout
				{
					Orientation = 1,
					Margin = new Thickness(0.0, 0.0, 0.0, 10.0),
					HorizontalOptions = LayoutOptions.Fill
				};
				this.MainConteiner.Children.Add(this.stlTitle);
				this.lblTitle = new Label();
				this.lblTitle.FontSize *= 1.5;
				this.lblTitle.VerticalOptions = LayoutOptions.Center;
				this.lblTitle.Text = this.Title;
				this.lblTitle.TextColor = this.Settings.TextColor;
				this.stlTitle.Children.Add(this.lblTitle);
			}
			this.stlButtons = new StackLayout
			{
				HorizontalOptions = LayoutOptions.End,
				Orientation = 1
			};
			if (!string.IsNullOrEmpty(this.Settings.OkButtonText))
			{
				this.btnOk = new Button();
				this.btnOk.Clicked += this.Btn_Clicked;
				this.btnOk.Text = this.Settings.OkButtonText;
				this.stlButtons.Children.Add(this.btnOk);
			}
			if (!string.IsNullOrEmpty(this.Settings.CancelButtonText))
			{
				if (this.btnOk != null)
				{
					this.btnOk.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
				}
				this.btnCancel = new Button();
				this.btnCancel.Clicked += this.Btn_Clicked;
				this.btnCancel.Text = this.Settings.CancelButtonText;
				this.stlButtons.Children.Add(this.btnCancel);
			}
			IList<View> list = this.MainConteiner.Children;
			View view = await this.BuildContent();
			list.Add(view);
			list = null;
			this.MainConteiner.Children.Add(this.stlButtons);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000289B File Offset: 0x00000A9B
		protected void CloseDialog()
		{
			if (this.buttonClicked != null)
			{
				this.buttonClicked.TrySetResult("");
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028B8 File Offset: 0x00000AB8
		private async Task<string> WaitForClick()
		{
			this.buttonClicked = new TaskCompletionSource<string>();
			return await this.buttonClicked.Task;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000028FB File Offset: 0x00000AFB
		private void Btn_Clicked(object sender, EventArgs e)
		{
			if (this.buttonClicked != null)
			{
				this.buttonClicked.TrySetResult(((Button)sender).Text);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000291C File Offset: 0x00000B1C
		protected Dialog()
		{
		}

		// Token: 0x0400001E RID: 30
		[CompilerGenerated]
		private DialogSettings <Settings>k__BackingField;

		// Token: 0x0400001F RID: 31
		[CompilerGenerated]
		private Layout<View> <MainConteiner>k__BackingField;

		// Token: 0x04000020 RID: 32
		[CompilerGenerated]
		private Frame <MainFrame>k__BackingField;

		// Token: 0x04000021 RID: 33
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x04000022 RID: 34
		protected Label lblTitle;

		// Token: 0x04000023 RID: 35
		protected Button btnOk;

		// Token: 0x04000024 RID: 36
		protected Button btnCancel;

		// Token: 0x04000025 RID: 37
		protected StackLayout stlTitle;

		// Token: 0x04000026 RID: 38
		protected StackLayout stlButtons;

		// Token: 0x04000027 RID: 39
		private TaskCompletionSource<string> buttonClicked;

		// Token: 0x0200000B RID: 11
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Initialize>d__25 : IAsyncStateMachine
		{
			// Token: 0x0600003A RID: 58 RVA: 0x00002924 File Offset: 0x00000B24
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dialog dialog = this;
				try
				{
					TaskAwaiter<View> taskAwaiter;
					if (num != 0)
					{
						dialog.Children.Clear();
						dialog.HorizontalOptions = LayoutOptions.Fill;
						dialog.VerticalOptions = LayoutOptions.Fill;
						dialog.BackgroundColor = dialog.Settings.BackgroundColor;
						dialog.Margin = new Thickness(-20.0);
						dialog.Padding = new Thickness(20.0);
						dialog.MainFrame = new Frame
						{
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Center,
							BackgroundColor = dialog.Settings.DialogColor,
							Padding = new Thickness(16.0),
							HasShadow = true
						};
						TargetIdiom idiom = Device.Idiom;
						if (idiom - 2 <= 1)
						{
							dialog.MainFrame.WidthRequest = 300.0;
						}
						dialog.Children.Add(dialog.MainFrame);
						dialog.MainConteiner = new StackLayout
						{
							Orientation = 0
						};
						dialog.MainFrame.Content = dialog.MainConteiner;
						if (!string.IsNullOrEmpty(dialog.Title))
						{
							dialog.stlTitle = new StackLayout
							{
								Orientation = 1,
								Margin = new Thickness(0.0, 0.0, 0.0, 10.0),
								HorizontalOptions = LayoutOptions.Fill
							};
							dialog.MainConteiner.Children.Add(dialog.stlTitle);
							dialog.lblTitle = new Label();
							dialog.lblTitle.FontSize *= 1.5;
							dialog.lblTitle.VerticalOptions = LayoutOptions.Center;
							dialog.lblTitle.Text = dialog.Title;
							dialog.lblTitle.TextColor = dialog.Settings.TextColor;
							dialog.stlTitle.Children.Add(dialog.lblTitle);
						}
						dialog.stlButtons = new StackLayout
						{
							HorizontalOptions = LayoutOptions.End,
							Orientation = 1
						};
						if (!string.IsNullOrEmpty(dialog.Settings.OkButtonText))
						{
							dialog.btnOk = new Button();
							dialog.btnOk.Clicked += dialog.Btn_Clicked;
							dialog.btnOk.Text = dialog.Settings.OkButtonText;
							dialog.stlButtons.Children.Add(dialog.btnOk);
						}
						if (!string.IsNullOrEmpty(dialog.Settings.CancelButtonText))
						{
							if (dialog.btnOk != null)
							{
								dialog.btnOk.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
							}
							dialog.btnCancel = new Button();
							dialog.btnCancel.Clicked += dialog.Btn_Clicked;
							dialog.btnCancel.Text = dialog.Settings.CancelButtonText;
							dialog.stlButtons.Children.Add(dialog.btnCancel);
						}
						list = dialog.MainConteiner.Children;
						taskAwaiter = dialog.BuildContent().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<View> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<View>, Dialog.<Initialize>d__25>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<View> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<View>);
						num2 = -1;
					}
					View result = taskAwaiter.GetResult();
					list.Add(result);
					list = null;
					dialog.MainConteiner.Children.Add(dialog.stlButtons);
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

			// Token: 0x0600003B RID: 59 RVA: 0x00002D24 File Offset: 0x00000F24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000028 RID: 40
			public int <>1__state;

			// Token: 0x04000029 RID: 41
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400002A RID: 42
			public Dialog <>4__this;

			// Token: 0x0400002B RID: 43
			private IList<View> <>7__wrap1;

			// Token: 0x0400002C RID: 44
			private TaskAwaiter<View> <>u__1;
		}

		// Token: 0x0200000C RID: 12
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowDialog>d__24 : IAsyncStateMachine
		{
			// Token: 0x0600003C RID: 60 RVA: 0x00002D34 File Offset: 0x00000F34
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dialog dialog = this;
				bool flag;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_01E4;
					}
					case 2:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0290;
					}
					default:
					{
						if (dialog.Settings == null)
						{
							dialog.Settings = new DialogSettings();
						}
						dialog.Parent = null;
						dialog.MinimumWidthRequest = parent.Width;
						Grid grid = parent as Grid;
						if (grid != null)
						{
							if (grid.RowDefinitions.Count > 1)
							{
								Grid.SetRowSpan(dialog, grid.RowDefinitions.Count);
							}
							if (grid.ColumnDefinitions.Count > 1)
							{
								Grid.SetColumnSpan(dialog, grid.ColumnDefinitions.Count);
							}
						}
						animLength = 400U;
						if (!dialog.Settings.DialogAnimation)
						{
							parent.Children.Add(dialog);
							goto IL_018D;
						}
						dialog.Opacity = 0.0;
						dialog.MainFrame.Scale = 0.75;
						parent.Children.Add(dialog);
						ViewExtensions.FadeTo(dialog, 1.0, animLength, Easing.SinOut);
						taskAwaiter = ViewExtensions.ScaleTo(dialog.MainFrame, 1.0, animLength, Easing.SinOut).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, Dialog.<ShowDialog>d__24>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					IL_018D:
					taskAwaiter3 = dialog.WaitForClick().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, Dialog.<ShowDialog>d__24>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01E4:
					string result2 = taskAwaiter3.GetResult();
					result = result2;
					if (!dialog.Settings.DialogAnimation)
					{
						goto IL_0298;
					}
					ViewExtensions.FadeTo(dialog, 0.0, animLength, Easing.SinIn);
					taskAwaiter = ViewExtensions.ScaleTo(dialog.MainFrame, 0.75, animLength, Easing.SinIn).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, Dialog.<ShowDialog>d__24>(ref taskAwaiter, ref this);
						return;
					}
					IL_0290:
					taskAwaiter.GetResult();
					IL_0298:
					parent.Children.Remove(dialog);
					string text = result;
					Button btnOk = dialog.btnOk;
					flag = text == ((btnOk != null) ? btnOk.Text : null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					result = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				result = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00003064 File Offset: 0x00001264
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400002D RID: 45
			public int <>1__state;

			// Token: 0x0400002E RID: 46
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400002F RID: 47
			public Dialog <>4__this;

			// Token: 0x04000030 RID: 48
			public Layout<View> parent;

			// Token: 0x04000031 RID: 49
			private uint <animLength>5__2;

			// Token: 0x04000032 RID: 50
			private string <result>5__3;

			// Token: 0x04000033 RID: 51
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000034 RID: 52
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x0200000D RID: 13
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WaitForClick>d__27 : IAsyncStateMachine
		{
			// Token: 0x0600003E RID: 62 RVA: 0x00003074 File Offset: 0x00001274
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dialog dialog = this;
				string result;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						dialog.buttonClicked = new TaskCompletionSource<string>();
						taskAwaiter = dialog.buttonClicked.Task.GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, Dialog.<WaitForClick>d__27>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x0600003F RID: 63 RVA: 0x0000313C File Offset: 0x0000133C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000035 RID: 53
			public int <>1__state;

			// Token: 0x04000036 RID: 54
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04000037 RID: 55
			public Dialog <>4__this;

			// Token: 0x04000038 RID: 56
			private TaskAwaiter<string> <>u__1;
		}
	}
}
