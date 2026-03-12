using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009E6 RID: 2534
	internal class ToyotaTPMSSensorItem : INotifyPropertyChanged
	{
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06005190 RID: 20880 RVA: 0x003F271C File Offset: 0x003F091C
		// (remove) Token: 0x06005191 RID: 20881 RVA: 0x003F2754 File Offset: 0x003F0954
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x170017AE RID: 6062
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x003F2789 File Offset: 0x003F0989
		// (set) Token: 0x06005193 RID: 20883 RVA: 0x003F2791 File Offset: 0x003F0991
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x003F27B8 File Offset: 0x003F09B8
		public ToyotaTPMSSensorItem(string id, byte[] data)
		{
			if (data == null)
			{
				this.Value = "Not ready";
				return;
			}
			string text = BitHelpers.ByteArrayToHexString(data);
			this.Value = text.Substring(1);
		}

		// Token: 0x04003179 RID: 12665
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400317A RID: 12666
		private string _Value = "";
	}
}
