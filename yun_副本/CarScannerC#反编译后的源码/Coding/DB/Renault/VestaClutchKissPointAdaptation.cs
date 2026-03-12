using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A17 RID: 2583
	internal class VestaClutchKissPointAdaptation : VestaAMTProcedure
	{
		// Token: 0x0600525C RID: 21084 RVA: 0x003F8686 File Offset: 0x003F6886
		public VestaClutchKissPointAdaptation()
			: base("19", "3. Процедура обучения точки касания сцепления (для коробки передач АМТ)", "", "Функция обучения точки касания измеряет и сохраняет значение положения актуатора выключения сцепления, соответствующее частично включенному сцеплению, при котором появляется сигнал определенной частоты с датчика частоты вращения первичного вала коробки передач.\nДлительность процедуры: около 15 секунд.\nПосле завершения процедуры необходимо выключить зажигание и подождать не менее 60 секунд (до отключения главного реле) для корректного сохранения результатов в памяти блока управления!\nУсловия для выполнения процедуры:\n-двигатель работает в режиме холостого хода;\n-автомобиль находится в неподвижном состоянии;\n-сигнал ДЧВПВ = 0 об / мин;\n-коробка передач в нейтральном положении;\n-нет активных кодов неисправностей;\n-сигнализатор диагностики АМТ неактивен.\nПЕРЕД ЗАПУСКОМ НАЖМИТЕ И ДЕРЖИТЕ НАЖАТОЙ ПЕДАЛЬ ТОРМОЗА ДО ОКОНЧАНИЯ ВЫПОЛНЕНИЯ ПРОЦЕДУРЫ!")
		{
			base.HasCurrentState = true;
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x003F86AC File Offset: 0x003F68AC
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			string text = string.Concat(new string[] { "ATSH", this.RequestHeader, ";ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", this.ATST, ";ATCRA", this.ResponseHeader });
			string text2 = "ATAR;ATFCSM0";
			CustomPID customPID = new CustomPID("", "", "222E0C", "7E1", "((A*256+B)+35340)/93", UnitsHelper.Units.None, 491.0, 810.0, text, text2, false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
			OBDRequest obdrequest = new OBDRequest("222E0C", this.RequestHeader, text, text2, false, customPID);
			customPID.ValueChanged += this.Pid_ValueChanged;
			OBDRequest obdrequest2 = new OBDRequest("10C0", this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest });
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.Success;
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x003F86F0 File Offset: 0x003F68F0
		private void Pid_ValueChanged(object sender, PID e)
		{
			CustomPID customPID = (CustomPID)e;
			base.CurrentState = "Текущее значение обучения = " + customPID.Value.ToString("0.00");
		}

		// Token: 0x02000A18 RID: 2584
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600525F RID: 21087 RVA: 0x003F8728 File Offset: 0x003F6928
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VestaClutchKissPointAdaptation vestaClutchKissPointAdaptation = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						vestaClutchKissPointAdaptation.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						string text = string.Concat(new string[] { "ATSH", vestaClutchKissPointAdaptation.RequestHeader, ";ATFCSH", vestaClutchKissPointAdaptation.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", vestaClutchKissPointAdaptation.ATST, ";ATCRA", vestaClutchKissPointAdaptation.ResponseHeader });
						string text2 = "ATAR;ATFCSM0";
						CustomPID customPID = new CustomPID("", "", "222E0C", "7E1", "((A*256+B)+35340)/93", UnitsHelper.Units.None, 491.0, 810.0, text, text2, false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
						OBDRequest obdrequest = new OBDRequest("222E0C", vestaClutchKissPointAdaptation.RequestHeader, text, text2, false, customPID);
						customPID.ValueChanged += vestaClutchKissPointAdaptation.Pid_ValueChanged;
						OBDRequest obdrequest2 = new OBDRequest("10C0", vestaClutchKissPointAdaptation.RequestHeader, text, text2, false)
						{
							ELMFormat = ELMFormat.CAN11bit
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VestaClutchKissPointAdaptation.<UpdateCurrentState>d__1>(ref taskAwaiter, ref this);
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

			// Token: 0x06005260 RID: 21088 RVA: 0x003F8914 File Offset: 0x003F6B14
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003245 RID: 12869
			public int <>1__state;

			// Token: 0x04003246 RID: 12870
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003247 RID: 12871
			public VestaClutchKissPointAdaptation <>4__this;

			// Token: 0x04003248 RID: 12872
			private TaskAwaiter <>u__1;
		}
	}
}
