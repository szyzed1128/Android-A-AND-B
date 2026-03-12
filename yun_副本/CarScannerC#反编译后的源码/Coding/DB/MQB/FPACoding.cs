using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AD1 RID: 2769
	internal class FPACoding : MQBParametrizeBase
	{
		// Token: 0x060056F4 RID: 22260 RVA: 0x00418258 File Offset: 0x00416458
		public FPACoding()
		{
			base.Group = CodingGroup.EngineAndPowertrain;
			base.ValueType = AdaptationValueTypes.MQBFPAEditor;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704;";
			base.PreWriteCommands = "700:1083;1003;22F1A0;22F1A1;22F1A4;22F198;1040;2704;2EF198;2EF199;31010300030100;31030300;";
			base.AddressFormatLength = 4;
			base.DataLengthFormatLength = 4;
			base.PostWriteCommands = "700:3E80;310102EF030100;310302EF;2EF1A0;2EF1A1;2EF1A4;1102;1102;1003;14FFFFFF;1902AF";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("19");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("19");
			this.RequiresPro = true;
			base.Name = Translate.GetString("codingDB_FPAEditor_Name");
			base.InnerDescription = Translate.GetString("codingDB_FPAEasyCoding_InnerDescription");
			base.Description = Translate.GetString("codingDB_FPAEditor_Description");
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x060056F5 RID: 22261 RVA: 0x00418320 File Offset: 0x00416520
		// (set) Token: 0x060056F6 RID: 22262 RVA: 0x00418328 File Offset: 0x00416528
		public FPAModel Model
		{
			get
			{
				return this._Model;
			}
			set
			{
				this._Model = value;
				base.OnPropertyChanged("Model");
			}
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x0041833C File Offset: 0x0041653C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
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
				this.HasCurrentState = true;
				codingRequestResult2 = codingRequestResult;
			}
			else if (!this.EcuPartAndSW.Contains("3Q0907530"))
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				this.HasCurrentState = true;
				codingRequestResult2 = codingRequestResult;
			}
			else
			{
				if (this.EcuPartAndSW.Trim().Replace("3Q0907530", "").Length >= 2)
				{
					base.Address = 2944;
				}
				else
				{
					base.Address = 9096;
				}
				base.DataLengthFormatLength = 4;
				base.DataLength = 4352;
				this.LoadModel(this.DatasetVersion);
				codingRequestResult2 = CodingRequestResult.Success;
			}
			return codingRequestResult2;
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x060056F8 RID: 22264 RVA: 0x00418387 File Offset: 0x00416587
		// (set) Token: 0x060056F9 RID: 22265 RVA: 0x0041838F File Offset: 0x0041658F
		public bool IsLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<IsLoaded>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<IsLoaded>k__BackingField = value;
			}
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x00418398 File Offset: 0x00416598
		public void LoadModel(string version)
		{
			this.originalBytes = null;
			try
			{
				string text = PackageFileReader.ReadFileToString("vag.fpa3Q0907530." + version);
				this.originalBytes = BitHelpers.ConvertHexToBytesX(text);
				this.Model = new FPAModel(this.originalBytes, this.DatasetVersion);
				base.CurrentState = this.DatasetVersion;
				this.HasCurrentState = true;
				this.IsLoaded = true;
			}
			catch (Exception)
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE + " (" + this.DatasetVersion + ")";
				this.HasCurrentState = true;
				this.IsLoaded = false;
			}
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x00418440 File Offset: 0x00416640
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			try
			{
				string text = BitHelpers.ByteArrayToHexString(this.Model.GetBytes());
				originalData = this.originalBytes;
				codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, text);
			}
			catch (Exception)
			{
				codingRequestResult = CodingRequestResult.UnknownError;
			}
			return codingRequestResult;
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x004184A8 File Offset: 0x004166A8
		[CompilerGenerated]
		private string <UpdateCurrentState>b__8_1(byte[] data, MQBEasyCodingItem coding)
		{
			this.EcuPartAndSW = coding.Device;
			string @string = Encoding.ASCII.GetString(data);
			this.DatasetVersion = @string.Substring(3, 2);
			return this.DatasetVersion;
		}

		// Token: 0x04003592 RID: 13714
		protected string DatasetVersion = "";

		// Token: 0x04003593 RID: 13715
		protected string EcuPartAndSW = "";

		// Token: 0x04003594 RID: 13716
		protected byte[] originalBytes;

		// Token: 0x04003595 RID: 13717
		private FPAModel _Model;

		// Token: 0x04003596 RID: 13718
		[CompilerGenerated]
		private bool <IsLoaded>k__BackingField;

		// Token: 0x02000AD2 RID: 2770
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060056FD RID: 22269 RVA: 0x004184E1 File Offset: 0x004166E1
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060056FE RID: 22270 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060056FF RID: 22271 RVA: 0x00016849 File Offset: 0x00014A49
			internal byte[] <UpdateCurrentState>b__8_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				return data;
			}

			// Token: 0x04003597 RID: 13719
			public static readonly FPACoding.<>c <>9 = new FPACoding.<>c();

			// Token: 0x04003598 RID: 13720
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__8_0;
		}

		// Token: 0x02000AD3 RID: 2771
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__14 : IAsyncStateMachine
		{
			// Token: 0x06005700 RID: 22272 RVA: 0x004184F0 File Offset: 0x004166F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPACoding fpacoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					try
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter;
						if (num != 0)
						{
							string text = BitHelpers.ByteArrayToHexString(fpacoding.Model.GetBytes());
							originalData = fpacoding.originalBytes;
							taskAwaiter = fpacoding.WriteDataToECU(password, UserFriendlyValue, progress, originalData, text).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPACoding.<Execute>d__14>(ref taskAwaiter, ref this);
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
						codingRequestResult = taskAwaiter.GetResult();
					}
					catch (Exception)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
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

			// Token: 0x06005701 RID: 22273 RVA: 0x004185F4 File Offset: 0x004167F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003599 RID: 13721
			public int <>1__state;

			// Token: 0x0400359A RID: 13722
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400359B RID: 13723
			public FPACoding <>4__this;

			// Token: 0x0400359C RID: 13724
			public byte[] originalData;

			// Token: 0x0400359D RID: 13725
			public string password;

			// Token: 0x0400359E RID: 13726
			public string UserFriendlyValue;

			// Token: 0x0400359F RID: 13727
			public IProgress<string> progress;

			// Token: 0x040035A0 RID: 13728
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000AD4 RID: 2772
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__8 : IAsyncStateMachine
		{
			// Token: 0x06005702 RID: 22274 RVA: 0x00418604 File Offset: 0x00416804
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPACoding fpacoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						fpacoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						fpacoding.DatasetVersion = "";
						fpacoding.EcuPartAndSW = "";
						taskAwaiter = new MQBEasyCodingItem("F182", "19", "", "", (byte[] data, string value, MQBEasyCodingItem coding) => data, delegate(byte[] data, MQBEasyCodingItem coding)
						{
							fpacoding.EcuPartAndSW = coding.Device;
							string @string = Encoding.ASCII.GetString(data);
							fpacoding.DatasetVersion = @string.Substring(3, 2);
							return fpacoding.DatasetVersion;
						}).UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPACoding.<UpdateCurrentState>d__8>(ref taskAwaiter, ref this);
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
						fpacoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						fpacoding.HasCurrentState = true;
						codingRequestResult = result;
					}
					else if (!fpacoding.EcuPartAndSW.Contains("3Q0907530"))
					{
						fpacoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						fpacoding.HasCurrentState = true;
						codingRequestResult = result;
					}
					else
					{
						if (fpacoding.EcuPartAndSW.Trim().Replace("3Q0907530", "").Length >= 2)
						{
							fpacoding.Address = 2944;
						}
						else
						{
							fpacoding.Address = 9096;
						}
						fpacoding.DataLengthFormatLength = 4;
						fpacoding.DataLength = 4352;
						fpacoding.LoadModel(fpacoding.DatasetVersion);
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

			// Token: 0x06005703 RID: 22275 RVA: 0x004187DC File Offset: 0x004169DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040035A1 RID: 13729
			public int <>1__state;

			// Token: 0x040035A2 RID: 13730
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040035A3 RID: 13731
			public FPACoding <>4__this;

			// Token: 0x040035A4 RID: 13732
			public IProgress<string> progress;

			// Token: 0x040035A5 RID: 13733
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
