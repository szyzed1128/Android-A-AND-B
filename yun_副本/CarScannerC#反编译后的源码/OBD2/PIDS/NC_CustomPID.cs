using System;
using System.Collections.ObjectModel;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000405 RID: 1029
	public class NC_CustomPID : CustomPID
	{
		// Token: 0x06002A24 RID: 10788 RVA: 0x001F1BFC File Offset: 0x001EFDFC
		public NC_CustomPID(string Name, string ShortName, string Command, string Header, string Formula, UnitsHelper.Units Units, double MinValue, double MaxValue, string BeforeCommand, string AfterCommand, bool IsAction, Roles Role, CustomPIDType Type, int StartByteId, int DataLength, double Multiplier, double Divider, double Offset, bool IsSigned, bool IsFormulaHidden, bool IsVisible = true, ObservableCollection<TranslationItem> Translations = null)
			: base(Name, ShortName, Command, Header, Formula, Units, MinValue, MaxValue, BeforeCommand, AfterCommand, IsAction, Role, Type, StartByteId, DataLength, Multiplier, Divider, Offset, IsSigned, IsFormulaHidden, IsVisible, Translations)
		{
		}

		// Token: 0x170011EE RID: 4590
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x001F1BC5 File Offset: 0x001EFDC5
		// (set) Token: 0x06002A26 RID: 10790 RVA: 0x001F1C38 File Offset: 0x001EFE38
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

		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x001F1C41 File Offset: 0x001EFE41
		// (set) Token: 0x06002A28 RID: 10792 RVA: 0x000027D4 File Offset: 0x000009D4
		public override string BeforeCommand
		{
			get
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.NissanConsult3OpenCloseSession)
				{
					return "10C0";
				}
				return "";
			}
			set
			{
			}
		}

		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06002A29 RID: 10793 RVA: 0x001F1C67 File Offset: 0x001EFE67
		// (set) Token: 0x06002A2A RID: 10794 RVA: 0x000027D4 File Offset: 0x000009D4
		public override string AfterCommand
		{
			get
			{
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.NissanConsult3OpenCloseSession)
				{
					return "1081";
				}
				return "";
			}
			set
			{
			}
		}
	}
}
