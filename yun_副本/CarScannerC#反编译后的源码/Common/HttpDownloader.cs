using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.PlatformAdapters;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007D2 RID: 2002
	public class HttpDownloader
	{
		// Token: 0x060046CB RID: 18123 RVA: 0x0036B778 File Offset: 0x00369978
		public static async Task<string> Get(string[] uris, int timeOutSeconds = 10, Func<string, string, bool> verifyData = null, bool useBestServer = true)
		{
			if (useBestServer)
			{
				if (HttpDownloader.BestServer == HttpDownloader.Server.Unknown)
				{
					await HttpDownloader.CheckPing();
				}
				if (HttpDownloader.BestServer != HttpDownloader.Server.Unknown)
				{
					string bestServerUri = "https://" + HttpDownloader.BestServer.ToString() + ".carscanner.info";
					uris = uris.OrderByDescending((string x) => x.StartsWith(bestServerUri)).ToArray<string>();
				}
			}
			foreach (string uri in uris)
			{
				string text = await HttpDownloader.Get(uri, timeOutSeconds);
				if (text != null)
				{
					if (verifyData == null || verifyData(uri, text))
					{
						return text;
					}
					uri = null;
				}
			}
			string[] array = null;
			return null;
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x0036B7D4 File Offset: 0x003699D4
		public static async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token, Stream outputStream, int bufferSize = 65536)
		{
			using (HttpClient _client = HttpDownloader.GetHttpClient())
			{
				try
				{
					HttpResponseMessage httpResponseMessage = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);
					if (!httpResponseMessage.IsSuccessStatusCode)
					{
						throw new Exception(string.Format("The request returned with HTTP status code {0}", httpResponseMessage.StatusCode));
					}
					HttpContentHeaders headers = httpResponseMessage.Content.Headers;
					if (headers != null)
					{
						ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
						if (contentDisposition != null)
						{
							string fileName = contentDisposition.FileName;
						}
					}
					long totalData = httpResponseMessage.Content.Headers.ContentLength.GetValueOrDefault(-1L);
					bool canSendProgress = totalData != -1L && progress != null;
					using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync())
					{
						long totalRead = 0L;
						byte[] buffer = new byte[bufferSize];
						bool isMoreDataToRead = true;
						do
						{
							token.ThrowIfCancellationRequested();
							int read = await stream.ReadAsync(buffer, 0, buffer.Length, token);
							if (read == 0)
							{
								isMoreDataToRead = false;
							}
							else
							{
								await outputStream.WriteAsync(buffer, 0, read);
								totalRead += (long)read;
								if (canSendProgress)
								{
									progress.Report((double)totalRead * 1.0 / ((double)totalData * 1.0) * 100.0);
								}
							}
						}
						while (isMoreDataToRead);
						buffer = null;
					}
					Stream stream = null;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			HttpClient _client = null;
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x0036B838 File Offset: 0x00369A38
		public static HttpClient GetHttpClient()
		{
			HttpClient httpClient;
			if (PlatformHelper.IsAndroid)
			{
				if (!PlatformHelper.IsPlatformVersionNewerOrEqual(26, 0))
				{
					httpClient = new HttpClient(PlatformHelper.DroidService.GetNewDroid_http_BypassSslValidationClientHandler());
				}
				else
				{
					httpClient = new HttpClient();
				}
			}
			else
			{
				httpClient = new HttpClient();
			}
			return httpClient;
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x0036B878 File Offset: 0x00369A78
		public static async Task<string> Get(string uri, int timeOutSeconds = 10)
		{
			HttpDownloader.<>c__DisplayClass3_0 CS$<>8__locals1 = new HttpDownloader.<>c__DisplayClass3_0();
			CS$<>8__locals1.str_result = null;
			CS$<>8__locals1.client = HttpDownloader.GetHttpClient();
			CS$<>8__locals1.cts = new CancellationTokenSource(TimeSpan.FromSeconds((double)timeOutSeconds));
			CS$<>8__locals1.http_request = new HttpRequestMessage
			{
				RequestUri = new Uri(uri),
				Method = HttpMethod.Get
			};
			CS$<>8__locals1.task_exc = null;
			CS$<>8__locals1.response = null;
			Task task = Task.Run(delegate
			{
				HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d <<Get>b__0>d;
				<<Get>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Get>b__0>d.<>4__this = CS$<>8__locals1;
				<<Get>b__0>d.<>1__state = -1;
				<<Get>b__0>d.<>t__builder.Start<HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d>(ref <<Get>b__0>d);
				return <<Get>b__0>d.<>t__builder.Task;
			});
			Task task2 = Task.Delay(TimeSpan.FromSeconds((double)timeOutSeconds));
			await Task.WhenAny(new Task[] { task, task2 });
			return CS$<>8__locals1.str_result;
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x0036B8C4 File Offset: 0x00369AC4
		public static async Task<string> Post(string[] uris, HttpContent content, int timeOutSeconds = 10)
		{
			string[] array = uris;
			for (int i = 0; i < array.Length; i++)
			{
				string text = await HttpDownloader.Post(array[i], content, timeOutSeconds);
				if (!string.IsNullOrEmpty(text))
				{
					return text;
				}
			}
			array = null;
			return null;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x0036B918 File Offset: 0x00369B18
		public static async Task<string> Post(string uri, HttpContent content, int timeOutSeconds = 10)
		{
			HttpDownloader.<>c__DisplayClass5_0 CS$<>8__locals1 = new HttpDownloader.<>c__DisplayClass5_0();
			CS$<>8__locals1.str_result = null;
			CS$<>8__locals1.client = HttpDownloader.GetHttpClient();
			CS$<>8__locals1.cts = new CancellationTokenSource(TimeSpan.FromSeconds((double)timeOutSeconds));
			CS$<>8__locals1.http_request = new HttpRequestMessage
			{
				RequestUri = new Uri(uri),
				Method = HttpMethod.Post,
				Content = content
			};
			CS$<>8__locals1.task_exc = null;
			CS$<>8__locals1.response = null;
			Task task = Task.Run(delegate
			{
				HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d <<Post>b__0>d;
				<<Post>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Post>b__0>d.<>4__this = CS$<>8__locals1;
				<<Post>b__0>d.<>1__state = -1;
				<<Post>b__0>d.<>t__builder.Start<HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d>(ref <<Post>b__0>d);
				return <<Post>b__0>d.<>t__builder.Task;
			});
			Task task2 = Task.Delay(TimeSpan.FromSeconds((double)timeOutSeconds));
			await Task.WhenAny(new Task[] { task, task2 });
			return CS$<>8__locals1.str_result;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0036B96C File Offset: 0x00369B6C
		public static async Task<HttpDownloader.Server> CheckPing()
		{
			try
			{
				string text = await HttpDownloader.Get("https://node2.carscanner.info/ping/ping", 4);
				if (text != null && text.Contains("PING!"))
				{
					HttpDownloader.BestServer = HttpDownloader.Server.node2;
					return HttpDownloader.Server.node2;
				}
				string text2 = await HttpDownloader.Get("https://node4.carscanner.info/ping/ping", 4);
				if (text2 != null && text2.Contains("PING!"))
				{
					HttpDownloader.BestServer = HttpDownloader.Server.node4;
					return HttpDownloader.Server.node4;
				}
				string text3 = await HttpDownloader.Get("https://node3.carscanner.info/ping/ping", 4);
				if (text3 != null && text3.Contains("PING!"))
				{
					HttpDownloader.BestServer = HttpDownloader.Server.node3;
					return HttpDownloader.Server.node3;
				}
			}
			catch (Exception)
			{
			}
			return HttpDownloader.Server.Unknown;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00002050 File Offset: 0x00000250
		public HttpDownloader()
		{
		}

		// Token: 0x04002911 RID: 10513
		public static HttpDownloader.Server BestServer;

		// Token: 0x020007D3 RID: 2003
		public enum Server
		{
			// Token: 0x04002913 RID: 10515
			Unknown,
			// Token: 0x04002914 RID: 10516
			node2,
			// Token: 0x04002915 RID: 10517
			node3,
			// Token: 0x04002916 RID: 10518
			node4
		}

		// Token: 0x020007D4 RID: 2004
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060046D3 RID: 18131 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060046D4 RID: 18132 RVA: 0x0036B9A7 File Offset: 0x00369BA7
			internal bool <Get>b__0(string x)
			{
				return x.StartsWith(this.bestServerUri);
			}

			// Token: 0x04002917 RID: 10519
			public string bestServerUri;
		}

		// Token: 0x020007D5 RID: 2005
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060046D5 RID: 18133 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060046D6 RID: 18134 RVA: 0x0036B9B8 File Offset: 0x00369BB8
			internal async Task <Get>b__0()
			{
				try
				{
					HttpResponseMessage httpResponseMessage = await this.client.SendAsync(this.http_request, this.cts.Token).ConfigureAwait(false);
					this.response = httpResponseMessage;
					if (this.response.IsSuccessStatusCode)
					{
						this.str_result = await this.response.Content.ReadAsStringAsync();
					}
				}
				catch (Exception ex)
				{
					this.task_exc = ex;
				}
			}

			// Token: 0x04002918 RID: 10520
			public HttpClient client;

			// Token: 0x04002919 RID: 10521
			public HttpRequestMessage http_request;

			// Token: 0x0400291A RID: 10522
			public CancellationTokenSource cts;

			// Token: 0x0400291B RID: 10523
			public HttpResponseMessage response;

			// Token: 0x0400291C RID: 10524
			public string str_result;

			// Token: 0x0400291D RID: 10525
			public Exception task_exc;

			// Token: 0x020007D6 RID: 2006
			[StructLayout(LayoutKind.Auto)]
			private struct <<Get>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060046D7 RID: 18135 RVA: 0x0036B9FC File Offset: 0x00369BFC
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					HttpDownloader.<>c__DisplayClass3_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<string> taskAwaiter;
							ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter;
							if (num != 0)
							{
								if (num == 1)
								{
									TaskAwaiter<string> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<string>);
									num2 = -1;
									goto IL_010B;
								}
								configuredTaskAwaiter = CS$<>8__locals1.client.SendAsync(CS$<>8__locals1.http_request, CS$<>8__locals1.cts.Token).ConfigureAwait(false).GetAwaiter();
								if (!configuredTaskAwaiter.IsCompleted)
								{
									num2 = 0;
									ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter, HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d>(ref configuredTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter);
								num2 = -1;
							}
							HttpResponseMessage result = configuredTaskAwaiter.GetResult();
							CS$<>8__locals1.response = result;
							if (!CS$<>8__locals1.response.IsSuccessStatusCode)
							{
								goto IL_011C;
							}
							taskAwaiter = CS$<>8__locals1.response.Content.ReadAsStringAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
							IL_010B:
							string result2 = taskAwaiter.GetResult();
							CS$<>8__locals1.str_result = result2;
							IL_011C:;
						}
						catch (Exception ex)
						{
							CS$<>8__locals1.task_exc = ex;
						}
					}
					catch (Exception ex2)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex2);
						return;
					}
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x060046D8 RID: 18136 RVA: 0x0036BB98 File Offset: 0x00369D98
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400291E RID: 10526
				public int <>1__state;

				// Token: 0x0400291F RID: 10527
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04002920 RID: 10528
				public HttpDownloader.<>c__DisplayClass3_0 <>4__this;

				// Token: 0x04002921 RID: 10529
				private ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter <>u__1;

				// Token: 0x04002922 RID: 10530
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020007D7 RID: 2007
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060046D9 RID: 18137 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060046DA RID: 18138 RVA: 0x0036BBA8 File Offset: 0x00369DA8
			internal async Task <Post>b__0()
			{
				try
				{
					HttpResponseMessage httpResponseMessage = await this.client.SendAsync(this.http_request, this.cts.Token).ConfigureAwait(false);
					this.response = httpResponseMessage;
					if (this.response.IsSuccessStatusCode)
					{
						this.str_result = await this.response.Content.ReadAsStringAsync();
					}
				}
				catch (Exception ex)
				{
					this.task_exc = ex;
				}
			}

			// Token: 0x04002923 RID: 10531
			public HttpClient client;

			// Token: 0x04002924 RID: 10532
			public HttpRequestMessage http_request;

			// Token: 0x04002925 RID: 10533
			public CancellationTokenSource cts;

			// Token: 0x04002926 RID: 10534
			public HttpResponseMessage response;

			// Token: 0x04002927 RID: 10535
			public string str_result;

			// Token: 0x04002928 RID: 10536
			public Exception task_exc;

			// Token: 0x020007D8 RID: 2008
			[StructLayout(LayoutKind.Auto)]
			private struct <<Post>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060046DB RID: 18139 RVA: 0x0036BBEC File Offset: 0x00369DEC
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					HttpDownloader.<>c__DisplayClass5_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<string> taskAwaiter;
							ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter;
							if (num != 0)
							{
								if (num == 1)
								{
									TaskAwaiter<string> taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<string>);
									num2 = -1;
									goto IL_010B;
								}
								configuredTaskAwaiter = CS$<>8__locals1.client.SendAsync(CS$<>8__locals1.http_request, CS$<>8__locals1.cts.Token).ConfigureAwait(false).GetAwaiter();
								if (!configuredTaskAwaiter.IsCompleted)
								{
									num2 = 0;
									ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter, HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d>(ref configuredTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter);
								num2 = -1;
							}
							HttpResponseMessage result = configuredTaskAwaiter.GetResult();
							CS$<>8__locals1.response = result;
							if (!CS$<>8__locals1.response.IsSuccessStatusCode)
							{
								goto IL_011C;
							}
							taskAwaiter = CS$<>8__locals1.response.Content.ReadAsStringAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
							IL_010B:
							string result2 = taskAwaiter.GetResult();
							CS$<>8__locals1.str_result = result2;
							IL_011C:;
						}
						catch (Exception ex)
						{
							CS$<>8__locals1.task_exc = ex;
						}
					}
					catch (Exception ex2)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex2);
						return;
					}
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x060046DC RID: 18140 RVA: 0x0036BD88 File Offset: 0x00369F88
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04002929 RID: 10537
				public int <>1__state;

				// Token: 0x0400292A RID: 10538
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x0400292B RID: 10539
				public HttpDownloader.<>c__DisplayClass5_0 <>4__this;

				// Token: 0x0400292C RID: 10540
				private ConfiguredTaskAwaitable<HttpResponseMessage>.ConfiguredTaskAwaiter <>u__1;

				// Token: 0x0400292D RID: 10541
				private TaskAwaiter<string> <>u__2;
			}
		}

		// Token: 0x020007D9 RID: 2009
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckPing>d__8 : IAsyncStateMachine
		{
			// Token: 0x060046DD RID: 18141 RVA: 0x0036BD98 File Offset: 0x00369F98
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HttpDownloader.Server server;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00FB;
						}
						case 2:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0179;
						}
						default:
							taskAwaiter = HttpDownloader.Get("https://node2.carscanner.info/ping/ping", 4).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<CheckPing>d__8>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						string result = taskAwaiter.GetResult();
						if (result != null && result.Contains("PING!"))
						{
							HttpDownloader.BestServer = HttpDownloader.Server.node2;
							server = HttpDownloader.Server.node2;
							goto IL_01C0;
						}
						taskAwaiter = HttpDownloader.Get("https://node4.carscanner.info/ping/ping", 4).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<CheckPing>d__8>(ref taskAwaiter, ref this);
							return;
						}
						IL_00FB:
						string result2 = taskAwaiter.GetResult();
						if (result2 != null && result2.Contains("PING!"))
						{
							HttpDownloader.BestServer = HttpDownloader.Server.node4;
							server = HttpDownloader.Server.node4;
							goto IL_01C0;
						}
						taskAwaiter = HttpDownloader.Get("https://node3.carscanner.info/ping/ping", 4).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<CheckPing>d__8>(ref taskAwaiter, ref this);
							return;
						}
						IL_0179:
						string result3 = taskAwaiter.GetResult();
						if (result3 != null && result3.Contains("PING!"))
						{
							HttpDownloader.BestServer = HttpDownloader.Server.node3;
							server = HttpDownloader.Server.node3;
							goto IL_01C0;
						}
					}
					catch (Exception)
					{
					}
					server = HttpDownloader.Server.Unknown;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01C0:
				num2 = -2;
				this.<>t__builder.SetResult(server);
			}

			// Token: 0x060046DE RID: 18142 RVA: 0x0036BFB0 File Offset: 0x0036A1B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400292E RID: 10542
			public int <>1__state;

			// Token: 0x0400292F RID: 10543
			public AsyncTaskMethodBuilder<HttpDownloader.Server> <>t__builder;

			// Token: 0x04002930 RID: 10544
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020007DA RID: 2010
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DownloadFileAsync>d__1 : IAsyncStateMachine
		{
			// Token: 0x060046DF RID: 18143 RVA: 0x0036BFC0 File Offset: 0x0036A1C0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					if (num > 3)
					{
						_client = HttpDownloader.GetHttpClient();
					}
					try
					{
						try
						{
							TaskAwaiter<HttpResponseMessage> taskAwaiter;
							TaskAwaiter<Stream> taskAwaiter3;
							switch (num)
							{
							case 0:
							{
								TaskAwaiter<HttpResponseMessage> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<HttpResponseMessage>);
								num = (num2 = -1);
								break;
							}
							case 1:
							{
								TaskAwaiter<Stream> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<Stream>);
								num = (num2 = -1);
								goto IL_017F;
							}
							case 2:
							case 3:
								IL_0190:
								try
								{
									TaskAwaiter<int> taskAwaiter5;
									if (num == 2)
									{
										TaskAwaiter<int> taskAwaiter6;
										taskAwaiter5 = taskAwaiter6;
										taskAwaiter6 = default(TaskAwaiter<int>);
										num = (num2 = -1);
										goto IL_023B;
									}
									TaskAwaiter taskAwaiter7;
									if (num == 3)
									{
										TaskAwaiter taskAwaiter8;
										taskAwaiter7 = taskAwaiter8;
										taskAwaiter8 = default(TaskAwaiter);
										num = (num2 = -1);
										goto IL_02C9;
									}
									totalRead = 0L;
									buffer = new byte[bufferSize];
									isMoreDataToRead = true;
									IL_01BF:
									token.ThrowIfCancellationRequested();
									taskAwaiter5 = stream.ReadAsync(buffer, 0, buffer.Length, token).GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 2);
										TaskAwaiter<int> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, HttpDownloader.<DownloadFileAsync>d__1>(ref taskAwaiter5, ref this);
										return;
									}
									IL_023B:
									int result = taskAwaiter5.GetResult();
									read = result;
									if (read == 0)
									{
										isMoreDataToRead = false;
										goto IL_0324;
									}
									taskAwaiter7 = outputStream.WriteAsync(buffer, 0, read).GetAwaiter();
									if (!taskAwaiter7.IsCompleted)
									{
										num = (num2 = 3);
										TaskAwaiter taskAwaiter8 = taskAwaiter7;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HttpDownloader.<DownloadFileAsync>d__1>(ref taskAwaiter7, ref this);
										return;
									}
									IL_02C9:
									taskAwaiter7.GetResult();
									totalRead += (long)read;
									if (canSendProgress)
									{
										progress.Report((double)totalRead * 1.0 / ((double)totalData * 1.0) * 100.0);
									}
									IL_0324:
									if (isMoreDataToRead)
									{
										goto IL_01BF;
									}
									buffer = null;
								}
								finally
								{
									if (num < 0 && stream != null)
									{
										((IDisposable)stream).Dispose();
									}
								}
								stream = null;
								goto IL_035A;
							default:
								taskAwaiter = _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									TaskAwaiter<HttpResponseMessage> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<HttpResponseMessage>, HttpDownloader.<DownloadFileAsync>d__1>(ref taskAwaiter, ref this);
									return;
								}
								break;
							}
							HttpResponseMessage result2 = taskAwaiter.GetResult();
							if (!result2.IsSuccessStatusCode)
							{
								throw new Exception(string.Format("The request returned with HTTP status code {0}", result2.StatusCode));
							}
							HttpContentHeaders headers = result2.Content.Headers;
							if (headers != null)
							{
								ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
								if (contentDisposition != null)
								{
									string fileName = contentDisposition.FileName;
								}
							}
							totalData = result2.Content.Headers.ContentLength.GetValueOrDefault(-1L);
							canSendProgress = totalData != -1L && progress != null;
							taskAwaiter3 = result2.Content.ReadAsStreamAsync().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter<Stream> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, HttpDownloader.<DownloadFileAsync>d__1>(ref taskAwaiter3, ref this);
								return;
							}
							IL_017F:
							Stream result3 = taskAwaiter3.GetResult();
							stream = result3;
							goto IL_0190;
						}
						catch (Exception ex)
						{
							throw ex;
						}
						IL_035A:;
					}
					finally
					{
						if (num < 0 && _client != null)
						{
							((IDisposable)_client).Dispose();
						}
					}
					_client = null;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060046E0 RID: 18144 RVA: 0x0036C3DC File Offset: 0x0036A5DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002931 RID: 10545
			public int <>1__state;

			// Token: 0x04002932 RID: 10546
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002933 RID: 10547
			public string url;

			// Token: 0x04002934 RID: 10548
			public CancellationToken token;

			// Token: 0x04002935 RID: 10549
			public IProgress<double> progress;

			// Token: 0x04002936 RID: 10550
			public int bufferSize;

			// Token: 0x04002937 RID: 10551
			public Stream outputStream;

			// Token: 0x04002938 RID: 10552
			private HttpClient <_client>5__2;

			// Token: 0x04002939 RID: 10553
			private long <totalData>5__3;

			// Token: 0x0400293A RID: 10554
			private bool <canSendProgress>5__4;

			// Token: 0x0400293B RID: 10555
			private TaskAwaiter<HttpResponseMessage> <>u__1;

			// Token: 0x0400293C RID: 10556
			private Stream <stream>5__5;

			// Token: 0x0400293D RID: 10557
			private TaskAwaiter<Stream> <>u__2;

			// Token: 0x0400293E RID: 10558
			private long <totalRead>5__6;

			// Token: 0x0400293F RID: 10559
			private byte[] <buffer>5__7;

			// Token: 0x04002940 RID: 10560
			private bool <isMoreDataToRead>5__8;

			// Token: 0x04002941 RID: 10561
			private int <read>5__9;

			// Token: 0x04002942 RID: 10562
			private TaskAwaiter<int> <>u__3;

			// Token: 0x04002943 RID: 10563
			private TaskAwaiter <>u__4;
		}

		// Token: 0x020007DB RID: 2011
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Get>d__0 : IAsyncStateMachine
		{
			// Token: 0x060046E1 RID: 18145 RVA: 0x0036C3EC File Offset: 0x0036A5EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter<HttpDownloader.Server> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_015F;
						}
						if (!useBestServer)
						{
							goto IL_00D2;
						}
						if (HttpDownloader.BestServer != HttpDownloader.Server.Unknown)
						{
							goto IL_007E;
						}
						taskAwaiter3 = HttpDownloader.CheckPing().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<HttpDownloader.Server> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<HttpDownloader.Server>, HttpDownloader.<Get>d__0>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<HttpDownloader.Server> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<HttpDownloader.Server>);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					IL_007E:
					if (HttpDownloader.BestServer != HttpDownloader.Server.Unknown)
					{
						HttpDownloader.<>c__DisplayClass0_0 CS$<>8__locals1 = new HttpDownloader.<>c__DisplayClass0_0();
						CS$<>8__locals1.bestServerUri = "https://" + HttpDownloader.BestServer.ToString() + ".carscanner.info";
						uris = uris.OrderByDescending((string x) => x.StartsWith(CS$<>8__locals1.bestServerUri)).ToArray<string>();
					}
					IL_00D2:
					array = uris;
					i = 0;
					goto IL_01A3;
					IL_015F:
					string result = taskAwaiter.GetResult();
					if (result != null)
					{
						if (verifyData == null || verifyData(uri, result))
						{
							text = result;
							goto IL_01DA;
						}
						uri = null;
					}
					i++;
					IL_01A3:
					if (i >= array.Length)
					{
						array = null;
						text = null;
					}
					else
					{
						uri = array[i];
						taskAwaiter = HttpDownloader.Get(uri, timeOutSeconds).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<Get>d__0>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_015F;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01DA:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x060046E2 RID: 18146 RVA: 0x0036C604 File Offset: 0x0036A804
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002944 RID: 10564
			public int <>1__state;

			// Token: 0x04002945 RID: 10565
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04002946 RID: 10566
			public bool useBestServer;

			// Token: 0x04002947 RID: 10567
			public string[] uris;

			// Token: 0x04002948 RID: 10568
			public int timeOutSeconds;

			// Token: 0x04002949 RID: 10569
			public Func<string, string, bool> verifyData;

			// Token: 0x0400294A RID: 10570
			private TaskAwaiter<HttpDownloader.Server> <>u__1;

			// Token: 0x0400294B RID: 10571
			private string[] <>7__wrap1;

			// Token: 0x0400294C RID: 10572
			private int <>7__wrap2;

			// Token: 0x0400294D RID: 10573
			private string <uri>5__4;

			// Token: 0x0400294E RID: 10574
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020007DC RID: 2012
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Get>d__3 : IAsyncStateMachine
		{
			// Token: 0x060046E3 RID: 18147 RVA: 0x0036C614 File Offset: 0x0036A814
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string str_result;
				try
				{
					TaskAwaiter<Task> taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new HttpDownloader.<>c__DisplayClass3_0();
						CS$<>8__locals1.str_result = null;
						CS$<>8__locals1.client = HttpDownloader.GetHttpClient();
						CS$<>8__locals1.cts = new CancellationTokenSource(TimeSpan.FromSeconds((double)timeOutSeconds));
						CS$<>8__locals1.http_request = new HttpRequestMessage
						{
							RequestUri = new Uri(uri),
							Method = HttpMethod.Get
						};
						CS$<>8__locals1.task_exc = null;
						CS$<>8__locals1.response = null;
						Task task = Task.Run(delegate
						{
							HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d <<Get>b__0>d;
							<<Get>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Get>b__0>d.<>4__this = CS$<>8__locals1;
							<<Get>b__0>d.<>1__state = -1;
							<<Get>b__0>d.<>t__builder.Start<HttpDownloader.<>c__DisplayClass3_0.<<Get>b__0>d>(ref <<Get>b__0>d);
							return <<Get>b__0>d.<>t__builder.Task;
						});
						Task task2 = Task.Delay(TimeSpan.FromSeconds((double)timeOutSeconds));
						taskAwaiter = Task.WhenAny(new Task[] { task, task2 }).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Task> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, HttpDownloader.<Get>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Task> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Task>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					str_result = CS$<>8__locals1.str_result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(str_result);
			}

			// Token: 0x060046E4 RID: 18148 RVA: 0x0036C7AC File Offset: 0x0036A9AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400294F RID: 10575
			public int <>1__state;

			// Token: 0x04002950 RID: 10576
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04002951 RID: 10577
			public int timeOutSeconds;

			// Token: 0x04002952 RID: 10578
			public string uri;

			// Token: 0x04002953 RID: 10579
			private HttpDownloader.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x04002954 RID: 10580
			private TaskAwaiter<Task> <>u__1;
		}

		// Token: 0x020007DD RID: 2013
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Post>d__4 : IAsyncStateMachine
		{
			// Token: 0x060046E5 RID: 18149 RVA: 0x0036C7BC File Offset: 0x0036A9BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
				try
				{
					if (num != 0)
					{
						array = uris;
						i = 0;
						goto IL_00B0;
					}
					TaskAwaiter<string> taskAwaiter2;
					TaskAwaiter<string> taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<string>);
					num2 = -1;
					IL_008E:
					string result = taskAwaiter.GetResult();
					if (!string.IsNullOrEmpty(result))
					{
						text = result;
						goto IL_00E7;
					}
					i++;
					IL_00B0:
					if (i >= array.Length)
					{
						array = null;
						text = null;
					}
					else
					{
						taskAwaiter = HttpDownloader.Post(array[i], content, timeOutSeconds).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, HttpDownloader.<Post>d__4>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_008E;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00E7:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x060046E6 RID: 18150 RVA: 0x0036C8D4 File Offset: 0x0036AAD4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002955 RID: 10581
			public int <>1__state;

			// Token: 0x04002956 RID: 10582
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04002957 RID: 10583
			public string[] uris;

			// Token: 0x04002958 RID: 10584
			public HttpContent content;

			// Token: 0x04002959 RID: 10585
			public int timeOutSeconds;

			// Token: 0x0400295A RID: 10586
			private string[] <>7__wrap1;

			// Token: 0x0400295B RID: 10587
			private int <>7__wrap2;

			// Token: 0x0400295C RID: 10588
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020007DE RID: 2014
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Post>d__5 : IAsyncStateMachine
		{
			// Token: 0x060046E7 RID: 18151 RVA: 0x0036C8E4 File Offset: 0x0036AAE4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string str_result;
				try
				{
					TaskAwaiter<Task> taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new HttpDownloader.<>c__DisplayClass5_0();
						CS$<>8__locals1.str_result = null;
						CS$<>8__locals1.client = HttpDownloader.GetHttpClient();
						CS$<>8__locals1.cts = new CancellationTokenSource(TimeSpan.FromSeconds((double)timeOutSeconds));
						CS$<>8__locals1.http_request = new HttpRequestMessage
						{
							RequestUri = new Uri(uri),
							Method = HttpMethod.Post,
							Content = content
						};
						CS$<>8__locals1.task_exc = null;
						CS$<>8__locals1.response = null;
						Task task = Task.Run(delegate
						{
							HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d <<Post>b__0>d;
							<<Post>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Post>b__0>d.<>4__this = CS$<>8__locals1;
							<<Post>b__0>d.<>1__state = -1;
							<<Post>b__0>d.<>t__builder.Start<HttpDownloader.<>c__DisplayClass5_0.<<Post>b__0>d>(ref <<Post>b__0>d);
							return <<Post>b__0>d.<>t__builder.Task;
						});
						Task task2 = Task.Delay(TimeSpan.FromSeconds((double)timeOutSeconds));
						taskAwaiter = Task.WhenAny(new Task[] { task, task2 }).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Task> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, HttpDownloader.<Post>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Task> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Task>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					str_result = CS$<>8__locals1.str_result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(str_result);
			}

			// Token: 0x060046E8 RID: 18152 RVA: 0x0036CA88 File Offset: 0x0036AC88
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400295D RID: 10589
			public int <>1__state;

			// Token: 0x0400295E RID: 10590
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400295F RID: 10591
			public int timeOutSeconds;

			// Token: 0x04002960 RID: 10592
			public string uri;

			// Token: 0x04002961 RID: 10593
			public HttpContent content;

			// Token: 0x04002962 RID: 10594
			private HttpDownloader.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x04002963 RID: 10595
			private TaskAwaiter<Task> <>u__1;
		}
	}
}
