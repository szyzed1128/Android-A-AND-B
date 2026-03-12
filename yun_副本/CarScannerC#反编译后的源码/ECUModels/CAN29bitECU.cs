using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004C8 RID: 1224
	internal class CAN29bitECU : CAN11bitECU
	{
		// Token: 0x06003063 RID: 12387 RVA: 0x00217CB8 File Offset: 0x00215EB8
		public CAN29bitECU(string name, string requestHeader, string responseHeader)
		{
			this.Name = name;
			this.Protocol = 7;
			if (requestHeader.Length == 8)
			{
				this.CANPriority = requestHeader.Substring(0, 2);
				base.RequestHeader = requestHeader.Substring(2);
			}
			else if (requestHeader.Length == 6)
			{
				base.RequestHeader = requestHeader;
			}
			base.ResponseHeader = responseHeader;
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x00217D28 File Offset: 0x00215F28
		public CAN29bitECU(string name, string canPriority, string requestHeader, string responseHeader)
			: this(name, canPriority + requestHeader, responseHeader)
		{
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x00217D3C File Offset: 0x00215F3C
		protected override OBDRequest GetATFCTestRequest(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback)
		{
			OBDRequest obdrequest = new OBDRequest("ATFCSH" + this.CANPriority + base.RequestHeader, "", new string[] { "ATSP" + this.Protocol.ToString("X1") }, new string[0], false);
			obdrequest.DoNotDecode = true;
			Predicate<OBDRequest> <>9__1;
			Action <>9__2;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (OBDRequest x) => x.Header == this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in otherECUs)
					{
						if (((!string.IsNullOrEmpty(iecu.RequestHeader) && iecu.GetRequestForCommand("03").BeforeCommands.Contains("ATFCSH" + iecu.RequestHeader)) || !string.IsNullOrEmpty(iecu.ExtendedAddress)) && !(iecu is OBD2Can11bitECU))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							Action badELMDetectedCallback2 = badELMDetectedCallback;
							if (badELMDetectedCallback2 == null)
							{
								return;
							}
							badELMDetectedCallback2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			};
			return obdrequest;
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06003066 RID: 12390 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		public override ELMFormat ELMFormat
		{
			get
			{
				return ELMFormat.CAN29bit;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06003067 RID: 12391 RVA: 0x00217DCA File Offset: 0x00215FCA
		// (set) Token: 0x06003068 RID: 12392 RVA: 0x00217DD2 File Offset: 0x00215FD2
		public override int Protocol
		{
			[CompilerGenerated]
			get
			{
				return this.<Protocol>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Protocol>k__BackingField = value;
			}
		} = 7;

		// Token: 0x17001296 RID: 4758
		// (get) Token: 0x06003069 RID: 12393 RVA: 0x00217DDB File Offset: 0x00215FDB
		// (set) Token: 0x0600306A RID: 12394 RVA: 0x00217DE3 File Offset: 0x00215FE3
		public virtual string CANPriority
		{
			[CompilerGenerated]
			get
			{
				return this.<CANPriority>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CANPriority>k__BackingField = value;
			}
		} = "18";

		// Token: 0x0600306B RID: 12395 RVA: 0x00217DEC File Offset: 0x00215FEC
		public override OBDRequest GetRequestForCommand(string cmd)
		{
			string text = "";
			if (string.IsNullOrEmpty(base.ResponseHeader) || base.RequestHeader == "DB33F1")
			{
				if (SharedSettings.Current.UseDefaultInit || !SharedSettings.Current.CustomInitString.Contains("ATCRA"))
				{
					text = "ATAR;";
				}
			}
			else
			{
				text = "ATCRA" + base.ResponseHeader + ";";
			}
			string text2;
			if (string.IsNullOrEmpty(base.RequestHeader) || base.RequestHeader == "7DF")
			{
				text2 = "ATFCSM0;";
			}
			else if (string.IsNullOrEmpty(base.ExtendedAddress))
			{
				text2 = "ATFCSH" + this.CANPriority + base.RequestHeader + ";ATFCSD300005;ATFCSM1;";
			}
			else
			{
				text2 = string.Concat(new string[] { "ATFCSH", this.CANPriority, base.RequestHeader, ";ATFCSD", base.ExtendedAddress, "300005;ATFCSM1;" });
			}
			string text3 = "";
			if (!string.IsNullOrEmpty(base.ExtendedAddress))
			{
				text3 = "ATCEA" + base.ExtendedAddress + ";";
			}
			string text4 = "";
			if (!string.IsNullOrEmpty(base.TesterAddress))
			{
				text4 = "ATTA" + base.TesterAddress + ";";
			}
			string text5 = "";
			if (!string.IsNullOrEmpty(base.RequestHeader))
			{
				text5 = "ATSP" + this.Protocol.ToString("X1") + ";";
			}
			string text6 = "";
			if (!string.IsNullOrEmpty(this.CANPriority))
			{
				text6 = "ATCP" + this.CANPriority + ";";
			}
			string text7 = string.Concat(new string[] { text6, text5, base.AdditionalPreInit, ";", text2, text, text3, text4, ";" });
			foreach (string text8 in base.OpenSessionCommands)
			{
				text7 = text7 + text8 + ";";
			}
			string text9 = base.AdditionalPostInit + ";";
			foreach (string text10 in base.CloseSessionCommands)
			{
				text9 = text9 + text10 + ";";
			}
			text9 += "ATAR;ATFCSM0;ATCEA;ATSTDEF;ATSP0";
			return new OBDRequest(cmd, base.RequestHeader, text7, text9, false)
			{
				ELMFormat = ELMFormat.CAN29bit
			};
		}

		// Token: 0x04001C27 RID: 7207
		[CompilerGenerated]
		private int <Protocol>k__BackingField;

		// Token: 0x04001C28 RID: 7208
		[CompilerGenerated]
		private string <CANPriority>k__BackingField;

		// Token: 0x020004C9 RID: 1225
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600306C RID: 12396 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600306D RID: 12397 RVA: 0x002180C4 File Offset: 0x002162C4
			internal void <GetATFCTestRequest>b__0(OBDRequest request, string data)
			{
				if (data.Contains("?"))
				{
					OBDDataReader obdreader = App.OBDReader;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__1) == null)
					{
						predicate = (this.<>9__1 = (OBDRequest x) => x.Header == this.<>4__this.RequestHeader);
					}
					obdreader.RemoveFromQueue(predicate);
					foreach (IECU iecu in this.otherECUs)
					{
						if (((!string.IsNullOrEmpty(iecu.RequestHeader) && iecu.GetRequestForCommand("03").BeforeCommands.Contains("ATFCSH" + iecu.RequestHeader)) || !string.IsNullOrEmpty(iecu.ExtendedAddress)) && !(iecu is OBD2Can11bitECU))
						{
							iecu.IsSelected = false;
							iecu.Reset();
							this.<>4__this.BadELMDetected = true;
						}
					}
					Action action;
					if ((action = this.<>9__2) == null)
					{
						action = (this.<>9__2 = delegate
						{
							Action action2 = this.badELMDetectedCallback;
							if (action2 == null)
							{
								return;
							}
							action2();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			}

			// Token: 0x0600306E RID: 12398 RVA: 0x002181CC File Offset: 0x002163CC
			internal bool <GetATFCTestRequest>b__1(OBDRequest x)
			{
				return x.Header == this.<>4__this.RequestHeader;
			}

			// Token: 0x0600306F RID: 12399 RVA: 0x002181E4 File Offset: 0x002163E4
			internal void <GetATFCTestRequest>b__2()
			{
				Action action = this.badELMDetectedCallback;
				if (action == null)
				{
					return;
				}
				action();
			}

			// Token: 0x04001C29 RID: 7209
			public CAN29bitECU <>4__this;

			// Token: 0x04001C2A RID: 7210
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001C2B RID: 7211
			public Action badELMDetectedCallback;

			// Token: 0x04001C2C RID: 7212
			public Predicate<OBDRequest> <>9__1;

			// Token: 0x04001C2D RID: 7213
			public Action <>9__2;
		}
	}
}
