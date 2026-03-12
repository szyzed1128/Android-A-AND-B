using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000207 RID: 519
	internal static class OnlinePatch
	{
		// Token: 0x06001A74 RID: 6772 RVA: 0x001243EC File Offset: 0x001225EC
		public static async Task GetPatch()
		{
			try
			{
				if (!(new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimePatchUpdateChecked) < TimeSpan.FromHours(1.0)))
				{
					string text = await HttpDownloader.Get(new string[]
					{
						"https://node2.carscanner.info/patch/" + App.Version + ".txt",
						"https://node3.carscanner.info/patch/" + App.Version + ".txt"
					}, 10, null, true);
					if (!string.IsNullOrEmpty(text))
					{
						SharedSettings.Current.LastTimePatchUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
						string[] array = (from x in text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
							where x.Contains('=')
							select x).ToArray<string>();
						Type typeFromHandle = typeof(SharedSettings);
						foreach (string text2 in array)
						{
							try
							{
								string[] array2 = (from x in text2.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)
									select x.Trim()).ToArray<string>();
								string text3 = array2[0];
								string text4 = array2[1];
								if (!SharedSettings.SkipListBase.Contains(text3) || SharedSettings.OnlinePatchAllowedList.Contains(text3))
								{
									PropertyInfo property = typeFromHandle.GetProperty(text3);
									if (property != null)
									{
										Type propertyType = property.PropertyType;
										if (propertyType.IsEnum || propertyType == typeof(int))
										{
											int num = int.Parse(text4, CultureInfo.InvariantCulture);
											property.SetValue(SharedSettings.Current, num);
										}
										else if (propertyType == typeof(long))
										{
											long num2 = long.Parse(text4, CultureInfo.InvariantCulture);
											property.SetValue(SharedSettings.Current, num2);
										}
										else if (propertyType == typeof(double))
										{
											double num3 = double.Parse(text4, CultureInfo.InvariantCulture);
											property.SetValue(SharedSettings.Current, num3);
										}
										else if (propertyType == typeof(string))
										{
											property.SetValue(SharedSettings.Current, text4);
										}
										else if (propertyType == typeof(bool))
										{
											if (!(text4 == "true") && !(text4 == "1"))
											{
												if (text4 == "false" || text4 == "0")
												{
													property.SetValue(SharedSettings.Current, false);
												}
											}
											else
											{
												property.SetValue(SharedSettings.Current, true);
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
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x02000208 RID: 520
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001A75 RID: 6773 RVA: 0x00124427 File Offset: 0x00122627
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001A76 RID: 6774 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001A77 RID: 6775 RVA: 0x00124433 File Offset: 0x00122633
			internal bool <GetPatch>b__0_0(string x)
			{
				return x.Contains('=');
			}

			// Token: 0x06001A78 RID: 6776 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <GetPatch>b__0_1(string x)
			{
				return x.Trim();
			}

			// Token: 0x04000BB4 RID: 2996
			public static readonly OnlinePatch.<>c <>9 = new OnlinePatch.<>c();

			// Token: 0x04000BB5 RID: 2997
			public static Func<string, bool> <>9__0_0;

			// Token: 0x04000BB6 RID: 2998
			public static Func<string, string> <>9__0_1;
		}

		// Token: 0x02000209 RID: 521
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetPatch>d__0 : IAsyncStateMachine
		{
			// Token: 0x06001A79 RID: 6777 RVA: 0x00124448 File Offset: 0x00122648
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							if (new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimePatchUpdateChecked) < TimeSpan.FromHours(1.0))
							{
								goto IL_0361;
							}
							taskAwaiter = HttpDownloader.Get(new string[]
							{
								"https://node2.carscanner.info/patch/" + App.Version + ".txt",
								"https://node3.carscanner.info/patch/" + App.Version + ".txt"
							}, 10, null, true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OnlinePatch.<GetPatch>d__0>(ref taskAwaiter, ref this);
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
						string result = taskAwaiter.GetResult();
						if (!string.IsNullOrEmpty(result))
						{
							SharedSettings.Current.LastTimePatchUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
							string[] array = (from x in result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
								where x.Contains('=')
								select x).ToArray<string>();
							Type typeFromHandle = typeof(SharedSettings);
							foreach (string text in array)
							{
								try
								{
									string[] array3 = (from x in text.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries)
										select x.Trim()).ToArray<string>();
									string text2 = array3[0];
									string text3 = array3[1];
									if (!SharedSettings.SkipListBase.Contains(text2) || SharedSettings.OnlinePatchAllowedList.Contains(text2))
									{
										PropertyInfo property = typeFromHandle.GetProperty(text2);
										if (property != null)
										{
											Type propertyType = property.PropertyType;
											if (propertyType.IsEnum || propertyType == typeof(int))
											{
												int num3 = int.Parse(text3, CultureInfo.InvariantCulture);
												property.SetValue(SharedSettings.Current, num3);
											}
											else if (propertyType == typeof(long))
											{
												long num4 = long.Parse(text3, CultureInfo.InvariantCulture);
												property.SetValue(SharedSettings.Current, num4);
											}
											else if (propertyType == typeof(double))
											{
												double num5 = double.Parse(text3, CultureInfo.InvariantCulture);
												property.SetValue(SharedSettings.Current, num5);
											}
											else if (propertyType == typeof(string))
											{
												property.SetValue(SharedSettings.Current, text3);
											}
											else if (propertyType == typeof(bool))
											{
												if (!(text3 == "true") && !(text3 == "1"))
												{
													if (text3 == "false" || text3 == "0")
													{
														property.SetValue(SharedSettings.Current, false);
													}
												}
												else
												{
													property.SetValue(SharedSettings.Current, true);
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
				IL_0361:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001A7A RID: 6778 RVA: 0x00124818 File Offset: 0x00122A18
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BB7 RID: 2999
			public int <>1__state;

			// Token: 0x04000BB8 RID: 3000
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000BB9 RID: 3001
			private TaskAwaiter<string> <>u__1;
		}
	}
}
