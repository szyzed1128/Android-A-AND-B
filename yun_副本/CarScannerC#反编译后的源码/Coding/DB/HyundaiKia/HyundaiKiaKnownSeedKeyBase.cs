using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BA6 RID: 2982
	internal class HyundaiKiaKnownSeedKeyBase
	{
		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x06005AB4 RID: 23220 RVA: 0x00434875 File Offset: 0x00432A75
		// (set) Token: 0x06005AB5 RID: 23221 RVA: 0x0043487D File Offset: 0x00432A7D
		public bool KeySuccess
		{
			[CompilerGenerated]
			get
			{
				return this.<KeySuccess>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<KeySuccess>k__BackingField = value;
			}
		}

		// Token: 0x06005AB6 RID: 23222 RVA: 0x00434886 File Offset: 0x00432A86
		public HyundaiKiaKnownSeedKeyBase(string requestHeader, string responseHeader, int attemptsMax = 10, int getSeedfunctionId = 1)
		{
			this.requestHeader = requestHeader;
			this.responseHeader = responseHeader;
			this.getSeedfunctionId = getSeedfunctionId;
			this.attemptsMax = attemptsMax;
		}

		// Token: 0x06005AB7 RID: 23223 RVA: 0x004348C0 File Offset: 0x00432AC0
		public OBDRequest GetRequest(string beforeCommands, string afterCommands)
		{
			OBDRequest obdrequest = new OBDRequest("27" + this.getSeedfunctionId.ToString("X2"), this.requestHeader, beforeCommands, afterCommands, false);
			obdrequest.DoNotDecode = false;
			this.attemptsCurrent = 0;
			obdrequest.Keys["MAX_ATTEMPTS"] = this.attemptsMax.ToString();
			obdrequest.ResponseReceived += this.Req_GetSeedResponseReceived;
			this.KeySuccess = false;
			obdrequest.ResponseDecoded += this.Req_ResponseDecoded;
			return obdrequest;
		}

		// Token: 0x06005AB8 RID: 23224 RVA: 0x0043494C File Offset: 0x00432B4C
		private void Req_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data == null && data.Length == 0)
			{
				if (this.attemptsCurrent < this.attemptsMax)
				{
					App.OBDReader.InsertRequestInQueue(request);
				}
				return;
			}
			string text = BitHelpers.ByteArrayToHexString(data);
			string text2;
			if (this.seedKeyDB.TryGetValue(text, out text2))
			{
				int funcSendKey = this.getSeedfunctionId + 1;
				OBDRequest obdrequest = new OBDRequest("27" + funcSendKey.ToString("X2") + text2, request.Header, request.BeforeCommands, request.AfterCommands, false);
				obdrequest.ResponseReceived += delegate(OBDRequest sendKeyReq2, string data2)
				{
					if (data2 != null && OBDDataReader.FilterHexAndNewLineOnly(data2).Contains("67" + funcSendKey.ToString("X2")))
					{
						this.KeySuccess = true;
					}
					if (!this.KeySuccess && this.attemptsCurrent < this.attemptsMax)
					{
						App.OBDReader.InsertRequestInQueue(request);
					}
				};
				App.OBDReader.InsertRequestInQueue(obdrequest);
				return;
			}
			if (this.attemptsCurrent < this.attemptsMax)
			{
				App.OBDReader.InsertRequestInQueue(request);
			}
		}

		// Token: 0x06005AB9 RID: 23225 RVA: 0x00434A37 File Offset: 0x00432C37
		private void Req_GetSeedResponseReceived(OBDRequest request, string data)
		{
			this.attemptsCurrent++;
			if (this.attemptsCurrent > this.attemptsMax)
			{
				request.DoNotDecode = true;
			}
		}

		// Token: 0x06005ABA RID: 23226 RVA: 0x00434A5C File Offset: 0x00432C5C
		public void FillDictionaryFromString(string input)
		{
			foreach (string text in input.Split(new char[] { '\n', '\r', ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (text.Contains('='))
				{
					string[] array2 = text.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
					if (array2.Length == 2)
					{
						this.seedKeyDB[array2[0]] = array2[1];
					}
				}
			}
		}

		// Token: 0x04003914 RID: 14612
		private Dictionary<string, string> seedKeyDB = new Dictionary<string, string>();

		// Token: 0x04003915 RID: 14613
		private string requestHeader;

		// Token: 0x04003916 RID: 14614
		private string responseHeader;

		// Token: 0x04003917 RID: 14615
		private int attemptsMax;

		// Token: 0x04003918 RID: 14616
		private int getSeedfunctionId = 1;

		// Token: 0x04003919 RID: 14617
		private int attemptsCurrent;

		// Token: 0x0400391A RID: 14618
		[CompilerGenerated]
		private bool <KeySuccess>k__BackingField;

		// Token: 0x0400391B RID: 14619
		private const string MAX_ATTEMPTS = "MAX_ATTEMPTS";

		// Token: 0x02000BA7 RID: 2983
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06005ABB RID: 23227 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06005ABC RID: 23228 RVA: 0x00434AC8 File Offset: 0x00432CC8
			internal void <Req_ResponseDecoded>b__0(OBDRequest sendKeyReq2, string data2)
			{
				if (data2 != null && OBDDataReader.FilterHexAndNewLineOnly(data2).Contains("67" + this.funcSendKey.ToString("X2")))
				{
					this.<>4__this.KeySuccess = true;
				}
				if (!this.<>4__this.KeySuccess && this.<>4__this.attemptsCurrent < this.<>4__this.attemptsMax)
				{
					App.OBDReader.InsertRequestInQueue(this.request);
				}
			}

			// Token: 0x0400391C RID: 14620
			public HyundaiKiaKnownSeedKeyBase <>4__this;

			// Token: 0x0400391D RID: 14621
			public OBDRequest request;

			// Token: 0x0400391E RID: 14622
			public int funcSendKey;
		}
	}
}
