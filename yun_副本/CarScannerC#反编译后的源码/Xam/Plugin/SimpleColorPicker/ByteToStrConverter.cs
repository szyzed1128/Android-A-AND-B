using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000004 RID: 4
	public class ByteToStrConverter : IValueConverter
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002311 File Offset: 0x00000511
		// (set) Token: 0x0600000B RID: 11 RVA: 0x00002319 File Offset: 0x00000519
		public byte DefaultValue
		{
			[CompilerGenerated]
			get
			{
				return this.<DefaultValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DefaultValue>k__BackingField = value;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002322 File Offset: 0x00000522
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return global::System.Convert.ToString(value ?? this.DefaultValue);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000233C File Offset: 0x0000053C
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				obj = global::System.Convert.ToByte(value);
			}
			catch (Exception ex)
			{
				Console.Write((ex != null) ? ex.Message : null);
				obj = this.DefaultValue;
			}
			return obj;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002050 File Offset: 0x00000250
		public ByteToStrConverter()
		{
		}

		// Token: 0x04000005 RID: 5
		[CompilerGenerated]
		private byte <DefaultValue>k__BackingField;
	}
}
