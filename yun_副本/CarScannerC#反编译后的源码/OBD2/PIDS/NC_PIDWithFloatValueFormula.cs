using System;
using System.Globalization;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000404 RID: 1028
	public class NC_PIDWithFloatValueFormula : NC_CustomPID
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x001F1AE4 File Offset: 0x001EFCE4
		public NC_PIDWithFloatValueFormula(string Name, string Command, Func<byte[], double> funcFormula, UnitsHelper.Units unit)
			: base(Name, Name, Command, "", "", unit, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.ByteSet, 0, 1, 1.0, 1.0, 0.0, false, true, true, null)
		{
			base.Name = Name;
			this.Command = Command;
			string text;
			if (Translate.HasString("PID_" + Command + "_Short", out text))
			{
				base.ShortName = text;
			}
			else
			{
				base.ShortName = Name;
			}
			int num = 0;
			if (int.TryParse(Command, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				base.intCommand = num;
			}
			else
			{
				base.intCommand = -1;
			}
			this.FuncFormula = funcFormula;
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x001F1BAA File Offset: 0x001EFDAA
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			base.Value = this.FuncFormula(data);
		}

		// Token: 0x170011ED RID: 4589
		// (get) Token: 0x06002A22 RID: 10786 RVA: 0x001F1BC5 File Offset: 0x001EFDC5
		// (set) Token: 0x06002A23 RID: 10787 RVA: 0x001F1BF2 File Offset: 0x001EFDF2
		public override string Header
		{
			get
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					return "7E0";
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
				{
					return "DA15F1";
				}
				return "";
			}
			set
			{
				base.Header = value;
			}
		}

		// Token: 0x040017D3 RID: 6099
		protected Func<byte[], double> FuncFormula;
	}
}
