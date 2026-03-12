using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000441 RID: 1089
	internal class PID_CalculatedAccelerationFromSpeed : CalculatedPIDV2
	{
		// Token: 0x06002DF1 RID: 11761 RVA: 0x00201568 File Offset: 0x001FF768
		public PID_CalculatedAccelerationFromSpeed()
			: base(PID.GetResourceString("PID_ACCELERATION"), "ACCELERATION_FROM_SPEED", UnitsHelper.Units.m_sec2, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 150.0;
			this.Command = "ACCELERATION_FROM_SPEED";
			base.Id = 236;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x002015E8 File Offset: 0x001FF7E8
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			this.DataCollection.Add(new KeyValuePair<TimeSpan, double>(this.SpeedPID.TimeStamp, this.SpeedPID.Value));
			if (this.DataCollection.Count > SharedSettings.Current.AccelerationItems)
			{
				this.DataCollection.RemoveAt(0);
			}
			if (this.DataCollection.Count < SharedSettings.Current.AccelerationItems)
			{
				result = 0.0;
				return false;
			}
			bool flag = true;
			double num = this.DataCollection[0].Value;
			for (int i = 1; i < this.DataCollection.Count; i++)
			{
				double value = this.DataCollection[i].Value;
				if (value < num)
				{
					flag = false;
					break;
				}
				num = value;
			}
			bool flag2 = true;
			if (!flag)
			{
				for (int j = 1; j < this.DataCollection.Count; j++)
				{
					double value2 = this.DataCollection[j].Value;
					if (value2 > num)
					{
						flag2 = false;
						break;
					}
					num = value2;
				}
			}
			if (flag || flag2)
			{
				double value3 = this.DataCollection[0].Value;
				double num2 = (this.DataCollection[this.DataCollection.Count - 1].Value - value3) / 3.6;
				double totalSeconds = (this.DataCollection[this.DataCollection.Count - 1].Key - this.DataCollection[0].Key).TotalSeconds;
				double num3 = num2 / totalSeconds;
				result = num3;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x002017A4 File Offset: 0x001FF9A4
		public override void Initialize()
		{
			this.SpeedPID = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed, App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 13) as IPIDFloatValue);
			base.DependencyPID = this.SpeedPID;
			base.Initialize();
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x00201811 File Offset: 0x001FFA11
		public override void ResetValues()
		{
			this.DataCollection.Clear();
			base.SetValue(0.0);
		}

		// Token: 0x040019DA RID: 6618
		private List<KeyValuePair<TimeSpan, double>> DataCollection = new List<KeyValuePair<TimeSpan, double>>(SharedSettings.Current.AccelerationItems);

		// Token: 0x040019DB RID: 6619
		private IPIDFloatValue SpeedPID;

		// Token: 0x02000442 RID: 1090
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002DF5 RID: 11765 RVA: 0x0020182D File Offset: 0x001FFA2D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002DF6 RID: 11766 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002DF7 RID: 11767 RVA: 0x0009AE64 File Offset: 0x00099064
			internal bool <Initialize>b__4_0(PID x)
			{
				return x.Id == 13;
			}

			// Token: 0x040019DC RID: 6620
			public static readonly PID_CalculatedAccelerationFromSpeed.<>c <>9 = new PID_CalculatedAccelerationFromSpeed.<>c();

			// Token: 0x040019DD RID: 6621
			public static Func<PID, bool> <>9__4_0;
		}
	}
}
