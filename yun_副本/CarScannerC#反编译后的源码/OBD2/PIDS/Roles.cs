using System;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FA RID: 1018
	public enum Roles
	{
		// Token: 0x040016C7 RID: 5831
		None,
		// Token: 0x040016C8 RID: 5832
		RPM,
		// Token: 0x040016C9 RID: 5833
		MAF,
		// Token: 0x040016CA RID: 5834
		MAP,
		// Token: 0x040016CB RID: 5835
		IAT,
		// Token: 0x040016CC RID: 5836
		Speed,
		// Token: 0x040016CD RID: 5837
		Injection,
		// Token: 0x040016CE RID: 5838
		Coolant,
		// Token: 0x040016CF RID: 5839
		Throttle,
		// Token: 0x040016D0 RID: 5840
		LOAD_PCT,
		// Token: 0x040016D1 RID: 5841
		LOAD_ABS,
		// Token: 0x040016D2 RID: 5842
		BARO,
		// Token: 0x040016D3 RID: 5843
		AMBIENT_TEMP,
		// Token: 0x040016D4 RID: 5844
		LAMBDA,
		// Token: 0x040016D5 RID: 5845
		FUEL_CUT_0,
		// Token: 0x040016D6 RID: 5846
		FUEL_CUT_1,
		// Token: 0x040016D7 RID: 5847
		InstantFuelRate,
		// Token: 0x040016D8 RID: 5848
		InstantFuelConsumption,
		// Token: 0x040016D9 RID: 5849
		FuelLevelInputPercent,
		// Token: 0x040016DA RID: 5850
		FuelLevelInputLiters,
		// Token: 0x040016DB RID: 5851
		AcceleratorPedalPosition,
		// Token: 0x040016DC RID: 5852
		CycleFuelConsumption,
		// Token: 0x040016DD RID: 5853
		CALC_AvgSpeed,
		// Token: 0x040016DE RID: 5854
		CALC_AvgSpeedTotal,
		// Token: 0x040016DF RID: 5855
		CALC_Distance,
		// Token: 0x040016E0 RID: 5856
		CALC_DistanceStatistics,
		// Token: 0x040016E1 RID: 5857
		CALC_InstantFuelRate,
		// Token: 0x040016E2 RID: 5858
		CALC_InstantFuelConsumption,
		// Token: 0x040016E3 RID: 5859
		CALC_AvgFuelConsumption,
		// Token: 0x040016E4 RID: 5860
		CALC_FuelUsed,
		// Token: 0x040016E5 RID: 5861
		CALC_Acceleration,
		// Token: 0x040016E6 RID: 5862
		CALC_Boost,
		// Token: 0x040016E7 RID: 5863
		CALC_PowerFromFuelConsumption,
		// Token: 0x040016E8 RID: 5864
		CALC_PowerFromAcceleration,
		// Token: 0x040016E9 RID: 5865
		CALC_TorqueFromPower,
		// Token: 0x040016EA RID: 5866
		GPS_Speed,
		// Token: 0x040016EB RID: 5867
		CALC_FuelUsedStatistics,
		// Token: 0x040016EC RID: 5868
		CALC_AvgFuelConsumptionStatistics,
		// Token: 0x040016ED RID: 5869
		CALC_FuelMoney,
		// Token: 0x040016EE RID: 5870
		CALC_FuelMoneyStatistics,
		// Token: 0x040016EF RID: 5871
		UNDEFINED,
		// Token: 0x040016F0 RID: 5872
		EV_BatteryVoltage,
		// Token: 0x040016F1 RID: 5873
		EV_BatteryCurrent,
		// Token: 0x040016F2 RID: 5874
		EV_BatteryPower,
		// Token: 0x040016F3 RID: 5875
		EV_CellVoltage,
		// Token: 0x040016F4 RID: 5876
		EV_MaxCellVoltage,
		// Token: 0x040016F5 RID: 5877
		EV_MinCellVoltage,
		// Token: 0x040016F6 RID: 5878
		EV_AvgCellVoltage,
		// Token: 0x040016F7 RID: 5879
		EV_ChargerPower,
		// Token: 0x040016F8 RID: 5880
		EV_ChargerCurrent,
		// Token: 0x040016F9 RID: 5881
		EV_ChargerVoltage,
		// Token: 0x040016FA RID: 5882
		CALC_PowerFromRPMAndTorque,
		// Token: 0x040016FB RID: 5883
		EV_INSTANT_CONSUMPTION_KM_KWH,
		// Token: 0x040016FC RID: 5884
		EV_INSTANT_CONSUMPTION_KWH_100KM,
		// Token: 0x040016FD RID: 5885
		GPS_Altitude,
		// Token: 0x040016FE RID: 5886
		FreeSpaceInTankCalculated,
		// Token: 0x040016FF RID: 5887
		EngineTorque
	}
}
