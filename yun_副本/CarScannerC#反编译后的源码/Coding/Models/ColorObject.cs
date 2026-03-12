using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Coding.Models
{
	// Token: 0x020009B1 RID: 2481
	public class ColorObject : INotifyPropertyChanged
	{
		// Token: 0x060050CC RID: 20684 RVA: 0x00002050 File Offset: 0x00000250
		public ColorObject()
		{
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x003EC33C File Offset: 0x003EA53C
		public ColorObject(byte r, byte g, byte b)
		{
			string text = r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
			this.Color = ColorConverters.FromHex(text);
		}

		// Token: 0x17001796 RID: 6038
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x003EC38A File Offset: 0x003EA58A
		// (set) Token: 0x060050CF RID: 20687 RVA: 0x003EC392 File Offset: 0x003EA592
		public int Number
		{
			[CompilerGenerated]
			get
			{
				return this.<Number>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Number>k__BackingField = value;
			}
		}

		// Token: 0x17001797 RID: 6039
		// (get) Token: 0x060050D0 RID: 20688 RVA: 0x003EC39B File Offset: 0x003EA59B
		// (set) Token: 0x060050D1 RID: 20689 RVA: 0x003EC3A3 File Offset: 0x003EA5A3
		public Color Color
		{
			get
			{
				return this._Color;
			}
			set
			{
				this._Color = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Color"));
			}
		}

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060050D2 RID: 20690 RVA: 0x003EC3C8 File Offset: 0x003EA5C8
		// (remove) Token: 0x060050D3 RID: 20691 RVA: 0x003EC400 File Offset: 0x003EA600
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

		// Token: 0x040030C8 RID: 12488
		[CompilerGenerated]
		private int <Number>k__BackingField;

		// Token: 0x040030C9 RID: 12489
		private Color _Color;

		// Token: 0x040030CA RID: 12490
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
