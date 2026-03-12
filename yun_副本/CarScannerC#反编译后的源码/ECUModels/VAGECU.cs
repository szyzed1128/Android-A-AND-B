using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.DTC.VagDTC;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000520 RID: 1312
	internal class VAGECU : CAN11bitECU, IVagECU
	{
		// Token: 0x060031D3 RID: 12755 RVA: 0x00227A24 File Offset: 0x00225C24
		public VAGECU(string name, string udsRequest, string tp20_address)
		{
			this.Name = name;
			base.RequestHeader = udsRequest;
			if (!string.IsNullOrEmpty(base.RequestHeader))
			{
				base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(udsRequest, "Audi", null);
			}
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1003" };
			base.CloseSessionCommands = new List<string> { "1001" };
			if (SharedSettings.Current.ShowExperimental)
			{
				base.CloseSessionCommands = new List<string> { "1001", "ATCAF0", "A8", "ATCAF1" };
			}
			base.ReadDTCCommands = new List<string> { "1902AE", "1800FF00", "1800FFFF" };
			if (base.RequestHeader == "713")
			{
				base.ReadDTCCommands = new List<string> { "190208", "190204", "1800FF00", "1800FFFF" };
			}
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			if (!string.IsNullOrEmpty(tp20_address))
			{
				base.ExtendedAddress = tp20_address;
				this.vwTp20Worker = new VwTp20Worker(base.ExtendedAddress);
			}
			if (string.IsNullOrEmpty(base.ExtendedAddress) && !string.IsNullOrEmpty(base.RequestHeader))
			{
				this.Name += " (UDS)";
			}
			else if (!string.IsNullOrEmpty(base.ExtendedAddress) && string.IsNullOrEmpty(base.RequestHeader))
			{
				this.Name += " (VW TP2.0)";
			}
			else
			{
				this.WorkingMode = VAGECU.WorkingModes.Both;
			}
			this.ResetWorkingMode();
			VAGECU.HasVWTP20Unit09 = true;
			VAGECU.HasVWTP20Unit19 = true;
			if (!this.Name.Contains(" (VW TP2.0)"))
			{
				this.AddUDSIdents();
				base.IdentsASCII["22F187"] = "VAG Part number";
				base.IdentsASCII["22F191"] = "Hardware Part number";
				base.IdentsASCII["22F19E"] = "ASAM/ODX";
				base.IdentsASCII["22F189"] = "Software version";
				base.IdentsASCII["22F1A3"] = "Hardware version";
				base.IdentsASCII["22F197"] = "System designation";
				base.IdentsASCII["22F1A2"] = "ASAM/ODX version";
				base.IdentsASCII["22F1B1"] = "Application software identification (id/version/valid/plausible/used)";
				base.IdentsHEX["222A2F"] = "Unit ID";
				base.IdentsHEX["22F17B"] = "Date of last coding";
				base.IdentsASCII["22F17C"] = "FAZIT Identification";
				base.IdentsHEX["22F186"] = "Diagnostic mode";
				base.IdentsHEX["22F19B"] = "Date of last adaptation";
				base.IdentsASCII["22F1A0"] = "Parameter set part number";
				base.IdentsASCII["22F1A1"] = "Parameter set version";
				base.IdentsHEX["22F1A9"] = "Parametrization date";
				base.IdentsASCII["22F1AA"] = "System abbreviation";
				if (base.RequestHeader == "7E0")
				{
					base.IdentsASCII["22F1AD"] = "Engine code";
				}
				base.IdentsHEX.TryAdd("220600", "Long Coding");
				if (base.IdentsHEX.ContainsKey("22F191"))
				{
					base.IdentsHEX.Remove("22F191");
				}
				if (base.IdentsASCII.ContainsKey("22F19A"))
				{
					base.IdentsASCII.Remove("22F19A");
				}
				if (base.RequestHeader == "773")
				{
					base.IdentsASCII.TryAdd("223C0A", "VCRN");
				}
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x060031D4 RID: 12756 RVA: 0x00227E44 File Offset: 0x00226044
		public override string ShortName
		{
			get
			{
				if (string.IsNullOrEmpty(this.Name))
				{
					return "";
				}
				int num = this.Name.IndexOf('.');
				return this.Name.Substring(0, num);
			}
		}

		// Token: 0x060031D5 RID: 12757 RVA: 0x00227E80 File Offset: 0x00226080
		private void ResetWorkingMode()
		{
			if (string.IsNullOrEmpty(base.ExtendedAddress) && !string.IsNullOrEmpty(base.RequestHeader))
			{
				this.WorkingMode = VAGECU.WorkingModes.UDS;
				return;
			}
			if (!string.IsNullOrEmpty(base.ExtendedAddress) && string.IsNullOrEmpty(base.RequestHeader))
			{
				this.WorkingMode = VAGECU.WorkingModes.TP20;
				return;
			}
			this.WorkingMode = VAGECU.WorkingModes.Both;
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x060031D6 RID: 12758 RVA: 0x00227ED8 File Offset: 0x002260D8
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return VAGECU.BuildList();
			}
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x00227EE0 File Offset: 0x002260E0
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("710", "1F", "19. CAN Gateway"),
				new ValueTuple<string, string, string>("70E", "20", "09. Onboard supply control unit"),
				new ValueTuple<string, string, string>("7E1", "02", "02. " + CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string, string>("713", "03", "03. " + CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("7E0", "01", "01. " + CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("712", "13", "04. Power steering"),
				new ValueTuple<string, string, string>("732", "31", "05. Authorization system for access and starting the engine"),
				new ValueTuple<string, string, string>("74D", "35", "06. Front passenger seat"),
				new ValueTuple<string, string, string>("", "3F", "07. Head unit controller"),
				new ValueTuple<string, string, string>("746", "2C", "08. Heater and climate"),
				new ValueTuple<string, string, string>("", "24", "0D. Slide Door Left"),
				new ValueTuple<string, string, string>("770", "58", "0E. Media player 1"),
				new ValueTuple<string, string, string>("", "4F", "0F. Digital radio tuner"),
				new ValueTuple<string, string, string>("70A", "1D", "10. Parking Assistant 2"),
				new ValueTuple<string, string, string>("7E2", "15", "11. Engine Electronics No. 2"),
				new ValueTuple<string, string, string>("757", "0B", "13. Active cruise control"),
				new ValueTuple<string, string, string>("772", "0C", "14. Electronic system Damping controls"),
				new ValueTuple<string, string, string>("715", "05", "15. SRS"),
				new ValueTuple<string, string, string>("70C", "2A", "16. Steering column"),
				new ValueTuple<string, string, string>("714", "07", "17. Dashboard"),
				new ValueTuple<string, string, string>("76A", "2F", "18. Additional heater"),
				new ValueTuple<string, string, string>("", "40", "1C. Car location"),
				new ValueTuple<string, string, string>("716", "1E", "1B. Active steering"),
				new ValueTuple<string, string, string>("", "2B", "1D. Driver identification"),
				new ValueTuple<string, string, string>("", "50", "1E. Mediaplayer 2"),
				new ValueTuple<string, string, string>("", "5F", "1F. Satelight radio tuner"),
				new ValueTuple<string, string, string>("730", "16", "20. FLA"),
				new ValueTuple<string, string, string>("728", "16", "21. Energy Management 2"),
				new ValueTuple<string, string, string>("70F", "0A", "22. Four-wheel drive"),
				new ValueTuple<string, string, string>("73B", "0D", "23. Increased braking force"),
				new ValueTuple<string, string, string>("711", "14", "25. Immobilizer"),
				new ValueTuple<string, string, string>("72D", "30", "26. Electric folding roof"),
				new ValueTuple<string, string, string>("", "3E", "27. Rear head unit controller"),
				new ValueTuple<string, string, string>("71A", "45", "28. Climate Control Panel"),
				new ValueTuple<string, string, string>("", "38", "29. Left side lights control"),
				new ValueTuple<string, string, string>("731", "", "2B. Steering column lock"),
				new ValueTuple<string, string, string>("", "59", "2D. Voice amplifier"),
				new ValueTuple<string, string, string>("", "54", "2E. Mediaplayer 3"),
				new ValueTuple<string, string, string>("", "4C", "2F. Digital TV-tuner"),
				new ValueTuple<string, string, string>("71E", "18", "32. Differential lock electronics"),
				new ValueTuple<string, string, string>("755", "04", "34. Ride height control system"),
				new ValueTuple<string, string, string>("74C", "26", "36. Driver seat adjustment"),
				new ValueTuple<string, string, string>("76C", "5B", "37. Navigation system"),
				new ValueTuple<string, string, string>("76C", "27", "38. Roof electronics"),
				new ValueTuple<string, string, string>("", "39", "39. Right side lights control"),
				new ValueTuple<string, string, string>("74E", "1C", "3C. Lane change assistant"),
				new ValueTuple<string, string, string>("72C", "3A", "3D. Special function"),
				new ValueTuple<string, string, string>("", "62", "3E. Mediaplayer 4"),
				new ValueTuple<string, string, string>("74A", "22", "42. Driver door electronic equipment"),
				new ValueTuple<string, string, string>("712", "09", "44. Power steering"),
				new ValueTuple<string, string, string>("", "21", "46. Central comfort systems module"),
				new ValueTuple<string, string, string>("76F", "53", "47. Acoustic system"),
				new ValueTuple<string, string, string>("", "37", "48. Rear seat behind driver"),
				new ValueTuple<string, string, string>("", "5D", "4E. Rear right control and indication unit"),
				new ValueTuple<string, string, string>("", "08", "4C. TPMS #2"),
				new ValueTuple<string, string, string>("", "28", "4F. Body control unit #2"),
				new ValueTuple<string, string, string>("7E6", "", "51. Electric drive"),
				new ValueTuple<string, string, string>("74B", "23", "52. Front passenger door electronics"),
				new ValueTuple<string, string, string>("752", "19", "53. Parking brake"),
				new ValueTuple<string, string, string>("754", "06", "55. Headlights corrector"),
				new ValueTuple<string, string, string>("", "52", "56. Head unit (MMI)"),
				new ValueTuple<string, string, string>("76D", "57", "57. Tv tuner"),
				new ValueTuple<string, string, string>("", "41", "59. Tow protection"),
				new ValueTuple<string, string, string>("", "1A", "5C. Lane assist"),
				new ValueTuple<string, string, string>("", "4B", "5D. Control"),
				new ValueTuple<string, string, string>("", "5E", "5E. Rear left control and indication unit"),
				new ValueTuple<string, string, string>("773", "4D", "5F. Electronic Information System 1"),
				new ValueTuple<string, string, string>("", "33", "61. Battery Regulation using CAN"),
				new ValueTuple<string, string, string>("", "24", "62. Rear left door electoronics"),
				new ValueTuple<string, string, string>("", "46", "63. Easy entry driver side"),
				new ValueTuple<string, string, string>("70B", "29", "65. TPMS"),
				new ValueTuple<string, string, string>("", "36", "66. Rear seats adjustment"),
				new ValueTuple<string, string, string>("", "5C", "67. Voice control"),
				new ValueTuple<string, string, string>("", "32", "68. Wipers electronics"),
				new ValueTuple<string, string, string>("747", "43", "69. Trailer functions"),
				new ValueTuple<string, string, string>("724", "", "6B. Aerodynamic control module"),
				new ValueTuple<string, string, string>("769", "49", "6C. Rear view camera system var.1"),
				new ValueTuple<string, string, string>("6B8", "", "6C. Rear view camera system var.2"),
				new ValueTuple<string, string, string>("", "3D", "6E. Control Head Roof"),
				new ValueTuple<string, string, string>("723", "34", "6D. Tailgate/Boot lid"),
				new ValueTuple<string, string, string>("745", "3C", "6F. Comfort system central module 2"),
				new ValueTuple<string, string, string>("71D", "0E", "71. Charger / 22.Four-wheel drive 2"),
				new ValueTuple<string, string, string>("", "25", "72. Rear right door electronics"),
				new ValueTuple<string, string, string>("", "47", "73. Easy entry passenger side"),
				new ValueTuple<string, string, string>("", "1B", "74. Chassis Control"),
				new ValueTuple<string, string, string>("767", "55", "75. Emergency Call and Communication Module"),
				new ValueTuple<string, string, string>("70A", "2D", "76. Parking assistant"),
				new ValueTuple<string, string, string>("76B", "5A", "77. Telephone"),
				new ValueTuple<string, string, string>("", "25", "78. Slide Door Right"),
				new ValueTuple<string, string, string>("", "2E", "7D. Additional heater"),
				new ValueTuple<string, string, string>("", "48", "7E. Indication panel in the dashboard"),
				new ValueTuple<string, string, string>("75A", "4E", "7F. Electronic Information System 2"),
				new ValueTuple<string, string, string>("753", "", "81. Selector"),
				new ValueTuple<string, string, string>("71B", "", "82. HUD"),
				new ValueTuple<string, string, string>("727", "", "84. Night vision"),
				new ValueTuple<string, string, string>("7E5", "", "8C. Hybrid battery management"),
				new ValueTuple<string, string, string>("6BC", "", "94. SRS/Airbag"),
				new ValueTuple<string, string, string>("784", "", "95. ESP"),
				new ValueTuple<string, string, string>("74F", "", "A5. Front Assistant A5"),
				new ValueTuple<string, string, string>("749", "", "A7. Infotainment Interface"),
				new ValueTuple<string, string, string>("71C", "", "A9. Structure-Borne Sound Actuator / Exhaust flaps"),
				new ValueTuple<string, string, string>("72A", "", "AC. Reducing agent metering system"),
				new ValueTuple<string, string, string>("762", "", "AD. Sensors for brake control unit"),
				new ValueTuple<string, string, string>("72E", "", "BA. Subframe mount"),
				new ValueTuple<string, string, string>("73E", "", "BB. Rear door behind driver"),
				new ValueTuple<string, string, string>("73F", "", "BC. Rear door behind passenger"),
				new ValueTuple<string, string, string>("765", "", "BD. High-voltage battery charging management"),
				new ValueTuple<string, string, string>("764", "", "C0. Actuator for exterior noise"),
				new ValueTuple<string, string, string>("743", "", "C3. Electric drive 3"),
				new ValueTuple<string, string, string>("741", "", "C4. Voltage converter"),
				new ValueTuple<string, string, string>("742", "", "C5. Thermal management"),
				new ValueTuple<string, string, string>("744", "", "C6. High-voltage battery charger"),
				new ValueTuple<string, string, string>("760", "", "CB. Rear axle steering")
			};
			List<IECU> list = new List<IECU>(array.Length + 10);
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "190208", "1800FF00", "1800FFFF" },
				ClearDTCCommands = new List<string> { "04", "14", "14FF00", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				VAGECU vagecu = new VAGECU(valueTuple.Item3, item, item2);
				list.Add(vagecu);
			}
			if (!SharedSettings.Current.ShowExperimental)
			{
				IECU iecu = list.FirstOrDefault((IECU x) => x.RequestHeader == "767");
				if (iecu != null)
				{
					iecu.ClearDTCCommands.Clear();
				}
			}
			IReadOnlyList<IECU> ecus = VAG29bitECU.ECUs;
			list.AddRange(ecus);
			return list;
		}

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x060031D8 RID: 12760 RVA: 0x00228CB7 File Offset: 0x00226EB7
		// (set) Token: 0x060031D9 RID: 12761 RVA: 0x00228CBF File Offset: 0x00226EBF
		public string ASAM
		{
			[CompilerGenerated]
			get
			{
				return this.<ASAM>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ASAM>k__BackingField = value;
			}
		} = "";

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x060031DA RID: 12762 RVA: 0x00228CC8 File Offset: 0x00226EC8
		// (set) Token: 0x060031DB RID: 12763 RVA: 0x00228CD0 File Offset: 0x00226ED0
		public string ASAMVer
		{
			[CompilerGenerated]
			get
			{
				return this.<ASAMVer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ASAMVer>k__BackingField = value;
			}
		} = "";

		// Token: 0x060031DC RID: 12764 RVA: 0x00228CDC File Offset: 0x00226EDC
		public override async Task ReadDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.ECUExists = false;
			this.cancellationToken = token;
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				App.OBDReader.ReplaceQueue(new OBDRequest[] { atshtestRequest, atfctestRequest });
				await App.OBDReader.WaitForCommandQueue();
			}
			if (!this.TestELMDevice || !base.BadELMDetected)
			{
				if (!string.IsNullOrEmpty(base.ExtendedAddress) && (string.IsNullOrEmpty(base.RequestHeader) || !this.ECUExists) && (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.TP20))
				{
					TaskAwaiter<bool> taskAwaiter = this.ReadDTCTP20(otherECUs, token).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.WorkingMode = VAGECU.WorkingModes.TP20;
					}
				}
				if (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.UDS)
				{
					OBDRequest[] dtcreadRequests = this.GetDTCReadRequests();
					App.OBDReader.ReplaceQueue(dtcreadRequests);
					await App.OBDReader.WaitForCommandQueue();
					await this.RequestFreezeFrames();
				}
				this.cancellationToken = CancellationToken.None;
			}
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x00228D38 File Offset: 0x00226F38
		protected override OBDRequest[] GetDTCReadRequests()
		{
			List<OBDRequest> list = base.ReadDTCCommands.Select((string x) => this.GetRequestForCommand(x)).ToList<OBDRequest>();
			OBDRequest[] testECUExistsRequest = this.GetTestECUExistsRequest();
			List<OBDRequest> list2 = new List<OBDRequest>(list.Count + testECUExistsRequest.Length);
			list2.AddRange(testECUExistsRequest);
			OBDRequest requestForCommand = this.GetRequestForCommand("22F19E");
			requestForCommand.ResponseDecoded += this.AsamRequest_ResponseDecoded;
			list2.Add(requestForCommand);
			OBDRequest requestForCommand2 = this.GetRequestForCommand("22F1A2");
			requestForCommand2.ResponseDecoded += this.AsamVerRequest_ResponseDecoded;
			list2.Add(requestForCommand2);
			list2.AddRange(list);
			foreach (OBDRequest obdrequest in list)
			{
				obdrequest.OBDMode = OBDDataReader.OBDModes.ReadDTC;
				obdrequest.CheckLength = true;
				obdrequest.ResponseReceived += base.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
				obdrequest.ResponseDecoded += this.DtcReadRequest_ResponseDecoded;
			}
			if (string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN) || !CarInfoViewModel.Instance.IsVINAvailable || (CarInfoViewModel.Instance.VIN != null && CarInfoViewModel.Instance.VIN.Contains("EEPR0MREADE")))
			{
				PIDWithStringValue vinPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Command == "0902" && x is PIDWithStringValue) as PIDWithStringValue;
				if (vinPid == null)
				{
					vinPid = PIDWithStringValue.PID0902_VIN();
				}
				OBDRequest obdrequest2 = new OBDRequest("0902", false, vinPid);
				obdrequest2.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
				{
					if (decodeResult)
					{
						CarInfoViewModel.Instance.VIN = vinPid.Value;
					}
				};
				FlowControlOverrides flowControlOverrideMode = SharedSettings.Current.FlowControlOverrideMode;
				if (flowControlOverrideMode - FlowControlOverrides.ForceOnFor7Ex <= 1)
				{
					obdrequest2.BeforeCommands = new string[] { "ATFCSH7E0", "ATFCSD300000", "ATFCSM1" };
					obdrequest2.AfterCommands = new string[] { "ATFCSM0" };
				}
				list2.Insert(0, obdrequest2);
			}
			return list2.ToArray();
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x00228F64 File Offset: 0x00227164
		private void AsamRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			this.ASAM = this.DecodeAsASCII(request.Command, data, true);
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x00228F7A File Offset: 0x0022717A
		private void AsamVerRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			this.ASAMVer = this.DecodeAsASCII(request.Command, data, true);
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x00228F90 File Offset: 0x00227190
		public override OBDRequest GetRequestForCommand(string cmd)
		{
			OBDRequest requestForCommand = base.GetRequestForCommand(cmd);
			if (base.RequestHeader == "713")
			{
				List<string> list = new List<string>(requestForCommand.BeforeCommands);
				List<string> list2 = new List<string>(requestForCommand.AfterCommands);
				list.Add("ATSTFF");
				list2.Add("ATST" + SharedSettings.Current.GetATST());
				requestForCommand.BeforeCommands = list.ToArray();
				requestForCommand.AfterCommands = list2.ToArray();
				if (!requestForCommand.Command.StartsWith("14"))
				{
					requestForCommand.ForceManualFlowControl = true;
				}
			}
			else if (base.RequestHeader == "773" && cmd == "223C0A")
			{
				List<string> list3 = new List<string>(requestForCommand.BeforeCommands);
				List<string> list4 = new List<string>(requestForCommand.AfterCommands);
				list3.Add("ATSTFF");
				list4.Add("ATST" + SharedSettings.Current.GetATST());
				requestForCommand.BeforeCommands = list3.ToArray();
				requestForCommand.AfterCommands = list4.ToArray();
				requestForCommand.ForceManualFlowControl = false;
			}
			else if (!requestForCommand.BeforeCommands.Any((string x) => x.StartsWith("ATST")))
			{
				requestForCommand.BeforeCommands = new List<string>(requestForCommand.BeforeCommands) { "ATST" + SharedSettings.Current.GetATST() }.ToArray();
			}
			return requestForCommand;
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x0022910F File Offset: 0x0022730F
		private OBDRequest GetVWTPRequestForCommand(string cmd)
		{
			return new OBDRequest("VWTP:" + base.ExtendedAddress + ":" + cmd, "000", "ATCAF0;ATV1;ATSP6;ATCM000", "ATSPDEF;ATCAF1;ATV0", false)
			{
				ELMFormat = ELMFormat.VwTp20
			};
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x00229143 File Offset: 0x00227343
		public override void Reset()
		{
			VAGECU.HasVWTP20Unit19 = true;
			VAGECU.HasVWTP20Unit09 = true;
			this.ResetWorkingMode();
			base.Reset();
		}

		// Token: 0x060031E3 RID: 12771 RVA: 0x00229160 File Offset: 0x00227360
		public override async Task ClearDTCAsync(IEnumerable<IECU> otherECUs, Action badELMDetectedCallback, CancellationToken token)
		{
			this.ECUExists = false;
			this.cancellationToken = token;
			if (this.TestELMDevice)
			{
				OBDRequest atshtestRequest = this.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
				OBDRequest atfctestRequest = this.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
				App.OBDReader.ReplaceQueue(new OBDRequest[] { atshtestRequest, atfctestRequest });
				await App.OBDReader.WaitForCommandQueue();
			}
			if (!string.IsNullOrEmpty(base.ExtendedAddress) && (string.IsNullOrEmpty(base.RequestHeader) || !this.ECUExists) && (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.TP20))
			{
				TaskAwaiter<bool> taskAwaiter = this.ClearDTCVWTP20Async(otherECUs, token).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					this.WorkingMode = VAGECU.WorkingModes.TP20;
				}
			}
			if (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.UDS)
			{
				List<OBDRequest> list = this.GetDTCClearRequests().ToList<OBDRequest>();
				list.Add(this.GetRequestForCommand("1001"));
				OBDRequest obdrequest = list.FirstOrDefault<OBDRequest>();
				if (obdrequest != null)
				{
					obdrequest.ResponseReceived += this.UDS_Test_request_ResponseReceived;
				}
				if (base.RequestHeader == "7E0" || base.RequestHeader == "7E1")
				{
					list.Insert(0, new OBDRequest("04", "700", "", "", false));
					if (SharedSettings.Current.VagSendRebootAfterClear)
					{
						list.Add(this.GetRequestForCommand("1102"));
					}
				}
				App.OBDReader.ReplaceQueue(list);
				await App.OBDReader.WaitForCommandQueue();
			}
			this.cancellationToken = CancellationToken.None;
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x060031E4 RID: 12772 RVA: 0x002291BB File Offset: 0x002273BB
		internal static bool HasVWTP20Support
		{
			get
			{
				return VAGECU.HasVWTP20Unit09 || VAGECU.HasVWTP20Unit19;
			}
		}

		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x060031E5 RID: 12773 RVA: 0x002291CE File Offset: 0x002273CE
		// (set) Token: 0x060031E6 RID: 12774 RVA: 0x002291D6 File Offset: 0x002273D6
		public bool IsDetectedAsExisting
		{
			[CompilerGenerated]
			get
			{
				return this.<IsDetectedAsExisting>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsDetectedAsExisting>k__BackingField = value;
			}
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x002291E0 File Offset: 0x002273E0
		protected override void DtcReadRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (base.RequestHeader == "710" && data != null && data.Length != 0)
			{
				VAGECU.HasVWTP20Unit19 = false;
			}
			if (base.RequestHeader == "70E" && data != null && data.Length != 0)
			{
				VAGECU.HasVWTP20Unit09 = false;
			}
			if (data != null && data.Length != 0)
			{
				this.ECUExists = true;
				List<DTCItemV2> dtcs = DTCDecoder.DecodeData(request, request.Header, data);
				Device.BeginInvokeOnMainThread(delegate
				{
					bool flag = false;
					using (List<DTCItemV2>.Enumerator enumerator = dtcs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtc = enumerator.Current;
							if (!this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
							{
								int num;
								if (int.TryParse(dtc.Code, out num))
								{
									foreach (string text in VagDTCDecoder.GetDescriptions(num, this.ASAM, this.ASAMVer, CarInfoViewModel.Instance.VIN))
									{
										if (!string.IsNullOrEmpty(text))
										{
											BrandAndDescription brandAndDescription = new BrandAndDescription(SharedSettings.Current.SelectedBrand, text);
											dtc.Descriptions.Add(brandAndDescription);
										}
									}
								}
								dtc.ECU = this.Name;
								this.DTCCollection.Add(dtc);
								DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
								if (recorder != null)
								{
									recorder.Record(dtc);
								}
								flag = true;
							}
						}
					}
					if (flag)
					{
						this.UpdateCollection();
					}
				});
			}
		}

		// Token: 0x060031E8 RID: 12776 RVA: 0x00229274 File Offset: 0x00227474
		public async Task RequestFreezeFrames()
		{
			if (base.Count != 0)
			{
				if (this.WorkingMode != VAGECU.WorkingModes.TP20)
				{
					List<OBDRequest> list = new List<OBDRequest>();
					using (IEnumerator<DTCItemV2> enumerator = base.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtc = enumerator.Current;
							if (dtc.RawCode.Length == 6)
							{
								OBDRequest requestForCommand = this.GetRequestForCommand("1906" + dtc.RawCode + "FF");
								requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
								{
									if (data != null && data.Length >= 14)
									{
										DateTime udsdateFromFreezeFrame = VAGECU.GetUDSDateFromFreezeFrame(new byte[]
										{
											data[10],
											data[11],
											data[12],
											data[13]
										});
										if (udsdateFromFreezeFrame > new DateTime(2008, 1, 1) && udsdateFromFreezeFrame < DateTimeNowHelper.NowSafe)
										{
											string freezeframeText2 = string.Format(Translate.GetString("dtc_VAGFreezeFrameUDS"), new object[]
											{
												data[0],
												data[1],
												data[2],
												data[4],
												((int)data[5] * 65536 + (int)data[6] * 256 + (int)data[7]).ToString() + " km",
												VAGECU.GetUDSDateStringFromFreezeFrame(udsdateFromFreezeFrame)
											});
											byte[] array = null;
											if (data.Length > 23 && data[14] == 112)
											{
												if (data.Length > 16)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_010C"),
														": ",
														(((int)data[15] * 256 + (int)data[16]) / 4).ToString(),
														" rpm"
													});
												}
												if (data.Length > 17)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_0104"),
														": ",
														((double)data[17] * 100.0 / 255.0).ToString("0.00"),
														" %"
													});
												}
												if (data.Length > 18)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_010D"),
														": ",
														UnitsHelper.GetValue((double)data[18], UnitsHelper.Units.kmh).ToString(),
														" ",
														UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
													});
												}
												if (data.Length > 19)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_0105"),
														": ",
														UnitsHelper.GetValue((double)(data[19] - 40), UnitsHelper.Units.celicium).ToString(),
														" ",
														UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
													});
												}
												if (data.Length > 20)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_015C"),
														": ",
														UnitsHelper.GetValue((double)(data[20] - 40), UnitsHelper.Units.celicium).ToString(),
														" ",
														UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
													});
												}
												if (data.Length > 21)
												{
													freezeframeText2 = string.Concat(new string[]
													{
														freezeframeText2,
														"\n",
														Translate.GetString("PID_0133"),
														": ",
														UnitsHelper.GetValue((double)data[21], UnitsHelper.Units.kPa).ToString(),
														" ",
														UnitsHelper.GetCaption(UnitsHelper.Units.kPa)
													});
												}
												if (data.Length > 23)
												{
													freezeframeText2 = freezeframeText2 + "\nKlemme 30 voltage: " + (((int)data[22] * 256 + (int)data[23]) / 1000).ToString();
												}
												if (data.Length > 24 && data[24] == 113)
												{
													array = data.Skip(25).ToArray<byte>();
												}
											}
											if (array == null && data.Length > 14 && data[14] == 113)
											{
												array = data.Skip(15).ToArray<byte>();
											}
											if (array != null && array.Length != 0)
											{
												StringBuilder stringBuilder = new StringBuilder(array.Length * 2 + 1);
												stringBuilder.Append("Paramteric values: ");
												foreach (byte b in array)
												{
													stringBuilder.Append(' ');
													stringBuilder.Append(b.ToString("X2"));
												}
												freezeframeText2 = freezeframeText2 + "\n" + stringBuilder.ToString();
											}
											if (!MainThread.IsMainThread)
											{
												Device.BeginInvokeOnMainThread(delegate
												{
													dtc.Payload = freezeframeText2;
													DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
													if (recorder3 == null)
													{
														return;
													}
													recorder3.Record(dtc);
												});
												return;
											}
											dtc.Payload = freezeframeText2;
											DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
											if (recorder == null)
											{
												return;
											}
											recorder.Record(dtc);
											return;
										}
										else
										{
											StringBuilder stringBuilder2 = new StringBuilder(data.Length * 2 + 1);
											stringBuilder2.Append("Freeze frame data: ");
											foreach (byte b2 in data)
											{
												stringBuilder2.Append(' ');
												stringBuilder2.Append(b2.ToString("X2"));
											}
											string freezeframeText = stringBuilder2.ToString();
											if (MainThread.IsMainThread)
											{
												dtc.Payload = freezeframeText;
												DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
												if (recorder2 == null)
												{
													return;
												}
												recorder2.Record(dtc);
												return;
											}
											else
											{
												Device.BeginInvokeOnMainThread(delegate
												{
													dtc.Payload = freezeframeText;
													DataRecorderV2 recorder4 = App.OBDReader.CurrentCarData.Recorder;
													if (recorder4 == null)
													{
														return;
													}
													recorder4.Record(dtc);
												});
											}
										}
									}
								};
								requestForCommand.ResponseReceived += base.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
								list.Add(requestForCommand);
							}
						}
					}
					if (list.Count != 0)
					{
						App.OBDReader.ReplaceQueue(list);
						await App.OBDReader.WaitForCommandQueue();
					}
				}
			}
		}

		// Token: 0x060031E9 RID: 12777 RVA: 0x002292B8 File Offset: 0x002274B8
		private static string GetUDSDateStringFromFreezeFrame(DateTime dt)
		{
			return string.Concat(new string[]
			{
				dt.Year.ToString("0000"),
				".",
				dt.Month.ToString("00"),
				".",
				dt.Day.ToString("00"),
				" ",
				dt.Hour.ToString("00"),
				":",
				dt.Minute.ToString("00"),
				":",
				dt.Second.ToString("00")
			});
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x00229388 File Offset: 0x00227588
		private static DateTime GetUDSDateFromFreezeFrame(byte[] data)
		{
			int num = (int)(data[3] & 63);
			int num2 = (int)data[2] * 256 + (int)data[3];
			num2 >>= 6;
			num2 &= 63;
			int num3 = (int)data[1] * 256 + (int)data[2];
			num3 >>= 4;
			num3 &= 31;
			int num4 = data[1] >> 1;
			num4 &= 31;
			int num5 = (int)data[0] * 256 + (int)data[1];
			num5 >>= 6;
			num5 &= 15;
			int num6 = data[0] >> 2;
			num6 += 2000;
			return new DateTime(num6, num5, num4, num3, num2, num);
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x00229410 File Offset: 0x00227610
		private void UDS_Test_request_ResponseReceived(OBDRequest request, string data)
		{
			if (data == null)
			{
				return;
			}
			if (data.Contains("NO DATA") || data.Contains("ERROR"))
			{
				return;
			}
			string text = OBDDataReader.FilterHexAndNewLineOnly(data);
			if (!string.IsNullOrEmpty(base.ResponseHeader) && data.Contains(base.ResponseHeader))
			{
				this.ECUExists = true;
				this.WorkingMode = VAGECU.WorkingModes.UDS;
				return;
			}
			if (text.Contains("7F" + request.Command.Substring(0, 2)))
			{
				this.ECUExists = true;
				this.WorkingMode = VAGECU.WorkingModes.UDS;
				return;
			}
			try
			{
				int num = int.Parse(request.Command.Substring(0, 2), NumberStyles.HexNumber);
				string text2 = (num + 64).ToString("X2");
				if (text.Contains(text2))
				{
					this.ECUExists = true;
					this.WorkingMode = VAGECU.WorkingModes.UDS;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x002294F4 File Offset: 0x002276F4
		protected async Task<bool> ReadDTCTP20(IEnumerable<IECU> otherECUs, CancellationToken token)
		{
			bool result = false;
			bool flag;
			if (string.IsNullOrEmpty(base.ExtendedAddress) || this.vwTp20Worker == null)
			{
				flag = result;
			}
			else if (!VAGECU.HasVWTP20Support)
			{
				flag = result;
			}
			else
			{
				bool pingSettingBefore = SharedSettings.Current.AlwaysPingECU;
				SharedSettings.Current.AlwaysPingECU = false;
				try
				{
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter2;
					if (SharedSettings.Current.VWTP20OpenSessionForDTCOperations)
					{
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter = this.vwTp20Worker.SendAndReadDataUsingRequestLoop("1089", true, false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
						}
						if (!taskAwaiter.GetResult().Item1)
						{
							return result;
						}
						this.ECUExists = true;
						result = true;
					}
					if (token.IsCancellationRequested)
					{
						return result;
					}
					if (this.ECUExists || !SharedSettings.Current.VWTP20OpenSessionForDTCOperations)
					{
						this.WorkingMode = VAGECU.WorkingModes.TP20;
						ValueTuple<bool, string> valueTuple = await this.vwTp20Worker.SendAndReadDataUsingRequestLoop("1802FF00", true, true);
						bool item = valueTuple.Item1;
						string item2 = valueTuple.Item2;
						if (!SharedSettings.Current.VWTP20OpenSessionForDTCOperations && !item)
						{
							if (base.ExtendedAddress == "1F")
							{
								foreach (IECU iecu in otherECUs)
								{
									if (iecu is VAGECU)
									{
										VAGECU vagecu = (VAGECU)iecu;
										if (vagecu.WorkingMode == VAGECU.WorkingModes.TP20)
										{
											vagecu.IsSelected = false;
										}
										else if (vagecu.WorkingMode == VAGECU.WorkingModes.Both)
										{
											vagecu.WorkingMode = VAGECU.WorkingModes.UDS;
										}
										if (string.IsNullOrEmpty(vagecu.RequestHeader))
										{
											vagecu.IsSelected = false;
										}
									}
								}
							}
							return result;
						}
						if (!SharedSettings.Current.VWTP20OpenSessionForDTCOperations && item)
						{
							this.ECUExists = true;
							result = true;
						}
						if (!string.IsNullOrEmpty(item2) && !item2.StartsWith("7F18"))
						{
							byte[] array = BitHelpers.ConvertHexToBytesX(item2);
							List<DTCItemV2> dtcs2 = DTCDecoder.DecodeData(this.GetRequestForCommand("1802FF00"), base.RequestHeader, array);
							Device.BeginInvokeOnMainThread(delegate
							{
								bool flag2 = false;
								using (List<DTCItemV2>.Enumerator enumerator2 = dtcs2.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										DTCItemV2 dtc = enumerator2.Current;
										dtc.Code = DTCItemV2.GetVAGCode(dtc.RawCode);
										if (!this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
										{
											dtc.LoadDescription();
											dtc.ECU = this.Name;
											this.DTCCollection.Add(dtc);
											DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
											if (recorder != null)
											{
												recorder.Record(dtc);
											}
											flag2 = true;
										}
									}
								}
								if (flag2)
								{
									this.UpdateCollection();
								}
							});
						}
						if (token.IsCancellationRequested)
						{
							return result;
						}
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter = this.vwTp20Worker.SendAndReadDataUsingRequestLoop("1800FF00", true, true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
						}
						string item3 = taskAwaiter.GetResult().Item2;
						if (!string.IsNullOrEmpty(item3) && !item3.StartsWith("7F18"))
						{
							byte[] array2 = BitHelpers.ConvertHexToBytesX(item3);
							List<DTCItemV2> dtcs = DTCDecoder.DecodeData(this.GetRequestForCommand("1800FF00"), base.RequestHeader, array2);
							Device.BeginInvokeOnMainThread(delegate
							{
								bool flag3 = false;
								using (List<DTCItemV2>.Enumerator enumerator3 = dtcs.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										DTCItemV2 dtc = enumerator3.Current;
										dtc.Code = DTCItemV2.GetVAGCode(dtc.RawCode);
										if (!this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
										{
											dtc.LoadDescription();
											dtc.ECU = this.Name;
											this.DTCCollection.Add(dtc);
											DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
											if (recorder2 != null)
											{
												recorder2.Record(dtc);
											}
											flag3 = true;
										}
									}
								}
								if (flag3)
								{
									this.UpdateCollection();
								}
							});
						}
						if (token.IsCancellationRequested)
						{
							return result;
						}
						await this.vwTp20Worker.CloseChannel(true);
					}
				}
				finally
				{
					SharedSettings.Current.AlwaysPingECU = pingSettingBefore;
				}
				flag = result;
			}
			return flag;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x00229548 File Offset: 0x00227748
		protected async Task<bool> ClearDTCVWTP20Async(IEnumerable<IECU> otherECUs, CancellationToken token)
		{
			bool result = false;
			bool flag;
			if (string.IsNullOrEmpty(base.ExtendedAddress) || this.vwTp20Worker == null)
			{
				flag = result;
			}
			else if (!VAGECU.HasVWTP20Support)
			{
				flag = result;
			}
			else
			{
				bool pingSettingBefore = SharedSettings.Current.AlwaysPingECU;
				SharedSettings.Current.AlwaysPingECU = false;
				try
				{
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter = this.vwTp20Worker.SendAndReadDataUsingRequestLoop("1089", true, false).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
					}
					if (!taskAwaiter.GetResult().Item1)
					{
						return result;
					}
					result = true;
					this.ECUExists = true;
					this.WorkingMode = VAGECU.WorkingModes.TP20;
					if (token.IsCancellationRequested)
					{
						return result;
					}
					if (this.ECUExists)
					{
						await this.vwTp20Worker.SendAndReadDataUsingRequestLoop("14FF00", true, false);
					}
					if (token.IsCancellationRequested)
					{
						return result;
					}
					await this.vwTp20Worker.CloseChannel(true);
				}
				finally
				{
					SharedSettings.Current.AlwaysPingECU = pingSettingBefore;
				}
				flag = result;
			}
			return flag;
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x00229593 File Offset: 0x00227793
		public static string BoolToIntString(bool value)
		{
			if (value)
			{
				return "1";
			}
			return "0";
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x002295A4 File Offset: 0x002277A4
		public override async Task<string> GetECUInformationReportAsync(CancellationToken token)
		{
			if (!string.IsNullOrEmpty(base.ExtendedAddress) && (string.IsNullOrEmpty(base.RequestHeader) || !this.ECUExists) && (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.TP20))
			{
				string text = await this.GetECUInformationReportAsync_VWTP20(token);
				if (!string.IsNullOrEmpty(text))
				{
					this.WorkingMode = VAGECU.WorkingModes.TP20;
					this.cancellationToken = CancellationToken.None;
					return text;
				}
			}
			string text3;
			if (this.WorkingMode == VAGECU.WorkingModes.Both || this.WorkingMode == VAGECU.WorkingModes.UDS)
			{
				string text2 = await this.GetECUInformationReportAsync_UDS(token);
				this.cancellationToken = CancellationToken.None;
				text3 = text2;
			}
			else
			{
				text3 = "";
			}
			return text3;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x002295F0 File Offset: 0x002277F0
		private async Task<string> GetECUInformationReportAsync_VWTP20(CancellationToken token)
		{
			base.IdentsASCII.TryAdd("1A9B", "Part number");
			base.IdentsASCII.TryAdd("1A9A", "Version");
			base.IdentsASCII.TryAdd("1A91", Translate.GetString("PID_1A91"));
			this.cancellationToken = token;
			StringBuilder sb = new StringBuilder(8);
			List<OBDRequest> requests = new List<OBDRequest>(base.IdentsASCII.Count + base.IdentsHEX.Count + 1);
			OBDRequest vwtprequestForCommand = this.GetVWTPRequestForCommand("1089");
			requests.Add(vwtprequestForCommand);
			foreach (KeyValuePair<string, string> keyValuePair in base.IdentsASCII)
			{
				string key = keyValuePair.Key;
				string requestTitle2 = keyValuePair.Value;
				OBDRequest vwtprequestForCommand2 = this.GetVWTPRequestForCommand(key);
				vwtprequestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text2 = this.DecodeAsASCII(decodedRequest.Command, data, true);
					if (!string.IsNullOrEmpty(text2))
					{
						sb.AppendLine(requestTitle2 + ": " + text2);
					}
					this.ECUExists = true;
				};
				requests.Add(vwtprequestForCommand2);
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in base.IdentsHEX)
			{
				string key2 = keyValuePair2.Key;
				string requestTitle = keyValuePair2.Value;
				OBDRequest vwtprequestForCommand3 = this.GetVWTPRequestForCommand(key2);
				vwtprequestForCommand3.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
				{
					string text3 = this.DecodeAsHEX(decodedRequest.Command, data);
					if (!string.IsNullOrEmpty(text3))
					{
						sb.AppendLine(requestTitle + ": " + text3);
					}
					this.ECUExists = true;
				};
				requests.Add(vwtprequestForCommand3);
			}
			Predicate<OBDRequest> <>9__3;
			vwtprequestForCommand.ResponseReceived += delegate(OBDRequest openSessionReq2, string data)
			{
				if (data == null || data == "" || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list = queueCopy;
					Predicate<OBDRequest> predicate;
					if ((predicate = <>9__3) == null)
					{
						predicate = (<>9__3 = (OBDRequest x) => requests.Contains(x));
					}
					list.RemoveAll(predicate);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			};
			App.OBDReader.ReplaceQueue(requests);
			await App.OBDReader.WaitForCommandQueue();
			string text = sb.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				text = "Protocol: VW TP 2.0\n" + text;
			}
			return sb.ToString();
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0022963C File Offset: 0x0022783C
		public async Task<string> GetECUInformationReportAsync_UDS(CancellationToken token)
		{
			string text2;
			if (base.RequestHeader == "710" || base.RequestHeader == "773" || base.RequestHeader == "757")
			{
				string text = await base.GetECUInformationReportAsync(token);
				string report = text;
				if (!string.IsNullOrEmpty(report))
				{
					OBDRequest requestForCommand = this.GetRequestForCommand("223C00");
					requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length != 0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append("SWaP list (Unlock code: available/Cancelled/Individualization valid/Prerequisites met/Configuration normal/Functionality available/Authentification valid):\n");
							for (int i = 0; i < data.Length; i += 5)
							{
								try
								{
									if (i + 5 <= data.Length)
									{
										if (data[i] + data[i + 1] + data[i + 2] + data[i + 3] != 0)
										{
											string text3 = data[i].ToString("X2") + data[i + 1].ToString("X2") + data[i + 2].ToString("X2") + data[i + 3].ToString("X2");
											byte b = data[i + 4];
											bool bit_0_ = BitHelpers.GetBit_0_7(b, 0);
											bool flag = !BitHelpers.GetBit_0_7(b, 1);
											bool bit_0_2 = BitHelpers.GetBit_0_7(b, 2);
											bool bit_0_3 = BitHelpers.GetBit_0_7(b, 3);
											bool bit_0_4 = BitHelpers.GetBit_0_7(b, 4);
											bool bit_0_5 = BitHelpers.GetBit_0_7(b, 5);
											bool bit_0_6 = BitHelpers.GetBit_0_7(b, 6);
											string text4 = string.Concat(new string[]
											{
												text3,
												": ",
												VAGECU.BoolToIntString(bit_0_),
												"/",
												VAGECU.BoolToIntString(flag),
												"/",
												VAGECU.BoolToIntString(bit_0_2),
												"/",
												VAGECU.BoolToIntString(bit_0_3),
												"/",
												VAGECU.BoolToIntString(bit_0_4),
												"/",
												VAGECU.BoolToIntString(bit_0_5),
												"/",
												VAGECU.BoolToIntString(bit_0_6),
												"\n"
											});
											stringBuilder.Append(text4);
										}
									}
								}
								catch (Exception)
								{
								}
							}
							report = report + "\n" + stringBuilder.ToString();
						}
					};
					App.OBDReader.ReplaceQueue(requestForCommand);
					await App.OBDReader.WaitForCommandQueue();
				}
				text2 = report;
			}
			else
			{
				text2 = await base.GetECUInformationReportAsync(token);
			}
			return text2;
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x00229688 File Offset: 0x00227888
		protected override string DecodeAsASCII(string command, byte[] data, bool filterPrintable)
		{
			if (!(command == "22F1B1"))
			{
				return base.DecodeAsASCII(command, data, filterPrintable);
			}
			if (data == null || data.Length == 0 || data.Length < 7)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i += 7)
			{
				string text = data[i].ToString("X2") + data[i + 1].ToString("X2");
				string @string = Encoding.ASCII.GetString(data, i + 2, 4);
				byte b = data[i + 6];
				stringBuilder.Append(string.Concat(new string[]
				{
					text,
					"/",
					@string,
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 0)),
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 1)),
					"/",
					VAGECU.BoolToIntString(BitHelpers.GetBit_0_7(b, 2)),
					"; "
				}));
			}
			string text2 = stringBuilder.ToString();
			if (text2[text2.Length - 1] == '\n')
			{
				text2 = text2.Substring(0, text2.Length - 1);
			}
			return text2;
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x002297C0 File Offset: 0x002279C0
		private string SwapIDToSwapName(string swapid)
		{
			if (base.RequestHeader == "710" && swapid != null)
			{
				int length = swapid.Length;
				if (length == 8)
				{
					switch (swapid[5])
					{
					case '1':
						if (!(swapid == "10000100"))
						{
							if (swapid == "10001100")
							{
								swapid += " (FPA #1)";
							}
						}
						else
						{
							swapid += " (MKE)";
						}
						break;
					case '2':
						if (swapid == "10001200")
						{
							swapid += " (FPA #2)";
						}
						break;
					case '3':
						if (!(swapid == "10000300"))
						{
							if (swapid == "10001300")
							{
								swapid += " (FPA #3)";
							}
						}
						else
						{
							swapid += " (GRA)";
						}
						break;
					case '4':
						if (swapid == "10001400")
						{
							swapid += " (FPA #4)";
						}
						break;
					case '5':
						if (swapid == "10001500")
						{
							swapid += " (FPA #5)";
						}
						break;
					case '6':
						if (swapid == "10001600")
						{
							swapid += " (FPA #6)";
						}
						break;
					case '7':
						if (swapid == "10001700")
						{
							swapid += " (FPA #7)";
						}
						break;
					case '8':
						if (swapid == "10001800")
						{
							swapid += " (FPA #9)";
						}
						break;
					}
				}
			}
			return swapid;
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x0022998C File Offset: 0x00227B8C
		// Note: this type is marked as 'beforefieldinit'.
		static VAGECU()
		{
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x00216C90 File Offset: 0x00214E90
		[CompilerGenerated]
		private OBDRequest <GetDTCReadRequests>b__19_0(string x)
		{
			return this.GetRequestForCommand(x);
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0022999A File Offset: 0x00227B9A
		[CompilerGenerated]
		[DebuggerHidden]
		private Task<string> <>n__0(CancellationToken token)
		{
			return base.GetECUInformationReportAsync(token);
		}

		// Token: 0x04001CC9 RID: 7369
		private VAGECU.WorkingModes WorkingMode;

		// Token: 0x04001CCA RID: 7370
		protected VwTp20Worker vwTp20Worker;

		// Token: 0x04001CCB RID: 7371
		[CompilerGenerated]
		private string <ASAM>k__BackingField;

		// Token: 0x04001CCC RID: 7372
		[CompilerGenerated]
		private string <ASAMVer>k__BackingField;

		// Token: 0x04001CCD RID: 7373
		private static bool HasVWTP20Unit19 = true;

		// Token: 0x04001CCE RID: 7374
		private static bool HasVWTP20Unit09 = true;

		// Token: 0x04001CCF RID: 7375
		[CompilerGenerated]
		private bool <IsDetectedAsExisting>k__BackingField;

		// Token: 0x02000521 RID: 1313
		private enum WorkingModes
		{
			// Token: 0x04001CD1 RID: 7377
			Both,
			// Token: 0x04001CD2 RID: 7378
			UDS,
			// Token: 0x04001CD3 RID: 7379
			TP20
		}

		// Token: 0x02000522 RID: 1314
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060031F7 RID: 12791 RVA: 0x002299A3 File Offset: 0x00227BA3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060031F8 RID: 12792 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060031F9 RID: 12793 RVA: 0x002299AF File Offset: 0x00227BAF
			internal bool <BuildList>b__9_0(IECU x)
			{
				return x.RequestHeader == "767";
			}

			// Token: 0x060031FA RID: 12794 RVA: 0x002299C1 File Offset: 0x00227BC1
			internal bool <GetDTCReadRequests>b__19_1(PID x)
			{
				return x.Command == "0902" && x is PIDWithStringValue;
			}

			// Token: 0x060031FB RID: 12795 RVA: 0x002299E0 File Offset: 0x00227BE0
			internal bool <GetRequestForCommand>b__22_0(string x)
			{
				return x.StartsWith("ATST");
			}

			// Token: 0x04001CD4 RID: 7380
			public static readonly VAGECU.<>c <>9 = new VAGECU.<>c();

			// Token: 0x04001CD5 RID: 7381
			public static Func<IECU, bool> <>9__9_0;

			// Token: 0x04001CD6 RID: 7382
			public static Func<PID, bool> <>9__19_1;

			// Token: 0x04001CD7 RID: 7383
			public static Func<string, bool> <>9__22_0;
		}

		// Token: 0x02000523 RID: 1315
		[CompilerGenerated]
		private sealed class <>c__DisplayClass19_0
		{
			// Token: 0x060031FC RID: 12796 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass19_0()
			{
			}

			// Token: 0x060031FD RID: 12797 RVA: 0x002299ED File Offset: 0x00227BED
			internal void <GetDTCReadRequests>b__2(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (decodeResult)
				{
					CarInfoViewModel.Instance.VIN = this.vinPid.Value;
				}
			}

			// Token: 0x04001CD8 RID: 7384
			public PIDWithStringValue vinPid;
		}

		// Token: 0x02000524 RID: 1316
		[CompilerGenerated]
		private sealed class <>c__DisplayClass34_0
		{
			// Token: 0x060031FE RID: 12798 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass34_0()
			{
			}

			// Token: 0x060031FF RID: 12799 RVA: 0x00229A08 File Offset: 0x00227C08
			internal void <DtcReadRequest_ResponseDecoded>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						VAGECU.<>c__DisplayClass34_1 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass34_1();
						CS$<>8__locals1.dtc = enumerator.Current;
						if (!this.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							int num;
							if (int.TryParse(CS$<>8__locals1.dtc.Code, out num))
							{
								foreach (string text in VagDTCDecoder.GetDescriptions(num, this.<>4__this.ASAM, this.<>4__this.ASAMVer, CarInfoViewModel.Instance.VIN))
								{
									if (!string.IsNullOrEmpty(text))
									{
										BrandAndDescription brandAndDescription = new BrandAndDescription(SharedSettings.Current.SelectedBrand, text);
										CS$<>8__locals1.dtc.Descriptions.Add(brandAndDescription);
									}
								}
							}
							CS$<>8__locals1.dtc.ECU = this.<>4__this.Name;
							this.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001CD9 RID: 7385
			public VAGECU <>4__this;

			// Token: 0x04001CDA RID: 7386
			public string responseHeader;

			// Token: 0x04001CDB RID: 7387
			public List<DTCItemV2> dtcs;
		}

		// Token: 0x02000525 RID: 1317
		[CompilerGenerated]
		private sealed class <>c__DisplayClass34_1
		{
			// Token: 0x06003200 RID: 12800 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass34_1()
			{
			}

			// Token: 0x06003201 RID: 12801 RVA: 0x00229BA0 File Offset: 0x00227DA0
			internal bool <DtcReadRequest_ResponseDecoded>b__1(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001CDC RID: 7388
			public DTCItemV2 dtc;
		}

		// Token: 0x02000526 RID: 1318
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_0
		{
			// Token: 0x06003202 RID: 12802 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_0()
			{
			}

			// Token: 0x06003203 RID: 12803 RVA: 0x00229BB8 File Offset: 0x00227DB8
			internal void <RequestFreezeFrames>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 14)
				{
					DateTime udsdateFromFreezeFrame = VAGECU.GetUDSDateFromFreezeFrame(new byte[]
					{
						data[10],
						data[11],
						data[12],
						data[13]
					});
					if (udsdateFromFreezeFrame > new DateTime(2008, 1, 1) && udsdateFromFreezeFrame < DateTimeNowHelper.NowSafe)
					{
						VAGECU.<>c__DisplayClass35_1 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass35_1();
						CS$<>8__locals1.CS$<>8__locals1 = this;
						CS$<>8__locals1.freezeframeText = string.Format(Translate.GetString("dtc_VAGFreezeFrameUDS"), new object[]
						{
							data[0],
							data[1],
							data[2],
							data[4],
							((int)data[5] * 65536 + (int)data[6] * 256 + (int)data[7]).ToString() + " km",
							VAGECU.GetUDSDateStringFromFreezeFrame(udsdateFromFreezeFrame)
						});
						byte[] array = null;
						if (data.Length > 23 && data[14] == 112)
						{
							if (data.Length > 16)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_010C"),
									": ",
									(((int)data[15] * 256 + (int)data[16]) / 4).ToString(),
									" rpm"
								});
							}
							if (data.Length > 17)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_0104"),
									": ",
									((double)data[17] * 100.0 / 255.0).ToString("0.00"),
									" %"
								});
							}
							if (data.Length > 18)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_010D"),
									": ",
									UnitsHelper.GetValue((double)data[18], UnitsHelper.Units.kmh).ToString(),
									" ",
									UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
								});
							}
							if (data.Length > 19)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_0105"),
									": ",
									UnitsHelper.GetValue((double)(data[19] - 40), UnitsHelper.Units.celicium).ToString(),
									" ",
									UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
								});
							}
							if (data.Length > 20)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_015C"),
									": ",
									UnitsHelper.GetValue((double)(data[20] - 40), UnitsHelper.Units.celicium).ToString(),
									" ",
									UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
								});
							}
							if (data.Length > 21)
							{
								CS$<>8__locals1.freezeframeText = string.Concat(new string[]
								{
									CS$<>8__locals1.freezeframeText,
									"\n",
									Translate.GetString("PID_0133"),
									": ",
									UnitsHelper.GetValue((double)data[21], UnitsHelper.Units.kPa).ToString(),
									" ",
									UnitsHelper.GetCaption(UnitsHelper.Units.kPa)
								});
							}
							if (data.Length > 23)
							{
								CS$<>8__locals1.freezeframeText = CS$<>8__locals1.freezeframeText + "\nKlemme 30 voltage: " + (((int)data[22] * 256 + (int)data[23]) / 1000).ToString();
							}
							if (data.Length > 24 && data[24] == 113)
							{
								array = data.Skip(25).ToArray<byte>();
							}
						}
						if (array == null && data.Length > 14 && data[14] == 113)
						{
							array = data.Skip(15).ToArray<byte>();
						}
						if (array != null && array.Length != 0)
						{
							StringBuilder stringBuilder = new StringBuilder(array.Length * 2 + 1);
							stringBuilder.Append("Paramteric values: ");
							foreach (byte b in array)
							{
								stringBuilder.Append(' ');
								stringBuilder.Append(b.ToString("X2"));
							}
							CS$<>8__locals1.freezeframeText = CS$<>8__locals1.freezeframeText + "\n" + stringBuilder.ToString();
						}
						if (!MainThread.IsMainThread)
						{
							Device.BeginInvokeOnMainThread(delegate
							{
								CS$<>8__locals1.CS$<>8__locals1.dtc.Payload = CS$<>8__locals1.freezeframeText;
								DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
								if (recorder3 == null)
								{
									return;
								}
								recorder3.Record(CS$<>8__locals1.CS$<>8__locals1.dtc);
							});
							return;
						}
						this.dtc.Payload = CS$<>8__locals1.freezeframeText;
						DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
						if (recorder == null)
						{
							return;
						}
						recorder.Record(this.dtc);
						return;
					}
					else
					{
						VAGECU.<>c__DisplayClass35_2 CS$<>8__locals2 = new VAGECU.<>c__DisplayClass35_2();
						CS$<>8__locals2.CS$<>8__locals2 = this;
						StringBuilder stringBuilder2 = new StringBuilder(data.Length * 2 + 1);
						stringBuilder2.Append("Freeze frame data: ");
						foreach (byte b2 in data)
						{
							stringBuilder2.Append(' ');
							stringBuilder2.Append(b2.ToString("X2"));
						}
						CS$<>8__locals2.freezeframeText = stringBuilder2.ToString();
						if (MainThread.IsMainThread)
						{
							this.dtc.Payload = CS$<>8__locals2.freezeframeText;
							DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
							if (recorder2 == null)
							{
								return;
							}
							recorder2.Record(this.dtc);
							return;
						}
						else
						{
							Device.BeginInvokeOnMainThread(delegate
							{
								CS$<>8__locals2.CS$<>8__locals2.dtc.Payload = CS$<>8__locals2.freezeframeText;
								DataRecorderV2 recorder4 = App.OBDReader.CurrentCarData.Recorder;
								if (recorder4 == null)
								{
									return;
								}
								recorder4.Record(CS$<>8__locals2.CS$<>8__locals2.dtc);
							});
						}
					}
				}
			}

			// Token: 0x04001CDD RID: 7389
			public DTCItemV2 dtc;
		}

		// Token: 0x02000527 RID: 1319
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_1
		{
			// Token: 0x06003204 RID: 12804 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_1()
			{
			}

			// Token: 0x06003205 RID: 12805 RVA: 0x0022A127 File Offset: 0x00228327
			internal void <RequestFreezeFrames>b__1()
			{
				this.CS$<>8__locals1.dtc.Payload = this.freezeframeText;
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(this.CS$<>8__locals1.dtc);
			}

			// Token: 0x04001CDE RID: 7390
			public string freezeframeText;

			// Token: 0x04001CDF RID: 7391
			public VAGECU.<>c__DisplayClass35_0 CS$<>8__locals1;
		}

		// Token: 0x02000528 RID: 1320
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_2
		{
			// Token: 0x06003206 RID: 12806 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_2()
			{
			}

			// Token: 0x06003207 RID: 12807 RVA: 0x0022A163 File Offset: 0x00228363
			internal void <RequestFreezeFrames>b__2()
			{
				this.CS$<>8__locals2.dtc.Payload = this.freezeframeText;
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(this.CS$<>8__locals2.dtc);
			}

			// Token: 0x04001CE0 RID: 7392
			public string freezeframeText;

			// Token: 0x04001CE1 RID: 7393
			public VAGECU.<>c__DisplayClass35_0 CS$<>8__locals2;
		}

		// Token: 0x02000529 RID: 1321
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_0
		{
			// Token: 0x06003208 RID: 12808 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_0()
			{
			}

			// Token: 0x06003209 RID: 12809 RVA: 0x0022A1A0 File Offset: 0x002283A0
			internal void <ReadDTCTP20>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						VAGECU.<>c__DisplayClass39_1 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass39_1();
						CS$<>8__locals1.dtc = enumerator.Current;
						CS$<>8__locals1.dtc.Code = DTCItemV2.GetVAGCode(CS$<>8__locals1.dtc.RawCode);
						if (!this.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							CS$<>8__locals1.dtc.ECU = this.<>4__this.Name;
							this.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001CE2 RID: 7394
			public List<DTCItemV2> dtcs;

			// Token: 0x04001CE3 RID: 7395
			public VAGECU <>4__this;
		}

		// Token: 0x0200052A RID: 1322
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_1
		{
			// Token: 0x0600320A RID: 12810 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_1()
			{
			}

			// Token: 0x0600320B RID: 12811 RVA: 0x0022A2A4 File Offset: 0x002284A4
			internal bool <ReadDTCTP20>b__1(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001CE4 RID: 7396
			public DTCItemV2 dtc;
		}

		// Token: 0x0200052B RID: 1323
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_2
		{
			// Token: 0x0600320C RID: 12812 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_2()
			{
			}

			// Token: 0x0600320D RID: 12813 RVA: 0x0022A2BC File Offset: 0x002284BC
			internal void <ReadDTCTP20>b__2()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						VAGECU.<>c__DisplayClass39_3 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass39_3();
						CS$<>8__locals1.dtc = enumerator.Current;
						CS$<>8__locals1.dtc.Code = DTCItemV2.GetVAGCode(CS$<>8__locals1.dtc.RawCode);
						if (!this.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							CS$<>8__locals1.dtc.ECU = this.<>4__this.Name;
							this.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001CE5 RID: 7397
			public List<DTCItemV2> dtcs;

			// Token: 0x04001CE6 RID: 7398
			public VAGECU <>4__this;
		}

		// Token: 0x0200052C RID: 1324
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_3
		{
			// Token: 0x0600320E RID: 12814 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_3()
			{
			}

			// Token: 0x0600320F RID: 12815 RVA: 0x0022A3C0 File Offset: 0x002285C0
			internal bool <ReadDTCTP20>b__3(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001CE7 RID: 7399
			public DTCItemV2 dtc;
		}

		// Token: 0x0200052D RID: 1325
		[CompilerGenerated]
		private sealed class <>c__DisplayClass43_0
		{
			// Token: 0x06003210 RID: 12816 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass43_0()
			{
			}

			// Token: 0x06003211 RID: 12817 RVA: 0x0022A3D8 File Offset: 0x002285D8
			internal void <GetECUInformationReportAsync_VWTP20>b__0(OBDRequest openSessionReq2, string data)
			{
				if (data == null || data == "" || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					List<OBDRequest> list = queueCopy;
					Predicate<OBDRequest> predicate;
					if ((predicate = this.<>9__3) == null)
					{
						predicate = (this.<>9__3 = (OBDRequest x) => this.requests.Contains(x));
					}
					list.RemoveAll(predicate);
					App.OBDReader.ReplaceQueue(queueCopy);
				}
			}

			// Token: 0x06003212 RID: 12818 RVA: 0x0022A43E File Offset: 0x0022863E
			internal bool <GetECUInformationReportAsync_VWTP20>b__3(OBDRequest x)
			{
				return this.requests.Contains(x);
			}

			// Token: 0x04001CE8 RID: 7400
			public VAGECU <>4__this;

			// Token: 0x04001CE9 RID: 7401
			public StringBuilder sb;

			// Token: 0x04001CEA RID: 7402
			public List<OBDRequest> requests;

			// Token: 0x04001CEB RID: 7403
			public Predicate<OBDRequest> <>9__3;
		}

		// Token: 0x0200052E RID: 1326
		[CompilerGenerated]
		private sealed class <>c__DisplayClass43_1
		{
			// Token: 0x06003213 RID: 12819 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass43_1()
			{
			}

			// Token: 0x06003214 RID: 12820 RVA: 0x0022A44C File Offset: 0x0022864C
			internal void <GetECUInformationReportAsync_VWTP20>b__1(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals1.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals1.<>4__this.ECUExists = true;
			}

			// Token: 0x04001CEC RID: 7404
			public string requestTitle;

			// Token: 0x04001CED RID: 7405
			public VAGECU.<>c__DisplayClass43_0 CS$<>8__locals1;
		}

		// Token: 0x0200052F RID: 1327
		[CompilerGenerated]
		private sealed class <>c__DisplayClass43_2
		{
			// Token: 0x06003215 RID: 12821 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass43_2()
			{
			}

			// Token: 0x06003216 RID: 12822 RVA: 0x0022A4B0 File Offset: 0x002286B0
			internal void <GetECUInformationReportAsync_VWTP20>b__2(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
			{
				string text = this.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
				if (!string.IsNullOrEmpty(text))
				{
					this.CS$<>8__locals2.sb.AppendLine(this.requestTitle + ": " + text);
				}
				this.CS$<>8__locals2.<>4__this.ECUExists = true;
			}

			// Token: 0x04001CEE RID: 7406
			public string requestTitle;

			// Token: 0x04001CEF RID: 7407
			public VAGECU.<>c__DisplayClass43_0 CS$<>8__locals2;
		}

		// Token: 0x02000530 RID: 1328
		[CompilerGenerated]
		private sealed class <>c__DisplayClass44_0
		{
			// Token: 0x06003217 RID: 12823 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass44_0()
			{
			}

			// Token: 0x06003218 RID: 12824 RVA: 0x0022A510 File Offset: 0x00228710
			internal void <GetECUInformationReportAsync_UDS>b__0(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("SWaP list (Unlock code: available/Cancelled/Individualization valid/Prerequisites met/Configuration normal/Functionality available/Authentification valid):\n");
					for (int i = 0; i < data.Length; i += 5)
					{
						try
						{
							if (i + 5 <= data.Length)
							{
								if (data[i] + data[i + 1] + data[i + 2] + data[i + 3] != 0)
								{
									string text = data[i].ToString("X2") + data[i + 1].ToString("X2") + data[i + 2].ToString("X2") + data[i + 3].ToString("X2");
									byte b = data[i + 4];
									bool bit_0_ = BitHelpers.GetBit_0_7(b, 0);
									bool flag = !BitHelpers.GetBit_0_7(b, 1);
									bool bit_0_2 = BitHelpers.GetBit_0_7(b, 2);
									bool bit_0_3 = BitHelpers.GetBit_0_7(b, 3);
									bool bit_0_4 = BitHelpers.GetBit_0_7(b, 4);
									bool bit_0_5 = BitHelpers.GetBit_0_7(b, 5);
									bool bit_0_6 = BitHelpers.GetBit_0_7(b, 6);
									string text2 = string.Concat(new string[]
									{
										text,
										": ",
										VAGECU.BoolToIntString(bit_0_),
										"/",
										VAGECU.BoolToIntString(flag),
										"/",
										VAGECU.BoolToIntString(bit_0_2),
										"/",
										VAGECU.BoolToIntString(bit_0_3),
										"/",
										VAGECU.BoolToIntString(bit_0_4),
										"/",
										VAGECU.BoolToIntString(bit_0_5),
										"/",
										VAGECU.BoolToIntString(bit_0_6),
										"\n"
									});
									stringBuilder.Append(text2);
								}
							}
						}
						catch (Exception)
						{
						}
					}
					this.report = this.report + "\n" + stringBuilder.ToString();
				}
			}

			// Token: 0x04001CF0 RID: 7408
			public string report;
		}

		// Token: 0x02000531 RID: 1329
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCAsync>d__25 : IAsyncStateMachine
		{
			// Token: 0x06003219 RID: 12825 RVA: 0x0022A6F4 File Offset: 0x002288F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_017A;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02AD;
					}
					default:
					{
						vagecu.ECUExists = false;
						vagecu.cancellationToken = token;
						if (!vagecu.TestELMDevice)
						{
							goto IL_00DE;
						}
						OBDRequest atshtestRequest = vagecu.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
						OBDRequest atfctestRequest = vagecu.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { atshtestRequest, atfctestRequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ClearDTCAsync>d__25>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					IL_00DE:
					if (string.IsNullOrEmpty(vagecu.ExtendedAddress) || (!string.IsNullOrEmpty(vagecu.RequestHeader) && vagecu.ECUExists) || (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.TP20))
					{
						goto IL_018A;
					}
					taskAwaiter5 = vagecu.ClearDTCVWTP20Async(otherECUs, token).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VAGECU.<ClearDTCAsync>d__25>(ref taskAwaiter5, ref this);
						return;
					}
					IL_017A:
					if (taskAwaiter5.GetResult())
					{
						vagecu.WorkingMode = VAGECU.WorkingModes.TP20;
					}
					IL_018A:
					if (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.UDS)
					{
						goto IL_02B4;
					}
					List<OBDRequest> list = vagecu.GetDTCClearRequests().ToList<OBDRequest>();
					OBDRequest requestForCommand = vagecu.GetRequestForCommand("1001");
					list.Add(requestForCommand);
					OBDRequest obdrequest = list.FirstOrDefault<OBDRequest>();
					if (obdrequest != null)
					{
						obdrequest.ResponseReceived += vagecu.UDS_Test_request_ResponseReceived;
					}
					if (vagecu.RequestHeader == "7E0" || vagecu.RequestHeader == "7E1")
					{
						OBDRequest obdrequest2 = new OBDRequest("04", "700", "", "", false);
						list.Insert(0, obdrequest2);
						if (SharedSettings.Current.VagSendRebootAfterClear)
						{
							list.Add(vagecu.GetRequestForCommand("1102"));
						}
					}
					App.OBDReader.ReplaceQueue(list);
					taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ClearDTCAsync>d__25>(ref taskAwaiter3, ref this);
						return;
					}
					IL_02AD:
					taskAwaiter3.GetResult();
					IL_02B4:
					vagecu.cancellationToken = CancellationToken.None;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600321A RID: 12826 RVA: 0x0022AA0C File Offset: 0x00228C0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CF1 RID: 7409
			public int <>1__state;

			// Token: 0x04001CF2 RID: 7410
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001CF3 RID: 7411
			public VAGECU <>4__this;

			// Token: 0x04001CF4 RID: 7412
			public CancellationToken token;

			// Token: 0x04001CF5 RID: 7413
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001CF6 RID: 7414
			public Action badELMDetectedCallback;

			// Token: 0x04001CF7 RID: 7415
			private TaskAwaiter <>u__1;

			// Token: 0x04001CF8 RID: 7416
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000532 RID: 1330
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearDTCVWTP20Async>d__40 : IAsyncStateMachine
		{
			// Token: 0x0600321B RID: 12827 RVA: 0x0022AA1C File Offset: 0x00228C1C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				bool flag;
				try
				{
					if (num > 2)
					{
						result = false;
						if (string.IsNullOrEmpty(vagecu.ExtendedAddress) || vagecu.vwTp20Worker == null)
						{
							flag = result;
							goto IL_0250;
						}
						if (!VAGECU.HasVWTP20Support)
						{
							flag = result;
							goto IL_0250;
						}
						pingSettingBefore = SharedSettings.Current.AlwaysPingECU;
						SharedSettings.Current.AlwaysPingECU = false;
					}
					try
					{
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter3;
						TaskAwaiter taskAwaiter4;
						switch (num)
						{
						case 0:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
							num = (num2 = -1);
							break;
						case 1:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
							num = (num2 = -1);
							goto IL_0195;
						case 2:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0210;
						}
						default:
							taskAwaiter3 = vagecu.vwTp20Worker.SendAndReadDataUsingRequestLoop("1089", true, false).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VAGECU.<ClearDTCVWTP20Async>d__40>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						if (!taskAwaiter3.GetResult().Item1)
						{
							flag = result;
							goto IL_0250;
						}
						result = true;
						vagecu.ECUExists = true;
						vagecu.WorkingMode = VAGECU.WorkingModes.TP20;
						if (token.IsCancellationRequested)
						{
							flag = result;
							goto IL_0250;
						}
						if (!vagecu.ECUExists)
						{
							goto IL_019D;
						}
						taskAwaiter3 = vagecu.vwTp20Worker.SendAndReadDataUsingRequestLoop("14FF00", true, false).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VAGECU.<ClearDTCVWTP20Async>d__40>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0195:
						taskAwaiter3.GetResult();
						IL_019D:
						if (token.IsCancellationRequested)
						{
							flag = result;
							goto IL_0250;
						}
						taskAwaiter4 = vagecu.vwTp20Worker.CloseChannel(true).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ClearDTCVWTP20Async>d__40>(ref taskAwaiter4, ref this);
							return;
						}
						IL_0210:
						taskAwaiter4.GetResult();
					}
					finally
					{
						if (num < 0)
						{
							SharedSettings.Current.AlwaysPingECU = pingSettingBefore;
						}
					}
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0250:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600321C RID: 12828 RVA: 0x0022ACC4 File Offset: 0x00228EC4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001CF9 RID: 7417
			public int <>1__state;

			// Token: 0x04001CFA RID: 7418
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001CFB RID: 7419
			public VAGECU <>4__this;

			// Token: 0x04001CFC RID: 7420
			public CancellationToken token;

			// Token: 0x04001CFD RID: 7421
			private bool <result>5__2;

			// Token: 0x04001CFE RID: 7422
			private bool <pingSettingBefore>5__3;

			// Token: 0x04001CFF RID: 7423
			private TaskAwaiter<ValueTuple<bool, string>> <>u__1;

			// Token: 0x04001D00 RID: 7424
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000533 RID: 1331
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync>d__42 : IAsyncStateMachine
		{
			// Token: 0x0600321D RID: 12829 RVA: 0x0022ACD4 File Offset: 0x00228ED4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_0148;
						}
						if (string.IsNullOrEmpty(vagecu.ExtendedAddress) || (!string.IsNullOrEmpty(vagecu.RequestHeader) && vagecu.ECUExists) || (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.TP20))
						{
							goto IL_00DD;
						}
						taskAwaiter = vagecu.GetECUInformationReportAsync_VWTP20(token).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VAGECU.<GetECUInformationReportAsync>d__42>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					if (!string.IsNullOrEmpty(result))
					{
						vagecu.WorkingMode = VAGECU.WorkingModes.TP20;
						vagecu.cancellationToken = CancellationToken.None;
						text = result;
						goto IL_017E;
					}
					IL_00DD:
					if (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.UDS)
					{
						text = "";
						goto IL_017E;
					}
					taskAwaiter = vagecu.GetECUInformationReportAsync_UDS(token).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VAGECU.<GetECUInformationReportAsync>d__42>(ref taskAwaiter, ref this);
						return;
					}
					IL_0148:
					string result2 = taskAwaiter.GetResult();
					vagecu.cancellationToken = CancellationToken.None;
					text = result2;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_017E:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600321E RID: 12830 RVA: 0x0022AE90 File Offset: 0x00229090
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D01 RID: 7425
			public int <>1__state;

			// Token: 0x04001D02 RID: 7426
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001D03 RID: 7427
			public VAGECU <>4__this;

			// Token: 0x04001D04 RID: 7428
			public CancellationToken token;

			// Token: 0x04001D05 RID: 7429
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000534 RID: 1332
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync_UDS>d__44 : IAsyncStateMachine
		{
			// Token: 0x0600321F RID: 12831 RVA: 0x0022AEA0 File Offset: 0x002290A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0176;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_01E5;
					}
					default:
						if (vagecu.RequestHeader == "710" || vagecu.RequestHeader == "773" || vagecu.RequestHeader == "757")
						{
							CS$<>8__locals1 = new VAGECU.<>c__DisplayClass44_0();
							taskAwaiter = vagecu.<>n__0(token).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VAGECU.<GetECUInformationReportAsync_UDS>d__44>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = vagecu.<>n__0(token).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VAGECU.<GetECUInformationReportAsync_UDS>d__44>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01E5;
						}
						break;
					}
					string result = taskAwaiter.GetResult();
					CS$<>8__locals1.report = result;
					if (string.IsNullOrEmpty(CS$<>8__locals1.report))
					{
						goto IL_017D;
					}
					OBDRequest requestForCommand = vagecu.GetRequestForCommand("223C00");
					requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length != 0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append("SWaP list (Unlock code: available/Cancelled/Individualization valid/Prerequisites met/Configuration normal/Functionality available/Authentification valid):\n");
							for (int i = 0; i < data.Length; i += 5)
							{
								try
								{
									if (i + 5 <= data.Length)
									{
										if (data[i] + data[i + 1] + data[i + 2] + data[i + 3] != 0)
										{
											string text2 = data[i].ToString("X2") + data[i + 1].ToString("X2") + data[i + 2].ToString("X2") + data[i + 3].ToString("X2");
											byte b = data[i + 4];
											bool bit_0_ = BitHelpers.GetBit_0_7(b, 0);
											bool flag = !BitHelpers.GetBit_0_7(b, 1);
											bool bit_0_2 = BitHelpers.GetBit_0_7(b, 2);
											bool bit_0_3 = BitHelpers.GetBit_0_7(b, 3);
											bool bit_0_4 = BitHelpers.GetBit_0_7(b, 4);
											bool bit_0_5 = BitHelpers.GetBit_0_7(b, 5);
											bool bit_0_6 = BitHelpers.GetBit_0_7(b, 6);
											string text3 = string.Concat(new string[]
											{
												text2,
												": ",
												VAGECU.BoolToIntString(bit_0_),
												"/",
												VAGECU.BoolToIntString(flag),
												"/",
												VAGECU.BoolToIntString(bit_0_2),
												"/",
												VAGECU.BoolToIntString(bit_0_3),
												"/",
												VAGECU.BoolToIntString(bit_0_4),
												"/",
												VAGECU.BoolToIntString(bit_0_5),
												"/",
												VAGECU.BoolToIntString(bit_0_6),
												"\n"
											});
											stringBuilder.Append(text3);
										}
									}
								}
								catch (Exception)
								{
								}
							}
							CS$<>8__locals1.report = CS$<>8__locals1.report + "\n" + stringBuilder.ToString();
						}
					};
					App.OBDReader.ReplaceQueue(requestForCommand);
					taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<GetECUInformationReportAsync_UDS>d__44>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0176:
					taskAwaiter3.GetResult();
					IL_017D:
					text = CS$<>8__locals1.report;
					goto IL_0208;
					IL_01E5:
					text = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0208:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06003220 RID: 12832 RVA: 0x0022B0E8 File Offset: 0x002292E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D06 RID: 7430
			public int <>1__state;

			// Token: 0x04001D07 RID: 7431
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001D08 RID: 7432
			public VAGECU <>4__this;

			// Token: 0x04001D09 RID: 7433
			public CancellationToken token;

			// Token: 0x04001D0A RID: 7434
			private VAGECU.<>c__DisplayClass44_0 <>8__1;

			// Token: 0x04001D0B RID: 7435
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001D0C RID: 7436
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000535 RID: 1333
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetECUInformationReportAsync_VWTP20>d__43 : IAsyncStateMachine
		{
			// Token: 0x06003221 RID: 12833 RVA: 0x0022B0F8 File Offset: 0x002292F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				string text2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new VAGECU.<>c__DisplayClass43_0();
						CS$<>8__locals1.<>4__this = this;
						vagecu.IdentsASCII.TryAdd("1A9B", "Part number");
						vagecu.IdentsASCII.TryAdd("1A9A", "Version");
						vagecu.IdentsASCII.TryAdd("1A91", Translate.GetString("PID_1A91"));
						vagecu.cancellationToken = token;
						CS$<>8__locals1.sb = new StringBuilder(8);
						CS$<>8__locals1.requests = new List<OBDRequest>(vagecu.IdentsASCII.Count + vagecu.IdentsHEX.Count + 1);
						OBDRequest vwtprequestForCommand = vagecu.GetVWTPRequestForCommand("1089");
						CS$<>8__locals1.requests.Add(vwtprequestForCommand);
						Dictionary<string, string>.Enumerator enumerator = vagecu.IdentsASCII.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair = enumerator.Current;
								VAGECU.<>c__DisplayClass43_1 CS$<>8__locals2 = new VAGECU.<>c__DisplayClass43_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								string key = keyValuePair.Key;
								CS$<>8__locals2.requestTitle = keyValuePair.Value;
								OBDRequest vwtprequestForCommand2 = vagecu.GetVWTPRequestForCommand(key);
								vwtprequestForCommand2.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text3 = CS$<>8__locals2.CS$<>8__locals1.<>4__this.DecodeAsASCII(decodedRequest.Command, data, true);
									if (!string.IsNullOrEmpty(text3))
									{
										CS$<>8__locals2.CS$<>8__locals1.sb.AppendLine(CS$<>8__locals2.requestTitle + ": " + text3);
									}
									CS$<>8__locals2.CS$<>8__locals1.<>4__this.ECUExists = true;
								};
								CS$<>8__locals2.CS$<>8__locals1.requests.Add(vwtprequestForCommand2);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = vagecu.IdentsHEX.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<string, string> keyValuePair2 = enumerator.Current;
								VAGECU.<>c__DisplayClass43_2 CS$<>8__locals3 = new VAGECU.<>c__DisplayClass43_2();
								CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
								string key2 = keyValuePair2.Key;
								CS$<>8__locals3.requestTitle = keyValuePair2.Value;
								OBDRequest vwtprequestForCommand3 = vagecu.GetVWTPRequestForCommand(key2);
								vwtprequestForCommand3.ResponseDecoded += delegate(OBDRequest decodedRequest, byte[] data, bool decodeResult, string decodedHeader)
								{
									string text4 = CS$<>8__locals3.CS$<>8__locals2.<>4__this.DecodeAsHEX(decodedRequest.Command, data);
									if (!string.IsNullOrEmpty(text4))
									{
										CS$<>8__locals3.CS$<>8__locals2.sb.AppendLine(CS$<>8__locals3.requestTitle + ": " + text4);
									}
									CS$<>8__locals3.CS$<>8__locals2.<>4__this.ECUExists = true;
								};
								CS$<>8__locals3.CS$<>8__locals2.requests.Add(vwtprequestForCommand3);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						vwtprequestForCommand.ResponseReceived += delegate(OBDRequest openSessionReq2, string data)
						{
							if (data == null || data == "" || data.Contains("NO DATA"))
							{
								List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
								List<OBDRequest> list = queueCopy;
								Predicate<OBDRequest> predicate;
								if ((predicate = CS$<>8__locals1.<>9__3) == null)
								{
									predicate = (CS$<>8__locals1.<>9__3 = (OBDRequest x) => CS$<>8__locals1.requests.Contains(x));
								}
								list.RemoveAll(predicate);
								App.OBDReader.ReplaceQueue(queueCopy);
							}
						};
						App.OBDReader.ReplaceQueue(CS$<>8__locals1.requests);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<GetECUInformationReportAsync_VWTP20>d__43>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					string text = CS$<>8__locals1.sb.ToString();
					if (!string.IsNullOrEmpty(text))
					{
						text = "Protocol: VW TP 2.0\n" + text;
					}
					text2 = CS$<>8__locals1.sb.ToString();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(text2);
			}

			// Token: 0x06003222 RID: 12834 RVA: 0x0022B454 File Offset: 0x00229654
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D0D RID: 7437
			public int <>1__state;

			// Token: 0x04001D0E RID: 7438
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001D0F RID: 7439
			public VAGECU <>4__this;

			// Token: 0x04001D10 RID: 7440
			public CancellationToken token;

			// Token: 0x04001D11 RID: 7441
			private VAGECU.<>c__DisplayClass43_0 <>8__1;

			// Token: 0x04001D12 RID: 7442
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000536 RID: 1334
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTCAsync>d__18 : IAsyncStateMachine
		{
			// Token: 0x06003223 RID: 12835 RVA: 0x0022B464 File Offset: 0x00229664
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0193;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0226;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0281;
					}
					default:
					{
						vagecu.ECUExists = false;
						vagecu.cancellationToken = token;
						if (!vagecu.TestELMDevice)
						{
							goto IL_00E2;
						}
						OBDRequest atshtestRequest = vagecu.GetATSHTestRequest(otherECUs, badELMDetectedCallback);
						OBDRequest atfctestRequest = vagecu.GetATFCTestRequest(otherECUs, badELMDetectedCallback);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { atshtestRequest, atfctestRequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ReadDTCAsync>d__18>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					IL_00E2:
					if (vagecu.TestELMDevice && vagecu.BadELMDetected)
					{
						goto IL_02AE;
					}
					if (string.IsNullOrEmpty(vagecu.ExtendedAddress) || (!string.IsNullOrEmpty(vagecu.RequestHeader) && vagecu.ECUExists) || (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.TP20))
					{
						goto IL_01A3;
					}
					taskAwaiter5 = vagecu.ReadDTCTP20(otherECUs, token).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VAGECU.<ReadDTCAsync>d__18>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0193:
					if (taskAwaiter5.GetResult())
					{
						vagecu.WorkingMode = VAGECU.WorkingModes.TP20;
					}
					IL_01A3:
					if (vagecu.WorkingMode != VAGECU.WorkingModes.Both && vagecu.WorkingMode != VAGECU.WorkingModes.UDS)
					{
						goto IL_0288;
					}
					OBDRequest[] dtcreadRequests = vagecu.GetDTCReadRequests();
					App.OBDReader.ReplaceQueue(dtcreadRequests);
					taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ReadDTCAsync>d__18>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0226:
					taskAwaiter3.GetResult();
					taskAwaiter3 = vagecu.RequestFreezeFrames().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ReadDTCAsync>d__18>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0281:
					taskAwaiter3.GetResult();
					IL_0288:
					vagecu.cancellationToken = CancellationToken.None;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02AE:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003224 RID: 12836 RVA: 0x0022B750 File Offset: 0x00229950
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D13 RID: 7443
			public int <>1__state;

			// Token: 0x04001D14 RID: 7444
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001D15 RID: 7445
			public VAGECU <>4__this;

			// Token: 0x04001D16 RID: 7446
			public CancellationToken token;

			// Token: 0x04001D17 RID: 7447
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001D18 RID: 7448
			public Action badELMDetectedCallback;

			// Token: 0x04001D19 RID: 7449
			private TaskAwaiter <>u__1;

			// Token: 0x04001D1A RID: 7450
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000537 RID: 1335
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDTCTP20>d__39 : IAsyncStateMachine
		{
			// Token: 0x06003225 RID: 12837 RVA: 0x0022B760 File Offset: 0x00229960
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				bool flag;
				try
				{
					if (num > 3)
					{
						result = false;
						if (string.IsNullOrEmpty(vagecu.ExtendedAddress) || vagecu.vwTp20Worker == null)
						{
							flag = result;
							goto IL_0493;
						}
						if (!VAGECU.HasVWTP20Support)
						{
							flag = result;
							goto IL_0493;
						}
						pingSettingBefore = SharedSettings.Current.AlwaysPingECU;
						SharedSettings.Current.AlwaysPingECU = false;
					}
					try
					{
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter3;
						TaskAwaiter taskAwaiter4;
						switch (num)
						{
						case 0:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
							num = (num2 = -1);
							break;
						case 1:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
							num = (num2 = -1);
							goto IL_01B7;
						case 2:
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
							num = (num2 = -1);
							goto IL_0378;
						case 3:
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0453;
						}
						default:
							if (!SharedSettings.Current.VWTP20OpenSessionForDTCOperations)
							{
								goto IL_011D;
							}
							taskAwaiter3 = vagecu.vwTp20Worker.SendAndReadDataUsingRequestLoop("1089", true, false).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VAGECU.<ReadDTCTP20>d__39>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						if (!taskAwaiter3.GetResult().Item1)
						{
							flag = result;
							goto IL_0493;
						}
						vagecu.ECUExists = true;
						result = true;
						IL_011D:
						if (token.IsCancellationRequested)
						{
							flag = result;
							goto IL_0493;
						}
						if (!vagecu.ECUExists && SharedSettings.Current.VWTP20OpenSessionForDTCOperations)
						{
							goto IL_045A;
						}
						vagecu.WorkingMode = VAGECU.WorkingModes.TP20;
						taskAwaiter3 = vagecu.vwTp20Worker.SendAndReadDataUsingRequestLoop("1802FF00", true, true).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VAGECU.<ReadDTCTP20>d__39>(ref taskAwaiter3, ref this);
							return;
						}
						IL_01B7:
						ValueTuple<bool, string> result2 = taskAwaiter3.GetResult();
						bool item = result2.Item1;
						string item2 = result2.Item2;
						if (!SharedSettings.Current.VWTP20OpenSessionForDTCOperations && !item)
						{
							if (vagecu.ExtendedAddress == "1F")
							{
								IEnumerator<IECU> enumerator = otherECUs.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										IECU iecu = enumerator.Current;
										if (iecu is VAGECU)
										{
											VAGECU vagecu2 = (VAGECU)iecu;
											if (vagecu2.WorkingMode == VAGECU.WorkingModes.TP20)
											{
												vagecu2.IsSelected = false;
											}
											else if (vagecu2.WorkingMode == VAGECU.WorkingModes.Both)
											{
												vagecu2.WorkingMode = VAGECU.WorkingModes.UDS;
											}
											if (string.IsNullOrEmpty(vagecu2.RequestHeader))
											{
												vagecu2.IsSelected = false;
											}
										}
									}
								}
								finally
								{
									if (num < 0 && enumerator != null)
									{
										enumerator.Dispose();
									}
								}
							}
							flag = result;
							goto IL_0493;
						}
						if (!SharedSettings.Current.VWTP20OpenSessionForDTCOperations && item)
						{
							vagecu.ECUExists = true;
							result = true;
						}
						if (!string.IsNullOrEmpty(item2) && !item2.StartsWith("7F18"))
						{
							VAGECU.<>c__DisplayClass39_0 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass39_0();
							CS$<>8__locals1.<>4__this = vagecu;
							byte[] array = BitHelpers.ConvertHexToBytesX(item2);
							CS$<>8__locals1.dtcs = DTCDecoder.DecodeData(vagecu.GetRequestForCommand("1802FF00"), vagecu.RequestHeader, array);
							Device.BeginInvokeOnMainThread(delegate
							{
								bool flag2 = false;
								using (List<DTCItemV2>.Enumerator enumerator2 = CS$<>8__locals1.dtcs.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										VAGECU.<>c__DisplayClass39_1 CS$<>8__locals3 = new VAGECU.<>c__DisplayClass39_1();
										CS$<>8__locals3.dtc = enumerator2.Current;
										CS$<>8__locals3.dtc.Code = DTCItemV2.GetVAGCode(CS$<>8__locals3.dtc.RawCode);
										if (!CS$<>8__locals1.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals3.dtc.Code))
										{
											CS$<>8__locals3.dtc.LoadDescription();
											CS$<>8__locals3.dtc.ECU = CS$<>8__locals1.<>4__this.Name;
											CS$<>8__locals1.<>4__this.DTCCollection.Add(CS$<>8__locals3.dtc);
											DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
											if (recorder != null)
											{
												recorder.Record(CS$<>8__locals3.dtc);
											}
											flag2 = true;
										}
									}
								}
								if (flag2)
								{
									CS$<>8__locals1.<>4__this.UpdateCollection();
								}
							});
						}
						if (token.IsCancellationRequested)
						{
							flag = result;
							goto IL_0493;
						}
						taskAwaiter3 = vagecu.vwTp20Worker.SendAndReadDataUsingRequestLoop("1800FF00", true, true).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 2);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, VAGECU.<ReadDTCTP20>d__39>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0378:
						string item3 = taskAwaiter3.GetResult().Item2;
						if (!string.IsNullOrEmpty(item3) && !item3.StartsWith("7F18"))
						{
							VAGECU.<>c__DisplayClass39_2 CS$<>8__locals2 = new VAGECU.<>c__DisplayClass39_2();
							CS$<>8__locals2.<>4__this = vagecu;
							byte[] array2 = BitHelpers.ConvertHexToBytesX(item3);
							CS$<>8__locals2.dtcs = DTCDecoder.DecodeData(vagecu.GetRequestForCommand("1800FF00"), vagecu.RequestHeader, array2);
							Device.BeginInvokeOnMainThread(delegate
							{
								bool flag3 = false;
								using (List<DTCItemV2>.Enumerator enumerator3 = CS$<>8__locals2.dtcs.GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										VAGECU.<>c__DisplayClass39_3 CS$<>8__locals4 = new VAGECU.<>c__DisplayClass39_3();
										CS$<>8__locals4.dtc = enumerator3.Current;
										CS$<>8__locals4.dtc.Code = DTCItemV2.GetVAGCode(CS$<>8__locals4.dtc.RawCode);
										if (!CS$<>8__locals2.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals4.dtc.Code))
										{
											CS$<>8__locals4.dtc.LoadDescription();
											CS$<>8__locals4.dtc.ECU = CS$<>8__locals2.<>4__this.Name;
											CS$<>8__locals2.<>4__this.DTCCollection.Add(CS$<>8__locals4.dtc);
											DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
											if (recorder2 != null)
											{
												recorder2.Record(CS$<>8__locals4.dtc);
											}
											flag3 = true;
										}
									}
								}
								if (flag3)
								{
									CS$<>8__locals2.<>4__this.UpdateCollection();
								}
							});
						}
						if (token.IsCancellationRequested)
						{
							flag = result;
							goto IL_0493;
						}
						taskAwaiter4 = vagecu.vwTp20Worker.CloseChannel(true).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<ReadDTCTP20>d__39>(ref taskAwaiter4, ref this);
							return;
						}
						IL_0453:
						taskAwaiter4.GetResult();
						IL_045A:;
					}
					finally
					{
						if (num < 0)
						{
							SharedSettings.Current.AlwaysPingECU = pingSettingBefore;
						}
					}
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0493:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06003226 RID: 12838 RVA: 0x0022BC60 File Offset: 0x00229E60
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D1B RID: 7451
			public int <>1__state;

			// Token: 0x04001D1C RID: 7452
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001D1D RID: 7453
			public VAGECU <>4__this;

			// Token: 0x04001D1E RID: 7454
			public CancellationToken token;

			// Token: 0x04001D1F RID: 7455
			public IEnumerable<IECU> otherECUs;

			// Token: 0x04001D20 RID: 7456
			private bool <result>5__2;

			// Token: 0x04001D21 RID: 7457
			private bool <pingSettingBefore>5__3;

			// Token: 0x04001D22 RID: 7458
			private TaskAwaiter<ValueTuple<bool, string>> <>u__1;

			// Token: 0x04001D23 RID: 7459
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000538 RID: 1336
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestFreezeFrames>d__35 : IAsyncStateMachine
		{
			// Token: 0x06003227 RID: 12839 RVA: 0x0022BC70 File Offset: 0x00229E70
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VAGECU vagecu = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (vagecu.Count == 0)
						{
							goto IL_0163;
						}
						if (vagecu.WorkingMode == VAGECU.WorkingModes.TP20)
						{
							goto IL_0148;
						}
						List<OBDRequest> list = new List<OBDRequest>();
						IEnumerator<DTCItemV2> enumerator = vagecu.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								VAGECU.<>c__DisplayClass35_0 CS$<>8__locals1 = new VAGECU.<>c__DisplayClass35_0();
								CS$<>8__locals1.dtc = enumerator.Current;
								if (CS$<>8__locals1.dtc.RawCode.Length == 6)
								{
									OBDRequest requestForCommand = vagecu.GetRequestForCommand("1906" + CS$<>8__locals1.dtc.RawCode + "FF");
									requestForCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
									{
										if (data != null && data.Length >= 14)
										{
											DateTime udsdateFromFreezeFrame = VAGECU.GetUDSDateFromFreezeFrame(new byte[]
											{
												data[10],
												data[11],
												data[12],
												data[13]
											});
											if (udsdateFromFreezeFrame > new DateTime(2008, 1, 1) && udsdateFromFreezeFrame < DateTimeNowHelper.NowSafe)
											{
												VAGECU.<>c__DisplayClass35_1 CS$<>8__locals2 = new VAGECU.<>c__DisplayClass35_1();
												CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
												CS$<>8__locals2.freezeframeText = string.Format(Translate.GetString("dtc_VAGFreezeFrameUDS"), new object[]
												{
													data[0],
													data[1],
													data[2],
													data[4],
													((int)data[5] * 65536 + (int)data[6] * 256 + (int)data[7]).ToString() + " km",
													VAGECU.GetUDSDateStringFromFreezeFrame(udsdateFromFreezeFrame)
												});
												byte[] array = null;
												if (data.Length > 23 && data[14] == 112)
												{
													if (data.Length > 16)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_010C"),
															": ",
															(((int)data[15] * 256 + (int)data[16]) / 4).ToString(),
															" rpm"
														});
													}
													if (data.Length > 17)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_0104"),
															": ",
															((double)data[17] * 100.0 / 255.0).ToString("0.00"),
															" %"
														});
													}
													if (data.Length > 18)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_010D"),
															": ",
															UnitsHelper.GetValue((double)data[18], UnitsHelper.Units.kmh).ToString(),
															" ",
															UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
														});
													}
													if (data.Length > 19)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_0105"),
															": ",
															UnitsHelper.GetValue((double)(data[19] - 40), UnitsHelper.Units.celicium).ToString(),
															" ",
															UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
														});
													}
													if (data.Length > 20)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_015C"),
															": ",
															UnitsHelper.GetValue((double)(data[20] - 40), UnitsHelper.Units.celicium).ToString(),
															" ",
															UnitsHelper.GetCaption(UnitsHelper.Units.celicium)
														});
													}
													if (data.Length > 21)
													{
														CS$<>8__locals2.freezeframeText = string.Concat(new string[]
														{
															CS$<>8__locals2.freezeframeText,
															"\n",
															Translate.GetString("PID_0133"),
															": ",
															UnitsHelper.GetValue((double)data[21], UnitsHelper.Units.kPa).ToString(),
															" ",
															UnitsHelper.GetCaption(UnitsHelper.Units.kPa)
														});
													}
													if (data.Length > 23)
													{
														CS$<>8__locals2.freezeframeText = CS$<>8__locals2.freezeframeText + "\nKlemme 30 voltage: " + (((int)data[22] * 256 + (int)data[23]) / 1000).ToString();
													}
													if (data.Length > 24 && data[24] == 113)
													{
														array = data.Skip(25).ToArray<byte>();
													}
												}
												if (array == null && data.Length > 14 && data[14] == 113)
												{
													array = data.Skip(15).ToArray<byte>();
												}
												if (array != null && array.Length != 0)
												{
													StringBuilder stringBuilder = new StringBuilder(array.Length * 2 + 1);
													stringBuilder.Append("Paramteric values: ");
													foreach (byte b in array)
													{
														stringBuilder.Append(' ');
														stringBuilder.Append(b.ToString("X2"));
													}
													CS$<>8__locals2.freezeframeText = CS$<>8__locals2.freezeframeText + "\n" + stringBuilder.ToString();
												}
												if (!MainThread.IsMainThread)
												{
													Device.BeginInvokeOnMainThread(delegate
													{
														CS$<>8__locals2.CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.freezeframeText;
														DataRecorderV2 recorder3 = App.OBDReader.CurrentCarData.Recorder;
														if (recorder3 == null)
														{
															return;
														}
														recorder3.Record(CS$<>8__locals2.CS$<>8__locals1.dtc);
													});
													return;
												}
												CS$<>8__locals1.dtc.Payload = CS$<>8__locals2.freezeframeText;
												DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
												if (recorder == null)
												{
													return;
												}
												recorder.Record(CS$<>8__locals1.dtc);
												return;
											}
											else
											{
												VAGECU.<>c__DisplayClass35_2 CS$<>8__locals3 = new VAGECU.<>c__DisplayClass35_2();
												CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
												StringBuilder stringBuilder2 = new StringBuilder(data.Length * 2 + 1);
												stringBuilder2.Append("Freeze frame data: ");
												foreach (byte b2 in data)
												{
													stringBuilder2.Append(' ');
													stringBuilder2.Append(b2.ToString("X2"));
												}
												CS$<>8__locals3.freezeframeText = stringBuilder2.ToString();
												if (MainThread.IsMainThread)
												{
													CS$<>8__locals1.dtc.Payload = CS$<>8__locals3.freezeframeText;
													DataRecorderV2 recorder2 = App.OBDReader.CurrentCarData.Recorder;
													if (recorder2 == null)
													{
														return;
													}
													recorder2.Record(CS$<>8__locals1.dtc);
													return;
												}
												else
												{
													Device.BeginInvokeOnMainThread(delegate
													{
														CS$<>8__locals3.CS$<>8__locals2.dtc.Payload = CS$<>8__locals3.freezeframeText;
														DataRecorderV2 recorder4 = App.OBDReader.CurrentCarData.Recorder;
														if (recorder4 == null)
														{
															return;
														}
														recorder4.Record(CS$<>8__locals3.CS$<>8__locals2.dtc);
													});
												}
											}
										}
									};
									requestForCommand.ResponseReceived += vagecu.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
									list.Add(requestForCommand);
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (list.Count == 0)
						{
							goto IL_0163;
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VAGECU.<RequestFreezeFrames>d__35>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					IL_0148:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0163:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003228 RID: 12840 RVA: 0x0022BE28 File Offset: 0x0022A028
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D24 RID: 7460
			public int <>1__state;

			// Token: 0x04001D25 RID: 7461
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001D26 RID: 7462
			public VAGECU <>4__this;

			// Token: 0x04001D27 RID: 7463
			private TaskAwaiter <>u__1;
		}
	}
}
