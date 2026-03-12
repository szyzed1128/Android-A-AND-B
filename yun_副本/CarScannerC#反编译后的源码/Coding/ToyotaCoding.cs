using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000913 RID: 2323
	internal class ToyotaCoding : KWP2000Coding
	{
		// Token: 0x06004D9A RID: 19866 RVA: 0x003A0538 File Offset: 0x0039E738
		[JsonConstructor]
		public ToyotaCoding(int UID, string Name, string Description, string Password, string PasswordHint, IEnumerable<TranslationItem> Translations, AdaptationValueTypes ValueType, string Address, string RequestHeader, string ResponseHeader, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned, bool ReversedByteSet, bool RequiresPro, IEnumerable<MQBAdaptationOption> Options)
			: base(UID, Name, Description, Password, PasswordHint, Translations, ValueType, Address, RequestHeader, ResponseHeader, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet, RequiresPro, Options)
		{
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0037C801 File Offset: 0x0037AA01
		protected override string WriteCommand
		{
			get
			{
				return "3B";
			}
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x003A056C File Offset: 0x0039E76C
		protected override string OpenSessionCommand
		{
			get
			{
				return "1003";
			}
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x06004D9D RID: 19869 RVA: 0x001ECB01 File Offset: 0x001EAD01
		protected override string CloseSessionCommand
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x001ECB08 File Offset: 0x001EAD08
		protected override CodingLogItem.CodingTypes CodingType
		{
			get
			{
				return CodingLogItem.CodingTypes.TOYOTA;
			}
		}
	}
}
