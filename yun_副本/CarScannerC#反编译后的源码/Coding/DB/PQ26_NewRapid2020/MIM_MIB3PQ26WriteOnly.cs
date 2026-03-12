using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A31 RID: 2609
	internal class MIM_MIB3PQ26WriteOnly : VIM_MIB3v2
	{
		// Token: 0x060052E3 RID: 21219 RVA: 0x003FAB78 File Offset: 0x003F8D78
		public MIM_MIB3PQ26WriteOnly()
		{
			base.Name = "Mirror link in motion speed threshold (MIB3)";
			base.Description = "Use this to activate MirrorLink in motion";
			base.InnerDescription = "To activate MirrorLink in motion, set speed to 255 (on some cars - 180)\nPrerequisites:\n1) MIB3\n2) Ignition turned ON\n3) Engine not running\n";
			base.Translations.Clear();
			base.Translations.Add(new TranslationItem("ru", "Порог скорости отключения MirrorLink в движении (MIB3)", "Используйте для активации MirrorLink в движении", "ФУНКЦИЯ ЭКСПЕРИМЕНТАЛЬНАЯ! ПОМНИТЕ, ЧТО ВСЕ НА ВАШ СТРАХ И РИСК!\nВНИМАНИЕ! Чтение текущего состояния и возврат к предыдущему значению через историю кодирования не поддерживается!\nЧтобы активировать MirrorLink в движении, задайте скорость для отключения 255 (на некоторых автомобилях - 180)\nУсловия:\n1) MIB3\n2) Зажигание включено\n3) Двигатель не запущен"));
			this.HasCurrentState = false;
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x003FABE4 File Offset: 0x003F8DE4
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.CurrentState = "0";
			return CodingRequestResult.Success;
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x003FAC28 File Offset: 0x003F8E28
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.LastReadData = BitHelpers.ConvertHexToBytesX("5630303310052005FF000000000000000000300506020602FF00FF00000C00772089");
			byte speed = 0;
			CodingRequestResult codingRequestResult;
			if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
			{
				codingRequestResult = CodingRequestResult.WrongInputValue;
			}
			else
			{
				OBDRequest obdrequest = new OBDRequest("010C", false);
				bool engineIsRunning = false;
				obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
				{
					if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
					{
						engineIsRunning = true;
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
				await App.OBDReader.WaitForCommandQueue();
				if (engineIsRunning)
				{
					codingRequestResult = CodingRequestResult.WrongConditions;
				}
				else
				{
					byte[] array = new byte[this.LastReadData.Length - 4];
					Array.Copy(this.LastReadData, 0, array, 0, array.Length);
					array[22] = speed;
					for (int i = 4; i < array.Length - 4; i++)
					{
						if (array[i] == 6)
						{
							array[i] = speed;
						}
					}
					byte[] array2 = Crc32.Calculate(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					codingRequestResult = await this.WriteDataToECU(password, value, progress, this.LastReadData, text);
				}
			}
			return codingRequestResult;
		}

		// Token: 0x02000A32 RID: 2610
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060052E6 RID: 21222 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060052E7 RID: 21223 RVA: 0x003FAC84 File Offset: 0x003F8E84
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x040032A7 RID: 12967
			public bool engineIsRunning;
		}

		// Token: 0x02000A33 RID: 2611
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x060052E8 RID: 21224 RVA: 0x003FACA8 File Offset: 0x003F8EA8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MIM_MIB3PQ26WriteOnly mim_MIB3PQ26WriteOnly = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_01FA;
						}
						CS$<>8__locals1 = new MIM_MIB3PQ26WriteOnly.<>c__DisplayClass2_0();
						mim_MIB3PQ26WriteOnly.LastReadData = BitHelpers.ConvertHexToBytesX("5630303310052005FF000000000000000000300506020602FF00FF00000C00772089");
						speed = 0;
						if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out speed))
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_0224;
						}
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MIM_MIB3PQ26WriteOnly.<Execute>d__2>(ref taskAwaiter3, ref this);
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
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_0224;
					}
					byte[] array = new byte[mim_MIB3PQ26WriteOnly.LastReadData.Length - 4];
					Array.Copy(mim_MIB3PQ26WriteOnly.LastReadData, 0, array, 0, array.Length);
					array[22] = speed;
					for (int i = 4; i < array.Length - 4; i++)
					{
						if (array[i] == 6)
						{
							array[i] = speed;
						}
					}
					byte[] array2 = Crc32.Calculate(array);
					string text = BitHelpers.ByteArrayToHexString(array.Concat(array2).ToArray<byte>());
					taskAwaiter = mim_MIB3PQ26WriteOnly.WriteDataToECU(password, value, progress, mim_MIB3PQ26WriteOnly.LastReadData, text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MIM_MIB3PQ26WriteOnly.<Execute>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_01FA:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0224:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060052E9 RID: 21225 RVA: 0x003FAF10 File Offset: 0x003F9110
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040032A8 RID: 12968
			public int <>1__state;

			// Token: 0x040032A9 RID: 12969
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040032AA RID: 12970
			public MIM_MIB3PQ26WriteOnly <>4__this;

			// Token: 0x040032AB RID: 12971
			public string value;

			// Token: 0x040032AC RID: 12972
			private MIM_MIB3PQ26WriteOnly.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x040032AD RID: 12973
			public string password;

			// Token: 0x040032AE RID: 12974
			public IProgress<string> progress;

			// Token: 0x040032AF RID: 12975
			private byte <speed>5__2;

			// Token: 0x040032B0 RID: 12976
			private TaskAwaiter <>u__1;

			// Token: 0x040032B1 RID: 12977
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000A34 RID: 2612
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__1 : IAsyncStateMachine
		{
			// Token: 0x060052EA RID: 21226 RVA: 0x003FAF20 File Offset: 0x003F9120
			void IAsyncStateMachine.MoveNext()
			{
				MIM_MIB3PQ26WriteOnly mim_MIB3PQ26WriteOnly = this;
				CodingRequestResult codingRequestResult;
				try
				{
					mim_MIB3PQ26WriteOnly.CurrentState = "0";
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060052EB RID: 21227 RVA: 0x003FAF80 File Offset: 0x003F9180
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040032B2 RID: 12978
			public int <>1__state;

			// Token: 0x040032B3 RID: 12979
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040032B4 RID: 12980
			public MIM_MIB3PQ26WriteOnly <>4__this;
		}
	}
}
