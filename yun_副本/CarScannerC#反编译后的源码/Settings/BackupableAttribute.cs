using System;
using System.Runtime.InteropServices;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x020001F1 RID: 497
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	[ComVisible(true)]
	public sealed class BackupableAttribute : Attribute
	{
		// Token: 0x06001A04 RID: 6660 RVA: 0x0011311B File Offset: 0x0011131B
		internal static Attribute GetCustomAttribute(Type type)
		{
			return new BackupableAttribute();
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x00113122 File Offset: 0x00111322
		internal static bool IsDefined(Type type)
		{
			return type.IsSerializable;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0011312A File Offset: 0x0011132A
		public BackupableAttribute()
		{
		}
	}
}
