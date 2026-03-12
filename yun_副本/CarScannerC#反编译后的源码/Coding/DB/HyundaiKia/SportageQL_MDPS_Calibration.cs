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
	// Token: 0x02000BD1 RID: 3025
	internal class SportageQL_MDPS_Calibration : MQBAdaptationTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005B2F RID: 23343 RVA: 0x004377C4 File Offset: 0x004359C4
		public SportageQL_MDPS_Calibration()
		{
			base.Name = "ASP (steering wheel position sensor) calibration  in power steering unit for Mando C-Type and R-Type";
			base.Description = "Compatibility confirmed with: Kia Sportage QL";
			base.InnerDescription = "With high possibility this would be compatible with many other Hyundai/Kia cars from ~2009 to ~2018WARNING! Experimental feature! If it's not compatible with your car you'll get uncalibrated power steering and different DTC! Think twice before using it!\n" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.Translations.Add(new TranslationItem("ru", "Калибровка ASP (датчик угла поворота руля) в системе ЭУР (для Mando C-Type и R-Type)", "Совместимость подтверждена с Kia Sportage QL", "С большой долей вероятности совместимо и с другими моделями Hyundai/Kia ~2009 .. ~2018 гг.\nВНИМАНИЕ! Функция экспериментальная и если она не подходит к вашему автомобилю - будет плохо. Подумайте, прежде чем использовать." + SupportedItemsDetectorBase.GetAccessKeyWarningRu));
			base.RequestHeader = "7D4";
			base.ResponseHeader = "7DC";
			base.ValueType = AdaptationValueTypes.OptionType;
			base.Options.Add(new MQBAdaptationOption("Start", "", new TranslationItem[]
			{
				new TranslationItem("ru", "Запуск", "", "")
			}));
			this.PasswordVisible = false;
			base.PasswordHint = "";
			base.CurrentState = "";
			base.Group = CodingGroup.ServiceProcedures;
			this.HasCurrentState = false;
		}

		// Token: 0x06005B30 RID: 23344 RVA: 0x004378B8 File Offset: 0x00435AB8
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult result = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("1090", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest2 = new OBDRequest("31010000", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (this.CheckForSuccessfullResponse(req, data))
				{
					result = CodingRequestResult.Success;
				}
			};
			obdrequest2.ResponseMarker = "7101";
			OBDRequest obdrequest3 = new OBDRequest("310100FF", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (this.CheckForSuccessfullResponse(req, data))
				{
					result = CodingRequestResult.Success;
				}
			};
			obdrequest3.ResponseMarker = "7101";
			OBDRequest obdrequest4 = new OBDRequest("1101", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest5 = new OBDRequest("3E00", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				DoNotDecode = true
			};
			OBDRequest obdrequest6 = new OBDRequest("144000", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest obdrequest7 = new OBDRequest("20", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest request = new SeedKeySteeringDB().GetRequest(this.BeforeCommands, this.AfterCommands);
			App.OBDReader.ReplaceQueue(new OBDRequest[]
			{
				obdrequest, obdrequest2, obdrequest3, request, obdrequest4, obdrequest5, obdrequest5, obdrequest5, obdrequest5, obdrequest5,
				obdrequest5, obdrequest5, obdrequest5, obdrequest5, obdrequest6, obdrequest7
			});
			await App.OBDReader.WaitForCommandQueue();
			return result;
		}

		// Token: 0x02000BD2 RID: 3026
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06005B31 RID: 23345 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06005B32 RID: 23346 RVA: 0x004378FB File Offset: 0x00435AFB
			internal void <Execute>b__0(OBDRequest req, string data)
			{
				if (this.<>4__this.CheckForSuccessfullResponse(req, data))
				{
					this.result = CodingRequestResult.Success;
				}
			}

			// Token: 0x06005B33 RID: 23347 RVA: 0x004378FB File Offset: 0x00435AFB
			internal void <Execute>b__1(OBDRequest req, string data)
			{
				if (this.<>4__this.CheckForSuccessfullResponse(req, data))
				{
					this.result = CodingRequestResult.Success;
				}
			}

			// Token: 0x04003983 RID: 14723
			public SportageQL_MDPS_Calibration <>4__this;

			// Token: 0x04003984 RID: 14724
			public CodingRequestResult result;
		}

		// Token: 0x02000BD3 RID: 3027
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__1 : IAsyncStateMachine
		{
			// Token: 0x06005B34 RID: 23348 RVA: 0x00437914 File Offset: 0x00435B14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SportageQL_MDPS_Calibration sportageQL_MDPS_Calibration = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new SportageQL_MDPS_Calibration.<>c__DisplayClass1_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.result = CodingRequestResult.UnknownError;
						sportageQL_MDPS_Calibration.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("1090", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						OBDRequest obdrequest2 = new OBDRequest("31010000", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						obdrequest2.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (CS$<>8__locals1.<>4__this.CheckForSuccessfullResponse(req, data))
							{
								CS$<>8__locals1.result = CodingRequestResult.Success;
							}
						};
						obdrequest2.ResponseMarker = "7101";
						OBDRequest obdrequest3 = new OBDRequest("310100FF", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						obdrequest3.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (CS$<>8__locals1.<>4__this.CheckForSuccessfullResponse(req, data))
							{
								CS$<>8__locals1.result = CodingRequestResult.Success;
							}
						};
						obdrequest3.ResponseMarker = "7101";
						OBDRequest obdrequest4 = new OBDRequest("1101", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						OBDRequest obdrequest5 = new OBDRequest("3E00", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false)
						{
							DoNotDecode = true
						};
						OBDRequest obdrequest6 = new OBDRequest("144000", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						OBDRequest obdrequest7 = new OBDRequest("20", sportageQL_MDPS_Calibration.RequestHeader, sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands, false);
						OBDRequest request = new SeedKeySteeringDB().GetRequest(sportageQL_MDPS_Calibration.BeforeCommands, sportageQL_MDPS_Calibration.AfterCommands);
						App.OBDReader.ReplaceQueue(new OBDRequest[]
						{
							obdrequest, obdrequest2, obdrequest3, request, obdrequest4, obdrequest5, obdrequest5, obdrequest5, obdrequest5, obdrequest5,
							obdrequest5, obdrequest5, obdrequest5, obdrequest5, obdrequest6, obdrequest7
						});
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SportageQL_MDPS_Calibration.<Execute>d__1>(ref taskAwaiter, ref this);
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

			// Token: 0x06005B35 RID: 23349 RVA: 0x00437BD0 File Offset: 0x00435DD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003985 RID: 14725
			public int <>1__state;

			// Token: 0x04003986 RID: 14726
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003987 RID: 14727
			public SportageQL_MDPS_Calibration <>4__this;

			// Token: 0x04003988 RID: 14728
			private SportageQL_MDPS_Calibration.<>c__DisplayClass1_0 <>8__1;

			// Token: 0x04003989 RID: 14729
			private TaskAwaiter <>u__1;
		}
	}
}
