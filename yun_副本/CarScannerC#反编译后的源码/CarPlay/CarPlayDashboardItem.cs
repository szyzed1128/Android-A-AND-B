using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BE0 RID: 3040
	public class CarPlayDashboardItem : IDisposable
	{
		// Token: 0x06005B59 RID: 23385 RVA: 0x00438BC8 File Offset: 0x00436DC8
		public CarPlayDashboardItem(ProxyItem proxy)
		{
			this.PID_ID = proxy.PID_Id;
			this.Model = new LiveDataPIDModel();
			this.Model.Mode = LiveDataModes.Dashboard;
			this.Model.DoubleFormat = proxy.ValueFormat;
			this._overrideName = proxy.OverrideName;
			this._customName = proxy.CustomName;
			this.Initialize();
		}

		// Token: 0x06005B5A RID: 23386 RVA: 0x00438C38 File Offset: 0x00436E38
		public CarPlayDashboardItem(PID pid)
		{
			if (pid == null)
			{
				pid = PID.Empty;
			}
			this.PID_ID = pid.Id;
			this.Model = new LiveDataPIDModel();
			this.Model.Mode = LiveDataModes.Dashboard;
			this._overrideName = false;
			this._customName = "";
			this.Initialize();
		}

		// Token: 0x06005B5B RID: 23387 RVA: 0x00438C9C File Offset: 0x00436E9C
		public void Initialize()
		{
			PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x != null && x.Id == this.PID_ID);
			if (pid == null)
			{
				this.Model.SelectedPID = PID.Empty;
				this.Title = Translate.GetString("pid_Empty");
				return;
			}
			this.Model.SelectedPID = pid;
			if (this._overrideName)
			{
				this.Title = this._customName;
				return;
			}
			this.Title = pid.ShortName;
		}

		// Token: 0x06005B5C RID: 23388 RVA: 0x00438D11 File Offset: 0x00436F11
		public void GetRequests(List<OBDRequest> requests)
		{
			this.Model.GetRequests(requests, "CarPlay", "");
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x00438D29 File Offset: 0x00436F29
		// (set) Token: 0x06005B5E RID: 23390 RVA: 0x00438D31 File Offset: 0x00436F31
		public LiveDataPIDModel Model
		{
			[CompilerGenerated]
			get
			{
				return this.<Model>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Model>k__BackingField = value;
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x00438D3A File Offset: 0x00436F3A
		// (set) Token: 0x06005B60 RID: 23392 RVA: 0x00438D42 File Offset: 0x00436F42
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x00438D4B File Offset: 0x00436F4B
		public string Value
		{
			get
			{
				if (string.IsNullOrEmpty(this.Model.Units))
				{
					return this.Model.TextValue;
				}
				return this.Model.TextValue + " " + this.Model.Units;
			}
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x00438D8B File Offset: 0x00436F8B
		public void Dispose()
		{
			LiveDataPIDModel model = this.Model;
			if (model == null)
			{
				return;
			}
			model.Unsubscribe();
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x00438D9D File Offset: 0x00436F9D
		[CompilerGenerated]
		private bool <Initialize>b__2_0(PID x)
		{
			return x != null && x.Id == this.PID_ID;
		}

		// Token: 0x040039AC RID: 14764
		private int PID_ID;

		// Token: 0x040039AD RID: 14765
		private bool _overrideName;

		// Token: 0x040039AE RID: 14766
		private string _customName = "";

		// Token: 0x040039AF RID: 14767
		[CompilerGenerated]
		private LiveDataPIDModel <Model>k__BackingField;

		// Token: 0x040039B0 RID: 14768
		[CompilerGenerated]
		private string <Title>k__BackingField;
	}
}
