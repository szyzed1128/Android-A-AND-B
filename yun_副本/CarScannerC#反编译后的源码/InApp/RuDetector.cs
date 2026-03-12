using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x0200049D RID: 1181
	public class RuDetector
	{
		// Token: 0x06002F78 RID: 12152 RVA: 0x00211184 File Offset: 0x0020F384
		public static async Task<bool> IsRuIP()
		{
			bool flag;
			if (RuDetector.PrecachedIsRuIP != null && RuDetector.PrecachedIsRuIP != null)
			{
				flag = RuDetector.PrecachedIsRuIP.Value;
			}
			else
			{
				int num = 0;
				try
				{
					bool flag2 = await RuDetector.IsRuIPV2();
					RuDetector.PrecachedIsRuIP = new bool?(flag2);
					return flag2;
				}
				catch (Exception obj)
				{
					num = 1;
				}
				if (num == 1)
				{
					object obj;
					Exception ex = (Exception)obj;
					bool flag3 = await RuDetector.IsRuIPV1();
					RuDetector.PrecachedIsRuIP = new bool?(flag3);
					flag = flag3;
				}
			}
			return flag;
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x002111C0 File Offset: 0x0020F3C0
		public static async Task<bool> IsRuIPV2()
		{
			await RuDetector.GetResponseFromDB_IP_COM();
			if (RuDetector.db_ip_response == null || !RuDetector.db_ip_response.Contains("countryCode"))
			{
				throw new ArgumentException("null data");
			}
			bool flag;
			if (RuDetector.db_ip_response.Contains("\"countryCode\": \"RU\"") || RuDetector.db_ip_response.Contains("\"countryCode\": \"ru\""))
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002F7A RID: 12154 RVA: 0x002111FC File Offset: 0x0020F3FC
		private static async Task GetResponseFromDB_IP_COM()
		{
			if (RuDetector.db_ip_response == null)
			{
				string text = await HttpDownloader.Get("https://api.db-ip.com/v2/free/self", 5);
				if (text != null && text.Contains("countryCode"))
				{
					RuDetector.db_ip_response = text;
				}
			}
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x00211238 File Offset: 0x0020F438
		public static async Task<bool> IsUkrIPV2()
		{
			if (RuDetector.db_ip_response == null)
			{
				TaskAwaiter<string> taskAwaiter = HttpDownloader.Get("https://api.db-ip.com/v2/free/self", 5).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<string> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<string>);
				}
				RuDetector.db_ip_response = taskAwaiter.GetResult();
			}
			if (RuDetector.db_ip_response == null)
			{
				throw new ArgumentException("null data");
			}
			bool flag;
			if (RuDetector.db_ip_response.Contains("\"countryCode\": \"UA\"") || RuDetector.db_ip_response.Contains("\"countryCode\": \"ua\""))
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x00211274 File Offset: 0x0020F474
		public static async Task<bool> IsRuIPV1()
		{
			try
			{
				string text = await HttpDownloader.Get(Encoding.ASCII.GetString(Convert.FromBase64String(Translate.GetString("ru_ipValidUri"))), 5);
				if (text != null && text.Contains("{\"countryCode\":\"RU\"}"))
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x002112B0 File Offset: 0x0020F4B0
		public static bool IsLanguage(string lang)
		{
			if (lang == null)
			{
				return false;
			}
			try
			{
				if (lang.Equals(App.CurrentLanguageCode, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x002112F0 File Offset: 0x0020F4F0
		public static bool IsRuLanguage()
		{
			return RuDetector.IsLanguage("ru");
		}

		// Token: 0x06002F7F RID: 12159 RVA: 0x002112FC File Offset: 0x0020F4FC
		public static bool IsLocale(string lang)
		{
			if (lang == null)
			{
				return false;
			}
			try
			{
				if (PlatformHelper.IsiOS && lang.Equals(App.CurrentLanguageCode, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (PlatformHelper.IsAndroid)
				{
					string resources_Configuration_Locale_Country = PlatformHelper.DroidService.Resources_Configuration_Locale_Country;
					if (lang.Equals(resources_Configuration_Locale_Country, StringComparison.OrdinalIgnoreCase) || lang.Equals(App.CurrentLanguageCode, StringComparison.OrdinalIgnoreCase))
					{
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x00211370 File Offset: 0x0020F570
		public static bool IsRuLocale()
		{
			return RuDetector.IsLocale("ru");
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x0021137C File Offset: 0x0020F57C
		public static async ValueTask<bool> IsUA()
		{
			if ((RuDetector.IsLanguage("RU") || RuDetector.IsLanguage("UK")) && (RuDetector.IsLocale("RU") || RuDetector.IsLocale("UA")))
			{
				try
				{
					TaskAwaiter<bool> taskAwaiter = RuDetector.IsUkrIPV2().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						return 1;
					}
				}
				catch (Exception)
				{
					return 0;
				}
			}
			return 0;
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x002113B8 File Offset: 0x0020F5B8
		internal static async Task<bool?> IsRuCurrency()
		{
			bool? flag;
			if (PlatformHelper.AppMarket == Markets.Rustore || PlatformHelper.AppMarket == Markets.RUS)
			{
				flag = new bool?(true);
			}
			else
			{
				bool isCurrencyRub = false;
				if (PlatformHelper.AppMarket == Markets.GooglePlay || PlatformHelper.AppMarket == Markets.HMS || PlatformHelper.AppMarket == Markets.AppStore)
				{
					string productId = "";
					switch (PlatformHelper.AppMarket)
					{
					case Markets.AppStore:
						productId = "ovz.CarScanner.ProL4";
						break;
					case Markets.GooglePlay:
						productId = "ovz.carscanner.pro3";
						break;
					case Markets.HMS:
						productId = "ovz.carscanner.pro3";
						break;
					}
					ICustomInAppManager manager = PlatformHelper.CommonService.GetNewInAppManagerInstance();
					manager.Initialize();
					TaskAwaiter<bool> taskAwaiter = manager.CanMakePayments().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						return null;
					}
					SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
					ProductsReceivedEvent onProductsReceived = delegate(List<IProduct> products, string DebugString)
					{
						if (products == null || products.Count == 0 || (products.Count > 0 && products[0] == null))
						{
							try
							{
								if (semaphore.CurrentCount < 1)
								{
									semaphore.Release();
								}
							}
							catch (Exception)
							{
							}
							return;
						}
						IProduct product = products[0];
						if (product.CurrencyCode == "RUB" || product.CurrencyCode == "rub" || product.CurrencyCode == "Rub")
						{
							isCurrencyRub = true;
						}
						try
						{
							if (semaphore.CurrentCount < 1)
							{
								semaphore.Release();
							}
						}
						catch (Exception)
						{
						}
					};
					manager.OnProductsReceived -= onProductsReceived;
					manager.OnProductsReceived += onProductsReceived;
					manager.RequestProductData(new List<string> { productId });
					await Task.WhenAny(new Task[]
					{
						semaphore.WaitAsync(),
						Task.Delay(5000)
					});
					manager.OnProductsReceived -= onProductsReceived;
					manager.FreeResources();
					productId = null;
					manager = null;
					onProductsReceived = null;
				}
				flag = new bool?(isCurrencyRub);
			}
			return flag;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x00002050 File Offset: 0x00000250
		public RuDetector()
		{
		}

		// Token: 0x04001B71 RID: 7025
		public static bool? PrecachedIsRuIP;

		// Token: 0x04001B72 RID: 7026
		private static string db_ip_response;

		// Token: 0x0200049E RID: 1182
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06002F84 RID: 12164 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06002F85 RID: 12165 RVA: 0x002113F4 File Offset: 0x0020F5F4
			internal void <IsRuCurrency>b__0(List<IProduct> products, string DebugString)
			{
				if (products == null || products.Count == 0 || (products.Count > 0 && products[0] == null))
				{
					try
					{
						if (this.semaphore.CurrentCount < 1)
						{
							this.semaphore.Release();
						}
					}
					catch (Exception)
					{
					}
					return;
				}
				IProduct product = products[0];
				if (product.CurrencyCode == "RUB" || product.CurrencyCode == "rub" || product.CurrencyCode == "Rub")
				{
					this.isCurrencyRub = true;
				}
				try
				{
					if (this.semaphore.CurrentCount < 1)
					{
						this.semaphore.Release();
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04001B73 RID: 7027
			public bool isCurrencyRub;

			// Token: 0x04001B74 RID: 7028
			public SemaphoreSlim semaphore;
		}

		// Token: 0x0200049F RID: 1183
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetResponseFromDB_IP_COM>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002F86 RID: 12166 RVA: 0x002114C0 File Offset: 0x0020F6C0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (RuDetector.db_ip_response != null)
						{
							goto IL_0085;
						}
						taskAwaiter = HttpDownloader.Get("https://api.db-ip.com/v2/free/self", 5).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, RuDetector.<GetResponseFromDB_IP_COM>d__3>(ref taskAwaiter, ref this);
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
					if (result != null && result.Contains("countryCode"))
					{
						RuDetector.db_ip_response = result;
					}
					IL_0085:;
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

			// Token: 0x06002F87 RID: 12167 RVA: 0x00211590 File Offset: 0x0020F790
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B75 RID: 7029
			public int <>1__state;

			// Token: 0x04001B76 RID: 7030
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B77 RID: 7031
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020004A0 RID: 1184
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsRuCurrency>d__12 : IAsyncStateMachine
		{
			// Token: 0x06002F88 RID: 12168 RVA: 0x002115A0 File Offset: 0x0020F7A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool? flag;
				try
				{
					TaskAwaiter<Task> taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Task> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<Task>);
							num2 = -1;
							goto IL_0218;
						}
						CS$<>8__locals1 = new RuDetector.<>c__DisplayClass12_0();
						if (PlatformHelper.AppMarket == Markets.Rustore || PlatformHelper.AppMarket == Markets.RUS)
						{
							flag = new bool?(true);
							goto IL_0284;
						}
						CS$<>8__locals1.isCurrencyRub = false;
						if (PlatformHelper.AppMarket != Markets.GooglePlay && PlatformHelper.AppMarket != Markets.HMS && PlatformHelper.AppMarket != Markets.AppStore)
						{
							goto IL_0251;
						}
						productId = "";
						switch (PlatformHelper.AppMarket)
						{
						case Markets.AppStore:
							productId = "ovz.CarScanner.ProL4";
							break;
						case Markets.GooglePlay:
							productId = "ovz.carscanner.pro3";
							break;
						case Markets.HMS:
							productId = "ovz.carscanner.pro3";
							break;
						}
						manager = PlatformHelper.CommonService.GetNewInAppManagerInstance();
						manager.Initialize();
						taskAwaiter5 = manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, RuDetector.<IsRuCurrency>d__12>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						flag = null;
						goto IL_0284;
					}
					CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
					onProductsReceived = delegate(List<IProduct> products, string DebugString)
					{
						if (products == null || products.Count == 0 || (products.Count > 0 && products[0] == null))
						{
							try
							{
								if (CS$<>8__locals1.semaphore.CurrentCount < 1)
								{
									CS$<>8__locals1.semaphore.Release();
								}
							}
							catch (Exception)
							{
							}
							return;
						}
						IProduct product = products[0];
						if (product.CurrencyCode == "RUB" || product.CurrencyCode == "rub" || product.CurrencyCode == "Rub")
						{
							CS$<>8__locals1.isCurrencyRub = true;
						}
						try
						{
							if (CS$<>8__locals1.semaphore.CurrentCount < 1)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						}
						catch (Exception)
						{
						}
					};
					manager.OnProductsReceived -= onProductsReceived;
					manager.OnProductsReceived += onProductsReceived;
					manager.RequestProductData(new List<string> { productId });
					taskAwaiter3 = Task.WhenAny(new Task[]
					{
						CS$<>8__locals1.semaphore.WaitAsync(),
						Task.Delay(5000)
					}).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, RuDetector.<IsRuCurrency>d__12>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0218:
					taskAwaiter3.GetResult();
					manager.OnProductsReceived -= onProductsReceived;
					manager.FreeResources();
					productId = null;
					manager = null;
					onProductsReceived = null;
					IL_0251:
					flag = new bool?(CS$<>8__locals1.isCurrencyRub);
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0284:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F89 RID: 12169 RVA: 0x00211868 File Offset: 0x0020FA68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B78 RID: 7032
			public int <>1__state;

			// Token: 0x04001B79 RID: 7033
			public AsyncTaskMethodBuilder<bool?> <>t__builder;

			// Token: 0x04001B7A RID: 7034
			private RuDetector.<>c__DisplayClass12_0 <>8__1;

			// Token: 0x04001B7B RID: 7035
			private string <productId>5__2;

			// Token: 0x04001B7C RID: 7036
			private ICustomInAppManager <manager>5__3;

			// Token: 0x04001B7D RID: 7037
			private ProductsReceivedEvent <onProductsReceived>5__4;

			// Token: 0x04001B7E RID: 7038
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B7F RID: 7039
			private TaskAwaiter<Task> <>u__2;
		}

		// Token: 0x020004A1 RID: 1185
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsRuIP>d__0 : IAsyncStateMachine
		{
			// Token: 0x06002F8A RID: 12170 RVA: 0x00211878 File Offset: 0x0020FA78
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					TaskAwaiter<bool> taskAwaiter2;
					TaskAwaiter<bool> taskAwaiter;
					int num3;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_0115;
						}
						if (RuDetector.PrecachedIsRuIP != null && RuDetector.PrecachedIsRuIP != null)
						{
							flag = RuDetector.PrecachedIsRuIP.Value;
							goto IL_0148;
						}
						num3 = 0;
					}
					try
					{
						if (num != 0)
						{
							taskAwaiter = RuDetector.IsRuIPV2().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, RuDetector.<IsRuIP>d__0>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						RuDetector.PrecachedIsRuIP = new bool?(result);
						flag = result;
						goto IL_0148;
					}
					catch (Exception obj)
					{
						num3 = 1;
					}
					if (num3 != 1)
					{
						goto IL_012D;
					}
					object obj;
					Exception ex = (Exception)obj;
					taskAwaiter = RuDetector.IsRuIPV1().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, RuDetector.<IsRuIP>d__0>(ref taskAwaiter, ref this);
						return;
					}
					IL_0115:
					bool result2 = taskAwaiter.GetResult();
					RuDetector.PrecachedIsRuIP = new bool?(result2);
					flag = result2;
					IL_012D:;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0148:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F8B RID: 12171 RVA: 0x00211A18 File Offset: 0x0020FC18
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B80 RID: 7040
			public int <>1__state;

			// Token: 0x04001B81 RID: 7041
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B82 RID: 7042
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020004A2 RID: 1186
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsRuIPV1>d__6 : IAsyncStateMachine
		{
			// Token: 0x06002F8C RID: 12172 RVA: 0x00211A28 File Offset: 0x0020FC28
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = HttpDownloader.Get(Encoding.ASCII.GetString(Convert.FromBase64String(Translate.GetString("ru_ipValidUri"))), 5).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, RuDetector.<IsRuIPV1>d__6>(ref taskAwaiter, ref this);
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
						if (result != null && result.Contains("{\"countryCode\":\"RU\"}"))
						{
							flag = true;
							goto IL_00B5;
						}
					}
					catch (Exception)
					{
					}
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00B5:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F8D RID: 12173 RVA: 0x00211B1C File Offset: 0x0020FD1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B83 RID: 7043
			public int <>1__state;

			// Token: 0x04001B84 RID: 7044
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B85 RID: 7045
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020004A3 RID: 1187
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsRuIPV2>d__2 : IAsyncStateMachine
		{
			// Token: 0x06002F8E RID: 12174 RVA: 0x00211B2C File Offset: 0x0020FD2C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = RuDetector.GetResponseFromDB_IP_COM().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RuDetector.<IsRuIPV2>d__2>(ref taskAwaiter, ref this);
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
					if (RuDetector.db_ip_response == null || !RuDetector.db_ip_response.Contains("countryCode"))
					{
						throw new ArgumentException("null data");
					}
					if (RuDetector.db_ip_response.Contains("\"countryCode\": \"RU\"") || RuDetector.db_ip_response.Contains("\"countryCode\": \"ru\""))
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F8F RID: 12175 RVA: 0x00211C28 File Offset: 0x0020FE28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B86 RID: 7046
			public int <>1__state;

			// Token: 0x04001B87 RID: 7047
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B88 RID: 7048
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020004A4 RID: 1188
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsUA>d__11 : IAsyncStateMachine
		{
			// Token: 0x06002F90 RID: 12176 RVA: 0x00211C38 File Offset: 0x0020FE38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					if (num == 0 || ((RuDetector.IsLanguage("RU") || RuDetector.IsLanguage("UK")) && (RuDetector.IsLocale("RU") || RuDetector.IsLocale("UA"))))
					{
						try
						{
							TaskAwaiter<bool> taskAwaiter3;
							if (num != 0)
							{
								taskAwaiter3 = RuDetector.IsUkrIPV2().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, RuDetector.<IsUA>d__11>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num2 = -1;
							}
							if (taskAwaiter3.GetResult())
							{
								flag = true;
								goto IL_00C0;
							}
						}
						catch (Exception)
						{
							flag = false;
							goto IL_00C0;
						}
					}
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00C0:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F91 RID: 12177 RVA: 0x00211D38 File Offset: 0x0020FF38
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B89 RID: 7049
			public int <>1__state;

			// Token: 0x04001B8A RID: 7050
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B8B RID: 7051
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020004A5 RID: 1189
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <IsUkrIPV2>d__4 : IAsyncStateMachine
		{
			// Token: 0x06002F92 RID: 12178 RVA: 0x00211D48 File Offset: 0x0020FF48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					TaskAwaiter<string> taskAwaiter3;
					if (num != 0)
					{
						if (RuDetector.db_ip_response != null)
						{
							goto IL_0076;
						}
						taskAwaiter3 = HttpDownloader.Get("https://api.db-ip.com/v2/free/self", 5).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, RuDetector.<IsUkrIPV2>d__4>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					RuDetector.db_ip_response = taskAwaiter3.GetResult();
					IL_0076:
					if (RuDetector.db_ip_response == null)
					{
						throw new ArgumentException("null data");
					}
					if (RuDetector.db_ip_response.Contains("\"countryCode\": \"UA\"") || RuDetector.db_ip_response.Contains("\"countryCode\": \"ua\""))
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F93 RID: 12179 RVA: 0x00211E44 File Offset: 0x00210044
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B8C RID: 7052
			public int <>1__state;

			// Token: 0x04001B8D RID: 7053
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B8E RID: 7054
			private TaskAwaiter<string> <>u__1;
		}
	}
}
