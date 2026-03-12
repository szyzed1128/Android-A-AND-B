using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200024F RID: 591
	public class PIDItemDataTemplateSelector : DataTemplateSelector
	{
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x0013BBA2 File Offset: 0x00139DA2
		// (set) Token: 0x06001BB8 RID: 7096 RVA: 0x0013BBAA File Offset: 0x00139DAA
		public DataTemplate EditableCustomPID
		{
			[CompilerGenerated]
			get
			{
				return this.<EditableCustomPID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EditableCustomPID>k__BackingField = value;
			}
		}

		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x0013BBB3 File Offset: 0x00139DB3
		// (set) Token: 0x06001BBA RID: 7098 RVA: 0x0013BBBB File Offset: 0x00139DBB
		public DataTemplate NonEditablePID
		{
			[CompilerGenerated]
			get
			{
				return this.<NonEditablePID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NonEditablePID>k__BackingField = value;
			}
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x0013BBC4 File Offset: 0x00139DC4
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			IPID ipid = (IPID)item;
			if (ipid is CustomPID && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(ipid))
			{
				return this.EditableCustomPID;
			}
			return this.NonEditablePID;
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x00016A2D File Offset: 0x00014C2D
		public PIDItemDataTemplateSelector()
		{
		}

		// Token: 0x04000CE3 RID: 3299
		[CompilerGenerated]
		private DataTemplate <EditableCustomPID>k__BackingField;

		// Token: 0x04000CE4 RID: 3300
		[CompilerGenerated]
		private DataTemplate <NonEditablePID>k__BackingField;
	}
}
