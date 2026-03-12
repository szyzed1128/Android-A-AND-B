using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200072A RID: 1834
	public class TestGroup : ObservableCollection<ECUTest>
	{
		// Token: 0x06003E7A RID: 15994 RVA: 0x0032C94E File Offset: 0x0032AB4E
		public TestGroup(string Name)
		{
			this.Name = Name;
		}

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x06003E7B RID: 15995 RVA: 0x0032C95D File Offset: 0x0032AB5D
		// (set) Token: 0x06003E7C RID: 15996 RVA: 0x0032C965 File Offset: 0x0032AB65
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

		// Token: 0x04002655 RID: 9813
		[CompilerGenerated]
		private string <Name>k__BackingField;
	}
}
