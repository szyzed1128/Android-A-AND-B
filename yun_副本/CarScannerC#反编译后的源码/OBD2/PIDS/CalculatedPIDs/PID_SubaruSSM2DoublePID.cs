using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000467 RID: 1127
	public class PID_SubaruSSM2DoublePID : CalculatedPIDV2
	{
		// Token: 0x06002EA5 RID: 11941 RVA: 0x002065D8 File Offset: 0x002047D8
		private PID_SubaruSSM2DoublePID(string command, string header, string name, string shortName, UnitsHelper.Units units, Roles role, string pid1, string pid2, Func<double, double, double> decode_action)
			: base(name, "", units, 0.0, 1.0, Roles.None)
		{
			this.Command = command;
			base.ShortName = shortName;
			base.Role = role;
			this.s_pid_A = pid1;
			this.s_pid_B = pid2;
			this.decode_action = decode_action;
			base.Minimum = 0.0;
			base.Maximum = 200.0;
			this.pid_A = new CustomPID("CARSCANNERGENERATEDPID", "GEN" + this.s_pid_A, this.s_pid_A, header, "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, false, null);
			this.pid_B = new CustomPID("CARSCANNERGENERATEDPID", "GEN" + this.s_pid_B, this.s_pid_B, header, "", UnitsHelper.Units.None, 0.0, 1.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, false, false, null);
		}

		// Token: 0x17001259 RID: 4697
		// (get) Token: 0x06002EA6 RID: 11942 RVA: 0x0020672D File Offset: 0x0020492D
		// (set) Token: 0x06002EA7 RID: 11943 RVA: 0x000027D4 File Offset: 0x000009D4
		public override bool IsAvailable
		{
			get
			{
				return SharedSettings.Current.BrandAndProfile.Contains("SSM2");
			}
			set
			{
			}
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x00206748 File Offset: 0x00204948
		public override void Initialize()
		{
			if (SharedSettings.Current.BrandAndProfile.Contains("SSM2"))
			{
				this.requiredPIDs.Clear();
				if (this.pid_A != null && this.pid_B != null)
				{
					this.requiredPIDs.Add(this.pid_A as PID);
					this.requiredPIDs.Add(this.pid_B as PID);
					base.DependencyPID = this.pid_B;
					this.IsAvailable = true;
				}
				else
				{
					this.IsAvailable = false;
				}
			}
			else
			{
				this.IsAvailable = false;
			}
			base.Initialize();
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x002067DD File Offset: 0x002049DD
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			result = this.decode_action(this.pid_A.Value, this.pid_B.Value);
			return true;
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x00206804 File Offset: 0x00204A04
		public static PID_SubaruSSM2DoublePID SSM2_RPM()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_RPM", "7E0", Translate.GetString("PID_010C"), Translate.GetString("PID_010C"), UnitsHelper.Units.rpm, Roles.RPM, "A80000000E", "A80000000F", (double _A, double _B) => Math.Round((_A * 256.0 + _B) / 4.0))
			{
				Id = 636
			};
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x0020686C File Offset: 0x00204A6C
		public static PID_SubaruSSM2DoublePID SSM2_MAF()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_MAF", "7E0", Translate.GetString("PID_0110"), Translate.GetString("PID_0110_Short"), UnitsHelper.Units.grams_sec, Roles.MAF, "A800000013", "A800000014", (double _A, double _B) => (_A * 256.0 + _B) / 100.0)
			{
				Id = 637
			};
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x002068D4 File Offset: 0x00204AD4
		public static PID_SubaruSSM2DoublePID SSM2_O2S1()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_O2S1", "7E0", Translate.GetString("PID_0114"), Translate.GetString("PID_0114"), UnitsHelper.Units.volts, Roles.None, "A800000016", "A800000017", (double _A, double _B) => (_A * 256.0 + _B) * 0.005)
			{
				Id = 638
			};
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x0020693C File Offset: 0x00204B3C
		public static PID_SubaruSSM2DoublePID SSM2_O2S2()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_O2S2", "7E0", Translate.GetString("PID_0115"), Translate.GetString("PID_0115"), UnitsHelper.Units.volts, Roles.None, "A800000018", "A800000019", (double _A, double _B) => (_A * 256.0 + _B) * 0.005)
			{
				Id = 639
			};
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x002069A4 File Offset: 0x00204BA4
		public static PID_SubaruSSM2DoublePID SSM2_O2S3()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_O2S3", "7E0", Translate.GetString("PID_0118"), Translate.GetString("PID_0118"), UnitsHelper.Units.volts, Roles.None, "A80000001A", "A80000001B", (double _A, double _B) => (_A * 256.0 + _B) * 0.005)
			{
				Id = 640
			};
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x00206A0C File Offset: 0x00204C0C
		public static PID_SubaruSSM2DoublePID SSM2_ATF_DeteriorationDegree()
		{
			return new PID_SubaruSSM2DoublePID("SSM2_ATF_DeteriorationDegree", "7E1", "ATF deterioration", "ATF deterioration", UnitsHelper.Units.percent, Roles.None, "A800000296", "A800000297", (double _A, double _B) => (_A * 256.0 + _B) * 40.0 / 13107.0)
			{
				Id = 641
			};
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x00206A6C File Offset: 0x00204C6C
		public static IEnumerable<PID_SubaruSSM2DoublePID> GET_SSM2_PIDS()
		{
			List<PID_SubaruSSM2DoublePID> list = new List<PID_SubaruSSM2DoublePID>();
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_ActualFuelPumpCurrent", "7E0", "Actual fuel pump current", "Actual fuel pump current", UnitsHelper.Units.mA, Roles.None, "A8000001F8", "A8000001F9", (double _A, double _B) => _A * 256.0 + _B)
			{
				Id = 800
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_ActualFuelPumpCurrent", "7E0", "DPF regeneration count", "DPF regeneration count", UnitsHelper.Units.None, Roles.None, "A80000029D", "A80000029E", (double _A, double _B) => _A * 256.0 + _B)
			{
				Id = 801
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_FinalInjectionAmount", "7E0", "Final injection amount", "Final injection amount", UnitsHelper.Units.mm3, Roles.None, "A8000001E2", "A8000001E3", (double _A, double _B) => (_A * 256.0 + _B) * 0.0039)
			{
				Id = 802
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_FinalMainInjectionPeriod", "7E0", "Final main injection period", "Final main injection period", UnitsHelper.Units.ms, Roles.None, "A800000257", "A800000258", (double _A, double _B) => (_A * 256.0 + _B) / 1000.0)
			{
				Id = 803
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_FuelTankAirPresserPressure", "7E0", "Fuel tank air presser pressure", "Fuel tank air presser pressure", UnitsHelper.Units.MPa, Roles.None, "A800000172", "A800000173", (double _A, double _B) => (_A * 256.0 + _B) * 0.01)
			{
				Id = 803
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_SteeringRotation", "7E1", "Steering rotation angle", "Steering rotation angle", UnitsHelper.Units.grads, Roles.None, "A80000008D", "A80000015A", (double _A, double _B) => (double)((short)((int)(_A * 256.0 + _B))))
			{
				Id = 804
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_Odometer", "7E0", "Odometer", "Odometer", UnitsHelper.Units.km, Roles.None, "A80000010E", "A80000010F", (double _A, double _B) => (_A * 256.0 + _B) * 2.0)
			{
				Id = 805
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_PumpDifference", "7E0", "Individual pump difference learning value", "Individual pump difference learning value", UnitsHelper.Units.mA, Roles.None, "A800000238", "A800000239", (double _A, double _B) => _A * 256.0 + _B - 1000.0)
			{
				Id = 806
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_MileageAfterLearning", "7E0", "Mileage after injector learning", "Mileage after injector learning", UnitsHelper.Units.km, Roles.None, "A8000001FA", "A8000001FB", (double _A, double _B) => (_A * 256.0 + _B) * 5.0)
			{
				Id = 807
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_MileageAfterReplacement", "7E0", "Mileage after injector replacement", "Mileage after injector replacement", UnitsHelper.Units.km, Roles.None, "A800000204", "A800000205", (double _A, double _B) => (_A * 256.0 + _B) * 5.0)
			{
				Id = 808
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_DistanceSinceDPFRegen", "7E0", "Distance since last DPF regeneration", "Distance since last DPF regeneration", UnitsHelper.Units.km, Roles.None, "A80000029B", "A80000029C", (double _A, double _B) => _A * 256.0 + _B)
			{
				Id = 809
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_SecondaryAirFlow", "7E0", "Secondary air flow", "Secondary air flow", UnitsHelper.Units.grams_sec, Roles.None, "A800000182", "A800000183", (double _A, double _B) => (_A * 256.0 + _B) * 0.01)
			{
				Id = 810
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_TargetRPM", "7E0", "Target engine speed", "Target engine speed", UnitsHelper.Units.rpm, Roles.None, "A8000001EE", "A8000001EF", (double _A, double _B) => (_A * 256.0 + _B) * 0.25)
			{
				Id = 811
			});
			list.Add(new PID_SubaruSSM2DoublePID("SSM2_TargetFuelPumpCurrent", "7E0", "Target fuel pump current", "Target fuel pump current", UnitsHelper.Units.rpm, Roles.None, "A8000001F6", "A8000001F7", (double _A, double _B) => _A * 256.0 + _B)
			{
				Id = 812
			});
			return list;
		}

		// Token: 0x04001A6F RID: 6767
		protected string s_pid_A;

		// Token: 0x04001A70 RID: 6768
		protected string s_pid_B;

		// Token: 0x04001A71 RID: 6769
		private IPIDFloatValue pid_A;

		// Token: 0x04001A72 RID: 6770
		private IPIDFloatValue pid_B;

		// Token: 0x04001A73 RID: 6771
		private Func<double, double, double> decode_action;

		// Token: 0x02000468 RID: 1128
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002EB1 RID: 11953 RVA: 0x00206F29 File Offset: 0x00205129
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002EB2 RID: 11954 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002EB3 RID: 11955 RVA: 0x00206F35 File Offset: 0x00205135
			internal double <SSM2_RPM>b__11_0(double _A, double _B)
			{
				return Math.Round((_A * 256.0 + _B) / 4.0);
			}

			// Token: 0x06002EB4 RID: 11956 RVA: 0x00206F53 File Offset: 0x00205153
			internal double <SSM2_MAF>b__12_0(double _A, double _B)
			{
				return (_A * 256.0 + _B) / 100.0;
			}

			// Token: 0x06002EB5 RID: 11957 RVA: 0x00206F6C File Offset: 0x0020516C
			internal double <SSM2_O2S1>b__13_0(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.005;
			}

			// Token: 0x06002EB6 RID: 11958 RVA: 0x00206F6C File Offset: 0x0020516C
			internal double <SSM2_O2S2>b__14_0(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.005;
			}

			// Token: 0x06002EB7 RID: 11959 RVA: 0x00206F6C File Offset: 0x0020516C
			internal double <SSM2_O2S3>b__15_0(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.005;
			}

			// Token: 0x06002EB8 RID: 11960 RVA: 0x00206F85 File Offset: 0x00205185
			internal double <SSM2_ATF_DeteriorationDegree>b__16_0(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 40.0 / 13107.0;
			}

			// Token: 0x06002EB9 RID: 11961 RVA: 0x00206FA8 File Offset: 0x002051A8
			internal double <GET_SSM2_PIDS>b__17_0(double _A, double _B)
			{
				return _A * 256.0 + _B;
			}

			// Token: 0x06002EBA RID: 11962 RVA: 0x00206FA8 File Offset: 0x002051A8
			internal double <GET_SSM2_PIDS>b__17_1(double _A, double _B)
			{
				return _A * 256.0 + _B;
			}

			// Token: 0x06002EBB RID: 11963 RVA: 0x00206FB7 File Offset: 0x002051B7
			internal double <GET_SSM2_PIDS>b__17_2(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.0039;
			}

			// Token: 0x06002EBC RID: 11964 RVA: 0x00206FD0 File Offset: 0x002051D0
			internal double <GET_SSM2_PIDS>b__17_3(double _A, double _B)
			{
				return (_A * 256.0 + _B) / 1000.0;
			}

			// Token: 0x06002EBD RID: 11965 RVA: 0x00206FE9 File Offset: 0x002051E9
			internal double <GET_SSM2_PIDS>b__17_4(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.01;
			}

			// Token: 0x06002EBE RID: 11966 RVA: 0x00207002 File Offset: 0x00205202
			internal double <GET_SSM2_PIDS>b__17_5(double _A, double _B)
			{
				return (double)((short)((int)(_A * 256.0 + _B)));
			}

			// Token: 0x06002EBF RID: 11967 RVA: 0x00207014 File Offset: 0x00205214
			internal double <GET_SSM2_PIDS>b__17_6(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 2.0;
			}

			// Token: 0x06002EC0 RID: 11968 RVA: 0x0020702D File Offset: 0x0020522D
			internal double <GET_SSM2_PIDS>b__17_7(double _A, double _B)
			{
				return _A * 256.0 + _B - 1000.0;
			}

			// Token: 0x06002EC1 RID: 11969 RVA: 0x00207046 File Offset: 0x00205246
			internal double <GET_SSM2_PIDS>b__17_8(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 5.0;
			}

			// Token: 0x06002EC2 RID: 11970 RVA: 0x00207046 File Offset: 0x00205246
			internal double <GET_SSM2_PIDS>b__17_9(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 5.0;
			}

			// Token: 0x06002EC3 RID: 11971 RVA: 0x00206FA8 File Offset: 0x002051A8
			internal double <GET_SSM2_PIDS>b__17_10(double _A, double _B)
			{
				return _A * 256.0 + _B;
			}

			// Token: 0x06002EC4 RID: 11972 RVA: 0x00206FE9 File Offset: 0x002051E9
			internal double <GET_SSM2_PIDS>b__17_11(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.01;
			}

			// Token: 0x06002EC5 RID: 11973 RVA: 0x0020705F File Offset: 0x0020525F
			internal double <GET_SSM2_PIDS>b__17_12(double _A, double _B)
			{
				return (_A * 256.0 + _B) * 0.25;
			}

			// Token: 0x06002EC6 RID: 11974 RVA: 0x00206FA8 File Offset: 0x002051A8
			internal double <GET_SSM2_PIDS>b__17_13(double _A, double _B)
			{
				return _A * 256.0 + _B;
			}

			// Token: 0x04001A74 RID: 6772
			public static readonly PID_SubaruSSM2DoublePID.<>c <>9 = new PID_SubaruSSM2DoublePID.<>c();

			// Token: 0x04001A75 RID: 6773
			public static Func<double, double, double> <>9__11_0;

			// Token: 0x04001A76 RID: 6774
			public static Func<double, double, double> <>9__12_0;

			// Token: 0x04001A77 RID: 6775
			public static Func<double, double, double> <>9__13_0;

			// Token: 0x04001A78 RID: 6776
			public static Func<double, double, double> <>9__14_0;

			// Token: 0x04001A79 RID: 6777
			public static Func<double, double, double> <>9__15_0;

			// Token: 0x04001A7A RID: 6778
			public static Func<double, double, double> <>9__16_0;

			// Token: 0x04001A7B RID: 6779
			public static Func<double, double, double> <>9__17_0;

			// Token: 0x04001A7C RID: 6780
			public static Func<double, double, double> <>9__17_1;

			// Token: 0x04001A7D RID: 6781
			public static Func<double, double, double> <>9__17_2;

			// Token: 0x04001A7E RID: 6782
			public static Func<double, double, double> <>9__17_3;

			// Token: 0x04001A7F RID: 6783
			public static Func<double, double, double> <>9__17_4;

			// Token: 0x04001A80 RID: 6784
			public static Func<double, double, double> <>9__17_5;

			// Token: 0x04001A81 RID: 6785
			public static Func<double, double, double> <>9__17_6;

			// Token: 0x04001A82 RID: 6786
			public static Func<double, double, double> <>9__17_7;

			// Token: 0x04001A83 RID: 6787
			public static Func<double, double, double> <>9__17_8;

			// Token: 0x04001A84 RID: 6788
			public static Func<double, double, double> <>9__17_9;

			// Token: 0x04001A85 RID: 6789
			public static Func<double, double, double> <>9__17_10;

			// Token: 0x04001A86 RID: 6790
			public static Func<double, double, double> <>9__17_11;

			// Token: 0x04001A87 RID: 6791
			public static Func<double, double, double> <>9__17_12;

			// Token: 0x04001A88 RID: 6792
			public static Func<double, double, double> <>9__17_13;
		}
	}
}
