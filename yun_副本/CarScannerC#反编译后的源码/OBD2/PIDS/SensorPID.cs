using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000424 RID: 1060
	public class SensorPID : PID, IPIDFloatValue, IPID, INotifyPropertyChanged
	{
		// Token: 0x06002D34 RID: 11572 RVA: 0x001FEC76 File Offset: 0x001FCE76
		public void SendNaN()
		{
			base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
			this.Value = double.NaN;
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x001FEC9C File Offset: 0x001FCE9C
		public SensorPID(string Name, string Command, UnitsHelper.Units units)
			: base(Name, Command)
		{
			this.Units = units;
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06002D36 RID: 11574 RVA: 0x001FECAD File Offset: 0x001FCEAD
		// (set) Token: 0x06002D37 RID: 11575 RVA: 0x001FECB5 File Offset: 0x001FCEB5
		public UnitsHelper.Units Units
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

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x001FECBE File Offset: 0x001FCEBE
		// (set) Token: 0x06002D39 RID: 11577 RVA: 0x001FECC6 File Offset: 0x001FCEC6
		public virtual double Value
		{
			get
			{
				return this._Value;
			}
			protected set
			{
				this._Value = value;
				base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
				this.OnValueChanged();
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x001FECEA File Offset: 0x001FCEEA
		// (set) Token: 0x06002D3B RID: 11579 RVA: 0x001FECF2 File Offset: 0x001FCEF2
		public new Roles Role
		{
			[CompilerGenerated]
			get
			{
				return this.<Role>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Role>k__BackingField = value;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string TextValueVariants
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000027D4 File Offset: 0x000009D4
		public virtual void Stop()
		{
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000027D4 File Offset: 0x000009D4
		public virtual void Start()
		{
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x000027D4 File Offset: 0x000009D4
		public virtual void Initialize()
		{
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x001FECFB File Offset: 0x001FCEFB
		protected override void ValueChangedSubscriptionAdded()
		{
			this.Start();
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x001FED03 File Offset: 0x001FCF03
		protected override void ValueChangedSubscriptionRemoved()
		{
			this.Stop();
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x000027D4 File Offset: 0x000009D4
		public void SetValue(double value)
		{
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x001FED0B File Offset: 0x001FCF0B
		public string GetTextValueVariantOrNull(double value)
		{
			return null;
		}

		// Token: 0x04001936 RID: 6454
		[CompilerGenerated]
		private UnitsHelper.Units <Units>k__BackingField;

		// Token: 0x04001937 RID: 6455
		protected double _Value;

		// Token: 0x04001938 RID: 6456
		[CompilerGenerated]
		private Roles <Role>k__BackingField;
	}
}
