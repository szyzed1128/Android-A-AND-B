using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000936 RID: 2358
	internal class HexCodingModel : INotifyPropertyChanged
	{
		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06004E34 RID: 20020 RVA: 0x003ADE70 File Offset: 0x003AC070
		// (remove) Token: 0x06004E35 RID: 20021 RVA: 0x003ADEA8 File Offset: 0x003AC0A8
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

		// Token: 0x17001789 RID: 6025
		// (get) Token: 0x06004E36 RID: 20022 RVA: 0x003ADEDD File Offset: 0x003AC0DD
		// (set) Token: 0x06004E37 RID: 20023 RVA: 0x003ADEE5 File Offset: 0x003AC0E5
		public bool UseCustomAddress
		{
			get
			{
				return this._UseCustomAddress;
			}
			set
			{
				this._UseCustomAddress = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("UseCustomAddress"));
			}
		}

		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x06004E38 RID: 20024 RVA: 0x003ADF09 File Offset: 0x003AC109
		// (set) Token: 0x06004E39 RID: 20025 RVA: 0x003ADF11 File Offset: 0x003AC111
		public string CustomReadAddress
		{
			get
			{
				return this._CustomReadAddress;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._CustomReadAddress = value.Replace(" ", "").Trim();
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CustomReadAddress"));
			}
		}

		// Token: 0x1700178B RID: 6027
		// (get) Token: 0x06004E3A RID: 20026 RVA: 0x003ADF4D File Offset: 0x003AC14D
		// (set) Token: 0x06004E3B RID: 20027 RVA: 0x003ADF55 File Offset: 0x003AC155
		public string CustomWriteAddress
		{
			get
			{
				return this._CustomWriteAddress;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this._CustomWriteAddress = value.Replace(" ", "").Trim();
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CustomWriteAddress"));
			}
		}

		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x06004E3C RID: 20028 RVA: 0x003ADF91 File Offset: 0x003AC191
		// (set) Token: 0x06004E3D RID: 20029 RVA: 0x003ADF99 File Offset: 0x003AC199
		public HexStringObject Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Data"));
			}
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x003ADFBD File Offset: 0x003AC1BD
		// (set) Token: 0x06004E3F RID: 20031 RVA: 0x003ADFC5 File Offset: 0x003AC1C5
		public ICodingContainer Coding
		{
			[CompilerGenerated]
			get
			{
				return this.<Coding>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Coding>k__BackingField = value;
			}
		}

		// Token: 0x06004E40 RID: 20032 RVA: 0x003ADFD0 File Offset: 0x003AC1D0
		public HexCodingModel(ICodingContainer coding)
		{
			bool flag = false;
			if (coding is CustomizableCodingTemplate && !(coding as CustomizableCodingTemplate).MakeChangesToInitialData)
			{
				flag = true;
			}
			this._Data = new HexStringObject("")
			{
				VariableDataLength = flag
			};
			this.Coding = coding;
			if (coding is CustomizableCodingTemplate)
			{
				CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)coding;
				this.CustomReadAddress = customizableCodingTemplate.ReadModeAndAddress;
				this.CustomWriteAddress = customizableCodingTemplate.WriteModeAndAddress;
			}
		}

		// Token: 0x04002EC5 RID: 11973
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002EC6 RID: 11974
		private bool _UseCustomAddress;

		// Token: 0x04002EC7 RID: 11975
		private string _CustomReadAddress = "";

		// Token: 0x04002EC8 RID: 11976
		private string _CustomWriteAddress = "";

		// Token: 0x04002EC9 RID: 11977
		private HexStringObject _Data;

		// Token: 0x04002ECA RID: 11978
		[CompilerGenerated]
		private ICodingContainer <Coding>k__BackingField;
	}
}
