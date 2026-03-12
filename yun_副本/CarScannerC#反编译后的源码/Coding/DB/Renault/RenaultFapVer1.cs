using System;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A01 RID: 2561
	internal class RenaultFapVer1 : RenaultService31Base
	{
		// Token: 0x060051F5 RID: 20981 RVA: 0x003F5D2C File Offset: 0x003F3F2C
		public RenaultFapVer1()
		{
			base.Name = Translate.GetString("codingDB_DpfServiceRegenerationWhileStanding_Name");
			base.Description = Translate.GetString("coding_Renault_Warning");
			base.InnerDescription = Translate.GetString("coding_Renault_DPFRegen_InnerDescription");
			this.Group = CodingGroup.EngineAndPowertrain;
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			this.PasswordVisible = false;
			base.CloseSessionCommand = "10C0";
			base.WriteModeAndAddress = "";
			this.ReadModeAndAddress = "";
			this.HasCurrentState = true;
			base.MakeChangesToInitialData = false;
			base.Protocol = "6";
			base.ATST = "DEF";
			this.opt_start = new MQBAdaptationOption(Translate.GetString("coding_Start"), "310D00");
			this.opt_stop = new MQBAdaptationOption(Translate.GetString("coding_Stop"), "320D00");
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(this.opt_start);
			this.Options.Add(this.opt_stop);
			this.RPM = double.NaN;
			this.COOLANT = double.NaN;
			this.SOOT = double.NaN;
		}

		// Token: 0x060051F6 RID: 20982 RVA: 0x003F5E64 File Offset: 0x003F4064
		protected override void OnStatusUpdateRequested()
		{
			OBDRequest obdrequest = new OBDRequest("22242C", "7E0", this.BeforeCommands, this.AfterCommands, true);
			obdrequest.ResponseDecoded += this.Soot_Request_ResponseDecoded;
			OBDRequest obdrequest2 = new OBDRequest("010C", true);
			obdrequest2.ResponseDecoded += this.Rpm_request_ResponseDecoded;
			OBDRequest obdrequest3 = new OBDRequest("0105", true);
			obdrequest3.ResponseDecoded += this.Coolant_request_ResponseDecoded;
			OBDRequest obdrequest4 = new OBDRequest("222434", "7E0", this.BeforeCommands, this.AfterCommands, true);
			obdrequest4.ResponseDecoded += this.RegenState_request_ResponseDecoded;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest4, obdrequest2, obdrequest3 });
		}

		// Token: 0x060051F7 RID: 20983 RVA: 0x003F5F2C File Offset: 0x003F412C
		private void RegenState_request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 1)
			{
				switch (data[0])
				{
				case 0:
					this.STATE = RenaultFapVer1.RegenStates.Wait_0;
					break;
				case 1:
					this.STATE = RenaultFapVer1.RegenStates.Heating_1;
					break;
				case 2:
					this.STATE = RenaultFapVer1.RegenStates.Regeneration_2;
					break;
				case 3:
					this.STATE = RenaultFapVer1.RegenStates.Cooling_3;
					break;
				case 4:
					this.STATE = RenaultFapVer1.RegenStates.SuccessFinish_4;
					break;
				case 5:
					this.STATE = RenaultFapVer1.RegenStates.Fail_5;
					break;
				}
			}
			this.UpdateStatusString();
		}

		// Token: 0x060051F8 RID: 20984 RVA: 0x003F5FA0 File Offset: 0x003F41A0
		private void Coolant_request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 1)
			{
				this.COOLANT = (double)(data[0] - 40);
			}
			else
			{
				this.COOLANT = double.NaN;
			}
			this.UpdateStatusString();
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x003F5FD0 File Offset: 0x003F41D0
		private void Rpm_request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 2)
			{
				double num = (double)((int)data[0] * 256 + (int)data[1]);
				this.RPM = num / 4.0;
			}
			else
			{
				this.RPM = double.NaN;
			}
			this.UpdateStatusString();
		}

		// Token: 0x060051FA RID: 20986 RVA: 0x003F6020 File Offset: 0x003F4220
		private void Soot_Request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 2)
			{
				double num = (double)((int)data[0] * 256 + (int)data[1]);
				this.SOOT = num / 100.0;
			}
			else
			{
				this.SOOT = double.NaN;
			}
			this.UpdateStatusString();
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x003F6070 File Offset: 0x003F4270
		private void UpdateStatusString()
		{
			StringBuilder stringBuilder = new StringBuilder(16);
			stringBuilder.Append("\n");
			stringBuilder.Append(Translate.GetString("coding_Renault_RegenState"));
			stringBuilder.Append(" ");
			switch (this.STATE)
			{
			case RenaultFapVer1.RegenStates.Wait_0:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_waiting"));
				break;
			case RenaultFapVer1.RegenStates.Heating_1:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_heating"));
				break;
			case RenaultFapVer1.RegenStates.Regeneration_2:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_regeneration"));
				break;
			case RenaultFapVer1.RegenStates.Cooling_3:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_cooling"));
				break;
			case RenaultFapVer1.RegenStates.SuccessFinish_4:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_success"));
				break;
			case RenaultFapVer1.RegenStates.Fail_5:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_fail"));
				break;
			}
			stringBuilder.Append("\n");
			stringBuilder.Append(Translate.GetString("PID_010C"));
			stringBuilder.Append(": ");
			if (double.IsNaN(this.RPM))
			{
				stringBuilder.Append("n/a");
			}
			else
			{
				stringBuilder.Append(this.RPM.ToString());
			}
			stringBuilder.Append("\n");
			stringBuilder.Append(Translate.GetString("PID_0105"));
			stringBuilder.Append(": ");
			if (double.IsNaN(this.COOLANT))
			{
				stringBuilder.Append("n/a");
			}
			else
			{
				stringBuilder.Append(this.COOLANT.ToString());
			}
			stringBuilder.Append("\n");
			stringBuilder.Append("DPF Soot: ");
			if (double.IsNaN(this.SOOT))
			{
				stringBuilder.Append("n/a");
			}
			else
			{
				stringBuilder.Append(this.SOOT.ToString());
			}
			string result = stringBuilder.ToString();
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				this.CurrentState = result;
			});
		}

		// Token: 0x040031D5 RID: 12757
		private MQBAdaptationOption opt_start;

		// Token: 0x040031D6 RID: 12758
		private MQBAdaptationOption opt_stop;

		// Token: 0x040031D7 RID: 12759
		private double RPM;

		// Token: 0x040031D8 RID: 12760
		private double COOLANT;

		// Token: 0x040031D9 RID: 12761
		private double SOOT;

		// Token: 0x040031DA RID: 12762
		private RenaultFapVer1.RegenStates STATE;

		// Token: 0x02000A02 RID: 2562
		private enum RegenStates
		{
			// Token: 0x040031DC RID: 12764
			Wait_0,
			// Token: 0x040031DD RID: 12765
			Heating_1,
			// Token: 0x040031DE RID: 12766
			Regeneration_2,
			// Token: 0x040031DF RID: 12767
			Cooling_3,
			// Token: 0x040031E0 RID: 12768
			SuccessFinish_4,
			// Token: 0x040031E1 RID: 12769
			Fail_5
		}

		// Token: 0x02000A03 RID: 2563
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x060051FC RID: 20988 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x060051FD RID: 20989 RVA: 0x003F6260 File Offset: 0x003F4460
			internal void <UpdateStatusString>b__0()
			{
				this.<>4__this.CurrentState = this.result;
			}

			// Token: 0x040031E2 RID: 12770
			public RenaultFapVer1 <>4__this;

			// Token: 0x040031E3 RID: 12771
			public string result;
		}
	}
}
