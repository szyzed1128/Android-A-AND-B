using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000ADA RID: 2778
	internal class FPAUserFriendlyCoding : FPACoding
	{
		// Token: 0x0600573F RID: 22335 RVA: 0x004196C0 File Offset: 0x004178C0
		public FPAUserFriendlyCoding()
		{
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Clear();
			base.Name = Translate.GetString("codingDB_FPAEasyCoding_Name");
			base.Description = Translate.GetString("codingDB_FPAEasyCoding_Description");
			base.InnerDescription = Translate.GetString("codingDB_FPAEasyCoding_InnerDescription");
		}

		// Token: 0x06005740 RID: 22336 RVA: 0x00419718 File Offset: 0x00417918
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.Options.Clear();
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			this.HasCurrentState = true;
			this.DatasetVersion = "";
			this.EcuPartAndSW = "";
			CodingRequestResult codingRequestResult = await new MQBEasyCodingItem("F182", "19", "", "", (byte[] data, string value, MQBEasyCodingItem coding) => data, delegate(byte[] data, MQBEasyCodingItem coding)
			{
				this.EcuPartAndSW = coding.Device;
				string @string = Encoding.ASCII.GetString(data);
				this.DatasetVersion = @string.Substring(3, 2);
				return this.DatasetVersion;
			}).UpdateCurrentState("", progress);
			CodingRequestResult codingRequestResult2;
			if (codingRequestResult != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult2 = codingRequestResult;
			}
			else if (!this.EcuPartAndSW.Contains("3Q0907530"))
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult2 = codingRequestResult;
			}
			else
			{
				string text = this.EcuPartAndSW.Trim().Replace("3Q0907530", "");
				if (text.Length >= 2)
				{
					base.Address = 2944;
				}
				else
				{
					base.Address = 9096;
				}
				base.DataLengthFormatLength = 4;
				base.DataLength = 4352;
				try
				{
					string text2 = PackageFileReader.ReadFileToString("vag.fpa3Q0907530." + this.DatasetVersion);
					this.originalBytes = BitHelpers.ConvertHexToBytesX(text2);
					base.Model = new FPAModel(this.originalBytes, this.DatasetVersion);
					this.HasCurrentState = false;
					base.CurrentState = "Ver.: " + this.DatasetVersion;
					MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title + " (" + this.DatasetVersion + ")", text2);
					MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (" + this.DatasetVersion + ")", this.MakeEnableOptionValue());
					MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption(Translate.GetString("coding_KeyOnOffReset"), "1102");
					base.Options.Clear();
					base.Options.Add(mqbadaptationOption2);
					base.Options.Add(mqbadaptationOption);
					base.Options.Add(mqbadaptationOption3);
				}
				catch (Exception)
				{
					this.HasCurrentState = true;
					base.CurrentState = string.Concat(new string[]
					{
						MQBAdaptationTemplate.UNSUPPORTED_TITLE,
						" (Ver.: ",
						this.DatasetVersion,
						" / ",
						text,
						")"
					});
				}
				codingRequestResult2 = CodingRequestResult.Success;
			}
			return codingRequestResult2;
		}

		// Token: 0x06005741 RID: 22337 RVA: 0x00419764 File Offset: 0x00417964
		private string MakeEnableOptionValue()
		{
			foreach (FPAProfile fpaprofile in base.Model.Profiles)
			{
				switch (fpaprofile.Value)
				{
				case 1:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 10:
				case 11:
					fpaprofile.ReturnAfterRestart = fpaprofile.Value;
					break;
				}
			}
			foreach (FPAControl fpacontrol in base.Model.Controls)
			{
				if (fpacontrol.ControlType > 0 && fpacontrol.ControlType <= 55)
				{
					fpacontrol.SaveOnRestart = true;
				}
			}
			return BitHelpers.ByteArrayToHexString(base.Model.GetBytes());
		}

		// Token: 0x06005742 RID: 22338 RVA: 0x0041986C File Offset: 0x00417A6C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (value == "1102")
			{
				this.BuildDefaultBeforeAndAfterCommands();
				OBDRequest obdrequest = new OBDRequest("1102", "710", this.BeforeCommands, this.AfterCommands, false);
				App.OBDReader.ReplaceQueue(obdrequest);
				await App.OBDReader.WaitForCommandQueue();
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				originalData = this.originalBytes;
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value);
			}
			return codingRequestResult;
		}

		// Token: 0x06005743 RID: 22339 RVA: 0x004198DC File Offset: 0x00417ADC
		[CompilerGenerated]
		private string <UpdateCurrentState>b__1_1(byte[] data, MQBEasyCodingItem coding)
		{
			this.EcuPartAndSW = coding.Device;
			string @string = Encoding.ASCII.GetString(data);
			this.DatasetVersion = @string.Substring(3, 2);
			return this.DatasetVersion;
		}

		// Token: 0x02000ADB RID: 2779
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005744 RID: 22340 RVA: 0x00419915 File Offset: 0x00417B15
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005745 RID: 22341 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005746 RID: 22342 RVA: 0x00016849 File Offset: 0x00014A49
			internal byte[] <UpdateCurrentState>b__1_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				return data;
			}

			// Token: 0x040035B7 RID: 13751
			public static readonly FPAUserFriendlyCoding.<>c <>9 = new FPAUserFriendlyCoding.<>c();

			// Token: 0x040035B8 RID: 13752
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;
		}

		// Token: 0x02000ADC RID: 2780
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005747 RID: 22343 RVA: 0x00419924 File Offset: 0x00417B24
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAUserFriendlyCoding fpauserFriendlyCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter3;
						if (num != 1)
						{
							if (value == "1102")
							{
								fpauserFriendlyCoding.BuildDefaultBeforeAndAfterCommands();
								OBDRequest obdrequest = new OBDRequest("1102", "710", fpauserFriendlyCoding.BeforeCommands, fpauserFriendlyCoding.AfterCommands, false);
								App.OBDReader.ReplaceQueue(obdrequest);
								taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAUserFriendlyCoding.<Execute>d__3>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00B9;
							}
							else
							{
								originalData = fpauserFriendlyCoding.originalBytes;
								taskAwaiter3 = fpauserFriendlyCoding.WriteDataToECU(password, UserFriendlyValue, progress, originalData, value).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter<CodingRequestResult> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPAUserFriendlyCoding.<Execute>d__3>(ref taskAwaiter3, ref this);
									return;
								}
							}
						}
						else
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
						}
						codingRequestResult = taskAwaiter3.GetResult();
						goto IL_0168;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00B9:
					taskAwaiter.GetResult();
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0168:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005748 RID: 22344 RVA: 0x00419ACC File Offset: 0x00417CCC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040035B9 RID: 13753
			public int <>1__state;

			// Token: 0x040035BA RID: 13754
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040035BB RID: 13755
			public string value;

			// Token: 0x040035BC RID: 13756
			public FPAUserFriendlyCoding <>4__this;

			// Token: 0x040035BD RID: 13757
			public byte[] originalData;

			// Token: 0x040035BE RID: 13758
			public string password;

			// Token: 0x040035BF RID: 13759
			public string UserFriendlyValue;

			// Token: 0x040035C0 RID: 13760
			public IProgress<string> progress;

			// Token: 0x040035C1 RID: 13761
			private TaskAwaiter <>u__1;

			// Token: 0x040035C2 RID: 13762
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000ADD RID: 2781
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005749 RID: 22345 RVA: 0x00419ADC File Offset: 0x00417CDC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAUserFriendlyCoding fpauserFriendlyCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						fpauserFriendlyCoding.Options.Clear();
						fpauserFriendlyCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						fpauserFriendlyCoding.HasCurrentState = true;
						fpauserFriendlyCoding.DatasetVersion = "";
						fpauserFriendlyCoding.EcuPartAndSW = "";
						taskAwaiter = new MQBEasyCodingItem("F182", "19", "", "", (byte[] data, string value, MQBEasyCodingItem coding) => data, delegate(byte[] data, MQBEasyCodingItem coding)
						{
							fpauserFriendlyCoding.EcuPartAndSW = coding.Device;
							string @string = Encoding.ASCII.GetString(data);
							fpauserFriendlyCoding.DatasetVersion = @string.Substring(3, 2);
							return fpauserFriendlyCoding.DatasetVersion;
						}).UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPAUserFriendlyCoding.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					CodingRequestResult result = taskAwaiter.GetResult();
					if (result != CodingRequestResult.Success)
					{
						fpauserFriendlyCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						codingRequestResult = result;
					}
					else if (!fpauserFriendlyCoding.EcuPartAndSW.Contains("3Q0907530"))
					{
						fpauserFriendlyCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						codingRequestResult = result;
					}
					else
					{
						string text = fpauserFriendlyCoding.EcuPartAndSW.Trim().Replace("3Q0907530", "");
						if (text.Length >= 2)
						{
							fpauserFriendlyCoding.Address = 2944;
						}
						else
						{
							fpauserFriendlyCoding.Address = 9096;
						}
						fpauserFriendlyCoding.DataLengthFormatLength = 4;
						fpauserFriendlyCoding.DataLength = 4352;
						try
						{
							string text2 = PackageFileReader.ReadFileToString("vag.fpa3Q0907530." + fpauserFriendlyCoding.DatasetVersion);
							fpauserFriendlyCoding.originalBytes = BitHelpers.ConvertHexToBytesX(text2);
							fpauserFriendlyCoding.Model = new FPAModel(fpauserFriendlyCoding.originalBytes, fpauserFriendlyCoding.DatasetVersion);
							fpauserFriendlyCoding.HasCurrentState = false;
							fpauserFriendlyCoding.CurrentState = "Ver.: " + fpauserFriendlyCoding.DatasetVersion;
							MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title + " (" + fpauserFriendlyCoding.DatasetVersion + ")", text2);
							MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (" + fpauserFriendlyCoding.DatasetVersion + ")", fpauserFriendlyCoding.MakeEnableOptionValue());
							MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption(Translate.GetString("coding_KeyOnOffReset"), "1102");
							fpauserFriendlyCoding.Options.Clear();
							fpauserFriendlyCoding.Options.Add(mqbadaptationOption2);
							fpauserFriendlyCoding.Options.Add(mqbadaptationOption);
							fpauserFriendlyCoding.Options.Add(mqbadaptationOption3);
						}
						catch (Exception)
						{
							fpauserFriendlyCoding.HasCurrentState = true;
							fpauserFriendlyCoding.CurrentState = string.Concat(new string[]
							{
								MQBAdaptationTemplate.UNSUPPORTED_TITLE,
								" (Ver.: ",
								fpauserFriendlyCoding.DatasetVersion,
								" / ",
								text,
								")"
							});
						}
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600574A RID: 22346 RVA: 0x00419E0C File Offset: 0x0041800C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040035C3 RID: 13763
			public int <>1__state;

			// Token: 0x040035C4 RID: 13764
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040035C5 RID: 13765
			public FPAUserFriendlyCoding <>4__this;

			// Token: 0x040035C6 RID: 13766
			public IProgress<string> progress;

			// Token: 0x040035C7 RID: 13767
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
