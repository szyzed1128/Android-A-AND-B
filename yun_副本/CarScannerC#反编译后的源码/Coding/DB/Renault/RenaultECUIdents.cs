using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A00 RID: 2560
	internal class RenaultECUIdents
	{
		// Token: 0x170017B8 RID: 6072
		// (get) Token: 0x060051E1 RID: 20961 RVA: 0x003F56E6 File Offset: 0x003F38E6
		// (set) Token: 0x060051E2 RID: 20962 RVA: 0x003F56EE File Offset: 0x003F38EE
		public string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x003F56F7 File Offset: 0x003F38F7
		// (set) Token: 0x060051E4 RID: 20964 RVA: 0x003F56FF File Offset: 0x003F38FF
		public int diagversion
		{
			[CompilerGenerated]
			get
			{
				return this.<diagversion>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<diagversion>k__BackingField = value;
			}
		} = -1;

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x060051E5 RID: 20965 RVA: 0x003F5708 File Offset: 0x003F3908
		// (set) Token: 0x060051E6 RID: 20966 RVA: 0x003F5710 File Offset: 0x003F3910
		public string supplier
		{
			[CompilerGenerated]
			get
			{
				return this.<supplier>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<supplier>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x060051E7 RID: 20967 RVA: 0x003F5719 File Offset: 0x003F3919
		// (set) Token: 0x060051E8 RID: 20968 RVA: 0x003F5721 File Offset: 0x003F3921
		public string version
		{
			[CompilerGenerated]
			get
			{
				return this.<version>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<version>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x060051E9 RID: 20969 RVA: 0x003F572A File Offset: 0x003F392A
		// (set) Token: 0x060051EA RID: 20970 RVA: 0x003F5732 File Offset: 0x003F3932
		public string soft
		{
			[CompilerGenerated]
			get
			{
				return this.<soft>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<soft>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060051EB RID: 20971 RVA: 0x003F573C File Offset: 0x003F393C
		public bool IsEmpty
		{
			get
			{
				return this.diagversion == -1 && this.supplier == "" && this.version == "" && this.soft == "";
			}
		}

		// Token: 0x060051EC RID: 20972 RVA: 0x003F578C File Offset: 0x003F398C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (!(obj is RenaultECUIdents))
			{
				return false;
			}
			RenaultECUIdents renaultECUIdents = (RenaultECUIdents)obj;
			return this.RequestHeader == renaultECUIdents.RequestHeader && this.diagversion == renaultECUIdents.diagversion && this.supplier == renaultECUIdents.supplier && this.version == renaultECUIdents.version && this.soft == renaultECUIdents.soft;
		}

		// Token: 0x060051ED RID: 20973 RVA: 0x003F580C File Offset: 0x003F3A0C
		public int PartiallyEqualsPoints(RenaultECUIdents target)
		{
			if (target == null)
			{
				return 0;
			}
			if (target.IsEmpty)
			{
				return 0;
			}
			if (this.RequestHeader != target.RequestHeader)
			{
				return 0;
			}
			int num = 0;
			int num2;
			int num3;
			if (this.soft == target.soft)
			{
				num++;
			}
			else if (int.TryParse(this.soft, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2) && int.TryParse(target.soft, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3) && num2 == num3)
			{
				num++;
			}
			if (this.supplier == target.supplier)
			{
				num++;
			}
			int num4;
			int num5;
			if (this.version == target.version)
			{
				num++;
			}
			else if (int.TryParse(this.version, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num4) && int.TryParse(target.version, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num5) && num4 == num5)
			{
				num++;
			}
			if (this.diagversion == target.diagversion)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060051EE RID: 20974 RVA: 0x003F5910 File Offset: 0x003F3B10
		public bool PartiallyEquals(RenaultECUIdents target)
		{
			if (target == null)
			{
				return false;
			}
			if (target.IsEmpty)
			{
				return false;
			}
			if (this.RequestHeader != target.RequestHeader)
			{
				return false;
			}
			int num = 0;
			int num2;
			int num3;
			if (this.soft == target.soft)
			{
				num++;
			}
			else if (int.TryParse(this.soft, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2) && int.TryParse(target.soft, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3) && num2 == num3)
			{
				num++;
			}
			if (this.supplier == target.supplier)
			{
				num++;
			}
			int num4;
			int num5;
			if (this.version == target.version)
			{
				num++;
			}
			else if (int.TryParse(this.version, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num4) && int.TryParse(target.version, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num5) && num4 == num5)
			{
				num++;
			}
			if (this.diagversion == target.diagversion)
			{
				num++;
			}
			return num >= 3 || (num >= 2 && this.supplier == target.supplier);
		}

		// Token: 0x060051EF RID: 20975 RVA: 0x003F5A34 File Offset: 0x003F3C34
		public override int GetHashCode()
		{
			return 437812 * (this.diagversion.GetHashCode() + this.supplier.GetHashCode() + this.version.GetHashCode() + this.soft.GetHashCode());
		}

		// Token: 0x060051F0 RID: 20976 RVA: 0x003F5A7C File Offset: 0x003F3C7C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("Header: {0} {{diagversion: \"{1}\" supplier: \"{2}\"", this.RequestHeader, this.diagversion, this.supplier.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\0", "")),
				" version: \"",
				this.version.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\0", ""),
				"\" soft: \"",
				this.soft.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\0", ""),
				"\"}"
			});
		}

		// Token: 0x060051F1 RID: 20977 RVA: 0x003F5B6C File Offset: 0x003F3D6C
		public static RenaultECUIdents From2180(string requestHeader, byte[] data)
		{
			RenaultECUIdents renaultECUIdents = new RenaultECUIdents();
			try
			{
				renaultECUIdents.RequestHeader = requestHeader;
				renaultECUIdents.diagversion = (int)data[5];
				renaultECUIdents.supplier = BitHelpers.ByteArrayToHexString(new byte[]
				{
					data[6],
					data[7],
					data[8]
				});
				renaultECUIdents.soft = data[14].ToString("X2") + data[15].ToString("X2");
				renaultECUIdents.version = data[16].ToString("X2") + data[17].ToString("X2");
			}
			catch (Exception)
			{
			}
			return renaultECUIdents;
		}

		// Token: 0x060051F2 RID: 20978 RVA: 0x003F5C28 File Offset: 0x003F3E28
		public static RenaultECUIdents FromLada220121(string requestHeader, byte[] data220121)
		{
			RenaultECUIdents renaultECUIdents = new RenaultECUIdents();
			try
			{
				char c = (char)data220121[5];
				if (char.IsNumber(c))
				{
					renaultECUIdents.diagversion = int.Parse(c.ToString());
				}
				renaultECUIdents.supplier = Encoding.ASCII.GetString(data220121).Trim().Replace("\0", "");
				renaultECUIdents.version = Encoding.ASCII.GetString(data220121).Trim().Replace("\0", "");
				renaultECUIdents.RequestHeader = requestHeader;
			}
			catch (Exception)
			{
			}
			return renaultECUIdents;
		}

		// Token: 0x060051F3 RID: 20979 RVA: 0x003F5CC0 File Offset: 0x003F3EC0
		public static string GetSoftFromLada220125(byte[] data220125)
		{
			string text;
			try
			{
				text = BitHelpers.ByteArrayToHexString(data220125);
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x060051F4 RID: 20980 RVA: 0x003F5CF0 File Offset: 0x003F3EF0
		public RenaultECUIdents()
		{
		}

		// Token: 0x040031D0 RID: 12752
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x040031D1 RID: 12753
		[CompilerGenerated]
		private int <diagversion>k__BackingField;

		// Token: 0x040031D2 RID: 12754
		[CompilerGenerated]
		private string <supplier>k__BackingField;

		// Token: 0x040031D3 RID: 12755
		[CompilerGenerated]
		private string <version>k__BackingField;

		// Token: 0x040031D4 RID: 12756
		[CompilerGenerated]
		private string <soft>k__BackingField;
	}
}
