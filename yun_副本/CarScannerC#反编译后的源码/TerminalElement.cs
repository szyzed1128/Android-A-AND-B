using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200005A RID: 90
	public class TerminalElement
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000222 RID: 546 RVA: 0x000177FE File Offset: 0x000159FE
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00017806 File Offset: 0x00015A06
		public TerminalElementTypes ElementType
		{
			[CompilerGenerated]
			get
			{
				return this.<ElementType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ElementType>k__BackingField = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0001780F File Offset: 0x00015A0F
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00017817 File Offset: 0x00015A17
		public string Text
		{
			[CompilerGenerated]
			get
			{
				return this.<Text>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Text>k__BackingField = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00017820 File Offset: 0x00015A20
		public Color Color
		{
			get
			{
				if (this.ElementType == TerminalElementTypes.Command)
				{
					return Color.Red;
				}
				return Color.Blue;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00002050 File Offset: 0x00000250
		public TerminalElement()
		{
		}

		// Token: 0x040001A1 RID: 417
		[CompilerGenerated]
		private TerminalElementTypes <ElementType>k__BackingField;

		// Token: 0x040001A2 RID: 418
		[CompilerGenerated]
		private string <Text>k__BackingField;

		// Token: 0x040001A3 RID: 419
		public bool LineFinished;
	}
}
