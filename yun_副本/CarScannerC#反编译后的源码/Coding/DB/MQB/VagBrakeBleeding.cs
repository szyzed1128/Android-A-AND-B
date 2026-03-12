using System;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B15 RID: 2837
	internal static class VagBrakeBleeding
	{
		// Token: 0x06005854 RID: 22612 RVA: 0x00422290 File Offset: 0x00420490
		public static ICodingContainer[] BuildBrakeBleed_MQB()
		{
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: First flushing cycle", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E01", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck2 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed rear", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E02", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck3 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed front", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E03", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck4 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Second flushing cycle", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E04", "3102039E", "", true);
			return new ICodingContainer[] { mqbsimpleOperationWith0102StatusCheck, mqbsimpleOperationWith0102StatusCheck2, mqbsimpleOperationWith0102StatusCheck3, mqbsimpleOperationWith0102StatusCheck4 };
		}

		// Token: 0x06005855 RID: 22613 RVA: 0x0042235C File Offset: 0x0042055C
		public static ICodingContainer[] BuildBrakeBleed_PQ262020()
		{
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Preparation", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E0000", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck2 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed front", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E0100", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck3 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed rear", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E0500", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck4 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: End", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E0700", "3102039E", "", true);
			return new ICodingContainer[] { mqbsimpleOperationWith0102StatusCheck, mqbsimpleOperationWith0102StatusCheck2, mqbsimpleOperationWith0102StatusCheck3, mqbsimpleOperationWith0102StatusCheck4 };
		}

		// Token: 0x06005856 RID: 22614 RVA: 0x00422428 File Offset: 0x00420628
		public static ICodingContainer[] BuildBrakeBleed_MLBEVO_A4B9()
		{
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Preparation", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E00", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck2 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed front", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E05", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck3 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: Bleed rear", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E01", "3102039E", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck4 = new MQBSimpleOperationWith0102StatusCheck("Brake bleeding: End", "WARNING! Experimental feature! Use at your own risk!", "", "03", "3101039E07", "3102039E", "", true);
			return new ICodingContainer[] { mqbsimpleOperationWith0102StatusCheck, mqbsimpleOperationWith0102StatusCheck2, mqbsimpleOperationWith0102StatusCheck3, mqbsimpleOperationWith0102StatusCheck4 };
		}

		// Token: 0x06005857 RID: 22615 RVA: 0x004224F4 File Offset: 0x004206F4
		public static ICodingContainer[] BuildParkingBrake_MLBEVO_A4B9()
		{
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck = new MQBSimpleOperationWith0102StatusCheck("Parking brake: start lining change mode", "WARNING! Experimental feature! Use at your own risk!", "", "03", "310103A1040000", "310203A1", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck2 = new MQBSimpleOperationWith0102StatusCheck("Parking brake: end lining change mode", "WARNING! Experimental feature! Use at your own risk!", "", "03", "310103A0040000", "310203A0", "", true);
			MQBSimpleOperationWith0102StatusCheck mqbsimpleOperationWith0102StatusCheck3 = new MQBSimpleOperationWith0102StatusCheck("Parking brake: start-up", "WARNING! Experimental feature! Use at your own risk!", "", "03", "31010542040000", "31020542", "", true);
			return new ICodingContainer[] { mqbsimpleOperationWith0102StatusCheck, mqbsimpleOperationWith0102StatusCheck2, mqbsimpleOperationWith0102StatusCheck3 };
		}
	}
}
