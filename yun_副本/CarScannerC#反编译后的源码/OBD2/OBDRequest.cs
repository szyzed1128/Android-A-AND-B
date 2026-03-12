using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms.Internals;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200038F RID: 911
	[DebuggerDisplay("{Header ?? \"\"}:{Command}")]
	public class OBDRequest
	{
		// Token: 0x17001187 RID: 4487
		// (get) Token: 0x06002669 RID: 9833 RVA: 0x001DC739 File Offset: 0x001DA939
		// (set) Token: 0x0600266A RID: 9834 RVA: 0x001DC741 File Offset: 0x001DA941
		public string Command
		{
			[CompilerGenerated]
			get
			{
				return this.<Command>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Command>k__BackingField = value;
			}
		}

		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x0600266B RID: 9835 RVA: 0x001DC74A File Offset: 0x001DA94A
		// (set) Token: 0x0600266C RID: 9836 RVA: 0x001DC752 File Offset: 0x001DA952
		public string ResponseMarker
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseMarker>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseMarker>k__BackingField = value;
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x001DC75B File Offset: 0x001DA95B
		// (set) Token: 0x0600266E RID: 9838 RVA: 0x001DC763 File Offset: 0x001DA963
		public bool Repeat
		{
			[CompilerGenerated]
			get
			{
				return this.<Repeat>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Repeat>k__BackingField = value;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x0600266F RID: 9839 RVA: 0x001DC76C File Offset: 0x001DA96C
		// (set) Token: 0x06002670 RID: 9840 RVA: 0x001DC774 File Offset: 0x001DA974
		public string Header
		{
			get
			{
				return this._Header;
			}
			set
			{
				if (value == null)
				{
					this._Header = "";
					return;
				}
				this._Header = value;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06002671 RID: 9841 RVA: 0x001DC78C File Offset: 0x001DA98C
		// (set) Token: 0x06002672 RID: 9842 RVA: 0x001DC794 File Offset: 0x001DA994
		public string[] BeforeCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<BeforeCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BeforeCommands>k__BackingField = value;
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06002673 RID: 9843 RVA: 0x001DC79D File Offset: 0x001DA99D
		// (set) Token: 0x06002674 RID: 9844 RVA: 0x001DC7A5 File Offset: 0x001DA9A5
		public string[] AfterCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<AfterCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AfterCommands>k__BackingField = value;
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x001DC7AE File Offset: 0x001DA9AE
		public IReadOnlyList<PID> PIDs
		{
			get
			{
				return this._PIDs;
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06002676 RID: 9846 RVA: 0x001DC7B6 File Offset: 0x001DA9B6
		// (set) Token: 0x06002677 RID: 9847 RVA: 0x001DC7BE File Offset: 0x001DA9BE
		public bool DoNotDecode
		{
			[CompilerGenerated]
			get
			{
				return this.<DoNotDecode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DoNotDecode>k__BackingField = value;
			}
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x001DC7C7 File Offset: 0x001DA9C7
		// (set) Token: 0x06002679 RID: 9849 RVA: 0x001DC7CF File Offset: 0x001DA9CF
		public int NR78RepeatCounter
		{
			[CompilerGenerated]
			get
			{
				return this.<NR78RepeatCounter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NR78RepeatCounter>k__BackingField = value;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x001DC7D8 File Offset: 0x001DA9D8
		// (set) Token: 0x0600267B RID: 9851 RVA: 0x001DC7E0 File Offset: 0x001DA9E0
		public bool CheckLength
		{
			[CompilerGenerated]
			get
			{
				return this.<CheckLength>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CheckLength>k__BackingField = value;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x001DC7E9 File Offset: 0x001DA9E9
		// (set) Token: 0x0600267D RID: 9853 RVA: 0x001DC7F1 File Offset: 0x001DA9F1
		public string Payload
		{
			[CompilerGenerated]
			get
			{
				return this.<Payload>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Payload>k__BackingField = value;
			}
		}

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x001DC7FA File Offset: 0x001DA9FA
		// (set) Token: 0x0600267F RID: 9855 RVA: 0x001DC802 File Offset: 0x001DAA02
		public virtual ELMFormat ELMFormat
		{
			[CompilerGenerated]
			get
			{
				return this.<ELMFormat>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ELMFormat>k__BackingField = value;
			}
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x06002680 RID: 9856 RVA: 0x001DC80B File Offset: 0x001DAA0B
		// (set) Token: 0x06002681 RID: 9857 RVA: 0x001DC813 File Offset: 0x001DAA13
		public OBDDataReader.OBDModes OBDMode
		{
			[CompilerGenerated]
			get
			{
				return this.<OBDMode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OBDMode>k__BackingField = value;
			}
		}

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x06002682 RID: 9858 RVA: 0x001DC81C File Offset: 0x001DAA1C
		// (set) Token: 0x06002683 RID: 9859 RVA: 0x001DC824 File Offset: 0x001DAA24
		public Dictionary<string, string> Keys
		{
			[CompilerGenerated]
			get
			{
				return this.<Keys>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Keys>k__BackingField = value;
			}
		}

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x001DC82D File Offset: 0x001DAA2D
		// (set) Token: 0x06002685 RID: 9861 RVA: 0x001DC835 File Offset: 0x001DAA35
		public int SkipCyclesTarget
		{
			[CompilerGenerated]
			get
			{
				return this.<SkipCyclesTarget>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SkipCyclesTarget>k__BackingField = value;
			}
		}

		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x06002686 RID: 9862 RVA: 0x001DC83E File Offset: 0x001DAA3E
		// (set) Token: 0x06002687 RID: 9863 RVA: 0x001DC846 File Offset: 0x001DAA46
		public int SkippedCycles
		{
			[CompilerGenerated]
			get
			{
				return this.<SkippedCycles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SkippedCycles>k__BackingField = value;
			}
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x001DC84F File Offset: 0x001DAA4F
		public override string ToString()
		{
			return this.Header + ":" + this.Command;
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x06002689 RID: 9865 RVA: 0x001DC867 File Offset: 0x001DAA67
		// (set) Token: 0x0600268A RID: 9866 RVA: 0x001DC86F File Offset: 0x001DAA6F
		public IProgress<string> Progress
		{
			[CompilerGenerated]
			get
			{
				return this.<Progress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Progress>k__BackingField = value;
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x001DC878 File Offset: 0x001DAA78
		protected OBDRequest()
			: this("", false)
		{
		}

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x001DC886 File Offset: 0x001DAA86
		// (set) Token: 0x0600268D RID: 9869 RVA: 0x001DC88E File Offset: 0x001DAA8E
		public int FailCounter
		{
			[CompilerGenerated]
			get
			{
				return this.<FailCounter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FailCounter>k__BackingField = value;
			}
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x001DC897 File Offset: 0x001DAA97
		public OBDRequest(string Command, bool Repeat)
			: this(Command, "", new string[0], new string[0], Repeat, new List<PID>(0))
		{
			this.FillPIDs();
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x001DC8BE File Offset: 0x001DAABE
		public OBDRequest(string Command, string Header, bool Repeat)
			: this(Command, Header, new string[0], new string[0], Repeat, new List<PID>(0))
		{
			this.FillPIDs();
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x001DC8E1 File Offset: 0x001DAAE1
		public OBDRequest(string Command, bool Repeat, List<PID> pids)
			: this(Command, "", new string[0], new string[0], Repeat, pids)
		{
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x001DC900 File Offset: 0x001DAB00
		public OBDRequest(string Command, bool Repeat, PID pid)
		{
			string text = "";
			string[] array = new string[0];
			string[] array2 = new string[0];
			List<PID> list;
			if (pid != null)
			{
				(list = new List<PID>(1)).Add(pid);
			}
			else
			{
				list = new List<PID>(0);
			}
			this..ctor(Command, text, array, array2, Repeat, list);
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x001DC93E File Offset: 0x001DAB3E
		public OBDRequest(string Command, string Header, string BeforeCommand, string AfterCommand, bool Repeat)
			: this(Command, Header, CustomPID.StringToCommands(BeforeCommand), CustomPID.StringToCommands(AfterCommand), Repeat, new List<PID>(0))
		{
			this.FillPIDs();
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x001DC963 File Offset: 0x001DAB63
		public OBDRequest(string Command, string Header, string[] BeforeCommands, string[] AfterCommands, bool Repeat)
			: this(Command, Header, BeforeCommands, AfterCommands, Repeat, new List<PID>(0))
		{
			this.FillPIDs();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x001DC97E File Offset: 0x001DAB7E
		public OBDRequest(string Command, string Header, string BeforeCommand, string AfterCommand, bool Repeat, List<PID> pids)
			: this(Command, Header, CustomPID.StringToCommands(BeforeCommand), CustomPID.StringToCommands(AfterCommand), Repeat, pids)
		{
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x001DC99C File Offset: 0x001DAB9C
		public OBDRequest(string Command, string Header, string BeforeCommand, string AfterCommand, bool Repeat, List<IPID> pids)
			: this(Command, Header, CustomPID.StringToCommands(BeforeCommand), CustomPID.StringToCommands(AfterCommand), Repeat, pids.Select((IPID x) => (PID)x).ToList<PID>())
		{
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x001DC9EC File Offset: 0x001DABEC
		public OBDRequest(string Command, string Header, string BeforeCommand, string AfterCommand, bool Repeat, IPID pid)
			: this(Command, Header, CustomPID.StringToCommands(BeforeCommand), CustomPID.StringToCommands(AfterCommand), Repeat, new List<PID>(1) { (PID)pid })
		{
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x001DCA24 File Offset: 0x001DAC24
		public OBDRequest(string Command, string Header, string[] BeforeCommands, string[] AfterCommands, bool Repeat, List<PID> pids)
		{
			this._Header = "";
			this.Payload = "";
			this.Keys = new Dictionary<string, string>(1);
			this.MaxLines = -1;
			base..ctor();
			if (Command == null)
			{
				Command = "";
			}
			this.Command = Command.Trim();
			this.Repeat = Repeat;
			this.Header = Header.Replace(" ", "");
			this.BeforeCommands = BeforeCommands;
			this.AfterCommands = AfterCommands;
			if (BeforeCommands == null)
			{
				BeforeCommands = new string[0];
			}
			else
			{
				for (int i = 0; i < BeforeCommands.Length; i++)
				{
					if (BeforeCommands[i] == "ATSTDEF")
					{
						BeforeCommands[i] = "ATST" + SharedSettings.Current.GetATST();
					}
					else if (BeforeCommands[i] == "ATSPDEF")
					{
						BeforeCommands[i] = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X");
					}
				}
			}
			if (AfterCommands == null)
			{
				AfterCommands = new string[0];
			}
			else
			{
				for (int j = 0; j < AfterCommands.Length; j++)
				{
					if (AfterCommands[j] == "ATSTDEF")
					{
						AfterCommands[j] = "ATST" + SharedSettings.Current.GetATST();
					}
					else if (AfterCommands[j] == "ATSPDEF")
					{
						AfterCommands[j] = "ATSP" + App.OBDReader.CurrentProtocolNumber.ToString("X");
					}
				}
			}
			this.ResponseMarker = OBDRequest.GetResponseMarkerFromCommand(Command);
			if (pids == null)
			{
				this.FillPIDs();
			}
			else
			{
				this._PIDs = pids;
			}
			if (Header == "000" && Command.StartsWith("VWTP"))
			{
				this.ELMFormat = ELMFormat.VwTp20;
				string[] array = Command.Split(VWTPManager.CmdSplitter, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 3)
				{
					string text = array[2];
					this.ResponseMarker = OBDRequest.GetResponseMarkerFromCommand(text);
				}
			}
			try
			{
				this.OverrideFlowControl();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x001DCC18 File Offset: 0x001DAE18
		private void OverrideFlowControl()
		{
			if (this.Header == "000")
			{
				return;
			}
			switch (SharedSettings.Current.FlowControlOverrideMode)
			{
			case FlowControlOverrides.Off:
				return;
			case FlowControlOverrides.ForceOnFor7Ex:
				if (string.IsNullOrEmpty(this.Header))
				{
					return;
				}
				if (this.Header.Length == 3 && this.Header.StartsWith("7E"))
				{
					bool flag = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSH"));
					bool flag2 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSD"));
					bool flag3 = this.BeforeCommands.Any((string x) => x == "ATFCSM1");
					if (flag2 || flag || flag3)
					{
						List<string> list = new List<string>(this.BeforeCommands);
						if (flag)
						{
							list.Add("ATFCSH" + this.Header);
						}
						if (flag2)
						{
							list.Add("ATFCSD300000");
						}
						if (flag3)
						{
							list.Add("ATFCSM1");
						}
						this.BeforeCommands = list.ToArray();
					}
					if (!this.AfterCommands.Contains("ATFCSM0"))
					{
						this.AfterCommands = this.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
						return;
					}
				}
				else if (this.Header.Length == 6 && this.Header.StartsWith("DA"))
				{
					bool flag4 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSH"));
					bool flag5 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSD"));
					bool flag6 = this.BeforeCommands.Any((string x) => x == "ATFCSM1");
					string text = this.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCP"));
					string text2 = App.OBDReader.ELMStatus.CAN29bitPriority;
					if (text != null && text.Length >= 4)
					{
						text2 = text.Substring(2).Trim();
					}
					if (flag5 || flag4 || flag6)
					{
						List<string> list2 = new List<string>(this.BeforeCommands);
						if (flag4)
						{
							list2.Add("ATFCSH" + text2 + this.Header);
						}
						if (flag5)
						{
							list2.Add("ATFCSD300000");
						}
						if (flag6)
						{
							list2.Add("ATFCSM1");
						}
						this.BeforeCommands = list2.ToArray();
					}
					if (!this.AfterCommands.Contains("ATFCSM0"))
					{
						this.AfterCommands = this.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
						return;
					}
				}
				break;
			case FlowControlOverrides.ForceOnForAll:
				if (string.IsNullOrEmpty(this.Header))
				{
					return;
				}
				if (this.Header.Length == 3)
				{
					bool flag7 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSH"));
					bool flag8 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSD"));
					bool flag9 = this.BeforeCommands.Any((string x) => x == "ATFCSM1");
					if (flag8 || flag7 || flag9)
					{
						List<string> list3 = new List<string>(this.BeforeCommands);
						if (flag7)
						{
							list3.Add("ATFCSH" + this.Header);
						}
						if (flag8)
						{
							list3.Add("ATFCSD300000");
						}
						if (flag9)
						{
							list3.Add("ATFCSM1");
						}
						this.BeforeCommands = list3.ToArray();
					}
					if (!this.AfterCommands.Contains("ATFCSM0"))
					{
						this.AfterCommands = this.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
						return;
					}
				}
				else
				{
					if (this.Header.Length == 6)
					{
						if (this.BeforeCommands.Any((string x) => x.StartsWith("ATCP")))
						{
							bool flag10 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSH"));
							bool flag11 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSD"));
							bool flag12 = this.BeforeCommands.Any((string x) => x == "ATFCSM1");
							if (flag11 || flag10 || flag12)
							{
								List<string> list4 = new List<string>(this.BeforeCommands);
								if (flag10)
								{
									string text3 = this.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCP")).Substring(4);
									list4.Add("ATFCSH" + text3 + this.Header);
								}
								if (flag11)
								{
									list4.Add("ATFCSD300000");
								}
								if (flag12)
								{
									list4.Add("ATFCSM1");
								}
								this.BeforeCommands = list4.ToArray();
							}
							if (!this.AfterCommands.Contains("ATFCSM0"))
							{
								this.AfterCommands = this.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
								return;
							}
							break;
						}
					}
					if (this.Header.Length == 6 && this.Header.StartsWith("DA"))
					{
						bool flag13 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSH"));
						bool flag14 = this.BeforeCommands.Any((string x) => x.StartsWith("ATFCSD"));
						bool flag15 = this.BeforeCommands.Any((string x) => x == "ATFCSM1");
						string text4 = this.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCP"));
						string text5 = App.OBDReader.ELMStatus.CAN29bitPriority;
						if (text4 != null && text4.Length >= 4)
						{
							text5 = text4.Substring(2).Trim();
						}
						if (flag14 || flag13 || flag15)
						{
							List<string> list5 = new List<string>(this.BeforeCommands);
							if (flag13)
							{
								list5.Add("ATFCSH" + text5 + this.Header);
							}
							if (flag14)
							{
								list5.Add("ATFCSD300000");
							}
							if (flag15)
							{
								list5.Add("ATFCSM1");
							}
							this.BeforeCommands = list5.ToArray();
						}
						if (!this.AfterCommands.Contains("ATFCSM0"))
						{
							this.AfterCommands = this.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
							return;
						}
					}
				}
				break;
			case FlowControlOverrides.ForceOffFor7Ex:
				if (string.IsNullOrEmpty(this.Header))
				{
					return;
				}
				if (!this.Header.StartsWith("7E"))
				{
					return;
				}
				if (this.BeforeCommands.Length != 0)
				{
					if (this.BeforeCommands.Any((string x) => x.StartsWith("ATFC")))
					{
						List<string> list6 = new List<string>(this.BeforeCommands);
						list6.RemoveAll((string x) => x.StartsWith("ATFC"));
						this.BeforeCommands = list6.ToArray();
					}
				}
				if (this.AfterCommands.Length != 0 && EnumerableExtensions.IndexOf<string>(this.AfterCommands, "ATFCSM0") >= 0)
				{
					List<string> list7 = new List<string>(this.AfterCommands);
					list7.RemoveAll((string x) => x == "ATFCSM0");
					this.AfterCommands = list7.ToArray();
				}
				break;
			case FlowControlOverrides.ForceOffForAll:
				if (string.IsNullOrEmpty(this.Header))
				{
					return;
				}
				if (this.BeforeCommands.Length != 0)
				{
					if (this.BeforeCommands.Any((string x) => x.StartsWith("ATFC")))
					{
						List<string> list8 = new List<string>(this.BeforeCommands);
						list8.RemoveAll((string x) => x.StartsWith("ATFC"));
						this.BeforeCommands = list8.ToArray();
					}
				}
				if (this.AfterCommands.Length != 0 && EnumerableExtensions.IndexOf<string>(this.AfterCommands, "ATFCSM0") >= 0)
				{
					List<string> list9 = new List<string>(this.AfterCommands);
					list9.RemoveAll((string x) => x == "ATFCSM0");
					this.AfterCommands = list9.ToArray();
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x001DD594 File Offset: 0x001DB794
		protected void FillPIDs()
		{
			List<PID> list = new List<PID>(LiveDataPIDModel._PIDCollection).Where((PID x) => x != null && x.Command == this.Command).ToList<PID>();
			if (list.Count == 0)
			{
				list = App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x != null && x.Command == this.Command).ToList<PID>();
			}
			if (!string.IsNullOrEmpty(this.Header))
			{
				list = list.Where((PID x) => x != null && ((x is CustomPID && (x as CustomPID).Header == this.Header) || !(x is CustomPID))).ToList<PID>();
			}
			this._PIDs = list;
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x001DD61C File Offset: 0x001DB81C
		public override bool Equals(object obj)
		{
			OBDRequest obdrequest = obj as OBDRequest;
			return obj != null && !(base.GetType() != obj.GetType()) && (this.AfterCommands == null || obdrequest.AfterCommands == null || ArrayHelpers.ArrayEquals<string>(this.AfterCommands, obdrequest.AfterCommands)) && (this.BeforeCommands == null || obdrequest.BeforeCommands == null || ArrayHelpers.ArrayEquals<string>(this.BeforeCommands, obdrequest.BeforeCommands)) && (this.Command == null || obdrequest.Command == null || this.Command.Equals(obdrequest.Command)) && (this.Header == null || obdrequest.Header == null || this.Header.Equals(obdrequest.Header)) && this.Repeat == obdrequest.Repeat;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x001DD6F0 File Offset: 0x001DB8F0
		public static string GetResponseMarkerFromCommand(string cmd)
		{
			if (string.IsNullOrEmpty(cmd))
			{
				return "";
			}
			if (cmd == "ATRV")
			{
				return "";
			}
			if (cmd.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
			{
				return "";
			}
			if (cmd.Length == 8)
			{
				if (cmd.StartsWith("18") || cmd.StartsWith("13") || cmd.StartsWith("19"))
				{
					cmd = cmd.Substring(0, 2);
				}
			}
			else if (cmd.Length == 4 && cmd.StartsWith("19"))
			{
				cmd = cmd.Substring(0, 2);
			}
			else if (cmd.Length == 6)
			{
				if (cmd.StartsWith("13") || cmd.StartsWith("17") || cmd == "13FFFF")
				{
					cmd = cmd.Substring(0, 2);
				}
				else if (cmd.StartsWith("19"))
				{
					cmd = cmd.Substring(0, 4);
				}
			}
			else if (cmd.Length == 12 && cmd.StartsWith("1906"))
			{
				cmd = cmd.Substring(0, 10);
			}
			else if (cmd.Length == 4 && cmd.StartsWith("13"))
			{
				cmd = "13";
			}
			else if (cmd.Length > 6 && cmd.StartsWith("2F"))
			{
				cmd = cmd.Substring(0, 6);
			}
			else if (cmd.StartsWith("23"))
			{
				cmd = "23";
			}
			else if (cmd.StartsWith("3D"))
			{
				cmd = cmd.Substring(0, 2);
			}
			else if (cmd.Length >= 6 && cmd.StartsWith("2E"))
			{
				cmd = cmd.Substring(0, 6);
			}
			if (cmd.Length > 6 && cmd.StartsWith("A8"))
			{
				cmd = cmd.Substring(0, 2);
			}
			if (cmd.Length == 8 && cmd.StartsWith("21") && cmd.EndsWith("8001"))
			{
				return "61FF";
			}
			if (cmd.Length == 6 && cmd.StartsWith("AA"))
			{
				return cmd.Substring(4, 2);
			}
			if (cmd.StartsWith("23"))
			{
				return "63";
			}
			string text;
			try
			{
				int num = BitHelpers.ConvertHexToInt(cmd.Substring(0, 2));
				num += 64;
				if (cmd.Length == 2)
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture);
				}
				else if (cmd.Length >= 6 && num == 65 && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 2);
				}
				else if (cmd.Length == 6)
				{
					if (App.OBDReader != null && SharedSettings.Current.DaihatsuKLine && (App.OBDReader.CurrentELMFormat == ELMFormat.KWP || App.OBDReader.CurrentELMFormat == ELMFormat.Unknown))
					{
						text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 2);
					}
					else
					{
						char c = cmd[0];
						text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2);
					}
				}
				else if (cmd.Length == 8 && cmd.StartsWith("2C"))
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 2);
				}
				else if (cmd.StartsWith("2C"))
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 2);
				}
				else if (cmd.Length == 8)
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 4);
				}
				else if (num == 98 && cmd.Length >= 10)
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2, 4);
				}
				else if ((cmd.Length == 12 || cmd.Length == 16) && App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && SharedSettings.Current.SelectedBrand == "Volvo")
				{
					string text2 = cmd.Substring(4, 4);
					byte b = byte.Parse(text2.Substring(0, 2), NumberStyles.HexNumber);
					text = (b + 64).ToString("X2") + text2.Substring(2);
				}
				else if (SharedSettings.Current.DecodeMUT2Compatible && App.OBDReader != null && (App.OBDReader.CurrentELMFormat == ELMFormat.KWP || App.OBDReader.CurrentELMFormat == ELMFormat.Unknown) && cmd.Length == 4 && cmd.StartsWith("A", StringComparison.Ordinal))
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture);
				}
				else
				{
					text = num.ToString("X2", CultureInfo.InvariantCulture) + cmd.Substring(2);
				}
			}
			catch
			{
				text = "";
			}
			return text;
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x0600269C RID: 9884 RVA: 0x001DDC04 File Offset: 0x001DBE04
		// (remove) Token: 0x0600269D RID: 9885 RVA: 0x001DDC3C File Offset: 0x001DBE3C
		public event ResponseReceivedDelegate ResponseReceived
		{
			[CompilerGenerated]
			add
			{
				ResponseReceivedDelegate responseReceivedDelegate = this.ResponseReceived;
				ResponseReceivedDelegate responseReceivedDelegate2;
				do
				{
					responseReceivedDelegate2 = responseReceivedDelegate;
					ResponseReceivedDelegate responseReceivedDelegate3 = (ResponseReceivedDelegate)Delegate.Combine(responseReceivedDelegate2, value);
					responseReceivedDelegate = Interlocked.CompareExchange<ResponseReceivedDelegate>(ref this.ResponseReceived, responseReceivedDelegate3, responseReceivedDelegate2);
				}
				while (responseReceivedDelegate != responseReceivedDelegate2);
			}
			[CompilerGenerated]
			remove
			{
				ResponseReceivedDelegate responseReceivedDelegate = this.ResponseReceived;
				ResponseReceivedDelegate responseReceivedDelegate2;
				do
				{
					responseReceivedDelegate2 = responseReceivedDelegate;
					ResponseReceivedDelegate responseReceivedDelegate3 = (ResponseReceivedDelegate)Delegate.Remove(responseReceivedDelegate2, value);
					responseReceivedDelegate = Interlocked.CompareExchange<ResponseReceivedDelegate>(ref this.ResponseReceived, responseReceivedDelegate3, responseReceivedDelegate2);
				}
				while (responseReceivedDelegate != responseReceivedDelegate2);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600269E RID: 9886 RVA: 0x001DDC74 File Offset: 0x001DBE74
		// (remove) Token: 0x0600269F RID: 9887 RVA: 0x001DDCAC File Offset: 0x001DBEAC
		public event ResponseDecodedDelegate ResponseDecoded
		{
			[CompilerGenerated]
			add
			{
				ResponseDecodedDelegate responseDecodedDelegate = this.ResponseDecoded;
				ResponseDecodedDelegate responseDecodedDelegate2;
				do
				{
					responseDecodedDelegate2 = responseDecodedDelegate;
					ResponseDecodedDelegate responseDecodedDelegate3 = (ResponseDecodedDelegate)Delegate.Combine(responseDecodedDelegate2, value);
					responseDecodedDelegate = Interlocked.CompareExchange<ResponseDecodedDelegate>(ref this.ResponseDecoded, responseDecodedDelegate3, responseDecodedDelegate2);
				}
				while (responseDecodedDelegate != responseDecodedDelegate2);
			}
			[CompilerGenerated]
			remove
			{
				ResponseDecodedDelegate responseDecodedDelegate = this.ResponseDecoded;
				ResponseDecodedDelegate responseDecodedDelegate2;
				do
				{
					responseDecodedDelegate2 = responseDecodedDelegate;
					ResponseDecodedDelegate responseDecodedDelegate3 = (ResponseDecodedDelegate)Delegate.Remove(responseDecodedDelegate2, value);
					responseDecodedDelegate = Interlocked.CompareExchange<ResponseDecodedDelegate>(ref this.ResponseDecoded, responseDecodedDelegate3, responseDecodedDelegate2);
				}
				while (responseDecodedDelegate != responseDecodedDelegate2);
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x001DDCE1 File Offset: 0x001DBEE1
		// (set) Token: 0x060026A1 RID: 9889 RVA: 0x001DDCE9 File Offset: 0x001DBEE9
		public bool UseCustomDecodeDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<UseCustomDecodeDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseCustomDecodeDelegate>k__BackingField = value;
			}
		}

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x001DDCF2 File Offset: 0x001DBEF2
		// (set) Token: 0x060026A3 RID: 9891 RVA: 0x001DDCFA File Offset: 0x001DBEFA
		public bool ForceManualFlowControl
		{
			[CompilerGenerated]
			get
			{
				return this.<ForceManualFlowControl>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<ForceManualFlowControl>k__BackingField = value;
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x060026A4 RID: 9892 RVA: 0x001DDD03 File Offset: 0x001DBF03
		// (set) Token: 0x060026A5 RID: 9893 RVA: 0x001DDD0B File Offset: 0x001DBF0B
		public int MaxLines
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxLines>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MaxLines>k__BackingField = value;
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x001DDD14 File Offset: 0x001DBF14
		public void OnResponseReceived(string data)
		{
			ResponseReceivedDelegate responseReceived = this.ResponseReceived;
			if (responseReceived == null)
			{
				return;
			}
			responseReceived(this, data);
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x001DDD28 File Offset: 0x001DBF28
		public void OnResponseDecoded(byte[] data, bool decodeResult, string responseHeader)
		{
			ResponseDecodedDelegate responseDecoded = this.ResponseDecoded;
			if (responseDecoded == null)
			{
				return;
			}
			responseDecoded(this, data, decodeResult, responseHeader);
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x001DDD3E File Offset: 0x001DBF3E
		[CompilerGenerated]
		private bool <FillPIDs>b__91_0(PID x)
		{
			return x != null && x.Command == this.Command;
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x001DDD3E File Offset: 0x001DBF3E
		[CompilerGenerated]
		private bool <FillPIDs>b__91_1(PID x)
		{
			return x != null && x.Command == this.Command;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x001DC685 File Offset: 0x001DA885
		[CompilerGenerated]
		private bool <FillPIDs>b__91_2(PID x)
		{
			return x != null && ((x is CustomPID && (x as CustomPID).Header == this.Header) || !(x is CustomPID));
		}

		// Token: 0x040014E2 RID: 5346
		public const string KEY_TESTER_PRESENT = "TesterPresent";

		// Token: 0x040014E3 RID: 5347
		public const string KEY_FC_HEADER = "FC_HEADER";

		// Token: 0x040014E4 RID: 5348
		public const string KEY_RECEIVE_HEADER = "RCV_HEADER";

		// Token: 0x040014E5 RID: 5349
		public const string KEY_EXT_ADDR = "EXT_ADDR";

		// Token: 0x040014E6 RID: 5350
		public const string KEY_EXT_TA = "EXT_TA";

		// Token: 0x040014E7 RID: 5351
		public const string KEY_ST_FC_REQHEADER = "ST_FC_REQ";

		// Token: 0x040014E8 RID: 5352
		public const string KEY_ST_FC_RESHEADER = "ST_FC_RES";

		// Token: 0x040014E9 RID: 5353
		[CompilerGenerated]
		private string <Command>k__BackingField;

		// Token: 0x040014EA RID: 5354
		[CompilerGenerated]
		private string <ResponseMarker>k__BackingField;

		// Token: 0x040014EB RID: 5355
		[CompilerGenerated]
		private bool <Repeat>k__BackingField;

		// Token: 0x040014EC RID: 5356
		private string _Header;

		// Token: 0x040014ED RID: 5357
		[CompilerGenerated]
		private string[] <BeforeCommands>k__BackingField;

		// Token: 0x040014EE RID: 5358
		[CompilerGenerated]
		private string[] <AfterCommands>k__BackingField;

		// Token: 0x040014EF RID: 5359
		protected List<PID> _PIDs;

		// Token: 0x040014F0 RID: 5360
		[CompilerGenerated]
		private bool <DoNotDecode>k__BackingField;

		// Token: 0x040014F1 RID: 5361
		[CompilerGenerated]
		private int <NR78RepeatCounter>k__BackingField;

		// Token: 0x040014F2 RID: 5362
		[CompilerGenerated]
		private bool <CheckLength>k__BackingField;

		// Token: 0x040014F3 RID: 5363
		[CompilerGenerated]
		private string <Payload>k__BackingField;

		// Token: 0x040014F4 RID: 5364
		[CompilerGenerated]
		private ELMFormat <ELMFormat>k__BackingField;

		// Token: 0x040014F5 RID: 5365
		[CompilerGenerated]
		private OBDDataReader.OBDModes <OBDMode>k__BackingField;

		// Token: 0x040014F6 RID: 5366
		[CompilerGenerated]
		private Dictionary<string, string> <Keys>k__BackingField;

		// Token: 0x040014F7 RID: 5367
		[CompilerGenerated]
		private int <SkipCyclesTarget>k__BackingField;

		// Token: 0x040014F8 RID: 5368
		[CompilerGenerated]
		private int <SkippedCycles>k__BackingField;

		// Token: 0x040014F9 RID: 5369
		[CompilerGenerated]
		private IProgress<string> <Progress>k__BackingField;

		// Token: 0x040014FA RID: 5370
		[CompilerGenerated]
		private int <FailCounter>k__BackingField;

		// Token: 0x040014FB RID: 5371
		[CompilerGenerated]
		private ResponseReceivedDelegate ResponseReceived;

		// Token: 0x040014FC RID: 5372
		[CompilerGenerated]
		private ResponseDecodedDelegate ResponseDecoded;

		// Token: 0x040014FD RID: 5373
		[CompilerGenerated]
		private bool <UseCustomDecodeDelegate>k__BackingField;

		// Token: 0x040014FE RID: 5374
		[CompilerGenerated]
		private bool <ForceManualFlowControl>k__BackingField;

		// Token: 0x040014FF RID: 5375
		[CompilerGenerated]
		private int <MaxLines>k__BackingField;

		// Token: 0x02000390 RID: 912
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060026AB RID: 9899 RVA: 0x001DDD56 File Offset: 0x001DBF56
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060026AC RID: 9900 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060026AD RID: 9901 RVA: 0x001DDD62 File Offset: 0x001DBF62
			internal PID <.ctor>b__87_0(IPID x)
			{
				return (PID)x;
			}

			// Token: 0x060026AE RID: 9902 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <OverrideFlowControl>b__90_3(string x)
			{
				return x.StartsWith("ATFCSH");
			}

			// Token: 0x060026AF RID: 9903 RVA: 0x001DDD77 File Offset: 0x001DBF77
			internal bool <OverrideFlowControl>b__90_4(string x)
			{
				return x.StartsWith("ATFCSD");
			}

			// Token: 0x060026B0 RID: 9904 RVA: 0x001DDD84 File Offset: 0x001DBF84
			internal bool <OverrideFlowControl>b__90_5(string x)
			{
				return x == "ATFCSM1";
			}

			// Token: 0x060026B1 RID: 9905 RVA: 0x001DDD91 File Offset: 0x001DBF91
			internal bool <OverrideFlowControl>b__90_0(string x)
			{
				return x.StartsWith("ATCP");
			}

			// Token: 0x060026B2 RID: 9906 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <OverrideFlowControl>b__90_6(string x)
			{
				return x.StartsWith("ATFCSH");
			}

			// Token: 0x060026B3 RID: 9907 RVA: 0x001DDD77 File Offset: 0x001DBF77
			internal bool <OverrideFlowControl>b__90_7(string x)
			{
				return x.StartsWith("ATFCSD");
			}

			// Token: 0x060026B4 RID: 9908 RVA: 0x001DDD84 File Offset: 0x001DBF84
			internal bool <OverrideFlowControl>b__90_8(string x)
			{
				return x == "ATFCSM1";
			}

			// Token: 0x060026B5 RID: 9909 RVA: 0x001DDD91 File Offset: 0x001DBF91
			internal bool <OverrideFlowControl>b__90_9(string x)
			{
				return x.StartsWith("ATCP");
			}

			// Token: 0x060026B6 RID: 9910 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <OverrideFlowControl>b__90_10(string x)
			{
				return x.StartsWith("ATFCSH");
			}

			// Token: 0x060026B7 RID: 9911 RVA: 0x001DDD77 File Offset: 0x001DBF77
			internal bool <OverrideFlowControl>b__90_11(string x)
			{
				return x.StartsWith("ATFCSD");
			}

			// Token: 0x060026B8 RID: 9912 RVA: 0x001DDD84 File Offset: 0x001DBF84
			internal bool <OverrideFlowControl>b__90_12(string x)
			{
				return x == "ATFCSM1";
			}

			// Token: 0x060026B9 RID: 9913 RVA: 0x001DDD91 File Offset: 0x001DBF91
			internal bool <OverrideFlowControl>b__90_13(string x)
			{
				return x.StartsWith("ATCP");
			}

			// Token: 0x060026BA RID: 9914 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <OverrideFlowControl>b__90_14(string x)
			{
				return x.StartsWith("ATFCSH");
			}

			// Token: 0x060026BB RID: 9915 RVA: 0x001DDD77 File Offset: 0x001DBF77
			internal bool <OverrideFlowControl>b__90_15(string x)
			{
				return x.StartsWith("ATFCSD");
			}

			// Token: 0x060026BC RID: 9916 RVA: 0x001DDD84 File Offset: 0x001DBF84
			internal bool <OverrideFlowControl>b__90_16(string x)
			{
				return x == "ATFCSM1";
			}

			// Token: 0x060026BD RID: 9917 RVA: 0x001DDD6A File Offset: 0x001DBF6A
			internal bool <OverrideFlowControl>b__90_17(string x)
			{
				return x.StartsWith("ATFCSH");
			}

			// Token: 0x060026BE RID: 9918 RVA: 0x001DDD77 File Offset: 0x001DBF77
			internal bool <OverrideFlowControl>b__90_18(string x)
			{
				return x.StartsWith("ATFCSD");
			}

			// Token: 0x060026BF RID: 9919 RVA: 0x001DDD84 File Offset: 0x001DBF84
			internal bool <OverrideFlowControl>b__90_19(string x)
			{
				return x == "ATFCSM1";
			}

			// Token: 0x060026C0 RID: 9920 RVA: 0x001DDD91 File Offset: 0x001DBF91
			internal bool <OverrideFlowControl>b__90_20(string x)
			{
				return x.StartsWith("ATCP");
			}

			// Token: 0x060026C1 RID: 9921 RVA: 0x001DC702 File Offset: 0x001DA902
			internal bool <OverrideFlowControl>b__90_1(string x)
			{
				return x.StartsWith("ATFC");
			}

			// Token: 0x060026C2 RID: 9922 RVA: 0x001DC702 File Offset: 0x001DA902
			internal bool <OverrideFlowControl>b__90_21(string x)
			{
				return x.StartsWith("ATFC");
			}

			// Token: 0x060026C3 RID: 9923 RVA: 0x001B17F2 File Offset: 0x001AF9F2
			internal bool <OverrideFlowControl>b__90_22(string x)
			{
				return x == "ATFCSM0";
			}

			// Token: 0x060026C4 RID: 9924 RVA: 0x001DC702 File Offset: 0x001DA902
			internal bool <OverrideFlowControl>b__90_2(string x)
			{
				return x.StartsWith("ATFC");
			}

			// Token: 0x060026C5 RID: 9925 RVA: 0x001DC702 File Offset: 0x001DA902
			internal bool <OverrideFlowControl>b__90_23(string x)
			{
				return x.StartsWith("ATFC");
			}

			// Token: 0x060026C6 RID: 9926 RVA: 0x001B17F2 File Offset: 0x001AF9F2
			internal bool <OverrideFlowControl>b__90_24(string x)
			{
				return x == "ATFCSM0";
			}

			// Token: 0x04001500 RID: 5376
			public static readonly OBDRequest.<>c <>9 = new OBDRequest.<>c();

			// Token: 0x04001501 RID: 5377
			public static Func<IPID, PID> <>9__87_0;

			// Token: 0x04001502 RID: 5378
			public static Func<string, bool> <>9__90_3;

			// Token: 0x04001503 RID: 5379
			public static Func<string, bool> <>9__90_4;

			// Token: 0x04001504 RID: 5380
			public static Func<string, bool> <>9__90_5;

			// Token: 0x04001505 RID: 5381
			public static Func<string, bool> <>9__90_0;

			// Token: 0x04001506 RID: 5382
			public static Func<string, bool> <>9__90_6;

			// Token: 0x04001507 RID: 5383
			public static Func<string, bool> <>9__90_7;

			// Token: 0x04001508 RID: 5384
			public static Func<string, bool> <>9__90_8;

			// Token: 0x04001509 RID: 5385
			public static Func<string, bool> <>9__90_9;

			// Token: 0x0400150A RID: 5386
			public static Func<string, bool> <>9__90_10;

			// Token: 0x0400150B RID: 5387
			public static Func<string, bool> <>9__90_11;

			// Token: 0x0400150C RID: 5388
			public static Func<string, bool> <>9__90_12;

			// Token: 0x0400150D RID: 5389
			public static Func<string, bool> <>9__90_13;

			// Token: 0x0400150E RID: 5390
			public static Func<string, bool> <>9__90_14;

			// Token: 0x0400150F RID: 5391
			public static Func<string, bool> <>9__90_15;

			// Token: 0x04001510 RID: 5392
			public static Func<string, bool> <>9__90_16;

			// Token: 0x04001511 RID: 5393
			public static Func<string, bool> <>9__90_17;

			// Token: 0x04001512 RID: 5394
			public static Func<string, bool> <>9__90_18;

			// Token: 0x04001513 RID: 5395
			public static Func<string, bool> <>9__90_19;

			// Token: 0x04001514 RID: 5396
			public static Func<string, bool> <>9__90_20;

			// Token: 0x04001515 RID: 5397
			public static Func<string, bool> <>9__90_1;

			// Token: 0x04001516 RID: 5398
			public static Predicate<string> <>9__90_21;

			// Token: 0x04001517 RID: 5399
			public static Predicate<string> <>9__90_22;

			// Token: 0x04001518 RID: 5400
			public static Func<string, bool> <>9__90_2;

			// Token: 0x04001519 RID: 5401
			public static Predicate<string> <>9__90_23;

			// Token: 0x0400151A RID: 5402
			public static Predicate<string> <>9__90_24;
		}
	}
}
