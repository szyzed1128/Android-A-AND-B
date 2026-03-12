using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000005 RID: 5
	public class ColorPickerDialog : Dialog
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002388 File Offset: 0x00000588
		public static async Task<Color> Show(Layout<View> parent, string title, Color defaultColor, ColorDialogSettings settings = null)
		{
			ColorPickerDialog dlg = new ColorPickerDialog
			{
				Parent = parent,
				Title = title,
				originalColor = defaultColor,
				settings = (settings ?? new ColorDialogSettings())
			};
			dlg.Settings = dlg.settings;
			await dlg.Initialize();
			TaskAwaiter<bool> taskAwaiter = dlg.ShowDialog(parent).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			Color color;
			if (taskAwaiter.GetResult())
			{
				color = dlg.colorEditor.Color;
			}
			else
			{
				color = defaultColor;
			}
			return color;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023E4 File Offset: 0x000005E4
		protected override async Task<View> BuildContent()
		{
			this.colorEditor = new ColorPickerMixer
			{
				TextColor = this.settings.TextColor,
				EditorsColor = this.settings.EditorsColor,
				ColorPreviewBorderColor = this.settings.ColorPreviewBorderColor,
				EditAlpha = this.settings.EditAlfa
			};
			this.colorEditor.Color = this.originalColor;
			return this.colorEditor;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002427 File Offset: 0x00000627
		public ColorPickerDialog()
		{
		}

		// Token: 0x04000006 RID: 6
		private Color originalColor;

		// Token: 0x04000007 RID: 7
		private ColorDialogSettings settings;

		// Token: 0x04000008 RID: 8
		private ColorPickerMixer colorEditor;

		// Token: 0x02000006 RID: 6
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BuildContent>d__4 : IAsyncStateMachine
		{
			// Token: 0x06000012 RID: 18 RVA: 0x00002430 File Offset: 0x00000630
			void IAsyncStateMachine.MoveNext()
			{
				ColorPickerDialog colorPickerDialog = this;
				View colorEditor;
				try
				{
					colorPickerDialog.colorEditor = new ColorPickerMixer
					{
						TextColor = colorPickerDialog.settings.TextColor,
						EditorsColor = colorPickerDialog.settings.EditorsColor,
						ColorPreviewBorderColor = colorPickerDialog.settings.ColorPreviewBorderColor,
						EditAlpha = colorPickerDialog.settings.EditAlfa
					};
					colorPickerDialog.colorEditor.Color = colorPickerDialog.originalColor;
					colorEditor = colorPickerDialog.colorEditor;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(colorEditor);
			}

			// Token: 0x06000013 RID: 19 RVA: 0x000024E8 File Offset: 0x000006E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000009 RID: 9
			public int <>1__state;

			// Token: 0x0400000A RID: 10
			public AsyncTaskMethodBuilder<View> <>t__builder;

			// Token: 0x0400000B RID: 11
			public ColorPickerDialog <>4__this;
		}

		// Token: 0x02000007 RID: 7
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Show>d__3 : IAsyncStateMachine
		{
			// Token: 0x06000014 RID: 20 RVA: 0x000024F8 File Offset: 0x000006F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Color color;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_012A;
						}
						dlg = new ColorPickerDialog
						{
							Parent = parent,
							Title = title,
							originalColor = defaultColor,
							settings = (settings ?? new ColorDialogSettings())
						};
						dlg.Settings = dlg.settings;
						taskAwaiter4 = dlg.Initialize().GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ColorPickerDialog.<Show>d__3>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter4.GetResult();
					taskAwaiter3 = dlg.ShowDialog(parent).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ColorPickerDialog.<Show>d__3>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012A:
					if (taskAwaiter3.GetResult())
					{
						color = dlg.colorEditor.Color;
					}
					else
					{
						color = defaultColor;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					dlg = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				dlg = null;
				this.<>t__builder.SetResult(color);
			}

			// Token: 0x06000015 RID: 21 RVA: 0x000026AC File Offset: 0x000008AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400000C RID: 12
			public int <>1__state;

			// Token: 0x0400000D RID: 13
			public AsyncTaskMethodBuilder<Color> <>t__builder;

			// Token: 0x0400000E RID: 14
			public Layout<View> parent;

			// Token: 0x0400000F RID: 15
			public string title;

			// Token: 0x04000010 RID: 16
			public Color defaultColor;

			// Token: 0x04000011 RID: 17
			public ColorDialogSettings settings;

			// Token: 0x04000012 RID: 18
			private ColorPickerDialog <dlg>5__2;

			// Token: 0x04000013 RID: 19
			private TaskAwaiter <>u__1;

			// Token: 0x04000014 RID: 20
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
