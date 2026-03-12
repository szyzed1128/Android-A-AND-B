using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BAD RID: 2989
	internal class HyundaiReset6AT : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005AC9 RID: 23241 RVA: 0x00435108 File Offset: 0x00433308
		public HyundaiReset6AT()
		{
			base.Name = "Reset automatic transmission adaptation (6/8-speed AT)";
			base.Description = "Compatibility: Hyundai/Kia with 6-speed Automatic Transmission (A6MF1/2/3, A6GF1/2/3, A6LF1/2/3). Compatibility depends on installed transmission control unit.\nRequired conditions: ignition: ON, engine NOT started.";
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптации АКПП (6-ступенчатая АКПП)", "Совместимость: 6АКПП Hyundai/Kia (A6MF1/2/3, A6GF1/2/3, A6LF1/2/3). Совместимость зависит от установленного блока управления АКПП, поэтому данная функция работает не на всех автомобилях.\nУсловия для выполнения: зажигание включено, двигатель не запущен!", SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7E1";
			base.ResponseHeader = "";
			this.BeforeCommands = "";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "Сбросить", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			this.HasCurrentState = false;
			base.InnerDescription = SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Group = CodingGroup.EngineAndPowertrain;
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x004351EF File Offset: 0x004333EF
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = "";
			this.AfterCommands = "20";
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x00435208 File Offset: 0x00433408
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = "";
			this.HasCurrentState = false;
			this.BuildDefaultBeforeAndAfterCommands();
			bool gen1_supported = false;
			bool gen2_supported = false;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("21A0", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("61A0"))
				{
					gen1_supported = true;
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("2201A0", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("6201A0"))
				{
					gen2_supported = true;
				}
				semaphore.Release();
			};
			base.CurrentState = "";
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
			await semaphore.WaitAsync();
			CodingRequestResult codingRequestResult;
			if (!gen1_supported && !gen2_supported)
			{
				base.CurrentState = Translate.GetString("coding_NotSupported");
				this.HasCurrentState = true;
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				this.HasCurrentState = false;
				if (gen1_supported)
				{
					base.CurrentState = "gen1";
				}
				if (gen2_supported)
				{
					base.CurrentState = "gen2";
				}
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x0043524C File Offset: 0x0043344C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			CodingRequestResult codingRequestResult = await this.UpdateCurrentState("", null);
			codingResult = codingRequestResult;
			CodingRequestResult codingRequestResult2;
			if (codingResult != CodingRequestResult.Success)
			{
				codingRequestResult2 = codingResult;
			}
			else
			{
				OBDRequest obdrequest = new OBDRequest("30E600", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data.Contains("70E6") || data.Contains("037F7078"))
					{
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_DataAccepted"));
						}
						codingResult = CodingRequestResult.Success;
					}
					else
					{
						IProgress<string> progress3 = progress;
						if (progress3 != null)
						{
							progress3.Report(Translate.GetString("coding_progress_DataRejected"));
						}
						if (data == null)
						{
							codingResult = CodingRequestResult.UnknownError;
						}
						else
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 12 && data.Contains("7F30"))
							{
								int num = data.IndexOf("7F30");
								int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
								if (num2 >= 128 || num2 == 34)
								{
									codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num2 == 51)
								{
									codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num2 == 49)
								{
									codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									codingResult = CodingRequestResult.UnknownError;
								}
							}
							else
							{
								codingResult = CodingRequestResult.UnknownError;
							}
						}
					}
					semaphore.Release();
				};
				OBDRequest obdrequest2 = new OBDRequest("2FF03101", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
				{
					if (data.Contains("6FF031") || data.Contains("037F2F78"))
					{
						IProgress<string> progress4 = progress;
						if (progress4 != null)
						{
							progress4.Report(Translate.GetString("coding_progress_DataAccepted"));
						}
						codingResult = CodingRequestResult.Success;
					}
					else
					{
						IProgress<string> progress5 = progress;
						if (progress5 != null)
						{
							progress5.Report(Translate.GetString("coding_progress_DataRejected"));
						}
						if (data == null)
						{
							codingResult = CodingRequestResult.UnknownError;
						}
						else
						{
							data = OBDDataReader.FilterHexAndNewLineOnly(data);
							if (data.Length >= 12 && data.Contains("7F2F"))
							{
								int num3 = data.IndexOf("7F2F");
								int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
								if (num4 >= 128 || num4 == 34)
								{
									codingResult = CodingRequestResult.WrongConditions;
								}
								else if (num4 == 51)
								{
									codingResult = CodingRequestResult.WrongAccessKey;
								}
								else if (num4 == 49)
								{
									codingResult = CodingRequestResult.NotSupported;
								}
								else
								{
									codingResult = CodingRequestResult.UnknownError;
								}
							}
							else
							{
								codingResult = CodingRequestResult.UnknownError;
							}
						}
					}
					semaphore.Release();
				};
				OBDRequest req_openSession1003 = new OBDRequest("1003", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				OBDRequest obdrequest3 = new OBDRequest("1090", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				req_openSession1003.ResponseMarker = "50";
				obdrequest3.ResponseMarker = "50";
				obdrequest3.ResponseReceived += delegate(OBDRequest req2, string data2)
				{
					if (data2 != null && data2.Contains("NO DATA"))
					{
						App.OBDReader.InsertRequestInQueue(req_openSession1003);
					}
				};
				if (base.CurrentState == "gen1")
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[] { req_openSession1003, obdrequest3, obdrequest });
				}
				else if (base.CurrentState == "gen2")
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[] { req_openSession1003, obdrequest3, obdrequest2 });
				}
				await semaphore.WaitAsync();
				codingRequestResult2 = codingResult;
			}
			return codingRequestResult2;
		}

		// Token: 0x0400392E RID: 14638
		private const string GEN1 = "gen1";

		// Token: 0x0400392F RID: 14639
		private const string GEN2 = "gen2";

		// Token: 0x02000BAE RID: 2990
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06005ACD RID: 23245 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06005ACE RID: 23246 RVA: 0x00435298 File Offset: 0x00433498
			internal void <UpdateCurrentState>b__0(OBDRequest request, string data)
			{
				if (data.Contains("61A0"))
				{
					this.gen1_supported = true;
				}
			}

			// Token: 0x06005ACF RID: 23247 RVA: 0x004352AE File Offset: 0x004334AE
			internal void <UpdateCurrentState>b__1(OBDRequest request, string data)
			{
				if (data.Contains("6201A0"))
				{
					this.gen2_supported = true;
				}
				this.semaphore.Release();
			}

			// Token: 0x04003930 RID: 14640
			public bool gen1_supported;

			// Token: 0x04003931 RID: 14641
			public bool gen2_supported;

			// Token: 0x04003932 RID: 14642
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000BAF RID: 2991
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06005AD0 RID: 23248 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06005AD1 RID: 23249 RVA: 0x004352D0 File Offset: 0x004334D0
			internal void <Execute>b__0(OBDRequest request, string data)
			{
				if (data.Contains("70E6") || data.Contains("037F7078"))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 12 && data.Contains("7F30"))
						{
							int num = data.IndexOf("7F30");
							int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								this.codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								this.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								this.codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							this.codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x06005AD2 RID: 23250 RVA: 0x004353D8 File Offset: 0x004335D8
			internal void <Execute>b__1(OBDRequest request, string data)
			{
				if (data.Contains("6FF031") || data.Contains("037F2F78"))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 12 && data.Contains("7F2F"))
						{
							int num = data.IndexOf("7F2F");
							int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								this.codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								this.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								this.codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							this.codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				this.semaphore.Release();
			}

			// Token: 0x06005AD3 RID: 23251 RVA: 0x004354DF File Offset: 0x004336DF
			internal void <Execute>b__2(OBDRequest req2, string data2)
			{
				if (data2 != null && data2.Contains("NO DATA"))
				{
					App.OBDReader.InsertRequestInQueue(this.req_openSession1003);
				}
			}

			// Token: 0x04003933 RID: 14643
			public IProgress<string> progress;

			// Token: 0x04003934 RID: 14644
			public CodingRequestResult codingResult;

			// Token: 0x04003935 RID: 14645
			public SemaphoreSlim semaphore;

			// Token: 0x04003936 RID: 14646
			public OBDRequest req_openSession1003;
		}

		// Token: 0x02000BB0 RID: 2992
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005AD4 RID: 23252 RVA: 0x00435504 File Offset: 0x00433704
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiReset6AT hyundaiReset6AT = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_02C1;
						}
						CS$<>8__locals1 = new HyundaiReset6AT.<>c__DisplayClass5_0();
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						taskAwaiter3 = hyundaiReset6AT.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, HyundaiReset6AT.<Execute>d__5>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					CodingRequestResult result = taskAwaiter3.GetResult();
					CS$<>8__locals1.codingResult = result;
					if (CS$<>8__locals1.codingResult != CodingRequestResult.Success)
					{
						codingRequestResult = CS$<>8__locals1.codingResult;
						goto IL_02F6;
					}
					OBDRequest obdrequest = new OBDRequest("30E600", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false)
					{
						ELMFormat = ELMFormat.CAN11bit
					};
					obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data.Contains("70E6") || data.Contains("037F7078"))
						{
							IProgress<string> progress = CS$<>8__locals1.progress;
							if (progress != null)
							{
								progress.Report(Translate.GetString("coding_progress_DataAccepted"));
							}
							CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						}
						else
						{
							IProgress<string> progress2 = CS$<>8__locals1.progress;
							if (progress2 != null)
							{
								progress2.Report(Translate.GetString("coding_progress_DataRejected"));
							}
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
							}
							else
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (data.Length >= 12 && data.Contains("7F30"))
								{
									int num3 = data.IndexOf("7F30");
									int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
									if (num4 >= 128 || num4 == 34)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
									}
									else if (num4 == 51)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									}
									else if (num4 == 49)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
									}
									else
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									}
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
							}
						}
						CS$<>8__locals1.semaphore.Release();
					};
					OBDRequest obdrequest2 = new OBDRequest("2FF03101", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false)
					{
						ELMFormat = ELMFormat.CAN11bit
					};
					obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if (data.Contains("6FF031") || data.Contains("037F2F78"))
						{
							IProgress<string> progress3 = CS$<>8__locals1.progress;
							if (progress3 != null)
							{
								progress3.Report(Translate.GetString("coding_progress_DataAccepted"));
							}
							CS$<>8__locals1.codingResult = CodingRequestResult.Success;
						}
						else
						{
							IProgress<string> progress4 = CS$<>8__locals1.progress;
							if (progress4 != null)
							{
								progress4.Report(Translate.GetString("coding_progress_DataRejected"));
							}
							if (data == null)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
							}
							else
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (data.Length >= 12 && data.Contains("7F2F"))
								{
									int num5 = data.IndexOf("7F2F");
									int num6 = int.Parse(data.Substring(num5 + 4, 2), NumberStyles.HexNumber);
									if (num6 >= 128 || num6 == 34)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
									}
									else if (num6 == 51)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
									}
									else if (num6 == 49)
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
									}
									else
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									}
								}
								else
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
							}
						}
						CS$<>8__locals1.semaphore.Release();
					};
					CS$<>8__locals1.req_openSession1003 = new OBDRequest("1003", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false)
					{
						ELMFormat = ELMFormat.CAN11bit
					};
					OBDRequest obdrequest3 = new OBDRequest("1090", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false)
					{
						ELMFormat = ELMFormat.CAN11bit
					};
					CS$<>8__locals1.req_openSession1003.ResponseMarker = "50";
					obdrequest3.ResponseMarker = "50";
					obdrequest3.ResponseReceived += delegate(OBDRequest req2, string data2)
					{
						if (data2 != null && data2.Contains("NO DATA"))
						{
							App.OBDReader.InsertRequestInQueue(CS$<>8__locals1.req_openSession1003);
						}
					};
					if (hyundaiReset6AT.CurrentState == "gen1")
					{
						App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.req_openSession1003, obdrequest3, obdrequest });
					}
					else if (hyundaiReset6AT.CurrentState == "gen2")
					{
						App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals1.req_openSession1003, obdrequest3, obdrequest2 });
					}
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiReset6AT.<Execute>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_02C1:
					taskAwaiter.GetResult();
					codingRequestResult = CS$<>8__locals1.codingResult;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02F6:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005AD5 RID: 23253 RVA: 0x00435840 File Offset: 0x00433A40
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003937 RID: 14647
			public int <>1__state;

			// Token: 0x04003938 RID: 14648
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003939 RID: 14649
			public IProgress<string> progress;

			// Token: 0x0400393A RID: 14650
			public HyundaiReset6AT <>4__this;

			// Token: 0x0400393B RID: 14651
			private HyundaiReset6AT.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x0400393C RID: 14652
			private TaskAwaiter<CodingRequestResult> <>u__1;

			// Token: 0x0400393D RID: 14653
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000BB1 RID: 2993
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005AD6 RID: 23254 RVA: 0x00435850 File Offset: 0x00433A50
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				HyundaiReset6AT hyundaiReset6AT = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new HyundaiReset6AT.<>c__DisplayClass4_0();
						hyundaiReset6AT.CurrentState = "";
						hyundaiReset6AT.HasCurrentState = false;
						hyundaiReset6AT.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.gen1_supported = false;
						CS$<>8__locals1.gen2_supported = false;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("21A0", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("61A0"))
							{
								CS$<>8__locals1.gen1_supported = true;
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("2201A0", hyundaiReset6AT.RequestHeader, hyundaiReset6AT.BeforeCommands, hyundaiReset6AT.AfterCommands, false);
						obdrequest2.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (data.Contains("6201A0"))
							{
								CS$<>8__locals1.gen2_supported = true;
							}
							CS$<>8__locals1.semaphore.Release();
						};
						hyundaiReset6AT.CurrentState = "";
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, HyundaiReset6AT.<UpdateCurrentState>d__4>(ref taskAwaiter, ref this);
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
					if (!CS$<>8__locals1.gen1_supported && !CS$<>8__locals1.gen2_supported)
					{
						hyundaiReset6AT.CurrentState = Translate.GetString("coding_NotSupported");
						hyundaiReset6AT.HasCurrentState = true;
						codingRequestResult = CodingRequestResult.NotSupported;
					}
					else
					{
						hyundaiReset6AT.HasCurrentState = false;
						if (CS$<>8__locals1.gen1_supported)
						{
							hyundaiReset6AT.CurrentState = "gen1";
						}
						if (CS$<>8__locals1.gen2_supported)
						{
							hyundaiReset6AT.CurrentState = "gen2";
						}
						codingRequestResult = CodingRequestResult.Success;
					}
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
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005AD7 RID: 23255 RVA: 0x00435A80 File Offset: 0x00433C80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400393E RID: 14654
			public int <>1__state;

			// Token: 0x0400393F RID: 14655
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003940 RID: 14656
			public HyundaiReset6AT <>4__this;

			// Token: 0x04003941 RID: 14657
			private HyundaiReset6AT.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x04003942 RID: 14658
			private TaskAwaiter <>u__1;
		}
	}
}
