using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000313 RID: 787
	internal class ELMState : INotifyPropertyChanged
	{
		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x001BDF10 File Offset: 0x001BC110
		// (set) Token: 0x0600241C RID: 9244 RVA: 0x001BDF18 File Offset: 0x001BC118
		public int Protocol
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
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x001BDF21 File Offset: 0x001BC121
		// (set) Token: 0x0600241E RID: 9246 RVA: 0x001BDF2C File Offset: 0x001BC12C
		public string ATST
		{
			get
			{
				return this._ATST;
			}
			private set
			{
				this._ATST = value;
				int num;
				if (int.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
				{
					this.ATSTms = num * 4;
				}
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x001BDF5D File Offset: 0x001BC15D
		// (set) Token: 0x06002420 RID: 9248 RVA: 0x001BDF65 File Offset: 0x001BC165
		public int ATSTms
		{
			[CompilerGenerated]
			get
			{
				return this.<ATSTms>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATSTms>k__BackingField = value;
			}
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x001BDF6E File Offset: 0x001BC16E
		// (set) Token: 0x06002422 RID: 9250 RVA: 0x001BDF76 File Offset: 0x001BC176
		public string Header
		{
			[CompilerGenerated]
			get
			{
				return this.<Header>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Header>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x001BDF7F File Offset: 0x001BC17F
		// (set) Token: 0x06002424 RID: 9252 RVA: 0x001BDF87 File Offset: 0x001BC187
		public bool DisplayHeaders
		{
			[CompilerGenerated]
			get
			{
				return this.<DisplayHeaders>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DisplayHeaders>k__BackingField = value;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x06002425 RID: 9253 RVA: 0x001BDF90 File Offset: 0x001BC190
		// (set) Token: 0x06002426 RID: 9254 RVA: 0x001BDF98 File Offset: 0x001BC198
		public bool InsertSpaces
		{
			[CompilerGenerated]
			get
			{
				return this.<InsertSpaces>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<InsertSpaces>k__BackingField = value;
			}
		} = true;

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x001BDFA1 File Offset: 0x001BC1A1
		// (set) Token: 0x06002428 RID: 9256 RVA: 0x001BDFA9 File Offset: 0x001BC1A9
		public bool DisplayEcho
		{
			[CompilerGenerated]
			get
			{
				return this.<DisplayEcho>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DisplayEcho>k__BackingField = value;
			}
		} = true;

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x001BDFB2 File Offset: 0x001BC1B2
		// (set) Token: 0x0600242A RID: 9258 RVA: 0x001BDFBA File Offset: 0x001BC1BA
		public int AdaptiveTimings
		{
			[CompilerGenerated]
			get
			{
				return this.<AdaptiveTimings>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AdaptiveTimings>k__BackingField = value;
			}
		} = 1;

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x0600242B RID: 9259 RVA: 0x001BDFC3 File Offset: 0x001BC1C3
		// (set) Token: 0x0600242C RID: 9260 RVA: 0x001BDFCB File Offset: 0x001BC1CB
		public bool ATAL
		{
			[CompilerGenerated]
			get
			{
				return this.<ATAL>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATAL>k__BackingField = value;
			}
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x001BDFD4 File Offset: 0x001BC1D4
		// (set) Token: 0x0600242E RID: 9262 RVA: 0x001BDFDC File Offset: 0x001BC1DC
		public bool ResponsesOn
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponsesOn>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ResponsesOn>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x001BDFE5 File Offset: 0x001BC1E5
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x001BDFED File Offset: 0x001BC1ED
		public string TesterAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<TesterAddress>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TesterAddress>k__BackingField = value;
			}
		} = "F1";

		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x001BDFF6 File Offset: 0x001BC1F6
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x001BDFFE File Offset: 0x001BC1FE
		public bool LineFeeds
		{
			[CompilerGenerated]
			get
			{
				return this.<LineFeeds>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<LineFeeds>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x001BE007 File Offset: 0x001BC207
		// (set) Token: 0x06002434 RID: 9268 RVA: 0x001BE00F File Offset: 0x001BC20F
		public string ATCRA
		{
			get
			{
				return this._ATCRA;
			}
			private set
			{
				this._ATCRA = value;
				this._ATCM = value;
				this._ATCF = value;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x001BE026 File Offset: 0x001BC226
		// (set) Token: 0x06002436 RID: 9270 RVA: 0x001BE02E File Offset: 0x001BC22E
		public string ATCM
		{
			get
			{
				return this._ATCM;
			}
			private set
			{
				this._ATCM = value;
				this._ATCRA = "";
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06002437 RID: 9271 RVA: 0x001BE042 File Offset: 0x001BC242
		// (set) Token: 0x06002438 RID: 9272 RVA: 0x001BE04A File Offset: 0x001BC24A
		public string ATCF
		{
			get
			{
				return this._ATCF;
			}
			private set
			{
				this._ATCF = value;
				this._ATCRA = "";
			}
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06002439 RID: 9273 RVA: 0x001BE05E File Offset: 0x001BC25E
		// (set) Token: 0x0600243A RID: 9274 RVA: 0x001BE066 File Offset: 0x001BC266
		public bool DisplayDLC
		{
			[CompilerGenerated]
			get
			{
				return this.<DisplayDLC>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DisplayDLC>k__BackingField = value;
			}
		}

		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x0600243B RID: 9275 RVA: 0x001BE06F File Offset: 0x001BC26F
		// (set) Token: 0x0600243C RID: 9276 RVA: 0x001BE077 File Offset: 0x001BC277
		public ELMState.FlowControlModes FlowControlMode
		{
			[CompilerGenerated]
			get
			{
				return this.<FlowControlMode>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FlowControlMode>k__BackingField = value;
			}
		}

		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x0600243D RID: 9277 RVA: 0x001BE080 File Offset: 0x001BC280
		// (set) Token: 0x0600243E RID: 9278 RVA: 0x001BE088 File Offset: 0x001BC288
		public string FlowControlHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<FlowControlHeader>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FlowControlHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x001BE091 File Offset: 0x001BC291
		// (set) Token: 0x06002440 RID: 9280 RVA: 0x001BE099 File Offset: 0x001BC299
		public string FlowControlData
		{
			[CompilerGenerated]
			get
			{
				return this.<FlowControlData>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FlowControlData>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x001BE0A2 File Offset: 0x001BC2A2
		// (set) Token: 0x06002442 RID: 9282 RVA: 0x001BE0AA File Offset: 0x001BC2AA
		public string ATPB
		{
			[CompilerGenerated]
			get
			{
				return this.<ATPB>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATPB>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x06002443 RID: 9283 RVA: 0x001BE0B3 File Offset: 0x001BC2B3
		// (set) Token: 0x06002444 RID: 9284 RVA: 0x001BE0BB File Offset: 0x001BC2BB
		public string ATCEA
		{
			[CompilerGenerated]
			get
			{
				return this.<ATCEA>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATCEA>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x06002445 RID: 9285 RVA: 0x001BE0C4 File Offset: 0x001BC2C4
		// (set) Token: 0x06002446 RID: 9286 RVA: 0x001BE0CC File Offset: 0x001BC2CC
		public bool CANAutoFormat_CAF
		{
			[CompilerGenerated]
			get
			{
				return this.<CANAutoFormat_CAF>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CANAutoFormat_CAF>k__BackingField = value;
			}
		} = true;

		// Token: 0x1700114E RID: 4430
		// (get) Token: 0x06002447 RID: 9287 RVA: 0x001BE0D5 File Offset: 0x001BC2D5
		// (set) Token: 0x06002448 RID: 9288 RVA: 0x001BE0DD File Offset: 0x001BC2DD
		public bool CANFlowControl_CFC
		{
			[CompilerGenerated]
			get
			{
				return this.<CANFlowControl_CFC>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CANFlowControl_CFC>k__BackingField = value;
			}
		} = true;

		// Token: 0x1700114F RID: 4431
		// (get) Token: 0x06002449 RID: 9289 RVA: 0x001BE0E6 File Offset: 0x001BC2E6
		// (set) Token: 0x0600244A RID: 9290 RVA: 0x001BE0EE File Offset: 0x001BC2EE
		public string CAN29bitPriority
		{
			[CompilerGenerated]
			get
			{
				return this.<CAN29bitPriority>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CAN29bitPriority>k__BackingField = value;
			}
		} = "18";

		// Token: 0x17001150 RID: 4432
		// (get) Token: 0x0600244B RID: 9291 RVA: 0x001BE0F7 File Offset: 0x001BC2F7
		// (set) Token: 0x0600244C RID: 9292 RVA: 0x001BE0FF File Offset: 0x001BC2FF
		public bool STNTransmitSegmentation
		{
			[CompilerGenerated]
			get
			{
				return this.<STNTransmitSegmentation>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<STNTransmitSegmentation>k__BackingField = value;
			}
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x001BE108 File Offset: 0x001BC308
		public void SetSTNTransmitSegmentation(bool value)
		{
			this.STNTransmitSegmentation = value;
		}

		// Token: 0x17001151 RID: 4433
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x001BE111 File Offset: 0x001BC311
		// (set) Token: 0x0600244F RID: 9295 RVA: 0x001BE119 File Offset: 0x001BC319
		public bool STNReceiveSegmentation
		{
			[CompilerGenerated]
			get
			{
				return this.<STNReceiveSegmentation>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<STNReceiveSegmentation>k__BackingField = value;
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x001BE122 File Offset: 0x001BC322
		public void SetSTNReceiveSegmentation(bool value)
		{
			this.STNReceiveSegmentation = value;
		}

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06002451 RID: 9297 RVA: 0x001BE12C File Offset: 0x001BC32C
		// (remove) Token: 0x06002452 RID: 9298 RVA: 0x001BE164 File Offset: 0x001BC364
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x17001152 RID: 4434
		// (get) Token: 0x06002453 RID: 9299 RVA: 0x001BE199 File Offset: 0x001BC399
		// (set) Token: 0x06002454 RID: 9300 RVA: 0x001BE1A1 File Offset: 0x001BC3A1
		public bool VariableDLC
		{
			[CompilerGenerated]
			get
			{
				return this.<VariableDLC>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<VariableDLC>k__BackingField = value;
			}
		}

		// Token: 0x17001153 RID: 4435
		// (get) Token: 0x06002455 RID: 9301 RVA: 0x001BE1AA File Offset: 0x001BC3AA
		// (set) Token: 0x06002456 RID: 9302 RVA: 0x001BE1B2 File Offset: 0x001BC3B2
		public ELMState.KWPBaudRates KWPBaudRate
		{
			[CompilerGenerated]
			get
			{
				return this.<KWPBaudRate>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<KWPBaudRate>k__BackingField = value;
			}
		} = ELMState.KWPBaudRates.BAUD_10400;

		// Token: 0x17001154 RID: 4436
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x001BE1BB File Offset: 0x001BC3BB
		// (set) Token: 0x06002458 RID: 9304 RVA: 0x001BE1C3 File Offset: 0x001BC3C3
		public string ATIIA
		{
			[CompilerGenerated]
			get
			{
				return this.<ATIIA>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATIIA>k__BackingField = value;
			}
		} = "33";

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x001BE1CC File Offset: 0x001BC3CC
		// (set) Token: 0x0600245A RID: 9306 RVA: 0x001BE1D4 File Offset: 0x001BC3D4
		public bool ELMSupportsATS0
		{
			[CompilerGenerated]
			get
			{
				return this.<ELMSupportsATS0>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<ELMSupportsATS0>k__BackingField = value;
			}
		} = true;

		// Token: 0x0600245B RID: 9307 RVA: 0x001BE1E0 File Offset: 0x001BC3E0
		public bool HasSTFlowControlPair(string header1, string header2)
		{
			string text;
			return header1 != null && header2 != null && (this.STFlowControlPairs.TryGetValue(header1, out text) && text == header2);
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x001BE211 File Offset: 0x001BC411
		// (set) Token: 0x0600245D RID: 9309 RVA: 0x001BE219 File Offset: 0x001BC419
		public string LastSentCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<LastSentCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<LastSentCommand>k__BackingField = value;
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x001BE224 File Offset: 0x001BC424
		public void UpdateStatusFromCommand(string cmd)
		{
			if (cmd != null)
			{
				this.LastSentCommand = cmd;
			}
			try
			{
				if (cmd != null && cmd.Length >= 2 && (cmd[1] == 't' || cmd[1] == 'T'))
				{
					if (cmd.IndexOf(' ') >= 0)
					{
						cmd = cmd.Replace(" ", "");
					}
					cmd = cmd.ToUpperInvariant();
					if (cmd.StartsWith("ATSPA"))
					{
						this.Protocol = 0;
					}
					else if (cmd.StartsWith("ATSP") && cmd.Length == 5)
					{
						int num = int.Parse(cmd[4].ToString(), NumberStyles.HexNumber);
						if (num > 0 && num <= 12)
						{
							this.Protocol = num;
						}
						else
						{
							this.Protocol = 0;
						}
						if (App.OBDReader.STCommandsStupported)
						{
							this.STFlowControlPairs.Clear();
							this.FlowControlHeader = "";
							this.ATCRA = "";
							this.ATCM = "";
							this.FlowControlMode = ELMState.FlowControlModes.Auto_0;
						}
					}
					else if (cmd.StartsWith("ATST") && cmd.Length == 6)
					{
						this.ATST = cmd.Substring(4, 2);
					}
					else if (cmd.StartsWith("ATTA") && cmd.Length == 6)
					{
						this.TesterAddress = cmd.Substring(4, 2);
					}
					else if (cmd.StartsWith("ATCER") && cmd.Length == 7)
					{
						this.TesterAddress = cmd.Substring(5, 2);
					}
					else if (cmd.StartsWith("ATCRA"))
					{
						if (cmd.Length == 8)
						{
							this.ATCRA = cmd.Substring(5, 3);
						}
						else if (cmd.Length == 13)
						{
							this.ATCRA = cmd.Substring(5, 8);
						}
					}
					else if (cmd.StartsWith("ATCM"))
					{
						this.ATCM = cmd.Substring(4);
					}
					else if (cmd.StartsWith("ATCF"))
					{
						this.ATCF = cmd.Substring(4);
					}
					else if (cmd.StartsWith("ATSH"))
					{
						this.Header = cmd.Substring(4);
					}
					else if (cmd.StartsWith("ATFCSH"))
					{
						this.FlowControlHeader = cmd.Substring(6);
					}
					else if (cmd.StartsWith("ATFCSD"))
					{
						this.FlowControlData = cmd.Substring(6);
					}
					else if (cmd.StartsWith("ATPB"))
					{
						this.ATPB = cmd.Substring(4, 4);
					}
					else if (cmd.Length == 7 && cmd.StartsWith("ATCEA"))
					{
						this.ATCEA = cmd.Substring(5);
					}
					else if (cmd.StartsWith("ATCP"))
					{
						this.CAN29bitPriority = cmd.Substring(4, 2);
					}
					else if (cmd.StartsWith("STCFCPA"))
					{
						string[] array = cmd.Substring(7).Split(new char[] { ',' });
						if (array.Length == 2)
						{
							this.STFlowControlPairs[array[0]] = array[1];
						}
					}
					else if (cmd.StartsWith("VTFCTRA"))
					{
						string[] array2 = cmd.Substring(7).Split(new char[] { ',' });
						if (array2.Length == 2)
						{
							this.STFlowControlPairs[array2[0]] = array2[1];
						}
					}
					else if (cmd.StartsWith("STCAF1,"))
					{
						string text = cmd.Substring(7);
						this.ATCEA = text;
					}
					else if (cmd.StartsWith("STP") && cmd.Length == 5)
					{
						int num2;
						if (int.TryParse(cmd.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
						{
							this.Protocol = num2;
						}
					}
					else if (cmd != null)
					{
						switch (cmd.Length)
						{
						case 3:
						{
							char c = cmd[2];
							if (c != 'D')
							{
								if (c != 'Z')
								{
									goto IL_094A;
								}
								if (!(cmd == "ATZ"))
								{
									goto IL_094A;
								}
							}
							else if (!(cmd == "ATD"))
							{
								goto IL_094A;
							}
							break;
						}
						case 4:
						{
							char c = cmd[2];
							if (c <= 'H')
							{
								switch (c)
								{
								case 'A':
									if (cmd == "ATAL")
									{
										this.ATAL = true;
										goto IL_094A;
									}
									if (!(cmd == "ATAR"))
									{
										goto IL_094A;
									}
									this.ATCRA = "";
									this.ATCM = "";
									this.ATCF = "";
									goto IL_094A;
								case 'B':
								case 'C':
									goto IL_094A;
								case 'D':
									if (cmd == "ATD0")
									{
										this.DisplayDLC = false;
										goto IL_094A;
									}
									if (!(cmd == "ATD1"))
									{
										goto IL_094A;
									}
									this.DisplayDLC = true;
									goto IL_094A;
								case 'E':
									if (cmd == "ATE0")
									{
										this.DisplayEcho = false;
										goto IL_094A;
									}
									if (!(cmd == "ATE1"))
									{
										goto IL_094A;
									}
									this.DisplayEcho = true;
									goto IL_094A;
								default:
									if (c != 'H')
									{
										goto IL_094A;
									}
									if (cmd == "ATH0")
									{
										this.DisplayHeaders = false;
										goto IL_094A;
									}
									if (!(cmd == "ATH1"))
									{
										goto IL_094A;
									}
									this.DisplayHeaders = true;
									goto IL_094A;
								}
							}
							else if (c != 'L')
							{
								if (c != 'N')
								{
									switch (c)
									{
									case 'R':
										if (cmd == "ATR0")
										{
											this.ResponsesOn = false;
											goto IL_094A;
										}
										if (!(cmd == "ATR1"))
										{
											goto IL_094A;
										}
										this.ResponsesOn = true;
										goto IL_094A;
									case 'S':
										if (cmd == "ATS0")
										{
											this.InsertSpaces = false;
											goto IL_094A;
										}
										if (!(cmd == "ATS1"))
										{
											goto IL_094A;
										}
										this.InsertSpaces = true;
										goto IL_094A;
									case 'T':
									case 'U':
										goto IL_094A;
									case 'V':
										if (cmd == "ATV1")
										{
											this.VariableDLC = true;
											goto IL_094A;
										}
										if (!(cmd == "ATV0"))
										{
											goto IL_094A;
										}
										this.VariableDLC = false;
										goto IL_094A;
									case 'W':
										if (!(cmd == "ATWS"))
										{
											goto IL_094A;
										}
										break;
									default:
										goto IL_094A;
									}
								}
								else
								{
									if (!(cmd == "ATNL"))
									{
										goto IL_094A;
									}
									this.ATAL = false;
									goto IL_094A;
								}
							}
							else
							{
								if (cmd == "ATL0")
								{
									this.LineFeeds = false;
									goto IL_094A;
								}
								if (!(cmd == "ATL1"))
								{
									goto IL_094A;
								}
								this.LineFeeds = true;
								goto IL_094A;
							}
							break;
						}
						case 5:
						{
							char c = cmd[4];
							switch (c)
							{
							case '0':
								if (!(cmd == "ATAT0"))
								{
									goto IL_094A;
								}
								this.AdaptiveTimings = 0;
								goto IL_094A;
							case '1':
								if (!(cmd == "ATAT1"))
								{
									goto IL_094A;
								}
								this.AdaptiveTimings = 1;
								goto IL_094A;
							case '2':
								if (!(cmd == "ATAT2"))
								{
									goto IL_094A;
								}
								this.AdaptiveTimings = 2;
								goto IL_094A;
							default:
								if (c != 'A')
								{
									goto IL_094A;
								}
								if (!(cmd == "ATCEA"))
								{
									goto IL_094A;
								}
								this.ATCEA = "";
								goto IL_094A;
							}
							break;
						}
						case 6:
						{
							char c = cmd[4];
							if (c <= '4')
							{
								if (c != '1')
								{
									if (c != '4')
									{
										goto IL_094A;
									}
									if (!(cmd == "ATIB48"))
									{
										goto IL_094A;
									}
									this.KWPBaudRate = ELMState.KWPBaudRates.BAUD_4800;
									goto IL_094A;
								}
								else
								{
									if (!(cmd == "ATIB10"))
									{
										goto IL_094A;
									}
									this.KWPBaudRate = ELMState.KWPBaudRates.BAUD_10400;
									goto IL_094A;
								}
							}
							else if (c != '9')
							{
								if (c != 'C')
								{
									if (c != 'F')
									{
										goto IL_094A;
									}
									if (cmd == "ATCAF0")
									{
										this.CANAutoFormat_CAF = false;
										goto IL_094A;
									}
									if (cmd == "ATCAF1")
									{
										this.CANAutoFormat_CAF = true;
										goto IL_094A;
									}
									if (!(cmd == "STCAF0"))
									{
										goto IL_094A;
									}
									this.ATCEA = "";
									goto IL_094A;
								}
								else
								{
									if (cmd == "ATCFC0")
									{
										this.CANFlowControl_CFC = false;
										goto IL_094A;
									}
									if (!(cmd == "ATCFC1"))
									{
										goto IL_094A;
									}
									this.CANFlowControl_CFC = true;
									goto IL_094A;
								}
							}
							else
							{
								if (!(cmd == "ATIB96"))
								{
									goto IL_094A;
								}
								this.KWPBaudRate = ELMState.KWPBaudRates.BAUD_9600;
								goto IL_094A;
							}
							break;
						}
						case 7:
							switch (cmd[6])
							{
							case '0':
								if (!(cmd == "ATFCSM0"))
								{
									goto IL_094A;
								}
								this.FlowControlMode = ELMState.FlowControlModes.Auto_0;
								goto IL_094A;
							case '1':
								if (!(cmd == "ATFCSM1"))
								{
									goto IL_094A;
								}
								this.FlowControlMode = ELMState.FlowControlModes.UserDefinedFull_1;
								goto IL_094A;
							case '2':
								if (!(cmd == "ATFCSM2"))
								{
									goto IL_094A;
								}
								this.FlowControlMode = ELMState.FlowControlModes.UserDefinedData_2;
								goto IL_094A;
							default:
								goto IL_094A;
							}
							break;
						default:
							goto IL_094A;
						}
						this.Reset();
					}
				}
				IL_094A:;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x001BEB9C File Offset: 0x001BCD9C
		public void Reset()
		{
			App.OBDReader.DebugWrite("\n[ELM RESET]\n");
			this.ATAL = false;
			this.ATCEA = "";
			this.ATCRA = "";
			this.ATIIA = "";
			this.ATPB = "";
			this.ATST = "32";
			this.CAN29bitPriority = "18";
			this.CANAutoFormat_CAF = true;
			this.CANFlowControl_CFC = true;
			this.DisplayDLC = false;
			this.DisplayEcho = true;
			this.DisplayHeaders = false;
			this.FlowControlData = "";
			this.FlowControlHeader = "";
			this.FlowControlMode = ELMState.FlowControlModes.Auto_0;
			this.Header = "";
			this.InsertSpaces = true;
			this.KWPBaudRate = ELMState.KWPBaudRates.BAUD_10400;
			this.LineFeeds = true;
			this.Protocol = 0;
			this.ResponsesOn = true;
			this.TesterAddress = "";
			this.ELMSupportsATS0 = true;
			this.STNReceiveSegmentation = false;
			this.STNTransmitSegmentation = false;
			this.VariableDLC = false;
			this.ATCRA = "";
			this.ATCM = "";
			this.ATCF = "";
			this.STFlowControlPairs.Clear();
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x001BECC4 File Offset: 0x001BCEC4
		protected void OnPropertyChanged(string propertyName)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			});
		}

		// Token: 0x17001157 RID: 4439
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x001BECE9 File Offset: 0x001BCEE9
		// (set) Token: 0x06002462 RID: 9314 RVA: 0x001BECF1 File Offset: 0x001BCEF1
		public int NoFinishCharacterErrors
		{
			get
			{
				return this._NoFinishCharacterErrors;
			}
			set
			{
				if (this._NoFinishCharacterErrors != value)
				{
					this._NoFinishCharacterErrors = value;
					this.OnPropertyChanged("NoFinishCharacterErrors");
				}
			}
		}

		// Token: 0x17001158 RID: 4440
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x001BED0E File Offset: 0x001BCF0E
		// (set) Token: 0x06002464 RID: 9316 RVA: 0x001BED16 File Offset: 0x001BCF16
		public int WrongNewLineCharactersCounter
		{
			get
			{
				return this._WrongNewLineCharactersCounter;
			}
			set
			{
				if (value != this._WrongNewLineCharactersCounter)
				{
					this._WrongNewLineCharactersCounter = value;
					this.OnPropertyChanged("WrongNewLineCharactersCounter");
				}
			}
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x001BED33 File Offset: 0x001BCF33
		// (set) Token: 0x06002466 RID: 9318 RVA: 0x001BED3B File Offset: 0x001BCF3B
		public int LostCANMultiframeCounter
		{
			get
			{
				return this._LostCANMultiframeCounter;
			}
			set
			{
				if (value != this._LostCANMultiframeCounter)
				{
					this._LostCANMultiframeCounter = value;
					this.OnPropertyChanged("LostCANMultiframeCounter");
				}
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x001BED58 File Offset: 0x001BCF58
		// (set) Token: 0x06002468 RID: 9320 RVA: 0x001BED60 File Offset: 0x001BCF60
		public bool ELMUnexpectedResetDetected
		{
			get
			{
				return this._ELMUnexpectedResetDetected;
			}
			set
			{
				if (value != this._ELMUnexpectedResetDetected)
				{
					this._ELMUnexpectedResetDetected = value;
					this.OnPropertyChanged("ELMUnexpectedResetDetected");
				}
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x001BED80 File Offset: 0x001BCF80
		public void CheckForErrors(string response)
		{
			if (response == null)
			{
				return;
			}
			if (!response.EndsWith("\r\r>", StringComparison.Ordinal) || response.Contains("\r\r\r", StringComparison.Ordinal))
			{
				int num = this.WrongNewLineCharactersCounter;
				this.WrongNewLineCharactersCounter = num + 1;
			}
			if (this.LastSentCommand.StartsWith("AT", StringComparison.OrdinalIgnoreCase) && response.Contains('?', StringComparison.Ordinal))
			{
				this.ReportATCommandNotSupported(this.LastSentCommand);
			}
			if (!response.EndsWith('>'))
			{
				int num = this.NoFinishCharacterErrors;
				this.NoFinishCharacterErrors = num + 1;
			}
		}

		// Token: 0x1700115B RID: 4443
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x001BEE01 File Offset: 0x001BD001
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x001BEE09 File Offset: 0x001BD009
		public HashSet<string> ATCommandsNotSupported
		{
			[CompilerGenerated]
			get
			{
				return this.<ATCommandsNotSupported>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ATCommandsNotSupported>k__BackingField = value;
			}
		} = new HashSet<string>();

		// Token: 0x0600246C RID: 9324 RVA: 0x001BEE12 File Offset: 0x001BD012
		public void ReportATCommandNotSupported(string cmd)
		{
			if (!this.ATCommandsNotSupported.Contains(cmd))
			{
				this.ATCommandsNotSupported.Add(cmd);
			}
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x001BEE2F File Offset: 0x001BD02F
		public void ResetErrorsState()
		{
			this.NoFinishCharacterErrors = 0;
			this.WrongNewLineCharactersCounter = 0;
			this.ATCommandsNotSupported.Clear();
			this.LostCANMultiframeCounter = 0;
			this.ELMUnexpectedResetDetected = false;
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x001BEE58 File Offset: 0x001BD058
		public ELMState()
		{
		}

		// Token: 0x040011D8 RID: 4568
		[CompilerGenerated]
		private int <Protocol>k__BackingField;

		// Token: 0x040011D9 RID: 4569
		private string _ATST = "32";

		// Token: 0x040011DA RID: 4570
		[CompilerGenerated]
		private int <ATSTms>k__BackingField;

		// Token: 0x040011DB RID: 4571
		[CompilerGenerated]
		private string <Header>k__BackingField;

		// Token: 0x040011DC RID: 4572
		[CompilerGenerated]
		private bool <DisplayHeaders>k__BackingField;

		// Token: 0x040011DD RID: 4573
		[CompilerGenerated]
		private bool <InsertSpaces>k__BackingField;

		// Token: 0x040011DE RID: 4574
		[CompilerGenerated]
		private bool <DisplayEcho>k__BackingField;

		// Token: 0x040011DF RID: 4575
		[CompilerGenerated]
		private int <AdaptiveTimings>k__BackingField;

		// Token: 0x040011E0 RID: 4576
		[CompilerGenerated]
		private bool <ATAL>k__BackingField;

		// Token: 0x040011E1 RID: 4577
		[CompilerGenerated]
		private bool <ResponsesOn>k__BackingField;

		// Token: 0x040011E2 RID: 4578
		[CompilerGenerated]
		private string <TesterAddress>k__BackingField;

		// Token: 0x040011E3 RID: 4579
		[CompilerGenerated]
		private bool <LineFeeds>k__BackingField;

		// Token: 0x040011E4 RID: 4580
		private string _ATCRA = "";

		// Token: 0x040011E5 RID: 4581
		private string _ATCM = "";

		// Token: 0x040011E6 RID: 4582
		private string _ATCF = "";

		// Token: 0x040011E7 RID: 4583
		[CompilerGenerated]
		private bool <DisplayDLC>k__BackingField;

		// Token: 0x040011E8 RID: 4584
		[CompilerGenerated]
		private ELMState.FlowControlModes <FlowControlMode>k__BackingField;

		// Token: 0x040011E9 RID: 4585
		[CompilerGenerated]
		private string <FlowControlHeader>k__BackingField;

		// Token: 0x040011EA RID: 4586
		[CompilerGenerated]
		private string <FlowControlData>k__BackingField;

		// Token: 0x040011EB RID: 4587
		[CompilerGenerated]
		private string <ATPB>k__BackingField;

		// Token: 0x040011EC RID: 4588
		[CompilerGenerated]
		private string <ATCEA>k__BackingField;

		// Token: 0x040011ED RID: 4589
		[CompilerGenerated]
		private bool <CANAutoFormat_CAF>k__BackingField;

		// Token: 0x040011EE RID: 4590
		[CompilerGenerated]
		private bool <CANFlowControl_CFC>k__BackingField;

		// Token: 0x040011EF RID: 4591
		[CompilerGenerated]
		private string <CAN29bitPriority>k__BackingField;

		// Token: 0x040011F0 RID: 4592
		[CompilerGenerated]
		private bool <STNTransmitSegmentation>k__BackingField;

		// Token: 0x040011F1 RID: 4593
		[CompilerGenerated]
		private bool <STNReceiveSegmentation>k__BackingField;

		// Token: 0x040011F2 RID: 4594
		private Dictionary<string, string> STFlowControlPairs = new Dictionary<string, string>(1);

		// Token: 0x040011F3 RID: 4595
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040011F4 RID: 4596
		[CompilerGenerated]
		private bool <VariableDLC>k__BackingField;

		// Token: 0x040011F5 RID: 4597
		[CompilerGenerated]
		private ELMState.KWPBaudRates <KWPBaudRate>k__BackingField;

		// Token: 0x040011F6 RID: 4598
		[CompilerGenerated]
		private string <ATIIA>k__BackingField;

		// Token: 0x040011F7 RID: 4599
		[CompilerGenerated]
		private bool <ELMSupportsATS0>k__BackingField;

		// Token: 0x040011F8 RID: 4600
		[CompilerGenerated]
		private string <LastSentCommand>k__BackingField;

		// Token: 0x040011F9 RID: 4601
		private int _NoFinishCharacterErrors;

		// Token: 0x040011FA RID: 4602
		private int _WrongNewLineCharactersCounter;

		// Token: 0x040011FB RID: 4603
		private int _LostCANMultiframeCounter;

		// Token: 0x040011FC RID: 4604
		private bool _ELMUnexpectedResetDetected;

		// Token: 0x040011FD RID: 4605
		[CompilerGenerated]
		private HashSet<string> <ATCommandsNotSupported>k__BackingField;

		// Token: 0x02000314 RID: 788
		public enum FlowControlModes
		{
			// Token: 0x040011FF RID: 4607
			Auto_0,
			// Token: 0x04001200 RID: 4608
			UserDefinedFull_1,
			// Token: 0x04001201 RID: 4609
			UserDefinedData_2
		}

		// Token: 0x02000315 RID: 789
		public enum KWPBaudRates
		{
			// Token: 0x04001203 RID: 4611
			BAUD_10400 = 10,
			// Token: 0x04001204 RID: 4612
			BAUD_4800 = 48,
			// Token: 0x04001205 RID: 4613
			BAUD_9600 = 96
		}

		// Token: 0x02000316 RID: 790
		[CompilerGenerated]
		private sealed class <>c__DisplayClass135_0
		{
			// Token: 0x0600246F RID: 9327 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass135_0()
			{
			}

			// Token: 0x06002470 RID: 9328 RVA: 0x001BEF46 File Offset: 0x001BD146
			internal void <OnPropertyChanged>b__0()
			{
				PropertyChangedEventHandler propertyChanged = this.<>4__this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.propertyName));
			}

			// Token: 0x04001206 RID: 4614
			public ELMState <>4__this;

			// Token: 0x04001207 RID: 4615
			public string propertyName;
		}
	}
}
