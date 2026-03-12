using System;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A04 RID: 2564
	internal class RenaultFapVer2 : RenaultService31Base
	{
		// Token: 0x060051FE RID: 20990 RVA: 0x003F6274 File Offset: 0x003F4474
		public RenaultFapVer2()
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
			this.opt_start = new MQBAdaptationOption(Translate.GetString("coding_Start"), "3101020D00");
			this.opt_stop = new MQBAdaptationOption(Translate.GetString("coding_Stop"), "3102020D00");
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(this.opt_start);
			this.Options.Add(this.opt_stop);
			this.RPM = double.NaN;
			this.COOLANT = double.NaN;
			this.SOOT = double.NaN;
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x003F63BC File Offset: 0x003F45BC
		protected override void OnStatusUpdateRequested()
		{
			OBDRequest obdrequest = new OBDRequest("22242C", "7E0", this.BeforeCommands, this.AfterCommands, true);
			obdrequest.ResponseDecoded += this.Soot_Request_ResponseDecoded;
			OBDRequest obdrequest2 = new OBDRequest("010C", true);
			obdrequest2.ResponseDecoded += this.Rpm_request_ResponseDecoded;
			OBDRequest obdrequest3 = new OBDRequest("0105", true);
			obdrequest3.ResponseDecoded += this.Coolant_request_ResponseDecoded;
			OBDRequest obdrequest4 = new OBDRequest(this.regenStateCommand, "7E0", this.BeforeCommands, this.AfterCommands, true);
			obdrequest4.ResponseDecoded += this.RegenState_request_ResponseDecoded;
			obdrequest4.ResponseMarker = "71";
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest4, obdrequest2, obdrequest3 });
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x003F6490 File Offset: 0x003F4690
		protected virtual void RegenState_request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 4)
			{
				switch (data[3])
				{
				case 0:
					this.STATE = RenaultFapVer2.RegenStates.Finished;
					break;
				case 1:
					this.STATE = RenaultFapVer2.RegenStates.Aborted;
					break;
				case 2:
					this.STATE = RenaultFapVer2.RegenStates.Started;
					break;
				default:
					this.STATE = RenaultFapVer2.RegenStates.Unknown;
					break;
				}
			}
			this.UpdateStatusString();
		}

		// Token: 0x06005201 RID: 20993 RVA: 0x003F64E8 File Offset: 0x003F46E8
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

		// Token: 0x06005202 RID: 20994 RVA: 0x003F6535 File Offset: 0x003F4735
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

		// Token: 0x06005203 RID: 20995 RVA: 0x003F6564 File Offset: 0x003F4764
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

		// Token: 0x06005204 RID: 20996 RVA: 0x003F65B4 File Offset: 0x003F47B4
		protected void UpdateStatusString()
		{
			StringBuilder stringBuilder = new StringBuilder(16);
			stringBuilder.Append("\n");
			stringBuilder.Append(Translate.GetString("coding_Renault_RegenState"));
			stringBuilder.Append(" ");
			switch (this.STATE)
			{
			case RenaultFapVer2.RegenStates.Finished:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_finished_not_started"));
				break;
			case RenaultFapVer2.RegenStates.Aborted:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_aborted"));
				break;
			case RenaultFapVer2.RegenStates.Started:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_started"));
				break;
			case RenaultFapVer2.RegenStates.Unknown:
				stringBuilder.Append(Translate.GetString("coding_Renault_RegenState_unknown"));
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

		// Token: 0x040031E4 RID: 12772
		protected string regenStateCommand = "3103020D00";

		// Token: 0x040031E5 RID: 12773
		protected MQBAdaptationOption opt_start;

		// Token: 0x040031E6 RID: 12774
		protected MQBAdaptationOption opt_stop;

		// Token: 0x040031E7 RID: 12775
		private double RPM;

		// Token: 0x040031E8 RID: 12776
		private double COOLANT;

		// Token: 0x040031E9 RID: 12777
		private double SOOT;

		// Token: 0x040031EA RID: 12778
		protected RenaultFapVer2.RegenStates STATE = RenaultFapVer2.RegenStates.Unknown;

		// Token: 0x02000A05 RID: 2565
		protected enum RegenStates
		{
			// Token: 0x040031EC RID: 12780
			Finished,
			// Token: 0x040031ED RID: 12781
			Aborted,
			// Token: 0x040031EE RID: 12782
			Started,
			// Token: 0x040031EF RID: 12783
			Unknown
		}

		// Token: 0x02000A06 RID: 2566
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x06005205 RID: 20997 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x06005206 RID: 20998 RVA: 0x003F6776 File Offset: 0x003F4976
			internal void <UpdateStatusString>b__0()
			{
				this.<>4__this.CurrentState = this.result;
			}

			// Token: 0x040031F0 RID: 12784
			public RenaultFapVer2 <>4__this;

			// Token: 0x040031F1 RID: 12785
			public string result;
		}
	}
}
