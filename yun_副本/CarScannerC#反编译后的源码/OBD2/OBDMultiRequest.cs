using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x0200038A RID: 906
	public class OBDMultiRequest : OBDRequest
	{
		// Token: 0x06002646 RID: 9798 RVA: 0x001DBB84 File Offset: 0x001D9D84
		public static IEnumerable<OBDRequest> Disassemble(OBDMultiRequest mreq)
		{
			List<OBDRequest> list = new List<OBDRequest>(mreq.Command.Length);
			foreach (string text in mreq.Commands.Keys)
			{
				OBDRequest obdrequest = new OBDRequest(text, mreq.Header, mreq.BeforeCommands, mreq.AfterCommands, mreq.Repeat);
				obdrequest.SkipCyclesTarget = mreq.SkipCyclesTarget;
				if (mreq.Keys != null && mreq.Keys.Count > 0)
				{
					foreach (KeyValuePair<string, string> keyValuePair in mreq.Keys)
					{
						obdrequest.Keys[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				list.Add(obdrequest);
			}
			return list;
		}

		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x001DBC88 File Offset: 0x001D9E88
		// (set) Token: 0x06002648 RID: 9800 RVA: 0x001DBC90 File Offset: 0x001D9E90
		public Dictionary<string, short> Commands
		{
			[CompilerGenerated]
			get
			{
				return this.<Commands>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Commands>k__BackingField = value;
			}
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x001DBC9C File Offset: 0x001D9E9C
		public OBDMultiRequest(List<OBDRequest> requests, bool repeat, int expectedLength)
		{
			base.BeforeCommands = new string[0];
			base.AfterCommands = new string[0];
			base.Repeat = repeat;
			base.Header = requests[0].Header;
			this.Commands = new Dictionary<string, short>(requests.Count);
			this.PIDs = new Dictionary<string, List<PID>>(requests.Count);
			this.ExpectedDataLength = expectedLength;
			StringBuilder stringBuilder = new StringBuilder(requests.Count);
			stringBuilder.Append(requests[0].Command);
			if (requests.Count > 1)
			{
				for (int i = 1; i < requests.Count; i++)
				{
					stringBuilder.Append(requests[i].Command.Substring(2));
				}
			}
			foreach (OBDRequest obdrequest in requests)
			{
				this.Commands.TryAdd(obdrequest.Command, OBDRequestQueueOptimizer.GetDataLength(obdrequest.Command, obdrequest.Header));
				if (obdrequest.BeforeCommands.Length > base.BeforeCommands.Length)
				{
					base.BeforeCommands = obdrequest.BeforeCommands;
				}
				if (obdrequest.AfterCommands.Length > base.AfterCommands.Length)
				{
					base.AfterCommands = obdrequest.AfterCommands;
				}
				this.PIDs.TryAdd(obdrequest.Command, obdrequest.PIDs.ToList<PID>());
				if (obdrequest.Keys != null && obdrequest.Keys.Count > 0)
				{
					foreach (KeyValuePair<string, string> keyValuePair in obdrequest.Keys)
					{
						base.Keys[keyValuePair.Key] = keyValuePair.Value;
					}
				}
			}
			base.Command = stringBuilder.ToString();
			base.Repeat = repeat;
			if (base.Command.StartsWith("01"))
			{
				base.ResponseMarker = OBDRequest.GetResponseMarkerFromCommand(base.Command.Substring(0, 4));
			}
			else if (base.Command.StartsWith("22"))
			{
				base.ResponseMarker = OBDRequest.GetResponseMarkerFromCommand(base.Command.Substring(0, 6));
			}
			else
			{
				int length = this.Commands.First<KeyValuePair<string, short>>().Key.Length;
				base.ResponseMarker = OBDRequest.GetResponseMarkerFromCommand(base.Command.Substring(0, length));
			}
			bool flag = base.BeforeCommands.Any((string x) => x.Contains("ATCEA") && x != "ATCEA");
			if (base.Command.Length > 14)
			{
				string text = base.Header;
				if (string.IsNullOrEmpty(text))
				{
					text = App.OBDReader.GetDefaultHeader();
				}
				if (!base.BeforeCommands.Contains("ATCAF0"))
				{
					base.BeforeCommands = base.BeforeCommands.Concat(new string[] { "ATAL", "ATCAF0" }).ToArray<string>();
				}
				if (!base.BeforeCommands.Contains("ATFCSM1"))
				{
					if (this.ELMFormat == ELMFormat.Unknown && text.Length != 3)
					{
						int length2 = text.Length;
					}
					if (this.ELMFormat == ELMFormat.CAN29bit || (this.ELMFormat == ELMFormat.Unknown && text != null && text.Length == 6))
					{
						string text2 = base.BeforeCommands.LastOrDefault((string x) => x != null && x.StartsWith("ATCP", StringComparison.OrdinalIgnoreCase));
						if (text2 == null)
						{
							text = "18" + text;
						}
						else
						{
							text2 = text2.Replace(" ", "");
							text = text2.Substring(4) + text;
						}
					}
					base.BeforeCommands = base.BeforeCommands.Concat(new string[]
					{
						"ATFCSH" + text,
						"ATFCSD300000",
						"ATFCSM1"
					}).ToArray<string>();
				}
				if (!base.AfterCommands.Contains("ATCAF1"))
				{
					base.AfterCommands = base.AfterCommands.Concat(new string[] { "ATCAF1", "ATCFC1" }).ToArray<string>();
				}
				if (!base.AfterCommands.Contains("ATFCSM0"))
				{
					base.AfterCommands = base.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
				}
				if (!flag && App.OBDReader.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel)
				{
					List<string> list = new List<string>(base.BeforeCommands);
					List<string> list2 = new List<string>(base.AfterCommands);
					list.RemoveAll((string x) => x == "ATCAF0" || x == "ATAL");
					list2.RemoveAll((string x) => x == "ATCAF1");
					if (string.IsNullOrEmpty(base.Header))
					{
						list.RemoveAll((string x) => x.StartsWith("ATFC"));
						list2.RemoveAll((string x) => x == "ATFCSM0");
						list2.RemoveAll((string x) => x == "ATCFC1");
						list2.RemoveAll((string x) => x == "ATCAF1");
					}
					base.BeforeCommands = list.ToArray();
					base.AfterCommands = list2.ToArray();
				}
			}
			else if (base.Command.StartsWith("22") && !string.IsNullOrEmpty(base.Header))
			{
				if (!base.BeforeCommands.Contains("ATFCSM1"))
				{
					base.BeforeCommands = base.BeforeCommands.Concat(new string[]
					{
						"ATFCSH" + base.Header,
						"ATFCSD300000",
						"ATFCSM1"
					}).ToArray<string>();
				}
				if (!base.AfterCommands.Contains("ATFCSM0"))
				{
					base.AfterCommands = base.AfterCommands.Concat(new string[] { "ATFCSM0" }).ToArray<string>();
				}
			}
			this.CanFrames = OBDMultiRequest.PrepareCanFrames(base.Command, flag);
		}

		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x001DC358 File Offset: 0x001DA558
		// (set) Token: 0x0600264B RID: 9803 RVA: 0x001DC360 File Offset: 0x001DA560
		[JsonIgnore]
		public string[] CanFrames
		{
			[CompilerGenerated]
			get
			{
				return this.<CanFrames>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CanFrames>k__BackingField = value;
			}
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x001DC36C File Offset: 0x001DA56C
		public static string[] PrepareCanFrames(string Command, bool hasExtendedAddress)
		{
			int num = 14;
			int num2 = 12;
			int num3 = 14;
			if (hasExtendedAddress)
			{
				num = 12;
				num2 = 10;
				num3 = 12;
			}
			string[] array;
			try
			{
				if (Command.Length <= num)
				{
					array = new string[] { Command };
				}
				else
				{
					string text = "1" + (Command.Length / 2).ToString("X3");
					List<string> list = new List<string>();
					list.Add(text + Command.Substring(0, num2));
					int num4 = 1;
					for (int i = num2; i < Command.Length; i += num3)
					{
						int num5 = i;
						int num6 = num3;
						if (Command.Length < num5 + num6)
						{
							num6 = Command.Length - num5;
						}
						string text2 = Command.Substring(num5, num6);
						text2 = "2" + num4.ToString("X1") + text2;
						list.Add(text2);
						num4++;
						if (num4 > 15)
						{
							num4 = 0;
						}
					}
					array = list.ToArray();
				}
			}
			catch (Exception)
			{
				array = new string[] { Command };
			}
			return array;
		}

		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x0600264D RID: 9805 RVA: 0x001DC488 File Offset: 0x001DA688
		// (set) Token: 0x0600264E RID: 9806 RVA: 0x001DC490 File Offset: 0x001DA690
		public new Dictionary<string, List<PID>> PIDs
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDs>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PIDs>k__BackingField = value;
			}
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x001DC49C File Offset: 0x001DA69C
		public override bool Equals(object obj)
		{
			OBDMultiRequest obdmultiRequest = obj as OBDMultiRequest;
			return obj != null && !(base.GetType() != obj.GetType()) && (base.AfterCommands == null || obdmultiRequest.AfterCommands == null || base.AfterCommands.SequenceEqual(obdmultiRequest.AfterCommands)) && (base.BeforeCommands == null || obdmultiRequest.BeforeCommands == null || base.BeforeCommands.SequenceEqual(obdmultiRequest.BeforeCommands)) && (base.Command == null || obdmultiRequest.Command == null || base.Command.Equals(obdmultiRequest.Command)) && (base.Header == null || obdmultiRequest.Header == null || base.Header.Equals(obdmultiRequest.Header)) && base.Repeat == obdmultiRequest.Repeat && this.Commands.SequenceEqual(obdmultiRequest.Commands);
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x001DC584 File Offset: 0x001DA784
		protected new void FillPIDs()
		{
			this._PIDs = new List<PID>();
			this.PIDs = new Dictionary<string, List<PID>>();
			using (Dictionary<string, short>.Enumerator enumerator = this.Commands.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, short> cmd = enumerator.Current;
					IEnumerable<PID> enumerable = LiveDataPIDModel._PIDCollection.Where((PID x) => x != null && x.Command == cmd.Key);
					if (enumerable.Count<PID>() == 0)
					{
						enumerable = App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x != null && x.Command == cmd.Key);
					}
					if (!string.IsNullOrEmpty(base.Header))
					{
						enumerable = enumerable.Where((PID x) => x != null && ((x is CustomPID && (x as CustomPID).Header == base.Header) || !(x is CustomPID)));
					}
					this.PIDs.TryAdd(cmd.Key, enumerable.ToList<PID>());
				}
			}
		}

		// Token: 0x17001186 RID: 4486
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x001DC674 File Offset: 0x001DA874
		// (set) Token: 0x06002652 RID: 9810 RVA: 0x001DC67C File Offset: 0x001DA87C
		public int ExpectedDataLength
		{
			[CompilerGenerated]
			get
			{
				return this.<ExpectedDataLength>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<ExpectedDataLength>k__BackingField = value;
			}
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x001DC685 File Offset: 0x001DA885
		[CompilerGenerated]
		private bool <FillPIDs>b__16_2(PID x)
		{
			return x != null && ((x is CustomPID && (x as CustomPID).Header == base.Header) || !(x is CustomPID));
		}

		// Token: 0x040014D4 RID: 5332
		[CompilerGenerated]
		private Dictionary<string, short> <Commands>k__BackingField;

		// Token: 0x040014D5 RID: 5333
		[CompilerGenerated]
		private string[] <CanFrames>k__BackingField;

		// Token: 0x040014D6 RID: 5334
		[CompilerGenerated]
		private Dictionary<string, List<PID>> <PIDs>k__BackingField;

		// Token: 0x040014D7 RID: 5335
		[CompilerGenerated]
		private int <ExpectedDataLength>k__BackingField;

		// Token: 0x0200038B RID: 907
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002654 RID: 9812 RVA: 0x001DC6BA File Offset: 0x001DA8BA
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002655 RID: 9813 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002656 RID: 9814 RVA: 0x001C6D23 File Offset: 0x001C4F23
			internal bool <.ctor>b__5_0(string x)
			{
				return x.Contains("ATCEA") && x != "ATCEA";
			}

			// Token: 0x06002657 RID: 9815 RVA: 0x001DC6C6 File Offset: 0x001DA8C6
			internal bool <.ctor>b__5_1(string x)
			{
				return x != null && x.StartsWith("ATCP", StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06002658 RID: 9816 RVA: 0x001DC6D9 File Offset: 0x001DA8D9
			internal bool <.ctor>b__5_2(string x)
			{
				return x == "ATCAF0" || x == "ATAL";
			}

			// Token: 0x06002659 RID: 9817 RVA: 0x001DC6F5 File Offset: 0x001DA8F5
			internal bool <.ctor>b__5_3(string x)
			{
				return x == "ATCAF1";
			}

			// Token: 0x0600265A RID: 9818 RVA: 0x001DC702 File Offset: 0x001DA902
			internal bool <.ctor>b__5_4(string x)
			{
				return x.StartsWith("ATFC");
			}

			// Token: 0x0600265B RID: 9819 RVA: 0x001B17F2 File Offset: 0x001AF9F2
			internal bool <.ctor>b__5_5(string x)
			{
				return x == "ATFCSM0";
			}

			// Token: 0x0600265C RID: 9820 RVA: 0x001DC70F File Offset: 0x001DA90F
			internal bool <.ctor>b__5_6(string x)
			{
				return x == "ATCFC1";
			}

			// Token: 0x0600265D RID: 9821 RVA: 0x001DC6F5 File Offset: 0x001DA8F5
			internal bool <.ctor>b__5_7(string x)
			{
				return x == "ATCAF1";
			}

			// Token: 0x040014D8 RID: 5336
			public static readonly OBDMultiRequest.<>c <>9 = new OBDMultiRequest.<>c();

			// Token: 0x040014D9 RID: 5337
			public static Func<string, bool> <>9__5_0;

			// Token: 0x040014DA RID: 5338
			public static Func<string, bool> <>9__5_1;

			// Token: 0x040014DB RID: 5339
			public static Predicate<string> <>9__5_2;

			// Token: 0x040014DC RID: 5340
			public static Predicate<string> <>9__5_3;

			// Token: 0x040014DD RID: 5341
			public static Predicate<string> <>9__5_4;

			// Token: 0x040014DE RID: 5342
			public static Predicate<string> <>9__5_5;

			// Token: 0x040014DF RID: 5343
			public static Predicate<string> <>9__5_6;

			// Token: 0x040014E0 RID: 5344
			public static Predicate<string> <>9__5_7;
		}

		// Token: 0x0200038C RID: 908
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x0600265E RID: 9822 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x0600265F RID: 9823 RVA: 0x001DC71C File Offset: 0x001DA91C
			internal bool <FillPIDs>b__0(PID x)
			{
				return x != null && x.Command == this.cmd.Key;
			}

			// Token: 0x06002660 RID: 9824 RVA: 0x001DC71C File Offset: 0x001DA91C
			internal bool <FillPIDs>b__1(PID x)
			{
				return x != null && x.Command == this.cmd.Key;
			}

			// Token: 0x040014E1 RID: 5345
			public KeyValuePair<string, short> cmd;
		}
	}
}
