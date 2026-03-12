using System;
using System.IO;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007E1 RID: 2017
	public static class Json
	{
		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x060046EE RID: 18158 RVA: 0x0036CA96 File Offset: 0x0036AC96
		private static IJson jsonProxy
		{
			get
			{
				if (Json._jsonProxy == null)
				{
					Json._jsonProxy = DependencyService.Get<IJson>(0);
				}
				return Json._jsonProxy;
			}
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x0036CAAF File Offset: 0x0036ACAF
		public static string SerializeObject(object obj)
		{
			return Json.jsonProxy.SerializeObject(obj);
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x0036CABC File Offset: 0x0036ACBC
		public static T DeserializeObject<T>(string value)
		{
			return Json.jsonProxy.DeserializeObject<T>(value);
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0036CAC9 File Offset: 0x0036ACC9
		public static void SerializeToStream(Stream stream, object obj)
		{
			Json.jsonProxy.SerializeToStream(stream, obj);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x0036CAD7 File Offset: 0x0036ACD7
		public static T DeserializeFromStream<T>(Stream stream)
		{
			return Json.jsonProxy.DeserializeFromStream<T>(stream);
		}

		// Token: 0x04002964 RID: 10596
		private static IJson _jsonProxy;
	}
}
