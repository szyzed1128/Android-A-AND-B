using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200086A RID: 2154
	public class CustomCodingsListViewModel : ObservableCollection<CustomizableCodingTemplate>
	{
		// Token: 0x0600496F RID: 18799 RVA: 0x00378D10 File Offset: 0x00376F10
		public void Load()
		{
			base.Clear();
			try
			{
				foreach (CustomizableCodingTemplate customizableCodingTemplate in JsonConvert.DeserializeObject<List<CustomizableCodingTemplate>>(SharedSettings.Current.CustomCodings))
				{
					base.Add(customizableCodingTemplate);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00378D84 File Offset: 0x00376F84
		public void Save()
		{
			string text = JsonConvert.SerializeObject(this);
			SharedSettings.Current.CustomCodings = text;
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x00378DA3 File Offset: 0x00376FA3
		public CustomCodingsListViewModel()
		{
		}
	}
}
