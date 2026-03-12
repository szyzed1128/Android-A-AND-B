using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008A9 RID: 2217
	public class MQBEasyCodingItem : MQBAdaptationTemplate
	{
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06004B56 RID: 19286 RVA: 0x0038334A File Offset: 0x0038154A
		// (set) Token: 0x06004B57 RID: 19287 RVA: 0x00383352 File Offset: 0x00381552
		public Func<byte[], string, MQBEasyCodingItem, byte[]> ChangeDataDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<ChangeDataDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChangeDataDelegate>k__BackingField = value;
			}
		}

		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06004B58 RID: 19288 RVA: 0x0038335B File Offset: 0x0038155B
		// (set) Token: 0x06004B59 RID: 19289 RVA: 0x00383363 File Offset: 0x00381563
		public Func<byte[], MQBEasyCodingItem, string> GetCurrentStateDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<GetCurrentStateDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetCurrentStateDelegate>k__BackingField = value;
			}
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x0038336C File Offset: 0x0038156C
		public MQBEasyCodingItem(CodingGroup Group, string Name, string Description, string Address, string RequestHeader, string ResponseHeader, string Password, string DeviceOrECUItemCode, Func<byte[], string, MQBEasyCodingItem, byte[]> ChangeDataDelegate, Func<byte[], MQBEasyCodingItem, string> GetCurrentStateDelegate, params MQBAdaptationOption[] Options)
		{
			base.Group = Group;
			base.Name = Name;
			base.Description = Description;
			this.Password = Password;
			this.RequiredDeviceOrECUItemCode = DeviceOrECUItemCode;
			this.ChangeDataDelegate = ChangeDataDelegate;
			this.GetCurrentStateDelegate = GetCurrentStateDelegate;
			if (this.GetCurrentStateDelegate == null)
			{
				this.GetCurrentStateDelegate = new Func<byte[], MQBEasyCodingItem, string>(this.DefaultGetStateDelegate);
			}
			base.Options.Clear();
			foreach (MQBAdaptationOption mqbadaptationOption in Options)
			{
				base.Options.Add(mqbadaptationOption);
			}
			base.Address = Address;
			base.ResponseHeader = ResponseHeader;
			base.RequestHeader = RequestHeader;
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00383449 File Offset: 0x00381649
		public string DefaultGetStateDelegate(byte[] data, MQBEasyCodingItem coding)
		{
			if (ArrayHelpers.ArrayEquals<byte>(this.ChangeDataDelegate(data, MQBAdaptationTemplate.EnableOption.Value, this), data))
			{
				return MQBAdaptationTemplate.EnableOption.Title;
			}
			return MQBAdaptationTemplate.DisableOption.Title;
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00383480 File Offset: 0x00381680
		public MQBEasyCodingItem(string Address, string RequestHeader, string ResponseHeader, string Password, string DeviceOrECUItemCode, Func<byte[], string, MQBEasyCodingItem, byte[]> ChangeDataDelegate, Func<byte[], MQBEasyCodingItem, string> GetCurrentStateDelegate)
			: this(CodingGroup.Other, "", "", Address, RequestHeader, ResponseHeader, Password, DeviceOrECUItemCode, ChangeDataDelegate, GetCurrentStateDelegate, Array.Empty<MQBAdaptationOption>())
		{
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x003834B0 File Offset: 0x003816B0
		public MQBEasyCodingItem(string Address, string Unit, string Password, string DeviceOrECUItemCode, Func<byte[], string, MQBEasyCodingItem, byte[]> ChangeDataDelegate, Func<byte[], MQBEasyCodingItem, string> GetCurrentStateDelegate = null)
			: this(CodingGroup.Other, "", "", Address, VagUnitHelper.GetRequestHeaderForMQBUnit(Unit), VagUnitHelper.GetResponseHeaderForMQBUnit(Unit), Password, DeviceOrECUItemCode, ChangeDataDelegate, GetCurrentStateDelegate, Array.Empty<MQBAdaptationOption>())
		{
		}

		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x06004B5E RID: 19294 RVA: 0x003834E7 File Offset: 0x003816E7
		// (set) Token: 0x06004B5F RID: 19295 RVA: 0x003834EF File Offset: 0x003816EF
		public bool SkipOnWrongDevice
		{
			[CompilerGenerated]
			get
			{
				return this.<SkipOnWrongDevice>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SkipOnWrongDevice>k__BackingField = value;
			}
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x06004B60 RID: 19296 RVA: 0x003834F8 File Offset: 0x003816F8
		// (set) Token: 0x06004B61 RID: 19297 RVA: 0x00383500 File Offset: 0x00381700
		public string RequiredDeviceOrECUItemCode
		{
			[CompilerGenerated]
			get
			{
				return this.<RequiredDeviceOrECUItemCode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequiredDeviceOrECUItemCode>k__BackingField = value;
			}
		} = "";

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x06004B62 RID: 19298 RVA: 0x00383509 File Offset: 0x00381709
		// (set) Token: 0x06004B63 RID: 19299 RVA: 0x00383511 File Offset: 0x00381711
		public string ComponentSystem
		{
			[CompilerGenerated]
			get
			{
				return this.<ComponentSystem>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ComponentSystem>k__BackingField = value;
			}
		} = "";

		// Token: 0x06004B64 RID: 19300 RVA: 0x0038351C File Offset: 0x0038171C
		public async Task RequestDeviceIdentsAsync()
		{
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("22F187", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += delegate(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string @string = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder = new StringBuilder(@string.Length);
					foreach (char c in @string)
					{
						if (char.IsLetterOrDigit(c))
						{
							stringBuilder.Append(c);
						}
					}
					this.Device = stringBuilder.ToString().ToUpperInvariant();
				}
			};
			OBDRequest obdrequest2 = new OBDRequest("22F191", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string string2 = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder2 = new StringBuilder(string2.Length);
					foreach (char c2 in string2)
					{
						if (char.IsLetterOrDigit(c2))
						{
							stringBuilder2.Append(c2);
						}
					}
					this.ECU = stringBuilder2.ToString().ToUpperInvariant();
				}
			};
			OBDRequest obdrequest3 = new OBDRequest("22F19E", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string string3 = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder3 = new StringBuilder(string3.Length);
					foreach (char c3 in string3)
					{
						if (char.IsLetterOrDigit(c3) || char.IsPunctuation(c3) || char.IsSymbol(c3))
						{
							stringBuilder3.Append(c3);
						}
					}
					this.ASAM = stringBuilder3.ToString().ToUpperInvariant();
				}
			};
			new OBDRequest("22F197", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					string string4 = Encoding.ASCII.GetString(data);
					StringBuilder stringBuilder4 = new StringBuilder(string4.Length);
					foreach (char c4 in string4)
					{
						if (char.IsLetterOrDigit(c4) || char.IsPunctuation(c4) || char.IsSymbol(c4))
						{
							stringBuilder4.Append(c4);
						}
					}
					this.ASAM = stringBuilder4.ToString().ToUpperInvariant();
				}
			};
			OBDRequest[] array = new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 };
			OBDRequest[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x00383560 File Offset: 0x00381760
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(this.ECU) || string.IsNullOrEmpty(this.Device) || string.IsNullOrEmpty(this.ASAM) || string.IsNullOrEmpty(this.ComponentSystem))
			{
				await this.RequestDeviceIdentsAsync();
			}
			Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
			if (tuple.Item2 != CodingRequestResult.Success && string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(this.Password) && tuple.Item2 == CodingRequestResult.WrongAccessKey)
			{
				tuple = await this.GetCurrentStateRawData(this.Password);
			}
			CodingRequestResult item = tuple.Item2;
			byte[] item2 = tuple.Item1;
			CodingRequestResult codingRequestResult;
			if (item != CodingRequestResult.Success || this.GetCurrentStateDelegate == null)
			{
				base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
				codingRequestResult = item;
			}
			else
			{
				try
				{
					string text = this.GetCurrentStateDelegate(item2, this);
					base.CurrentState = text;
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception)
				{
					string text = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					base.CurrentState = text;
					codingRequestResult = CodingRequestResult.NotSupported;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06004B66 RID: 19302 RVA: 0x003835AB File Offset: 0x003817AB
		// (set) Token: 0x06004B67 RID: 19303 RVA: 0x003835B3 File Offset: 0x003817B3
		public string Device
		{
			[CompilerGenerated]
			get
			{
				return this.<Device>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Device>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06004B68 RID: 19304 RVA: 0x003835BC File Offset: 0x003817BC
		// (set) Token: 0x06004B69 RID: 19305 RVA: 0x003835C4 File Offset: 0x003817C4
		public string ECU
		{
			[CompilerGenerated]
			get
			{
				return this.<ECU>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ECU>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001702 RID: 5890
		// (get) Token: 0x06004B6A RID: 19306 RVA: 0x003835CD File Offset: 0x003817CD
		// (set) Token: 0x06004B6B RID: 19307 RVA: 0x003835D5 File Offset: 0x003817D5
		public string ASAM
		{
			[CompilerGenerated]
			get
			{
				return this.<ASAM>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ASAM>k__BackingField = value;
			}
		} = "";

		// Token: 0x06004B6C RID: 19308 RVA: 0x003835E0 File Offset: 0x003817E0
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (progress != null)
			{
				progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
			}
			await this.RequestDeviceIdentsAsync();
			CodingRequestResult codingRequestResult;
			if (!string.IsNullOrEmpty(this.RequiredDeviceOrECUItemCode) && !this.CheckDevice(this.RequiredDeviceOrECUItemCode))
			{
				codingRequestResult = CodingRequestResult.WrongDevice;
			}
			else
			{
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData("");
				if (tuple.Item2 == CodingRequestResult.WrongAccessKey && !string.IsNullOrEmpty(password))
				{
					tuple = await this.GetCurrentStateRawData(password);
				}
				if (tuple.Item1 != null && tuple.Item2 == CodingRequestResult.Success)
				{
					byte[] array;
					try
					{
						array = this.ChangeDataDelegate(tuple.Item1, value, this);
					}
					catch (CodingException ex)
					{
						if (ex.ExceptionConsequence == ExceptionConsequences.SkipIgnore)
						{
							return CodingRequestResult.Success;
						}
						return CodingRequestResult.InitialDataIncorrect;
					}
					catch (Exception)
					{
						return CodingRequestResult.UnknownError;
					}
					codingRequestResult = await base.Execute(password, BitHelpers.ByteArrayToHexString(array), UserFriendlyValue, progress, tuple.Item1, skipIfTheSameData);
				}
				else
				{
					codingRequestResult = tuple.Item2;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06004B6D RID: 19309 RVA: 0x00383650 File Offset: 0x00381850
		public bool CheckDevice(string device)
		{
			if (string.IsNullOrEmpty(device))
			{
				return true;
			}
			device = "*" + device.ToUpperInvariant() + "*";
			return MQBEasyCodingItem.MatchPattern(this.ECU.ToUpperInvariant(), device) || MQBEasyCodingItem.MatchPattern(this.Device.ToUpperInvariant(), device);
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x003836AC File Offset: 0x003818AC
		internal static bool MatchPattern(string str, string pattern)
		{
			return new Regex("^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(str);
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x003836FC File Offset: 0x003818FC
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__24_0(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string @string = Encoding.ASCII.GetString(data);
				StringBuilder stringBuilder = new StringBuilder(@string.Length);
				foreach (char c in @string)
				{
					if (char.IsLetterOrDigit(c))
					{
						stringBuilder.Append(c);
					}
				}
				this.Device = stringBuilder.ToString().ToUpperInvariant();
			}
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x00383764 File Offset: 0x00381964
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__24_1(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string @string = Encoding.ASCII.GetString(data);
				StringBuilder stringBuilder = new StringBuilder(@string.Length);
				foreach (char c in @string)
				{
					if (char.IsLetterOrDigit(c))
					{
						stringBuilder.Append(c);
					}
				}
				this.ECU = stringBuilder.ToString().ToUpperInvariant();
			}
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x003837CC File Offset: 0x003819CC
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__24_2(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string @string = Encoding.ASCII.GetString(data);
				StringBuilder stringBuilder = new StringBuilder(@string.Length);
				foreach (char c in @string)
				{
					if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
					{
						stringBuilder.Append(c);
					}
				}
				this.ASAM = stringBuilder.ToString().ToUpperInvariant();
			}
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x00383844 File Offset: 0x00381A44
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__24_3(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string @string = Encoding.ASCII.GetString(data);
				StringBuilder stringBuilder = new StringBuilder(@string.Length);
				foreach (char c in @string)
				{
					if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsSymbol(c))
					{
						stringBuilder.Append(c);
					}
				}
				this.ASAM = stringBuilder.ToString().ToUpperInvariant();
			}
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x003838BA File Offset: 0x00381ABA
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__0(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			return base.Execute(password, value, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x04002C0A RID: 11274
		[CompilerGenerated]
		private Func<byte[], string, MQBEasyCodingItem, byte[]> <ChangeDataDelegate>k__BackingField;

		// Token: 0x04002C0B RID: 11275
		[CompilerGenerated]
		private Func<byte[], MQBEasyCodingItem, string> <GetCurrentStateDelegate>k__BackingField;

		// Token: 0x04002C0C RID: 11276
		[CompilerGenerated]
		private bool <SkipOnWrongDevice>k__BackingField;

		// Token: 0x04002C0D RID: 11277
		[CompilerGenerated]
		private string <RequiredDeviceOrECUItemCode>k__BackingField;

		// Token: 0x04002C0E RID: 11278
		[CompilerGenerated]
		private string <ComponentSystem>k__BackingField;

		// Token: 0x04002C0F RID: 11279
		[CompilerGenerated]
		private string <Device>k__BackingField;

		// Token: 0x04002C10 RID: 11280
		[CompilerGenerated]
		private string <ECU>k__BackingField;

		// Token: 0x04002C11 RID: 11281
		[CompilerGenerated]
		private string <ASAM>k__BackingField;

		// Token: 0x020008AA RID: 2218
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__38 : IAsyncStateMachine
		{
			// Token: 0x06004B74 RID: 19316 RVA: 0x003838CC File Offset: 0x00381ACC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBEasyCodingItem mqbeasyCodingItem = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter3;
					TaskAwaiter<CodingRequestResult> taskAwaiter5;
					IProgress<string> progress;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						goto IL_011B;
					}
					case 2:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						goto IL_0196;
					}
					case 3:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_026B;
					}
					default:
						progress = progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
						}
						taskAwaiter = mqbeasyCodingItem.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBEasyCodingItem.<Execute>d__38>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					if (!string.IsNullOrEmpty(mqbeasyCodingItem.RequiredDeviceOrECUItemCode) && !mqbeasyCodingItem.CheckDevice(mqbeasyCodingItem.RequiredDeviceOrECUItemCode))
					{
						codingRequestResult = CodingRequestResult.WrongDevice;
						goto IL_0297;
					}
					taskAwaiter3 = mqbeasyCodingItem.GetCurrentStateRawData("").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBEasyCodingItem.<Execute>d__38>(ref taskAwaiter3, ref this);
						return;
					}
					IL_011B:
					Tuple<byte[], CodingRequestResult> tuple = taskAwaiter3.GetResult();
					if (tuple.Item2 != CodingRequestResult.WrongAccessKey || string.IsNullOrEmpty(password))
					{
						goto IL_019E;
					}
					taskAwaiter3 = mqbeasyCodingItem.GetCurrentStateRawData(password).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBEasyCodingItem.<Execute>d__38>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0196:
					tuple = taskAwaiter3.GetResult();
					IL_019E:
					if (tuple.Item1 == null || tuple.Item2 != CodingRequestResult.Success)
					{
						codingRequestResult = tuple.Item2;
						goto IL_0297;
					}
					byte[] array;
					try
					{
						array = mqbeasyCodingItem.ChangeDataDelegate(tuple.Item1, value, mqbeasyCodingItem);
					}
					catch (CodingException ex)
					{
						if (ex.ExceptionConsequence == ExceptionConsequences.SkipIgnore)
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_0297;
						}
						codingRequestResult = CodingRequestResult.InitialDataIncorrect;
						goto IL_0297;
					}
					catch (Exception)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
						goto IL_0297;
					}
					string text = BitHelpers.ByteArrayToHexString(array);
					taskAwaiter5 = mqbeasyCodingItem.<>n__0(password, text, UserFriendlyValue, progress, tuple.Item1, skipIfTheSameData).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<CodingRequestResult> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBEasyCodingItem.<Execute>d__38>(ref taskAwaiter5, ref this);
						return;
					}
					IL_026B:
					codingRequestResult = taskAwaiter5.GetResult();
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0297:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004B75 RID: 19317 RVA: 0x00383BD0 File Offset: 0x00381DD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C12 RID: 11282
			public int <>1__state;

			// Token: 0x04002C13 RID: 11283
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C14 RID: 11284
			public IProgress<string> progress;

			// Token: 0x04002C15 RID: 11285
			public MQBEasyCodingItem <>4__this;

			// Token: 0x04002C16 RID: 11286
			public string password;

			// Token: 0x04002C17 RID: 11287
			public string value;

			// Token: 0x04002C18 RID: 11288
			public string UserFriendlyValue;

			// Token: 0x04002C19 RID: 11289
			public bool skipIfTheSameData;

			// Token: 0x04002C1A RID: 11290
			private TaskAwaiter <>u__1;

			// Token: 0x04002C1B RID: 11291
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__2;

			// Token: 0x04002C1C RID: 11292
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x020008AB RID: 2219
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestDeviceIdentsAsync>d__24 : IAsyncStateMachine
		{
			// Token: 0x06004B76 RID: 19318 RVA: 0x00383BE0 File Offset: 0x00381DE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBEasyCodingItem mqbeasyCodingItem = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						mqbeasyCodingItem.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("22F187", mqbeasyCodingItem.RequestHeader, mqbeasyCodingItem.BeforeCommands, mqbeasyCodingItem.AfterCommands, false);
						obdrequest.ResponseDecoded += delegate(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string @string = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder = new StringBuilder(@string.Length);
								foreach (char c in @string)
								{
									if (char.IsLetterOrDigit(c))
									{
										stringBuilder.Append(c);
									}
								}
								base.Device = stringBuilder.ToString().ToUpperInvariant();
							}
						};
						OBDRequest obdrequest2 = new OBDRequest("22F191", mqbeasyCodingItem.RequestHeader, mqbeasyCodingItem.BeforeCommands, mqbeasyCodingItem.AfterCommands, false);
						obdrequest2.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string string2 = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder2 = new StringBuilder(string2.Length);
								foreach (char c2 in string2)
								{
									if (char.IsLetterOrDigit(c2))
									{
										stringBuilder2.Append(c2);
									}
								}
								base.ECU = stringBuilder2.ToString().ToUpperInvariant();
							}
						};
						OBDRequest obdrequest3 = new OBDRequest("22F19E", mqbeasyCodingItem.RequestHeader, mqbeasyCodingItem.BeforeCommands, mqbeasyCodingItem.AfterCommands, false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string string3 = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder3 = new StringBuilder(string3.Length);
								foreach (char c3 in string3)
								{
									if (char.IsLetterOrDigit(c3) || char.IsPunctuation(c3) || char.IsSymbol(c3))
									{
										stringBuilder3.Append(c3);
									}
								}
								base.ASAM = stringBuilder3.ToString().ToUpperInvariant();
							}
						};
						new OBDRequest("22F197", mqbeasyCodingItem.RequestHeader, mqbeasyCodingItem.BeforeCommands, mqbeasyCodingItem.AfterCommands, false);
						obdrequest3.ResponseDecoded += delegate(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								string string4 = Encoding.ASCII.GetString(data);
								StringBuilder stringBuilder4 = new StringBuilder(string4.Length);
								foreach (char c4 in string4)
								{
									if (char.IsLetterOrDigit(c4) || char.IsPunctuation(c4) || char.IsSymbol(c4))
									{
										stringBuilder4.Append(c4);
									}
								}
								base.ASAM = stringBuilder4.ToString().ToUpperInvariant();
							}
						};
						OBDRequest[] array = new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 };
						OBDRequest[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							array2[i].ELMFormat = ELMFormat.CAN11bit;
						}
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBEasyCodingItem.<RequestDeviceIdentsAsync>d__24>(ref taskAwaiter, ref this);
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

			// Token: 0x06004B77 RID: 19319 RVA: 0x00383DB8 File Offset: 0x00381FB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C1D RID: 11293
			public int <>1__state;

			// Token: 0x04002C1E RID: 11294
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002C1F RID: 11295
			public MQBEasyCodingItem <>4__this;

			// Token: 0x04002C20 RID: 11296
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008AC RID: 2220
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__25 : IAsyncStateMachine
		{
			// Token: 0x06004B78 RID: 19320 RVA: 0x00383DC8 File Offset: 0x00381FC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBEasyCodingItem mqbeasyCodingItem = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						goto IL_0115;
					}
					case 2:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
						goto IL_01AB;
					}
					default:
						if (!string.IsNullOrEmpty(mqbeasyCodingItem.ECU) && !string.IsNullOrEmpty(mqbeasyCodingItem.Device) && !string.IsNullOrEmpty(mqbeasyCodingItem.ASAM) && !string.IsNullOrEmpty(mqbeasyCodingItem.ComponentSystem))
						{
							goto IL_00B2;
						}
						taskAwaiter = mqbeasyCodingItem.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBEasyCodingItem.<UpdateCurrentState>d__25>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00B2:
					taskAwaiter3 = mqbeasyCodingItem.GetCurrentStateRawData(password).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBEasyCodingItem.<UpdateCurrentState>d__25>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0115:
					Tuple<byte[], CodingRequestResult> tuple = taskAwaiter3.GetResult();
					if (tuple.Item2 == CodingRequestResult.Success || !string.IsNullOrEmpty(password) || string.IsNullOrEmpty(mqbeasyCodingItem.Password) || tuple.Item2 != CodingRequestResult.WrongAccessKey)
					{
						goto IL_01B4;
					}
					taskAwaiter3 = mqbeasyCodingItem.GetCurrentStateRawData(mqbeasyCodingItem.Password).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBEasyCodingItem.<UpdateCurrentState>d__25>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01AB:
					tuple = taskAwaiter3.GetResult();
					IL_01B4:
					CodingRequestResult item = tuple.Item2;
					byte[] item2 = tuple.Item1;
					if (item != CodingRequestResult.Success || mqbeasyCodingItem.GetCurrentStateDelegate == null)
					{
						mqbeasyCodingItem.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						codingRequestResult = item;
					}
					else
					{
						try
						{
							string text = mqbeasyCodingItem.GetCurrentStateDelegate(item2, mqbeasyCodingItem);
							mqbeasyCodingItem.CurrentState = text;
							codingRequestResult = CodingRequestResult.Success;
						}
						catch (Exception)
						{
							string text = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							mqbeasyCodingItem.CurrentState = text;
							codingRequestResult = CodingRequestResult.NotSupported;
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

			// Token: 0x06004B79 RID: 19321 RVA: 0x00384048 File Offset: 0x00382248
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C21 RID: 11297
			public int <>1__state;

			// Token: 0x04002C22 RID: 11298
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C23 RID: 11299
			public MQBEasyCodingItem <>4__this;

			// Token: 0x04002C24 RID: 11300
			public string password;

			// Token: 0x04002C25 RID: 11301
			private TaskAwaiter <>u__1;

			// Token: 0x04002C26 RID: 11302
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__2;
		}
	}
}
