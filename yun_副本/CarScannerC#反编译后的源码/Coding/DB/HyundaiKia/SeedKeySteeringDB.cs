using System;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BC5 RID: 3013
	internal class SeedKeySteeringDB : HyundaiKiaKnownSeedKeyBase
	{
		// Token: 0x06005B12 RID: 23314 RVA: 0x00436D4A File Offset: 0x00434F4A
		public SeedKeySteeringDB()
			: base("7D4", "7DC", 10, 1)
		{
			base.FillDictionaryFromString("0017=00B8;0055=02AB;007A=03D6;003B=01DF;0003=001A;0069=034D;004F=027B;0028=0144");
		}
	}
}
