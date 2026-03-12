using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B0F RID: 2831
	internal class MQBAudioDataSetMIB3 : MQBCustomizableMode22Coding
	{
		// Token: 0x06005843 RID: 22595 RVA: 0x004219C0 File Offset: 0x0041FBC0
		public MQBAudioDataSetMIB3()
		{
			base.Address = "7201";
			base.Group = CodingGroup.MultimediaSoundQuality;
			base.Name = Translate.GetString("codingDB_SoundProcessingPreset_Name") + " (MIB3)";
			base.Description = Translate.GetString("codingDB_SoundProcessingPreset_Description");
			base.InnerDescription = Translate.GetString("codingDB_SoundProcessingPreset_InnerDescription");
			base.ValueType = AdaptationValueTypes.OptionType;
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F");
			this.Password = "20103";
			base.PreReadCommands = "1003;1040";
			base.PreWriteCommands = "1003;1040;22F1A0;22F1A1;2704;2EF198;2EF199;2EF1A0;2EF1A1;";
			base.PostWriteCommands = "1102;";
			string text = "vag.mib3sound";
			string[] array = (from x in PackageFileReader.GetFilesInDirectory(text)
				orderby x
				select x).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), i + 1), text + "." + array[i]);
				base.Options.Add(mqbadaptationOption);
			}
		}

		// Token: 0x06005844 RID: 22596 RVA: 0x00421AF0 File Offset: 0x0041FCF0
		protected void LoadOptionValue(MQBAdaptationOption opt)
		{
			if (opt.Value.Contains('.'))
			{
				string text = PackageFileReader.ReadFileToString(opt.Value);
				opt.Value = text;
			}
		}

		// Token: 0x06005845 RID: 22597 RVA: 0x00421B20 File Offset: 0x0041FD20
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			foreach (MQBAdaptationOption mqbadaptationOption in base.Options)
			{
				this.LoadOptionValue(mqbadaptationOption);
			}
			if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.DeveloperMode)
			{
				MQBAdaptationOption mqbadaptationOption2 = base.Options.FirstOrDefault<MQBAdaptationOption>();
				if (!base.Options.Any((MQBAdaptationOption x) => x.Title.Contains("Zero")) && mqbadaptationOption2 != null)
				{
					MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption("Zero (no CRC)", new string('0', mqbadaptationOption2.Value.Length));
					byte[] array = new byte[mqbadaptationOption2.Value.Length / 2 - 4];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
					byte[] array2 = Crc32.Calculate(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					MQBAdaptationOption mqbadaptationOption4 = new MQBAdaptationOption("Zero (CRC)", text);
					MQBAdaptationOption mqbadaptationOption5 = new MQBAdaptationOption("FF (no CRC)", new string('F', mqbadaptationOption2.Value.Length));
					array = new byte[mqbadaptationOption2.Value.Length / 2 - 4];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = byte.MaxValue;
					}
					array2 = Crc32.Calculate(array);
					text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					MQBAdaptationOption mqbadaptationOption6 = new MQBAdaptationOption("FF (CRC)", text);
					base.Options.Add(mqbadaptationOption3);
					base.Options.Add(mqbadaptationOption4);
					base.Options.Add(mqbadaptationOption6);
					base.Options.Add(mqbadaptationOption5);
					int length = base.Options.First<MQBAdaptationOption>().Value.Length;
					foreach (MQBAdaptationOption mqbadaptationOption7 in base.Options)
					{
						if (mqbadaptationOption7.Value.Length != length)
						{
							throw new Exception("Wrong length! Option=" + mqbadaptationOption7.Title + "\nValue=\n" + mqbadaptationOption7.Value);
						}
					}
				}
			}
			if (string.IsNullOrEmpty(password))
			{
				password = this.Password;
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
				codingRequestResult = item;
			}
			else
			{
				this.LastReadData = item2;
				string hex = BitHelpers.ByteArrayToHexString(item2);
				MQBAdaptationOption mqbadaptationOption8 = base.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == hex);
				if (mqbadaptationOption8 != null)
				{
					base.CurrentState = mqbadaptationOption8.Title;
				}
				else if (hex.Length >= 8)
				{
					base.CurrentState = new string(hex.TakeLast(8).ToArray<char>());
				}
				else
				{
					base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
				}
				codingRequestResult = item;
			}
			return codingRequestResult;
		}

		// Token: 0x06005846 RID: 22598 RVA: 0x00421B6C File Offset: 0x0041FD6C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (this.LastReadData == null)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				OBDRequest obdrequest = new OBDRequest("010C", false);
				bool engineIsRunning = false;
				obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
					{
						engineIsRunning = true;
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
				await App.OBDReader.WaitForCommandQueue();
				if (engineIsRunning)
				{
					codingRequestResult = CodingRequestResult.WrongConditions;
				}
				else
				{
					string text = "";
					MQBAdaptationOption mqbadaptationOption = base.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == value);
					if (mqbadaptationOption != null)
					{
						text = mqbadaptationOption.Title;
					}
					codingRequestResult = await this.WriteDataToECU(password, text, progress, this.LastReadData, value);
				}
			}
			return codingRequestResult;
		}

		// Token: 0x040036CB RID: 14027
		protected byte[] LastReadData;

		// Token: 0x02000B10 RID: 2832
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005847 RID: 22599 RVA: 0x00421BC8 File Offset: 0x0041FDC8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005848 RID: 22600 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005849 RID: 22601 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <.ctor>b__0_0(string x)
			{
				return x;
			}

			// Token: 0x0600584A RID: 22602 RVA: 0x00421BD4 File Offset: 0x0041FDD4
			internal bool <UpdateCurrentState>b__3_1(MQBAdaptationOption x)
			{
				return x.Title.Contains("Zero");
			}

			// Token: 0x040036CC RID: 14028
			public static readonly MQBAudioDataSetMIB3.<>c <>9 = new MQBAudioDataSetMIB3.<>c();

			// Token: 0x040036CD RID: 14029
			public static Func<string, string> <>9__0_0;

			// Token: 0x040036CE RID: 14030
			public static Func<MQBAdaptationOption, bool> <>9__3_1;
		}

		// Token: 0x02000B11 RID: 2833
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x0600584B RID: 22603 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x0600584C RID: 22604 RVA: 0x00421BE6 File Offset: 0x0041FDE6
			internal bool <UpdateCurrentState>b__0(MQBAdaptationOption x)
			{
				return x.Value == this.hex;
			}

			// Token: 0x040036CF RID: 14031
			public string hex;
		}

		// Token: 0x02000B12 RID: 2834
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x0600584D RID: 22605 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x0600584E RID: 22606 RVA: 0x00421BF9 File Offset: 0x0041FDF9
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x0600584F RID: 22607 RVA: 0x00421C1D File Offset: 0x0041FE1D
			internal bool <Execute>b__1(MQBAdaptationOption x)
			{
				return x.Value == this.value;
			}

			// Token: 0x040036D0 RID: 14032
			public bool engineIsRunning;

			// Token: 0x040036D1 RID: 14033
			public string value;
		}

		// Token: 0x02000B13 RID: 2835
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005850 RID: 22608 RVA: 0x00421C30 File Offset: 0x0041FE30
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAudioDataSetMIB3 mqbaudioDataSetMIB = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_01A5;
						}
						CS$<>8__locals1 = new MQBAudioDataSetMIB3.<>c__DisplayClass4_0();
						CS$<>8__locals1.value = value;
						if (mqbaudioDataSetMIB.LastReadData == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_01CF;
						}
						OBDRequest obdrequest = new OBDRequest("010C", false);
						CS$<>8__locals1.engineIsRunning = false;
						obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
							{
								CS$<>8__locals1.engineIsRunning = true;
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAudioDataSetMIB3.<Execute>d__4>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_01CF;
					}
					string text = "";
					MQBAdaptationOption mqbadaptationOption = mqbaudioDataSetMIB.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.value);
					if (mqbadaptationOption != null)
					{
						text = mqbadaptationOption.Title;
					}
					taskAwaiter = mqbaudioDataSetMIB.WriteDataToECU(password, text, progress, mqbaudioDataSetMIB.LastReadData, CS$<>8__locals1.value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBAudioDataSetMIB3.<Execute>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_01A5:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01CF:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005851 RID: 22609 RVA: 0x00421E44 File Offset: 0x00420044
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036D2 RID: 14034
			public int <>1__state;

			// Token: 0x040036D3 RID: 14035
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036D4 RID: 14036
			public string value;

			// Token: 0x040036D5 RID: 14037
			public MQBAudioDataSetMIB3 <>4__this;

			// Token: 0x040036D6 RID: 14038
			private MQBAudioDataSetMIB3.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x040036D7 RID: 14039
			public string password;

			// Token: 0x040036D8 RID: 14040
			public IProgress<string> progress;

			// Token: 0x040036D9 RID: 14041
			private TaskAwaiter <>u__1;

			// Token: 0x040036DA RID: 14042
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B14 RID: 2836
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005852 RID: 22610 RVA: 0x00421E54 File Offset: 0x00420054
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAudioDataSetMIB3 mqbaudioDataSetMIB = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBAudioDataSetMIB3.<>c__DisplayClass3_0();
						IEnumerator<MQBAdaptationOption> enumerator = mqbaudioDataSetMIB.Options.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MQBAdaptationOption mqbadaptationOption = enumerator.Current;
								mqbaudioDataSetMIB.LoadOptionValue(mqbadaptationOption);
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (SharedSettings.Current.ShowExperimental && SharedSettings.Current.DeveloperMode)
						{
							MQBAdaptationOption mqbadaptationOption2 = mqbaudioDataSetMIB.Options.FirstOrDefault<MQBAdaptationOption>();
							if (!mqbaudioDataSetMIB.Options.Any((MQBAdaptationOption x) => x.Title.Contains("Zero")) && mqbadaptationOption2 != null)
							{
								MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption("Zero (no CRC)", new string('0', mqbadaptationOption2.Value.Length));
								byte[] array = new byte[mqbadaptationOption2.Value.Length / 2 - 4];
								for (int i = 0; i < array.Length; i++)
								{
									array[i] = 0;
								}
								byte[] array2 = Crc32.Calculate(array);
								string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
								MQBAdaptationOption mqbadaptationOption4 = new MQBAdaptationOption("Zero (CRC)", text);
								MQBAdaptationOption mqbadaptationOption5 = new MQBAdaptationOption("FF (no CRC)", new string('F', mqbadaptationOption2.Value.Length));
								array = new byte[mqbadaptationOption2.Value.Length / 2 - 4];
								for (int j = 0; j < array.Length; j++)
								{
									array[j] = byte.MaxValue;
								}
								array2 = Crc32.Calculate(array);
								text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
								MQBAdaptationOption mqbadaptationOption6 = new MQBAdaptationOption("FF (CRC)", text);
								mqbaudioDataSetMIB.Options.Add(mqbadaptationOption3);
								mqbaudioDataSetMIB.Options.Add(mqbadaptationOption4);
								mqbaudioDataSetMIB.Options.Add(mqbadaptationOption6);
								mqbaudioDataSetMIB.Options.Add(mqbadaptationOption5);
								int length = mqbaudioDataSetMIB.Options.First<MQBAdaptationOption>().Value.Length;
								enumerator = mqbaudioDataSetMIB.Options.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										MQBAdaptationOption mqbadaptationOption7 = enumerator.Current;
										if (mqbadaptationOption7.Value.Length != length)
										{
											throw new Exception("Wrong length! Option=" + mqbadaptationOption7.Title + "\nValue=\n" + mqbadaptationOption7.Value);
										}
									}
								}
								finally
								{
									if (num < 0 && enumerator != null)
									{
										enumerator.Dispose();
									}
								}
							}
						}
						if (string.IsNullOrEmpty(password))
						{
							password = mqbaudioDataSetMIB.Password;
						}
						taskAwaiter = mqbaudioDataSetMIB.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAudioDataSetMIB3.<UpdateCurrentState>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num = (num2 = -1);
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						mqbaudioDataSetMIB.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mqbaudioDataSetMIB.LastReadData = item2;
						CS$<>8__locals1.hex = BitHelpers.ByteArrayToHexString(item2);
						MQBAdaptationOption mqbadaptationOption8 = mqbaudioDataSetMIB.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.hex);
						if (mqbadaptationOption8 != null)
						{
							mqbaudioDataSetMIB.CurrentState = mqbadaptationOption8.Title;
						}
						else if (CS$<>8__locals1.hex.Length >= 8)
						{
							mqbaudioDataSetMIB.CurrentState = new string(CS$<>8__locals1.hex.TakeLast(8).ToArray<char>());
						}
						else
						{
							mqbaudioDataSetMIB.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						}
						codingRequestResult = item;
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

			// Token: 0x06005853 RID: 22611 RVA: 0x00422280 File Offset: 0x00420480
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036DB RID: 14043
			public int <>1__state;

			// Token: 0x040036DC RID: 14044
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036DD RID: 14045
			public MQBAudioDataSetMIB3 <>4__this;

			// Token: 0x040036DE RID: 14046
			public string password;

			// Token: 0x040036DF RID: 14047
			private MQBAudioDataSetMIB3.<>c__DisplayClass3_0 <>8__1;

			// Token: 0x040036E0 RID: 14048
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
