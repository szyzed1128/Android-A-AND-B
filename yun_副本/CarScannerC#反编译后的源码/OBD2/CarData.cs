using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.PIDS.InternalActions;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x020002F3 RID: 755
	public class CarData
	{
		// Token: 0x06002386 RID: 9094 RVA: 0x001B1D94 File Offset: 0x001AFF94
		public CarData()
		{
			this.NissanConsultPIDs = new List<PID>();
			this.FreezeFrameNumber = 0;
			this.LiveDataPIDs = new List<PID>();
		}

		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x06002387 RID: 9095 RVA: 0x001B1E27 File Offset: 0x001B0027
		// (set) Token: 0x06002388 RID: 9096 RVA: 0x001B1E2F File Offset: 0x001B002F
		public ConcurrentQueue<DTCItemV2> DTCs
		{
			[CompilerGenerated]
			get
			{
				return this.<DTCs>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DTCs>k__BackingField = value;
			}
		} = new ConcurrentQueue<DTCItemV2>();

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x06002389 RID: 9097 RVA: 0x001B1E38 File Offset: 0x001B0038
		// (set) Token: 0x0600238A RID: 9098 RVA: 0x001B1E40 File Offset: 0x001B0040
		public DataRecorderV2 Recorder
		{
			[CompilerGenerated]
			get
			{
				return this.<Recorder>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Recorder>k__BackingField = value;
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x0600238B RID: 9099 RVA: 0x001B1E4C File Offset: 0x001B004C
		// (remove) Token: 0x0600238C RID: 9100 RVA: 0x001B1E84 File Offset: 0x001B0084
		private event CarDataChanged CarDataChanged
		{
			[CompilerGenerated]
			add
			{
				CarDataChanged carDataChanged = this.CarDataChanged;
				CarDataChanged carDataChanged2;
				do
				{
					carDataChanged2 = carDataChanged;
					CarDataChanged carDataChanged3 = (CarDataChanged)Delegate.Combine(carDataChanged2, value);
					carDataChanged = Interlocked.CompareExchange<CarDataChanged>(ref this.CarDataChanged, carDataChanged3, carDataChanged2);
				}
				while (carDataChanged != carDataChanged2);
			}
			[CompilerGenerated]
			remove
			{
				CarDataChanged carDataChanged = this.CarDataChanged;
				CarDataChanged carDataChanged2;
				do
				{
					carDataChanged2 = carDataChanged;
					CarDataChanged carDataChanged3 = (CarDataChanged)Delegate.Remove(carDataChanged2, value);
					carDataChanged = Interlocked.CompareExchange<CarDataChanged>(ref this.CarDataChanged, carDataChanged3, carDataChanged2);
				}
				while (carDataChanged != carDataChanged2);
			}
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x001B1EBC File Offset: 0x001B00BC
		private void OnCarDataChanged(PID PIDChanged)
		{
			CarDataChanged myEvent = this.CarDataChanged;
			if (myEvent != null)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					myEvent(PIDChanged);
				});
			}
		}

		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x001B1EFB File Offset: 0x001B00FB
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x001B1F21 File Offset: 0x001B0121
		public List<PID> LiveDataPIDs
		{
			get
			{
				if (this._LiveDataPIDs == null)
				{
					this._LiveDataPIDs = new List<PID>(600);
					this.CreateEmptyPIDS();
				}
				return this._LiveDataPIDs;
			}
			set
			{
				this._LiveDataPIDs = value;
			}
		}

		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06002390 RID: 9104 RVA: 0x001B1F2A File Offset: 0x001B012A
		// (set) Token: 0x06002391 RID: 9105 RVA: 0x001B1F32 File Offset: 0x001B0132
		public int FreezeFrameNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<FreezeFrameNumber>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FreezeFrameNumber>k__BackingField = value;
			}
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x001B1F3C File Offset: 0x001B013C
		public void CreateEmptyPIDS()
		{
			object obj = this.createLockObject;
			lock (obj)
			{
				this.CreateMode01PIDs();
				if (SharedSettings.Current.DaihatsuKLine)
				{
					this.ApplyDaihatsuFix();
					this.CreateCalculatedPIDs();
				}
				else
				{
					this.ApplyMode01Prefix();
					this.CreateCalculatedPIDs();
				}
				this.LiveDataPIDs.Add(PIDWithStringValue.PID0902_VIN());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID0904_CalibrationID());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID090A_EcuName());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A90_KWP_VIN());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A91_KWP_VehicleManufacturerECUHardwareNumber());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A92_KWP_SystemSupplierECUHardwareNumber());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A93_KWP_SystemSupplierECUHardwareVersion());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A94_KWP_SystemSupplierECUSoftwareNumber());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A95_KWP_SystemSupplierECUSoftwareVersion());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A96_KWP_ExhaustRegulationOrTypeApprovalNumber());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A97_KWP_SystemNameOrEngineType());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A98_KWP_RepairShopCodeOrTesterSerialNumber());
				this.LiveDataPIDs.Add(PIDWithStringValue.PID1A99_KWP_ProgrammingDate());
				this.CreateMode02PIDs();
				this.Mode06TestCollection = new ObservableCollection<Mode6Test>();
			}
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x001B2090 File Offset: 0x001B0290
		private void ResetAvailablePids(List<PID> pid_dict)
		{
			foreach (PID pid in pid_dict)
			{
				if (pid != null && !(pid is CustomPID))
				{
					pid.IsAvailable = false;
				}
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x001B20EC File Offset: 0x001B02EC
		public void ResetAvailablePids()
		{
			this._findByIdCacheDict.Clear();
			this.IsPIDTestingMode = false;
			this.AddOrRemoveNissanConsultPIDsV2(App.OBDReader.IsNissanConsult2Protocol);
			this.ResetAvailablePids(this.LiveDataPIDs);
			if (this.NissanConsultPIDs != null)
			{
				this.ResetAvailablePids(this.NissanConsultPIDs);
			}
			this.ResetAvailablePids(this.Mode02PIDs);
			MainThread.InvokeOnMainThreadAsync(delegate
			{
				this.Mode06TestCollection.Clear();
			});
			PIDWithFloatValueFormula.ResetScalingToDefaults();
			this.ShouldSetAsAvailable = false;
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x001B2168 File Offset: 0x001B0368
		public void InitializeCalculatedPIDs()
		{
			OBDDataReader obdreader = App.OBDReader;
			if (obdreader != null)
			{
				obdreader.DebugWrite("\r\n[InitializeCalculatedPIDs->start]\r\n");
			}
			foreach (CalculatedPIDV2 calculatedPIDV in (from x in this.LiveDataPIDs
				where x is CalculatedPIDV2
				select x as CalculatedPIDV2).ToList<CalculatedPIDV2>())
			{
				try
				{
					OBDDataReader obdreader2 = App.OBDReader;
					if (obdreader2 != null)
					{
						obdreader2.DebugWrite(string.Format("\r\n[InitializeCalculatedPIDs->({0}):{1}]\r\n", calculatedPIDV.Id, calculatedPIDV.Name));
					}
					calculatedPIDV.Initialize();
					OBDDataReader obdreader3 = App.OBDReader;
					if (obdreader3 != null)
					{
						obdreader3.DebugWrite(string.Format("\r\n[InitializeCalculatedPIDs->({0}):{1}:Supported={2}]\r\n", calculatedPIDV.Id, calculatedPIDV.Name, calculatedPIDV.IsAvailable));
					}
				}
				catch (Exception ex)
				{
					OBDDataReader obdreader4 = App.OBDReader;
					if (obdreader4 != null)
					{
						obdreader4.DebugWrite("\r\n[InitializeCalculatedPIDs->exc: " + ex.ToString() + "]\r\n");
					}
				}
			}
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x001B22C0 File Offset: 0x001B04C0
		private void CreateCalculatedPIDs()
		{
			try
			{
				if ((SharedSettings.Current.SelectedProfileV2Name != null && SharedSettings.Current.SelectedProfileV2Name.Contains("SSM2")) || (SharedSettings.Current.SelectedProfileName != null && SharedSettings.Current.SelectedProfileName.Contains("SSM2")))
				{
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_RPM());
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_MAF());
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_O2S1());
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_O2S2());
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_O2S3());
					this.LiveDataPIDs.Add(PID_SubaruSSM2DoublePID.SSM2_ATF_DeteriorationDegree());
					this.LiveDataPIDs.AddRange(PID_SubaruSSM2DoublePID.GET_SSM2_PIDS());
				}
				PID_EconomizerFSSandThrottlePosition pid_EconomizerFSSandThrottlePosition = new PID_EconomizerFSSandThrottlePosition(0)
				{
					Id = 229
				};
				this.LiveDataPIDs.Add(pid_EconomizerFSSandThrottlePosition);
				PID_CalculatedInstantFuelRate pid_CalculatedInstantFuelRate = new PID_CalculatedInstantFuelRate(pid_EconomizerFSSandThrottlePosition)
				{
					Id = 230
				};
				this.LiveDataPIDs.Add(pid_CalculatedInstantFuelRate);
				PID_CalculatedInstantFuelConsumption pid_CalculatedInstantFuelConsumption = new PID_CalculatedInstantFuelConsumption(pid_CalculatedInstantFuelRate)
				{
					Id = 231
				};
				this.LiveDataPIDs.Add(pid_CalculatedInstantFuelConsumption);
				PID_TotalFuelUsed pid_TotalFuelUsed = new PID_TotalFuelUsed(pid_CalculatedInstantFuelRate)
				{
					Id = 232
				};
				this.LiveDataPIDs.Add(pid_TotalFuelUsed);
				PID_TotalDistance pid_TotalDistance = new PID_TotalDistance
				{
					Id = 235
				};
				this.LiveDataPIDs.Add(pid_TotalDistance);
				PID_CalculatedAvgSpeed pid_CalculatedAvgSpeed = new PID_CalculatedAvgSpeed(pid_TotalDistance)
				{
					Id = 233
				};
				this.LiveDataPIDs.Add(pid_CalculatedAvgSpeed);
				PID_CalculatedAVGFuelConsumption pid_CalculatedAVGFuelConsumption = new PID_CalculatedAVGFuelConsumption(pid_TotalDistance, pid_TotalFuelUsed)
				{
					Id = 234
				};
				this.LiveDataPIDs.Add(pid_CalculatedAVGFuelConsumption);
				PID_CalculatedAccelerationFromSpeed pid_CalculatedAccelerationFromSpeed = new PID_CalculatedAccelerationFromSpeed
				{
					Id = 236
				};
				this.LiveDataPIDs.Add(pid_CalculatedAccelerationFromSpeed);
				PID_CalculatedBoost pid_CalculatedBoost = new PID_CalculatedBoost
				{
					Id = 237
				};
				this.LiveDataPIDs.Add(pid_CalculatedBoost);
				PID_CalculatedPowerFromFuelConsumption pid_CalculatedPowerFromFuelConsumption = new PID_CalculatedPowerFromFuelConsumption(pid_CalculatedInstantFuelRate)
				{
					Id = 238
				};
				this.LiveDataPIDs.Add(pid_CalculatedPowerFromFuelConsumption);
				if (SharedSettings.Current.ShowExperimental)
				{
					PID_CalculatedPowerFromAcceleration pid_CalculatedPowerFromAcceleration = new PID_CalculatedPowerFromAcceleration(pid_CalculatedAccelerationFromSpeed)
					{
						Id = 239
					};
					this.LiveDataPIDs.Add(pid_CalculatedPowerFromAcceleration);
					PID_Calculated_TorqueFromPower_Fuel pid_Calculated_TorqueFromPower_Fuel = new PID_Calculated_TorqueFromPower_Fuel(pid_CalculatedPowerFromFuelConsumption)
					{
						Id = 240
					};
					this.LiveDataPIDs.Add(pid_Calculated_TorqueFromPower_Fuel);
				}
				PID_GPSSpeed pid_GPSSpeed = new PID_GPSSpeed
				{
					Id = 241
				};
				this.LiveDataPIDs.Add(pid_GPSSpeed);
				this.LiveDataPIDs.Add(new PID_CalculatedAvgSpeedGPS(pid_GPSSpeed)
				{
					Id = 543
				});
				PID_TotalDistanceStatistics pid_TotalDistanceStatistics = new PID_TotalDistanceStatistics
				{
					Id = 630
				};
				this.LiveDataPIDs.Add(pid_TotalDistanceStatistics);
				PID_TotalFuelUsedStatistics pid_TotalFuelUsedStatistics = new PID_TotalFuelUsedStatistics(pid_TotalFuelUsed)
				{
					Id = 631
				};
				this.LiveDataPIDs.Add(pid_TotalFuelUsedStatistics);
				PID_AvgFuelConsumptionStatistics pid_AvgFuelConsumptionStatistics = new PID_AvgFuelConsumptionStatistics(pid_TotalFuelUsedStatistics, pid_TotalDistanceStatistics)
				{
					Id = 632
				};
				this.LiveDataPIDs.Add(pid_AvgFuelConsumptionStatistics);
				PID_FuelMoney pid_FuelMoney = new PID_FuelMoney(pid_TotalFuelUsed)
				{
					Id = 633
				};
				this.LiveDataPIDs.Add(pid_FuelMoney);
				PID_FuelMoneyStatistics pid_FuelMoneyStatistics = new PID_FuelMoneyStatistics(pid_FuelMoney)
				{
					Id = 634
				};
				this.LiveDataPIDs.Add(pid_FuelMoneyStatistics);
				this.LiveDataPIDs.Add(new PID_DistanceToEmpty
				{
					Id = 635
				});
				this.LiveDataPIDs.Add(new PID_EV_BatteryPower
				{
					Id = 801
				});
				this.LiveDataPIDs.Add(new PID_EV_MaxCellVoltage
				{
					Id = 802
				});
				this.LiveDataPIDs.Add(new PID_EV_MinCellVoltage
				{
					Id = 803
				});
				this.LiveDataPIDs.Add(new PID_Calculated_EV_CharghingPower
				{
					Id = 805
				});
				PID_AvgFuelConsumption10Seconds pid_AvgFuelConsumption10Seconds = new PID_AvgFuelConsumption10Seconds(pid_CalculatedAVGFuelConsumption)
				{
					Id = 804
				};
				this.LiveDataPIDs.Add(pid_AvgFuelConsumption10Seconds);
				PID_CalculatedFuelLevelLiters pid_CalculatedFuelLevelLiters = new PID_CalculatedFuelLevelLiters();
				this.LiveDataPIDs.Add(pid_CalculatedFuelLevelLiters);
				PID_PowerFromAir pid_PowerFromAir = new PID_PowerFromAir();
				this.LiveDataPIDs.Add(pid_PowerFromAir);
				this.LiveDataPIDs.Add(new PID_EV_InstantConsumptioKMKWH());
				this.LiveDataPIDs.Add(new PID_EV_InstantConsumption_KWH_100KM());
				PID_GPSAltitude pid_GPSAltitude = new PID_GPSAltitude
				{
					Id = 816
				};
				this.LiveDataPIDs.Add(pid_GPSAltitude);
				PID_CalculatedRpmX1000 pid_CalculatedRpmX = new PID_CalculatedRpmX1000
				{
					Id = 900
				};
				this.LiveDataPIDs.Add(pid_CalculatedRpmX);
				PID_CalculatedFreeSpaceInFuelTank pid_CalculatedFreeSpaceInFuelTank = new PID_CalculatedFreeSpaceInFuelTank
				{
					Id = 901
				};
				this.LiveDataPIDs.Add(pid_CalculatedFreeSpaceInFuelTank);
				PID_PowerFromRPMAndTorque pid_PowerFromRPMAndTorque = new PID_PowerFromRPMAndTorque
				{
					Id = 902
				};
				this.LiveDataPIDs.Add(pid_PowerFromRPMAndTorque);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x001B2778 File Offset: 0x001B0978
		public void AddOrRemoveNissanConsultPIDsV2(bool NissanProtocolEnabled)
		{
			bool flag = false;
			if (SharedSettings.Current.ProfileUpdateAlias == "ff233ab82c2544ad912df6e4e92adf00" || SharedSettings.Current.ProfileUpdateAlias == "Nissan Consult III" || ((SharedSettings.Current.SelectedBrand == "Nissan" || SharedSettings.Current.SelectedBrand == "Infiniti") && SharedSettings.Current.AddNissanConsult3Pids) || ((SharedSettings.Current.SelectedBrand == "Nissan" || SharedSettings.Current.SelectedBrand == "Infiniti") && (SharedSettings.Current.ProtocolNumber == 11 || SharedSettings.Current.ProtocolNumber == 43)))
			{
				flag = true;
			}
			if (this.NissanConsultPIDs.Count == 0)
			{
				this.CreateNissanConsult2PIDs();
			}
			if (flag)
			{
				using (List<PID>.Enumerator enumerator = this.NissanConsultPIDs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PID pid = enumerator.Current;
						if (!this.LiveDataPIDs.Contains(pid))
						{
							this.LiveDataPIDs.Add(pid);
						}
					}
					return;
				}
			}
			foreach (PID pid2 in this.NissanConsultPIDs)
			{
				if (this.LiveDataPIDs.Contains(pid2))
				{
					this.LiveDataPIDs.Remove(pid2);
				}
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x001B2900 File Offset: 0x001B0B00
		public List<PID> CreateNissanPidsList()
		{
			return new List<PID>(300)
			{
				PIDWithFloatValueFormula.PID_Nissan_221101_EngineCoolantTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221102_VehicleSpeed(),
				PIDWithFloatValueFormula.PID_Nissan_221103_ControlModuleVoltage(),
				PIDWithFloatValueFormula.PID_Nissan_221104_FuelTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221105_EGRTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221106_IntakeAirTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221107_TimingAdvance2(),
				PIDWithFloatValueFormula.PID_Nissan_221108_TimingAdvance3(),
				PIDWithFloatValueFormula.PID_Nissan_221109_TimingAdvance4(),
				PIDWithFloatValueFormula.PID_Nissan_22110A_TimingAdvance1(),
				PIDWithFloatValueFormula.PID_Nissan_22110B_IdleValve(),
				PIDWithFloatValueFormula.PID_Nissan_22110C_IdleValveSteps(),
				PIDWithFloatValueFormula.PID_Nissan_22110D_IdleRPMSetpoint(),
				PIDWithFloatValueFormula.PID_Nissan_22110E_MAPSensorVoltage(),
				PIDWithFloatValueFormula.PID_Nissan_22110F_EVAPSteps(),
				PIDWithFloatValueFormula.PID_Nissan_221110_EVAPPercent(),
				PIDWithFloatValueFormula.PID_Nissan_221111_FuelTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221112_EGRSteps(),
				PIDWithFloatValueFormula.PID_Nissan_221114_FuelLevel(),
				PIDWithFloatValueFormula.PID_Nissan_221117_CalculatedLoadValue(),
				PIDWithFloatValueFormula.PID_Nissan_221118_O2B1S1(),
				PIDWithFloatValueFormula.PID_Nissan_221119_O2B2S1(),
				PIDWithFloatValueFormula.PID_Nissan_22111A_O2B1S2(),
				PIDWithFloatValueFormula.PID_Nissan_22111B_O2B2S2(),
				PIDWithFloatValueFormula.PID_Nissan_22111C_ThrottlePosition1(),
				PIDWithFloatValueFormula.PID_Nissan_22111D_ThrottlePosition2(),
				PIDWithFloatValueFormula.PID_Nissan_22111E_ThrottlePosition3(),
				PIDWithFloatValueFormula.PID_Nissan_22111F_EngingeOilTemperature(),
				PIDWithFloatValueFormula.PID_Nissan_221123_STFTBank1(),
				PIDWithFloatValueFormula.PID_Nissan_221124_STFTBank2(),
				PIDWithFloatValueFormula.PID_Nissan_221125_LTFTBank1(),
				PIDWithFloatValueFormula.PID_Nissan_221126_LTFTBank2(),
				PIDWithFloatValueFormula.PID_Nissan_221150_O2SensorHeaterDuty(),
				PIDWithFloatValueFormula.PID_Nissan_221201_EngineRPM(),
				PIDWithFloatValueFormula.PID_Nissan_221203_DistanceWithMIL(),
				PIDWithFloatValueFormula.PID_Nissan_221204_MAFVoltageB1(),
				PIDWithFloatValueFormula.PID_Nissan_221205_MAFVoltageB2(),
				PIDWithFloatValueFormula.PID_Nissan_221206_InjectionTimingB1(),
				PIDWithFloatValueFormula.PID_Nissan_221207_InjectionTimingB2(),
				PIDWithFloatValueFormula.PID_Nissan_221208_BaseInjectionTiming(),
				PIDWithFloatValueFormula.PID_Nissan_221209_MAF(),
				PIDWithFloatValueFormula.PID_Nissan_22120C_FuelPressure(),
				PIDWithFloatValueFormula.PID_Nissan_22120D_AcceleratorPedalPositionSensor1Volts(),
				PIDWithFloatValueFormula.PID_Nissan_22120E_AcceleratorPedalPositionSensor2Volts(),
				PIDWithFloatValueFormula.PID_Nissan_22120F_ThrottlePositionSensor1Volts(),
				PIDWithFloatValueFormula.PID_Nissan_221210_ThrottlePositionSensor2Volts(),
				PIDWithFloatValueFormula.PID_Nissan_22121B_BaseInjectionTimingLowThreshold(),
				PIDWithFloatValueFormula.PID_Nissan_22121C_BaseInjectionTimingHighThreshold(),
				PIDWithFloatValueFormula.PID_Nissan_2212A1_VehicleSpeed(),
				new NC_CustomPID("FPCM Driver Volts", "FPCM Driver Volts", "221113", "", "", UnitsHelper.Units.volts, 0.0, 12.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.04, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("EVAP system pressure", "EVAP SYS PRES", "221115", "", "", UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Absolute pressure", "ABSOL PRES/SE", "221116", "", string.Empty, UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Exhaust gas temperature B1", "EX/G TMP S-B1", "221121", "", "", UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Exhaust gas temperature  B2", "EX/G TMP S-B2", "221122", "", "", UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM-B1", "INT/V TIM-B1", "221127", "", "", UnitsHelper.Units.grads, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM-B2", "INT/V TIM-B2", "221128", "", "", UnitsHelper.Units.grads, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Atmospheric pressure", "ATOM PRES SEN", "221129", "", "", UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("MAP SENSOR", "MAP SENSOR", "22112A", "", "", UnitsHelper.Units.volts, 0.0, 6.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("EVAP DIAG STATUS???", "EVAP DIAG STATUS", "22112B", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Timing advance correction", "Tim.adv.corr.", "22112D", "", "", UnitsHelper.Units.grads, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("Idle rpm correction", "Corr.idle", "22112E", "", "", UnitsHelper.Units.rpm, 0.0, 3200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 12.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("POS COUNT", "POS COUNT", "22112F", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Pressure regulator", "PRESS REG", "221130", "", "", UnitsHelper.Units.percent, 0.0, 130.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Injection timing angle", "FUEL INJ TIM", "221131", "", "", UnitsHelper.Units.grads, -200.0, 50.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 50.0, false, false, true, null),
				new NC_CustomPID("Injection timing angle 2", "FUEL INJ TIM", "221132", "", "", UnitsHelper.Units.grads, -200.0, 70.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 60.0, false, false, true, null),
				new NC_CustomPID("Injection timing angle 3", "FUEL INJ TIM", "221133", "", "", UnitsHelper.Units.grads, -200.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 110.0, false, false, true, null),
				new NC_CustomPID("A/F RATIO", "A/F RATIO", "221134", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 256.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B1) °CA", "INT/V TIM(B1) °CA", "221135", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -64.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B1) 2 °CA", "INT/V TIM(B1) 2 °CA", "221136", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, -128.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B2) °CA", "INT/V TIM(B2) °CA", "221137", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -128.0, false, false, true, null),
				new NC_CustomPID("INT/V SOL(B1)", "INT/V SOL(B1)", "221138", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.390625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/V SOL(B2)", "INT/V SOL(B2)", "221139", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.390625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VTC Angel Intake", "VTC.Ang.In", "22113A", "", "", UnitsHelper.Units.grads, -64.0, 65.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -64.0, false, false, true, null),
				new NC_CustomPID("VTC Angel Intake 2", "VTC.Ang.In", "22113B", "", "", UnitsHelper.Units.grads, -128.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, -128.0, false, false, true, null),
				new NC_CustomPID("IGN TIMING (3)", "IGN TIMING", "22113C", "", "", UnitsHelper.Units.grads, -150.0, 112.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 112.0, false, false, true, null),
				new NC_CustomPID("Угол впрыска топлива (4)", "FUEL INJ TIM", "22113D", "", "", UnitsHelper.Units.grads, -150.0, 112.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 112.0, false, false, true, null),
				new NC_CustomPID("EVAP DIAG STATUS", "EVAP DIAG STATUS", "221144", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("EVAP SYSTEM CLOSE STATUS", "EVAP SYSTEM CLOSE STATUS", "221146", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Lowest Short term fuel trim B1", "ST.Cor.Min(B1)", "221147", "", "", UnitsHelper.Units.percent, -100.0, 155.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, -100.0, false, false, true, null),
				new NC_CustomPID("Highest Short term fuel trim B1", "ST.Cor.Max(B1)", "221148", "", "", UnitsHelper.Units.percent, -100.0, 155.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, -100.0, false, false, true, null),
				new NC_CustomPID("A/F S1 Heater Bank 1", "A/F S1 HTR(B1)", "22114E", "", "", UnitsHelper.Units.percent, 0.0, 102.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.4, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("A/F S1 Heater Bank 2", "A/F S1 HTR(B2)", "22114F", "", "", UnitsHelper.Units.percent, 0.0, 102.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.4, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("A/C POS SIG", "A/C POS SIG", "221157", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("IGN TIMING (4)", "IGN TIMING", "22115B", "", "", UnitsHelper.Units.grads, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.75, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("IDL LNG F/T", "IDL LNG F/T", "22115C", "", "", UnitsHelper.Units.microseconds, -512.0, 510.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 4.0, 1.0, -512.0, false, false, true, null),
				new NC_CustomPID("Short term fuel trim", "ST.Cor", "22115F", "", "", UnitsHelper.Units.percent, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.78125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Long term fuel trim", "LT.Cor", "221161", "", "", UnitsHelper.Units.percent, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.78125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("IGN TIMING (5)", "IGN TIMING", "221162", "", "", UnitsHelper.Units.grads, -200.0, 60.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 60.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B2) °CA", "INT/V TIM(B2) °CA", "221163", "", "", UnitsHelper.Units.None, -64.0, 65.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -64.0, false, false, true, null),
				new NC_CustomPID("EXH/V TIM B1 °CA", "EXH/V TIM B1 °CA", "221164", "", "", UnitsHelper.Units.None, -64.0, 65.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -64.0, false, false, true, null),
				new NC_CustomPID("EXH/V TIM B2 °CA", "EXH/V TIM B2 °CA", "221165", "", "", UnitsHelper.Units.None, -64.0, 65.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, -64.0, false, false, true, null),
				new NC_CustomPID("COOLAN TEMP/S", "COOLAN TEMP/S", "221167", "", "", UnitsHelper.Units.celicium, -48.0, 144.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.75, 1.0, -48.0, false, false, true, null),
				new NC_CustomPID("VHCL SPEED SE", "Скорость SE", "221168", "", "", UnitsHelper.Units.kmh, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/A TEMP SE", "INT/A TEMP SE", "22116A", "", "", UnitsHelper.Units.celicium, -50.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.75, 1.0, -48.0, false, false, true, null),
				new NC_CustomPID("IACV-AAC/V", "IACV-AAC/V", "22116B", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.39215686274509803, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("PURG VOL C/V", "PURG VOL C/V", "22116D", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.39, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("HO2S1 (B1)", "HO2S1 (B1)", "22116E", "", "", UnitsHelper.Units.volts, -1.0, 2.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.0049, 1.0, -0.07, false, false, true, null),
				new NC_CustomPID("HO2S2 (B1)", "HO2S2 (B1)", "22116F", "", "", UnitsHelper.Units.volts, -1.0, 2.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.0049, 1.0, -0.07, false, false, true, null),
				new NC_CustomPID("ABSOL TH-P/S", "ABSOL TH-P/S", "221170", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.4165, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("MAS A/F SE-B1", "MAS A/F SE-B1", "221171", "", "", UnitsHelper.Units.volts, 0.0, 5.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.0196078431372549, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("B/FUEL SCHDL", "B/FUEL SCHDL", "221172", "", "", UnitsHelper.Units.ms, 0.0, 13.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.05, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("THRTL POS SEN", "THRTL POS SEN", "221173", "", "", UnitsHelper.Units.volts, 0.0, 5.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.0196078431372549, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("CO ADJUSTMENT", "CO ADJUSTMENT", "221174", "", "", UnitsHelper.Units.percent, -255.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("SLOW INJ", "SLOW INJ", "221175", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.39215686274509803, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("MAIN INJ", "MAIN INJ", "221176", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.39215686274509803, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("ETHANOL M/R", "ETHANOL M/R", "221177", "", "", UnitsHelper.Units.percent, 0.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FAN DUTY", "ABSOL FAN DUTY", "221178", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("AC EVA TEMP", "AC EVA TEMP", "221179", "", "", UnitsHelper.Units.celicium, -30.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.33, 1.0, -30.0, false, false, true, null),
				new NC_CustomPID("AC EVA TARGET", "AC EVA TARGET", "22117A", "", "", UnitsHelper.Units.celicium, -30.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.33, 1.0, -30.0, false, false, true, null),
				new NC_CustomPID("ALT DUTY", "ALT DUTY", "22117B", "", "", UnitsHelper.Units.percent, 0.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("ACCEL PEDAL POSI", "ACCEL PEDAL POSI", "22117C", "", "", UnitsHelper.Units.percent, 0.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VVEL SEN LEARN-B1", "VVEL SEN LEARN-B1", "22117D", "", "", UnitsHelper.Units.volts, 0.0, 7.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.026, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VVEL SEN LEARN-B2", "VVEL SEN LEARN-B2", "22117E", "", "", UnitsHelper.Units.volts, 0.0, 7.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.026, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FUEL PUMP DUTY", "FUEL PUMP DUTY", "22117F", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("IGN TIMING (6)", "IGN TIMING", "221181", "", "", UnitsHelper.Units.grads, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.75, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("BAT TEMP SEN?", "BAT TEMP SEN", "221182", "", "", UnitsHelper.Units.volts, 0.0, 20.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.02, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("GEAR POSITION", "GEAR POS", "221183", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("ALCOHOL MIX", "ALCOHOL MIX", "221184", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("ETHANOL DENS", "ETHANOL DENS", "221185", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 0.00392156862745098, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("HO2S1 HTR VOLT", "HO2S1 HTR VOLT", "221187", "", "", UnitsHelper.Units.mV, 0.0, 5200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 20.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("THRTL STK CNT B1", "THRTL STK CNT B1", "22118A", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("THRTL STK CNT B2", "THRTL STK CNT B2", "22118B", "", "", UnitsHelper.Units.None, 0.0, 255.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FUEL INJ TIMG(6)", "FUEL INJ TIMG", "22118C", "", "", UnitsHelper.Units.grads, 255.0, 80.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, -1.0, 1.0, 80.0, false, false, true, null),
				new NC_CustomPID("CKPS-RPM(POS)", "CKPS-RPM(POS)", "221202", "", "", UnitsHelper.Units.rpm, 0.0, 8000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 12.5, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Injection B1 (2)", "Inj B1", "221211", "", "", UnitsHelper.Units.ms, 0.0, 256000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0128, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Injection B2 (2)", "Inj B2", "221212", "", "", UnitsHelper.Units.ms, 0.0, 256000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0128, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("F/INJ EG STRT", "F/INJ EG STRT", "221213", "", "", UnitsHelper.Units.ms, 0.0, 256000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0128, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("BRAKE BST PRESS SE 1", "BRAKE BST PRESS SE 1", "221214", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("BRAKE BST PRESS SE 2", "BRAKE BST PRESS SE 2", "221215", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("SWL C/V (B1)", "SWL C/V (B1)", "221216", "", "", UnitsHelper.Units.None, -128.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.001953125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("SWL C/V (B2)", "SWL C/V (B2)", "221217", "", "", UnitsHelper.Units.None, -128.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.001953125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("I/P PULLY SPD", "I/P PULLY SPD", "221219", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("TMBL POS SEN", "TMBL POS SEN", "22121F", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Lowest MAF", "MAF min", "221221", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Highest MAF", "MAF max", "221222", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("AC PRESS SEN", "AC PRESS SEN", "221223", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Wideband O2 B1 S1", "A/F SEN1 (B1)", "221225", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Wideband O2 B2 S1", "A/F SEN1 (B2)", "221226", "", "", UnitsHelper.Units.volts, 0.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.005, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("SWL/C POSI SE(1)", "SWL/C POSI SE", "221227", "", "", UnitsHelper.Units.grads, 0.0, 8000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.11, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Engine Torque?", "ENG NM", "221228", "", "", UnitsHelper.Units.Nm, 0.0, 2560000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.25, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("Cruise speed", "Скорость 4", "22122A", "", "", UnitsHelper.Units.kmh, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.05625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Cruise set speed", "TGTСкор.", "22122B", "", "", UnitsHelper.Units.kmh, 0.0, 200.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.05625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VTC DTY IN B1", "VTC DTY IN B1", "22122D", "", "", UnitsHelper.Units.percent, 0.0, 6400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.09765625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VTC DTY IN B2", "VTC DTY IN B2", "22122E", "", "", UnitsHelper.Units.percent, 0.0, 6400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.09765625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VTC DTY EX B1", "VTC DTY EX B1", "22122F", "", "", UnitsHelper.Units.percent, 0.0, 6400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.09765625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("VTC DTY EX B2", "VTC DTY EX B2", "221230", "", "", UnitsHelper.Units.percent, 0.0, 6400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.09765625, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INJ PULSE-B1", "INJ PULSE-B11", "221232", "", "", UnitsHelper.Units.ms, 0.0, 1000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.001518, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("BAT CUR SEN(Norm 2600-3500 on XX)", "BAT CUR SEN", "221246", "", "", UnitsHelper.Units.mV, 2000.0, 3500.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("A/F ADJ-B1 (49)", "A/F ADJ-B1", "221249", "", "", UnitsHelper.Units.None, 0.0, 150.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.002, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("A/F ADJ-B2 (4A)", "A/F ADJ-B1", "22124A", "", "", UnitsHelper.Units.None, 0.0, 150.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.002, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("TP SEN 1-B2 (4B)", "TP SEN 1-B2", "22124B", "", "", UnitsHelper.Units.mV, 0.0, 350000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("TP SEN 2-B2 (4C)", "TP SEN 2-B2", "22124C", "", "", UnitsHelper.Units.mV, 0.0, 350000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B1) (4D) °CA", "INT/V TIM(B1) °CA", "22124D", "", "", UnitsHelper.Units.None, -16384.0, 16384.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.5, 1.0, -16384.0, false, false, true, null),
				new NC_CustomPID("INT/V TIM(B2) (4E) °CA", "INT/V TIM(B2) °CA", "22124E", "", "", UnitsHelper.Units.None, -16384.0, 16384.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.5, 1.0, -16384.0, false, false, true, null),
				new NC_CustomPID("ENG POWER RQST", "ENG POWER RQST", "221257", "", "", UnitsHelper.Units.kW, 0.0, 1000000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.03125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("ENG SPEED RQST", "ENG SPEED RQST", "221258", "", "", UnitsHelper.Units.rpm, 0.0, 1000000.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.78125, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("VVEL POSITION SEN-B1(5A)", "VVEL POSITION SEN-B1", "22125A", "", "", UnitsHelper.Units.volts, -400.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0048, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("VVEL POSITION SEN-B2(5B)", "VVEL POSITION SEN-B2", "22125B", "", "", UnitsHelper.Units.volts, -400.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0048, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("VVEL TIM-B1(5D)", "VVEL TIM-B1", "22125D", "", "", UnitsHelper.Units.grads, -400.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0055, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("VVEL TIM-B2(5E)", "VVEL TIM-B2", "22125E", "", "", UnitsHelper.Units.grads, -400.0, 400.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.0055, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("A/F LEARN-B1(61)", "A/F LEARN-B1", "221261", "", "", UnitsHelper.Units.percent, -3072.0, 3072.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.046875, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("BOOST S/V DUTY(63)", "BOOST S/V DUTY", "221263", "", "", UnitsHelper.Units.percent, -128.0, 128.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.001953125, 1.0, 0.0, true, false, true, null),
				new NC_CustomPID("ETHANOL CONS(64)", "ETHANOL CONS", "221264", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("H/P FUEL PUMP DEG", "H/P FUEL PUMP DEG", "22126A", "", "", UnitsHelper.Units.grads, -6553.6, 6553.6, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.1, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FUEL PRES SEN V", "FUEL PRES SEN V", "221277", "", "", UnitsHelper.Units.mV, 0.0, 327680.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("EOP SENSOR", "EOP SENSOR", "221278", "", "", UnitsHelper.Units.mV, 0.0, 327680.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("L/FUEL PRES SEN", "L/FUEL PRES SEN", "221279", "", "", UnitsHelper.Units.kPa, 0.0, 655.36, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("L/FUEL PRES SEN V", "L/FUEL PRES SEN V", "221279", "", "", UnitsHelper.Units.mV, 0.0, 327680.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("STRT LEARN VALUE", "STRT LEARN VALUE", "22127D", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("CML B/DCHG CRNT", "CML B/DCHG CRNT", "22127F", "", "", UnitsHelper.Units.None, 0.0, 327680.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("TRGT ALT VLTG", "TRGT ALT VLTG", "221281", "", "", UnitsHelper.Units.mV, 0.0, 1310720.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 20.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FUEL PRES 1", "FUEL PRES 1", "221282", "", "", UnitsHelper.Units.kPa, 0.0, 16.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.23374999999999999, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("FUEL PRES 2", "FUEL PRES 2", "221283", "", "", UnitsHelper.Units.kPa, 0.0, 16.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.23374999999999999, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("INJ PULSE", "INJ PULSE", "221284", "", "", UnitsHelper.Units.ms, 0.0, 655360.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.01, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("PROPANE RATIO", "PROPANE RATIO", "221285", "", "", UnitsHelper.Units.percent, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.048828125, 1.0, -100.0, false, false, true, null),
				new NC_CustomPID("A/F ADJ(86)", "A/F ADJ", "221286", "", "", UnitsHelper.Units.None, 0.0, 32.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 0.00048828125, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("A/F LRN CNTR B2(88)", "A/F LRN CNTR B2", "221288", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("A/F LRN CNTR B1(89)", "A/F LRN CNTR B1", "221289", "", "", UnitsHelper.Units.None, 0.0, 65536.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("G SENSOR", "G SENSOR", "22128A", "", "", UnitsHelper.Units.mV, 0.0, 327680.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 2, 5.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Snow", "Snow", "221301", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("Throttle closed", "CLSD THL/P SW", "221301", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("CVTC LEARNING", "CVTC LEARNING", "221301", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("Throttle closed(02)", "CLSD THL/P SW", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("Starter", "START SIGNAL", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("CVT position P/N", "P/N POSI SW", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("Power Steering", "PW/ST SIGNAL", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("Air conditioner", "AIR COND SIG", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("additional load", "LOAD SIGNAL", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("CAN CON VC SW", "CAN CON VC SW", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("AMB TEMP SW", "AMB TEMP SW", "221302", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("Shift Solenoid B", "Shift Solenoid B", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("Shift Solenoid A", "Shift Solenoid A", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("A/T Data 2 In", "A/T Data 2 In", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("A/T Data 1 In", "A/T Data 1 In", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("O2 B2 S2", "HO2S2 MNTR(B2)", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("O2 B1 S2", "HO2S2 MNTR(B1)", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("O2 B2 S1", "HO2S1 MNTR(B2)", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("O2 B1 S1", "HO2S1 MNTR(B1)", "221303", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("Susp Unload", "Susp Unload", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("A/C Pressure switch", "A/C PRESS SW", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("TCC SWITCH", "TCC SWITCH", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("OD SWITCH", "OD SWITCH", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("Heater fan switch", "HEATER FAN SW", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("CLUTCH P/P SW", "CLUTCH P/P SW", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("SWL CON VC SW", "SWL CON VC SW", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("ignition", "IGNITION SW", "221304", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("Brake switch", "BRAKE SW", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("REV UNIT DR", "REV UNIT DR", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("BOOST VCUM SW", "BOOST VCUM SW", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("FTVOLNEX", "FTVOLNEX", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("A/V learning on idle", "A/V LEARN", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("FQCAL", "FQCAL", "221305", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("VVL S/V-EXH", "VVL S/V-EXH", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("VVL S/V-INT", "VVL S/V-INT", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("INT/V SOL-B2", "INT/V SOL-B2", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("INT/V SOL-B1", "INT/V SOL-B1", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("VIAS S/V-1", "VIAS S/V-1", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("VARI DUCT S/V", "VARI DUCT S/V", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("VARI RESO S/V", "VARI RESO S/V", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("SWRL CONT S/V", "SWRL CONT S/V", "221306", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("A/T Data 3 Out", "A/T Data 3 Out", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("Idle switch", "Idle sw", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("ENGINE MOUNT", "ENGINE MOUNT", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("OD CANCEL S/V", "OD CANCEL S/V", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("TCC SOL/V", "TCC SOL/V", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("EGRC", "EGRC", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("A/C relay", "A/C relay", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("IACV-IDLE/UP", "IACV-IDLE/UP", "221307", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("Fuel pump relay", "FUEL PUMP", "221308", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("VVL S/V", "VVL S/V", "221308", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("FUEL C/V ENG", "FUEL C/V ENG", "221308", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("FUEL C/V CYL", "FUEL C/V CYL", "221308", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("EGRC (08)", "EGRC SOL/V", "221308", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("PURG CONT S/V (09)", "PURG CONT S/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("VENT CONT S/V (09)", "VENT CONT S/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("PURG CONT S/V1 (09)", "PURG CONT S/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("VC/V BYPAS S/V (09)", "VC/V BYPAS S/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("MAP/BARO SW/V (09)", "MAP/BARO SW/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("P/REG CONT/V (09)", "P/REG CONT/V", "221309", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("COLD START/V", "COLD START/V", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("IACV-FICD S/V (0A)", "P/REG CONT/V", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("AUXI CONT S/V (0A)", "AUXI CONT S/V", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("AIR PUMP RLY (0A)", "AIR PUMP RLY", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("AIR/P CNT S/V (0A)", "AIR/P CNT S/V", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("Throttle relay", "THRTL RELAY", "22130A", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("HO2S2 HTR (B2)", "HO2S2 HTR (B2)", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("HO2S2 HTR (B1)", "HO2S2 HTR (B1)", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("HO2S1 HTR (B2)", "HO2S1 HTR (B2)", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("HO2S1 HTR (B1)", "HO2S1 HTR (B1)", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("Fan Low TEST", "Fan.Low", "22130B", "", "Shr(And(A,4),2)", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Fan Hi TEST", "Fan.Hi", "22130B", "", "Shr(And(A,1),0)", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("Fan Mode", "Fan.Mod", "22130B", "", "GetBit(A,4)+GetBit(A,5)", UnitsHelper.Units.None, 0.0, 2.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null),
				new NC_CustomPID("W/G SOL/V-B2", "W/G SOL/V-B2", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("W/G SOL/V-B1", "W/G SOL/V-B1", "22130B", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("SRT STATUS: CATALYST", "SRT STATUS: CATALYST", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("SRT STATUS: HEATED CAT", "SRT STATUS: HEATED CAT", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("SRT STATUS: EVAP SYSTEM", "SRT STATUS: EVAP SYSTEM", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("SRT STATUS: S-AIR SYSTEM", "SRT STATUS: S-AIR SYSTEM", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("SRT STATUS: REFRIGERANT", "SRT STATUS: REFRIGERANT", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("SRT STATUS: HO2S", "SRT STATUS: HO2S", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("SRT STATUS: HO2S HTR", "SRT STATUS: HO2S HTR", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("SRT STATUS: EGR/VVT SYSTEM", "SRT STATUS: EGR/VVT SYSTEM", "22130C", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("MAIN SW", "MAIN SW", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("CANCEL SW", "CANCEL SW", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("RESUME/ACC SW", "RESUME/ACC SW", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("SET SW", "SET SW", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("BRAKE SW1", "BRAKE SW1", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("BRAKE SW2", "BRAKE SW2", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("DIST SW", "DIST SW", "22130D", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("VHCL SPD CUT", "VHCL SPD CUT", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("LO SPEED CUT", "LO SPEED CUT", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("CRUISE LAMP", "CRUISE LAMP", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("AT OD MONITOR", "AT OD MONITOR", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("AT OD CANCEL", "AT OD CANCEL", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("SET LAMP", "SET LAMP", "22130E", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("Initial Diagnostic", "Init.Diag.", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("Transmit Diagnostic", "Tx.Diag.", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("TCM", "TCM", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("VDC/TCS/ABS", "VDC/TCS/ABS", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("Meter/M and A", "Meter/M and A", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("ICC", "ICC", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("BCM/SEC", "BCM/SEC", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				},
				new NC_CustomPID("IPDM E/R", "IPDM E/R", "22130F", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 7
				},
				new NC_CustomPID("SCB/V CON S/V", "IPDM E/R", "221311", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("VT CONT LEARN", "VT CONT LEARN", "221311", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("EXH V/T LEARN", "EXH V/T LEARN", "221311", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("LPG CUT S/V", "LPG CUT S/V", "221312", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 0
				},
				new NC_CustomPID("SLOW CUT S/V", "SLOW CUT S/V", "221312", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("ALT DUTY SIG", "ALT DUTY SIG", "221313", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("VIAS S/V-2", "VIAS S/V-2", "221313", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4
				},
				new NC_CustomPID("BLOWER FAN SW", "BLOWER FAN SW", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 1
				},
				new NC_CustomPID("DEFOGGER SW", "DEFOGGER SW", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 2
				},
				new NC_CustomPID("FR FOG SW", "FR FOG SW", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 3
				},
				new NC_CustomPID("HI BEAM SW", "HI BEAM SW", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 4,
					Id = 903
				},
				new NC_CustomPID("LOW BEAM SW", "LOW BEAM SW", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 5
				},
				new NC_CustomPID("LIGHT SW 1ST", "LIGHT SW 1ST", "221314", "", "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.BitValue, 0, 1, 1.0, 1.0, 0.0, false, false, true, null)
				{
					Bit = 6
				}
			};
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x001B9018 File Offset: 0x001B7218
		private void CreateNissanConsult2PIDs()
		{
			this.NissanConsultPIDs.Clear();
			this.NissanConsultPIDs.AddRange(this.CreateNissanPidsList());
			for (int i = 0; i < this.NissanConsultPIDs.Count; i++)
			{
				PID pid = this.NissanConsultPIDs[i];
				pid.Id = 250 + i;
				if (pid.Id == 543)
				{
					pid.Id = 903;
				}
				pid.Name = "[NC] " + pid.Name;
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x001B90A0 File Offset: 0x001B72A0
		private void CreateMode01PIDs()
		{
			this.LiveDataPIDs = new List<PID>(600);
			this.LiveDataPIDs.Add(new DeviceTimePID
			{
				Id = 800
			});
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PIDATRV_OBDVoltage());
			this.LiveDataPIDs.Add(new PID_SupportedPids("0100", this.LiveDataPIDs, "")
			{
				Id = 1
			});
			this.LiveDataPIDs.Add(new PID_Status(PID.GetResourceString("PID_0101_Status"), "0101")
			{
				Id = 2
			});
			this.LiveDataPIDs.Add(new PID0103_FuelSystemStatus
			{
				Id = 3
			});
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0104_CalculatedEngineLoadValue());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0105_EngineCoolantTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0106_STFTB1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0107_LTFTB1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0108_STFTB2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0109_LTFTB2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010A_FuelPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010B_IntakeManifoldAbsolutePressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010C_EngineRPM());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010D_VehicleSpeed());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010E_TimingAdvance());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID010F_IntakeAirTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0110_MAFAirFlowRate());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0111_ThrottlePosition());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID0112_CommandedSecondaryAirStatus());
			this.LiveDataPIDs.Add(new PID_OxygenSensorsPresent("0113")
			{
				Id = 19
			});
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0114_O2S1B1_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0114_O2S1B1_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0115_O2S2B1_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0115_O2S2B1_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0116_O2S3B1_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0116_O2S3B1_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0117_O2S4B1_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0117_O2S4B1_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0118_O2S1B2_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0118_O2S1B2_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0119_O2S2B2_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0119_O2S2B2_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID011A_O2S3B2_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID011A_O2S3B2_Trim());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID011B_O2S4B2_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID011B_O2S4B2_Trim());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID011C_ObdStandard());
			this.LiveDataPIDs.Add(new PID_OxygenSensorsPresent("0113")
			{
				Id = 37
			});
			this.LiveDataPIDs.Add(PIDWithStringValue.PID011E_AuxillaryInputStatus());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID011F_RunTimeSinceEngineStart());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0121_DistanceTraveledWithMILLampOn());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0122_FuelRailPressure_RelativeToManifoldVacuum());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0123_FuelRailPressure_DieselOrGasolineDirectInject());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0124_O2S1WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0124_O2S1WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0125_O2S2WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0125_O2S2WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0126_O2S3WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0126_O2S3WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0127_O2S4WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0127_O2S4WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0128_O2S5WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0128_O2S5WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0129_O2S4WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0129_O2S4WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012A_O2S7WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012A_O2S7WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012B_O2S8WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012B_O2S8WR_Voltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012C_CommandedEGR());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012D_EGRError());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012E_CommandedEvaporativePurge());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID012F_FuelLevelInput());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0130_WarmupsSinceCodesCleared());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0131_DistanceTraveledSinceCodesCleared());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0132_EvapSystemVaporPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0133_BarometricPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0134_O2S1WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0134_O2S1WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0135_O2S2WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0135_O2S2WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0136_O2S3WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0136_O2S3WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0137_O2S4WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0137_O2S4WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0138_O2S5WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0138_O2S5WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0139_O2S6WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0139_O2S6WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013A_O2S7WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013A_O2S7WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013B_O2S8WR_CurrentmA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013B_O2S8WR_EqRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013C_CatalystTemperatureB1S1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013D_CatalystTemperatureB2S1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013E_CatalystTemperatureB1S2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID013F_CatalystTemperatureB2S2());
			this.LiveDataPIDs.Add(new PID_Status(PID.GetResourceString("PID_0141"), "0141")
			{
				Id = 89
			});
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0142_ControlModuleVoltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0143_AbsoluteLoadValue());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0144_FuelAirCommandedEquivalenceRatio());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0145_RelativeThrottlePosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0146_AmbientAirTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0147_AbsoluteThrottlePositionB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0148_AbsoluteThrottlePositionC());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0149_AbsolutePedalPositionD());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID014A_AbsolutePedalPositionE());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID014B_AbsolutePedalPositionF());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID014C_CommandedThrottleActuator());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID014D_TimeRunWithMILon());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID014E_TimeSinceTroubleCodesCleared());
			this.LiveDataPIDs.Add(new PID014F_MaxValues
			{
				Id = 103
			});
			this.LiveDataPIDs.Add(new PID0150_MAFMaxValues
			{
				Id = 104
			});
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0150_MaximumValueForAirFlowRateFromMassAirFlowSensor());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID0151_FuelType());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0152_EthanolFuelPercent());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0153_AbsoluteEvapSystemVaporPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0154_EvapSystemVaporPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0155_ShortTermSecondaryOxygenSensorTrimB1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0155_ShortTermSecondaryOxygenSensorTrimB3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0156_LongTermSecondaryOxygenSensorTrimB1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0156_LongTermSecondaryOxygenSensorTrimB3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0157_ShortTermSecondaryOxygenSensorTrimB2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0157_ShortTermSecondaryOxygenSensorTrimB4());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0158_LongTermSecondaryOxygenSensorTrimB2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0158_LongTermSecondaryOxygenSensorTrimB4());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0159_FuelRailPressureAbsolute());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID015A_RelativeAcceleratorPedalPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID015B_HybridBatteryPackRemainingLife());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID015C_EngineOilTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID015D_FuelInjectionTiming());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID015E_EngineFuelRate());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0161_DriversDemandEnginePercentTorque());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0162_ActualEnginePercentTorque());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0163_EngineReferenceTorque());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtIdle());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint4());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint5());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0166_MAFSensorA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0166_MAFSensorB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0167_ECTSensorA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0167_ECTSensorB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_CommandedEGRDutyCycleA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_ActualEGRDutyCycleA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_EGRErrorA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_CommandedEGRDutyCycleB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_ActualEGRDutyCycleB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0169_EGRErrorB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016A_CommandedIntakeAirFlowAControl());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016A_RelativeIntakeAirFlowAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016A_CommandedIntakeAirFlowBControl());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016A_RelativeIntakeAirFlowBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank1Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank1Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank2Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank2Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016C_CommandedThrottleActuatorAControl());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016C_RelativeThrottleAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016C_CommandedThrottleActuatorBControl());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016C_RelativeThrottleBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016D_CommandedFuelRailPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016D_FuelRailPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016D_FuelRailTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016E_CommandedInjectionControlPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016E_InjectionControlPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016F_TurbochargerCompressorInletPressureSensorA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID016F_TurbochargerCompressorInletPressureSensorB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0170_CommandedBoostPressureA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0170_BoostPressureSensorA());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0170_CommandedBoostPressureB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0170_BoostPressureSensorB());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0171_CommandedVariableGeometryTurboAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0171_VariableGeometryTurboAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0171_CommandedVariableGeometryTurboBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0171_VariableGeometryTurboBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0172_CommandedWastegateAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0172_WastegateAPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0172_CommandedWastegateBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0172_WastegateBPosition());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0173_ExhaustPressureSensorBank1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0173_ExhaustPressureSensorBank2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0174_TurbochargerARPM());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0174_TurbochargerBRPM());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerACompressorInletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerACompressorOutletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerATurbineInletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerATurbineOutletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBCompressorInletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBCompressorOutletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBTurbineInletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBTurbineOutletTemperature());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank1Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank1Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank2Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank2Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor4());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor4());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1DeltaPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1InletPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1OutletPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2DeltaPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2InletPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2OutletPressure());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank1InletTemperatureSensor());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank1OutletTemperatureSensor());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank2InletTemperatureSensor());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID017F_TotalEngineRunTime());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID017F_TotalIdleRunTime());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID017F_TotalRunTimeWithPTOActive());
			this.LiveDataPIDs.AddRange(PIDWithStringValue.GetPIDs_0181());
			this.LiveDataPIDs.AddRange(PIDWithStringValue.GetPIDs_0182());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0183_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0183_2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0184_ManifoldSurfaceTemperature());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0185());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0186());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0187());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0188_A0());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0188_A1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0188_A2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0188_A3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0188_A7());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0188());
			this.LiveDataPIDs.AddRange(PIDWithStringValue.GetPIDs_0189());
			this.LiveDataPIDs.AddRange(PIDWithStringValue.GetPIDs_018A());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID018B());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID018C());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID018D());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID018E());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID018F());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID_0190_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0190_2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0190_3());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID_0191_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0191_2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID0191_3());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0192());
			this.LiveDataPIDs.Add(ModernPIDWithFloatValue.GetPID0193());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0194());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0198());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID0199());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID_019A_1());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID019A_2_3());
			this.LiveDataPIDs.Add(PIDWithStringValue.PID_019B_1());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID019B());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID019C());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID019D_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID019D_2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID019E());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID019F());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01A1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01A2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID_01A4_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID_01A4_2());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01A5());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01A6());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01A7());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01A8());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01AA_VehicleSpeedLimit());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01AB());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01AC());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01AD());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01AE());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01AF_CommandedFreshAirFlow());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B0());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01B2_TractionBatteryPackPerformanceRetentionRate());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B3());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B4());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B5());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B6());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01B7());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01B8_TimeSinceLastCellBalancing());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01B9_BatteryMinCellVoltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01B9_BatteryMaxCellVoltage());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01BA_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01BA_2());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01BA_3());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01BB());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01BC());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01BD());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01BE());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01BF());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01C1());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01C2());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01C3());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01C4_1());
			this.LiveDataPIDs.Add(PIDWithFloatValueFormula.PID01C4_2());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01C5());
			this.LiveDataPIDs.AddRange(ModernPIDWithFloatValue.GetPID01CB());
			this.LiveDataPIDs.Add(InternalActionPID.PID_ActionReset());
			PIDWithFloatValueFormula.ResetScalingToDefaults();
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x001BA3C4 File Offset: 0x001B85C4
		public void CreateMode02PIDs()
		{
			this.Mode02PIDs = new List<PID>(255);
			this.Mode02PIDs.Add(new PID_SupportedPids("0100", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(new PID_Status(PID.GetResourceString("PID_0101_Status"), "0101"));
			this.Mode02PIDs.Add(PIDWithStringValue.PID0102_FreezFrameDTC());
			this.Mode02PIDs.Add(new PID0103_FuelSystemStatus());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0104_CalculatedEngineLoadValue());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0105_EngineCoolantTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0106_STFTB1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0107_LTFTB1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0108_STFTB2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0109_LTFTB2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010A_FuelPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010B_IntakeManifoldAbsolutePressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010C_EngineRPM());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010D_VehicleSpeed());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010E_TimingAdvance());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID010F_IntakeAirTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0110_MAFAirFlowRate());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0111_ThrottlePosition());
			this.Mode02PIDs.Add(PIDWithStringValue.PID0112_CommandedSecondaryAirStatus());
			this.Mode02PIDs.Add(new PID_OxygenSensorsPresent("0113"));
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0114_O2S1B1_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0114_O2S1B1_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0115_O2S2B1_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0115_O2S2B1_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0116_O2S3B1_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0116_O2S3B1_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0117_O2S4B1_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0117_O2S4B1_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0118_O2S1B2_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0118_O2S1B2_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0119_O2S2B2_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0119_O2S2B2_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID011A_O2S3B2_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID011A_O2S3B2_Trim());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID011B_O2S4B2_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID011B_O2S4B2_Trim());
			this.Mode02PIDs.Add(PIDWithStringValue.PID011C_ObdStandard());
			this.Mode02PIDs.Add(new PID_OxygenSensorsPresent("0113"));
			this.Mode02PIDs.Add(PIDWithStringValue.PID011E_AuxillaryInputStatus());
			this.Mode02PIDs.Add(PIDWithStringValue.PID011F_RunTimeSinceEngineStart());
			this.Mode02PIDs.Add(new PID_SupportedPids("0120", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0121_DistanceTraveledWithMILLampOn());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0122_FuelRailPressure_RelativeToManifoldVacuum());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0123_FuelRailPressure_DieselOrGasolineDirectInject());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0124_O2S1WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0124_O2S1WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0125_O2S2WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0125_O2S2WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0126_O2S3WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0126_O2S3WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0127_O2S4WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0127_O2S4WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0128_O2S5WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0128_O2S5WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0129_O2S4WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0129_O2S4WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012A_O2S7WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012A_O2S7WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012B_O2S8WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012B_O2S8WR_Voltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012C_CommandedEGR());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012D_EGRError());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012E_CommandedEvaporativePurge());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID012F_FuelLevelInput());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0130_WarmupsSinceCodesCleared());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0131_DistanceTraveledSinceCodesCleared());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0132_EvapSystemVaporPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0133_BarometricPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0134_O2S1WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0134_O2S1WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0135_O2S2WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0135_O2S2WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0136_O2S3WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0136_O2S3WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0137_O2S4WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0137_O2S4WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0138_O2S5WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0138_O2S5WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0139_O2S6WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0139_O2S6WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013A_O2S7WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013A_O2S7WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013B_O2S8WR_CurrentmA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013B_O2S8WR_EqRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013C_CatalystTemperatureB1S1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013D_CatalystTemperatureB2S1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013E_CatalystTemperatureB1S2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID013F_CatalystTemperatureB2S2());
			this.Mode02PIDs.Add(new PID_SupportedPids("0140", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(new PID_Status(PID.GetResourceString("PID_0141"), "0141"));
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0142_ControlModuleVoltage());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0143_AbsoluteLoadValue());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0144_FuelAirCommandedEquivalenceRatio());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0145_RelativeThrottlePosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0146_AmbientAirTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0147_AbsoluteThrottlePositionB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0148_AbsoluteThrottlePositionC());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0149_AbsolutePedalPositionD());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID014A_AbsolutePedalPositionE());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID014B_AbsolutePedalPositionF());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID014C_CommandedThrottleActuator());
			this.Mode02PIDs.Add(PIDWithStringValue.PID014D_TimeRunWithMILon());
			this.Mode02PIDs.Add(PIDWithStringValue.PID014E_TimeSinceTroubleCodesCleared());
			this.Mode02PIDs.Add(new PID014F_MaxValues());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0150_MaximumValueForAirFlowRateFromMassAirFlowSensor());
			this.Mode02PIDs.Add(PIDWithStringValue.PID0151_FuelType());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0152_EthanolFuelPercent());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0153_AbsoluteEvapSystemVaporPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0154_EvapSystemVaporPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0155_ShortTermSecondaryOxygenSensorTrimB1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0155_ShortTermSecondaryOxygenSensorTrimB3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0156_LongTermSecondaryOxygenSensorTrimB1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0156_LongTermSecondaryOxygenSensorTrimB3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0157_ShortTermSecondaryOxygenSensorTrimB2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0157_ShortTermSecondaryOxygenSensorTrimB4());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0158_LongTermSecondaryOxygenSensorTrimB2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0158_LongTermSecondaryOxygenSensorTrimB4());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0159_FuelRailPressureAbsolute());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID015A_RelativeAcceleratorPedalPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID015B_HybridBatteryPackRemainingLife());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID015C_EngineOilTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID015D_FuelInjectionTiming());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID015E_EngineFuelRate());
			this.Mode02PIDs.Add(new PID_SupportedPids("0160", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0161_DriversDemandEnginePercentTorque());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0162_ActualEnginePercentTorque());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0163_EngineReferenceTorque());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtIdle());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint4());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0164_EnginePercentTorqueDataAtPoint5());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0166_MAFSensorA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0166_MAFSensorB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0167_ECTSensorA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0167_ECTSensorB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB1S3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0168_IATSensorB2S3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_CommandedEGRDutyCycleA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_ActualEGRDutyCycleA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_EGRErrorA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_CommandedEGRDutyCycleB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_ActualEGRDutyCycleB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0169_EGRErrorB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016A_CommandedIntakeAirFlowAControl());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016A_RelativeIntakeAirFlowAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016A_CommandedIntakeAirFlowBControl());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016A_RelativeIntakeAirFlowBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank1Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank1Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank2Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016B_ExhaustGasRecirculationTempBank2Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016C_CommandedThrottleActuatorAControl());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016C_RelativeThrottleAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016C_CommandedThrottleActuatorBControl());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016C_RelativeThrottleBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016D_CommandedFuelRailPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016D_FuelRailPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016D_FuelRailTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016E_CommandedInjectionControlPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016E_InjectionControlPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016F_TurbochargerCompressorInletPressureSensorA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID016F_TurbochargerCompressorInletPressureSensorB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0170_CommandedBoostPressureA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0170_BoostPressureSensorA());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0170_CommandedBoostPressureB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0170_BoostPressureSensorB());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0171_CommandedVariableGeometryTurboAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0171_VariableGeometryTurboAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0171_CommandedVariableGeometryTurboBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0171_VariableGeometryTurboBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0172_CommandedWastegateAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0172_WastegateAPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0172_CommandedWastegateBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0172_WastegateBPosition());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0173_ExhaustPressureSensorBank1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0173_ExhaustPressureSensorBank2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0174_TurbochargerARPM());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0174_TurbochargerBRPM());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerACompressorInletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerACompressorOutletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerATurbineInletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0175_TurbochargerATurbineOutletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBCompressorInletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBCompressorOutletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBTurbineInletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0176_TurbochargerBTurbineOutletTemperature());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank1Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank1Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank2Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0177_ChargeAirCoolerTemperatureBank2Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0178_ExhaustGasTemperatureBank1Sensor4());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor2());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor3());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0179_ExhaustGasTemperatureBank2Sensor4());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1DeltaPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1InletPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017A_DieselParticulateFilterBank1OutletPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2DeltaPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2InletPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017B_DieselParticulateFilterBank2OutletPressure());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank1InletTemperatureSensor());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank1OutletTemperatureSensor());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID017C_DPFBank2InletTemperatureSensor());
			this.Mode02PIDs.Add(PIDWithStringValue.PID017F_TotalEngineRunTime());
			this.Mode02PIDs.Add(PIDWithStringValue.PID017F_TotalIdleRunTime());
			this.Mode02PIDs.Add(PIDWithStringValue.PID017F_TotalRunTimeWithPTOActive());
			this.Mode02PIDs.Add(new PID_SupportedPids("0180", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0183_1());
			this.Mode02PIDs.Add(PIDWithFloatValueFormula.PID0183_2());
			this.Mode02PIDs.Add(new PID_SupportedPids("01A0", this.Mode02PIDs, ""));
			this.Mode02PIDs.Add(new PID_SupportedPids("01C0", this.Mode02PIDs, ""));
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x001BB23C File Offset: 0x001B943C
		public async ValueTask<bool> Decode(string cmd, string response_header, OBDRequest request, byte[] data, TimeSpan timeStamp)
		{
			App.OBDReader.LastReadTimeStampTicks = timeStamp.Ticks;
			bool flag;
			if (this.IsPIDTestingMode)
			{
				flag = true;
			}
			else if (this.ShouldSetAsAvailable)
			{
				flag = this.DecodeWithoutRequest(cmd, data, timeStamp, request.Header, response_header);
			}
			else if (cmd.StartsWith("02", StringComparison.Ordinal))
			{
				flag = this.DecodeWithoutRequest(cmd, data, timeStamp, request.Header, response_header);
			}
			else
			{
				bool result = false;
				IReadOnlyList<PID> pids_to_decode = null;
				if (request is OBDMultiRequest)
				{
					pids_to_decode = (request as OBDMultiRequest).PIDs[cmd];
				}
				else
				{
					pids_to_decode = request.PIDs;
				}
				if (pids_to_decode != null && pids_to_decode.Count > 0)
				{
					if (pids_to_decode.Count == 1)
					{
						try
						{
							pids_to_decode[0].Decode(data, timeStamp, response_header);
							result = true;
							goto IL_0346;
						}
						catch
						{
							goto IL_0346;
						}
					}
					if (pids_to_decode.Count < 10)
					{
						using (IEnumerator<PID> enumerator = pids_to_decode.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								PID pid = enumerator.Current;
								try
								{
									pid.Decode(data, timeStamp, response_header);
									result = true;
								}
								catch
								{
								}
							}
							goto IL_0346;
						}
					}
					Task[] array = new Task[pids_to_decode.Count];
					for (int i = 0; i < pids_to_decode.Count; i++)
					{
						int num = i;
						Task task = Task.Run(delegate
						{
							try
							{
								pids_to_decode[num].Decode(data, timeStamp, response_header);
								result = true;
							}
							catch
							{
							}
						});
						array[i] = task;
					}
					await Task.WhenAll(array);
					IL_0346:
					flag = result;
				}
				else
				{
					flag = this.DecodeWithoutRequest(cmd, data, timeStamp, request.Header, response_header);
				}
			}
			return flag;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x001BB2AC File Offset: 0x001B94AC
		public bool DecodeDTC(string cmd, OBDRequest request, string response_header, byte[] data, TimeSpan timeStamp)
		{
			try
			{
				foreach (DTCItemV2 dtcitemV in DTCDecoder.DecodeData(request, response_header, data))
				{
					dtcitemV.Request = request;
					this.DTCs.Enqueue(dtcitemV);
				}
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Ff_req_ResponseReceived(OBDRequest request, string data)
		{
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x001BB324 File Offset: 0x001B9524
		public bool DecodeWithoutRequest(string cmd, byte[] data, TimeSpan timeStamp, string request_header, string response_header)
		{
			App.OBDReader.LastReadTimeStampTicks = timeStamp.Ticks;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (SharedSettings.Current.UseOBD2)
			{
				if (cmd.StartsWith("02"))
				{
					string just_pid = "01" + cmd.Substring(2, 2);
					PID[] array = this.Mode02PIDs.Where((PID x) => x.Command == just_pid).ToArray<PID>();
					int num = array.Length;
					if (num > 0)
					{
						for (int i = 0; i < num; i++)
						{
							int num2 = i;
							try
							{
								array[num2].Decode(data, timeStamp, request_header);
								flag2 = true;
							}
							catch
							{
							}
						}
					}
				}
				else
				{
					PID[] array2;
					if (App.OBDReader.IsNissanConsult2Protocol)
					{
						array2 = LiveDataPIDModel._PIDCollection.Where((PID x) => x.Command == cmd).ToArray<PID>();
					}
					else
					{
						array2 = LiveDataPIDModel._PIDCollection.Where((PID x) => !(x is CustomPID) && x.Command == cmd).ToArray<PID>();
					}
					PID[] array;
					if (array2.Length != 0)
					{
						array = array2;
					}
					else
					{
						array = this.LiveDataPIDs.Where((PID x) => x.Command == cmd).ToArray<PID>();
						if (this.ShouldSetAsAvailable)
						{
							PID[] array3 = array;
							for (int j = 0; j < array3.Length; j++)
							{
								array3[j].IsAvailable = true;
							}
						}
					}
					int num3 = array.Length;
					if (num3 > 0)
					{
						for (int k = 0; k < num3; k++)
						{
							int num4 = k;
							try
							{
								array[num4].Decode(data, timeStamp, response_header);
								flag2 = true;
							}
							catch
							{
							}
						}
					}
				}
			}
			if (CustomPIDViewModel.CurrentProfile.PidCollection.Count > 0)
			{
				CustomPID[] array4;
				if (string.IsNullOrEmpty(request_header))
				{
					array4 = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.IsAvailable && x.Command == cmd).ToArray<CustomPID>();
				}
				else
				{
					array4 = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.IsAvailable && x.Command == cmd && x.Header == request_header).ToArray<CustomPID>();
				}
				int num5 = array4.Length;
				if (num5 > 0)
				{
					for (int l = 0; l < num5; l++)
					{
						int num6 = l;
						try
						{
							array4[num6].Decode(data, timeStamp, response_header);
							flag3 = true;
						}
						catch
						{
						}
					}
				}
			}
			if (CustomPIDViewModel.CurrentCustom.PidCollection.Count > 0)
			{
				CustomPID[] array5;
				if (string.IsNullOrEmpty(request_header))
				{
					array5 = CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.IsAvailable && x.Command == cmd).ToArray<CustomPID>();
				}
				else
				{
					array5 = CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.IsAvailable && x.Command == cmd && x.Header == request_header).ToArray<CustomPID>();
				}
				int num7 = array5.Length;
				if (num7 > 0)
				{
					for (int m = 0; m < num7; m++)
					{
						int num8 = m;
						try
						{
							array5[num8].Decode(data, timeStamp, response_header);
							flag4 = true;
						}
						catch
						{
						}
					}
				}
			}
			if (flag2 || flag3 || flag4)
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x001BB658 File Offset: 0x001B9858
		public bool DecodeMode06(string PID, byte[] data, bool IsCAN)
		{
			bool flag = false;
			try
			{
				List<Mode6Test> decoded_tests = Mode6Test.DecodeFromBytes(PID, data, IsCAN);
				Device.BeginInvokeOnMainThread(delegate
				{
					foreach (Mode6Test mode6Test in decoded_tests)
					{
						this.Mode06TestCollection.Add(mode6Test);
					}
				});
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x001BB6B0 File Offset: 0x001B98B0
		private void ApplyDaihatsuFix()
		{
			foreach (PID pid in this.LiveDataPIDs)
			{
				if (pid.Command.StartsWith("01", StringComparison.Ordinal))
				{
					pid.Command = "21" + pid.Command.Substring(2) + "01";
					int num = 0;
					if (int.TryParse(pid.Command, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
					{
						pid.intCommand = num;
					}
					else
					{
						pid.intCommand = -1;
					}
					if (pid is PID_SupportedPids)
					{
						(pid as PID_SupportedPids).UpdateInterval();
					}
				}
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x001BB774 File Offset: 0x001B9974
		private void ApplyMode01Prefix()
		{
			string mode01Prefix = SharedSettings.Current.Mode01Prefix;
			PID pid = this.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 1);
			string text = pid.Command.Substring(0, pid.Command.Length - 2);
			if (text == mode01Prefix)
			{
				return;
			}
			foreach (PID pid2 in this.LiveDataPIDs)
			{
				if (pid2.Command.StartsWith(text))
				{
					string text2 = mode01Prefix + pid2.Command.Substring(text.Length);
					pid2.Command = text2;
					int num = 0;
					if (int.TryParse(pid2.Command, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
					{
						pid2.intCommand = num;
					}
					else
					{
						pid2.intCommand = -1;
					}
				}
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x001BB880 File Offset: 0x001B9A80
		public IEnumerable<IPIDFloatValue> FindPIDsByRole(Roles role)
		{
			IEnumerable<PID> enumerable = this.LiveDataPIDs.Where((PID x) => x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == role);
			IEnumerable<CustomPID> enumerable2 = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x != null && x != null && x.Role == role);
			IEnumerable<CustomPID> enumerable3 = CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x != null && x.Role == role && x != null);
			return (from x in enumerable.Concat(enumerable2).Concat(enumerable3)
				select (IPIDFloatValue)x).ToList<IPIDFloatValue>();
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x001BB920 File Offset: 0x001B9B20
		public IPIDFloatValue FindPIDByRole(Roles role)
		{
			IPIDFloatValue ipidfloatValue = null;
			if (this._findByRoleCache.TryGetValue(role, out ipidfloatValue))
			{
				return ipidfloatValue;
			}
			try
			{
				if (!SharedSettings.Current.SensorsSearchOrderDefault)
				{
					ipidfloatValue = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Role == role && x != null);
					if (ipidfloatValue == null)
					{
						ipidfloatValue = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && x != null && x.Role == role);
						if (ipidfloatValue == null)
						{
							ipidfloatValue = this.LiveDataPIDs.FirstOrDefault((PID x) => x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == role) as IPIDFloatValue;
						}
					}
				}
				else
				{
					ipidfloatValue = this.LiveDataPIDs.FirstOrDefault((PID x) => x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == role) as IPIDFloatValue;
					if (ipidfloatValue == null)
					{
						ipidfloatValue = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && x != null && x.Role == role);
						if (ipidfloatValue == null)
						{
							ipidfloatValue = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Role == role && x != null);
						}
					}
				}
				if (ipidfloatValue != null)
				{
					object findByRoleLockObj = this._findByRoleLockObj;
					lock (findByRoleLockObj)
					{
						this._findByRoleCache[role] = ipidfloatValue;
					}
				}
			}
			catch (Exception)
			{
			}
			if (ipidfloatValue == null)
			{
				return null;
			}
			return ipidfloatValue;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x001BBA74 File Offset: 0x001B9C74
		public IPIDFloatValue FindPIDByRole(Roles role, IPIDFloatValue default_pid)
		{
			IPIDFloatValue ipidfloatValue = this.FindPIDByRole(role);
			if (ipidfloatValue == null)
			{
				return default_pid;
			}
			return ipidfloatValue;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x001BBA90 File Offset: 0x001B9C90
		internal IPID FindPIDById(int id)
		{
			IPID ipid = null;
			if (this._findByIdCacheDict.TryGetValue(id, out ipid))
			{
				return ipid;
			}
			ipid = this.LiveDataPIDs.FirstOrDefault((PID x) => x != null && x.Id == id);
			if (ipid == null)
			{
				ipid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Id == id);
			}
			if (ipid == null)
			{
				ipid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Id == id);
			}
			if (ipid != null)
			{
				object idCacheLockObject = this._idCacheLockObject;
				lock (idCacheLockObject)
				{
					this._findByIdCacheDict[id] = ipid;
				}
			}
			return ipid;
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x001BBB5C File Offset: 0x001B9D5C
		internal IPID FindPIDByName(string name)
		{
			IPID ipid = null;
			if (this._findByNameCacheDict.TryGetValue(name, out ipid))
			{
				return ipid;
			}
			ipid = this.LiveDataPIDs.FirstOrDefault((PID x) => x != null && x.Name == name);
			if (ipid == null)
			{
				ipid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Name == name);
			}
			if (ipid == null)
			{
				ipid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x != null && x.Name == name);
			}
			if (ipid != null)
			{
				object nameCacheLockObject = this._nameCacheLockObject;
				lock (nameCacheLockObject)
				{
					this._findByNameCacheDict[name] = ipid;
				}
			}
			return ipid;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x001BBC28 File Offset: 0x001B9E28
		internal void ClearFindCache()
		{
			try
			{
				this._findByIdCacheDict.Clear();
				this._findByNameCacheDict.Clear();
				this._findByRoleCache.Clear();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x001BBC6C File Offset: 0x001B9E6C
		public void ResetFuelDistanceSpeed()
		{
			if (this.LiveDataPIDs == null)
			{
				return;
			}
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				PID_CalculatedAVGFuelConsumption pid_CalculatedAVGFuelConsumption = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption) as PID_CalculatedAVGFuelConsumption;
				PID_CalculatedAvgSpeed pid_CalculatedAvgSpeed = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_CalculatedAvgSpeed) as PID_CalculatedAvgSpeed;
				PID_TotalFuelUsed pid_TotalFuelUsed = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalFuelUsed) as PID_TotalFuelUsed;
				PID_TotalDistance pid_TotalDistance = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistance) as PID_TotalDistance;
				if (pid_TotalDistance != null)
				{
					pid_TotalDistance.ResetValues();
				}
				if (pid_TotalFuelUsed != null)
				{
					pid_TotalFuelUsed.ResetValues();
				}
				if (pid_CalculatedAvgSpeed != null)
				{
					pid_CalculatedAvgSpeed.ResetValues();
				}
				if (pid_CalculatedAVGFuelConsumption == null)
				{
					return;
				}
				pid_CalculatedAVGFuelConsumption.ResetValues();
			});
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x001BBC88 File Offset: 0x001B9E88
		[CompilerGenerated]
		private void <ResetAvailablePids>b__30_0()
		{
			this.Mode06TestCollection.Clear();
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x001BBC98 File Offset: 0x001B9E98
		[CompilerGenerated]
		private void <ResetFuelDistanceSpeed>b__60_0()
		{
			PID_CalculatedAVGFuelConsumption pid_CalculatedAVGFuelConsumption = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption) as PID_CalculatedAVGFuelConsumption;
			PID_CalculatedAvgSpeed pid_CalculatedAvgSpeed = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_CalculatedAvgSpeed) as PID_CalculatedAvgSpeed;
			PID_TotalFuelUsed pid_TotalFuelUsed = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalFuelUsed) as PID_TotalFuelUsed;
			PID_TotalDistance pid_TotalDistance = this.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_TotalDistance) as PID_TotalDistance;
			if (pid_TotalDistance != null)
			{
				pid_TotalDistance.ResetValues();
			}
			if (pid_TotalFuelUsed != null)
			{
				pid_TotalFuelUsed.ResetValues();
			}
			if (pid_CalculatedAvgSpeed != null)
			{
				pid_CalculatedAvgSpeed.ResetValues();
			}
			if (pid_CalculatedAVGFuelConsumption == null)
			{
				return;
			}
			pid_CalculatedAVGFuelConsumption.ResetValues();
		}

		// Token: 0x04001149 RID: 4425
		public bool EmptyPIDsCreated;

		// Token: 0x0400114A RID: 4426
		[CompilerGenerated]
		private ConcurrentQueue<DTCItemV2> <DTCs>k__BackingField;

		// Token: 0x0400114B RID: 4427
		public bool IsPIDTestingMode;

		// Token: 0x0400114C RID: 4428
		[CompilerGenerated]
		private DataRecorderV2 <Recorder>k__BackingField;

		// Token: 0x0400114D RID: 4429
		public bool ShouldSetAsAvailable;

		// Token: 0x0400114E RID: 4430
		[CompilerGenerated]
		private CarDataChanged CarDataChanged;

		// Token: 0x0400114F RID: 4431
		public ObservableCollection<Mode6Test> Mode06TestCollection;

		// Token: 0x04001150 RID: 4432
		private List<PID> _LiveDataPIDs;

		// Token: 0x04001151 RID: 4433
		public List<PID> Mode02PIDs;

		// Token: 0x04001152 RID: 4434
		public List<PID> NissanConsultPIDs;

		// Token: 0x04001153 RID: 4435
		[CompilerGenerated]
		private int <FreezeFrameNumber>k__BackingField;

		// Token: 0x04001154 RID: 4436
		private object createLockObject = new object();

		// Token: 0x04001155 RID: 4437
		public const int LOWEST_NISSAN_ID = 250;

		// Token: 0x04001156 RID: 4438
		public const int HIGHEST_NISSAN_ID = 542;

		// Token: 0x04001157 RID: 4439
		private object dtc_lock = new object();

		// Token: 0x04001158 RID: 4440
		private Dictionary<Roles, IPIDFloatValue> _findByRoleCache = new Dictionary<Roles, IPIDFloatValue>();

		// Token: 0x04001159 RID: 4441
		private object _findByRoleLockObj = new object();

		// Token: 0x0400115A RID: 4442
		private Dictionary<int, IPID> _findByIdCacheDict = new Dictionary<int, IPID>();

		// Token: 0x0400115B RID: 4443
		private object _idCacheLockObject = new object();

		// Token: 0x0400115C RID: 4444
		private Dictionary<string, IPID> _findByNameCacheDict = new Dictionary<string, IPID>();

		// Token: 0x0400115D RID: 4445
		private object _nameCacheLockObject = new object();

		// Token: 0x020002F4 RID: 756
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060023AC RID: 9132 RVA: 0x001BBD8C File Offset: 0x001B9F8C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060023AD RID: 9133 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060023AE RID: 9134 RVA: 0x000ABFE4 File Offset: 0x000AA1E4
			internal bool <InitializeCalculatedPIDs>b__31_0(PID x)
			{
				return x is CalculatedPIDV2;
			}

			// Token: 0x060023AF RID: 9135 RVA: 0x000ABFEF File Offset: 0x000AA1EF
			internal CalculatedPIDV2 <InitializeCalculatedPIDs>b__31_1(PID x)
			{
				return x as CalculatedPIDV2;
			}

			// Token: 0x060023B0 RID: 9136 RVA: 0x001BBD98 File Offset: 0x001B9F98
			internal bool <ApplyMode01Prefix>b__47_0(PID x)
			{
				return x.Id == 1;
			}

			// Token: 0x060023B1 RID: 9137 RVA: 0x0004C724 File Offset: 0x0004A924
			internal IPIDFloatValue <FindPIDsByRole>b__48_3(PID x)
			{
				return (IPIDFloatValue)x;
			}

			// Token: 0x060023B2 RID: 9138 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <ResetFuelDistanceSpeed>b__60_1(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x060023B3 RID: 9139 RVA: 0x000AC032 File Offset: 0x000AA232
			internal bool <ResetFuelDistanceSpeed>b__60_2(PID x)
			{
				return x is PID_CalculatedAvgSpeed;
			}

			// Token: 0x060023B4 RID: 9140 RVA: 0x000AC048 File Offset: 0x000AA248
			internal bool <ResetFuelDistanceSpeed>b__60_3(PID x)
			{
				return x is PID_TotalFuelUsed;
			}

			// Token: 0x060023B5 RID: 9141 RVA: 0x000AC03D File Offset: 0x000AA23D
			internal bool <ResetFuelDistanceSpeed>b__60_4(PID x)
			{
				return x is PID_TotalDistance;
			}

			// Token: 0x0400115E RID: 4446
			public static readonly CarData.<>c <>9 = new CarData.<>c();

			// Token: 0x0400115F RID: 4447
			public static Func<PID, bool> <>9__31_0;

			// Token: 0x04001160 RID: 4448
			public static Func<PID, CalculatedPIDV2> <>9__31_1;

			// Token: 0x04001161 RID: 4449
			public static Func<PID, bool> <>9__47_0;

			// Token: 0x04001162 RID: 4450
			public static Func<PID, IPIDFloatValue> <>9__48_3;

			// Token: 0x04001163 RID: 4451
			public static Func<PID, bool> <>9__60_1;

			// Token: 0x04001164 RID: 4452
			public static Func<PID, bool> <>9__60_2;

			// Token: 0x04001165 RID: 4453
			public static Func<PID, bool> <>9__60_3;

			// Token: 0x04001166 RID: 4454
			public static Func<PID, bool> <>9__60_4;
		}

		// Token: 0x020002F5 RID: 757
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x060023B6 RID: 9142 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x060023B7 RID: 9143 RVA: 0x001BBDA3 File Offset: 0x001B9FA3
			internal void <OnCarDataChanged>b__0()
			{
				this.myEvent(this.PIDChanged);
			}

			// Token: 0x04001167 RID: 4455
			public CarDataChanged myEvent;

			// Token: 0x04001168 RID: 4456
			public PID PIDChanged;
		}

		// Token: 0x020002F6 RID: 758
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x060023B8 RID: 9144 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x04001169 RID: 4457
			public IReadOnlyList<PID> pids_to_decode;

			// Token: 0x0400116A RID: 4458
			public byte[] data;

			// Token: 0x0400116B RID: 4459
			public TimeSpan timeStamp;

			// Token: 0x0400116C RID: 4460
			public string response_header;

			// Token: 0x0400116D RID: 4461
			public bool result;
		}

		// Token: 0x020002F7 RID: 759
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_1
		{
			// Token: 0x060023B9 RID: 9145 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_1()
			{
			}

			// Token: 0x060023BA RID: 9146 RVA: 0x001BBDB8 File Offset: 0x001B9FB8
			internal void <Decode>b__0()
			{
				try
				{
					this.CS$<>8__locals1.pids_to_decode[this.num].Decode(this.CS$<>8__locals1.data, this.CS$<>8__locals1.timeStamp, this.CS$<>8__locals1.response_header);
					this.CS$<>8__locals1.result = true;
				}
				catch
				{
				}
			}

			// Token: 0x0400116E RID: 4462
			public int num;

			// Token: 0x0400116F RID: 4463
			public CarData.<>c__DisplayClass40_0 CS$<>8__locals1;
		}

		// Token: 0x020002F8 RID: 760
		[CompilerGenerated]
		private sealed class <>c__DisplayClass44_0
		{
			// Token: 0x060023BB RID: 9147 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass44_0()
			{
			}

			// Token: 0x060023BC RID: 9148 RVA: 0x001BBE24 File Offset: 0x001BA024
			internal bool <DecodeWithoutRequest>b__1(PID x)
			{
				return x.Command == this.cmd;
			}

			// Token: 0x060023BD RID: 9149 RVA: 0x001BBE37 File Offset: 0x001BA037
			internal bool <DecodeWithoutRequest>b__2(PID x)
			{
				return !(x is CustomPID) && x.Command == this.cmd;
			}

			// Token: 0x060023BE RID: 9150 RVA: 0x001BBE24 File Offset: 0x001BA024
			internal bool <DecodeWithoutRequest>b__3(PID x)
			{
				return x.Command == this.cmd;
			}

			// Token: 0x060023BF RID: 9151 RVA: 0x001BBE54 File Offset: 0x001BA054
			internal bool <DecodeWithoutRequest>b__4(CustomPID x)
			{
				return x.IsAvailable && x.Command == this.cmd;
			}

			// Token: 0x060023C0 RID: 9152 RVA: 0x001BBE71 File Offset: 0x001BA071
			internal bool <DecodeWithoutRequest>b__5(CustomPID x)
			{
				return x.IsAvailable && x.Command == this.cmd && x.Header == this.request_header;
			}

			// Token: 0x060023C1 RID: 9153 RVA: 0x001BBE54 File Offset: 0x001BA054
			internal bool <DecodeWithoutRequest>b__6(CustomPID x)
			{
				return x.IsAvailable && x.Command == this.cmd;
			}

			// Token: 0x060023C2 RID: 9154 RVA: 0x001BBE71 File Offset: 0x001BA071
			internal bool <DecodeWithoutRequest>b__7(CustomPID x)
			{
				return x.IsAvailable && x.Command == this.cmd && x.Header == this.request_header;
			}

			// Token: 0x04001170 RID: 4464
			public string cmd;

			// Token: 0x04001171 RID: 4465
			public string request_header;
		}

		// Token: 0x020002F9 RID: 761
		[CompilerGenerated]
		private sealed class <>c__DisplayClass44_1
		{
			// Token: 0x060023C3 RID: 9155 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass44_1()
			{
			}

			// Token: 0x060023C4 RID: 9156 RVA: 0x001BBEA1 File Offset: 0x001BA0A1
			internal bool <DecodeWithoutRequest>b__0(PID x)
			{
				return x.Command == this.just_pid;
			}

			// Token: 0x04001172 RID: 4466
			public string just_pid;
		}

		// Token: 0x020002FA RID: 762
		[CompilerGenerated]
		private sealed class <>c__DisplayClass45_0
		{
			// Token: 0x060023C5 RID: 9157 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass45_0()
			{
			}

			// Token: 0x060023C6 RID: 9158 RVA: 0x001BBEB4 File Offset: 0x001BA0B4
			internal void <DecodeMode06>b__0()
			{
				foreach (Mode6Test mode6Test in this.decoded_tests)
				{
					this.<>4__this.Mode06TestCollection.Add(mode6Test);
				}
			}

			// Token: 0x04001173 RID: 4467
			public CarData <>4__this;

			// Token: 0x04001174 RID: 4468
			public List<Mode6Test> decoded_tests;
		}

		// Token: 0x020002FB RID: 763
		[CompilerGenerated]
		private sealed class <>c__DisplayClass48_0
		{
			// Token: 0x060023C7 RID: 9159 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass48_0()
			{
			}

			// Token: 0x060023C8 RID: 9160 RVA: 0x001BBF14 File Offset: 0x001BA114
			internal bool <FindPIDsByRole>b__0(PID x)
			{
				return x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == this.role;
			}

			// Token: 0x060023C9 RID: 9161 RVA: 0x001BBF39 File Offset: 0x001BA139
			internal bool <FindPIDsByRole>b__1(CustomPID x)
			{
				return x != null && x != null && x.Role == this.role;
			}

			// Token: 0x060023CA RID: 9162 RVA: 0x001BBF51 File Offset: 0x001BA151
			internal bool <FindPIDsByRole>b__2(CustomPID x)
			{
				return x != null && x.Role == this.role && x != null;
			}

			// Token: 0x04001175 RID: 4469
			public Roles role;
		}

		// Token: 0x020002FC RID: 764
		[CompilerGenerated]
		private sealed class <>c__DisplayClass51_0
		{
			// Token: 0x060023CB RID: 9163 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass51_0()
			{
			}

			// Token: 0x060023CC RID: 9164 RVA: 0x001BBF6A File Offset: 0x001BA16A
			internal bool <FindPIDByRole>b__0(CustomPID x)
			{
				return x != null && x.Role == this.role && x != null;
			}

			// Token: 0x060023CD RID: 9165 RVA: 0x001BBF83 File Offset: 0x001BA183
			internal bool <FindPIDByRole>b__1(CustomPID x)
			{
				return x != null && x != null && x.Role == this.role;
			}

			// Token: 0x060023CE RID: 9166 RVA: 0x001BBF9B File Offset: 0x001BA19B
			internal bool <FindPIDByRole>b__2(PID x)
			{
				return x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == this.role;
			}

			// Token: 0x060023CF RID: 9167 RVA: 0x001BBF9B File Offset: 0x001BA19B
			internal bool <FindPIDByRole>b__3(PID x)
			{
				return x != null && x is IPIDFloatValue && x.IsAvailable && x.Role == this.role;
			}

			// Token: 0x060023D0 RID: 9168 RVA: 0x001BBF83 File Offset: 0x001BA183
			internal bool <FindPIDByRole>b__4(CustomPID x)
			{
				return x != null && x != null && x.Role == this.role;
			}

			// Token: 0x060023D1 RID: 9169 RVA: 0x001BBF6A File Offset: 0x001BA16A
			internal bool <FindPIDByRole>b__5(CustomPID x)
			{
				return x != null && x.Role == this.role && x != null;
			}

			// Token: 0x04001176 RID: 4470
			public Roles role;
		}

		// Token: 0x020002FD RID: 765
		[CompilerGenerated]
		private sealed class <>c__DisplayClass55_0
		{
			// Token: 0x060023D2 RID: 9170 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass55_0()
			{
			}

			// Token: 0x060023D3 RID: 9171 RVA: 0x001BBFC0 File Offset: 0x001BA1C0
			internal bool <FindPIDById>b__0(PID x)
			{
				return x != null && x.Id == this.id;
			}

			// Token: 0x060023D4 RID: 9172 RVA: 0x001BBFC0 File Offset: 0x001BA1C0
			internal bool <FindPIDById>b__1(CustomPID x)
			{
				return x != null && x.Id == this.id;
			}

			// Token: 0x060023D5 RID: 9173 RVA: 0x001BBFC0 File Offset: 0x001BA1C0
			internal bool <FindPIDById>b__2(CustomPID x)
			{
				return x != null && x.Id == this.id;
			}

			// Token: 0x04001177 RID: 4471
			public int id;
		}

		// Token: 0x020002FE RID: 766
		[CompilerGenerated]
		private sealed class <>c__DisplayClass58_0
		{
			// Token: 0x060023D6 RID: 9174 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass58_0()
			{
			}

			// Token: 0x060023D7 RID: 9175 RVA: 0x001BBFD5 File Offset: 0x001BA1D5
			internal bool <FindPIDByName>b__0(PID x)
			{
				return x != null && x.Name == this.name;
			}

			// Token: 0x060023D8 RID: 9176 RVA: 0x001BBFD5 File Offset: 0x001BA1D5
			internal bool <FindPIDByName>b__1(CustomPID x)
			{
				return x != null && x.Name == this.name;
			}

			// Token: 0x060023D9 RID: 9177 RVA: 0x001BBFD5 File Offset: 0x001BA1D5
			internal bool <FindPIDByName>b__2(CustomPID x)
			{
				return x != null && x.Name == this.name;
			}

			// Token: 0x04001178 RID: 4472
			public string name;
		}

		// Token: 0x020002FF RID: 767
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Decode>d__40 : IAsyncStateMachine
		{
			// Token: 0x060023DA RID: 9178 RVA: 0x001BBFF0 File Offset: 0x001BA1F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarData carData = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new CarData.<>c__DisplayClass40_0();
						CS$<>8__locals1.data = data;
						CS$<>8__locals1.timeStamp = timeStamp;
						CS$<>8__locals1.response_header = response_header;
						App.OBDReader.LastReadTimeStampTicks = CS$<>8__locals1.timeStamp.Ticks;
						if (carData.IsPIDTestingMode)
						{
							flag = true;
							goto IL_03AF;
						}
						if (carData.ShouldSetAsAvailable)
						{
							flag = carData.DecodeWithoutRequest(cmd, CS$<>8__locals1.data, CS$<>8__locals1.timeStamp, request.Header, CS$<>8__locals1.response_header);
							goto IL_03AF;
						}
						if (cmd.StartsWith("02", StringComparison.Ordinal))
						{
							flag = carData.DecodeWithoutRequest(cmd, CS$<>8__locals1.data, CS$<>8__locals1.timeStamp, request.Header, CS$<>8__locals1.response_header);
							goto IL_03AF;
						}
						CS$<>8__locals1.result = false;
						CS$<>8__locals1.pids_to_decode = null;
						if (request is OBDMultiRequest)
						{
							CS$<>8__locals1.pids_to_decode = (request as OBDMultiRequest).PIDs[cmd];
						}
						else
						{
							CS$<>8__locals1.pids_to_decode = request.PIDs;
						}
						if (CS$<>8__locals1.pids_to_decode == null || CS$<>8__locals1.pids_to_decode.Count <= 0)
						{
							flag = carData.DecodeWithoutRequest(cmd, CS$<>8__locals1.data, CS$<>8__locals1.timeStamp, request.Header, CS$<>8__locals1.response_header);
							goto IL_03AF;
						}
						if (CS$<>8__locals1.pids_to_decode.Count == 1)
						{
							try
							{
								CS$<>8__locals1.pids_to_decode[0].Decode(CS$<>8__locals1.data, CS$<>8__locals1.timeStamp, CS$<>8__locals1.response_header);
								CS$<>8__locals1.result = true;
								goto IL_0346;
							}
							catch
							{
								goto IL_0346;
							}
						}
						if (CS$<>8__locals1.pids_to_decode.Count < 10)
						{
							IEnumerator<PID> enumerator = CS$<>8__locals1.pids_to_decode.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									PID pid = enumerator.Current;
									try
									{
										pid.Decode(CS$<>8__locals1.data, CS$<>8__locals1.timeStamp, CS$<>8__locals1.response_header);
										CS$<>8__locals1.result = true;
									}
									catch
									{
									}
								}
								goto IL_0346;
							}
							finally
							{
								if (num < 0 && enumerator != null)
								{
									enumerator.Dispose();
								}
							}
						}
						Task[] array = new Task[CS$<>8__locals1.pids_to_decode.Count];
						for (int i = 0; i < CS$<>8__locals1.pids_to_decode.Count; i++)
						{
							Task task = Task.Run(new Action(new CarData.<>c__DisplayClass40_1
							{
								CS$<>8__locals1 = CS$<>8__locals1,
								num = i
							}.<Decode>b__0));
							array[i] = task;
						}
						taskAwaiter = Task.WhenAll(array).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarData.<Decode>d__40>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					IL_0346:
					flag = CS$<>8__locals1.result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_03AF:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060023DB RID: 9179 RVA: 0x001BC42C File Offset: 0x001BA62C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001179 RID: 4473
			public int <>1__state;

			// Token: 0x0400117A RID: 4474
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400117B RID: 4475
			public byte[] data;

			// Token: 0x0400117C RID: 4476
			public TimeSpan timeStamp;

			// Token: 0x0400117D RID: 4477
			public string response_header;

			// Token: 0x0400117E RID: 4478
			public CarData <>4__this;

			// Token: 0x0400117F RID: 4479
			public string cmd;

			// Token: 0x04001180 RID: 4480
			public OBDRequest request;

			// Token: 0x04001181 RID: 4481
			private CarData.<>c__DisplayClass40_0 <>8__1;

			// Token: 0x04001182 RID: 4482
			private TaskAwaiter <>u__1;
		}
	}
}
