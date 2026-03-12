using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005E4 RID: 1508
	[Preserve(AllMembers = true)]
	public class TapAnimationGrid : Grid
	{
		// Token: 0x060035E4 RID: 13796 RVA: 0x0026AB68 File Offset: 0x00268D68
		public TapAnimationGrid()
		{
			this.Initialize();
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x0026AB76 File Offset: 0x00268D76
		// (set) Token: 0x060035E6 RID: 13798 RVA: 0x0026AB88 File Offset: 0x00268D88
		public int AnimationDuration
		{
			get
			{
				return (int)base.GetValue(TapAnimationGrid.AnimationDurationProperty);
			}
			set
			{
				base.SetValue(TapAnimationGrid.AnimationDurationProperty, value);
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x060035E7 RID: 13799 RVA: 0x0026AB9B File Offset: 0x00268D9B
		// (set) Token: 0x060035E8 RID: 13800 RVA: 0x0026ABAD File Offset: 0x00268DAD
		public Color AnimationColor
		{
			get
			{
				return (Color)base.GetValue(TapAnimationGrid.AnimationColorProperty);
			}
			set
			{
				base.SetValue(TapAnimationGrid.AnimationColorProperty, value);
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x060035E9 RID: 13801 RVA: 0x0026ABC0 File Offset: 0x00268DC0
		// (set) Token: 0x060035EA RID: 13802 RVA: 0x0026ABD2 File Offset: 0x00268DD2
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(TapAnimationGrid.CommandProperty);
			}
			set
			{
				base.SetValue(TapAnimationGrid.CommandProperty, value);
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x0026ABE0 File Offset: 0x00268DE0
		// (set) Token: 0x060035EC RID: 13804 RVA: 0x0026ABED File Offset: 0x00268DED
		public object CommandParameter
		{
			get
			{
				return base.GetValue(TapAnimationGrid.CommandParameterProperty);
			}
			set
			{
				base.SetValue(TapAnimationGrid.CommandParameterProperty, value);
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x0026ABFB File Offset: 0x00268DFB
		// (set) Token: 0x060035EE RID: 13806 RVA: 0x0026AC0D File Offset: 0x00268E0D
		public bool Tapped
		{
			get
			{
				return (bool)base.GetValue(TapAnimationGrid.TappedProperty);
			}
			set
			{
				base.SetValue(TapAnimationGrid.TappedProperty, value);
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x0026AC20 File Offset: 0x00268E20
		public ICommand TappedCommand
		{
			get
			{
				ICommand command;
				if ((command = this.tappedCommand) == null)
				{
					command = (this.tappedCommand = new Command(delegate
					{
						if (this.Tapped)
						{
							this.Tapped = false;
						}
						else
						{
							this.Tapped = true;
						}
						if (this.Command != null)
						{
							this.Command.Execute(this.CommandParameter);
						}
					}));
				}
				return command;
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0026AC54 File Offset: 0x00268E54
		private static async void OnTapped(BindableObject bindable, object oldValue, object newValue)
		{
			TapAnimationGrid grid = (TapAnimationGrid)bindable;
			Color bgcolor = grid.BackgroundColor;
			if (!(bgcolor == grid.AnimationColor))
			{
				grid.BackgroundColor = grid.AnimationColor;
				await Task.Delay(grid.AnimationDuration).ConfigureAwait(true);
				grid.BackgroundColor = bgcolor;
			}
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0026AC8B File Offset: 0x00268E8B
		private void Initialize()
		{
			base.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = this.TappedCommand
			});
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0026ACAC File Offset: 0x00268EAC
		// Note: this type is marked as 'beforefieldinit'.
		static TapAnimationGrid()
		{
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0026ADAA File Offset: 0x00268FAA
		[CompilerGenerated]
		private void <get_TappedCommand>b__23_0()
		{
			if (this.Tapped)
			{
				this.Tapped = false;
			}
			else
			{
				this.Tapped = true;
			}
			if (this.Command != null)
			{
				this.Command.Execute(this.CommandParameter);
			}
		}

		// Token: 0x04002018 RID: 8216
		public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(TapAnimationGrid), null, 2, null, null, null, null, null);

		// Token: 0x04002019 RID: 8217
		public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(TapAnimationGrid), null, 2, null, null, null, null, null);

		// Token: 0x0400201A RID: 8218
		public static readonly BindableProperty TappedProperty = BindableProperty.Create("Tapped", typeof(bool), typeof(TapAnimationGrid), false, 1, null, new BindableProperty.BindingPropertyChangedDelegate(TapAnimationGrid.OnTapped), null, null, null);

		// Token: 0x0400201B RID: 8219
		private ICommand tappedCommand;

		// Token: 0x0400201C RID: 8220
		public static readonly BindableProperty AnimationDurationProperty = BindableProperty.Create("AnimationDuration", typeof(int), typeof(TapAnimationGrid), 100, 2, null, null, null, null, null);

		// Token: 0x0400201D RID: 8221
		public static readonly BindableProperty AnimationColorProperty = BindableProperty.Create("AnimationColor", typeof(Color), typeof(TapAnimationGrid), Color.Gray, 2, null, null, null, null, null);

		// Token: 0x020005E5 RID: 1509
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnTapped>d__24 : IAsyncStateMachine
		{
			// Token: 0x060035F4 RID: 13812 RVA: 0x0026ADE0 File Offset: 0x00268FE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter;
					if (num != 0)
					{
						grid = (TapAnimationGrid)bindable;
						bgcolor = grid.BackgroundColor;
						if (bgcolor == grid.AnimationColor)
						{
							goto IL_00FE;
						}
						grid.BackgroundColor = grid.AnimationColor;
						configuredTaskAwaiter = Task.Delay(grid.AnimationDuration).ConfigureAwait(true).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, TapAnimationGrid.<OnTapped>d__24>(ref configuredTaskAwaiter, ref this);
							return;
						}
					}
					else
					{
						ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num2 = -1;
					}
					configuredTaskAwaiter.GetResult();
					grid.BackgroundColor = bgcolor;
				}
				catch (Exception ex)
				{
					num2 = -2;
					grid = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00FE:
				num2 = -2;
				grid = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060035F5 RID: 13813 RVA: 0x0026AF18 File Offset: 0x00269118
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400201E RID: 8222
			public int <>1__state;

			// Token: 0x0400201F RID: 8223
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002020 RID: 8224
			public BindableObject bindable;

			// Token: 0x04002021 RID: 8225
			private TapAnimationGrid <grid>5__2;

			// Token: 0x04002022 RID: 8226
			private Color <bgcolor>5__3;

			// Token: 0x04002023 RID: 8227
			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter <>u__1;
		}
	}
}
