using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AD5 RID: 2773
	internal class FPAModel
	{
		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x004187EA File Offset: 0x004169EA
		// (set) Token: 0x06005705 RID: 22277 RVA: 0x004187F2 File Offset: 0x004169F2
		public List<FPAProfile> Profiles
		{
			[CompilerGenerated]
			get
			{
				return this.<Profiles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Profiles>k__BackingField = value;
			}
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x004187FC File Offset: 0x004169FC
		public FPAModel(byte[] data, string version)
		{
			this.Version = version;
			this.data = data;
			this.Profiles = new List<FPAProfile>();
			for (int i = 0; i < 12; i++)
			{
				FPAProfile fpaprofile = new FPAProfile(i, data);
				this.Profiles.Add(fpaprofile);
			}
			this.Controls = FPAControl.GetControlsForSavingAfterRestart(data);
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x06005707 RID: 22279 RVA: 0x00418855 File Offset: 0x00416A55
		// (set) Token: 0x06005708 RID: 22280 RVA: 0x0041885D File Offset: 0x00416A5D
		public List<FPAControl> Controls
		{
			get
			{
				return this._Controls;
			}
			set
			{
				this._Controls = value;
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x06005709 RID: 22281 RVA: 0x00418866 File Offset: 0x00416A66
		// (set) Token: 0x0600570A RID: 22282 RVA: 0x0041886E File Offset: 0x00416A6E
		public string Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x00418878 File Offset: 0x00416A78
		public byte[] GetBytes()
		{
			byte[] array = this.data.ToArray<byte>();
			foreach (FPAProfile fpaprofile in this.Profiles)
			{
				fpaprofile.ApplyToBytes(array);
			}
			for (int i = 0; i < this.Controls.Count; i++)
			{
				byte[] saveOnRestartBytes = this.Controls[i].GetSaveOnRestartBytes();
				array[722 + i * 2] = saveOnRestartBytes[0];
				array[722 + i * 2 + 1] = saveOnRestartBytes[1];
			}
			byte[] array2 = new byte[array.Length - 4];
			Array.Copy(array, array2, array2.Length);
			byte[] array3 = Crc32.Calculate(array2);
			Array.Reverse<byte>(array3);
			array = array2.Concat(array3).ToArray<byte>();
			return array;
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x0600570C RID: 22284 RVA: 0x00418958 File Offset: 0x00416B58
		public FPAProfile Profile1
		{
			get
			{
				return this.Profiles[0];
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x0600570D RID: 22285 RVA: 0x00418966 File Offset: 0x00416B66
		public FPAProfile Profile2
		{
			get
			{
				return this.Profiles[1];
			}
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x0600570E RID: 22286 RVA: 0x00418974 File Offset: 0x00416B74
		public FPAProfile Profile3
		{
			get
			{
				return this.Profiles[2];
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x0600570F RID: 22287 RVA: 0x00418982 File Offset: 0x00416B82
		public FPAProfile Profile4
		{
			get
			{
				return this.Profiles[3];
			}
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x06005710 RID: 22288 RVA: 0x00418990 File Offset: 0x00416B90
		public FPAProfile Profile5
		{
			get
			{
				return this.Profiles[4];
			}
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x06005711 RID: 22289 RVA: 0x0041899E File Offset: 0x00416B9E
		public FPAProfile Profile6
		{
			get
			{
				return this.Profiles[5];
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x06005712 RID: 22290 RVA: 0x004189AC File Offset: 0x00416BAC
		public FPAProfile Profile7
		{
			get
			{
				return this.Profiles[6];
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x06005713 RID: 22291 RVA: 0x004189BA File Offset: 0x00416BBA
		public FPAProfile Profile8
		{
			get
			{
				return this.Profiles[7];
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x06005714 RID: 22292 RVA: 0x004189C8 File Offset: 0x00416BC8
		public FPAProfile Profile9
		{
			get
			{
				return this.Profiles[8];
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x06005715 RID: 22293 RVA: 0x004189D6 File Offset: 0x00416BD6
		public FPAProfile Profile10
		{
			get
			{
				return this.Profiles[9];
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x06005716 RID: 22294 RVA: 0x004189E5 File Offset: 0x00416BE5
		public FPAProfile Profile11
		{
			get
			{
				return this.Profiles[10];
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x06005717 RID: 22295 RVA: 0x004189F4 File Offset: 0x00416BF4
		public FPAProfile Profile12
		{
			get
			{
				return this.Profiles[11];
			}
		}

		// Token: 0x040035A6 RID: 13734
		[CompilerGenerated]
		private List<FPAProfile> <Profiles>k__BackingField;

		// Token: 0x040035A7 RID: 13735
		private byte[] data;

		// Token: 0x040035A8 RID: 13736
		private List<FPAControl> _Controls;

		// Token: 0x040035A9 RID: 13737
		[CompilerGenerated]
		private string <Version>k__BackingField;
	}
}
