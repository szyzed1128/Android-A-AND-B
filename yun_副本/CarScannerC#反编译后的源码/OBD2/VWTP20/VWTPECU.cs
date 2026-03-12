using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;
using ECUSIM2000v2.Primitives;

namespace CarScannerXamarinForms.OBD2.VWTP20
{
	// Token: 0x020003B4 RID: 948
	internal class VWTPECU
	{
		// Token: 0x0600275E RID: 10078 RVA: 0x001E29DC File Offset: 0x001E0BDC
		public VWTPECU(string unit, IVWTPManager manager)
		{
			this.Unit = unit;
			this.Manager = manager;
		}

		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x001E2A44 File Offset: 0x001E0C44
		// (set) Token: 0x06002760 RID: 10080 RVA: 0x001E2A4C File Offset: 0x001E0C4C
		public string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x06002761 RID: 10081 RVA: 0x001E2A55 File Offset: 0x001E0C55
		// (set) Token: 0x06002762 RID: 10082 RVA: 0x001E2A5D File Offset: 0x001E0C5D
		public string ResponseHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x06002763 RID: 10083 RVA: 0x001E2A66 File Offset: 0x001E0C66
		// (set) Token: 0x06002764 RID: 10084 RVA: 0x001E2A6E File Offset: 0x001E0C6E
		public int BlockSize
		{
			get
			{
				return this._BlockSize;
			}
			set
			{
				if (this._BlockSize == value)
				{
					return;
				}
				this._BlockSize = value;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x001E2A81 File Offset: 0x001E0C81
		private Stopwatch sw
		{
			get
			{
				return App.OBDReader.stopwatch;
			}
		}

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x001E2A90 File Offset: 0x001E0C90
		// (set) Token: 0x06002767 RID: 10087 RVA: 0x001E2B30 File Offset: 0x001E0D30
		public VWTPECU.ECUState CurrentState
		{
			get
			{
				switch (this._CurrentState)
				{
				case VWTPECU.ECUState.WaitingForIncomingConnection:
					if (this.sw.ElapsedMilliseconds - this.WaitingForConnectionTimestampMs < (long)(this.TimeoutForReceiveT1 * 2))
					{
						return VWTPECU.ECUState.WaitingForIncomingConnection;
					}
					return VWTPECU.ECUState.Disconnected;
				case VWTPECU.ECUState.ConnectedWaitingForData:
				case VWTPECU.ECUState.ConnectedWaitingForAck:
					if (this.sw.ElapsedMilliseconds - this.LastTimeActiveConnectionTestTimeout <= (long)SharedSettings.Current.VWTP_ActiveConnectionTestTimeout)
					{
						return this._CurrentState;
					}
					if (this.sw.ElapsedMilliseconds - this.LastTimeActiveConnectionTestTimeout <= (long)(SharedSettings.Current.VWTP_ActiveConnectionTestTimeout + this.PassiveConnectionTestTimeout))
					{
						return VWTPECU.ECUState.ConnectedWaitingForConnectionTest;
					}
					return VWTPECU.ECUState.Disconnected;
				}
				return VWTPECU.ECUState.Disconnected;
			}
			set
			{
				switch (value)
				{
				case VWTPECU.ECUState.Disconnected:
					this.LastTimeActiveConnectionTestTimeout = 0L;
					this.PacketCounter = 0;
					break;
				case VWTPECU.ECUState.WaitingForIncomingConnection:
					this.WaitingForConnectionTimestampMs = this.sw.ElapsedMilliseconds;
					break;
				case VWTPECU.ECUState.ConnectedWaitingForData:
					if (this._CurrentState == VWTPECU.ECUState.WaitingForIncomingConnection)
					{
						this.LastTimeActiveConnectionTestTimeout = this.sw.ElapsedMilliseconds;
						this.PacketCounter = 0;
					}
					break;
				}
				this._CurrentState = value;
			}
		}

		// Token: 0x170011A6 RID: 4518
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x001E2B9D File Offset: 0x001E0D9D
		// (set) Token: 0x06002769 RID: 10089 RVA: 0x001E2BA5 File Offset: 0x001E0DA5
		private int PacketCounter
		{
			get
			{
				return this._PacketCounter;
			}
			set
			{
				if (value > 15)
				{
					this._PacketCounter = 0;
					return;
				}
				this._PacketCounter = value;
			}
		}

		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x0600276A RID: 10090 RVA: 0x001E2BBB File Offset: 0x001E0DBB
		// (set) Token: 0x0600276B RID: 10091 RVA: 0x001E2BC3 File Offset: 0x001E0DC3
		public string Unit
		{
			[CompilerGenerated]
			get
			{
				return this.<Unit>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Unit>k__BackingField = value;
			}
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x001E2BCC File Offset: 0x001E0DCC
		public async ValueTask<string> SendCommand(string cmd)
		{
			TaskAwaiter<bool> taskAwaiter2;
			if (this.CurrentState == VWTPECU.ECUState.Disconnected)
			{
				TaskAwaiter<bool> taskAwaiter = this.ChannelSetup().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					return "NO DATA";
				}
				this.CurrentState = VWTPECU.ECUState.WaitingForIncomingConnection;
			}
			if (App.OBDReader.ELMStatus.Header != this.RequestHeader)
			{
				await App.OBDReader.SendString("ATSH" + this.RequestHeader);
				await App.OBDReader.ReadData(2500, null, -1);
				App.OBDReader.SetELM327_LastSentHeader(this.RequestHeader);
			}
			if (App.OBDReader.ELMStatus.ATST != SharedSettings.Current.GetATST())
			{
				await App.OBDReader.SendString("ATST" + SharedSettings.Current.GetATST());
				await App.OBDReader.ReadData(2500, null, -1);
			}
			if (this.CurrentState == VWTPECU.ECUState.WaitingForIncomingConnection)
			{
				TaskAwaiter<bool> taskAwaiter = this.OpenConnection().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					this.CurrentState = VWTPECU.ECUState.Disconnected;
					return "NO DATA";
				}
			}
			if (this.CurrentState == VWTPECU.ECUState.ConnectedWaitingForConnectionTest)
			{
				TaskAwaiter<bool> taskAwaiter = this.SendConnectionTest().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
					this._CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
				}
			}
			byte[] dataBytes = null;
			if (this.CurrentState == VWTPECU.ECUState.ConnectedWaitingForData)
			{
				await this.SendConnectionConfirmationIfNeeded();
				dataBytes = await this.SendRequest(cmd);
				await this.SendConnectionConfirmationIfNeeded();
			}
			string text;
			if (dataBytes == null || dataBytes.Length == 0)
			{
				text = "NO DATA";
			}
			else
			{
				text = BitHelpers.ByteArrayToHexString(dataBytes);
			}
			return text;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x001E2C18 File Offset: 0x001E0E18
		private async Task<bool> ChannelSetup()
		{
			if (App.OBDReader.ELMStatus.Protocol != 6)
			{
				await App.OBDReader.SendString("ATSP6");
				await App.OBDReader.ReadData(2500, null, -1);
			}
			if (App.OBDReader.ELMStatus.Header != "200")
			{
				await App.OBDReader.SendString("ATSH200");
				await App.OBDReader.ReadData(2500, null, -1);
				App.OBDReader.SetELM327_LastSentHeader("200");
			}
			if (App.OBDReader.ELMStatus.ATST != SharedSettings.Current.VWTP_ChannelSetupATST)
			{
				await App.OBDReader.SendString("ATST0A");
				await App.OBDReader.ReadData(2500, null, -1);
			}
			this.ResponseHeader = this.Manager.GetFreeCRAChannel().ToString("X3");
			string text = string.Concat(new string[]
			{
				this.Unit,
				"C00010",
				this.ResponseHeader.Substring(1),
				"0",
				this.ResponseHeader.Substring(0, 1),
				"011"
			});
			await App.OBDReader.SendString(text);
			string text2 = await App.OBDReader.ReadData(2500, null, -1);
			List<CANFrameRaw> list = this.ELMResponseToCANFrames(text2);
			bool flag;
			if (list.Count == 0)
			{
				flag = false;
			}
			else
			{
				CANFrameRaw canframeRaw = list.FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data.Length == 7 && x.Data[1] == 208);
				if (canframeRaw == null)
				{
					if (this.ConnectionParametersFrame != null)
					{
						if (list.FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data[1] == 216) != null)
						{
							await App.OBDReader.SendString(this.ConnectionParametersFrame.DataHex);
							this.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
							await App.OBDReader.ReadData(2500, null, -1);
							this._CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
							return true;
						}
					}
					flag = false;
				}
				else
				{
					this.ResponseHeader = ((int)canframeRaw.Data[3] * 256 + (int)canframeRaw.Data[2]).ToString("X3");
					this.RequestHeader = ((int)canframeRaw.Data[5] * 256 + (int)canframeRaw.Data[4]).ToString("X3");
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x001E2C5C File Offset: 0x001E0E5C
		private async Task<bool> OpenConnection()
		{
			string text = "A00F8AFF32FF";
			await App.OBDReader.SendString(text);
			string text2 = await App.OBDReader.ReadData(2500, null, -1);
			CANFrameRaw canframeRaw = this.ELMResponseToCANFrames(text2).FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data.Length != 0 && x.Data[0] == 161);
			bool flag;
			if (canframeRaw == null)
			{
				flag = false;
			}
			else
			{
				this.ConnectionParametersFrame = canframeRaw;
				this.BlockSize = (int)canframeRaw.Data[1];
				this.TimeoutForReceiveT1 = VWTPECU.ConvertCanTimingToMsec(canframeRaw.Data[2]);
				this.TimeoutBetweenConsecutiveFramesT3 = VWTPECU.ConvertCanTimingToMsec(canframeRaw.Data[4]);
				this.CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x001E2CA0 File Offset: 0x001E0EA0
		private async Task<byte[]> SendRequest(string cmd)
		{
			byte[] array = BitHelpers.ConvertHexToBytesX(cmd);
			if (array.Length <= 5)
			{
				string text = "1" + this.PacketCounter.ToString("X1") + array.Length.ToString("X4") + cmd;
				int num = this.PacketCounter;
				this.PacketCounter = num + 1;
				await App.OBDReader.SendString(text);
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream(array);
				List<string> result = new List<string>(array.Length / 7 + 1);
				int num2 = 5;
				int num3 = 7;
				string text2 = "2" + this.PacketCounter.ToString("X1") + array.Length.ToString("X4") + cmd.Substring(0, num2 * 2);
				int num = this.PacketCounter;
				this.PacketCounter = num + 1;
				result.Add(text2);
				memoryStream.Seek((long)num2, SeekOrigin.Begin);
				while (memoryStream.Position < memoryStream.Length)
				{
					byte[] array2 = new byte[num3];
					long num4 = (long)memoryStream.Read(array2, 0, array2.Length);
					string text3 = BitHelpers.ByteArrayToHexString(array2).Substring(0, (int)num4 * 2);
					string text4 = "2";
					if (memoryStream.Position >= memoryStream.Length)
					{
						text4 = "1";
					}
					text3 = text4 + this.PacketCounter.ToString("X1") + text3;
					num = this.PacketCounter;
					this.PacketCounter = num + 1;
					result.Add(text3);
				}
				for (int i = 0; i < result.Count; i = num + 1)
				{
					if (i == result.Count - 2)
					{
						await App.OBDReader.SendString(result[i]);
						await App.OBDReader.ReadData(2500, null, -1);
					}
					else if (i == result.Count - 1)
					{
						await App.OBDReader.SendString(result[i]);
					}
					else
					{
						await App.OBDReader.SendString(result[i]);
						await App.OBDReader.ReadData(2500, null, -1);
					}
					num = i;
				}
				result = null;
			}
			List<CANFrameRaw> resultFrames = new List<CANFrameRaw>();
			List<CANFrameRaw>.Enumerator enumerator;
			CANFrameRaw lastFrame;
			do
			{
				string text5 = await App.OBDReader.ReadData(2500, null, -1);
				List<CANFrameRaw> frames = (from x in this.ELMResponseToCANFrames(text5)
					where x.CanIdHex == this.ResponseHeader
					select x).ToList<CANFrameRaw>();
				using (List<CANFrameRaw>.Enumerator enumerator = frames.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						await this.SendConnectionTestIfNeeded(enumerator.Current);
					}
				}
				enumerator = default(List<CANFrameRaw>.Enumerator);
				if (frames.Count <= 0)
				{
					goto IL_0864;
				}
				resultFrames.AddRange(frames);
				lastFrame = resultFrames[frames.Count - 1];
				if (!this.IsAckRequiredForDataFrame(lastFrame))
				{
					goto IL_085D;
				}
				int num5 = (int)((lastFrame.Data[0] & 15) + 1);
				if (num5 > 15)
				{
					num5 = 0;
				}
				int num6 = 176 + num5;
				await App.OBDReader.SendString(num6.ToString("X2"));
			}
			while (!this.IsEndOfMessage(lastFrame));
			using (List<CANFrameRaw>.Enumerator enumerator = this.ELMResponseToCANFrames(await App.OBDReader.ReadData(2500, null, -1)).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					await this.SendConnectionTestIfNeeded(enumerator.Current);
				}
			}
			enumerator = default(List<CANFrameRaw>.Enumerator);
			IL_085D:
			lastFrame = null;
			IL_0864:
			this.dataResult.Clear();
			foreach (CANFrameRaw canframeRaw in resultFrames)
			{
				if (canframeRaw.Data.Length > 1 && this.IsDataFrameVWTP(canframeRaw))
				{
					this.dataResult.AddRange(canframeRaw.Data.Skip(1));
				}
			}
			return this.dataResult.ToArray();
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x001E2CEC File Offset: 0x001E0EEC
		private async ValueTask<bool> SendConnectionTestIfNeeded(CANFrameRaw frame)
		{
			bool flag;
			if (this.ConnectionParametersFrame == null)
			{
				flag = false;
			}
			else if (frame.DataHex == "A3")
			{
				await this.SendConnectionTest();
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x001E2D38 File Offset: 0x001E0F38
		private async Task<bool> SendConnectionTest()
		{
			bool flag;
			if (this.ConnectionParametersFrame == null)
			{
				flag = false;
			}
			else
			{
				await App.OBDReader.SendString(this.ConnectionParametersFrame.DataHex);
				this.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
				await App.OBDReader.ReadData(2500, null, -1);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x001E2D7C File Offset: 0x001E0F7C
		private async ValueTask<bool> SendConnectionConfirmationIfNeeded()
		{
			bool flag;
			if (this.CurrentState != VWTPECU.ECUState.ConnectedWaitingForData)
			{
				flag = false;
			}
			else if (App.OBDReader.stopwatch.ElapsedMilliseconds - this.LastTimeActiveConnectionTestTimeout > 600L)
			{
				flag = await this.SendConnectionConfirmation();
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x001E2DC0 File Offset: 0x001E0FC0
		private async ValueTask<bool> SendConnectionConfirmation()
		{
			await App.OBDReader.SendString("A3");
			string text = await App.OBDReader.ReadData(2500, null, -1);
			List<CANFrameRaw> list = this.ELMResponseToCANFrames(text);
			if (list.Count > 0)
			{
				if (list.Any((CANFrameRaw x) => x.Data[0] == 161))
				{
					this.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
					return 1;
				}
			}
			this.CurrentState = VWTPECU.ECUState.Disconnected;
			return 0;
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x001E2E04 File Offset: 0x001E1004
		private bool IsDataFrameVWTP(CANFrameRaw frame)
		{
			if (frame == null)
			{
				return false;
			}
			if (frame.Data == null)
			{
				return false;
			}
			if (frame.Data.Length == 0)
			{
				return false;
			}
			int num = (int)(frame.Data[0] & 240);
			return num == 0 || num == 16 || num == 32 || num == 48;
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x001E2E4F File Offset: 0x001E104F
		private bool IsEndOfMessage(CANFrameRaw frame)
		{
			return frame != null && frame.Data != null && frame.Data.Length != 0 && (frame.Data[0] & 16) == 16;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x001E2E7D File Offset: 0x001E107D
		private bool IsAckRequiredForDataFrame(CANFrameRaw frame)
		{
			return frame != null && frame.Data != null && frame.Data.Length != 0 && (frame.Data[0] & 32) != 32;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x001E2EAC File Offset: 0x001E10AC
		internal static int ConvertCanTimingToMsec(byte b)
		{
			int num = (b >> 6) & 3;
			int num2 = (int)(b & 63);
			switch (num)
			{
			case 0:
				return num2 / 1000;
			case 1:
				return num2;
			case 2:
				return num2 * 10;
			case 3:
				return num2 * 100;
			default:
				return num2;
			}
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x001E2EF4 File Offset: 0x001E10F4
		private List<CANFrameRaw> ELMResponseToCANFrames(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return VWTPECU.emptyFrameList;
			}
			if (data.Contains("NO DATA"))
			{
				return VWTPECU.emptyFrameList;
			}
			string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 0)
			{
				return VWTPECU.emptyFrameList;
			}
			List<CANFrameRaw> list = new List<CANFrameRaw>(array.Length);
			foreach (string text in array)
			{
				string text2 = text.Substring(0, 3);
				string text3 = text.Substring(3);
				if (text3.Length % 2 == 0)
				{
					CANFrameRaw canframeRaw = new CANFrameRaw(text2, text3.Length / 2, text3);
					list.Add(canframeRaw);
				}
			}
			return list;
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x001E2F94 File Offset: 0x001E1194
		// Note: this type is marked as 'beforefieldinit'.
		static VWTPECU()
		{
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x001E2FAC File Offset: 0x001E11AC
		[CompilerGenerated]
		private bool <SendRequest>b__39_0(CANFrameRaw x)
		{
			return x.CanIdHex == this.ResponseHeader;
		}

		// Token: 0x040015CB RID: 5579
		private IVWTPManager Manager;

		// Token: 0x040015CC RID: 5580
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x040015CD RID: 5581
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x040015CE RID: 5582
		public int TimeoutForReceiveT1 = 200;

		// Token: 0x040015CF RID: 5583
		public int TimeoutBetweenConsecutiveFramesT3 = 10;

		// Token: 0x040015D0 RID: 5584
		public int PassiveConnectionTestTimeout = 1050;

		// Token: 0x040015D1 RID: 5585
		private const int TIME_K = 1;

		// Token: 0x040015D2 RID: 5586
		private long WaitingForConnectionTimestampMs;

		// Token: 0x040015D3 RID: 5587
		private long LastTimeActiveConnectionTestTimeout;

		// Token: 0x040015D4 RID: 5588
		private int _BlockSize = 15;

		// Token: 0x040015D5 RID: 5589
		private CANFrameRaw ConnectionParametersFrame;

		// Token: 0x040015D6 RID: 5590
		private VWTPECU.ECUState _CurrentState;

		// Token: 0x040015D7 RID: 5591
		private int _PacketCounter;

		// Token: 0x040015D8 RID: 5592
		[CompilerGenerated]
		private string <Unit>k__BackingField;

		// Token: 0x040015D9 RID: 5593
		private List<byte> dataResult = new List<byte>();

		// Token: 0x040015DA RID: 5594
		private static List<CANFrameRaw> emptyFrameList = new List<CANFrameRaw>(0);

		// Token: 0x040015DB RID: 5595
		private static List<string> epmtyStringList = new List<string>(0);

		// Token: 0x020003B5 RID: 949
		public enum ECUState
		{
			// Token: 0x040015DD RID: 5597
			Disconnected,
			// Token: 0x040015DE RID: 5598
			WaitingForIncomingConnection,
			// Token: 0x040015DF RID: 5599
			ConnectedWaitingForData,
			// Token: 0x040015E0 RID: 5600
			ConnectedWaitingForAck,
			// Token: 0x040015E1 RID: 5601
			ConnectedWaitingForConnectionTest
		}

		// Token: 0x020003B6 RID: 950
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600277B RID: 10107 RVA: 0x001E2FBF File Offset: 0x001E11BF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600277C RID: 10108 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600277D RID: 10109 RVA: 0x001E2FCB File Offset: 0x001E11CB
			internal bool <ChannelSetup>b__37_0(CANFrameRaw x)
			{
				return x.Data != null && x.Data.Length == 7 && x.Data[1] == 208;
			}

			// Token: 0x0600277E RID: 10110 RVA: 0x001E2FF1 File Offset: 0x001E11F1
			internal bool <ChannelSetup>b__37_1(CANFrameRaw x)
			{
				return x.Data != null && x.Data[1] == 216;
			}

			// Token: 0x0600277F RID: 10111 RVA: 0x001E300C File Offset: 0x001E120C
			internal bool <OpenConnection>b__38_0(CANFrameRaw x)
			{
				return x.Data != null && x.Data.Length != 0 && x.Data[0] == 161;
			}

			// Token: 0x06002780 RID: 10112 RVA: 0x001E3030 File Offset: 0x001E1230
			internal bool <SendConnectionConfirmation>b__44_0(CANFrameRaw x)
			{
				return x.Data[0] == 161;
			}

			// Token: 0x040015E2 RID: 5602
			public static readonly VWTPECU.<>c <>9 = new VWTPECU.<>c();

			// Token: 0x040015E3 RID: 5603
			public static Func<CANFrameRaw, bool> <>9__37_0;

			// Token: 0x040015E4 RID: 5604
			public static Func<CANFrameRaw, bool> <>9__37_1;

			// Token: 0x040015E5 RID: 5605
			public static Func<CANFrameRaw, bool> <>9__38_0;

			// Token: 0x040015E6 RID: 5606
			public static Func<CANFrameRaw, bool> <>9__44_0;
		}

		// Token: 0x020003B7 RID: 951
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ChannelSetup>d__37 : IAsyncStateMachine
		{
			// Token: 0x06002781 RID: 10113 RVA: 0x001E3044 File Offset: 0x001E1244
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_011A;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01A0;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0209;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02A3;
					}
					case 5:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_030C;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03DB;
					}
					case 7:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0444;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0532;
					}
					case 9:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_05B1;
					}
					default:
						if (App.OBDReader.ELMStatus.Protocol == 6)
						{
							goto IL_0122;
						}
						taskAwaiter = App.OBDReader.SendString("ATSP6").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_011A:
					taskAwaiter3.GetResult();
					IL_0122:
					if (!(App.OBDReader.ELMStatus.Header != "200"))
					{
						goto IL_0220;
					}
					taskAwaiter = App.OBDReader.SendString("ATSH200").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter, ref this);
						return;
					}
					IL_01A0:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0209:
					taskAwaiter3.GetResult();
					App.OBDReader.SetELM327_LastSentHeader("200");
					IL_0220:
					if (!(App.OBDReader.ELMStatus.ATST != SharedSettings.Current.VWTP_ChannelSetupATST))
					{
						goto IL_0314;
					}
					taskAwaiter = App.OBDReader.SendString("ATST0A").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter, ref this);
						return;
					}
					IL_02A3:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_030C:
					taskAwaiter3.GetResult();
					IL_0314:
					vwtpecu.ResponseHeader = vwtpecu.Manager.GetFreeCRAChannel().ToString("X3");
					string text = string.Concat(new string[]
					{
						vwtpecu.Unit,
						"C00010",
						vwtpecu.ResponseHeader.Substring(1),
						"0",
						vwtpecu.ResponseHeader.Substring(0, 1),
						"011"
					});
					taskAwaiter = App.OBDReader.SendString(text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter, ref this);
						return;
					}
					IL_03DB:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0444:
					string result = taskAwaiter3.GetResult();
					List<CANFrameRaw> list = vwtpecu.ELMResponseToCANFrames(result);
					if (list.Count == 0)
					{
						flag = false;
						goto IL_063F;
					}
					CANFrameRaw canframeRaw = list.FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data.Length == 7 && x.Data[1] == 208);
					if (canframeRaw == null)
					{
						if (vwtpecu.ConnectionParametersFrame != null)
						{
							if (list.FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data[1] == 216) != null)
							{
								taskAwaiter = App.OBDReader.SendString(vwtpecu.ConnectionParametersFrame.DataHex).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 8;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0532;
							}
						}
						flag = false;
						goto IL_063F;
					}
					vwtpecu.ResponseHeader = ((int)canframeRaw.Data[3] * 256 + (int)canframeRaw.Data[2]).ToString("X3");
					vwtpecu.RequestHeader = ((int)canframeRaw.Data[5] * 256 + (int)canframeRaw.Data[4]).ToString("X3");
					flag = true;
					goto IL_063F;
					IL_0532:
					taskAwaiter.GetResult();
					vwtpecu.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 9;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<ChannelSetup>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_05B1:
					taskAwaiter3.GetResult();
					vwtpecu._CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
					flag = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_063F:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002782 RID: 10114 RVA: 0x001E36C0 File Offset: 0x001E18C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015E7 RID: 5607
			public int <>1__state;

			// Token: 0x040015E8 RID: 5608
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040015E9 RID: 5609
			public VWTPECU <>4__this;

			// Token: 0x040015EA RID: 5610
			private TaskAwaiter <>u__1;

			// Token: 0x040015EB RID: 5611
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020003B8 RID: 952
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OpenConnection>d__38 : IAsyncStateMachine
		{
			// Token: 0x06002783 RID: 10115 RVA: 0x001E36D0 File Offset: 0x001E18D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00E3;
						}
						string text = "A00F8AFF32FF";
						taskAwaiter3 = App.OBDReader.SendString(text).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<OpenConnection>d__38>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<OpenConnection>d__38>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E3:
					string result = taskAwaiter.GetResult();
					CANFrameRaw canframeRaw = vwtpecu.ELMResponseToCANFrames(result).FirstOrDefault((CANFrameRaw x) => x.Data != null && x.Data.Length != 0 && x.Data[0] == 161);
					if (canframeRaw == null)
					{
						flag = false;
					}
					else
					{
						vwtpecu.ConnectionParametersFrame = canframeRaw;
						vwtpecu.BlockSize = (int)canframeRaw.Data[1];
						vwtpecu.TimeoutForReceiveT1 = VWTPECU.ConvertCanTimingToMsec(canframeRaw.Data[2]);
						vwtpecu.TimeoutBetweenConsecutiveFramesT3 = VWTPECU.ConvertCanTimingToMsec(canframeRaw.Data[4]);
						vwtpecu.CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
						flag = true;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002784 RID: 10116 RVA: 0x001E3894 File Offset: 0x001E1A94
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015EC RID: 5612
			public int <>1__state;

			// Token: 0x040015ED RID: 5613
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040015EE RID: 5614
			public VWTPECU <>4__this;

			// Token: 0x040015EF RID: 5615
			private TaskAwaiter <>u__1;

			// Token: 0x040015F0 RID: 5616
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020003B9 RID: 953
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendCommand>d__36 : IAsyncStateMachine
		{
			// Token: 0x06002785 RID: 10117 RVA: 0x001E38A4 File Offset: 0x001E1AA4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				string text;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					TaskAwaiter<string> taskAwaiter6;
					ValueTaskAwaiter<bool> valueTaskAwaiter;
					TaskAwaiter<byte[]> taskAwaiter8;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_013D;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_01A6;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0250;
					}
					case 4:
					{
						TaskAwaiter<string> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_02B9;
					}
					case 5:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_031E;
					case 6:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0396;
					case 7:
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num2 = -1;
						goto IL_0429;
					}
					case 8:
					{
						TaskAwaiter<byte[]> taskAwaiter9;
						taskAwaiter8 = taskAwaiter9;
						taskAwaiter9 = default(TaskAwaiter<byte[]>);
						num2 = -1;
						goto IL_048E;
					}
					case 9:
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num2 = -1;
						goto IL_04FB;
					}
					default:
						if (vwtpecu.CurrentState != VWTPECU.ECUState.Disconnected)
						{
							goto IL_00B3;
						}
						taskAwaiter3 = vwtpecu.ChannelSetup().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						text = "NO DATA";
						goto IL_054A;
					}
					vwtpecu.CurrentState = VWTPECU.ECUState.WaitingForIncomingConnection;
					IL_00B3:
					if (!(App.OBDReader.ELMStatus.Header != vwtpecu.RequestHeader))
					{
						goto IL_01BE;
					}
					taskAwaiter4 = App.OBDReader.SendString("ATSH" + vwtpecu.RequestHeader).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendCommand>d__36>(ref taskAwaiter4, ref this);
						return;
					}
					IL_013D:
					taskAwaiter4.GetResult();
					taskAwaiter6 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter6, ref this);
						return;
					}
					IL_01A6:
					taskAwaiter6.GetResult();
					App.OBDReader.SetELM327_LastSentHeader(vwtpecu.RequestHeader);
					IL_01BE:
					if (!(App.OBDReader.ELMStatus.ATST != SharedSettings.Current.GetATST()))
					{
						goto IL_02C1;
					}
					taskAwaiter4 = App.OBDReader.SendString("ATST" + SharedSettings.Current.GetATST()).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendCommand>d__36>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0250:
					taskAwaiter4.GetResult();
					taskAwaiter6 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<string> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter6, ref this);
						return;
					}
					IL_02B9:
					taskAwaiter6.GetResult();
					IL_02C1:
					if (vwtpecu.CurrentState != VWTPECU.ECUState.WaitingForIncomingConnection)
					{
						goto IL_0339;
					}
					taskAwaiter3 = vwtpecu.OpenConnection().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter3, ref this);
						return;
					}
					IL_031E:
					if (!taskAwaiter3.GetResult())
					{
						vwtpecu.CurrentState = VWTPECU.ECUState.Disconnected;
						text = "NO DATA";
						goto IL_054A;
					}
					IL_0339:
					if (vwtpecu.CurrentState != VWTPECU.ECUState.ConnectedWaitingForConnectionTest)
					{
						goto IL_03BB;
					}
					taskAwaiter3 = vwtpecu.SendConnectionTest().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 6;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0396:
					if (taskAwaiter3.GetResult())
					{
						vwtpecu.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
						vwtpecu._CurrentState = VWTPECU.ECUState.ConnectedWaitingForData;
					}
					IL_03BB:
					dataBytes = null;
					if (vwtpecu.CurrentState != VWTPECU.ECUState.ConnectedWaitingForData)
					{
						goto IL_0503;
					}
					valueTaskAwaiter = vwtpecu.SendConnectionConfirmationIfNeeded().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 7;
						ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, VWTPECU.<SendCommand>d__36>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_0429:
					valueTaskAwaiter.GetResult();
					taskAwaiter8 = vwtpecu.SendRequest(cmd).GetAwaiter();
					if (!taskAwaiter8.IsCompleted)
					{
						num2 = 8;
						TaskAwaiter<byte[]> taskAwaiter9 = taskAwaiter8;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte[]>, VWTPECU.<SendCommand>d__36>(ref taskAwaiter8, ref this);
						return;
					}
					IL_048E:
					byte[] result = taskAwaiter8.GetResult();
					dataBytes = result;
					valueTaskAwaiter = vwtpecu.SendConnectionConfirmationIfNeeded().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 9;
						ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, VWTPECU.<SendCommand>d__36>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_04FB:
					valueTaskAwaiter.GetResult();
					IL_0503:
					if (dataBytes == null || dataBytes.Length == 0)
					{
						text = "NO DATA";
					}
					else
					{
						text = BitHelpers.ByteArrayToHexString(dataBytes);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					dataBytes = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_054A:
				num2 = -2;
				dataBytes = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06002786 RID: 10118 RVA: 0x001E3E34 File Offset: 0x001E2034
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015F1 RID: 5617
			public int <>1__state;

			// Token: 0x040015F2 RID: 5618
			public AsyncValueTaskMethodBuilder<string> <>t__builder;

			// Token: 0x040015F3 RID: 5619
			public VWTPECU <>4__this;

			// Token: 0x040015F4 RID: 5620
			public string cmd;

			// Token: 0x040015F5 RID: 5621
			private byte[] <dataBytes>5__2;

			// Token: 0x040015F6 RID: 5622
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040015F7 RID: 5623
			private TaskAwaiter <>u__2;

			// Token: 0x040015F8 RID: 5624
			private TaskAwaiter<string> <>u__3;

			// Token: 0x040015F9 RID: 5625
			private ValueTaskAwaiter<bool> <>u__4;

			// Token: 0x040015FA RID: 5626
			private TaskAwaiter<byte[]> <>u__5;
		}

		// Token: 0x020003BA RID: 954
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendConnectionConfirmation>d__44 : IAsyncStateMachine
		{
			// Token: 0x06002787 RID: 10119 RVA: 0x001E3E44 File Offset: 0x001E2044
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00E1;
						}
						taskAwaiter3 = App.OBDReader.SendString("A3").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendConnectionConfirmation>d__44>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendConnectionConfirmation>d__44>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E1:
					string result = taskAwaiter.GetResult();
					List<CANFrameRaw> list = vwtpecu.ELMResponseToCANFrames(result);
					if (list.Count > 0)
					{
						if (list.Any((CANFrameRaw x) => x.Data[0] == 161))
						{
							vwtpecu.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
							flag = true;
							goto IL_0161;
						}
					}
					vwtpecu.CurrentState = VWTPECU.ECUState.Disconnected;
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0161:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002788 RID: 10120 RVA: 0x001E3FE4 File Offset: 0x001E21E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040015FB RID: 5627
			public int <>1__state;

			// Token: 0x040015FC RID: 5628
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040015FD RID: 5629
			public VWTPECU <>4__this;

			// Token: 0x040015FE RID: 5630
			private TaskAwaiter <>u__1;

			// Token: 0x040015FF RID: 5631
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020003BB RID: 955
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendConnectionConfirmationIfNeeded>d__43 : IAsyncStateMachine
		{
			// Token: 0x06002789 RID: 10121 RVA: 0x001E3FF4 File Offset: 0x001E21F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					ValueTaskAwaiter<bool> valueTaskAwaiter;
					if (num != 0)
					{
						if (vwtpecu.CurrentState != VWTPECU.ECUState.ConnectedWaitingForData)
						{
							flag = false;
							goto IL_00BB;
						}
						if (App.OBDReader.stopwatch.ElapsedMilliseconds - vwtpecu.LastTimeActiveConnectionTestTimeout <= 600L)
						{
							flag = true;
							goto IL_00BB;
						}
						valueTaskAwaiter = vwtpecu.SendConnectionConfirmation().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, VWTPECU.<SendConnectionConfirmationIfNeeded>d__43>(ref valueTaskAwaiter, ref this);
							return;
						}
					}
					else
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num2 = -1;
					}
					flag = valueTaskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00BB:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600278A RID: 10122 RVA: 0x001E40E0 File Offset: 0x001E22E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001600 RID: 5632
			public int <>1__state;

			// Token: 0x04001601 RID: 5633
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001602 RID: 5634
			public VWTPECU <>4__this;

			// Token: 0x04001603 RID: 5635
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020003BC RID: 956
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendConnectionTest>d__42 : IAsyncStateMachine
		{
			// Token: 0x0600278B RID: 10123 RVA: 0x001E40F0 File Offset: 0x001E22F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0105;
						}
						if (vwtpecu.ConnectionParametersFrame == null)
						{
							flag = false;
							goto IL_012A;
						}
						taskAwaiter3 = App.OBDReader.SendString(vwtpecu.ConnectionParametersFrame.DataHex).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendConnectionTest>d__42>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					vwtpecu.LastTimeActiveConnectionTestTimeout = App.OBDReader.stopwatch.ElapsedMilliseconds;
					taskAwaiter = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendConnectionTest>d__42>(ref taskAwaiter, ref this);
						return;
					}
					IL_0105:
					taskAwaiter.GetResult();
					flag = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_012A:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600278C RID: 10124 RVA: 0x001E4258 File Offset: 0x001E2458
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001604 RID: 5636
			public int <>1__state;

			// Token: 0x04001605 RID: 5637
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001606 RID: 5638
			public VWTPECU <>4__this;

			// Token: 0x04001607 RID: 5639
			private TaskAwaiter <>u__1;

			// Token: 0x04001608 RID: 5640
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020003BD RID: 957
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendConnectionTestIfNeeded>d__41 : IAsyncStateMachine
		{
			// Token: 0x0600278D RID: 10125 RVA: 0x001E4268 File Offset: 0x001E2468
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				bool flag;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						if (vwtpecu.ConnectionParametersFrame == null)
						{
							flag = false;
							goto IL_00B1;
						}
						if (!(frame.DataHex == "A3"))
						{
							flag = false;
							goto IL_00B1;
						}
						taskAwaiter = vwtpecu.SendConnectionTest().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VWTPECU.<SendConnectionTestIfNeeded>d__41>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					flag = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00B1:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600278E RID: 10126 RVA: 0x001E434C File Offset: 0x001E254C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001609 RID: 5641
			public int <>1__state;

			// Token: 0x0400160A RID: 5642
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400160B RID: 5643
			public VWTPECU <>4__this;

			// Token: 0x0400160C RID: 5644
			public CANFrameRaw frame;

			// Token: 0x0400160D RID: 5645
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020003BE RID: 958
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendRequest>d__39 : IAsyncStateMachine
		{
			// Token: 0x0600278F RID: 10127 RVA: 0x001E435C File Offset: 0x001E255C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VWTPECU vwtpecu = this;
				byte[] array3;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					int num7;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_02C9;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0332;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_03C0;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0438;
					}
					case 5:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_04A1;
					}
					case 6:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0545;
					}
					case 7:
					{
						IL_0583:
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter;
							if (num != 7)
							{
								if (!enumerator.MoveNext())
								{
									goto IL_060E;
								}
								CANFrameRaw canframeRaw = enumerator.Current;
								valueTaskAwaiter = vwtpecu.SendConnectionTestIfNeeded(canframeRaw).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 7);
									ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, VWTPECU.<SendRequest>d__39>(ref valueTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
								num = (num2 = -1);
							}
							valueTaskAwaiter.GetResult();
							IL_060E:;
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<CANFrameRaw>.Enumerator);
						if (frames.Count <= 0)
						{
							goto IL_0864;
						}
						resultFrames.AddRange(frames);
						lastFrame = resultFrames[frames.Count - 1];
						if (!vwtpecu.IsAckRequiredForDataFrame(lastFrame))
						{
							goto IL_085D;
						}
						int num3 = (int)((lastFrame.Data[0] & 15) + 1);
						if (num3 > 15)
						{
							num3 = 0;
						}
						int num4 = 176 + num3;
						taskAwaiter = App.OBDReader.SendString(num4.ToString("X2")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 8);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendRequest>d__39>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0711;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0711;
					}
					case 9:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_078C;
					}
					case 10:
						IL_07AC:
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter;
							if (num != 10)
							{
								if (!enumerator.MoveNext())
								{
									goto IL_0839;
								}
								CANFrameRaw canframeRaw2 = enumerator.Current;
								valueTaskAwaiter = vwtpecu.SendConnectionTestIfNeeded(canframeRaw2).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 10);
									ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, VWTPECU.<SendRequest>d__39>(ref valueTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
								num = (num2 = -1);
							}
							valueTaskAwaiter.GetResult();
							IL_0839:;
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<CANFrameRaw>.Enumerator);
						goto IL_085D;
					default:
					{
						byte[] array = BitHelpers.ConvertHexToBytesX(cmd);
						if (array.Length > 5)
						{
							MemoryStream memoryStream = new MemoryStream(array);
							result = new List<string>(array.Length / 7 + 1);
							int num5 = 5;
							int num6 = 7;
							string text = "2" + vwtpecu.PacketCounter.ToString("X1") + array.Length.ToString("X4") + cmd.Substring(0, num5 * 2);
							num7 = vwtpecu.PacketCounter;
							vwtpecu.PacketCounter = num7 + 1;
							result.Add(text);
							memoryStream.Seek((long)num5, SeekOrigin.Begin);
							while (memoryStream.Position < memoryStream.Length)
							{
								byte[] array2 = new byte[num6];
								long num8 = (long)memoryStream.Read(array2, 0, array2.Length);
								string text2 = BitHelpers.ByteArrayToHexString(array2);
								text2 = text2.Substring(0, (int)num8 * 2);
								string text3 = "2";
								if (memoryStream.Position >= memoryStream.Length)
								{
									text3 = "1";
								}
								text2 = text3 + vwtpecu.PacketCounter.ToString("X1") + text2;
								num7 = vwtpecu.PacketCounter;
								vwtpecu.PacketCounter = num7 + 1;
								result.Add(text2);
							}
							i = 0;
							goto IL_04BB;
						}
						string text4 = "1" + vwtpecu.PacketCounter.ToString("X1") + array.Length.ToString("X4") + cmd;
						num7 = vwtpecu.PacketCounter;
						vwtpecu.PacketCounter = num7 + 1;
						taskAwaiter = App.OBDReader.SendString(text4).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendRequest>d__39>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					goto IL_04D8;
					IL_02C9:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendRequest>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0332:
					taskAwaiter3.GetResult();
					goto IL_04A9;
					IL_03C0:
					taskAwaiter.GetResult();
					goto IL_04A9;
					IL_0438:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 5);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendRequest>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_04A1:
					taskAwaiter3.GetResult();
					IL_04A9:
					num7 = i;
					i = num7 + 1;
					IL_04BB:
					if (i >= result.Count)
					{
						result = null;
					}
					else if (i == result.Count - 2)
					{
						taskAwaiter = App.OBDReader.SendString(result[i]).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendRequest>d__39>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_02C9;
					}
					else if (i == result.Count - 1)
					{
						taskAwaiter = App.OBDReader.SendString(result[i]).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendRequest>d__39>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_03C0;
					}
					else
					{
						taskAwaiter = App.OBDReader.SendString(result[i]).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VWTPECU.<SendRequest>d__39>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0438;
					}
					IL_04D8:
					resultFrames = new List<CANFrameRaw>();
					IL_04E3:
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 6);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendRequest>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0545:
					string result2 = taskAwaiter3.GetResult();
					frames = (from x in vwtpecu.ELMResponseToCANFrames(result2)
						where x.CanIdHex == base.ResponseHeader
						select x).ToList<CANFrameRaw>();
					enumerator = frames.GetEnumerator();
					goto IL_0583;
					IL_0711:
					taskAwaiter.GetResult();
					if (!vwtpecu.IsEndOfMessage(lastFrame))
					{
						goto IL_04E3;
					}
					taskAwaiter3 = App.OBDReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 9);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VWTPECU.<SendRequest>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_078C:
					string result3 = taskAwaiter3.GetResult();
					List<CANFrameRaw> list = vwtpecu.ELMResponseToCANFrames(result3);
					enumerator = list.GetEnumerator();
					goto IL_07AC;
					IL_085D:
					lastFrame = null;
					IL_0864:
					vwtpecu.dataResult.Clear();
					List<CANFrameRaw>.Enumerator enumerator2 = resultFrames.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							CANFrameRaw canframeRaw3 = enumerator2.Current;
							if (canframeRaw3.Data.Length > 1 && vwtpecu.IsDataFrameVWTP(canframeRaw3))
							{
								vwtpecu.dataResult.AddRange(canframeRaw3.Data.Skip(1));
							}
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
					array3 = vwtpecu.dataResult.ToArray();
				}
				catch (Exception ex)
				{
					num2 = -2;
					resultFrames = null;
					frames = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				resultFrames = null;
				frames = null;
				this.<>t__builder.SetResult(array3);
			}

			// Token: 0x06002790 RID: 10128 RVA: 0x001E4CF8 File Offset: 0x001E2EF8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400160E RID: 5646
			public int <>1__state;

			// Token: 0x0400160F RID: 5647
			public AsyncTaskMethodBuilder<byte[]> <>t__builder;

			// Token: 0x04001610 RID: 5648
			public string cmd;

			// Token: 0x04001611 RID: 5649
			public VWTPECU <>4__this;

			// Token: 0x04001612 RID: 5650
			private List<CANFrameRaw> <resultFrames>5__2;

			// Token: 0x04001613 RID: 5651
			private List<CANFrameRaw> <frames>5__3;

			// Token: 0x04001614 RID: 5652
			private TaskAwaiter <>u__1;

			// Token: 0x04001615 RID: 5653
			private List<string> <result>5__4;

			// Token: 0x04001616 RID: 5654
			private int <i>5__5;

			// Token: 0x04001617 RID: 5655
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001618 RID: 5656
			private List<CANFrameRaw>.Enumerator <>7__wrap5;

			// Token: 0x04001619 RID: 5657
			private ValueTaskAwaiter<bool> <>u__3;

			// Token: 0x0400161A RID: 5658
			private CANFrameRaw <lastFrame>5__7;
		}
	}
}
