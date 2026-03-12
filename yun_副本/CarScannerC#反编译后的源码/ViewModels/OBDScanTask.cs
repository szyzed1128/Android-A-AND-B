using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000723 RID: 1827
	public class OBDScanTask
	{
		// Token: 0x17001448 RID: 5192
		// (get) Token: 0x06003E0A RID: 15882 RVA: 0x0032B332 File Offset: 0x00329532
		// (set) Token: 0x06003E0B RID: 15883 RVA: 0x0032B33A File Offset: 0x0032953A
		public string HeadersStart
		{
			[CompilerGenerated]
			get
			{
				return this.<HeadersStart>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HeadersStart>k__BackingField = value;
			}
		}

		// Token: 0x17001449 RID: 5193
		// (get) Token: 0x06003E0C RID: 15884 RVA: 0x0032B343 File Offset: 0x00329543
		// (set) Token: 0x06003E0D RID: 15885 RVA: 0x0032B34B File Offset: 0x0032954B
		public string HeadersEnd
		{
			[CompilerGenerated]
			get
			{
				return this.<HeadersEnd>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<HeadersEnd>k__BackingField = value;
			}
		}

		// Token: 0x1700144A RID: 5194
		// (get) Token: 0x06003E0E RID: 15886 RVA: 0x0032B354 File Offset: 0x00329554
		// (set) Token: 0x06003E0F RID: 15887 RVA: 0x0032B35C File Offset: 0x0032955C
		public string PIDTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDTemplate>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PIDTemplate>k__BackingField = value;
			}
		}

		// Token: 0x1700144B RID: 5195
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x0032B36E File Offset: 0x0032956E
		// (set) Token: 0x06003E10 RID: 15888 RVA: 0x0032B365 File Offset: 0x00329565
		public string PIDStart
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDStart>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PIDStart>k__BackingField = value;
			}
		}

		// Token: 0x1700144C RID: 5196
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x0032B37F File Offset: 0x0032957F
		// (set) Token: 0x06003E12 RID: 15890 RVA: 0x0032B376 File Offset: 0x00329576
		public string PIDEnd
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDEnd>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PIDEnd>k__BackingField = value;
			}
		}

		// Token: 0x1700144D RID: 5197
		// (get) Token: 0x06003E14 RID: 15892 RVA: 0x0032B387 File Offset: 0x00329587
		// (set) Token: 0x06003E15 RID: 15893 RVA: 0x0032B38F File Offset: 0x0032958F
		public string StartDiagnosticsCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<StartDiagnosticsCommands>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<StartDiagnosticsCommands>k__BackingField = value;
			}
		}

		// Token: 0x1700144E RID: 5198
		// (get) Token: 0x06003E16 RID: 15894 RVA: 0x0032B398 File Offset: 0x00329598
		// (set) Token: 0x06003E17 RID: 15895 RVA: 0x0032B3A0 File Offset: 0x003295A0
		public string StopDiagnosticsCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<StopDiagnosticsCommands>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<StopDiagnosticsCommands>k__BackingField = value;
			}
		}

		// Token: 0x1700144F RID: 5199
		// (get) Token: 0x06003E18 RID: 15896 RVA: 0x0032B3A9 File Offset: 0x003295A9
		// (set) Token: 0x06003E19 RID: 15897 RVA: 0x0032B3B1 File Offset: 0x003295B1
		public bool SendStartCommandsOnEachPID
		{
			[CompilerGenerated]
			get
			{
				return this.<SendStartCommandsOnEachPID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SendStartCommandsOnEachPID>k__BackingField = value;
			}
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x0032B3BC File Offset: 0x003295BC
		public OBDScanTask(string pidTemplate, string pidStart, string pidEnd, string headersStart, string headersEnd, string startDiagnosticsCommands, string stopDiagnosticsCommands, bool sendStartCommandsOnEachPID)
		{
			this.PIDTemplate = pidTemplate;
			this.PIDStart = pidStart;
			this.PIDEnd = pidEnd;
			this.HeadersStart = headersStart;
			this.HeadersEnd = headersEnd;
			this.StartDiagnosticsCommands = startDiagnosticsCommands;
			this.StopDiagnosticsCommands = stopDiagnosticsCommands;
			this.SendStartCommandsOnEachPID = sendStartCommandsOnEachPID;
		}

		// Token: 0x06003E1B RID: 15899 RVA: 0x0032B40C File Offset: 0x0032960C
		public List<OBDRequest> GetRequests()
		{
			List<OBDRequest> list2;
			try
			{
				List<OBDRequest> list = new List<OBDRequest>();
				int length = this.HeadersStart.Length;
				int length2 = this.PIDStart.Length;
				int num = int.Parse(this.PIDStart, NumberStyles.HexNumber);
				int num2 = int.Parse(this.PIDEnd, NumberStyles.HexNumber);
				if (num > num2)
				{
					int num3 = num;
					num = num2;
					num2 = num3;
				}
				if (this.HeadersStart == "" && this.HeadersEnd == "")
				{
					this.GetRequestsForHeader(length2, num, num2, list, "");
				}
				else
				{
					int num4 = int.Parse(this.HeadersStart, NumberStyles.HexNumber);
					int num5 = int.Parse(this.HeadersEnd, NumberStyles.HexNumber);
					if (num4 > num5)
					{
						int num6 = num4;
						num4 = num5;
						num5 = num6;
					}
					for (int i = num4; i <= num5; i++)
					{
						string text = i.ToString("X" + length.ToString());
						this.GetRequestsForHeader(length2, num, num2, list, text);
					}
				}
				list2 = list;
			}
			catch
			{
				list2 = new List<OBDRequest>();
			}
			return list2;
		}

		// Token: 0x06003E1C RID: 15900 RVA: 0x0032B528 File Offset: 0x00329728
		private void GetRequestsForHeader(int pid_length, int i_pidStart, int i_pidEnd, List<OBDRequest> result, string header)
		{
			for (int i = i_pidStart; i <= i_pidEnd; i++)
			{
				string text = i.ToString("X" + pid_length.ToString());
				string text2 = string.Format(this.PIDTemplate, text);
				if (this.SendStartCommandsOnEachPID)
				{
					OBDRequest obdrequest = new OBDRequest(text2, header, this.StartDiagnosticsCommands, this.StopDiagnosticsCommands, false, new List<PID>(0));
					result.Add(obdrequest);
				}
				else if (i == i_pidStart)
				{
					OBDRequest obdrequest2 = new OBDRequest(text2, header, this.StartDiagnosticsCommands, "", false, new List<PID>(0));
					result.Add(obdrequest2);
				}
				else if (i == i_pidEnd)
				{
					OBDRequest obdrequest3 = new OBDRequest(text2, header, "", this.StopDiagnosticsCommands, false, new List<PID>(0));
					result.Add(obdrequest3);
				}
				else
				{
					OBDRequest obdrequest4 = new OBDRequest(text2, header, "", "", false, new List<PID>(0));
					result.Add(obdrequest4);
				}
			}
		}

		// Token: 0x0400260C RID: 9740
		[CompilerGenerated]
		private string <HeadersStart>k__BackingField;

		// Token: 0x0400260D RID: 9741
		[CompilerGenerated]
		private string <HeadersEnd>k__BackingField;

		// Token: 0x0400260E RID: 9742
		[CompilerGenerated]
		private string <PIDTemplate>k__BackingField;

		// Token: 0x0400260F RID: 9743
		[CompilerGenerated]
		private string <PIDStart>k__BackingField;

		// Token: 0x04002610 RID: 9744
		[CompilerGenerated]
		private string <PIDEnd>k__BackingField;

		// Token: 0x04002611 RID: 9745
		[CompilerGenerated]
		private string <StartDiagnosticsCommands>k__BackingField;

		// Token: 0x04002612 RID: 9746
		[CompilerGenerated]
		private string <StopDiagnosticsCommands>k__BackingField;

		// Token: 0x04002613 RID: 9747
		[CompilerGenerated]
		private bool <SendStartCommandsOnEachPID>k__BackingField;
	}
}
