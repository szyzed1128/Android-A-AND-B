using System;
using System.Collections.Generic;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x0200040D RID: 1037
	internal class PIDOverrideDictionary : Dictionary<int, PIDOverride>
	{
		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06002A8B RID: 10891 RVA: 0x001F2832 File Offset: 0x001F0A32
		public static PIDOverrideDictionary Instance
		{
			get
			{
				if (PIDOverrideDictionary._Instance == null)
				{
					PIDOverrideDictionary pidoverrideDictionary = new PIDOverrideDictionary();
					pidoverrideDictionary.Load();
					PIDOverrideDictionary._Instance = pidoverrideDictionary;
				}
				return PIDOverrideDictionary._Instance;
			}
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x001F2850 File Offset: 0x001F0A50
		private PIDOverrideDictionary()
		{
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x001F2858 File Offset: 0x001F0A58
		public void SetProperty(int Id, string propertyName, string value)
		{
			PIDOverride pidoverride = null;
			if (!base.TryGetValue(Id, out pidoverride))
			{
				pidoverride = new PIDOverride
				{
					ID = Id
				};
				base[Id] = pidoverride;
			}
			if (!(propertyName == "Name"))
			{
				if (propertyName == "ShortName")
				{
					pidoverride.ShortName = value;
				}
			}
			else
			{
				pidoverride.Name = value;
			}
			this.Save();
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x001F28BC File Offset: 0x001F0ABC
		public void SetProperty(int Id, string propertyName, int value)
		{
			PIDOverride pidoverride = null;
			if (!base.TryGetValue(Id, out pidoverride))
			{
				pidoverride = new PIDOverride
				{
					ID = Id
				};
				base[Id] = pidoverride;
			}
			if (propertyName == "SkipCycles")
			{
				pidoverride.SkipCycles = value;
			}
			this.Save();
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x001F2908 File Offset: 0x001F0B08
		public void SetProperty(int Id, string propertyName, Roles value)
		{
			PIDOverride pidoverride = null;
			if (!base.TryGetValue(Id, out pidoverride))
			{
				pidoverride = new PIDOverride
				{
					ID = Id
				};
				base[Id] = pidoverride;
			}
			if (propertyName == "Role")
			{
				pidoverride.Role = value;
			}
			this.Save();
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x001F2954 File Offset: 0x001F0B54
		public void SetProperty(int Id, string propertyName, UnitsHelper.Units value)
		{
			PIDOverride pidoverride = null;
			if (!base.TryGetValue(Id, out pidoverride))
			{
				pidoverride = new PIDOverride
				{
					ID = Id
				};
				base[Id] = pidoverride;
			}
			if (propertyName == "Unit")
			{
				pidoverride.Unit = value;
			}
			this.Save();
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x001F29A0 File Offset: 0x001F0BA0
		public void Load()
		{
			base.Clear();
			if (!string.IsNullOrEmpty(SharedSettings.Current.PIDOverridesData))
			{
				try
				{
					Dictionary<int, PIDOverride> dictionary = JsonConvert.DeserializeObject<Dictionary<int, PIDOverride>>(SharedSettings.Current.PIDOverridesData);
					foreach (int num in dictionary.Keys)
					{
						base[num] = dictionary[num];
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002A92 RID: 10898 RVA: 0x001F2A34 File Offset: 0x001F0C34
		public void Save()
		{
			string text = JsonConvert.SerializeObject(this);
			SharedSettings.Current.PIDOverridesData = text;
		}

		// Token: 0x040017FD RID: 6141
		private static PIDOverrideDictionary _Instance;
	}
}
