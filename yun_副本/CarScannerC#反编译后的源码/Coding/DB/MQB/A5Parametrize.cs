using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A79 RID: 2681
	internal class A5Parametrize : MQBParametrizeBase
	{
		// Token: 0x060054B2 RID: 21682 RVA: 0x00404330 File Offset: 0x00402530
		public A5Parametrize()
		{
			base.Group = CodingGroup.Assistance;
			base.ValueType = AdaptationValueTypes.OptionType;
			this.Password = "20103";
			base.PreReadCommands = "1003;1040;2704;";
			base.PreWriteCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;";
			base.PostWriteCommands = "700:3E80;37;700:3E80;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF;1902AF;757:1003;757:14FFFFFF;757:1102;757:14FFFFFF;757:1102;";
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("A5");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("A5");
			this.RequiresPro = true;
			base.Address = 1;
			base.DataLengthFormatLength = 4;
			base.AddressFormatLength = 4;
			base.Name = Translate.GetString("codingDB_A5TJA_Name");
			base.Description = Translate.GetString("codingDB_A5TJA_Description");
			base.InnerDescription = Translate.GetString("codingDB_A5TJA_InnerDescription");
			this.DefaultMaxBlockSize = 258U;
		}

		// Token: 0x060054B3 RID: 21683 RVA: 0x00404428 File Offset: 0x00402628
		protected async Task GetIdents()
		{
			List<OBDRequest> list = new List<OBDRequest>(4);
			this.BuildDefaultBeforeAndAfterCommands();
			if (!string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
			{
				this.VIN = CarInfoViewModel.Instance.VIN;
			}
			else
			{
				OBDRequest obdrequest = new OBDRequest("22F190", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1;ATAL;ATCRA7E8;ATST64", "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF", false);
				obdrequest.ResponseDecoded += this.VinNumber_ResponseDecoded;
				list.Add(obdrequest);
			}
			OBDRequest obdrequest2 = new OBDRequest("22F187", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseDecoded += this.PartNumReq_ResponseDecoded;
			OBDRequest obdrequest3 = new OBDRequest("22F189", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseDecoded += this.FirmwareReq_ResponseDecoded;
			OBDRequest obdrequest4 = new OBDRequest("1003", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			this.PartNumber = "";
			this.FirmwareVersion = "";
			list.Add(obdrequest4);
			list.Add(obdrequest2);
			list.Add(obdrequest3);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
		}

		// Token: 0x060054B4 RID: 21684 RVA: 0x0040446B File Offset: 0x0040266B
		private void VinNumber_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.VIN = Encoding.ASCII.GetString(data);
			}
		}

		// Token: 0x060054B5 RID: 21685 RVA: 0x00404488 File Offset: 0x00402688
		private void FirmwareReq_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string text = Encoding.ASCII.GetString(data);
				text = text.Trim().Replace(" ", "");
				this.FirmwareVersion = text;
			}
		}

		// Token: 0x060054B6 RID: 21686 RVA: 0x004044C8 File Offset: 0x004026C8
		private void PartNumReq_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string text = Encoding.ASCII.GetString(data);
				text = text.Trim().Replace(" ", "");
				this.PartNumber = text;
			}
		}

		// Token: 0x060054B7 RID: 21687 RVA: 0x00404508 File Offset: 0x00402708
		protected void LoadOptionValue(MQBAdaptationOption opt)
		{
			if (opt.Value.Contains('.'))
			{
				string text = PackageFileReader.ReadFileToString(opt.Value);
				opt.Value = text;
			}
		}

		// Token: 0x060054B8 RID: 21688 RVA: 0x00404538 File Offset: 0x00402738
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.Options.Clear();
			await this.GetIdents();
			if (this.PartNumber == "3Q0980654L")
			{
				if (this.FirmwareVersion == "0610")
				{
					MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA+EA+VZA)", this.path + ".3Q0980654L_0610_1.dat");
					base.Options.Add(mqbadaptationOption);
					mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 2) + " (VZA)", this.path + ".3Q0980654L_0610_2.dat");
					base.Options.Add(mqbadaptationOption);
					mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 3) + " (Kodiaq TJA+EA+VZA)", this.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
					base.Options.Add(mqbadaptationOption);
					this.LoadBinaryFromFolderByFilenamePart("3Q0980654L");
				}
			}
			else if (this.PartNumber == "3Q0980654H")
			{
				if (this.FirmwareVersion == "0271" || this.FirmwareVersion == "0272")
				{
					MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", this.path + ".3Q0980654H_0271_0272.dat");
					base.Options.Add(mqbadaptationOption2);
					this.LoadBinaryFromFolderByFilenamePart("3Q0980654H");
				}
				else
				{
					MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption("TJA" + string.Format(Translate.GetString("coding_Variant"), 1), this.path + ".3Q0980654H_tiguan_tja.dat");
					base.Options.Add(mqbadaptationOption3);
					mqbadaptationOption3 = new MQBAdaptationOption("TJA" + string.Format(Translate.GetString("coding_Variant"), 2), this.path + ".3Q0980654H_tiguan_xl_tja.dat");
					base.Options.Add(mqbadaptationOption3);
					this.LoadBinaryFromFolderByFilenamePart("3Q0980654H");
				}
			}
			else if (this.PartNumber == "3Q0980654G")
			{
				MQBAdaptationOption mqbadaptationOption4 = new MQBAdaptationOption(Translate.GetString("coding_Apply"), this.path + ".3Q0980654G_tiguan_tja.dat");
				base.Options.Add(mqbadaptationOption4);
			}
			else if (this.PartNumber == "3Q0980654S")
			{
				if (this.FirmwareVersion == "0920")
				{
					MQBAdaptationOption mqbadaptationOption5 = new MQBAdaptationOption(Translate.GetString("coding_Apply"), this.path + ".3Q0980654S.dat");
					base.Options.Add(mqbadaptationOption5);
				}
			}
			else if (this.PartNumber == "3QD980654")
			{
				string firmwareVersion = this.FirmwareVersion;
				if (!(firmwareVersion == "1272"))
				{
					if (firmwareVersion == "1611")
					{
						MQBAdaptationOption mqbadaptationOption6 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", this.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
						base.Options.Add(mqbadaptationOption6);
						this.LoadBinaryFromFolderByFilenamePart("3Q0980654L");
					}
				}
				else
				{
					MQBAdaptationOption mqbadaptationOption7 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", this.path + ".3QD980654_1272_Kodiaq.dat");
					base.Options.Add(mqbadaptationOption7);
					this.LoadBinaryFromFolderByFilenamePart("3Q0980654H");
				}
			}
			else if (this.PartNumber == "3QD980654A")
			{
				if (this.FirmwareVersion == "1611")
				{
					MQBAdaptationOption mqbadaptationOption8 = new MQBAdaptationOption("TJA " + string.Format(Translate.GetString("coding_Variant"), 1), this.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
					base.Options.Add(mqbadaptationOption8);
					this.LoadBinaryFromFolderByFilenamePart("3Q0980654L");
				}
			}
			else if (this.PartNumber == "3Q0980653F")
			{
				MQBAdaptationOption mqbadaptationOption9 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", this.path + ".3Q0980653F_from_Passat_for_all_cars.dat");
				base.Options.Add(mqbadaptationOption9);
			}
			foreach (MQBAdaptationOption mqbadaptationOption10 in base.Options)
			{
				this.LoadOptionValue(mqbadaptationOption10);
			}
			CodingRequestResult codingRequestResult;
			if (base.Options.Count == 0)
			{
				this.HasCurrentState = true;
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				base.CurrentState = "Detected HW: " + this.PartNumber + " SW: " + this.FirmwareVersion;
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x060054B9 RID: 21689 RVA: 0x0040457C File Offset: 0x0040277C
		private void LoadBinaryFromFolderByFilenamePart(string part_fn)
		{
			string text = "vag.a5_60sec.";
			foreach (string text2 in (from x in PackageFileReader.GetFilesInDirectory(text)
				where x.Contains(part_fn)
				select x).ToArray<string>())
			{
				using (Stream stream = PackageFileReader.OpenFileStream(text + text2))
				{
					byte[] array2 = new byte[stream.Length];
					stream.Read(array2, 0, array2.Length);
					string text3 = BitHelpers.ByteArrayToHexString(array2);
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
					MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(fileNameWithoutExtension.Substring(fileNameWithoutExtension.IndexOf("_wTJ_") + 5).Replace('_', ' '), text3);
					base.Options.Add(mqbadaptationOption);
				}
			}
		}

		// Token: 0x060054BA RID: 21690 RVA: 0x00404660 File Offset: 0x00402860
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (!SharedSettings.Current.DatasetAllowed)
			{
				base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				if (!this.allowWhileEngineRunning)
				{
					A5Parametrize.<>c__DisplayClass13_0 CS$<>8__locals1 = new A5Parametrize.<>c__DisplayClass13_0();
					OBDRequest obdrequest = new OBDRequest("010C", false);
					CS$<>8__locals1.engineIsRunning = false;
					obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
						{
							CS$<>8__locals1.engineIsRunning = true;
						}
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
					await App.OBDReader.WaitForCommandQueue();
					if (CS$<>8__locals1.engineIsRunning)
					{
						return CodingRequestResult.WrongConditions;
					}
					CS$<>8__locals1 = null;
				}
				base.DataLength = value.Length / 2;
				MQBAdaptationTemplate lcA5 = new MQBAdaptationTemplate(0, "", "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, "0600", base.RequestHeader, base.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0]);
				CodingRequestResult codingRequestResult2 = await lcA5.UpdateCurrentState("", null);
				if (codingRequestResult2 != CodingRequestResult.Success)
				{
					codingRequestResult = codingRequestResult2;
				}
				else
				{
					string lcBefore = lcA5.CurrentState;
					CodingRequestResult result = await this.WriteDataToECU(password, UserFriendlyValue, progress, null, value);
					TaskAwaiter<CodingRequestResult> taskAwaiter = lcA5.UpdateCurrentState("", null).GetAwaiter();
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
					}
					if (taskAwaiter.GetResult() != CodingRequestResult.Success)
					{
						CodingLogItem.RecordToLog("A5 Long coding backup", "", CodingLogItem.CodingTypes.MQB, "0600", lcBefore, lcBefore, "", base.RequestHeader, base.ResponseHeader, "1003", "", "", "96", null, "");
					}
					else
					{
						string currentState = lcA5.CurrentState;
						if (lcBefore != currentState)
						{
							taskAwaiter = lcA5.Execute("", lcBefore, "A5 Long coding restore", progress, BitHelpers.ConvertHexToBytesX(currentState), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							}
							if (taskAwaiter.GetResult() != CodingRequestResult.Success)
							{
								CodingLogItem.RecordToLog("A5 Long coding backup", "", CodingLogItem.CodingTypes.MQB, "0600", lcBefore, lcBefore, "", base.RequestHeader, base.ResponseHeader, "1003", "", "", "96", null, "");
							}
						}
					}
					codingRequestResult = result;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x040033CB RID: 13259
		private string path = "vag.a5tja";

		// Token: 0x040033CC RID: 13260
		protected string PartNumber = "";

		// Token: 0x040033CD RID: 13261
		protected string FirmwareVersion = "";

		// Token: 0x040033CE RID: 13262
		protected string VIN = "";

		// Token: 0x040033CF RID: 13263
		protected bool allowWhileEngineRunning = true;

		// Token: 0x02000A7A RID: 2682
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x060054BB RID: 21691 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x060054BC RID: 21692 RVA: 0x004046C4 File Offset: 0x004028C4
			internal bool <LoadBinaryFromFolderByFilenamePart>b__0(string x)
			{
				return x.Contains(this.part_fn);
			}

			// Token: 0x040033D0 RID: 13264
			public string part_fn;
		}

		// Token: 0x02000A7B RID: 2683
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x060054BD RID: 21693 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x060054BE RID: 21694 RVA: 0x004046D2 File Offset: 0x004028D2
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x040033D1 RID: 13265
			public bool engineIsRunning;
		}

		// Token: 0x02000A7C RID: 2684
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__13 : IAsyncStateMachine
		{
			// Token: 0x060054BF RID: 21695 RVA: 0x004046F8 File Offset: 0x004028F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				A5Parametrize a5Parametrize = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<CodingRequestResult> taskAwaiter5;
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
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_01EA;
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_027D;
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_02F0;
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_03E3;
					default:
					{
						if (!SharedSettings.Current.DatasetAllowed)
						{
							a5Parametrize.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							codingRequestResult = CodingRequestResult.NotSupported;
							goto IL_0468;
						}
						if (a5Parametrize.allowWhileEngineRunning)
						{
							goto IL_0120;
						}
						CS$<>8__locals1 = new A5Parametrize.<>c__DisplayClass13_0();
						OBDRequest obdrequest = new OBDRequest("010C", false);
						CS$<>8__locals1.engineIsRunning = false;
						obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
							{
								CS$<>8__locals1.engineIsRunning = true;
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, A5Parametrize.<Execute>d__13>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_0468;
					}
					CS$<>8__locals1 = null;
					IL_0120:
					a5Parametrize.DataLength = value.Length / 2;
					lcA5 = new MQBAdaptationTemplate(0, "", "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, "0600", a5Parametrize.RequestHeader, a5Parametrize.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[0]);
					taskAwaiter5 = lcA5.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, A5Parametrize.<Execute>d__13>(ref taskAwaiter5, ref this);
						return;
					}
					IL_01EA:
					CodingRequestResult result2 = taskAwaiter5.GetResult();
					if (result2 != CodingRequestResult.Success)
					{
						codingRequestResult = result2;
						goto IL_0468;
					}
					lcBefore = lcA5.CurrentState;
					taskAwaiter5 = a5Parametrize.WriteDataToECU(password, UserFriendlyValue, progress, null, value).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, A5Parametrize.<Execute>d__13>(ref taskAwaiter5, ref this);
						return;
					}
					IL_027D:
					CodingRequestResult result3 = taskAwaiter5.GetResult();
					result = result3;
					taskAwaiter5 = lcA5.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 3;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, A5Parametrize.<Execute>d__13>(ref taskAwaiter5, ref this);
						return;
					}
					IL_02F0:
					if (taskAwaiter5.GetResult() != CodingRequestResult.Success)
					{
						CodingLogItem.RecordToLog("A5 Long coding backup", "", CodingLogItem.CodingTypes.MQB, "0600", lcBefore, lcBefore, "", a5Parametrize.RequestHeader, a5Parametrize.ResponseHeader, "1003", "", "", "96", null, "");
						goto IL_0438;
					}
					string currentState = lcA5.CurrentState;
					if (!(lcBefore != currentState))
					{
						goto IL_0438;
					}
					taskAwaiter5 = lcA5.Execute("", lcBefore, "A5 Long coding restore", progress, BitHelpers.ConvertHexToBytesX(currentState), true).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 4;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, A5Parametrize.<Execute>d__13>(ref taskAwaiter5, ref this);
						return;
					}
					IL_03E3:
					if (taskAwaiter5.GetResult() != CodingRequestResult.Success)
					{
						CodingLogItem.RecordToLog("A5 Long coding backup", "", CodingLogItem.CodingTypes.MQB, "0600", lcBefore, lcBefore, "", a5Parametrize.RequestHeader, a5Parametrize.ResponseHeader, "1003", "", "", "96", null, "");
					}
					IL_0438:
					codingRequestResult = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					lcA5 = null;
					lcBefore = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0468:
				num2 = -2;
				lcA5 = null;
				lcBefore = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060054C0 RID: 21696 RVA: 0x00404BAC File Offset: 0x00402DAC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040033D2 RID: 13266
			public int <>1__state;

			// Token: 0x040033D3 RID: 13267
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040033D4 RID: 13268
			public A5Parametrize <>4__this;

			// Token: 0x040033D5 RID: 13269
			private A5Parametrize.<>c__DisplayClass13_0 <>8__1;

			// Token: 0x040033D6 RID: 13270
			public string value;

			// Token: 0x040033D7 RID: 13271
			public string password;

			// Token: 0x040033D8 RID: 13272
			public string UserFriendlyValue;

			// Token: 0x040033D9 RID: 13273
			public IProgress<string> progress;

			// Token: 0x040033DA RID: 13274
			private MQBAdaptationTemplate <lcA5>5__2;

			// Token: 0x040033DB RID: 13275
			private string <lcBefore>5__3;

			// Token: 0x040033DC RID: 13276
			private CodingRequestResult <result>5__4;

			// Token: 0x040033DD RID: 13277
			private TaskAwaiter <>u__1;

			// Token: 0x040033DE RID: 13278
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000A7D RID: 2685
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetIdents>d__5 : IAsyncStateMachine
		{
			// Token: 0x060054C1 RID: 21697 RVA: 0x00404BBC File Offset: 0x00402DBC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				A5Parametrize a5Parametrize = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						List<OBDRequest> list = new List<OBDRequest>(4);
						a5Parametrize.BuildDefaultBeforeAndAfterCommands();
						if (!string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
						{
							a5Parametrize.VIN = CarInfoViewModel.Instance.VIN;
						}
						else
						{
							OBDRequest obdrequest = new OBDRequest("22F190", "7E0", "ATFCSH7E0;ATFCSD300000;ATFCSM1;ATAL;ATCRA7E8;ATST64", "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF", false);
							obdrequest.ResponseDecoded += a5Parametrize.VinNumber_ResponseDecoded;
							list.Add(obdrequest);
						}
						OBDRequest obdrequest2 = new OBDRequest("22F187", a5Parametrize.RequestHeader, a5Parametrize.BeforeCommands, a5Parametrize.AfterCommands, false);
						obdrequest2.ResponseDecoded += a5Parametrize.PartNumReq_ResponseDecoded;
						OBDRequest obdrequest3 = new OBDRequest("22F189", a5Parametrize.RequestHeader, a5Parametrize.BeforeCommands, a5Parametrize.AfterCommands, false);
						obdrequest3.ResponseDecoded += a5Parametrize.FirmwareReq_ResponseDecoded;
						OBDRequest obdrequest4 = new OBDRequest("1003", a5Parametrize.RequestHeader, a5Parametrize.BeforeCommands, a5Parametrize.AfterCommands, false);
						a5Parametrize.PartNumber = "";
						a5Parametrize.FirmwareVersion = "";
						list.Add(obdrequest4);
						list.Add(obdrequest2);
						list.Add(obdrequest3);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, A5Parametrize.<GetIdents>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
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

			// Token: 0x060054C2 RID: 21698 RVA: 0x00404DA8 File Offset: 0x00402FA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040033DF RID: 13279
			public int <>1__state;

			// Token: 0x040033E0 RID: 13280
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040033E1 RID: 13281
			public A5Parametrize <>4__this;

			// Token: 0x040033E2 RID: 13282
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A7E RID: 2686
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__10 : IAsyncStateMachine
		{
			// Token: 0x060054C3 RID: 21699 RVA: 0x00404DB8 File Offset: 0x00402FB8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				A5Parametrize a5Parametrize = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						a5Parametrize.Options.Clear();
						taskAwaiter = a5Parametrize.GetIdents().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, A5Parametrize.<UpdateCurrentState>d__10>(ref taskAwaiter, ref this);
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
					if (a5Parametrize.PartNumber == "3Q0980654L")
					{
						if (a5Parametrize.FirmwareVersion == "0610")
						{
							MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA+EA+VZA)", a5Parametrize.path + ".3Q0980654L_0610_1.dat");
							a5Parametrize.Options.Add(mqbadaptationOption);
							mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 2) + " (VZA)", a5Parametrize.path + ".3Q0980654L_0610_2.dat");
							a5Parametrize.Options.Add(mqbadaptationOption);
							mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 3) + " (Kodiaq TJA+EA+VZA)", a5Parametrize.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
							a5Parametrize.Options.Add(mqbadaptationOption);
							string text = "3Q0980654L";
							a5Parametrize.LoadBinaryFromFolderByFilenamePart(text);
						}
					}
					else if (a5Parametrize.PartNumber == "3Q0980654H")
					{
						if (a5Parametrize.FirmwareVersion == "0271" || a5Parametrize.FirmwareVersion == "0272")
						{
							MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", a5Parametrize.path + ".3Q0980654H_0271_0272.dat");
							a5Parametrize.Options.Add(mqbadaptationOption2);
							string text2 = "3Q0980654H";
							a5Parametrize.LoadBinaryFromFolderByFilenamePart(text2);
						}
						else
						{
							MQBAdaptationOption mqbadaptationOption3 = new MQBAdaptationOption("TJA" + string.Format(Translate.GetString("coding_Variant"), 1), a5Parametrize.path + ".3Q0980654H_tiguan_tja.dat");
							a5Parametrize.Options.Add(mqbadaptationOption3);
							mqbadaptationOption3 = new MQBAdaptationOption("TJA" + string.Format(Translate.GetString("coding_Variant"), 2), a5Parametrize.path + ".3Q0980654H_tiguan_xl_tja.dat");
							a5Parametrize.Options.Add(mqbadaptationOption3);
							string text3 = "3Q0980654H";
							a5Parametrize.LoadBinaryFromFolderByFilenamePart(text3);
						}
					}
					else if (a5Parametrize.PartNumber == "3Q0980654G")
					{
						MQBAdaptationOption mqbadaptationOption4 = new MQBAdaptationOption(Translate.GetString("coding_Apply"), a5Parametrize.path + ".3Q0980654G_tiguan_tja.dat");
						a5Parametrize.Options.Add(mqbadaptationOption4);
					}
					else if (a5Parametrize.PartNumber == "3Q0980654S")
					{
						if (a5Parametrize.FirmwareVersion == "0920")
						{
							MQBAdaptationOption mqbadaptationOption5 = new MQBAdaptationOption(Translate.GetString("coding_Apply"), a5Parametrize.path + ".3Q0980654S.dat");
							a5Parametrize.Options.Add(mqbadaptationOption5);
						}
					}
					else if (a5Parametrize.PartNumber == "3QD980654")
					{
						string firmwareVersion = a5Parametrize.FirmwareVersion;
						if (!(firmwareVersion == "1272"))
						{
							if (firmwareVersion == "1611")
							{
								MQBAdaptationOption mqbadaptationOption6 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", a5Parametrize.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
								a5Parametrize.Options.Add(mqbadaptationOption6);
								string text4 = "3Q0980654L";
								a5Parametrize.LoadBinaryFromFolderByFilenamePart(text4);
							}
						}
						else
						{
							MQBAdaptationOption mqbadaptationOption7 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", a5Parametrize.path + ".3QD980654_1272_Kodiaq.dat");
							a5Parametrize.Options.Add(mqbadaptationOption7);
							string text5 = "3Q0980654H";
							a5Parametrize.LoadBinaryFromFolderByFilenamePart(text5);
						}
					}
					else if (a5Parametrize.PartNumber == "3QD980654A")
					{
						if (a5Parametrize.FirmwareVersion == "1611")
						{
							MQBAdaptationOption mqbadaptationOption8 = new MQBAdaptationOption("TJA " + string.Format(Translate.GetString("coding_Variant"), 1), a5Parametrize.path + ".l_koidaq_stock_3QD980654A_Unknown.dat");
							a5Parametrize.Options.Add(mqbadaptationOption8);
							string text6 = "3Q0980654L";
							a5Parametrize.LoadBinaryFromFolderByFilenamePart(text6);
						}
					}
					else if (a5Parametrize.PartNumber == "3Q0980653F")
					{
						MQBAdaptationOption mqbadaptationOption9 = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), 1) + " (TJA)", a5Parametrize.path + ".3Q0980653F_from_Passat_for_all_cars.dat");
						a5Parametrize.Options.Add(mqbadaptationOption9);
					}
					IEnumerator<MQBAdaptationOption> enumerator = a5Parametrize.Options.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MQBAdaptationOption mqbadaptationOption10 = enumerator.Current;
							a5Parametrize.LoadOptionValue(mqbadaptationOption10);
						}
					}
					finally
					{
						if (num < 0 && enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					if (a5Parametrize.Options.Count == 0)
					{
						a5Parametrize.HasCurrentState = true;
						a5Parametrize.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						codingRequestResult = CodingRequestResult.NotSupported;
					}
					else
					{
						a5Parametrize.CurrentState = "Detected HW: " + a5Parametrize.PartNumber + " SW: " + a5Parametrize.FirmwareVersion;
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060054C4 RID: 21700 RVA: 0x004053B0 File Offset: 0x004035B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040033E3 RID: 13283
			public int <>1__state;

			// Token: 0x040033E4 RID: 13284
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040033E5 RID: 13285
			public A5Parametrize <>4__this;

			// Token: 0x040033E6 RID: 13286
			private TaskAwaiter <>u__1;
		}
	}
}
