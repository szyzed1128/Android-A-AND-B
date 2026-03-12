using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E9 RID: 2025
	public class JsonColorConverter : JsonColorConverterBase
	{
		// Token: 0x06004714 RID: 18196 RVA: 0x0036CDF0 File Offset: 0x0036AFF0
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			uint num = 0U;
			if (uint.TryParse(reader.Value as string, out num))
			{
				return Color.FromUint(num);
			}
			return Color.Default;
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0036CE2C File Offset: 0x0036B02C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is Color)
			{
				string text = JsonColorConverter.ToUint((Color)value);
				writer.WriteValue(text);
			}
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0036CE54 File Offset: 0x0036B054
		public static string ToHex(Color color)
		{
			int num = (int)(color.R * 255.0);
			int num2 = (int)(color.G * 255.0);
			int num3 = (int)(color.B * 255.0);
			int num4 = (int)(color.A * 255.0);
			return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { num4, num, num2, num3 });
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0036CEE4 File Offset: 0x0036B0E4
		public static string ToUint(Color color)
		{
			return (((uint)(color.A * 255.0) << 24) | ((uint)(color.R * 255.0) << 16) | ((uint)(color.G * 255.0) << 8) | (uint)(color.B * 255.0)).ToString();
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0036CF4C File Offset: 0x0036B14C
		public JsonColorConverter()
		{
		}
	}
}
