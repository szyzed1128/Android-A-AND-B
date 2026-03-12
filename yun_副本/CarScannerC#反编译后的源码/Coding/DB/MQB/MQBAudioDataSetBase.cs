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
	// Token: 0x02000B09 RID: 2825
	internal class MQBAudioDataSetBase : MQBParametrizeBase
	{
		// Token: 0x06005831 RID: 22577 RVA: 0x0042128C File Offset: 0x0041F48C
		public MQBAudioDataSetBase(string path)
		{
			base.Group = CodingGroup.MultimediaSoundQuality;
			base.ValueType = AdaptationValueTypes.OptionType;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704;";
			base.PreWriteCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;";
			base.PostWriteCommands = "700:3E80;37;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF;1902AF";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F");
			this.RequiresPro = true;
			string[] array = (from x in PackageFileReader.GetFilesInDirectory(path)
				orderby x
				select x).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), i + 1), path + "." + array[i]);
				base.Options.Add(mqbadaptationOption);
			}
			this.allowWhileEngineRunning = true;
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x00421384 File Offset: 0x0041F584
		public override uint DatasetUploadMaxBlockSize
		{
			get
			{
				if (SharedSettings.Current.DatasetUploadMaxBlockSize <= 255)
				{
					return (uint)SharedSettings.Current.DatasetUploadMaxBlockSize;
				}
				if (SharedSettings.Current.CodingLastPlatformSelected == "MLB-EVO-A4B9")
				{
					return 255U;
				}
				return (uint)SharedSettings.Current.DatasetUploadMaxBlockSize;
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x004213D4 File Offset: 0x0041F5D4
		protected void LoadOptionValue(MQBAdaptationOption opt)
		{
			if (opt.Value.Contains('.'))
			{
				string text = PackageFileReader.ReadFileToString(opt.Value);
				opt.Value = text;
			}
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06005834 RID: 22580 RVA: 0x00421403 File Offset: 0x0041F603
		// (set) Token: 0x06005835 RID: 22581 RVA: 0x0042140F File Offset: 0x0041F60F
		public override string ATST
		{
			get
			{
				return SharedSettings.Current.GetATST();
			}
			set
			{
				base.ATST = value;
			}
		}

		// Token: 0x06005836 RID: 22582 RVA: 0x00421418 File Offset: 0x0041F618
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult codingRequestResult;
			if (!SharedSettings.Current.DatasetAllowed)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				foreach (MQBAdaptationOption mqbadaptationOption in base.Options)
				{
					this.LoadOptionValue(mqbadaptationOption);
				}
				this.LastReadData = null;
				if (string.IsNullOrEmpty(password))
				{
					password = this.Password;
				}
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password, progress);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					base.CurrentState = Translate.GetString("coding_DatasetEmptyOrNotSupported");
					codingRequestResult = item;
				}
				else
				{
					this.LastReadData = item2;
					if (item2 != null && item2.Length >= 4)
					{
						string hex = BitHelpers.ByteArrayToHexString(item2);
						MQBAdaptationOption mqbadaptationOption2 = base.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == hex);
						if (mqbadaptationOption2 != null)
						{
							base.CurrentState = mqbadaptationOption2.Title;
						}
						else
						{
							string text = item2[item2.Length - 2].ToString("X2") + item2[item2.Length - 1].ToString("X2");
							char c = (char)item2[item2.Length - 4];
							string text2 = c.ToString();
							c = (char)item2[item2.Length - 3];
							base.CurrentState = text2 + c.ToString() + " / " + text;
						}
					}
					codingRequestResult = item;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06005837 RID: 22583 RVA: 0x0042146C File Offset: 0x0041F66C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (!SharedSettings.Current.DatasetAllowed)
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				if (!this.allowWhileEngineRunning)
				{
					MQBAudioDataSetBase.<>c__DisplayClass11_0 CS$<>8__locals1 = new MQBAudioDataSetBase.<>c__DisplayClass11_0();
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
					await App.OBDReader.WaitForCommandQueue();
					if (CS$<>8__locals1.engineIsRunning)
					{
						return CodingRequestResult.WrongConditions;
					}
					CS$<>8__locals1 = null;
				}
				if (!this.allowExecuteWithEmptyOriginalData && this.LastReadData == null)
				{
					codingRequestResult = CodingRequestResult.NotSupported;
				}
				else if (skipIfTheSameData && this.LastReadData != null && BitHelpers.ByteArrayToHexString(this.LastReadData) == value)
				{
					codingRequestResult = CodingRequestResult.Success;
				}
				else
				{
					codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, this.LastReadData, value);
				}
			}
			return codingRequestResult;
		}

		// Token: 0x040036B3 RID: 14003
		protected byte[] LastReadData;

		// Token: 0x040036B4 RID: 14004
		protected bool allowWhileEngineRunning;

		// Token: 0x040036B5 RID: 14005
		protected bool allowExecuteWithEmptyOriginalData = true;

		// Token: 0x02000B0A RID: 2826
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005838 RID: 22584 RVA: 0x004214D9 File Offset: 0x0041F6D9
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005839 RID: 22585 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600583A RID: 22586 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <.ctor>b__0_0(string x)
			{
				return x;
			}

			// Token: 0x040036B6 RID: 14006
			public static readonly MQBAudioDataSetBase.<>c <>9 = new MQBAudioDataSetBase.<>c();

			// Token: 0x040036B7 RID: 14007
			public static Func<string, string> <>9__0_0;
		}

		// Token: 0x02000B0B RID: 2827
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x0600583B RID: 22587 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x0600583C RID: 22588 RVA: 0x004214E5 File Offset: 0x0041F6E5
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x040036B8 RID: 14008
			public bool engineIsRunning;
		}

		// Token: 0x02000B0C RID: 2828
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x0600583D RID: 22589 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x0600583E RID: 22590 RVA: 0x00421509 File Offset: 0x0041F709
			internal bool <UpdateCurrentState>b__0(MQBAdaptationOption x)
			{
				return x.Value == this.hex;
			}

			// Token: 0x040036B9 RID: 14009
			public string hex;
		}

		// Token: 0x02000B0D RID: 2829
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__11 : IAsyncStateMachine
		{
			// Token: 0x0600583F RID: 22591 RVA: 0x0042151C File Offset: 0x0041F71C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAudioDataSetBase mqbaudioDataSetBase = this;
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
							goto IL_01C9;
						}
						if (!SharedSettings.Current.DatasetAllowed)
						{
							mqbaudioDataSetBase.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_01EC;
						}
						if (mqbaudioDataSetBase.allowWhileEngineRunning)
						{
							goto IL_0110;
						}
						CS$<>8__locals1 = new MQBAudioDataSetBase.<>c__DisplayClass11_0();
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAudioDataSetBase.<Execute>d__11>(ref taskAwaiter3, ref this);
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
						goto IL_01EC;
					}
					CS$<>8__locals1 = null;
					IL_0110:
					if (!mqbaudioDataSetBase.allowExecuteWithEmptyOriginalData && mqbaudioDataSetBase.LastReadData == null)
					{
						codingRequestResult = CodingRequestResult.NotSupported;
						goto IL_01EC;
					}
					if (skipIfTheSameData && mqbaudioDataSetBase.LastReadData != null && BitHelpers.ByteArrayToHexString(mqbaudioDataSetBase.LastReadData) == value)
					{
						codingRequestResult = CodingRequestResult.Success;
						goto IL_01EC;
					}
					taskAwaiter = mqbaudioDataSetBase.WriteDataToECU(password, UserFriendlyValue, progress, mqbaudioDataSetBase.LastReadData, value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBAudioDataSetBase.<Execute>d__11>(ref taskAwaiter, ref this);
						return;
					}
					IL_01C9:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01EC:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005840 RID: 22592 RVA: 0x00421748 File Offset: 0x0041F948
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036BA RID: 14010
			public int <>1__state;

			// Token: 0x040036BB RID: 14011
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036BC RID: 14012
			public MQBAudioDataSetBase <>4__this;

			// Token: 0x040036BD RID: 14013
			private MQBAudioDataSetBase.<>c__DisplayClass11_0 <>8__1;

			// Token: 0x040036BE RID: 14014
			public bool skipIfTheSameData;

			// Token: 0x040036BF RID: 14015
			public string value;

			// Token: 0x040036C0 RID: 14016
			public string password;

			// Token: 0x040036C1 RID: 14017
			public string UserFriendlyValue;

			// Token: 0x040036C2 RID: 14018
			public IProgress<string> progress;

			// Token: 0x040036C3 RID: 14019
			private TaskAwaiter <>u__1;

			// Token: 0x040036C4 RID: 14020
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000B0E RID: 2830
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__8 : IAsyncStateMachine
		{
			// Token: 0x06005841 RID: 22593 RVA: 0x00421758 File Offset: 0x0041F958
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAudioDataSetBase mqbaudioDataSetBase = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (!SharedSettings.Current.DatasetAllowed)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_0201;
						}
						IEnumerator<MQBAdaptationOption> enumerator = mqbaudioDataSetBase.Options.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MQBAdaptationOption mqbadaptationOption = enumerator.Current;
								mqbaudioDataSetBase.LoadOptionValue(mqbadaptationOption);
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						mqbaudioDataSetBase.LastReadData = null;
						if (string.IsNullOrEmpty(password))
						{
							password = mqbaudioDataSetBase.Password;
						}
						taskAwaiter = mqbaudioDataSetBase.GetCurrentStateRawData(password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAudioDataSetBase.<UpdateCurrentState>d__8>(ref taskAwaiter, ref this);
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
						mqbaudioDataSetBase.CurrentState = Translate.GetString("coding_DatasetEmptyOrNotSupported");
						codingRequestResult = item;
					}
					else
					{
						mqbaudioDataSetBase.LastReadData = item2;
						if (item2 != null && item2.Length >= 4)
						{
							MQBAudioDataSetBase.<>c__DisplayClass8_0 CS$<>8__locals1 = new MQBAudioDataSetBase.<>c__DisplayClass8_0();
							CS$<>8__locals1.hex = BitHelpers.ByteArrayToHexString(item2);
							MQBAdaptationOption mqbadaptationOption2 = mqbaudioDataSetBase.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.hex);
							if (mqbadaptationOption2 != null)
							{
								mqbaudioDataSetBase.CurrentState = mqbadaptationOption2.Title;
							}
							else
							{
								string text = item2[item2.Length - 2].ToString("X2") + item2[item2.Length - 1].ToString("X2");
								char c = (char)item2[item2.Length - 4];
								string text2 = c.ToString();
								c = (char)item2[item2.Length - 3];
								string text3 = text2 + c.ToString();
								mqbaudioDataSetBase.CurrentState = text3 + " / " + text;
							}
						}
						codingRequestResult = item;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0201:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005842 RID: 22594 RVA: 0x004219B0 File Offset: 0x0041FBB0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040036C5 RID: 14021
			public int <>1__state;

			// Token: 0x040036C6 RID: 14022
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040036C7 RID: 14023
			public MQBAudioDataSetBase <>4__this;

			// Token: 0x040036C8 RID: 14024
			public string password;

			// Token: 0x040036C9 RID: 14025
			public IProgress<string> progress;

			// Token: 0x040036CA RID: 14026
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
