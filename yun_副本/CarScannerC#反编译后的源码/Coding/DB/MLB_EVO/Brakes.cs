using System;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MLB_EVO
{
	// Token: 0x02000B7A RID: 2938
	internal static class Brakes
	{
		// Token: 0x06005A42 RID: 23106 RVA: 0x00431190 File Offset: 0x0042F390
		public static ICodingContainer BrakePressureSensorBasicAdaptation()
		{
			return new MQBSimpleOperationWith0102StatusCheck("Brake pedal position sensor: reset zero point (basic setting)", "", "", "03", "31010595040000", "31020595", "", true)
			{
				PasswordHint = "40168, 38561, 70392, 11820",
				Translations = 
				{
					new TranslationItem("ru", "Датчик положения педали тормоза: сброс нулевого положения", "", "")
				},
				Group = CodingGroup.Brakes
			};
		}
	}
}
