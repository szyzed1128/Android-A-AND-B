using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005E6 RID: 1510
	public static class ToastHelper
	{
		// Token: 0x060035F6 RID: 13814 RVA: 0x0026AF28 File Offset: 0x00269128
		public static void DisplayToast(string message, double durationSec = 3.0)
		{
			try
			{
				ToastHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = new ToastHelper.<>c__DisplayClass0_0();
				CS$<>8__locals1.page = App.GetCurrentPage();
				if (CS$<>8__locals1.page != null)
				{
					CS$<>8__locals1.snackOpts = new SnackBarOptions
					{
						MessageOptions = new MessageOptions
						{
							Message = message,
							Foreground = (Color)Application.Current.Resources["TextInverseColor"]
						},
						BackgroundColor = (Color)Application.Current.Resources["TextColor"],
						Duration = TimeSpan.FromSeconds(durationSec)
					};
					if (App.CurrentLanguageCode == "ar")
					{
						CS$<>8__locals1.snackOpts.IsRtl = true;
					}
					else
					{
						CS$<>8__locals1.snackOpts.IsRtl = false;
					}
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						ToastHelper.<>c__DisplayClass0_0.<<DisplayToast>b__0>d <<DisplayToast>b__0>d;
						<<DisplayToast>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<DisplayToast>b__0>d.<>4__this = CS$<>8__locals1;
						<<DisplayToast>b__0>d.<>1__state = -1;
						<<DisplayToast>b__0>d.<>t__builder.Start<ToastHelper.<>c__DisplayClass0_0.<<DisplayToast>b__0>d>(ref <<DisplayToast>b__0>d);
					});
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0026B00C File Offset: 0x0026920C
		public static async void DisplayOBDStatusToast(OBDDataReaderStatus status)
		{
			await Task.Delay(1000);
			if (App.OBDReader.CurrentStatus == status)
			{
				try
				{
					ToastHelper.<>c__DisplayClass1_0 CS$<>8__locals1 = new ToastHelper.<>c__DisplayClass1_0();
					if (Application.Current != null)
					{
						if (Application.Current.Resources != null)
						{
							CS$<>8__locals1.page = App.GetCurrentPage();
							if (CS$<>8__locals1.page != null)
							{
								if (!(CS$<>8__locals1.page is SimpleMainPage))
								{
									string text = null;
									Color color = Color.White;
									switch (status)
									{
									case OBDDataReaderStatus.Disconnected:
									case OBDDataReaderStatus.Disconnecting:
										text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusDisconnected");
										color = (Color)Application.Current.Resources["RedTextColor"];
										break;
									case OBDDataReaderStatus.ConnectingToELM:
										text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnecting");
										color = (Color)Application.Current.Resources["YellowTextColor"];
										break;
									case OBDDataReaderStatus.ConnectedToELM:
										text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnected");
										color = (Color)Application.Current.Resources["YellowTextColor"];
										break;
									case OBDDataReaderStatus.ConnectingToECU:
										text = Translate.GetString("MainPage_ECU_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnecting");
										color = (Color)Application.Current.Resources["YellowTextColor"];
										break;
									case OBDDataReaderStatus.ConnectedToECU:
										text = Translate.GetString("MainPage_ECU_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnected");
										color = (Color)Application.Current.Resources["GreenTextColor"];
										break;
									}
									CS$<>8__locals1.snackOpts = new SnackBarOptions
									{
										MessageOptions = new MessageOptions
										{
											Message = text,
											Foreground = color
										},
										BackgroundColor = Color.Gray,
										Duration = TimeSpan.FromSeconds(2.0)
									};
									if (App.CurrentLanguageCode == "ar")
									{
										CS$<>8__locals1.snackOpts.IsRtl = true;
									}
									else
									{
										CS$<>8__locals1.snackOpts.IsRtl = false;
									}
									MainThreadHelper.InvokeOnMainThread(delegate
									{
										ToastHelper.<>c__DisplayClass1_0.<<DisplayOBDStatusToast>b__0>d <<DisplayOBDStatusToast>b__0>d;
										<<DisplayOBDStatusToast>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
										<<DisplayOBDStatusToast>b__0>d.<>4__this = CS$<>8__locals1;
										<<DisplayOBDStatusToast>b__0>d.<>1__state = -1;
										<<DisplayOBDStatusToast>b__0>d.<>t__builder.Start<ToastHelper.<>c__DisplayClass1_0.<<DisplayOBDStatusToast>b__0>d>(ref <<DisplayOBDStatusToast>b__0>d);
									});
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x020005E7 RID: 1511
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060035F8 RID: 13816 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060035F9 RID: 13817 RVA: 0x0026B044 File Offset: 0x00269244
			internal async void <DisplayToast>b__0()
			{
				try
				{
					await VisualElementExtension.DisplaySnackBarAsync(this.page, this.snackOpts);
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04002024 RID: 8228
			public Page page;

			// Token: 0x04002025 RID: 8229
			public SnackBarOptions snackOpts;

			// Token: 0x020005E8 RID: 1512
			[StructLayout(LayoutKind.Auto)]
			private struct <<DisplayToast>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060035FA RID: 13818 RVA: 0x0026B07C File Offset: 0x0026927C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					ToastHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<bool> taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = VisualElementExtension.DisplaySnackBarAsync(CS$<>8__locals1.page, CS$<>8__locals1.snackOpts).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ToastHelper.<>c__DisplayClass0_0.<<DisplayToast>b__0>d>(ref taskAwaiter, ref this);
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
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
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

				// Token: 0x060035FB RID: 13819 RVA: 0x0026B150 File Offset: 0x00269350
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04002026 RID: 8230
				public int <>1__state;

				// Token: 0x04002027 RID: 8231
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04002028 RID: 8232
				public ToastHelper.<>c__DisplayClass0_0 <>4__this;

				// Token: 0x04002029 RID: 8233
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x020005E9 RID: 1513
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060035FC RID: 13820 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060035FD RID: 13821 RVA: 0x0026B160 File Offset: 0x00269360
			internal async void <DisplayOBDStatusToast>b__0()
			{
				try
				{
					await VisualElementExtension.DisplaySnackBarAsync(this.page, this.snackOpts);
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0400202A RID: 8234
			public Page page;

			// Token: 0x0400202B RID: 8235
			public SnackBarOptions snackOpts;

			// Token: 0x020005EA RID: 1514
			[StructLayout(LayoutKind.Auto)]
			private struct <<DisplayOBDStatusToast>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060035FE RID: 13822 RVA: 0x0026B198 File Offset: 0x00269398
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					ToastHelper.<>c__DisplayClass1_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<bool> taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = VisualElementExtension.DisplaySnackBarAsync(CS$<>8__locals1.page, CS$<>8__locals1.snackOpts).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ToastHelper.<>c__DisplayClass1_0.<<DisplayOBDStatusToast>b__0>d>(ref taskAwaiter, ref this);
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
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
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

				// Token: 0x060035FF RID: 13823 RVA: 0x0026B26C File Offset: 0x0026946C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400202C RID: 8236
				public int <>1__state;

				// Token: 0x0400202D RID: 8237
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x0400202E RID: 8238
				public ToastHelper.<>c__DisplayClass1_0 <>4__this;

				// Token: 0x0400202F RID: 8239
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x020005EB RID: 1515
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DisplayOBDStatusToast>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003600 RID: 13824 RVA: 0x0026B27C File Offset: 0x0026947C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					OBDDataReaderStatus _status;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						_status = status;
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToastHelper.<DisplayOBDStatusToast>d__1>(ref taskAwaiter, ref this);
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
					if (App.OBDReader.CurrentStatus == _status)
					{
						try
						{
							ToastHelper.<>c__DisplayClass1_0 CS$<>8__locals1 = new ToastHelper.<>c__DisplayClass1_0();
							if (Application.Current != null)
							{
								if (Application.Current.Resources != null)
								{
									CS$<>8__locals1.page = App.GetCurrentPage();
									if (CS$<>8__locals1.page != null)
									{
										if (!(CS$<>8__locals1.page is SimpleMainPage))
										{
											string text = null;
											Color color = Color.White;
											switch (status)
											{
											case OBDDataReaderStatus.Disconnected:
											case OBDDataReaderStatus.Disconnecting:
												text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusDisconnected");
												color = (Color)Application.Current.Resources["RedTextColor"];
												break;
											case OBDDataReaderStatus.ConnectingToELM:
												text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnecting");
												color = (Color)Application.Current.Resources["YellowTextColor"];
												break;
											case OBDDataReaderStatus.ConnectedToELM:
												text = Translate.GetString("MainPage_ELM_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnected");
												color = (Color)Application.Current.Resources["YellowTextColor"];
												break;
											case OBDDataReaderStatus.ConnectingToECU:
												text = Translate.GetString("MainPage_ECU_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnecting");
												color = (Color)Application.Current.Resources["YellowTextColor"];
												break;
											case OBDDataReaderStatus.ConnectedToECU:
												text = Translate.GetString("MainPage_ECU_Connection.Text") + " " + Translate.GetString("MainPage_StatusConnected");
												color = (Color)Application.Current.Resources["GreenTextColor"];
												break;
											}
											CS$<>8__locals1.snackOpts = new SnackBarOptions
											{
												MessageOptions = new MessageOptions
												{
													Message = text,
													Foreground = color
												},
												BackgroundColor = Color.Gray,
												Duration = TimeSpan.FromSeconds(2.0)
											};
											if (App.CurrentLanguageCode == "ar")
											{
												CS$<>8__locals1.snackOpts.IsRtl = true;
											}
											else
											{
												CS$<>8__locals1.snackOpts.IsRtl = false;
											}
											MainThreadHelper.InvokeOnMainThread(delegate
											{
												ToastHelper.<>c__DisplayClass1_0.<<DisplayOBDStatusToast>b__0>d <<DisplayOBDStatusToast>b__0>d;
												<<DisplayOBDStatusToast>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
												<<DisplayOBDStatusToast>b__0>d.<>4__this = CS$<>8__locals1;
												<<DisplayOBDStatusToast>b__0>d.<>1__state = -1;
												<<DisplayOBDStatusToast>b__0>d.<>t__builder.Start<ToastHelper.<>c__DisplayClass1_0.<<DisplayOBDStatusToast>b__0>d>(ref <<DisplayOBDStatusToast>b__0>d);
											});
										}
									}
								}
							}
						}
						catch (Exception)
						{
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

			// Token: 0x06003601 RID: 13825 RVA: 0x0026B5B0 File Offset: 0x002697B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002030 RID: 8240
			public int <>1__state;

			// Token: 0x04002031 RID: 8241
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002032 RID: 8242
			public OBDDataReaderStatus status;

			// Token: 0x04002033 RID: 8243
			private OBDDataReaderStatus <_status>5__2;

			// Token: 0x04002034 RID: 8244
			private TaskAwaiter <>u__1;
		}
	}
}
