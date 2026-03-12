using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ECUModels;

namespace CarScannerXamarinForms.Coding.DB.VWTP
{
	// Token: 0x020009CF RID: 2511
	internal class VWLongCoding : CustomizableCodingTemplate
	{
		// Token: 0x0600512D RID: 20781 RVA: 0x003EFCAC File Offset: 0x003EDEAC
		public VWLongCoding(string unit, string name)
		{
			this.ValueType = AdaptationValueTypes.InputHexDataType;
			this.UnitId = unit;
			base.Name = "NEW LC_" + name;
			this.Group = CodingGroup.LongCoding;
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x003EFCE6 File Offset: 0x003EDEE6
		private bool HasVWTPAddress
		{
			get
			{
				return !string.IsNullOrEmpty(this.VWTPHeader);
			}
		}

		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x0600512F RID: 20783 RVA: 0x003EFCF6 File Offset: 0x003EDEF6
		private bool HasCAN11bitAddress
		{
			get
			{
				return !string.IsNullOrEmpty(this.CAN11bitRequestHeader);
			}
		}

		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x003EFD06 File Offset: 0x003EDF06
		private bool HasCAN29bitAddress
		{
			get
			{
				return !string.IsNullOrEmpty(this.CAN29bitRequestHeader);
			}
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x003EFD16 File Offset: 0x003EDF16
		public override Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			return base.UpdateCurrentState(password, progress);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x003EFD20 File Offset: 0x003EDF20
		private void SetCAN11BitSettings()
		{
			base.RequestHeader = this.CAN11bitRequestHeader;
			base.ResponseHeader = this.CAN11bitResponseHeader;
			base.DataLength = 0;
			base.Protocol = "6";
			this.ReadModeAndAddress = "220600";
			base.WriteModeAndAddress = "2E0600";
			base.OpenSessionCommand = "1003";
			base.MakeChangesToInitialData = true;
			if (this.UnitId == "5F")
			{
				base.PreWriteCommands = "22F199;22F198;22F1A5;22F1A0;2EF199;2EF198;2EF1A5;2EF1A0;2703;2704;";
			}
			else
			{
				base.PreWriteCommands = "22F199;22F198;2EF199;2EF198;2703;2704;";
			}
			this.BuildDefaultBeforeAndAfterCommands();
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x003EFDB0 File Offset: 0x003EDFB0
		private void SetCAN29BitSettings()
		{
			base.RequestHeader = this.CAN29bitRequestHeader;
			base.ResponseHeader = this.CAN29bitResponseHeader;
			base.DataLength = 0;
			base.Protocol = "7";
			base.OpenSessionCommand = "1003";
			this.ReadModeAndAddress = "220600";
			base.WriteModeAndAddress = "2E0600";
			if (this.UnitId == "5F")
			{
				base.PreWriteCommands = "22F199;22F198;22F1A5;22F1A0;2EF199;2EF198;2EF1A5;2EF1A0;2703;2704;";
			}
			else
			{
				base.PreWriteCommands = "22F199;22F198;2EF199;2EF198;2703;2704;";
			}
			base.MakeChangesToInitialData = true;
			this.BuildDefaultBeforeAndAfterCommands();
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x003EFE40 File Offset: 0x003EE040
		private void SetVWTP_UDSSettings()
		{
			base.OpenSessionCommand = "VWTP:" + this.VWTPHeader + ":1089";
			base.RequestHeader = "000";
			base.ResponseHeader = "";
			this.ReadModeAndAddress = "VWTP:" + this.VWTPHeader + ":220600";
			base.WriteModeAndAddress = "VWTP:" + this.VWTPHeader + ":2E0600";
			base.PreWriteCommands = "";
			base.DataLength = 0;
			base.MakeChangesToInitialData = true;
			this.BuildDefaultBeforeAndAfterCommands();
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x003EFED4 File Offset: 0x003EE0D4
		private void SetVWTP_KWPSettings()
		{
			base.OpenSessionCommand = "VWTP:" + this.VWTPHeader + ":1089";
			base.RequestHeader = "000";
			base.ResponseHeader = "";
			this.ReadModeAndAddress = "VWTP:" + this.VWTPHeader + ":1A9A";
			base.WriteModeAndAddress = "VWTP:" + this.VWTPHeader + ":3B9A";
			base.PreWriteCommands = "";
			base.StartByteId = 12;
			base.DataLength = 0;
			base.MakeChangesToInitialData = true;
			this.BuildDefaultBeforeAndAfterCommands();
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x003EFF70 File Offset: 0x003EE170
		public override async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			Tuple<byte[], CodingRequestResult> tuple5;
			try
			{
				if (this.ProtocolType == VWLongCoding.ProtocolTypes.Unknown)
				{
					if (this.HasCAN11bitAddress)
					{
						this.ProtocolType = VWLongCoding.ProtocolTypes.CAN11bit;
					}
					else if (this.HasCAN29bitAddress)
					{
						this.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
					}
					else
					{
						this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
					}
				}
				if (this.ProtocolType == VWLongCoding.ProtocolTypes.CAN11bit)
				{
					this.SetCAN11BitSettings();
					Tuple<byte[], CodingRequestResult> tuple = await base.GetCurrentStateRawData(password);
					if (tuple.Item2 == CodingRequestResult.Success)
					{
						this.protocolConfirmed = true;
						this.ProtocolType = VWLongCoding.ProtocolTypes.CAN11bit;
						return tuple;
					}
					if (!this.protocolConfirmed)
					{
						if (this.HasCAN29bitAddress)
						{
							this.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
						}
						else
						{
							this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
						}
					}
				}
				if (this.ProtocolType == VWLongCoding.ProtocolTypes.CAN29bit)
				{
					this.SetCAN29BitSettings();
					Tuple<byte[], CodingRequestResult> tuple2 = await base.GetCurrentStateRawData(password);
					if (tuple2.Item2 == CodingRequestResult.Success)
					{
						this.protocolConfirmed = true;
						this.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
						return tuple2;
					}
					if (!this.protocolConfirmed && this.HasVWTPAddress)
					{
						this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
					}
				}
				if (this.ProtocolType == VWLongCoding.ProtocolTypes.VWTP20_UDS)
				{
					this.SetVWTP_UDSSettings();
					Tuple<byte[], CodingRequestResult> tuple3 = await base.GetCurrentStateRawData(password);
					if (tuple3.Item2 == CodingRequestResult.Success)
					{
						this.protocolConfirmed = true;
						this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
						return tuple3;
					}
					if (!this.protocolConfirmed && this.HasVWTPAddress)
					{
						this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_KWP;
					}
				}
				if (this.ProtocolType == VWLongCoding.ProtocolTypes.VWTP20_KWP)
				{
					this.SetVWTP_KWPSettings();
					Tuple<byte[], CodingRequestResult> tuple4 = await base.GetCurrentStateRawData(password);
					if (tuple4.Item2 == CodingRequestResult.Success)
					{
						int num = (int)(tuple4.Item1[11] - 1);
						base.DataLength = num;
						this.VWTP_KWP_OriginalData = tuple4.Item1;
						byte[] array = new byte[num];
						Array.Copy(tuple4.Item1, 12, array, 0, array.Length);
						this.protocolConfirmed = true;
						this.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_KWP;
						return new Tuple<byte[], CodingRequestResult>(array, tuple4.Item2);
					}
				}
				this.ProtocolType = VWLongCoding.ProtocolTypes.Unknown;
				tuple5 = new Tuple<byte[], CodingRequestResult>(new byte[0], CodingRequestResult.NotSupported);
			}
			catch (Exception)
			{
				tuple5 = new Tuple<byte[], CodingRequestResult>(new byte[0], CodingRequestResult.UnknownError);
			}
			return tuple5;
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x003EFFBC File Offset: 0x003EE1BC
		protected override async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			CodingRequestResult codingRequestResult;
			switch (this.ProtocolType)
			{
			case VWLongCoding.ProtocolTypes.Unknown:
				codingRequestResult = CodingRequestResult.NotSupported;
				break;
			case VWLongCoding.ProtocolTypes.CAN11bit:
			case VWLongCoding.ProtocolTypes.CAN29bit:
			case VWLongCoding.ProtocolTypes.VWTP20_UDS:
				codingRequestResult = await base.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
				break;
			case VWLongCoding.ProtocolTypes.VWTP20_KWP:
			{
				byte[] array = new byte[this.VWTP_KWP_OriginalData.Length];
				Array.Copy(this.VWTP_KWP_OriginalData, array, array.Length);
				byte[] array2 = BitHelpers.ConvertHexToBytesX(checked_value);
				Array.Copy(array2, 0, array, 12, array2.Length);
				codingRequestResult = await base.WriteDataToECU(password, UserFriendlyValue, progress, originalData, BitHelpers.ByteArrayToHexString(array));
				break;
			}
			default:
				codingRequestResult = CodingRequestResult.NotSupported;
				break;
			}
			return codingRequestResult;
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x003F002C File Offset: 0x003EE22C
		public static List<VWLongCoding> BuildLongCodingList()
		{
			IEnumerable<IECU> ecus = VAGECU.ECUs;
			IReadOnlyList<IECU> ecus2 = VAG29bitECU.ECUs;
			List<IECU> list = (from x in ecus.Concat(ecus2)
				where x is VAGECU || x is VAG29bitECU
				select x).ToList<IECU>();
			List<VWLongCoding> list2 = new List<VWLongCoding>();
			foreach (IECU iecu in list)
			{
				string id = VWLongCoding.GetUnitIdFromName(iecu.Name);
				if (!string.IsNullOrEmpty(id))
				{
					VWLongCoding vwlongCoding = list2.FirstOrDefault((VWLongCoding x) => x.UnitId == id);
					if (vwlongCoding == null)
					{
						VWLongCoding vwlongCoding2 = new VWLongCoding(id, iecu.Name);
						if (!string.IsNullOrEmpty(iecu.RequestHeader))
						{
							if (iecu.RequestHeader.Length == 3)
							{
								vwlongCoding2.CAN11bitRequestHeader = iecu.RequestHeader;
								vwlongCoding2.CAN11bitResponseHeader = iecu.ResponseHeader;
							}
							if (iecu.RequestHeader.Length == 6)
							{
								VAG29bitECU vag29bitECU = iecu as VAG29bitECU;
								if (vag29bitECU != null)
								{
									vwlongCoding2.CAN29bitRequestHeader = vag29bitECU.CANPriority + vag29bitECU.RequestHeader;
									vwlongCoding2.CAN29bitResponseHeader = vag29bitECU.ResponseHeader;
								}
							}
						}
						if (!string.IsNullOrEmpty(iecu.ExtendedAddress))
						{
							vwlongCoding2.VWTPHeader = iecu.ExtendedAddress;
						}
						list2.Add(vwlongCoding2);
					}
					else
					{
						VWLongCoding vwlongCoding3 = vwlongCoding;
						if (!string.IsNullOrEmpty(iecu.RequestHeader))
						{
							if (iecu.RequestHeader.Length == 3)
							{
								vwlongCoding3.CAN11bitRequestHeader = iecu.RequestHeader;
								vwlongCoding3.CAN11bitResponseHeader = iecu.ResponseHeader;
							}
							if (iecu.RequestHeader.Length == 6)
							{
								VAG29bitECU vag29bitECU2 = iecu as VAG29bitECU;
								if (vag29bitECU2 != null)
								{
									vwlongCoding3.CAN29bitRequestHeader = vag29bitECU2.CANPriority + iecu.RequestHeader;
									vwlongCoding3.CAN29bitResponseHeader = iecu.ResponseHeader;
								}
							}
						}
						if (!string.IsNullOrEmpty(iecu.ExtendedAddress))
						{
							vwlongCoding3.VWTPHeader = iecu.ExtendedAddress;
						}
					}
				}
			}
			return list2;
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x003F0250 File Offset: 0x003EE450
		private static string GetUnitIdFromName(string name)
		{
			if (name == null)
			{
				return "";
			}
			int num = name.IndexOf('.');
			if (num < 0)
			{
				return "";
			}
			return name.Substring(0, num);
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x003F0281 File Offset: 0x003EE481
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<Tuple<byte[], CodingRequestResult>> <>n__0(string password)
		{
			return base.GetCurrentStateRawData(password);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x003F028A File Offset: 0x003EE48A
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<CodingRequestResult> <>n__1(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			return base.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
		}

		// Token: 0x04003124 RID: 12580
		private string UnitId = "";

		// Token: 0x04003125 RID: 12581
		private string CAN11bitRequestHeader;

		// Token: 0x04003126 RID: 12582
		private string CAN11bitResponseHeader;

		// Token: 0x04003127 RID: 12583
		private string VWTPHeader;

		// Token: 0x04003128 RID: 12584
		private string CAN29bitRequestHeader;

		// Token: 0x04003129 RID: 12585
		private string CAN29bitResponseHeader;

		// Token: 0x0400312A RID: 12586
		private byte[] VWTP_KWP_OriginalData;

		// Token: 0x0400312B RID: 12587
		private VWLongCoding.ProtocolTypes ProtocolType;

		// Token: 0x0400312C RID: 12588
		private bool protocolConfirmed;

		// Token: 0x020009D0 RID: 2512
		private enum ProtocolTypes
		{
			// Token: 0x0400312E RID: 12590
			Unknown,
			// Token: 0x0400312F RID: 12591
			CAN11bit,
			// Token: 0x04003130 RID: 12592
			CAN29bit,
			// Token: 0x04003131 RID: 12593
			VWTP20_UDS,
			// Token: 0x04003132 RID: 12594
			VWTP20_KWP
		}

		// Token: 0x020009D1 RID: 2513
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600513C RID: 20796 RVA: 0x003F0299 File Offset: 0x003EE499
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600513D RID: 20797 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600513E RID: 20798 RVA: 0x003F02A5 File Offset: 0x003EE4A5
			internal bool <BuildLongCodingList>b__24_0(IECU x)
			{
				return x is VAGECU || x is VAG29bitECU;
			}

			// Token: 0x04003133 RID: 12595
			public static readonly VWLongCoding.<>c <>9 = new VWLongCoding.<>c();

			// Token: 0x04003134 RID: 12596
			public static Func<IECU, bool> <>9__24_0;
		}

		// Token: 0x020009D2 RID: 2514
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			// Token: 0x0600513F RID: 20799 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass24_0()
			{
			}

			// Token: 0x06005140 RID: 20800 RVA: 0x003F02BA File Offset: 0x003EE4BA
			internal bool <BuildLongCodingList>b__1(VWLongCoding x)
			{
				return x.UnitId == this.id;
			}

			// Token: 0x04003135 RID: 12597
			public string id;
		}

		// Token: 0x020009D3 RID: 2515
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__22 : IAsyncStateMachine
		{
			// Token: 0x06005141 RID: 20801 RVA: 0x003F02D0 File Offset: 0x003EE4D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWLongCoding vwlongCoding = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					try
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							goto IL_017D;
						}
						case 2:
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							goto IL_022B;
						}
						case 3:
						{
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
							num2 = -1;
							goto IL_02D9;
						}
						default:
							if (vwlongCoding.ProtocolType == VWLongCoding.ProtocolTypes.Unknown)
							{
								if (vwlongCoding.HasCAN11bitAddress)
								{
									vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.CAN11bit;
								}
								else if (vwlongCoding.HasCAN29bitAddress)
								{
									vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
								}
								else
								{
									vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
								}
							}
							if (vwlongCoding.ProtocolType != VWLongCoding.ProtocolTypes.CAN11bit)
							{
								goto IL_010E;
							}
							vwlongCoding.SetCAN11BitSettings();
							taskAwaiter = vwlongCoding.<>n__0(password).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VWLongCoding.<GetCurrentStateRawData>d__22>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
						if (result.Item2 == CodingRequestResult.Success)
						{
							vwlongCoding.protocolConfirmed = true;
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.CAN11bit;
							tuple = result;
							goto IL_038B;
						}
						if (!vwlongCoding.protocolConfirmed)
						{
							if (vwlongCoding.HasCAN29bitAddress)
							{
								vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
							}
							else
							{
								vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
							}
						}
						IL_010E:
						if (vwlongCoding.ProtocolType != VWLongCoding.ProtocolTypes.CAN29bit)
						{
							goto IL_01BC;
						}
						vwlongCoding.SetCAN29BitSettings();
						taskAwaiter = vwlongCoding.<>n__0(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VWLongCoding.<GetCurrentStateRawData>d__22>(ref taskAwaiter, ref this);
							return;
						}
						IL_017D:
						Tuple<byte[], CodingRequestResult> result2 = taskAwaiter.GetResult();
						if (result2.Item2 == CodingRequestResult.Success)
						{
							vwlongCoding.protocolConfirmed = true;
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.CAN29bit;
							tuple = result2;
							goto IL_038B;
						}
						if (!vwlongCoding.protocolConfirmed && vwlongCoding.HasVWTPAddress)
						{
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
						}
						IL_01BC:
						if (vwlongCoding.ProtocolType != VWLongCoding.ProtocolTypes.VWTP20_UDS)
						{
							goto IL_026A;
						}
						vwlongCoding.SetVWTP_UDSSettings();
						taskAwaiter = vwlongCoding.<>n__0(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VWLongCoding.<GetCurrentStateRawData>d__22>(ref taskAwaiter, ref this);
							return;
						}
						IL_022B:
						Tuple<byte[], CodingRequestResult> result3 = taskAwaiter.GetResult();
						if (result3.Item2 == CodingRequestResult.Success)
						{
							vwlongCoding.protocolConfirmed = true;
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_UDS;
							tuple = result3;
							goto IL_038B;
						}
						if (!vwlongCoding.protocolConfirmed && vwlongCoding.HasVWTPAddress)
						{
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_KWP;
						}
						IL_026A:
						if (vwlongCoding.ProtocolType != VWLongCoding.ProtocolTypes.VWTP20_KWP)
						{
							goto IL_034B;
						}
						vwlongCoding.SetVWTP_KWPSettings();
						taskAwaiter = vwlongCoding.<>n__0(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, VWLongCoding.<GetCurrentStateRawData>d__22>(ref taskAwaiter, ref this);
							return;
						}
						IL_02D9:
						Tuple<byte[], CodingRequestResult> result4 = taskAwaiter.GetResult();
						if (result4.Item2 == CodingRequestResult.Success)
						{
							int num3 = (int)(result4.Item1[11] - 1);
							vwlongCoding.DataLength = num3;
							vwlongCoding.VWTP_KWP_OriginalData = result4.Item1;
							byte[] array = new byte[num3];
							Array.Copy(result4.Item1, 12, array, 0, array.Length);
							vwlongCoding.protocolConfirmed = true;
							vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.VWTP20_KWP;
							tuple = new Tuple<byte[], CodingRequestResult>(array, result4.Item2);
							goto IL_038B;
						}
						IL_034B:
						vwlongCoding.ProtocolType = VWLongCoding.ProtocolTypes.Unknown;
						tuple = new Tuple<byte[], CodingRequestResult>(new byte[0], CodingRequestResult.NotSupported);
					}
					catch (Exception)
					{
						tuple = new Tuple<byte[], CodingRequestResult>(new byte[0], CodingRequestResult.UnknownError);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_038B:
				num2 = -2;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06005142 RID: 20802 RVA: 0x003F06B0 File Offset: 0x003EE8B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003136 RID: 12598
			public int <>1__state;

			// Token: 0x04003137 RID: 12599
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04003138 RID: 12600
			public VWLongCoding <>4__this;

			// Token: 0x04003139 RID: 12601
			public string password;

			// Token: 0x0400313A RID: 12602
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}

		// Token: 0x020009D4 RID: 2516
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__23 : IAsyncStateMachine
		{
			// Token: 0x06005143 RID: 20803 RVA: 0x003F06C0 File Offset: 0x003EE8C0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWLongCoding vwlongCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							switch (vwlongCoding.ProtocolType)
							{
							case VWLongCoding.ProtocolTypes.Unknown:
								codingRequestResult = CodingRequestResult.NotSupported;
								goto IL_01A7;
							case VWLongCoding.ProtocolTypes.CAN11bit:
							case VWLongCoding.ProtocolTypes.CAN29bit:
							case VWLongCoding.ProtocolTypes.VWTP20_UDS:
								taskAwaiter = vwlongCoding.<>n__1(password, UserFriendlyValue, progress, originalData, checked_value).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VWLongCoding.<WriteDataToECU>d__23>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00BE;
							case VWLongCoding.ProtocolTypes.VWTP20_KWP:
							{
								byte[] array = new byte[vwlongCoding.VWTP_KWP_OriginalData.Length];
								Array.Copy(vwlongCoding.VWTP_KWP_OriginalData, array, array.Length);
								byte[] array2 = BitHelpers.ConvertHexToBytesX(checked_value);
								Array.Copy(array2, 0, array, 12, array2.Length);
								string text = BitHelpers.ByteArrayToHexString(array);
								taskAwaiter = vwlongCoding.<>n__1(password, UserFriendlyValue, progress, originalData, text).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VWLongCoding.<WriteDataToECU>d__23>(ref taskAwaiter, ref this);
									return;
								}
								break;
							}
							default:
								codingRequestResult = CodingRequestResult.NotSupported;
								goto IL_01A7;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
						}
						codingRequestResult = taskAwaiter.GetResult();
						goto IL_01A7;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
					num2 = -1;
					IL_00BE:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A7:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005144 RID: 20804 RVA: 0x003F08A4 File Offset: 0x003EEAA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400313B RID: 12603
			public int <>1__state;

			// Token: 0x0400313C RID: 12604
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400313D RID: 12605
			public VWLongCoding <>4__this;

			// Token: 0x0400313E RID: 12606
			public string password;

			// Token: 0x0400313F RID: 12607
			public string UserFriendlyValue;

			// Token: 0x04003140 RID: 12608
			public IProgress<string> progress;

			// Token: 0x04003141 RID: 12609
			public byte[] originalData;

			// Token: 0x04003142 RID: 12610
			public string checked_value;

			// Token: 0x04003143 RID: 12611
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
