using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000896 RID: 2198
	internal class MQBAlternativeCoding : MQBAdaptationTemplate
	{
		// Token: 0x06004AE3 RID: 19171 RVA: 0x00380D4C File Offset: 0x0037EF4C
		public MQBAlternativeCoding(CodingGroup Group, string Name, string Description, string RequestHeader, string ResponseHeader, string Password, params MQBAdaptationOption[] Options)
		{
			base.Group = Group;
			base.Name = Name;
			base.Description = Description;
			this.Password = Password;
			base.Options.Clear();
			if (Options != null && Options.Length != 0)
			{
				foreach (MQBAdaptationOption mqbadaptationOption in Options)
				{
					base.Options.Add(mqbadaptationOption);
				}
			}
			else
			{
				base.Options.Add(MQBAdaptationTemplate.EnableOption);
				base.Options.Add(MQBAdaptationTemplate.DisableOption);
			}
			base.ResponseHeader = ResponseHeader;
			base.RequestHeader = RequestHeader;
		}

		// Token: 0x170016E5 RID: 5861
		// (get) Token: 0x06004AE4 RID: 19172 RVA: 0x00380E17 File Offset: 0x0037F017
		// (set) Token: 0x06004AE5 RID: 19173 RVA: 0x00380E1F File Offset: 0x0037F01F
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

		// Token: 0x170016E6 RID: 5862
		// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x00380E28 File Offset: 0x0037F028
		// (set) Token: 0x06004AE7 RID: 19175 RVA: 0x00380E30 File Offset: 0x0037F030
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

		// Token: 0x170016E7 RID: 5863
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x00380E39 File Offset: 0x0037F039
		// (set) Token: 0x06004AE9 RID: 19177 RVA: 0x00380E41 File Offset: 0x0037F041
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

		// Token: 0x06004AEA RID: 19178 RVA: 0x00380E4C File Offset: 0x0037F04C
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
			OBDRequest[] array = new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 };
			OBDRequest[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x06004AEB RID: 19179 RVA: 0x00380E8F File Offset: 0x0037F08F
		public MQBAlternativeCoding(CodingGroup Group, string Name, string Description, string RequestHeader, string ResponseHeader, string Password, params MQBAlternativeContainer[] Alternatives)
			: this(Group, Name, Description, RequestHeader, ResponseHeader, Password, new MQBAdaptationOption[0])
		{
			this.Alternatives.AddRange(Alternatives);
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00380EB3 File Offset: 0x0037F0B3
		public MQBAlternativeCoding(string RequestHeader, string ResponseHeader, string Password, params MQBAlternativeContainer[] Alternatives)
			: this(CodingGroup.Other, "", "", RequestHeader, ResponseHeader, Password, Alternatives)
		{
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00380ECB File Offset: 0x0037F0CB
		public MQBAlternativeCoding(string RequestHeader, string ResponseHeader, params MQBAlternativeContainer[] Alternatives)
			: this(CodingGroup.Other, "", "", RequestHeader, ResponseHeader, "", Alternatives)
		{
		}

		// Token: 0x06004AEE RID: 19182 RVA: 0x00380EE6 File Offset: 0x0037F0E6
		public MQBAlternativeCoding(string Unit, params MQBAlternativeContainer[] Alternatives)
			: this(CodingGroup.Other, "", "", VagUnitHelper.GetRequestHeaderForMQBUnit(Unit), VagUnitHelper.GetResponseHeaderForMQBUnit(Unit), "", Alternatives)
		{
		}

		// Token: 0x06004AEF RID: 19183 RVA: 0x00380F0C File Offset: 0x0037F10C
		public static MQBAlternativeCoding BuildAltCodingSwitchForUnit09(CodingGroup Group, string Name, string Description, string NewGenAddress, int NewGenByte, int NewGenBit, string OldGenAddress, int OldGenByte, int OldGenBit, params TranslationItem[] translations)
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, NewGenAddress, "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, NewGenByte, NewGenBit, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, NewGenByte, NewGenBit, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, OldGenAddress, "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, OldGenByte, OldGenBit, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, OldGenByte, OldGenBit, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(Group, Name, Description, "70E", "778", "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			if (string.IsNullOrEmpty(NewGenAddress))
			{
				mqbalternativeCoding.Alternatives.Remove(mqbalternativeContainerForUnit);
			}
			if (string.IsNullOrEmpty(OldGenAddress))
			{
				mqbalternativeCoding.Alternatives.Remove(mqbalternativeContainerForUnit2);
			}
			if (translations != null)
			{
				foreach (TranslationItem translationItem in translations)
				{
					mqbalternativeCoding.Translations.Add(translationItem);
				}
			}
			return mqbalternativeCoding;
		}

		// Token: 0x170016E8 RID: 5864
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x00380FF5 File Offset: 0x0037F1F5
		// (set) Token: 0x06004AF1 RID: 19185 RVA: 0x00380FFD File Offset: 0x0037F1FD
		public CodingRequestResult CodingResultWhenNoSuitableAlternativeFound
		{
			[CompilerGenerated]
			get
			{
				return this.<CodingResultWhenNoSuitableAlternativeFound>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CodingResultWhenNoSuitableAlternativeFound>k__BackingField = value;
			}
		} = CodingRequestResult.NotSupported;

		// Token: 0x170016E9 RID: 5865
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x00381006 File Offset: 0x0037F206
		// (set) Token: 0x06004AF3 RID: 19187 RVA: 0x0038100E File Offset: 0x0037F20E
		public List<MQBAlternativeContainer> Alternatives
		{
			[CompilerGenerated]
			get
			{
				return this.<Alternatives>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Alternatives>k__BackingField = value;
			}
		} = new List<MQBAlternativeContainer>();

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x00381017 File Offset: 0x0037F217
		// (set) Token: 0x06004AF5 RID: 19189 RVA: 0x0038101F File Offset: 0x0037F21F
		public MQBAlternativeContainer CurrentContainer
		{
			get
			{
				return this._CurrentContainer;
			}
			set
			{
				this._CurrentContainer = value;
				base.OnPropertyChanged("CurrentContainer");
				if (!string.IsNullOrEmpty(value.Password))
				{
					this.Password = value.Password;
				}
			}
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x0038104C File Offset: 0x0037F24C
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (string.IsNullOrEmpty(this.ECU) || string.IsNullOrEmpty(this.Device) || string.IsNullOrEmpty(this.ASAM))
			{
				await this.RequestDeviceIdentsAsync();
			}
			foreach (MQBAlternativeContainer alt in this.Alternatives)
			{
				TaskAwaiter<bool> taskAwaiter = alt.CheckIsSupported(base.RequestHeader, base.ResponseHeader, this).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this.CurrentContainer = alt;
					base.Address = alt.Address;
					break;
				}
				alt = null;
			}
			List<MQBAlternativeContainer>.Enumerator enumerator = default(List<MQBAlternativeContainer>.Enumerator);
			CodingRequestResult codingRequestResult;
			if (this.CurrentContainer == null)
			{
				if (this.CodingResultWhenNoSuitableAlternativeFound == CodingRequestResult.NotSupported)
				{
					base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				}
				codingRequestResult = this.CodingResultWhenNoSuitableAlternativeFound;
			}
			else
			{
				string text = "";
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (this.CurrentContainer.GetCurrentStateDelegate == null)
				{
					base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
					codingRequestResult = item;
				}
				else if (item != CodingRequestResult.Success)
				{
					base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else
				{
					try
					{
						text = this.CurrentContainer.GetCurrentStateDelegate(item2, this);
					}
					catch (Exception)
					{
						base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.InitialDataIncorrect);
						return CodingRequestResult.InitialDataIncorrect;
					}
					base.CurrentState = text;
					codingRequestResult = CodingRequestResult.Success;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00381097 File Offset: 0x0037F297
		public bool IsMIB3()
		{
			return this.ASAM != null && (this.ASAM.Contains("EV_MUOIMQB") || this.ASAM.Contains("EV_MUCNSMQB"));
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x003810C8 File Offset: 0x0037F2C8
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			if (progress != null)
			{
				progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
			}
			if (string.IsNullOrEmpty(this.ECU) || string.IsNullOrEmpty(this.Device) || string.IsNullOrEmpty(this.ASAM))
			{
				await this.RequestDeviceIdentsAsync();
			}
			if (this.CurrentContainer == null)
			{
				foreach (MQBAlternativeContainer alt in this.Alternatives)
				{
					TaskAwaiter<bool> taskAwaiter = alt.CheckIsSupported(base.RequestHeader, base.ResponseHeader, this).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.CurrentContainer = alt;
						base.Address = alt.Address;
						break;
					}
					alt = null;
				}
				List<MQBAlternativeContainer>.Enumerator enumerator = default(List<MQBAlternativeContainer>.Enumerator);
			}
			CodingRequestResult codingRequestResult;
			if (this.CurrentContainer == null)
			{
				if (this.CodingResultWhenNoSuitableAlternativeFound == CodingRequestResult.NotSupported)
				{
					base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				}
				codingRequestResult = this.CodingResultWhenNoSuitableAlternativeFound;
			}
			else
			{
				if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(this.Password))
				{
					password = this.Password;
				}
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
						array = this.CurrentContainer.ChangeDataDelegate(tuple.Item1, value, this);
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
					string text = BitHelpers.ByteArrayToHexString(array);
					codingRequestResult = await base.WriteDataToECU(password, UserFriendlyValue, progress, tuple.Item1, text);
				}
				else
				{
					codingRequestResult = tuple.Item2;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0038112C File Offset: 0x0037F32C
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__13_0(OBDRequest device_request2, byte[] data, bool result, string responseHeader)
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

		// Token: 0x06004AFA RID: 19194 RVA: 0x00381194 File Offset: 0x0037F394
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__13_1(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
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

		// Token: 0x06004AFB RID: 19195 RVA: 0x003811FC File Offset: 0x0037F3FC
		[CompilerGenerated]
		private void <RequestDeviceIdentsAsync>b__13_2(OBDRequest ecu_request2, byte[] data, bool result, string responseHeader)
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

		// Token: 0x06004AFC RID: 19196 RVA: 0x00381272 File Offset: 0x0037F472
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__0(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			return base.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
		}

		// Token: 0x04002BA9 RID: 11177
		[CompilerGenerated]
		private string <Device>k__BackingField;

		// Token: 0x04002BAA RID: 11178
		[CompilerGenerated]
		private string <ECU>k__BackingField;

		// Token: 0x04002BAB RID: 11179
		[CompilerGenerated]
		private string <ASAM>k__BackingField;

		// Token: 0x04002BAC RID: 11180
		[CompilerGenerated]
		private CodingRequestResult <CodingResultWhenNoSuitableAlternativeFound>k__BackingField;

		// Token: 0x04002BAD RID: 11181
		[CompilerGenerated]
		private List<MQBAlternativeContainer> <Alternatives>k__BackingField;

		// Token: 0x04002BAE RID: 11182
		private MQBAlternativeContainer _CurrentContainer;

		// Token: 0x02000897 RID: 2199
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06004AFD RID: 19197 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06004AFE RID: 19198 RVA: 0x00381284 File Offset: 0x0037F484
			internal byte[] <BuildAltCodingSwitchForUnit09>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, this.NewGenByte, this.NewGenBit, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, this.NewGenByte, this.NewGenBit, false);
				}
				return array;
			}

			// Token: 0x06004AFF RID: 19199 RVA: 0x003812E4 File Offset: 0x0037F4E4
			internal byte[] <BuildAltCodingSwitchForUnit09>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, this.OldGenByte, this.OldGenBit, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, this.OldGenByte, this.OldGenBit, false);
				}
				return array;
			}

			// Token: 0x04002BAF RID: 11183
			public int NewGenByte;

			// Token: 0x04002BB0 RID: 11184
			public int NewGenBit;

			// Token: 0x04002BB1 RID: 11185
			public int OldGenByte;

			// Token: 0x04002BB2 RID: 11186
			public int OldGenBit;
		}

		// Token: 0x02000898 RID: 2200
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__33 : IAsyncStateMachine
		{
			// Token: 0x06004B00 RID: 19200 RVA: 0x00381344 File Offset: 0x0037F544
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAlternativeCoding mqbalternativeCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter6;
					TaskAwaiter<CodingRequestResult> taskAwaiter8;
					IProgress<string> progress;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
						IL_00E4:
						try
						{
							if (num != 1)
							{
								goto IL_0197;
							}
							TaskAwaiter<bool> taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							IL_0168:
							if (taskAwaiter5.GetResult())
							{
								mqbalternativeCoding.CurrentContainer = alt;
								mqbalternativeCoding.Address = alt.Address;
								goto IL_01A7;
							}
							alt = null;
							IL_0197:
							if (enumerator.MoveNext())
							{
								alt = enumerator.Current;
								taskAwaiter5 = alt.CheckIsSupported(mqbalternativeCoding.RequestHeader, mqbalternativeCoding.ResponseHeader, mqbalternativeCoding).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBAlternativeCoding.<Execute>d__33>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_0168;
							}
							IL_01A7:;
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<MQBAlternativeContainer>.Enumerator);
						goto IL_01CB;
					case 2:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num = (num2 = -1);
						goto IL_0276;
					}
					case 3:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num = (num2 = -1);
						goto IL_02F1;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter9;
						taskAwaiter8 = taskAwaiter9;
						taskAwaiter9 = default(TaskAwaiter<CodingRequestResult>);
						num = (num2 = -1);
						goto IL_03C5;
					}
					default:
						progress = progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
						}
						if (!string.IsNullOrEmpty(mqbalternativeCoding.ECU) && !string.IsNullOrEmpty(mqbalternativeCoding.Device) && !string.IsNullOrEmpty(mqbalternativeCoding.ASAM))
						{
							goto IL_00C8;
						}
						taskAwaiter3 = mqbalternativeCoding.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAlternativeCoding.<Execute>d__33>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					IL_00C8:
					if (mqbalternativeCoding.CurrentContainer == null)
					{
						enumerator = mqbalternativeCoding.Alternatives.GetEnumerator();
						goto IL_00E4;
					}
					IL_01CB:
					if (mqbalternativeCoding.CurrentContainer == null)
					{
						if (mqbalternativeCoding.CodingResultWhenNoSuitableAlternativeFound == CodingRequestResult.NotSupported)
						{
							mqbalternativeCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						}
						codingRequestResult = mqbalternativeCoding.CodingResultWhenNoSuitableAlternativeFound;
						goto IL_03F1;
					}
					if (string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(mqbalternativeCoding.Password))
					{
						password = mqbalternativeCoding.Password;
					}
					taskAwaiter6 = mqbalternativeCoding.GetCurrentStateRawData("").GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAlternativeCoding.<Execute>d__33>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0276:
					Tuple<byte[], CodingRequestResult> tuple = taskAwaiter6.GetResult();
					if (tuple.Item2 != CodingRequestResult.WrongAccessKey || string.IsNullOrEmpty(password))
					{
						goto IL_02F9;
					}
					taskAwaiter6 = mqbalternativeCoding.GetCurrentStateRawData(password).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 3);
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAlternativeCoding.<Execute>d__33>(ref taskAwaiter6, ref this);
						return;
					}
					IL_02F1:
					tuple = taskAwaiter6.GetResult();
					IL_02F9:
					if (tuple.Item1 == null || tuple.Item2 != CodingRequestResult.Success)
					{
						codingRequestResult = tuple.Item2;
						goto IL_03F1;
					}
					byte[] array;
					try
					{
						array = mqbalternativeCoding.CurrentContainer.ChangeDataDelegate(tuple.Item1, value, mqbalternativeCoding);
					}
					catch (CodingException ex)
					{
						if (ex.ExceptionConsequence == ExceptionConsequences.SkipIgnore)
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_03F1;
						}
						codingRequestResult = CodingRequestResult.InitialDataIncorrect;
						goto IL_03F1;
					}
					catch (Exception)
					{
						codingRequestResult = CodingRequestResult.UnknownError;
						goto IL_03F1;
					}
					string text = BitHelpers.ByteArrayToHexString(array);
					taskAwaiter8 = mqbalternativeCoding.<>n__0(password, UserFriendlyValue, progress, tuple.Item1, text).GetAwaiter();
					if (!taskAwaiter8.IsCompleted)
					{
						num = (num2 = 4);
						TaskAwaiter<CodingRequestResult> taskAwaiter9 = taskAwaiter8;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBAlternativeCoding.<Execute>d__33>(ref taskAwaiter8, ref this);
						return;
					}
					IL_03C5:
					codingRequestResult = taskAwaiter8.GetResult();
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_03F1:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004B01 RID: 19201 RVA: 0x003817BC File Offset: 0x0037F9BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BB3 RID: 11187
			public int <>1__state;

			// Token: 0x04002BB4 RID: 11188
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002BB5 RID: 11189
			public IProgress<string> progress;

			// Token: 0x04002BB6 RID: 11190
			public MQBAlternativeCoding <>4__this;

			// Token: 0x04002BB7 RID: 11191
			public string password;

			// Token: 0x04002BB8 RID: 11192
			public string value;

			// Token: 0x04002BB9 RID: 11193
			public string UserFriendlyValue;

			// Token: 0x04002BBA RID: 11194
			private TaskAwaiter <>u__1;

			// Token: 0x04002BBB RID: 11195
			private List<MQBAlternativeContainer>.Enumerator <>7__wrap1;

			// Token: 0x04002BBC RID: 11196
			private MQBAlternativeContainer <alt>5__3;

			// Token: 0x04002BBD RID: 11197
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04002BBE RID: 11198
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__3;

			// Token: 0x04002BBF RID: 11199
			private TaskAwaiter<CodingRequestResult> <>u__4;
		}

		// Token: 0x02000899 RID: 2201
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestDeviceIdentsAsync>d__13 : IAsyncStateMachine
		{
			// Token: 0x06004B02 RID: 19202 RVA: 0x003817CC File Offset: 0x0037F9CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAlternativeCoding mqbalternativeCoding = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						mqbalternativeCoding.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("22F187", mqbalternativeCoding.RequestHeader, mqbalternativeCoding.BeforeCommands, mqbalternativeCoding.AfterCommands, false);
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
						OBDRequest obdrequest2 = new OBDRequest("22F191", mqbalternativeCoding.RequestHeader, mqbalternativeCoding.BeforeCommands, mqbalternativeCoding.AfterCommands, false);
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
						OBDRequest obdrequest3 = new OBDRequest("22F19E", mqbalternativeCoding.RequestHeader, mqbalternativeCoding.BeforeCommands, mqbalternativeCoding.AfterCommands, false);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAlternativeCoding.<RequestDeviceIdentsAsync>d__13>(ref taskAwaiter, ref this);
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

			// Token: 0x06004B03 RID: 19203 RVA: 0x00381974 File Offset: 0x0037FB74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BC0 RID: 11200
			public int <>1__state;

			// Token: 0x04002BC1 RID: 11201
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002BC2 RID: 11202
			public MQBAlternativeCoding <>4__this;

			// Token: 0x04002BC3 RID: 11203
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200089A RID: 2202
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__31 : IAsyncStateMachine
		{
			// Token: 0x06004B04 RID: 19204 RVA: 0x00381984 File Offset: 0x0037FB84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAlternativeCoding mqbalternativeCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter6;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						IL_00B6:
						try
						{
							if (num != 1)
							{
								goto IL_0169;
							}
							TaskAwaiter<bool> taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							IL_013A:
							if (taskAwaiter5.GetResult())
							{
								mqbalternativeCoding.CurrentContainer = alt;
								mqbalternativeCoding.Address = alt.Address;
								goto IL_0179;
							}
							alt = null;
							IL_0169:
							if (enumerator.MoveNext())
							{
								alt = enumerator.Current;
								taskAwaiter5 = alt.CheckIsSupported(mqbalternativeCoding.RequestHeader, mqbalternativeCoding.ResponseHeader, mqbalternativeCoding).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBAlternativeCoding.<UpdateCurrentState>d__31>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_013A;
							}
							IL_0179:;
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<MQBAlternativeContainer>.Enumerator);
						if (mqbalternativeCoding.CurrentContainer == null)
						{
							if (mqbalternativeCoding.CodingResultWhenNoSuitableAlternativeFound == CodingRequestResult.NotSupported)
							{
								mqbalternativeCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							}
							codingRequestResult = mqbalternativeCoding.CodingResultWhenNoSuitableAlternativeFound;
							goto IL_02BE;
						}
						string text = "";
						taskAwaiter6 = mqbalternativeCoding.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAlternativeCoding.<UpdateCurrentState>d__31>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0229;
					}
					case 2:
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num = (num2 = -1);
						goto IL_0229;
					}
					default:
						if (!string.IsNullOrEmpty(mqbalternativeCoding.ECU) && !string.IsNullOrEmpty(mqbalternativeCoding.Device) && !string.IsNullOrEmpty(mqbalternativeCoding.ASAM))
						{
							goto IL_00A5;
						}
						taskAwaiter3 = mqbalternativeCoding.RequestDeviceIdentsAsync().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBAlternativeCoding.<UpdateCurrentState>d__31>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					IL_00A5:
					enumerator = mqbalternativeCoding.Alternatives.GetEnumerator();
					goto IL_00B6;
					IL_0229:
					Tuple<byte[], CodingRequestResult> result = taskAwaiter6.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (mqbalternativeCoding.CurrentContainer.GetCurrentStateDelegate == null)
					{
						mqbalternativeCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						codingRequestResult = item;
					}
					else if (item != CodingRequestResult.Success)
					{
						mqbalternativeCoding.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						string text;
						try
						{
							text = mqbalternativeCoding.CurrentContainer.GetCurrentStateDelegate(item2, mqbalternativeCoding);
						}
						catch (Exception)
						{
							mqbalternativeCoding.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(CodingRequestResult.InitialDataIncorrect);
							codingRequestResult = CodingRequestResult.InitialDataIncorrect;
							goto IL_02BE;
						}
						mqbalternativeCoding.CurrentState = text;
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02BE:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06004B05 RID: 19205 RVA: 0x00381CB0 File Offset: 0x0037FEB0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BC4 RID: 11204
			public int <>1__state;

			// Token: 0x04002BC5 RID: 11205
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002BC6 RID: 11206
			public MQBAlternativeCoding <>4__this;

			// Token: 0x04002BC7 RID: 11207
			public string password;

			// Token: 0x04002BC8 RID: 11208
			private TaskAwaiter <>u__1;

			// Token: 0x04002BC9 RID: 11209
			private List<MQBAlternativeContainer>.Enumerator <>7__wrap1;

			// Token: 0x04002BCA RID: 11210
			private MQBAlternativeContainer <alt>5__3;

			// Token: 0x04002BCB RID: 11211
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04002BCC RID: 11212
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__3;
		}
	}
}
