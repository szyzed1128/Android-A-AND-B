using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000014 RID: 20
	public class ColorValue : INotifyPropertyChanged
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000088 RID: 136 RVA: 0x00004E38 File Offset: 0x00003038
		// (remove) Token: 0x06000089 RID: 137 RVA: 0x00004E70 File Offset: 0x00003070
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

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004EA5 File Offset: 0x000030A5
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004EAD File Offset: 0x000030AD
		public byte A
		{
			get
			{
				return this.a;
			}
			set
			{
				if (value != this.a)
				{
					this.a = value;
					this.ValuesChanged(new string[] { "A", "Value", "Hexa" });
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004EE3 File Offset: 0x000030E3
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004EEB File Offset: 0x000030EB
		public byte R
		{
			get
			{
				return this.r;
			}
			set
			{
				if (value != this.r)
				{
					this.r = value;
					this.ValuesChanged(new string[] { "R", "Value", "Hexa" });
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004F21 File Offset: 0x00003121
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004F29 File Offset: 0x00003129
		public byte G
		{
			get
			{
				return this.g;
			}
			set
			{
				if (value != this.g)
				{
					this.g = value;
					this.ValuesChanged(new string[] { "G", "Value", "Hexa" });
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00004F5F File Offset: 0x0000315F
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00004F67 File Offset: 0x00003167
		public byte B
		{
			get
			{
				return this.b;
			}
			set
			{
				if (value != this.b)
				{
					this.b = value;
					this.ValuesChanged(new string[] { "B", "Value", "Hexa" });
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004F9D File Offset: 0x0000319D
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00004FBC File Offset: 0x000031BC
		public Color Value
		{
			get
			{
				return ColorPickerUtils.ColorFromARGB(this.a, this.r, this.g, this.b);
			}
			set
			{
				if (this.A == value.A.ToByte() && this.R == value.R.ToByte() && this.G == value.G.ToByte() && this.B == value.B.ToByte())
				{
					return;
				}
				this.valueIsChanging = true;
				this.A = value.A.ToByte();
				this.R = value.R.ToByte();
				this.G = value.G.ToByte();
				this.B = value.B.ToByte();
				this.valueIsChanging = false;
				this.ValuesChanged(new string[] { "Value", "Hexa" });
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000508C File Offset: 0x0000328C
		// (set) Token: 0x06000095 RID: 149 RVA: 0x000050CC File Offset: 0x000032CC
		public string Hexa
		{
			get
			{
				string text = this.Value.ToHex().TrimStart('#');
				if (!this.EditAlpha && text.Length > 6)
				{
					text = text.Substring(2);
				}
				return text;
			}
			set
			{
				string text = value.TrimStart('#');
				if ((this.EditAlpha && text.Length != 8) || (!this.EditAlpha && text.Length != 6))
				{
					return;
				}
				try
				{
					Color color = Color.FromHex(text);
					if (this.Value != color)
					{
						this.Value = color;
					}
				}
				catch (Exception ex)
				{
					Console.Write((ex != null) ? ex.Message : null);
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00005148 File Offset: 0x00003348
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00005150 File Offset: 0x00003350
		public bool EditAlpha
		{
			get
			{
				return this.editAlpha;
			}
			set
			{
				if (value != this.editAlpha)
				{
					this.editAlpha = value;
					this.ValuesChanged(new string[] { "EditAlpha", "Hexa" });
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005180 File Offset: 0x00003380
		private void ValuesChanged(params string[] propNames)
		{
			if (this.valueIsChanging)
			{
				propNames = propNames.Where((string p) => p != "Value" && p != "Hexa").ToArray<string>();
			}
			if (propNames != null && propNames.Length != 0)
			{
				foreach (string text in propNames)
				{
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs(text));
					}
				}
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000051F4 File Offset: 0x000033F4
		public ColorValue()
		{
		}

		// Token: 0x04000057 RID: 87
		private byte a = byte.MaxValue;

		// Token: 0x04000058 RID: 88
		private byte r = byte.MaxValue;

		// Token: 0x04000059 RID: 89
		private byte g = byte.MaxValue;

		// Token: 0x0400005A RID: 90
		private byte b = byte.MaxValue;

		// Token: 0x0400005B RID: 91
		private bool valueIsChanging;

		// Token: 0x0400005C RID: 92
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400005D RID: 93
		private bool editAlpha = true;

		// Token: 0x02000015 RID: 21
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600009A RID: 154 RVA: 0x0000522F File Offset: 0x0000342F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600009B RID: 155 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600009C RID: 156 RVA: 0x0000523B File Offset: 0x0000343B
			internal bool <ValuesChanged>b__30_0(string p)
			{
				return p != "Value" && p != "Hexa";
			}

			// Token: 0x0400005E RID: 94
			public static readonly ColorValue.<>c <>9 = new ColorValue.<>c();

			// Token: 0x0400005F RID: 95
			public static Func<string, bool> <>9__30_0;
		}
	}
}
