using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms
{
	// Token: 0x020001BA RID: 442
	public class PIDAdapter
	{
		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06001732 RID: 5938 RVA: 0x000AC5A2 File Offset: 0x000AA7A2
		// (set) Token: 0x06001733 RID: 5939 RVA: 0x000AC5AA File Offset: 0x000AA7AA
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

		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06001734 RID: 5940 RVA: 0x000AC5B3 File Offset: 0x000AA7B3
		// (set) Token: 0x06001735 RID: 5941 RVA: 0x000AC5BB File Offset: 0x000AA7BB
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
		}

		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06001736 RID: 5942 RVA: 0x000AC5C4 File Offset: 0x000AA7C4
		// (set) Token: 0x06001737 RID: 5943 RVA: 0x000AC5CC File Offset: 0x000AA7CC
		public string Units
		{
			[CompilerGenerated]
			get
			{
				return this.<Units>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Units>k__BackingField = value;
			}
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000AC5D8 File Offset: 0x000AA7D8
		public PIDAdapter(PID pid)
		{
			this.Value = "";
			this.Units = "";
			this.Name = pid.Name;
			if (pid is IPIDFloatValue)
			{
				IPIDFloatValue ipidfloatValue = pid as IPIDFloatValue;
				this.Value = UnitsHelper.GetValue(ipidfloatValue.Value, ipidfloatValue.Units).ToString(CultureInfo.InvariantCulture);
				this.Units = UnitsHelper.GetCaption(ipidfloatValue.Units).ToString();
				return;
			}
			if (pid is PIDWithStringValue)
			{
				this.Value = (pid as PIDWithStringValue).Value;
				return;
			}
			if (pid is PID_Status)
			{
				PID0101DataItem value = (pid as PID_Status).Value;
				if (value != null)
				{
					this.Value = value.ToString();
					return;
				}
			}
			else if (pid is PID0103_FuelSystemStatus)
			{
				PID0103_FuelSystemStatus pid0103_FuelSystemStatus = pid as PID0103_FuelSystemStatus;
				this.Value = PID0103_FuelSystemStatus.GetFullCaption(pid0103_FuelSystemStatus.Value[0]);
				if (pid0103_FuelSystemStatus.Value[1] != PID0103_FuelSystemStatus.FuelSystemStatuses.None)
				{
					this.Value = this.Value + "\r\n" + PID0103_FuelSystemStatus.GetFullCaption(pid0103_FuelSystemStatus.Value[1]);
				}
			}
		}

		// Token: 0x04000A16 RID: 2582
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04000A17 RID: 2583
		[CompilerGenerated]
		private string <Value>k__BackingField;

		// Token: 0x04000A18 RID: 2584
		[CompilerGenerated]
		private string <Units>k__BackingField;
	}
}
