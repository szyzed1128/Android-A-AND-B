using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BE3 RID: 3043
	public class CarPlayDashboardPage : List<CarPlayDashboardItem>
	{
		// Token: 0x06005B81 RID: 23425 RVA: 0x00439620 File Offset: 0x00437820
		public CarPlayDashboardPage()
		{
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x00439633 File Offset: 0x00437833
		public CarPlayDashboardPage(int capacity)
			: base(capacity)
		{
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06005B83 RID: 23427 RVA: 0x00439647 File Offset: 0x00437847
		// (set) Token: 0x06005B84 RID: 23428 RVA: 0x0043964F File Offset: 0x0043784F
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		} = "";

		// Token: 0x06005B85 RID: 23429 RVA: 0x00439658 File Offset: 0x00437858
		internal void GetRequests(List<OBDRequest> requests)
		{
			if (requests == null)
			{
				return;
			}
			foreach (CarPlayDashboardItem carPlayDashboardItem in this)
			{
				carPlayDashboardItem.GetRequests(requests);
			}
		}

		// Token: 0x06005B86 RID: 23430 RVA: 0x004396A8 File Offset: 0x004378A8
		public void Initialize()
		{
			foreach (CarPlayDashboardItem carPlayDashboardItem in this)
			{
				carPlayDashboardItem.Initialize();
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06005B87 RID: 23431 RVA: 0x004396F4 File Offset: 0x004378F4
		// (set) Token: 0x06005B88 RID: 23432 RVA: 0x004396FC File Offset: 0x004378FC
		public bool UpdateInBackground
		{
			[CompilerGenerated]
			get
			{
				return this.<UpdateInBackground>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UpdateInBackground>k__BackingField = value;
			}
		}

		// Token: 0x040039C1 RID: 14785
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x040039C2 RID: 14786
		[CompilerGenerated]
		private bool <UpdateInBackground>k__BackingField;
	}
}
