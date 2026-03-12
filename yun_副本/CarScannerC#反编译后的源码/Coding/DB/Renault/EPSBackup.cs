using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x020009F7 RID: 2551
	internal class EPSBackup : CustomizableCodingTemplate
	{
		// Token: 0x060051C1 RID: 20929 RVA: 0x003F3D68 File Offset: 0x003F1F68
		public EPSBackup()
		{
			base.Name = "EPS Configuration Backup";
			base.InnerDescription = "This creates 15 items in Coding History, that you can use to restore your EPS module configuration";
			base.RequestHeader = "742";
			base.ResponseHeader = "762";
			this.Group = CodingGroup.Other;
			this.HasCurrentState = false;
			this.PasswordVisible = false;
			this.Options.Add(new MQBAdaptationOption("CREATE BACKUP", "00", new TranslationItem[]
			{
				new TranslationItem("ru", "СОЗДАТЬ РЕЗЕРВНУЮ КОПИЮ", "", "")
			}));
			TranslationItem translationItem = new TranslationItem("ru", "Резервное копирование конфигурации EPS (ЭУР)", "", "");
			translationItem.AdditionalText = "Этот пункт создает в Истории кодирования 15 пунктов с настройками блока ЭУР. Для восстановления воспользуйтесь историей кодирование и восстановите все 15 пунктов.";
			base.Translations.Add(translationItem);
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x003F3EB4 File Offset: 0x003F20B4
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			for (int i = 0; i < this.addresses.Length; i++)
			{
				string addr = this.addresses[i];
				CustomizableCodingContainerBackupBase customizableCodingContainerBackupBase = new CustomizableCodingContainerBackupBase();
				customizableCodingContainerBackupBase.RequestHeader = base.RequestHeader;
				customizableCodingContainerBackupBase.ResponseHeader = base.ResponseHeader;
				customizableCodingContainerBackupBase.Name = base.Name + " [" + addr + "]";
				customizableCodingContainerBackupBase.MakeChangesToInitialData = true;
				customizableCodingContainerBackupBase.PasswordVisible = false;
				customizableCodingContainerBackupBase.Protocol = "6";
				customizableCodingContainerBackupBase.OpenSessionCommand = "1003";
				customizableCodingContainerBackupBase.PostWriteCommands = "1101";
				customizableCodingContainerBackupBase.ReadModeAndAddress = "22" + addr;
				customizableCodingContainerBackupBase.WriteModeAndAddress = "2E" + addr;
				if (progress != null)
				{
					progress.Report("Reading " + addr + "...");
				}
				CodingRequestResult codingRequestResult = await customizableCodingContainerBackupBase.Execute("", "", "", null, null, false);
				if (progress != null)
				{
					progress.Report("Reading " + addr + ": " + MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult));
				}
				if (codingRequestResult != CodingRequestResult.Success)
				{
					return codingRequestResult;
				}
				addr = null;
			}
			return CodingRequestResult.Success;
		}

		// Token: 0x040031A9 RID: 12713
		private string[] addresses = new string[]
		{
			"01C7", "01E7", "01C6", "01C3", "017D", "01D0", "01CE", "01CF", "012A", "01CB",
			"0168", "C000", "C200", "01CD", "01CA"
		};

		// Token: 0x020009F8 RID: 2552
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__2 : IAsyncStateMachine
		{
			// Token: 0x060051C3 RID: 20931 RVA: 0x003F3F00 File Offset: 0x003F2100
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				EPSBackup epsbackup = this;
				CodingRequestResult codingRequestResult;
				try
				{
					if (num != 0)
					{
						i = 0;
						goto IL_01AE;
					}
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					TaskAwaiter<CodingRequestResult> taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
					num2 = -1;
					IL_015A:
					CodingRequestResult result = taskAwaiter.GetResult();
					IProgress<string> progress = progress;
					if (progress != null)
					{
						progress.Report("Reading " + addr + ": " + MQBAdaptationTemplate.CodingRequestResultToString(result));
					}
					if (result != CodingRequestResult.Success)
					{
						codingRequestResult = result;
						goto IL_01DE;
					}
					addr = null;
					int num3 = i;
					i = num3 + 1;
					IL_01AE:
					if (i >= epsbackup.addresses.Length)
					{
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						addr = epsbackup.addresses[i];
						CustomizableCodingContainerBackupBase customizableCodingContainerBackupBase = new CustomizableCodingContainerBackupBase();
						customizableCodingContainerBackupBase.RequestHeader = epsbackup.RequestHeader;
						customizableCodingContainerBackupBase.ResponseHeader = epsbackup.ResponseHeader;
						customizableCodingContainerBackupBase.Name = epsbackup.Name + " [" + addr + "]";
						customizableCodingContainerBackupBase.MakeChangesToInitialData = true;
						customizableCodingContainerBackupBase.PasswordVisible = false;
						customizableCodingContainerBackupBase.Protocol = "6";
						customizableCodingContainerBackupBase.OpenSessionCommand = "1003";
						customizableCodingContainerBackupBase.PostWriteCommands = "1101";
						customizableCodingContainerBackupBase.ReadModeAndAddress = "22" + addr;
						customizableCodingContainerBackupBase.WriteModeAndAddress = "2E" + addr;
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report("Reading " + addr + "...");
						}
						taskAwaiter = customizableCodingContainerBackupBase.Execute("", "", "", null, null, false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, EPSBackup.<Execute>d__2>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_015A;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01DE:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060051C4 RID: 20932 RVA: 0x003F411C File Offset: 0x003F231C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031AA RID: 12714
			public int <>1__state;

			// Token: 0x040031AB RID: 12715
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040031AC RID: 12716
			public EPSBackup <>4__this;

			// Token: 0x040031AD RID: 12717
			public IProgress<string> progress;

			// Token: 0x040031AE RID: 12718
			private int <i>5__2;

			// Token: 0x040031AF RID: 12719
			private string <addr>5__3;

			// Token: 0x040031B0 RID: 12720
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}
	}
}
