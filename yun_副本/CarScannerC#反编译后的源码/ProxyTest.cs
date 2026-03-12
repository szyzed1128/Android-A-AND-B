using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.SpeedTest;

namespace CarScannerXamarinForms
{
	// Token: 0x020001BB RID: 443
	public class ProxyTest
	{
		// Token: 0x17000F89 RID: 3977
		// (get) Token: 0x06001739 RID: 5945 RVA: 0x000AC6E5 File Offset: 0x000AA8E5
		// (set) Token: 0x0600173A RID: 5946 RVA: 0x000AC6ED File Offset: 0x000AA8ED
		public ProxyTest.TestTypes TestType
		{
			[CompilerGenerated]
			get
			{
				return this.<TestType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TestType>k__BackingField = value;
			}
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x0600173B RID: 5947 RVA: 0x000AC6F6 File Offset: 0x000AA8F6
		// (set) Token: 0x0600173C RID: 5948 RVA: 0x000AC6FE File Offset: 0x000AA8FE
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x000AC707 File Offset: 0x000AA907
		// (set) Token: 0x0600173E RID: 5950 RVA: 0x000AC70F File Offset: 0x000AA90F
		public double StartSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<StartSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StartSpeed>k__BackingField = value;
			}
		}

		// Token: 0x17000F8C RID: 3980
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x000AC718 File Offset: 0x000AA918
		// (set) Token: 0x06001740 RID: 5952 RVA: 0x000AC720 File Offset: 0x000AA920
		public double EndSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<EndSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EndSpeed>k__BackingField = value;
			}
		}

		// Token: 0x17000F8D RID: 3981
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x000AC729 File Offset: 0x000AA929
		// (set) Token: 0x06001742 RID: 5954 RVA: 0x000AC731 File Offset: 0x000AA931
		public string Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		} = "";

		// Token: 0x17000F8E RID: 3982
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x000AC73A File Offset: 0x000AA93A
		// (set) Token: 0x06001744 RID: 5956 RVA: 0x000AC742 File Offset: 0x000AA942
		public DateTime Timestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<Timestamp>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Timestamp>k__BackingField = value;
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x000AC74C File Offset: 0x000AA94C
		public static ProxyTest GetProxyTest(ISpeedTestBase speedTest)
		{
			if (speedTest is BrakeDistanceTest)
			{
				BrakeDistanceTest brakeDistanceTest = speedTest as BrakeDistanceTest;
				return new ProxyTest
				{
					Name = brakeDistanceTest.Name,
					StartSpeed = brakeDistanceTest.StartSpeed,
					EndSpeed = brakeDistanceTest.EndSpeed,
					TestType = ProxyTest.TestTypes.BrakeDistanceTest,
					Value = brakeDistanceTest.Value
				};
			}
			if (speedTest is BrakeTimeTest)
			{
				BrakeTimeTest brakeTimeTest = speedTest as BrakeTimeTest;
				return new ProxyTest
				{
					Name = brakeTimeTest.Name,
					StartSpeed = brakeTimeTest.StartSpeed,
					EndSpeed = brakeTimeTest.EndSpeed,
					TestType = ProxyTest.TestTypes.BrakeTimeTest,
					Value = brakeTimeTest.Value
				};
			}
			if (speedTest is Acceleration402mTest)
			{
				Acceleration402mTest acceleration402mTest = speedTest as Acceleration402mTest;
				return new ProxyTest
				{
					Name = acceleration402mTest.Name,
					StartSpeed = acceleration402mTest.StartSpeed,
					EndSpeed = acceleration402mTest.EndSpeed,
					TestType = ProxyTest.TestTypes.Acceleration402mTest,
					Value = acceleration402mTest.Value
				};
			}
			if (speedTest is SpeedTestV1)
			{
				SpeedTestV1 speedTestV = speedTest as SpeedTestV1;
				return new ProxyTest
				{
					Name = speedTestV.Name,
					StartSpeed = speedTestV.StartSpeed,
					EndSpeed = speedTestV.EndSpeed,
					TestType = ProxyTest.TestTypes.AcclerationTest,
					Value = speedTestV.Value
				};
			}
			return null;
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x000AC88C File Offset: 0x000AAA8C
		public static ISpeedTest GetSpeedTest(ProxyTest proxyTest)
		{
			if (proxyTest.TestType == ProxyTest.TestTypes.AcclerationTest)
			{
				return new SpeedTestV1(App.OBDReader, proxyTest.Name, proxyTest.StartSpeed, proxyTest.EndSpeed);
			}
			if (proxyTest.TestType == ProxyTest.TestTypes.BrakeDistanceTest)
			{
				return new BrakeDistanceTest(App.OBDReader, proxyTest.Name, proxyTest.StartSpeed, proxyTest.EndSpeed);
			}
			if (proxyTest.TestType == ProxyTest.TestTypes.BrakeTimeTest)
			{
				return new BrakeTimeTest(App.OBDReader, proxyTest.Name, proxyTest.StartSpeed, proxyTest.EndSpeed);
			}
			if (proxyTest.TestType == ProxyTest.TestTypes.Acceleration402mTest)
			{
				return new Acceleration402mTest(App.OBDReader, proxyTest.Name);
			}
			return null;
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x000AC925 File Offset: 0x000AAB25
		public ProxyTest()
		{
		}

		// Token: 0x04000A19 RID: 2585
		[CompilerGenerated]
		private ProxyTest.TestTypes <TestType>k__BackingField;

		// Token: 0x04000A1A RID: 2586
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04000A1B RID: 2587
		[CompilerGenerated]
		private double <StartSpeed>k__BackingField;

		// Token: 0x04000A1C RID: 2588
		[CompilerGenerated]
		private double <EndSpeed>k__BackingField;

		// Token: 0x04000A1D RID: 2589
		[CompilerGenerated]
		private string <Value>k__BackingField;

		// Token: 0x04000A1E RID: 2590
		[CompilerGenerated]
		private DateTime <Timestamp>k__BackingField;

		// Token: 0x020001BC RID: 444
		public enum TestTypes
		{
			// Token: 0x04000A20 RID: 2592
			AcclerationTest,
			// Token: 0x04000A21 RID: 2593
			BrakeTimeTest,
			// Token: 0x04000A22 RID: 2594
			BrakeDistanceTest,
			// Token: 0x04000A23 RID: 2595
			Acceleration402mTest
		}
	}
}
