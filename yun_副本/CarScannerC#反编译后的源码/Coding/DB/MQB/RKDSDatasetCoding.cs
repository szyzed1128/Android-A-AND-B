using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using CarScannerXamarinForms.Common;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B3D RID: 2877
	internal class RKDSDatasetCoding : MQBParametrizeBase
	{
		// Token: 0x06005929 RID: 22825 RVA: 0x00429158 File Offset: 0x00427358
		public RKDSDatasetCoding()
		{
			base.Group = CodingGroup.TPMS;
			base.Name = Translate.GetString("codingDB_VagRKDS_Name");
			base.Description = Translate.GetString("codingDB_VagRKDS_Description");
			base.InnerDescription = Translate.GetString("codingDB_VagRKDS_InnerDescription");
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("65");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("65");
			base.Address = 8259584;
			base.DataLength = 2048;
			base.DataLengthFormatLength = 2;
			base.AddressFormatLength = 4;
			base.ValueType = AdaptationValueTypes.MQBRKDSGenerator;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704;22F182";
			base.PreWriteCommands = "700:1083;1003;1040;22F1A0;22F1A1;22F1A4;22F182;2704;2EF198;2EF199;31010300030100;31030300;";
			base.PostWriteCommands = "310102EF030100;310302EF;2EF1A0;2EF1A1;2EF1A4;1102;1003;14FFFFFF;";
			this.Model = new RKDSGenerator();
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x0600592A RID: 22826 RVA: 0x00429231 File Offset: 0x00427431
		// (set) Token: 0x0600592B RID: 22827 RVA: 0x00429239 File Offset: 0x00427439
		public RKDSGenerator Model
		{
			[CompilerGenerated]
			get
			{
				return this.<Model>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Model>k__BackingField = value;
			}
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x00429244 File Offset: 0x00427444
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.ecu = null;
			this.EcuPartAndSW = "";
			CodingRequestResult codingRequestResult = await new MQBEasyCodingItem("F182", "65", "", "", (byte[] data, string value, MQBEasyCodingItem coding) => data, delegate(byte[] data, MQBEasyCodingItem coding)
			{
				this.EcuPartAndSW = coding.Device;
				return "";
			}).UpdateCurrentState("", progress);
			CodingRequestResult codingRequestResult2;
			if (codingRequestResult != CodingRequestResult.Success)
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				this.HasCurrentState = true;
				codingRequestResult2 = codingRequestResult;
			}
			else
			{
				RKDSEcu rkdsecu = RKDSEcu.GetECUs().FirstOrDefault((RKDSEcu x) => x.Name == this.EcuPartAndSW);
				if (rkdsecu == null)
				{
					base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					this.HasCurrentState = true;
					codingRequestResult2 = codingRequestResult;
				}
				else
				{
					this.ecu = rkdsecu;
					codingRequestResult2 = CodingRequestResult.Success;
				}
			}
			return codingRequestResult2;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x00429290 File Offset: 0x00427490
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (this.ecu == null)
			{
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				string text = value;
				if (string.IsNullOrEmpty(value))
				{
					text = BitHelpers.ByteArrayToHexString(this.Model.Generate(this.ecu, this.Model.Tiresets.ToArray<RKDSTireset>()));
				}
				codingRequestResult = await base.Execute(password, text, UserFriendlyValue, progress, null, skipIfTheSameData);
			}
			return codingRequestResult;
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x00429300 File Offset: 0x00427500
		public async Task<string> PickFileAsync()
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null)
				{
					using (Stream stream = await fileResult.OpenReadAsync())
					{
						using (StreamReader streamReader = new StreamReader(stream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x0600592F RID: 22831 RVA: 0x0042933C File Offset: 0x0042753C
		public string ODISXmlToHex(string input)
		{
			try
			{
				foreach (XElement xelement in XDocument.Parse(input).Elements().First((XElement x) => x.Name.ToString().ToUpperInvariant() == "MESSAGE")
					.Element(XName.Get("RESULT"))
					.Element(XName.Get("RESPONSE"))
					.Element(XName.Get("DATA"))
					.Elements(XName.Get("PARAMETER_DATA")))
				{
					string value = xelement.Attribute(XName.Get("START_ADDRESS")).Value;
					string text = xelement.Value.Replace("0x", "").Replace(",", "").Replace(" ", "")
						.Replace("\r", "")
						.Replace("\n", "")
						.Trim();
					if (text.Length / 2 == 2048)
					{
						return text;
					}
				}
			}
			catch (Exception)
			{
				throw new ArgumentException();
			}
			return null;
		}

		// Token: 0x06005930 RID: 22832 RVA: 0x0042949C File Offset: 0x0042769C
		public string VCPXmlToHex(string input)
		{
			try
			{
				string text = XDocument.Parse(input).Elements().First((XElement x) => x.Name.ToString().ToUpperInvariant() == "ZDC")
					.Element(XName.Get("DATENBEREICHE"))
					.Element(XName.Get("DATENBEREICH"))
					.Element(XName.Get("DATEN"))
					.Value.Replace("0x", "").Replace(",", "").Replace(" ", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Trim();
				if (text.Length / 2 == 2048)
				{
					return text;
				}
			}
			catch (Exception)
			{
				throw new ArgumentException();
			}
			return null;
		}

		// Token: 0x06005931 RID: 22833 RVA: 0x0042958C File Offset: 0x0042778C
		[CompilerGenerated]
		private string <UpdateCurrentState>b__7_1(byte[] data, MQBEasyCodingItem coding)
		{
			this.EcuPartAndSW = coding.Device;
			return "";
		}

		// Token: 0x06005932 RID: 22834 RVA: 0x0042959F File Offset: 0x0042779F
		[CompilerGenerated]
		private bool <UpdateCurrentState>b__7_2(RKDSEcu x)
		{
			return x.Name == this.EcuPartAndSW;
		}

		// Token: 0x06005933 RID: 22835 RVA: 0x004295B2 File Offset: 0x004277B2
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__0(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return base.Execute(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x040037AC RID: 14252
		[CompilerGenerated]
		private RKDSGenerator <Model>k__BackingField;

		// Token: 0x040037AD RID: 14253
		private RKDSEcu ecu;

		// Token: 0x040037AE RID: 14254
		protected string EcuPartAndSW = "";

		// Token: 0x02000B3E RID: 2878
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005934 RID: 22836 RVA: 0x004295C3 File Offset: 0x004277C3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005935 RID: 22837 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005936 RID: 22838 RVA: 0x00016849 File Offset: 0x00014A49
			internal byte[] <UpdateCurrentState>b__7_0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				return data;
			}

			// Token: 0x06005937 RID: 22839 RVA: 0x004295CF File Offset: 0x004277CF
			internal bool <ODISXmlToHex>b__10_0(XElement x)
			{
				return x.Name.ToString().ToUpperInvariant() == "MESSAGE";
			}

			// Token: 0x06005938 RID: 22840 RVA: 0x004295EB File Offset: 0x004277EB
			internal bool <VCPXmlToHex>b__11_0(XElement x)
			{
				return x.Name.ToString().ToUpperInvariant() == "ZDC";
			}

			// Token: 0x040037AF RID: 14255
			public static readonly RKDSDatasetCoding.<>c <>9 = new RKDSDatasetCoding.<>c();

			// Token: 0x040037B0 RID: 14256
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__7_0;

			// Token: 0x040037B1 RID: 14257
			public static Func<XElement, bool> <>9__10_0;

			// Token: 0x040037B2 RID: 14258
			public static Func<XElement, bool> <>9__11_0;
		}

		// Token: 0x02000B3F RID: 2879
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__8 : IAsyncStateMachine
		{
			// Token: 0x06005939 RID: 22841 RVA: 0x00429608 File Offset: 0x00427808
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RKDSDatasetCoding rkdsdatasetCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (rkdsdatasetCoding.ecu == null)
						{
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_00F0;
						}
						string text = value;
						if (string.IsNullOrEmpty(value))
						{
							text = BitHelpers.ByteArrayToHexString(rkdsdatasetCoding.Model.Generate(rkdsdatasetCoding.ecu, rkdsdatasetCoding.Model.Tiresets.ToArray<RKDSTireset>()));
						}
						taskAwaiter = rkdsdatasetCoding.<>n__0(password, text, UserFriendlyValue, progress, null, skipIfTheSameData).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, RKDSDatasetCoding.<Execute>d__8>(ref taskAwaiter, ref this);
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
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00F0:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600593A RID: 22842 RVA: 0x0042972C File Offset: 0x0042792C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037B3 RID: 14259
			public int <>1__state;

			// Token: 0x040037B4 RID: 14260
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037B5 RID: 14261
			public RKDSDatasetCoding <>4__this;

			// Token: 0x040037B6 RID: 14262
			public string value;

			// Token: 0x040037B7 RID: 14263
			public string password;

			// Token: 0x040037B8 RID: 14264
			public string UserFriendlyValue;

			// Token: 0x040037B9 RID: 14265
			public IProgress<string> progress;

			// Token: 0x040037BA RID: 14266
			public bool skipIfTheSameData;

			// Token: 0x040037BB RID: 14267
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B40 RID: 2880
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PickFileAsync>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600593B RID: 22843 RVA: 0x0042973C File Offset: 0x0042793C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
				try
				{
					try
					{
						TaskAwaiter<Stream> taskAwaiter;
						TaskAwaiter<FileResult> taskAwaiter3;
						if (num != 0)
						{
							if (num == 1)
							{
								TaskAwaiter<Stream> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<Stream>);
								num = (num2 = -1);
								goto IL_00CF;
							}
							taskAwaiter3 = FilePicker.PickAsync(null).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<FileResult> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, RKDSDatasetCoding.<PickFileAsync>d__9>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<FileResult> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<FileResult>);
							num = (num2 = -1);
						}
						FileResult result = taskAwaiter3.GetResult();
						if (result == null)
						{
							goto IL_010B;
						}
						taskAwaiter = result.OpenReadAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter<Stream> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Stream>, RKDSDatasetCoding.<PickFileAsync>d__9>(ref taskAwaiter, ref this);
							return;
						}
						IL_00CF:
						Stream result2 = taskAwaiter.GetResult();
						try
						{
							StreamReader streamReader = new StreamReader(result2);
							try
							{
								text = streamReader.ReadToEnd();
								goto IL_012D;
							}
							finally
							{
								if (num < 0 && streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && result2 != null)
							{
								((IDisposable)result2).Dispose();
							}
						}
						IL_010B:;
					}
					catch (Exception)
					{
					}
					text = null;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_012D:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600593C RID: 22844 RVA: 0x004298F0 File Offset: 0x00427AF0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037BC RID: 14268
			public int <>1__state;

			// Token: 0x040037BD RID: 14269
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x040037BE RID: 14270
			private TaskAwaiter<FileResult> <>u__1;

			// Token: 0x040037BF RID: 14271
			private TaskAwaiter<Stream> <>u__2;
		}

		// Token: 0x02000B41 RID: 2881
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600593D RID: 22845 RVA: 0x00429900 File Offset: 0x00427B00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RKDSDatasetCoding rkdsdatasetCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						rkdsdatasetCoding.ecu = null;
						rkdsdatasetCoding.EcuPartAndSW = "";
						taskAwaiter = new MQBEasyCodingItem("F182", "65", "", "", (byte[] data, string value, MQBEasyCodingItem coding) => data, delegate(byte[] data, MQBEasyCodingItem coding)
						{
							rkdsdatasetCoding.EcuPartAndSW = coding.Device;
							return "";
						}).UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, RKDSDatasetCoding.<UpdateCurrentState>d__7>(ref taskAwaiter, ref this);
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
						rkdsdatasetCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						rkdsdatasetCoding.HasCurrentState = true;
						codingRequestResult = result;
					}
					else
					{
						RKDSEcu rkdsecu = RKDSEcu.GetECUs().FirstOrDefault((RKDSEcu x) => x.Name == rkdsdatasetCoding.EcuPartAndSW);
						if (rkdsecu == null)
						{
							rkdsdatasetCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							rkdsdatasetCoding.HasCurrentState = true;
							codingRequestResult = result;
						}
						else
						{
							rkdsdatasetCoding.ecu = rkdsecu;
							codingRequestResult = CodingRequestResult.Success;
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
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600593E RID: 22846 RVA: 0x00429A80 File Offset: 0x00427C80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040037C0 RID: 14272
			public int <>1__state;

			// Token: 0x040037C1 RID: 14273
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040037C2 RID: 14274
			public RKDSDatasetCoding <>4__this;

			// Token: 0x040037C3 RID: 14275
			public IProgress<string> progress;

			// Token: 0x040037C4 RID: 14276
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
