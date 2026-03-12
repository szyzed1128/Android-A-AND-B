using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms
{
	// Token: 0x0200017A RID: 378
	public class LiveDataPageSettings
	{
		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x0600157F RID: 5503 RVA: 0x0009580E File Offset: 0x00093A0E
		// (set) Token: 0x06001580 RID: 5504 RVA: 0x00095816 File Offset: 0x00093A16
		public int ChartsVisible
		{
			[CompilerGenerated]
			get
			{
				return this.<ChartsVisible>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChartsVisible>k__BackingField = value;
			}
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06001581 RID: 5505 RVA: 0x0009581F File Offset: 0x00093A1F
		// (set) Token: 0x06001582 RID: 5506 RVA: 0x00095827 File Offset: 0x00093A27
		public int PIDId0
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDId0>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PIDId0>k__BackingField = value;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06001583 RID: 5507 RVA: 0x00095830 File Offset: 0x00093A30
		// (set) Token: 0x06001584 RID: 5508 RVA: 0x00095838 File Offset: 0x00093A38
		public int PIDId1
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDId1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PIDId1>k__BackingField = value;
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x06001585 RID: 5509 RVA: 0x00095841 File Offset: 0x00093A41
		// (set) Token: 0x06001586 RID: 5510 RVA: 0x00095849 File Offset: 0x00093A49
		public int PIDId2
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDId2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PIDId2>k__BackingField = value;
			}
		}

		// Token: 0x17000F44 RID: 3908
		// (get) Token: 0x06001587 RID: 5511 RVA: 0x00095852 File Offset: 0x00093A52
		// (set) Token: 0x06001588 RID: 5512 RVA: 0x0009585A File Offset: 0x00093A5A
		public int PIDId3
		{
			[CompilerGenerated]
			get
			{
				return this.<PIDId3>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PIDId3>k__BackingField = value;
			}
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x00095864 File Offset: 0x00093A64
		public static LiveDataPageSettings Load()
		{
			return new LiveDataPageSettings
			{
				ChartsVisible = SharedSettings.Current.ChartsVisible,
				PIDId0 = SharedSettings.Current.LiveDataPIDId0,
				PIDId1 = SharedSettings.Current.LiveDataPIDId1,
				PIDId2 = SharedSettings.Current.LiveDataPIDId2,
				PIDId3 = SharedSettings.Current.LiveDataPIDId3
			};
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x000958C8 File Offset: 0x00093AC8
		public void Save(LiveDataPageSettings ldps)
		{
			SharedSettings.Current.ChartsVisible = ldps.ChartsVisible;
			SharedSettings.Current.LiveDataPIDId0 = ldps.PIDId0;
			SharedSettings.Current.LiveDataPIDId1 = ldps.PIDId1;
			SharedSettings.Current.LiveDataPIDId2 = ldps.PIDId2;
			SharedSettings.Current.LiveDataPIDId3 = ldps.PIDId3;
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x00095925 File Offset: 0x00093B25
		public void Save()
		{
			this.Save(this);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x00002050 File Offset: 0x00000250
		public LiveDataPageSettings()
		{
		}

		// Token: 0x04000601 RID: 1537
		[CompilerGenerated]
		private int <ChartsVisible>k__BackingField;

		// Token: 0x04000602 RID: 1538
		[CompilerGenerated]
		private int <PIDId0>k__BackingField;

		// Token: 0x04000603 RID: 1539
		[CompilerGenerated]
		private int <PIDId1>k__BackingField;

		// Token: 0x04000604 RID: 1540
		[CompilerGenerated]
		private int <PIDId2>k__BackingField;

		// Token: 0x04000605 RID: 1541
		[CompilerGenerated]
		private int <PIDId3>k__BackingField;
	}
}
