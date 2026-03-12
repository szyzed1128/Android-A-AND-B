using System;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.Subaru
{
	// Token: 0x020009F6 RID: 2550
	internal class SubaruResetSaS : CustomizableCodingTemplate
	{
		// Token: 0x060051BC RID: 20924 RVA: 0x003F3C18 File Offset: 0x003F1E18
		public SubaruResetSaS()
		{
			base.Name = "VSC (VDC) Steering Angle Centering Mode";
			base.Description = "Compatibility not tested, highly possible 2014-2019 MY";
			base.InnerDescription = "C1711 steering angle sensor\r\nC0071 steering angle sensor malfunction\r\nNO SIGNAL FROM STEERING ANGLE SENSOR";
			base.RequestHeader = "7B0";
			base.ResponseHeader = "7B8";
			this.Options.Add(new MQBAdaptationOption("Start", "24", new TranslationItem[]
			{
				new TranslationItem("ru", "Запуск", "", "")
			}));
			base.MakeChangesToInitialData = false;
			base.WriteModeAndAddress = "310100";
			this.ReadModeAndAddress = "";
			base.PostWriteCommands = "3E;3E;31030024";
			this.PasswordVisible = false;
		}

		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x060051BD RID: 20925 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x060051BE RID: 20926 RVA: 0x003F0A8F File Offset: 0x003EEC8F
		public override bool HasCurrentState
		{
			get
			{
				return true;
			}
			set
			{
				base.HasCurrentState = value;
			}
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x003F3CD0 File Offset: 0x003F1ED0
		public override Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			OBDRequest obdrequest = new OBDRequest("221029", "7B0", "", "", true);
			obdrequest.ResponseDecoded += this.Req_ResponseDecoded;
			App.OBDReader.ReplaceQueue(obdrequest);
			return base.UpdateCurrentState(password, progress);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x003F3D20 File Offset: 0x003F1F20
		private void Req_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length >= 2)
			{
				short num = (short)((int)data[0] * 256 + (int)data[1]);
				base.CurrentState = string.Format("Current angle = {0}°", num);
				return;
			}
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
		}
	}
}
