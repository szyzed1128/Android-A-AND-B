using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200087B RID: 2171
	internal class HyundaiKiaUDSCoding : UDSCoding
	{
		// Token: 0x06004A0C RID: 18956 RVA: 0x0037C4D4 File Offset: 0x0037A6D4
		public HyundaiKiaUDSCoding(int UID, string Name, string Description, string Password, string PasswordHint, IEnumerable<TranslationItem> Translations, AdaptationValueTypes ValueType, string Address, string RequestHeader, string ResponseHeader, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned, bool ReversedByteSet, bool RequiresPro, IEnumerable<MQBAdaptationOption> Options)
			: base(UID, Name, Description, Password, PasswordHint, Translations, ValueType, Address, RequestHeader, ResponseHeader, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet, RequiresPro, Options)
		{
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x06004A0D RID: 18957 RVA: 0x001ECD4C File Offset: 0x001EAF4C
		protected override CodingLogItem.CodingTypes CodingType
		{
			get
			{
				return CodingLogItem.CodingTypes.HyundaiKiaUDS;
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x0037C508 File Offset: 0x0037A708
		protected override string OpenSessionCommand
		{
			get
			{
				return "20;1003";
			}
		}

		// Token: 0x06004A0F RID: 18959 RVA: 0x0037C510 File Offset: 0x0037A710
		protected override OBDRequest[] GetAccessKeysRequests(string password, IProgress<string> progress, Action wrongPasswordCallback)
		{
			bool zero_seed = false;
			OBDRequest req_sendKey = new OBDRequest("2702", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest = new OBDRequest("2701", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += delegate(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					int num = BitConverter.ToInt32(getSeedData, 0);
					if (num == 0)
					{
						zero_seed = true;
					}
					if (!zero_seed)
					{
						string text = (12 - num).ToString("X8");
						req_sendKey.Command = "2702" + text;
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					req_sendKey.Command = "3E";
				}
			};
			req_sendKey.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6702")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress3 = progress;
						if (progress3 == null)
						{
							return;
						}
						progress3.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else
					{
						Action wrongPasswordCallback2 = wrongPasswordCallback;
						if (wrongPasswordCallback2 == null)
						{
							return;
						}
						wrongPasswordCallback2();
					}
				}
			};
			return new OBDRequest[] { obdrequest, req_sendKey };
		}

		// Token: 0x0200087C RID: 2172
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06004A10 RID: 18960 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06004A11 RID: 18961 RVA: 0x0037C5B8 File Offset: 0x0037A7B8
			internal void <GetAccessKeysRequests>b__0(OBDRequest req_getSeed2, byte[] getSeedData, bool getSeedDataResults, string responseHeader)
			{
				try
				{
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(getSeedData);
					}
					int num = BitConverter.ToInt32(getSeedData, 0);
					if (num == 0)
					{
						this.zero_seed = true;
					}
					if (!this.zero_seed)
					{
						string text = (12 - num).ToString("X8");
						this.req_sendKey.Command = "2702" + text;
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_SendingPassword"));
						}
					}
					else
					{
						this.req_sendKey.Command = "3E";
					}
				}
				catch (Exception)
				{
					App.OBDReader.DebugWrite("\nerror_wrong_seed\n");
					this.req_sendKey.Command = "3E";
				}
			}

			// Token: 0x06004A12 RID: 18962 RVA: 0x0037C678 File Offset: 0x0037A878
			internal void <GetAccessKeysRequests>b__1(OBDRequest request, string data)
			{
				if (request.Command != "3E")
				{
					if (data != null)
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
					}
					if ((data != null && data.Contains("6702")) || SharedSettings.Current.IgnoreCodingErrors)
					{
						IProgress<string> progress = this.progress;
						if (progress == null)
						{
							return;
						}
						progress.Report(Translate.GetString("coding_progress_SuccessPassword"));
						return;
					}
					else
					{
						Action action = this.wrongPasswordCallback;
						if (action == null)
						{
							return;
						}
						action();
					}
				}
			}

			// Token: 0x04002AD7 RID: 10967
			public bool zero_seed;

			// Token: 0x04002AD8 RID: 10968
			public OBDRequest req_sendKey;

			// Token: 0x04002AD9 RID: 10969
			public IProgress<string> progress;

			// Token: 0x04002ADA RID: 10970
			public Action wrongPasswordCallback;
		}
	}
}
