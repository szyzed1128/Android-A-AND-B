using System;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MLB_EVO
{
	// Token: 0x02000B79 RID: 2937
	internal class AT
	{
		// Token: 0x06005A3F RID: 23103 RVA: 0x004310A8 File Offset: 0x0042F2A8
		public static ICodingContainer ZH8HP_ResetSystemSpecificAdaptationValues()
		{
			return new MQBSimpleOperationWith0102StatusCheck("AT (8HP): Reset system-specific adaptation values", "WARNING! You can't undo this operation! Use at your own risk!", "", "02", "31010210040000", "31020210", "", true)
			{
				PasswordHint = "",
				Translations = 
				{
					new TranslationItem("ru", "AT (8HP): Сброс всех настроечных значений", "Внимание! Эта операция необратима. Используйте на свой страх и риск!", "")
				},
				Group = CodingGroup.ServiceProcedures
			};
		}

		// Token: 0x06005A40 RID: 23104 RVA: 0x00431118 File Offset: 0x0042F318
		public static ICodingContainer ZH8HP_QuickAdaptation()
		{
			return new MQBSimpleOperationWith0102StatusCheck("AT (8HP): Quick adaptation", "WARNING! You can't undo this operation! Use at your own risk!", "", "02", "310104BA040000", "310204BA", "", true)
			{
				PasswordHint = "",
				InnerDescription = "Prerequisites:\n-ATF temperature is warm (>40 C)\n-Engine is running\n-Parking brake applied\n-Selector position: D\n-Brake pedal applied.\nTime: ~5 minutes.\nIf you run this operation twice during the same ignition cycle, adaptation data is not reset, but optimized.",
				Translations = 
				{
					new TranslationItem("ru", "AT (8HP): Быстрая адаптация", "Внимание! Эта операция необратима. Используйте на свой страх и риск!", "Условия:\n-КП прогрета\n-двигатель работает\n-стояночный тормоз задействован\n-положение селектора: D\n-педаль тормоза нажата.\nПримерное время выполнения: ~ 5 минут\nЕсли адаптацмия повторяется в том же цикле зажигания (без промежуточного выключения), то полученные ранее значения адаптаций не удаляются, а оптимизируются")
				},
				Group = CodingGroup.ServiceProcedures
			};
		}

		// Token: 0x06005A41 RID: 23105 RVA: 0x00002050 File Offset: 0x00000250
		public AT()
		{
		}
	}
}
