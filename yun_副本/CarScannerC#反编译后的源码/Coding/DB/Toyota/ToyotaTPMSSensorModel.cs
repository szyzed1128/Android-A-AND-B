using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009E7 RID: 2535
	internal class ToyotaTPMSSensorModel : CustomizableCodingTemplate
	{
		// Token: 0x06005195 RID: 20885 RVA: 0x003F27F9 File Offset: 0x003F09F9
		public ToyotaTPMSSensorModel()
		{
			base.Name = "TPMS sensors";
			this.ValueType = AdaptationValueTypes.ToyotaTPMSSensor;
		}

		// Token: 0x170017AF RID: 6063
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x003F2834 File Offset: 0x003F0A34
		// (set) Token: 0x06005197 RID: 20887 RVA: 0x003F283C File Offset: 0x003F0A3C
		public ObservableCollection<ToyotaTPMSSensorItem> SensorItems
		{
			[CompilerGenerated]
			get
			{
				return this.<SensorItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SensorItems>k__BackingField = value;
			}
		} = new ObservableCollection<ToyotaTPMSSensorItem>();

		// Token: 0x06005198 RID: 20888 RVA: 0x003F2848 File Offset: 0x003F0A48
		private void TPMS_Request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			string command = request.Command;
			string text = request.Command.Substring(2);
			if (data.Length < 4)
			{
				ToyotaTPMSSensorItem toyotaTPMSSensorItem = new ToyotaTPMSSensorItem(text, null);
				this.SensorItems.Add(toyotaTPMSSensorItem);
				return;
			}
			ToyotaTPMSSensorItem toyotaTPMSSensorItem2 = new ToyotaTPMSSensorItem(text, data);
			this.SensorItems.Add(toyotaTPMSSensorItem2);
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x003F2898 File Offset: 0x003F0A98
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.A801Items.Clear();
			OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit("A801", "750", "2A");
			obdrequestForUnit.ResponseReceived += delegate(OBDRequest req, string response)
			{
				List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, "2A", response);
				this.A801Items.AddRange(list);
			};
			this.A803Items.Clear();
			OBDRequest obdrequestForUnit2 = ToyotaUnitsDetector.GetOBDRequestForUnit("A803", "750", "2A");
			obdrequestForUnit2.ResponseReceived += delegate(OBDRequest req, string response)
			{
				List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, "2A", response);
				this.A803Items.AddRange(list2);
			};
			this.SensorItems.Clear();
			OBDRequest obdrequestForUnit3 = ToyotaUnitsDetector.GetOBDRequestForUnit("210F", "750", "2A");
			obdrequestForUnit3.ResponseDecoded += this.TPMS_Request_ResponseDecoded;
			OBDRequest obdrequestForUnit4 = ToyotaUnitsDetector.GetOBDRequestForUnit("2110", "750", "2A");
			obdrequestForUnit4.ResponseDecoded += this.TPMS_Request_ResponseDecoded;
			OBDRequest obdrequestForUnit5 = ToyotaUnitsDetector.GetOBDRequestForUnit("2111", "750", "2A");
			obdrequestForUnit5.ResponseDecoded += this.TPMS_Request_ResponseDecoded;
			OBDRequest obdrequestForUnit6 = ToyotaUnitsDetector.GetOBDRequestForUnit("2112", "750", "2A");
			obdrequestForUnit6.ResponseDecoded += this.TPMS_Request_ResponseDecoded;
			OBDRequest obdrequestForUnit7 = ToyotaUnitsDetector.GetOBDRequestForUnit("2113", "750", "2A");
			obdrequestForUnit7.ResponseDecoded += this.TPMS_Request_ResponseDecoded;
			OBDRequest[] array = new OBDRequest[] { obdrequestForUnit, obdrequestForUnit2, obdrequestForUnit3, obdrequestForUnit4, obdrequestForUnit5, obdrequestForUnit6, obdrequestForUnit7 };
			App.OBDReader.ReplaceQueue(array);
			await App.OBDReader.WaitForCommandQueue();
			if (this.A801Items.Count > 0 && this.A803Items.Count > 0)
			{
				if (this.A803Items.FirstOrDefault((ToyotaA8Item x) => x.IdHex == "0E") != null && this.SensorItems.Count > 0)
				{
					return CodingRequestResult.Success;
				}
			}
			return CodingRequestResult.UnknownError;
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x003F28DC File Offset: 0x003F0ADC
		[CompilerGenerated]
		private void <UpdateCurrentState>b__8_0(OBDRequest req, string response)
		{
			List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, "2A", response);
			this.A801Items.AddRange(list);
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x003F2904 File Offset: 0x003F0B04
		[CompilerGenerated]
		private void <UpdateCurrentState>b__8_1(OBDRequest req, string response)
		{
			List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, "2A", response);
			this.A803Items.AddRange(list);
		}

		// Token: 0x0400317B RID: 12667
		private List<ToyotaA8Item> A801Items = new List<ToyotaA8Item>();

		// Token: 0x0400317C RID: 12668
		private List<ToyotaA8Item> A803Items = new List<ToyotaA8Item>();

		// Token: 0x0400317D RID: 12669
		[CompilerGenerated]
		private ObservableCollection<ToyotaTPMSSensorItem> <SensorItems>k__BackingField;

		// Token: 0x020009E8 RID: 2536
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600519C RID: 20892 RVA: 0x003F292A File Offset: 0x003F0B2A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600519D RID: 20893 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600519E RID: 20894 RVA: 0x003F2936 File Offset: 0x003F0B36
			internal bool <UpdateCurrentState>b__8_2(ToyotaA8Item x)
			{
				return x.IdHex == "0E";
			}

			// Token: 0x0400317E RID: 12670
			public static readonly ToyotaTPMSSensorModel.<>c <>9 = new ToyotaTPMSSensorModel.<>c();

			// Token: 0x0400317F RID: 12671
			public static Func<ToyotaA8Item, bool> <>9__8_2;
		}

		// Token: 0x020009E9 RID: 2537
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600519F RID: 20895 RVA: 0x003F2948 File Offset: 0x003F0B48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ToyotaTPMSSensorModel toyotaTPMSSensorModel = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						toyotaTPMSSensorModel.A801Items.Clear();
						OBDRequest obdrequestForUnit = ToyotaUnitsDetector.GetOBDRequestForUnit("A801", "750", "2A");
						obdrequestForUnit.ResponseReceived += delegate(OBDRequest req, string response)
						{
							List<ToyotaA8Item> list = ToyotaA8Parser.ParseResponse(req, "2A", response);
							toyotaTPMSSensorModel.A801Items.AddRange(list);
						};
						toyotaTPMSSensorModel.A803Items.Clear();
						OBDRequest obdrequestForUnit2 = ToyotaUnitsDetector.GetOBDRequestForUnit("A803", "750", "2A");
						obdrequestForUnit2.ResponseReceived += delegate(OBDRequest req, string response)
						{
							List<ToyotaA8Item> list2 = ToyotaA8Parser.ParseResponse(req, "2A", response);
							toyotaTPMSSensorModel.A803Items.AddRange(list2);
						};
						toyotaTPMSSensorModel.SensorItems.Clear();
						OBDRequest obdrequestForUnit3 = ToyotaUnitsDetector.GetOBDRequestForUnit("210F", "750", "2A");
						obdrequestForUnit3.ResponseDecoded += toyotaTPMSSensorModel.TPMS_Request_ResponseDecoded;
						OBDRequest obdrequestForUnit4 = ToyotaUnitsDetector.GetOBDRequestForUnit("2110", "750", "2A");
						obdrequestForUnit4.ResponseDecoded += toyotaTPMSSensorModel.TPMS_Request_ResponseDecoded;
						OBDRequest obdrequestForUnit5 = ToyotaUnitsDetector.GetOBDRequestForUnit("2111", "750", "2A");
						obdrequestForUnit5.ResponseDecoded += toyotaTPMSSensorModel.TPMS_Request_ResponseDecoded;
						OBDRequest obdrequestForUnit6 = ToyotaUnitsDetector.GetOBDRequestForUnit("2112", "750", "2A");
						obdrequestForUnit6.ResponseDecoded += toyotaTPMSSensorModel.TPMS_Request_ResponseDecoded;
						OBDRequest obdrequestForUnit7 = ToyotaUnitsDetector.GetOBDRequestForUnit("2113", "750", "2A");
						obdrequestForUnit7.ResponseDecoded += toyotaTPMSSensorModel.TPMS_Request_ResponseDecoded;
						OBDRequest[] array = new OBDRequest[] { obdrequestForUnit, obdrequestForUnit2, obdrequestForUnit3, obdrequestForUnit4, obdrequestForUnit5, obdrequestForUnit6, obdrequestForUnit7 };
						App.OBDReader.ReplaceQueue(array);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ToyotaTPMSSensorModel.<UpdateCurrentState>d__8>(ref taskAwaiter, ref this);
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
					if (toyotaTPMSSensorModel.A801Items.Count > 0 && toyotaTPMSSensorModel.A803Items.Count > 0)
					{
						if (toyotaTPMSSensorModel.A803Items.FirstOrDefault((ToyotaA8Item x) => x.IdHex == "0E") != null && toyotaTPMSSensorModel.SensorItems.Count > 0)
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_0261;
						}
					}
					codingRequestResult = CodingRequestResult.UnknownError;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0261:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060051A0 RID: 20896 RVA: 0x003F2BE8 File Offset: 0x003F0DE8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003180 RID: 12672
			public int <>1__state;

			// Token: 0x04003181 RID: 12673
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003182 RID: 12674
			public ToyotaTPMSSensorModel <>4__this;

			// Token: 0x04003183 RID: 12675
			private TaskAwaiter <>u__1;
		}
	}
}
