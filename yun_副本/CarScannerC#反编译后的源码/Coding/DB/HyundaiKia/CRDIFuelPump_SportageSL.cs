using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B8A RID: 2954
	internal class CRDIFuelPump_SportageSL : CRDIFuelPump_SorentoUMFL
	{
		// Token: 0x06005A71 RID: 23153 RVA: 0x00432880 File Offset: 0x00430A80
		public CRDIFuelPump_SportageSL()
		{
			base.Name = "Fuel system air remove (gen.2)";
			base.Description = "Compatibility: Hyundai/KIA with CRDI ~2009 .. ~2018";
			base.InnerDescription = SupportedItemsDetectorBase.GetAccessKeyWarning;
			this.startPumpCmd = "31860201";
			this.continuePumpCmd = "318601";
			this.openSessionCmd = "1090";
			base.Translations.Clear();
			base.Translations.Add(new TranslationItem("ru", "Удаление воздуха из топливой системы (прокачка топливной системы). Поколение 2", "Совместимость: Hyundai/Kia с дизельными двигателями ~2009 .. ~2016", SupportedItemsDetectorBase.GetAccessKeyWarningRu));
		}
	}
}
