using System;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B16 RID: 2838
	internal class MQBCantonDataSetMIB2 : MQBAudioDataSetBase
	{
		// Token: 0x06005858 RID: 22616 RVA: 0x00422594 File Offset: 0x00420794
		public MQBCantonDataSetMIB2()
			: base("vag.cantonmib2")
		{
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("47");
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("47");
			this.RequiresPro = true;
			base.Name = "Additional amplifier (CANTON, Fender, Dynaudio, etc.) sound processing preset (MIB2)";
			base.Description = "This feature changes sound configuration in additional amplifier (unit 47) to improve sound quality.\nWARNING! It takes about 3-5 minutes to read current data and 5-15 minutes to write new value!";
			base.InnerDescription = "This feature requires high quality ELM327! With low quality ELM327 there's a high possibility to break data saving process.";
			base.Address = 851968;
			base.DataLength = 65536;
			base.DataLengthFormatLength = 4;
			base.AddressFormatLength = 4;
			this.ATST = "16";
			base.Translations.Add(new TranslationItem("ru", "Предустановки обработки звука усилителем дополнительной акустической системы Canton, Fender, Dynaudio и т.п. (MIB2)", "Этот пункт изменяет предустановки, ответственные за обработку звука (\"параметрию\") усилителем дополнительной акустической системы (блок 47).\nВНИМАНИЕ! Чтение текущего значения занимает 3-5 минут. Запись нового значения занимает 5-15 минут.", "\nДля корректной работы этого пункта необходим качественный адаптер ELM327! С некачественным адаптером есть высокий шанс того, что запись будет прервана в середине процесса.\nПеред записью нового значения убедитесь, что этот процесс не будет прерван (входящим звонком или другим фактором)."));
			this.allowWhileEngineRunning = true;
			this.allowExecuteWithEmptyOriginalData = false;
		}
	}
}
