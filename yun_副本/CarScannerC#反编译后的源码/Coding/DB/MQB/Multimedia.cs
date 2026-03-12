using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B25 RID: 2853
	internal static class Multimedia
	{
		// Token: 0x060058A2 RID: 22690 RVA: 0x004243A4 File Offset: 0x004225A4
		public static ICodingContainer MQB_5F_RearCameraInstalled()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 19, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 19, 4, false);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_RearCameraInstalled_Name") + " (MIB2)", Translate.GetString("codingDB_RearCameraInstalled_Description"), "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB });
		}

		// Token: 0x060058A3 RID: 22691 RVA: 0x0042441C File Offset: 0x0042261C
		public static ICodingContainer MQB_5F_RearCameraInstalledMIB3()
		{
			MQBAdaptationOption opt_no = MQBAdaptationTemplate.DisableOption;
			MQBAdaptationOption opt_no_legal_screen = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (no legal screen)", "01");
			MQBAdaptationOption opt_legal_screen = new MQBAdaptationOption(MQBAdaptationTemplate.EnableOption.Title + " (legal screen)", "10");
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB3("0505", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == opt_no.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else if (value == opt_no_legal_screen.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else if (value == opt_legal_screen.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_RearCameraInstalled_Name") + " (MIB3)", Translate.GetString("codingDB_RearCameraInstalled_Description"), "773", "7DD", "20103", new MQBAdaptationOption[] { opt_no, opt_no_legal_screen, opt_legal_screen })
			{
				Alternatives = { mqbalternativeContainerFor5FMIB }
			};
		}

		// Token: 0x060058A4 RID: 22692 RVA: 0x004244FC File Offset: 0x004226FC
		public static ICodingContainer MQB_5F_AMRadio()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 1, true);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0559", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 4, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 4, 7, true);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_AmRadio_Name"), "Enable or disable AM radio", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
		}

		// Token: 0x060058A5 RID: 22693 RVA: 0x00424594 File Offset: 0x00422794
		public static ICodingContainer MQB_5F_AUXInEnable()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 4, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0559", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 1, 5, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_AuxInputActivation_Name"), "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
		}

		// Token: 0x060058A6 RID: 22694 RVA: 0x0042462C File Offset: 0x0042282C
		public static ICodingContainer MQB_5F_DashboardDisplayPictures()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0B5A", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, true);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0B5A", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 7, 5, false);
					BitHelpers.SwitchBitInByte(array2, 7, 4, true);
					BitHelpers.SwitchBitInByte(array2, 6, 1, false);
					BitHelpers.SwitchBitInByte(array2, 6, 0, true);
					BitHelpers.SwitchBitInByte(array2, 7, 7, false);
					BitHelpers.SwitchBitInByte(array2, 7, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 7, 5, false);
					BitHelpers.SwitchBitInByte(array2, 7, 4, false);
					BitHelpers.SwitchBitInByte(array2, 6, 1, false);
					BitHelpers.SwitchBitInByte(array2, 6, 0, false);
					BitHelpers.SwitchBitInByte(array2, 7, 7, false);
					BitHelpers.SwitchBitInByte(array2, 7, 6, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_DisplayCallPicturesAlbumCoversAndRadiostationsArtOnActiveInfoDisplayAid_Name"), "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x004246C4 File Offset: 0x004228C4
		public static ICodingContainer MQB_5F_OffroadModeDisplayInMMI_Var2()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 16, 7, true);
						BitHelpers.SwitchBitInByte(array2, 16, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 16, 7, false);
						BitHelpers.SwitchBitInByte(array2, 16, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 16, 0, true);
					BitHelpers.SwitchBitInByte(array2, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 16, 0, false);
					BitHelpers.SwitchBitInByte(array2, 16, 2, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("773", "7DD", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 21, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 21, 0, false);
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", "", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array4, 21, 7, false);
						BitHelpers.SwitchBitInByte(array4, 21, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array4, 21, 7, false);
						BitHelpers.SwitchBitInByte(array4, 21, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 21, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 21, 0, false);
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("773", "7DD", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 24, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 24, 2, false);
				}
				return array5;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0558", "20103", delegate(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array6 = new byte[data.Length];
				Array.Copy(data, 0, array6, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array6, 1, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array6, 1, 7, false);
				}
				return array6;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding3 = new MQBAlternativeCoding(VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			mqbalternativeCoding3.PostWriteCommands = "1102";
			return new MQBMultipleCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_ShowOffroadScreenInMultimediaSystem_Name"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2, mqbalternativeCoding3 })
			{
				InnerDescription = Translate.GetString("codingDB_ShowOffroadScreenInMultimediaSystem_InnerDescription")
			};
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x004248A4 File Offset: 0x00422AA4
		public static ICodingContainer MQB_5F_TripComputerAvailableWithIgnitionOff()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 10, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 10, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 10, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_AccessTripComputerInMultimediaWhenIgnitionTurnedOff_Name"), "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x0042494C File Offset: 0x00422B4C
		public static MQBEasyCodingItem MQB_5F_DeveloperModeActivation()
		{
			return new MQBEasyCodingItem(CodingGroup.Multimedia, Translate.GetString("codingDB_MultimediaDeveloperModeActivation_Name"), "", "243F", "773", "7DD", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
			{
				PreWriteCommands = "104F",
				InnerDescription = Translate.GetString("codingDB_MultimediaDeveloperModeActivation_InnerDescription")
			};
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x004249DC File Offset: 0x00422BDC
		public static ICodingContainer MQB_5F_EcoDriving()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 37, 0, true);
					BitHelpers.SwitchBitInByte(array, 37, 1, false);
					BitHelpers.SwitchBitInByte(array, 37, 2, true);
					BitHelpers.SwitchBitInByte(array, 37, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 37, 0, false);
					BitHelpers.SwitchBitInByte(array, 37, 1, false);
					BitHelpers.SwitchBitInByte(array, 37, 2, false);
					BitHelpers.SwitchBitInByte(array, 37, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 35, 7, true);
						BitHelpers.SwitchBitInByte(array2, 35, 6, false);
						BitHelpers.SwitchBitInByte(array2, 35, 5, true);
						BitHelpers.SwitchBitInByte(array2, 35, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 35, 7, false);
						BitHelpers.SwitchBitInByte(array2, 35, 6, false);
						BitHelpers.SwitchBitInByte(array2, 35, 5, false);
						BitHelpers.SwitchBitInByte(array2, 35, 4, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 35, 0, true);
					BitHelpers.SwitchBitInByte(array2, 35, 1, false);
					BitHelpers.SwitchBitInByte(array2, 35, 2, true);
					BitHelpers.SwitchBitInByte(array2, 35, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 35, 0, false);
					BitHelpers.SwitchBitInByte(array2, 35, 1, false);
					BitHelpers.SwitchBitInByte(array2, 35, 2, false);
					BitHelpers.SwitchBitInByte(array2, 35, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_EcoDrivingStatisticsDisplayGreenBlueMenu_Name"), "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x00424A84 File Offset: 0x00422C84
		public static ICodingContainer MQB_5F_OilLevel()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, true);
					BitHelpers.SwitchBitInByte(array, 18, 1, true);
					BitHelpers.SwitchBitInByte(array, 18, 2, true);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, false);
					BitHelpers.SwitchBitInByte(array, 18, 1, false);
					BitHelpers.SwitchBitInByte(array, 18, 2, false);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 18, 7, true);
						BitHelpers.SwitchBitInByte(array2, 18, 6, true);
						BitHelpers.SwitchBitInByte(array2, 18, 5, true);
						BitHelpers.SwitchBitInByte(array2, 18, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 18, 7, false);
						BitHelpers.SwitchBitInByte(array2, 18, 6, false);
						BitHelpers.SwitchBitInByte(array2, 18, 5, false);
						BitHelpers.SwitchBitInByte(array2, 18, 4, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 18, 0, true);
					BitHelpers.SwitchBitInByte(array2, 18, 1, true);
					BitHelpers.SwitchBitInByte(array2, 18, 2, true);
					BitHelpers.SwitchBitInByte(array2, 18, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 18, 0, false);
					BitHelpers.SwitchBitInByte(array2, 18, 1, false);
					BitHelpers.SwitchBitInByte(array2, 18, 2, false);
					BitHelpers.SwitchBitInByte(array2, 18, 3, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_DisplayOilLevelInMultimediaSystem_Name"), "Audi TT", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				RequiresPro = false
			};
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x00424B2C File Offset: 0x00422D2C
		public static ICodingContainer MQB_5F_DrivingSchool()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array2, 34, 7, true);
						BitHelpers.SwitchBitInByte(array2, 34, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array2, 34, 7, false);
						BitHelpers.SwitchBitInByte(array2, 34, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[34] = 5;
				}
				else
				{
					array2[34] = 0;
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_DisplayDrivingSchoolModeInMmi_Name"), "", "773", "7DD", "", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 })
			{
				PostWriteCommands = "1102",
				RequiresPro = false
			};
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x00424BE0 File Offset: 0x00422DE0
		public static MQBEasyCodingItem ConfirmInstallationChanges()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("codingDB_Confirm_Name"), "01");
			return new MQBEasyCodingItem(CodingGroup.Multimedia, Translate.GetString("codingDB_ConfirmationOfInstallationChanges_Name"), Translate.GetString("codingDB_ConfirmationOfInstallationChanges_Description"), "0B2E", VagUnitHelper.GetRequestHeaderForMQBUnit("5F"), VagUnitHelper.GetResponseHeaderForMQBUnit("5F"), "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = array[0] ^ 201;
					array[1] = array[1] ^ 210;
				}
				return array;
			}, null, new MQBAdaptationOption[] { mqbadaptationOption })
			{
				HasCurrentState = false
			};
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x00424C74 File Offset: 0x00422E74
		public static ICodingContainer MQB_5F_CDRipping()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("0600", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 24, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 24, 5, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0557", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 6, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_CdRipping_Name"), Translate.GetString("codingDB_CdRipping_Description"), "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x00424D10 File Offset: 0x00422F10
		public static IEnumerable<ICodingContainer> MIB2_HMI_SpeedByOneByte()
		{
			List<ICodingContainer> list = new List<ICodingContainer>(26);
			for (int i = 0; i < 26; i++)
			{
				VIM_MIB2TestOneByte vim_MIB2TestOneByte = new VIM_MIB2TestOneByte(i);
				list.Add(vim_MIB2TestOneByte);
			}
			return list;
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x00424D44 File Offset: 0x00422F44
		public static ICodingContainer MIB3_WirelessAppleCarPlay()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0516", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0B43", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 7, false);
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("0555", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 0, 0, false);
					BitHelpers.SwitchBitInByte(array3, 1, 3, false);
					BitHelpers.SwitchBitInByte(array3, 1, 2, true);
					BitHelpers.SwitchBitInByte(array3, 1, 5, false);
					BitHelpers.SwitchBitInByte(array3, 1, 4, true);
					BitHelpers.SwitchBitInByte(array3, 0, 1, true);
					BitHelpers.SwitchBitInByte(array3, 0, 3, true);
					BitHelpers.SwitchBitInByte(array3, 5, 6, false);
					BitHelpers.SwitchBitInByte(array3, 0, 2, true);
					BitHelpers.SwitchBitInByte(array3, 0, 4, true);
					BitHelpers.SwitchBitInByte(array3, 0, 7, true);
					BitHelpers.SwitchBitInByte(array3, 1, 1, false);
					BitHelpers.SwitchBitInByte(array3, 1, 0, true);
					BitHelpers.SwitchBitInByte(array3, 4, 4, false);
					BitHelpers.SwitchBitInByte(array3, 4, 3, true);
					BitHelpers.SwitchBitInByte(array3, 0, 3, true);
					BitHelpers.SwitchBitInByte(array3, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 0, 0, true);
					BitHelpers.SwitchBitInByte(array3, 1, 3, true);
					BitHelpers.SwitchBitInByte(array3, 1, 2, false);
					BitHelpers.SwitchBitInByte(array3, 1, 5, true);
					BitHelpers.SwitchBitInByte(array3, 1, 4, false);
					BitHelpers.SwitchBitInByte(array3, 0, 1, false);
					BitHelpers.SwitchBitInByte(array3, 0, 3, false);
					BitHelpers.SwitchBitInByte(array3, 5, 6, true);
					BitHelpers.SwitchBitInByte(array3, 0, 2, false);
					BitHelpers.SwitchBitInByte(array3, 0, 4, false);
					BitHelpers.SwitchBitInByte(array3, 0, 7, false);
					BitHelpers.SwitchBitInByte(array3, 1, 1, true);
					BitHelpers.SwitchBitInByte(array3, 1, 0, false);
					BitHelpers.SwitchBitInByte(array3, 4, 4, false);
					BitHelpers.SwitchBitInByte(array3, 4, 3, false);
					BitHelpers.SwitchBitInByte(array3, 0, 3, false);
					BitHelpers.SwitchBitInByte(array3, 0, 1, false);
				}
				return array3;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem4 = new MQBEasyCodingItem("0517", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array4, 0, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array4, 0, 7, false);
				}
				return array4;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem5 = new MQBEasyCodingItem("0518", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array5 = new byte[data.Length];
				Array.Copy(data, 0, array5, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array5, 0, 5, true);
					BitHelpers.SwitchBitInByte(array5, 0, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array5, 0, 5, false);
					BitHelpers.SwitchBitInByte(array5, 0, 4, false);
				}
				return array5;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Wireless connectivity for Apple CarPlay (MIB3)", "Use this to activate wireless connectivity for Apple CarPlay. On some units enables wireless Android Auto too. MIB3 only!", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3, mqbeasyCodingItem4, mqbeasyCodingItem5 })
			{
				InnerDescription = "Compatiblity and abbility to activate this features depends on your multimedia unit, car selling region and on your phone.\nOn some cars CarPlay activates, but AndroidAuto doesn't.",
				Translations = 
				{
					new TranslationItem("ru", "Беспроводное подключение для Apple CarPlay (MIB3)", "Используйте для активации беспроводного подключения к Apple CarPlay. На Skoda Rapid 2020-2021 при этом активируется и беспроводной Android Auto. Только для MIB3!", "Совместимость и возможность активации этих функций зависит от вашего мультимедийного устройства, от региона продажи автомобиля и от возможностей вашего телефона.\nНа некоторых автомобилях активируется только беспроводное подключение к Apple CarPlay, но не к Android Auto.\nВНИМАНИЕ! Эта функция не активирует Car Play или Android Auto, а только беспроводное подключение к ним. Эти функции должны быть у вас уже активированы с завода.\nSWING не поддерживается.")
				}
			};
		}

		// Token: 0x060058B1 RID: 22705 RVA: 0x00424ED0 File Offset: 0x004230D0
		public static ICodingContainer MQB_5F_SecondPhoneSupport()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("22AD", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 2, true);
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 2, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0553", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, true);
					BitHelpers.SwitchBitInByte(array2, 0, 3, true);
					BitHelpers.SwitchBitInByte(array2, 1, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
					BitHelpers.SwitchBitInByte(array2, 0, 0, false);
					BitHelpers.SwitchBitInByte(array2, 0, 2, false);
					BitHelpers.SwitchBitInByte(array2, 0, 3, false);
					BitHelpers.SwitchBitInByte(array2, 1, 4, false);
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Multimedia, "", "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array3, 11, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array3, 11, 6, false);
				}
				return array3;
			}, null);
			return new MQBMultipleCoding(CodingGroup.Multimedia, Translate.GetString("codingDB_SecondPhone"), "", false, new ICodingContainer[] { mqbalternativeCoding, mqbeasyCodingItem });
		}

		// Token: 0x060058B2 RID: 22706 RVA: 0x00424FC4 File Offset: 0x004231C4
		public static ICodingContainer MQB_5F_NHTSA_NoSoftKeyboard()
		{
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("052A", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}, null);
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("052A", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 0, 1, false);
				}
				return array2;
			}, null);
			return new MQBAlternativeCoding(CodingGroup.Multimedia, "Apple CarPlay software keyboard limitation", "nhtsa_limitation_switches_for_carplay_no_softKeyboard", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 })
			{
				InnerDescription = "MIB2, MIB3 supported, MIB1 not supported"
			};
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x00425060 File Offset: 0x00423260
		public static ICodingContainer MQB_5F_RadioArtDB()
		{
			MQBAdaptationOption[] options = new MQBAdaptationOption[]
			{
				new MQBAdaptationOption(MQBAdaptationTemplate.DisableOption.Title, "0"),
				new MQBAdaptationOption("AUTO", "1"),
				new MQBAdaptationOption("Belarus", "2"),
				new MQBAdaptationOption("Belgium", "3"),
				new MQBAdaptationOption("Bosnia and Herzegovina", "4"),
				new MQBAdaptationOption("Bulgaria", "5"),
				new MQBAdaptationOption("Denmark", "6"),
				new MQBAdaptationOption("Germany", "7"),
				new MQBAdaptationOption("Estonia", "8"),
				new MQBAdaptationOption("Finland", "9"),
				new MQBAdaptationOption("France", "10"),
				new MQBAdaptationOption("Greece", "11"),
				new MQBAdaptationOption("Ireland", "12"),
				new MQBAdaptationOption("Iceland", "13"),
				new MQBAdaptationOption("Italy", "14"),
				new MQBAdaptationOption("Croatia", "15"),
				new MQBAdaptationOption("Liechtenstein", "16"),
				new MQBAdaptationOption("Lithuania", "17"),
				new MQBAdaptationOption("Luxembourg", "18"),
				new MQBAdaptationOption("Monaco", "20"),
				new MQBAdaptationOption("Montenegro", "21"),
				new MQBAdaptationOption("Netherlands", "22"),
				new MQBAdaptationOption("Norway", "23"),
				new MQBAdaptationOption("Austria", "24"),
				new MQBAdaptationOption("Poland", "25"),
				new MQBAdaptationOption("Portugal", "26"),
				new MQBAdaptationOption("Romania", "27"),
				new MQBAdaptationOption("Russia ", "28"),
				new MQBAdaptationOption("Sweden", "30"),
				new MQBAdaptationOption("Switzerland", "31"),
				new MQBAdaptationOption("Serbia", "32"),
				new MQBAdaptationOption("Slovakia", "33"),
				new MQBAdaptationOption("Slovenia", "34"),
				new MQBAdaptationOption("Spain", "35"),
				new MQBAdaptationOption("Czech Republic", "36"),
				new MQBAdaptationOption("Turkey", "37"),
				new MQBAdaptationOption("Ukraine", "38"),
				new MQBAdaptationOption("Hungary", "39"),
				new MQBAdaptationOption("United Kingdom", "41"),
				new MQBAdaptationOption("Cyprus", "42"),
				new MQBAdaptationOption("Moldova", "43"),
				new MQBAdaptationOption("Latvia", "44"),
				new MQBAdaptationOption("Macedonia ", "45"),
				new MQBAdaptationOption("Faeroes", "46"),
				new MQBAdaptationOption("Albania", "47"),
				new MQBAdaptationOption("Gibraltar", "48"),
				new MQBAdaptationOption("Andorra", "49"),
				new MQBAdaptationOption("Kosovo", "50"),
				new MQBAdaptationOption("Moscow", "51"),
				new MQBAdaptationOption("Saint Petersburg", "52"),
				new MQBAdaptationOption("Novosibirsk", "53"),
				new MQBAdaptationOption("Yekaterinburg", "54"),
				new MQBAdaptationOption("Nizhny Novgorod", "55"),
				new MQBAdaptationOption("Europe", "69")
			};
			MQBAlternativeContainerFor5FMIB25 mqbalternativeContainerFor5FMIB = new MQBAlternativeContainerFor5FMIB25("22AD", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = (byte)int.Parse(value, CultureInfo.InvariantCulture);
				array[6] = b;
				return array;
			}, delegate(byte[] data, MQBAlternativeCoding coding2)
			{
				coding2.StartByteId = 6;
				byte b2 = data[coding2.StartByteId];
				string s_byte = b2.ToString(CultureInfo.InvariantCulture);
				MQBAdaptationOption mqbadaptationOption2 = options.FirstOrDefault((MQBAdaptationOption x) => x.Value == s_byte);
				if (mqbadaptationOption2 != null)
				{
					return mqbadaptationOption2.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			MQBAlternativeContainerFor5FMIB3 mqbalternativeContainerFor5FMIB2 = new MQBAlternativeContainerFor5FMIB3("0559", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				byte b3 = (byte)int.Parse(value, CultureInfo.InvariantCulture);
				array2[0] = b3;
				return array2;
			}, delegate(byte[] data, MQBAlternativeCoding coding2)
			{
				coding2.StartByteId = 0;
				byte b4 = data[coding2.StartByteId];
				string s_byte = b4.ToString(CultureInfo.InvariantCulture);
				MQBAdaptationOption mqbadaptationOption3 = options.FirstOrDefault((MQBAdaptationOption x) => x.Value == s_byte);
				if (mqbadaptationOption3 != null)
				{
					return mqbadaptationOption3.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			});
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding(CodingGroup.Multimedia, Translate.GetString("coding_DB_RadioArtCoverDB_Name"), "", "773", "7DD", "20103", new MQBAlternativeContainer[] { mqbalternativeContainerFor5FMIB, mqbalternativeContainerFor5FMIB2 });
			mqbalternativeCoding.Options.Clear();
			foreach (MQBAdaptationOption mqbadaptationOption in options)
			{
				mqbalternativeCoding.Options.Add(mqbadaptationOption);
			}
			return mqbalternativeCoding;
		}

		// Token: 0x060058B4 RID: 22708 RVA: 0x0042555C File Offset: 0x0042375C
		public static ICodingContainer ResetToFactorySettings_FactorySettings()
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Stop", "310203E7");
			return new MQBServiceProcedure
			{
				Group = CodingGroup.Multimedia,
				Name = "Reset to factory setting",
				Description = "Basic settings",
				Unit = "5F",
				CancelOption = mqbadaptationOption,
				Options = 
				{
					new MQBAdaptationOption("Factory setting", "310103E7040000"),
					new MQBAdaptationOption("Internal measured values", "310103E7040001"),
					new MQBAdaptationOption("Active noise reduction", "310103E7040002"),
					mqbadaptationOption
				}
			};
		}

		// Token: 0x060058B5 RID: 22709 RVA: 0x00425608 File Offset: 0x00423808
		public static ICodingContainer MQB_5F_DisplayCharismaDriveMode()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[17] = 5;
				}
				else
				{
					array[17] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array2[17] = 160;
					}
					else
					{
						array2[17] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[17] = 5;
				}
				else
				{
					array2[17] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[23] = 9;
				}
				else
				{
					array3[23] = 0;
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array4[23] = 66;
					}
					else
					{
						array4[23] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[23] = 33;
				}
				else
				{
					array4[23] = 0;
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Display Drive mode selection in MMI", "Requires Drive Mode to be activated in Gateway", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2 });
		}

		// Token: 0x060058B6 RID: 22710 RVA: 0x0042572C File Offset: 0x0042392C
		public static ICodingContainer MQB_5F_DisplayStartStopInfo()
		{
			MQBAlternativeContainer mqbalternativeContainer = new MQBAlternativeContainer("0B3C", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[39] = 5;
				}
				else
				{
					array[39] = 0;
				}
				return array;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer2 = new MQBAlternativeContainer("0B1B", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array2[39] = 160;
					}
					else
					{
						array2[39] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[39] = 5;
				}
				else
				{
					array2[39] = 0;
				}
				return array2;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer, mqbalternativeContainer2 });
			MQBAlternativeContainer mqbalternativeContainer3 = new MQBAlternativeContainer("0B3D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[39] = 9;
				}
				else
				{
					array3[39] = 0;
				}
				return array3;
			}, null);
			MQBAlternativeContainer mqbalternativeContainer4 = new MQBAlternativeContainer("0B1D", "20103", delegate(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array4 = new byte[data.Length];
				Array.Copy(data, 0, array4, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array4[39] = 66;
					}
					else
					{
						array4[39] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array4[39] = 33;
				}
				else
				{
					array4[39] = 0;
				}
				return array4;
			}, null);
			MQBAlternativeCoding mqbalternativeCoding2 = new MQBAlternativeCoding("5F", new MQBAlternativeContainer[] { mqbalternativeContainer3, mqbalternativeContainer4 });
			return new MQBMultipleCoding(CodingGroup.Multimedia, "Display Start Stop information in MMI", "", false, new ICodingContainer[] { mqbalternativeCoding, mqbalternativeCoding2 });
		}

		// Token: 0x02000B26 RID: 2854
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060058B7 RID: 22711 RVA: 0x0042584F File Offset: 0x00423A4F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060058B8 RID: 22712 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060058B9 RID: 22713 RVA: 0x0042585C File Offset: 0x00423A5C
			internal byte[] <MQB_5F_RearCameraInstalled>b__0_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 19, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 19, 4, false);
				}
				return array;
			}

			// Token: 0x060058BA RID: 22714 RVA: 0x004258A8 File Offset: 0x00423AA8
			internal byte[] <MQB_5F_AMRadio>b__2_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 14, 1, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 14, 1, true);
				}
				return array;
			}

			// Token: 0x060058BB RID: 22715 RVA: 0x004258F4 File Offset: 0x00423AF4
			internal byte[] <MQB_5F_AMRadio>b__2_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 4, 7, true);
				}
				return array;
			}

			// Token: 0x060058BC RID: 22716 RVA: 0x00425940 File Offset: 0x00423B40
			internal byte[] <MQB_5F_AUXInEnable>b__3_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 8, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 8, 4, false);
				}
				return array;
			}

			// Token: 0x060058BD RID: 22717 RVA: 0x0042598C File Offset: 0x00423B8C
			internal byte[] <MQB_5F_AUXInEnable>b__3_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
				}
				return array;
			}

			// Token: 0x060058BE RID: 22718 RVA: 0x004259D8 File Offset: 0x00423BD8
			internal byte[] <MQB_5F_DashboardDisplayPictures>b__4_0(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, true);
					BitHelpers.SwitchBitInByte(array, 6, 1, true);
					BitHelpers.SwitchBitInByte(array, 6, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 2, false);
				}
				return array;
			}

			// Token: 0x060058BF RID: 22719 RVA: 0x00425A48 File Offset: 0x00423C48
			internal byte[] <MQB_5F_DashboardDisplayPictures>b__4_1(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, true);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 0, true);
					BitHelpers.SwitchBitInByte(array, 7, 7, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 7, 5, false);
					BitHelpers.SwitchBitInByte(array, 7, 4, false);
					BitHelpers.SwitchBitInByte(array, 6, 1, false);
					BitHelpers.SwitchBitInByte(array, 6, 0, false);
					BitHelpers.SwitchBitInByte(array, 7, 7, false);
					BitHelpers.SwitchBitInByte(array, 7, 6, false);
				}
				return array;
			}

			// Token: 0x060058C0 RID: 22720 RVA: 0x00425AEC File Offset: 0x00423CEC
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}

			// Token: 0x060058C1 RID: 22721 RVA: 0x00425B4C File Offset: 0x00423D4C
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 16, 7, true);
						BitHelpers.SwitchBitInByte(array, 16, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 16, 7, false);
						BitHelpers.SwitchBitInByte(array, 16, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, true);
					BitHelpers.SwitchBitInByte(array, 16, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 16, 0, false);
					BitHelpers.SwitchBitInByte(array, 16, 2, false);
				}
				return array;
			}

			// Token: 0x060058C2 RID: 22722 RVA: 0x00425BF4 File Offset: 0x00423DF4
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 21, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 21, 0, false);
				}
				return array;
			}

			// Token: 0x060058C3 RID: 22723 RVA: 0x00425C40 File Offset: 0x00423E40
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 21, 7, false);
						BitHelpers.SwitchBitInByte(array, 21, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 21, 7, false);
						BitHelpers.SwitchBitInByte(array, 21, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 21, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 21, 0, false);
				}
				return array;
			}

			// Token: 0x060058C4 RID: 22724 RVA: 0x00425CD4 File Offset: 0x00423ED4
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_4(byte[] data, string value, MQBAlternativeCoding coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 24, 2, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 24, 2, false);
				}
				return array;
			}

			// Token: 0x060058C5 RID: 22725 RVA: 0x00425D20 File Offset: 0x00423F20
			internal byte[] <MQB_5F_OffroadModeDisplayInMMI_Var2>b__5_5(byte[] data, string value, MQBAlternativeCoding coding)
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

			// Token: 0x060058C6 RID: 22726 RVA: 0x00425D6C File Offset: 0x00423F6C
			internal byte[] <MQB_5F_TripComputerAvailableWithIgnitionOff>b__6_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}

			// Token: 0x060058C7 RID: 22727 RVA: 0x00425DB8 File Offset: 0x00423FB8
			internal byte[] <MQB_5F_TripComputerAvailableWithIgnitionOff>b__6_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 10, 6, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 10, 1, false);
				}
				return array;
			}

			// Token: 0x060058C8 RID: 22728 RVA: 0x00425E38 File Offset: 0x00424038
			internal byte[] <MQB_5F_DeveloperModeActivation>b__7_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x060058C9 RID: 22729 RVA: 0x00425E78 File Offset: 0x00424078
			internal byte[] <MQB_5F_EcoDriving>b__8_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 37, 0, true);
					BitHelpers.SwitchBitInByte(array, 37, 1, false);
					BitHelpers.SwitchBitInByte(array, 37, 2, true);
					BitHelpers.SwitchBitInByte(array, 37, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 37, 0, false);
					BitHelpers.SwitchBitInByte(array, 37, 1, false);
					BitHelpers.SwitchBitInByte(array, 37, 2, false);
					BitHelpers.SwitchBitInByte(array, 37, 3, false);
				}
				return array;
			}

			// Token: 0x060058CA RID: 22730 RVA: 0x00425F00 File Offset: 0x00424100
			internal byte[] <MQB_5F_EcoDriving>b__8_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 35, 7, true);
						BitHelpers.SwitchBitInByte(array, 35, 6, false);
						BitHelpers.SwitchBitInByte(array, 35, 5, true);
						BitHelpers.SwitchBitInByte(array, 35, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 35, 7, false);
						BitHelpers.SwitchBitInByte(array, 35, 6, false);
						BitHelpers.SwitchBitInByte(array, 35, 5, false);
						BitHelpers.SwitchBitInByte(array, 35, 4, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 35, 0, true);
					BitHelpers.SwitchBitInByte(array, 35, 1, false);
					BitHelpers.SwitchBitInByte(array, 35, 2, true);
					BitHelpers.SwitchBitInByte(array, 35, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 35, 0, false);
					BitHelpers.SwitchBitInByte(array, 35, 1, false);
					BitHelpers.SwitchBitInByte(array, 35, 2, false);
					BitHelpers.SwitchBitInByte(array, 35, 3, false);
				}
				return array;
			}

			// Token: 0x060058CB RID: 22731 RVA: 0x00425FF8 File Offset: 0x004241F8
			internal byte[] <MQB_5F_OilLevel>b__9_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, true);
					BitHelpers.SwitchBitInByte(array, 18, 1, true);
					BitHelpers.SwitchBitInByte(array, 18, 2, true);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, false);
					BitHelpers.SwitchBitInByte(array, 18, 1, false);
					BitHelpers.SwitchBitInByte(array, 18, 2, false);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				return array;
			}

			// Token: 0x060058CC RID: 22732 RVA: 0x00426080 File Offset: 0x00424280
			internal byte[] <MQB_5F_OilLevel>b__9_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 18, 7, true);
						BitHelpers.SwitchBitInByte(array, 18, 6, true);
						BitHelpers.SwitchBitInByte(array, 18, 5, true);
						BitHelpers.SwitchBitInByte(array, 18, 4, false);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 18, 7, false);
						BitHelpers.SwitchBitInByte(array, 18, 6, false);
						BitHelpers.SwitchBitInByte(array, 18, 5, false);
						BitHelpers.SwitchBitInByte(array, 18, 4, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, true);
					BitHelpers.SwitchBitInByte(array, 18, 1, true);
					BitHelpers.SwitchBitInByte(array, 18, 2, true);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 18, 0, false);
					BitHelpers.SwitchBitInByte(array, 18, 1, false);
					BitHelpers.SwitchBitInByte(array, 18, 2, false);
					BitHelpers.SwitchBitInByte(array, 18, 3, false);
				}
				return array;
			}

			// Token: 0x060058CD RID: 22733 RVA: 0x00426178 File Offset: 0x00424378
			internal byte[] <MQB_5F_DrivingSchool>b__10_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}

			// Token: 0x060058CE RID: 22734 RVA: 0x004261BC File Offset: 0x004243BC
			internal byte[] <MQB_5F_DrivingSchool>b__10_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						BitHelpers.SwitchBitInByte(array, 34, 7, true);
						BitHelpers.SwitchBitInByte(array, 34, 5, true);
					}
					else
					{
						BitHelpers.SwitchBitInByte(array, 34, 7, false);
						BitHelpers.SwitchBitInByte(array, 34, 5, false);
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[34] = 5;
				}
				else
				{
					array[34] = 0;
				}
				return array;
			}

			// Token: 0x060058CF RID: 22735 RVA: 0x00426244 File Offset: 0x00424444
			internal byte[] <ConfirmInstallationChanges>b__11_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = array[0] ^ 201;
					array[1] = array[1] ^ 210;
				}
				return array;
			}

			// Token: 0x060058D0 RID: 22736 RVA: 0x00426294 File Offset: 0x00424494
			internal byte[] <MQB_5F_CDRipping>b__12_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 24, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 24, 5, false);
				}
				return array;
			}

			// Token: 0x060058D1 RID: 22737 RVA: 0x004262E0 File Offset: 0x004244E0
			internal byte[] <MQB_5F_CDRipping>b__12_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x060058D2 RID: 22738 RVA: 0x0042632C File Offset: 0x0042452C
			internal byte[] <MIB3_WirelessAppleCarPlay>b__14_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x060058D3 RID: 22739 RVA: 0x00426378 File Offset: 0x00424578
			internal byte[] <MIB3_WirelessAppleCarPlay>b__14_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x060058D4 RID: 22740 RVA: 0x004263C4 File Offset: 0x004245C4
			internal byte[] <MIB3_WirelessAppleCarPlay>b__14_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 1, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 2, true);
					BitHelpers.SwitchBitInByte(array, 1, 5, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 5, 6, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, true);
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 1, 1, false);
					BitHelpers.SwitchBitInByte(array, 1, 0, true);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
					BitHelpers.SwitchBitInByte(array, 4, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 0, true);
					BitHelpers.SwitchBitInByte(array, 1, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 2, false);
					BitHelpers.SwitchBitInByte(array, 1, 5, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 5, 6, true);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 1, 1, true);
					BitHelpers.SwitchBitInByte(array, 1, 0, false);
					BitHelpers.SwitchBitInByte(array, 4, 4, false);
					BitHelpers.SwitchBitInByte(array, 4, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x060058D5 RID: 22741 RVA: 0x00426534 File Offset: 0x00424734
			internal byte[] <MIB3_WirelessAppleCarPlay>b__14_3(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
				}
				return array;
			}

			// Token: 0x060058D6 RID: 22742 RVA: 0x00426580 File Offset: 0x00424780
			internal byte[] <MIB3_WirelessAppleCarPlay>b__14_4(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, true);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 5, false);
					BitHelpers.SwitchBitInByte(array, 0, 4, false);
				}
				return array;
			}

			// Token: 0x060058D7 RID: 22743 RVA: 0x004265DC File Offset: 0x004247DC
			internal byte[] <MQB_5F_SecondPhoneSupport>b__15_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 2, 7, true);
					BitHelpers.SwitchBitInByte(array, 3, 2, true);
					BitHelpers.SwitchBitInByte(array, 3, 1, true);
					BitHelpers.SwitchBitInByte(array, 3, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 3, 2, false);
					BitHelpers.SwitchBitInByte(array, 3, 1, false);
					BitHelpers.SwitchBitInByte(array, 3, 0, false);
				}
				return array;
			}

			// Token: 0x060058D8 RID: 22744 RVA: 0x00426654 File Offset: 0x00424854
			internal byte[] <MQB_5F_SecondPhoneSupport>b__15_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, true);
					BitHelpers.SwitchBitInByte(array, 0, 3, true);
					BitHelpers.SwitchBitInByte(array, 1, 4, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
					BitHelpers.SwitchBitInByte(array, 0, 0, false);
					BitHelpers.SwitchBitInByte(array, 0, 2, false);
					BitHelpers.SwitchBitInByte(array, 0, 3, false);
					BitHelpers.SwitchBitInByte(array, 1, 4, false);
				}
				return array;
			}

			// Token: 0x060058D9 RID: 22745 RVA: 0x004266E8 File Offset: 0x004248E8
			internal byte[] <MQB_5F_SecondPhoneSupport>b__15_2(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 11, 6, false);
				}
				return array;
			}

			// Token: 0x060058DA RID: 22746 RVA: 0x00426734 File Offset: 0x00424934
			internal byte[] <MQB_5F_NHTSA_NoSoftKeyboard>b__16_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x060058DB RID: 22747 RVA: 0x00426780 File Offset: 0x00424980
			internal byte[] <MQB_5F_NHTSA_NoSoftKeyboard>b__16_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 0, 1, false);
				}
				return array;
			}

			// Token: 0x060058DC RID: 22748 RVA: 0x004267CC File Offset: 0x004249CC
			internal byte[] <MQB_5F_RadioArtDB>b__17_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = (byte)int.Parse(value, CultureInfo.InvariantCulture);
				array[6] = b;
				return array;
			}

			// Token: 0x060058DD RID: 22749 RVA: 0x00426800 File Offset: 0x00424A00
			internal byte[] <MQB_5F_RadioArtDB>b__17_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				byte b = (byte)int.Parse(value, CultureInfo.InvariantCulture);
				array[0] = b;
				return array;
			}

			// Token: 0x060058DE RID: 22750 RVA: 0x00426834 File Offset: 0x00424A34
			internal byte[] <MQB_5F_DisplayCharismaDriveMode>b__19_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[17] = 5;
				}
				else
				{
					array[17] = 0;
				}
				return array;
			}

			// Token: 0x060058DF RID: 22751 RVA: 0x00426878 File Offset: 0x00424A78
			internal byte[] <MQB_5F_DisplayCharismaDriveMode>b__19_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[17] = 160;
					}
					else
					{
						array[17] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[17] = 5;
				}
				else
				{
					array[17] = 0;
				}
				return array;
			}

			// Token: 0x060058E0 RID: 22752 RVA: 0x004268E8 File Offset: 0x00424AE8
			internal byte[] <MQB_5F_DisplayCharismaDriveMode>b__19_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[23] = 9;
				}
				else
				{
					array[23] = 0;
				}
				return array;
			}

			// Token: 0x060058E1 RID: 22753 RVA: 0x0042692C File Offset: 0x00424B2C
			internal byte[] <MQB_5F_DisplayCharismaDriveMode>b__19_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[23] = 66;
					}
					else
					{
						array[23] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[23] = 33;
				}
				else
				{
					array[23] = 0;
				}
				return array;
			}

			// Token: 0x060058E2 RID: 22754 RVA: 0x00426998 File Offset: 0x00424B98
			internal byte[] <MQB_5F_DisplayStartStopInfo>b__20_0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[39] = 5;
				}
				else
				{
					array[39] = 0;
				}
				return array;
			}

			// Token: 0x060058E3 RID: 22755 RVA: 0x004269DC File Offset: 0x00424BDC
			internal byte[] <MQB_5F_DisplayStartStopInfo>b__20_1(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[39] = 160;
					}
					else
					{
						array[39] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[39] = 5;
				}
				else
				{
					array[39] = 0;
				}
				return array;
			}

			// Token: 0x060058E4 RID: 22756 RVA: 0x00426A4C File Offset: 0x00424C4C
			internal byte[] <MQB_5F_DisplayStartStopInfo>b__20_2(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[39] = 9;
				}
				else
				{
					array[39] = 0;
				}
				return array;
			}

			// Token: 0x060058E5 RID: 22757 RVA: 0x00426A90 File Offset: 0x00424C90
			internal byte[] <MQB_5F_DisplayStartStopInfo>b__20_3(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (codingItem.IsMIB3())
				{
					if (value == MQBAdaptationTemplate.EnableOption.Value)
					{
						array[39] = 66;
					}
					else
					{
						array[39] = 0;
					}
				}
				else if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[39] = 33;
				}
				else
				{
					array[39] = 0;
				}
				return array;
			}

			// Token: 0x04003735 RID: 14133
			public static readonly Multimedia.<>c <>9 = new Multimedia.<>c();

			// Token: 0x04003736 RID: 14134
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__0_0;

			// Token: 0x04003737 RID: 14135
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_0;

			// Token: 0x04003738 RID: 14136
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__2_1;

			// Token: 0x04003739 RID: 14137
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_0;

			// Token: 0x0400373A RID: 14138
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__3_1;

			// Token: 0x0400373B RID: 14139
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_0;

			// Token: 0x0400373C RID: 14140
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__4_1;

			// Token: 0x0400373D RID: 14141
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_0;

			// Token: 0x0400373E RID: 14142
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_1;

			// Token: 0x0400373F RID: 14143
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_2;

			// Token: 0x04003740 RID: 14144
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_3;

			// Token: 0x04003741 RID: 14145
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_4;

			// Token: 0x04003742 RID: 14146
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__5_5;

			// Token: 0x04003743 RID: 14147
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_0;

			// Token: 0x04003744 RID: 14148
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__6_1;

			// Token: 0x04003745 RID: 14149
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__7_0;

			// Token: 0x04003746 RID: 14150
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_0;

			// Token: 0x04003747 RID: 14151
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__8_1;

			// Token: 0x04003748 RID: 14152
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_0;

			// Token: 0x04003749 RID: 14153
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__9_1;

			// Token: 0x0400374A RID: 14154
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_0;

			// Token: 0x0400374B RID: 14155
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__10_1;

			// Token: 0x0400374C RID: 14156
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__11_0;

			// Token: 0x0400374D RID: 14157
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_0;

			// Token: 0x0400374E RID: 14158
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__12_1;

			// Token: 0x0400374F RID: 14159
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_0;

			// Token: 0x04003750 RID: 14160
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_1;

			// Token: 0x04003751 RID: 14161
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_2;

			// Token: 0x04003752 RID: 14162
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_3;

			// Token: 0x04003753 RID: 14163
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__14_4;

			// Token: 0x04003754 RID: 14164
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_0;

			// Token: 0x04003755 RID: 14165
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__15_1;

			// Token: 0x04003756 RID: 14166
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__15_2;

			// Token: 0x04003757 RID: 14167
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__16_0;

			// Token: 0x04003758 RID: 14168
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__16_1;

			// Token: 0x04003759 RID: 14169
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_0;

			// Token: 0x0400375A RID: 14170
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__17_2;

			// Token: 0x0400375B RID: 14171
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__19_0;

			// Token: 0x0400375C RID: 14172
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__19_1;

			// Token: 0x0400375D RID: 14173
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__19_2;

			// Token: 0x0400375E RID: 14174
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__19_3;

			// Token: 0x0400375F RID: 14175
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__20_0;

			// Token: 0x04003760 RID: 14176
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__20_1;

			// Token: 0x04003761 RID: 14177
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__20_2;

			// Token: 0x04003762 RID: 14178
			public static Func<byte[], string, MQBAlternativeCoding, byte[]> <>9__20_3;
		}

		// Token: 0x02000B27 RID: 2855
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x060058E6 RID: 22758 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x060058E7 RID: 22759 RVA: 0x00426AFC File Offset: 0x00424CFC
			internal string <MQB_5F_RadioArtDB>b__1(byte[] data, MQBAlternativeCoding coding2)
			{
				Multimedia.<>c__DisplayClass17_1 CS$<>8__locals1 = new Multimedia.<>c__DisplayClass17_1();
				coding2.StartByteId = 6;
				byte b = data[coding2.StartByteId];
				CS$<>8__locals1.s_byte = b.ToString(CultureInfo.InvariantCulture);
				MQBAdaptationOption mqbadaptationOption = this.options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.s_byte);
				if (mqbadaptationOption != null)
				{
					return mqbadaptationOption.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x060058E8 RID: 22760 RVA: 0x00426B58 File Offset: 0x00424D58
			internal string <MQB_5F_RadioArtDB>b__3(byte[] data, MQBAlternativeCoding coding2)
			{
				Multimedia.<>c__DisplayClass17_2 CS$<>8__locals1 = new Multimedia.<>c__DisplayClass17_2();
				coding2.StartByteId = 0;
				byte b = data[coding2.StartByteId];
				CS$<>8__locals1.s_byte = b.ToString(CultureInfo.InvariantCulture);
				MQBAdaptationOption mqbadaptationOption = this.options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.s_byte);
				if (mqbadaptationOption != null)
				{
					return mqbadaptationOption.Title;
				}
				return MQBAdaptationTemplate.UNKNOWN_TITLE;
			}

			// Token: 0x04003763 RID: 14179
			public MQBAdaptationOption[] options;
		}

		// Token: 0x02000B28 RID: 2856
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_1
		{
			// Token: 0x060058E9 RID: 22761 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_1()
			{
			}

			// Token: 0x060058EA RID: 22762 RVA: 0x00426BB4 File Offset: 0x00424DB4
			internal bool <MQB_5F_RadioArtDB>b__4(MQBAdaptationOption x)
			{
				return x.Value == this.s_byte;
			}

			// Token: 0x04003764 RID: 14180
			public string s_byte;
		}

		// Token: 0x02000B29 RID: 2857
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_2
		{
			// Token: 0x060058EB RID: 22763 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_2()
			{
			}

			// Token: 0x060058EC RID: 22764 RVA: 0x00426BC7 File Offset: 0x00424DC7
			internal bool <MQB_5F_RadioArtDB>b__5(MQBAdaptationOption x)
			{
				return x.Value == this.s_byte;
			}

			// Token: 0x04003765 RID: 14181
			public string s_byte;
		}

		// Token: 0x02000B2A RID: 2858
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060058ED RID: 22765 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060058EE RID: 22766 RVA: 0x00426BDC File Offset: 0x00424DDC
			internal byte[] <MQB_5F_RearCameraInstalledMIB3>b__0(byte[] data, string value, MQBAlternativeCoding codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == this.opt_no.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				else if (value == this.opt_no_legal_screen.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, false);
					BitHelpers.SwitchBitInByte(array, 0, 6, true);
				}
				else if (value == this.opt_legal_screen.Value)
				{
					BitHelpers.SwitchBitInByte(array, 0, 7, true);
					BitHelpers.SwitchBitInByte(array, 0, 6, false);
				}
				return array;
			}

			// Token: 0x04003766 RID: 14182
			public MQBAdaptationOption opt_no;

			// Token: 0x04003767 RID: 14183
			public MQBAdaptationOption opt_no_legal_screen;

			// Token: 0x04003768 RID: 14184
			public MQBAdaptationOption opt_legal_screen;
		}
	}
}
