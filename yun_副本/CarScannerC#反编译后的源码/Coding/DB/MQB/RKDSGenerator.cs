using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B42 RID: 2882
	public class RKDSGenerator : INotifyPropertyChanged
	{
		// Token: 0x0600593F RID: 22847 RVA: 0x00429A90 File Offset: 0x00427C90
		public RKDSGenerator()
		{
			this.IsIndividualVisible = false;
			RKDSTireset rkdstireset = new RKDSTireset
			{
				Name = "Standard",
				FrontFull = 2.6,
				RearFull = 2.8,
				FrontPartial = 2.3,
				RearPartial = 2.3,
				FrontComfort = 25.5,
				RearComfort = 25.5
			};
			RKDSTireset rkdstireset2 = new RKDSTireset
			{
				Name = "Comfort",
				FrontFull = 2.1,
				RearFull = 2.1,
				FrontPartial = 2.1,
				RearPartial = 2.1,
				FrontComfort = 25.5,
				RearComfort = 25.5
			};
			this.Tiresets.Add(rkdstireset);
			this.Tiresets.Add(rkdstireset2);
			this._TiresetsCount = 2;
			this.IndividualTireset = new RKDSTireset
			{
				Name = "Individual",
				FrontFull = 25.5,
				RearFull = 25.5,
				FrontPartial = 25.5,
				RearPartial = 25.5,
				FrontComfort = 25.5,
				RearComfort = 25.5
			};
		}

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06005940 RID: 22848 RVA: 0x00429C24 File Offset: 0x00427E24
		// (remove) Token: 0x06005941 RID: 22849 RVA: 0x00429C5C File Offset: 0x00427E5C
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

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06005942 RID: 22850 RVA: 0x00429C91 File Offset: 0x00427E91
		// (set) Token: 0x06005943 RID: 22851 RVA: 0x00429C99 File Offset: 0x00427E99
		public ObservableCollection<RKDSTireset> Tiresets
		{
			[CompilerGenerated]
			get
			{
				return this.<Tiresets>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Tiresets>k__BackingField = value;
			}
		} = new ObservableCollection<RKDSTireset>();

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06005944 RID: 22852 RVA: 0x00429CA2 File Offset: 0x00427EA2
		// (set) Token: 0x06005945 RID: 22853 RVA: 0x00429CAA File Offset: 0x00427EAA
		public bool IsComfortVisible
		{
			get
			{
				return this._IsComfortVisible;
			}
			set
			{
				this._IsComfortVisible = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("IsComfortVisible"));
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x00429CCE File Offset: 0x00427ECE
		// (set) Token: 0x06005947 RID: 22855 RVA: 0x00429CD6 File Offset: 0x00427ED6
		public bool IsIndividualVisible
		{
			get
			{
				return this._IsIndividualVisible;
			}
			set
			{
				this._IsIndividualVisible = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("IsIndividualVisible"));
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x06005948 RID: 22856 RVA: 0x00429CFA File Offset: 0x00427EFA
		// (set) Token: 0x06005949 RID: 22857 RVA: 0x00429D02 File Offset: 0x00427F02
		public RKDSTireset IndividualTireset
		{
			[CompilerGenerated]
			get
			{
				return this.<IndividualTireset>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IndividualTireset>k__BackingField = value;
			}
		}

		// Token: 0x1700183D RID: 6205
		// (get) Token: 0x0600594A RID: 22858 RVA: 0x00429D0B File Offset: 0x00427F0B
		// (set) Token: 0x0600594B RID: 22859 RVA: 0x00429D14 File Offset: 0x00427F14
		public int TiresetsCount
		{
			get
			{
				return this._TiresetsCount;
			}
			set
			{
				if (this._TiresetsCount != value)
				{
					this._TiresetsCount = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs("TiresetsCount"));
					}
					while (this.Tiresets.Count > this.TiresetsCount)
					{
						this.Tiresets.RemoveAt(this.Tiresets.Count - 1);
					}
					while (this.Tiresets.Count < this.TiresetsCount)
					{
						RKDSTireset rkdstireset = new RKDSTireset
						{
							Name = "#" + this.Tiresets.Count.ToString()
						};
						this.Tiresets.Add(rkdstireset);
					}
				}
			}
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x00429DC8 File Offset: 0x00427FC8
		public byte[] Generate(RKDSEcu ecu)
		{
			List<RKDSTireset> list = this.Tiresets.ToList<RKDSTireset>();
			return this.Generate(ecu, list.ToArray());
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x00429DF0 File Offset: 0x00427FF0
		public byte[] Generate(RKDSEcu ecu, RKDSTireset[] tireSets)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.WriteByte(ecu.Type);
			memoryStream.WriteByte(0);
			RKDSTireset rkdstireset = new RKDSTireset
			{
				Name = "",
				FrontComfort = 25.5,
				FrontFull = 25.5,
				FrontPartial = 25.5,
				RearComfort = 25.5,
				RearFull = 25.5,
				RearPartial = 25.5
			};
			for (int i = 0; i < 10; i++)
			{
				if (i < tireSets.Length)
				{
					byte[] bytes = tireSets[i].GetBytes(ecu.Name);
					memoryStream.Write(bytes, 0, bytes.Length);
				}
				else
				{
					byte[] bytes2 = rkdstireset.GetBytes(ecu.Name);
					memoryStream.Write(bytes2, 0, bytes2.Length);
				}
			}
			if (this.IsIndividualVisible)
			{
				byte[] bytes3 = this.IndividualTireset.GetBytes(ecu.Name);
				memoryStream.Write(bytes3, 0, bytes3.Length);
			}
			else
			{
				rkdstireset.Name = "Individual";
				byte[] bytes4 = rkdstireset.GetBytes(ecu.Name);
				memoryStream.Write(bytes4, 0, bytes4.Length);
			}
			for (int j = 0; j < 555; j++)
			{
				memoryStream.WriteByte(byte.MaxValue);
			}
			memoryStream.WriteByte((byte)ecu.Version[0]);
			memoryStream.WriteByte((byte)ecu.Version[1]);
			byte[] array = memoryStream.ToArray();
			byte[] array2 = Crc32.Calculate(array);
			return array.Concat(array2).ToArray<byte>();
		}

		// Token: 0x040037C5 RID: 14277
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040037C6 RID: 14278
		[CompilerGenerated]
		private ObservableCollection<RKDSTireset> <Tiresets>k__BackingField;

		// Token: 0x040037C7 RID: 14279
		private bool _IsComfortVisible;

		// Token: 0x040037C8 RID: 14280
		private bool _IsIndividualVisible;

		// Token: 0x040037C9 RID: 14281
		[CompilerGenerated]
		private RKDSTireset <IndividualTireset>k__BackingField;

		// Token: 0x040037CA RID: 14282
		private int _TiresetsCount = 1;
	}
}
