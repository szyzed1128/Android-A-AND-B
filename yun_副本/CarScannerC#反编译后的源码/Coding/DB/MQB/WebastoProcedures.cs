using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B78 RID: 2936
	internal class WebastoProcedures
	{
		// Token: 0x06005A3C RID: 23100 RVA: 0x00430FE0 File Offset: 0x0042F1E0
		internal static ICodingContainer DisengageHeater()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Disengage aux. heater (webasto)", "", "This procedure is often used to unlock additional heater", "18", "3101047F040000", "3102047F", "", false)
			{
				Group = CodingGroup.Climate,
				Translations = 
				{
					new TranslationItem("ru", "Отключение дополнительного отопителя (webasto)", "", "Эта процедура часто используется для того, чтобы разблокировать дополнительный отопитель. Используйте с осторожностью!")
				}
			};
		}

		// Token: 0x06005A3D RID: 23101 RVA: 0x00431044 File Offset: 0x0042F244
		internal static ICodingContainer UnlockHeater()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Unlock aux. heater (webasto)", "", "Warning! Most of users recommend using Disengage heater procedure to unlock it.", "18", "31010569040000", "31020569", "", false)
			{
				Group = CodingGroup.Climate,
				Translations = 
				{
					new TranslationItem("ru", "Разблокировка дополнительного отопителя (webasto)", "", "ВНИМАНИЕ! Большинство пользователей рекомендует использовать другую процедуру для разблокировки отопителя - Отключение дополнительного отопителя. Используйте с осторожностью!")
				}
			};
		}

		// Token: 0x06005A3E RID: 23102 RVA: 0x00002050 File Offset: 0x00000250
		public WebastoProcedures()
		{
		}
	}
}
