using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000915 RID: 2325
	internal class UDSCoding : KWP2000Coding
	{
		// Token: 0x06004DC2 RID: 19906 RVA: 0x003A07B0 File Offset: 0x0039E9B0
		[JsonConstructor]
		public UDSCoding(int UID, string Name, string Description, string Password, string PasswordHint, IEnumerable<TranslationItem> Translations, AdaptationValueTypes ValueType, string Address, string RequestHeader, string ResponseHeader, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned, bool ReversedByteSet, bool RequiresPro, IEnumerable<MQBAdaptationOption> Options)
			: base(UID, Name, Description, Password, PasswordHint, Translations, ValueType, Address, RequestHeader, ResponseHeader, StartByteId, DataLength, Multiplier, Offset, IsSigned, ReversedByteSet, RequiresPro, Options)
		{
		}

		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x06004DC3 RID: 19907 RVA: 0x003A07E4 File Offset: 0x0039E9E4
		protected override string WriteCommand
		{
			get
			{
				return "2E";
			}
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x06004DC4 RID: 19908 RVA: 0x003A07EB File Offset: 0x0039E9EB
		protected override string ReadService
		{
			get
			{
				return "22";
			}
		}

		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x001ECB01 File Offset: 0x001EAD01
		protected override string OpenSessionCommand
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x001ECB01 File Offset: 0x001EAD01
		protected override string CloseSessionCommand
		{
			get
			{
				return "";
			}
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x001ECEE5 File Offset: 0x001EB0E5
		protected override CodingLogItem.CodingTypes CodingType
		{
			get
			{
				return CodingLogItem.CodingTypes.UDS;
			}
		}
	}
}
