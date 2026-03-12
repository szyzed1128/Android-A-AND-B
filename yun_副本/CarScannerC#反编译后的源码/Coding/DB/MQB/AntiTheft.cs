using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A82 RID: 2690
	internal static class AntiTheft
	{
		// Token: 0x060054D7 RID: 21719 RVA: 0x004059A4 File Offset: 0x00403BA4
		public static ICodingContainer Alarm_ActivateAntiTheftAlarm()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemActivation_Name"), "", "0D07", 1, 1, "0600", 12, 0, Array.Empty<TranslationItem>());
			mqbalternativeCoding.InnerDescription = Translate.GetString("codingDB_AntiTheftAlarmSystemActivation_InnerDescription");
			return mqbalternativeCoding;
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x004059EC File Offset: 0x00403BEC
		public static ICodingContainer Alarm_DisableCabinAlarmSensor()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemDisableCabinAlarmSensorByDoubleClickOnLockButton_Name"), "", "0D07", 2, 3, "", 99999, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x00405A28 File Offset: 0x00403C28
		public static ICodingContainer Alarm_ParkingHeaterInstalled()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemParkingHeaterWebasto_Name"), "", "0D07", 2, 1, "", 9999, 0, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x00405A64 File Offset: 0x00403C64
		public static ICodingContainer Alarm_UseSignalhorn()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemUseSignalHornForAlarms_Name"), "", "0D07", 0, 7, "0D07", 0, 7, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x00405A9C File Offset: 0x00403C9C
		public static ICodingContainer Alarm_AlarmSignal()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemAlarmSignalStyle_Name"), "", "0D07", 0, 7, "0D07", 0, 7, Array.Empty<TranslationItem>());
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_FrequencyModulated_Name"), MQBAdaptationTemplate.DisableOption.Value);
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("codingDB_Discontinuous_Name"), MQBAdaptationTemplate.EnableOption.Value);
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			mqbalternativeCoding.Options.Add(mqbadaptationOption2);
			return mqbalternativeCoding;
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x00405B2C File Offset: 0x00403D2C
		public static ICodingContainer Alarm_DeactivationSensorsHMI()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemDeactivateSensorsInHmi_Name"), "", "0D07", 1, 3, "", 999999, 7, Array.Empty<TranslationItem>());
			mqbalternativeCoding.Translations.Add(new TranslationItem("ru", "Заводская охранная система: отключение датчиков в меню", "", ""));
			return mqbalternativeCoding;
		}

		// Token: 0x060054DD RID: 21725 RVA: 0x00405B8C File Offset: 0x00403D8C
		public static ICodingContainer Alarm_DelayBeforeActivation()
		{
			MQBAdaptationOption opt_driver_door_contact = new MQBAdaptationOption(Translate.GetString("codingDB_AfterDriverDoorLockBySignal_Name"), "01");
			MQBAdaptationOption opt_driver_door_lock_cylinder = new MQBAdaptationOption(Translate.GetString("codingDB_AfterDriverDoorMechanicalLock_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				else if (value == opt_driver_door_contact.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else if (value == opt_driver_door_lock_cylinder.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
				}
				else if (value == opt_driver_door_contact.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
				}
				else if (value == opt_driver_door_lock_cylinder.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Assistance, Translate.GetString("codingDB_AntiTheftAlarmSystemDelayBeforeAlarmSystemActivatedAfterLockingCar_Name"), "", VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(MQBAdaptationTemplate.DisableOption);
			mqbalternativeCoding.Options.Add(opt_driver_door_contact);
			mqbalternativeCoding.Options.Add(opt_driver_door_lock_cylinder);
			return mqbalternativeCoding;
		}

		// Token: 0x060054DE RID: 21726 RVA: 0x00405C88 File Offset: 0x00403E88
		public static ICodingContainer Alarm_CamperMode()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemCamperMode_Name"), "", "0D07", 0, 4, "0D07", 0, 4, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054DF RID: 21727 RVA: 0x00405CC0 File Offset: 0x00403EC0
		public static ICodingContainer Alarm_SoundOut()
		{
			MQBAdaptationOption opt_horn = new MQBAdaptationOption(Translate.GetString("codingDB_SignalHorn_Name"), "01");
			MQBAdaptationOption opt_siren = new MQBAdaptationOption(Translate.GetString("codingDB_Siren_Name"), "10");
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				else if (value == opt_horn.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else if (value == opt_siren.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0600", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 12, 4, false);
					BitHelpers.SwitchBitInByte(array2, 12, 3, false);
				}
				else if (value == opt_horn.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 12, 4, false);
					BitHelpers.SwitchBitInByte(array2, 12, 3, true);
				}
				else if (value == opt_siren.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 12, 4, true);
					BitHelpers.SwitchBitInByte(array2, 12, 3, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemSoundDevice"), "", VagUnitHelper.GetRequestHeaderForMQBUnit("09"), VagUnitHelper.GetResponseHeaderForMQBUnit("09"), "31347", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 });
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(MQBAdaptationTemplate.DisableOption);
			mqbalternativeCoding.Options.Add(opt_horn);
			mqbalternativeCoding.Options.Add(opt_siren);
			mqbalternativeCoding.InnerDescription = Translate.GetString("codingDB_AntiTheftAlarmSystemSoundDevice_InnerDescription");
			return mqbalternativeCoding;
		}

		// Token: 0x060054E0 RID: 21728 RVA: 0x00405DD0 File Offset: 0x00403FD0
		public static ICodingContainer Alarm_RearWindowBreakSensor()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemRearWindowBreakSensor_Name"), "", "0D07", 1, 2, "0600", 12, 1, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054E1 RID: 21729 RVA: 0x00405E08 File Offset: 0x00404008
		public static ICodingContainer Alarm_DeactivateAntiTheftByUnlockCylinder()
		{
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit = new MQBAlternativeContainerForUnit09(true, "0D07", "31347", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerForUnit09 mqbalternativeContainerForUnit2 = new MQBAlternativeContainerForUnit09(false, "0600", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 12, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 12, 6, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemDeactivateByOpeningDoorLock_Name"), Translate.GetString("codingDB_AntiTheftAlarmSystemDeactivateByOpeningDoorLock_Description"), "70E", "778", "", new MQBAlternativeContainer[] { mqbalternativeContainerForUnit, mqbalternativeContainerForUnit2 })
			{
				RequiresPro = true
			};
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x00405EB8 File Offset: 0x004040B8
		public static ICodingContainer Alarm_RearWindowBreakSensorForInteriorMonitoring()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemSensorForInteriorMonitoring_Name"), "", "0D07", 2, 0, "", 999999, 1, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x00405EF4 File Offset: 0x004040F4
		public static ICodingContainer Alarm_SirenAlarms()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemSirenAlarms_Name"), "", "0D07", 0, 6, "0D07", 0, 6, Array.Empty<TranslationItem>());
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_10Alarms_Name"), MQBAdaptationTemplate.DisableOption.Value);
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("codingDB_1Alarm_Name"), MQBAdaptationTemplate.EnableOption.Value);
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			mqbalternativeCoding.Options.Add(mqbadaptationOption2);
			return mqbalternativeCoding;
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x00405F84 File Offset: 0x00404184
		public static ICodingContainer Alarm_TiltSensor()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemTiltSensor_Name"), "", "0D07", 1, 6, "0600", 12, 7, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x00405FBC File Offset: 0x004041BC
		public static ICodingContainer Alarm_MonitoringInsideLockingLeaver()
		{
			return MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, Translate.GetString("codingDB_AntiTheftAlarmSystemMonitorInsideLockingLeaver_Name"), "", "0D07", 0, 5, "0D07", 0, 5, Array.Empty<TranslationItem>());
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x00405FF4 File Offset: 0x004041F4
		public static ICodingContainer Alarm_PanicAlarmDelay()
		{
			MQBAlternativeCoding mqbalternativeCoding = MQBAlternativeCoding.BuildAltCodingSwitchForUnit09(CodingGroup.AntiTheftAlarm, "Anti-theft alarm system: panic alarm delay", "", "0D07", 0, 1, "0D07", 0, 1, Array.Empty<TranslationItem>());
			mqbalternativeCoding.Translations.Add(new TranslationItem("ru", Translate.GetString("codingDB_AntiTheftAlarmSystemPanicAlarmDelay_Name"), "", ""));
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_2SecondsDelay_Name"), MQBAdaptationTemplate.EnableOption.Value);
			mqbalternativeCoding.Options.Clear();
			mqbalternativeCoding.Options.Add(MQBAdaptationTemplate.DisableOption);
			mqbalternativeCoding.Options.Add(mqbadaptationOption);
			return mqbalternativeCoding;
		}

		// Token: 0x02000A83 RID: 2691
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060054E7 RID: 21735 RVA: 0x0040608F File Offset: 0x0040428F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060054E8 RID: 21736 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060054E9 RID: 21737 RVA: 0x0040609C File Offset: 0x0040429C
			internal byte[] <Alarm_DeactivateAntiTheftByUnlockCylinder>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 7, false);
				}
				return array;
			}

			// Token: 0x060054EA RID: 21738 RVA: 0x004060E8 File Offset: 0x004042E8
			internal byte[] <Alarm_DeactivateAntiTheftByUnlockCylinder>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 12, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 12, 6, false);
				}
				return array;
			}

			// Token: 0x040033F2 RID: 13298
			public static readonly AntiTheft.<>c <>9 = new AntiTheft.<>c();

			// Token: 0x040033F3 RID: 13299
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x040033F4 RID: 13300
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;
		}

		// Token: 0x02000A84 RID: 2692
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x060054EB RID: 21739 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x060054EC RID: 21740 RVA: 0x00406134 File Offset: 0x00404334
			internal byte[] <Alarm_DelayBeforeActivation>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				else if (value == this.opt_driver_door_contact.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else if (value == this.opt_driver_door_lock_cylinder.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x060054ED RID: 21741 RVA: 0x004061CC File Offset: 0x004043CC
			internal byte[] <Alarm_DelayBeforeActivation>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				else if (value == this.opt_driver_door_contact.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
				}
				else if (value == this.opt_driver_door_lock_cylinder.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
				}
				return array;
			}

			// Token: 0x040033F5 RID: 13301
			public MQBAdaptationOption opt_driver_door_contact;

			// Token: 0x040033F6 RID: 13302
			public MQBAdaptationOption opt_driver_door_lock_cylinder;
		}

		// Token: 0x02000A85 RID: 2693
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x060054EE RID: 21742 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x060054EF RID: 21743 RVA: 0x00406264 File Offset: 0x00404464
			internal byte[] <Alarm_SoundOut>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				else if (value == this.opt_horn.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else if (value == this.opt_siren.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				return array;
			}

			// Token: 0x060054F0 RID: 21744 RVA: 0x004062FC File Offset: 0x004044FC
			internal byte[] <Alarm_SoundOut>b__1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.DisableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 12, 4, false);
					BitHelpers.SwitchBitInByte(array, 12, 3, false);
				}
				else if (value == this.opt_horn.Value)
				{
					BitHelpers.SwitchBitInByte(array, 12, 4, false);
					BitHelpers.SwitchBitInByte(array, 12, 3, true);
				}
				else if (value == this.opt_siren.Value)
				{
					BitHelpers.SwitchBitInByte(array, 12, 4, true);
					BitHelpers.SwitchBitInByte(array, 12, 3, false);
				}
				return array;
			}

			// Token: 0x040033F7 RID: 13303
			public MQBAdaptationOption opt_horn;

			// Token: 0x040033F8 RID: 13304
			public MQBAdaptationOption opt_siren;
		}
	}
}
