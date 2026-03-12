using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BCC RID: 3020
	internal class SportageQLThrottleAdaptationReset : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005B24 RID: 23332 RVA: 0x004371A4 File Offset: 0x004353A4
		public SportageQLThrottleAdaptationReset()
		{
			base.Name = "Reset throttle adaptation";
			base.Description = "Compatibility confirmed with: Kia Sportage QL (gasoline)";
			base.InnerDescription = "With high possibility this would be compatible with most of Hyundai/Kia cars from ~2009 to ~2015." + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Сброс адаптации дроссельной заслонки", "Совместимость подтверждена с Kia Sportage QL", "С большой долей вероятности совместимо с большинством моделей Hyundai/Kia ~2009 .. ~2015 гг." + SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Reset adaptations", "", new TranslationItem[]
			{
				new TranslationItem("ru", "Сброс адаптаций", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.EngineAndPowertrain;
			this.HasCurrentState = false;
		}

		// Token: 0x06005B25 RID: 23333 RVA: 0x00437294 File Offset: 0x00435494
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			byte[] array = new byte[]
			{
				177, 178, 179, 180, 181, 182, 183, 184, 186, 187,
				188, 189, 190, 191, 192, 193, 194, 195, 196, 197,
				198, 199, 200, 201, 202, 204, 205, 206, 207, 208,
				209, 210, 211
			};
			List<OBDRequest> list = new List<OBDRequest>(array.Length + 1);
			OBDRequest obdrequest = new OBDRequest("1090", "7E0", "", "", false);
			list.Add(obdrequest);
			foreach (byte b in array)
			{
				OBDRequest obdrequest2 = new OBDRequest("30" + b.ToString("X2") + "04", "7E0", "", "", false);
				list.Add(obdrequest2);
			}
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.Success;
		}

		// Token: 0x02000BCD RID: 3021
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005B26 RID: 23334 RVA: 0x004372D0 File Offset: 0x004354D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						byte[] array = new byte[]
						{
							177, 178, 179, 180, 181, 182, 183, 184, 186, 187,
							188, 189, 190, 191, 192, 193, 194, 195, 196, 197,
							198, 199, 200, 201, 202, 204, 205, 206, 207, 208,
							209, 210, 211
						};
						List<OBDRequest> list = new List<OBDRequest>(array.Length + 1);
						OBDRequest obdrequest = new OBDRequest("1090", "7E0", "", "", false);
						list.Add(obdrequest);
						foreach (byte b in array)
						{
							OBDRequest obdrequest2 = new OBDRequest("30" + b.ToString("X2") + "04", "7E0", "", "", false);
							list.Add(obdrequest2);
						}
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SportageQLThrottleAdaptationReset.<Execute>d__1>(ref taskAwaiter, ref this);
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
					codingRequestResult = CodingRequestResult.Success;
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

			// Token: 0x06005B27 RID: 23335 RVA: 0x00437438 File Offset: 0x00435638
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003979 RID: 14713
			public int <>1__state;

			// Token: 0x0400397A RID: 14714
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400397B RID: 14715
			private TaskAwaiter <>u__1;
		}
	}
}
