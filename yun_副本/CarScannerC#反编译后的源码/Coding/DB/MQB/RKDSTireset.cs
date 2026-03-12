using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B44 RID: 2884
	public class RKDSTireset : INotifyPropertyChanged
	{
		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x06005958 RID: 22872 RVA: 0x0042A0F4 File Offset: 0x004282F4
		// (set) Token: 0x06005959 RID: 22873 RVA: 0x0042A0FC File Offset: 0x004282FC
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x0600595A RID: 22874 RVA: 0x0042A11E File Offset: 0x0042831E
		// (set) Token: 0x0600595B RID: 22875 RVA: 0x0042A126 File Offset: 0x00428326
		public double FrontPartial
		{
			get
			{
				return this._FrontPartial;
			}
			set
			{
				if (this._FrontPartial != value)
				{
					this._FrontPartial = value;
					this.OnPropertyChanged("FrontPartial");
				}
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x0600595C RID: 22876 RVA: 0x0042A143 File Offset: 0x00428343
		// (set) Token: 0x0600595D RID: 22877 RVA: 0x0042A14B File Offset: 0x0042834B
		public double FrontFull
		{
			get
			{
				return this._FrontFull;
			}
			set
			{
				if (this._FrontFull != value)
				{
					this._FrontFull = value;
					this.OnPropertyChanged("FrontFull");
				}
			}
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x0600595E RID: 22878 RVA: 0x0042A168 File Offset: 0x00428368
		// (set) Token: 0x0600595F RID: 22879 RVA: 0x0042A170 File Offset: 0x00428370
		public double FrontComfort
		{
			get
			{
				return this._FrontComfort;
			}
			set
			{
				if (this._FrontComfort != value)
				{
					this._FrontComfort = value;
					this.OnPropertyChanged("FrontComfort");
				}
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x06005960 RID: 22880 RVA: 0x0042A18D File Offset: 0x0042838D
		// (set) Token: 0x06005961 RID: 22881 RVA: 0x0042A195 File Offset: 0x00428395
		public double RearPartial
		{
			get
			{
				return this._RearPartial;
			}
			set
			{
				if (this._RearPartial != value)
				{
					this._RearPartial = value;
					this.OnPropertyChanged("RearPartial");
				}
			}
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06005962 RID: 22882 RVA: 0x0042A1B2 File Offset: 0x004283B2
		// (set) Token: 0x06005963 RID: 22883 RVA: 0x0042A1BA File Offset: 0x004283BA
		public double RearFull
		{
			get
			{
				return this._RearFull;
			}
			set
			{
				if (this._RearFull != value)
				{
					this._RearFull = value;
					this.OnPropertyChanged("RearFull");
				}
			}
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06005964 RID: 22884 RVA: 0x0042A1D7 File Offset: 0x004283D7
		// (set) Token: 0x06005965 RID: 22885 RVA: 0x0042A1DF File Offset: 0x004283DF
		public double RearComfort
		{
			get
			{
				return this._RearComfort;
			}
			set
			{
				if (this._RearComfort != value)
				{
					this._RearComfort = value;
					this.OnPropertyChanged("RearComfort");
				}
			}
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x0042A1FC File Offset: 0x004283FC
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06005967 RID: 22887 RVA: 0x0042A218 File Offset: 0x00428418
		// (remove) Token: 0x06005968 RID: 22888 RVA: 0x0042A250 File Offset: 0x00428450
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

		// Token: 0x06005969 RID: 22889 RVA: 0x0042A285 File Offset: 0x00428485
		private byte PressureToByte(double pressure)
		{
			return (byte)((int)(pressure * 10.0));
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x0042A294 File Offset: 0x00428494
		public byte[] GetBytes(string ECUName)
		{
			MemoryStream memoryStream = new MemoryStream();
			for (int i = 0; i < 61; i++)
			{
				memoryStream.WriteByte(0);
			}
			if (this.Name.Length > 60)
			{
				this.Name = this.Name.Substring(0, 60);
			}
			byte[] bytes = Encoding.ASCII.GetBytes(this.Name);
			memoryStream.WriteByte((byte)bytes.Length);
			memoryStream.Write(bytes, 0, bytes.Length);
			int num = 61 - bytes.Length;
			for (int j = 0; j < num; j++)
			{
				memoryStream.WriteByte(0);
			}
			if (ECUName == "3AA907273H")
			{
				memoryStream.WriteByte(this.PressureToByte(this.FrontPartial));
				memoryStream.WriteByte(this.PressureToByte(this.FrontFull));
				memoryStream.WriteByte(this.PressureToByte(this.FrontComfort));
				memoryStream.WriteByte(this.PressureToByte(this.RearPartial));
				memoryStream.WriteByte(this.PressureToByte(this.RearFull));
				memoryStream.WriteByte(this.PressureToByte(this.RearComfort));
			}
			else
			{
				memoryStream.WriteByte(this.PressureToByte(this.FrontFull));
				memoryStream.WriteByte(this.PressureToByte(this.FrontPartial));
				memoryStream.WriteByte(this.PressureToByte(this.FrontComfort));
				memoryStream.WriteByte(this.PressureToByte(this.RearFull));
				memoryStream.WriteByte(this.PressureToByte(this.RearPartial));
				memoryStream.WriteByte(this.PressureToByte(this.RearComfort));
			}
			for (int k = 0; k < 6; k++)
			{
				if (k == 0 && ECUName == "5Q0907273B")
				{
					memoryStream.WriteByte(byte.MaxValue);
				}
				else
				{
					memoryStream.WriteByte(0);
				}
			}
			return memoryStream.ToArray();
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x0042A444 File Offset: 0x00428644
		public RKDSTireset()
		{
		}

		// Token: 0x040037CF RID: 14287
		private string _Name = "";

		// Token: 0x040037D0 RID: 14288
		private double _FrontPartial = 25.5;

		// Token: 0x040037D1 RID: 14289
		private double _FrontFull = 25.5;

		// Token: 0x040037D2 RID: 14290
		private double _FrontComfort = 25.5;

		// Token: 0x040037D3 RID: 14291
		private double _RearPartial = 25.5;

		// Token: 0x040037D4 RID: 14292
		private double _RearFull = 25.5;

		// Token: 0x040037D5 RID: 14293
		private double _RearComfort = 25.5;

		// Token: 0x040037D6 RID: 14294
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
