using System;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200087E RID: 2174
	public enum CodingRequestResult
	{
		// Token: 0x04002AFE RID: 11006
		Success,
		// Token: 0x04002AFF RID: 11007
		WrongAccessKey,
		// Token: 0x04002B00 RID: 11008
		WrongConditions,
		// Token: 0x04002B01 RID: 11009
		UnknownError,
		// Token: 0x04002B02 RID: 11010
		WrongDate,
		// Token: 0x04002B03 RID: 11011
		CodingPatternRefused,
		// Token: 0x04002B04 RID: 11012
		InitialDataIncorrect,
		// Token: 0x04002B05 RID: 11013
		WrongDevice,
		// Token: 0x04002B06 RID: 11014
		InitialDataEmpty,
		// Token: 0x04002B07 RID: 11015
		WrongInputValue,
		// Token: 0x04002B08 RID: 11016
		CodingPatternEmpty,
		// Token: 0x04002B09 RID: 11017
		WrongSeed,
		// Token: 0x04002B0A RID: 11018
		NotSupported,
		// Token: 0x04002B0B RID: 11019
		NoData,
		// Token: 0x04002B0C RID: 11020
		ELMBufferFull,
		// Token: 0x04002B0D RID: 11021
		OperationInProgress
	}
}
