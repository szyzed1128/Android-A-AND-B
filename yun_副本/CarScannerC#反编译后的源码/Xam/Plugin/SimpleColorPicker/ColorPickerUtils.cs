using System;
using System.Reflection;
using Xamarin.Forms;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000013 RID: 19
	public static class ColorPickerUtils
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00004D00 File Offset: 0x00002F00
		public static string ToHex(this Color color)
		{
			int num = (int)(color.R * 255.0);
			int num2 = (int)(color.G * 255.0);
			int num3 = (int)(color.B * 255.0);
			int num4 = (int)(color.A * 255.0);
			return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", new object[] { num4, num, num2, num3 });
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004D8D File Offset: 0x00002F8D
		public static double ToDouble(this byte b)
		{
			return (double)b / 255.0;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004D9B File Offset: 0x00002F9B
		public static byte ToByte(this double d)
		{
			return (byte)(d * 255.0);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004DA9 File Offset: 0x00002FA9
		public static Color ColorFromARGB(byte a, byte r, byte g, byte b)
		{
			return Color.FromRgba(r.ToDouble(), g.ToDouble(), b.ToDouble(), a.ToDouble());
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004DC8 File Offset: 0x00002FC8
		public static T GetRootParent<T>(Element view) where T : Layout
		{
			Element element = view;
			T t = element as T;
			for (;;)
			{
				Element parent = element.Parent;
				if (parent == null || !parent.GetType().GetTypeInfo().IsSubclassOf(typeof(Element)))
				{
					break;
				}
				element = element.Parent;
				if (element.GetType().GetTypeInfo().IsSubclassOf(typeof(T)))
				{
					t = (T)((object)element);
				}
			}
			return t;
		}
	}
}
