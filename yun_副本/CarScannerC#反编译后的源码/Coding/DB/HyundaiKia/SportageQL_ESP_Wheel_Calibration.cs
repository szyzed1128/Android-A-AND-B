using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BCE RID: 3022
	internal class SportageQL_ESP_Wheel_Calibration : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005B28 RID: 23336 RVA: 0x00437448 File Offset: 0x00435648
		public SportageQL_ESP_Wheel_Calibration()
		{
			base.Name = "SAS (steering wheel position sensor) calibration in ESP system";
			base.Description = "Compatibility confirmed with: Kia Sportage QL";
			base.InnerDescription = "With high possibility this would be compatible with many other Hyundai/Kia cars from ~2009 to ~2018WARNING! Experimental feature! If it's not compatible with your car you'll get uncalibrated power steering and different DTC! Think twice before using it!\n" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Калибровка SAS (датчик угла поворота руля) в системе ESP", "Совместимость подтверждена с Kia Sportage QL", "С большой долей вероятности совместимо и с другими моделями Hyundai/Kia ~2009 .. ~2018 гг.\nВНИМАНИЕ! Функция экспериментальная и если она не подходит к вашему автомобилю - будет плохо. Подумайте, прежде чем использовать." + SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7D1";
			base.ResponseHeader = "7D9";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Start", "", new TranslationItem[]
			{
				new TranslationItem("ru", "Запуск", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.EngineAndPowertrain;
			this.HasCurrentState = false;
		}

		// Token: 0x06005B29 RID: 23337 RVA: 0x00437538 File Offset: 0x00435738
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult result = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("1003", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest("3101AC01", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (this.CheckForSuccessfullResponse(req, data))
				{
					result = CodingRequestResult.Success;
				}
			};
			obdrequest2.ResponseMarker = "7101";
			OBDRequest obdrequest3 = new OBDRequest("3101AC02", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (this.CheckForSuccessfullResponse(req, data))
				{
					result = CodingRequestResult.Success;
				}
			};
			obdrequest3.ResponseMarker = "7101";
			OBDRequest obdrequest4 = new OBDRequest("14FFFFFF", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest5 = new OBDRequest("20", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4, obdrequest5 });
			await App.OBDReader.WaitForCommandQueue();
			return result;
		}

		// Token: 0x02000BCF RID: 3023
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005B2A RID: 23338 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005B2B RID: 23339 RVA: 0x0043757B File Offset: 0x0043577B
			internal void <Execute>b__0(OBDRequest req, string data)
			{
				if (this.<>4__this.CheckForSuccessfullResponse(req, data))
				{
					this.result = CodingRequestResult.Success;
				}
			}

			// Token: 0x06005B2C RID: 23340 RVA: 0x0043757B File Offset: 0x0043577B
			internal void <Execute>b__1(OBDRequest req, string data)
			{
				if (this.<>4__this.CheckForSuccessfullResponse(req, data))
				{
					this.result = CodingRequestResult.Success;
				}
			}

			// Token: 0x0400397C RID: 14716
			public SportageQL_ESP_Wheel_Calibration <>4__this;

			// Token: 0x0400397D RID: 14717
			public CodingRequestResult result;
		}

		// Token: 0x02000BD0 RID: 3024
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005B2D RID: 23341 RVA: 0x00437594 File Offset: 0x00435794
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SportageQL_ESP_Wheel_Calibration sportageQL_ESP_Wheel_Calibration = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new SportageQL_ESP_Wheel_Calibration.<>c__DisplayClass1_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.result = CodingRequestResult.UnknownError;
						sportageQL_ESP_Wheel_Calibration.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("1003", sportageQL_ESP_Wheel_Calibration.RequestHeader, sportageQL_ESP_Wheel_Calibration.BeforeCommands, sportageQL_ESP_Wheel_Calibration.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("3101AC01", sportageQL_ESP_Wheel_Calibration.RequestHeader, sportageQL_ESP_Wheel_Calibration.BeforeCommands, sportageQL_ESP_Wheel_Calibration.AfterCommands, false);
						obdrequest2.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (CS$<>8__locals1.<>4__this.CheckForSuccessfullResponse(req, data))
							{
								CS$<>8__locals1.result = CodingRequestResult.Success;
							}
						};
						obdrequest2.ResponseMarker = "7101";
						OBDRequest obdrequest3 = new OBDRequest("3101AC02", sportageQL_ESP_Wheel_Calibration.RequestHeader, sportageQL_ESP_Wheel_Calibration.BeforeCommands, sportageQL_ESP_Wheel_Calibration.AfterCommands, false);
						obdrequest3.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (CS$<>8__locals1.<>4__this.CheckForSuccessfullResponse(req, data))
							{
								CS$<>8__locals1.result = CodingRequestResult.Success;
							}
						};
						obdrequest3.ResponseMarker = "7101";
						OBDRequest obdrequest4 = new OBDRequest("14FFFFFF", sportageQL_ESP_Wheel_Calibration.RequestHeader, sportageQL_ESP_Wheel_Calibration.BeforeCommands, sportageQL_ESP_Wheel_Calibration.AfterCommands, false);
						OBDRequest obdrequest5 = new OBDRequest("20", sportageQL_ESP_Wheel_Calibration.RequestHeader, sportageQL_ESP_Wheel_Calibration.BeforeCommands, sportageQL_ESP_Wheel_Calibration.AfterCommands, false);
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3, obdrequest4, obdrequest5 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SportageQL_ESP_Wheel_Calibration.<Execute>d__1>(ref taskAwaiter, ref this);
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
					result = CS$<>8__locals1.result;
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
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06005B2E RID: 23342 RVA: 0x004377B4 File Offset: 0x004359B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400397E RID: 14718
			public int <>1__state;

			// Token: 0x0400397F RID: 14719
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003980 RID: 14720
			public SportageQL_ESP_Wheel_Calibration <>4__this;

			// Token: 0x04003981 RID: 14721
			private SportageQL_ESP_Wheel_Calibration.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x04003982 RID: 14722
			private TaskAwaiter <>u__1;
		}
	}
}
