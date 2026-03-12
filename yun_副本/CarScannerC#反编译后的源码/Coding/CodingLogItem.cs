using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200085A RID: 2138
	public class CodingLogItem : INotifyPropertyChanged
	{
		// Token: 0x060048EF RID: 18671 RVA: 0x00376704 File Offset: 0x00374904
		public CodingLogItem()
		{
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x00376778 File Offset: 0x00374978
		public CodingLogItem(CodingLogItem.CodingTypes Type, long Timestamp, string Title, string UserFriendlyValue, string Address, string OldData, string NewData, string Password, string RequestHeader, string ResponseHeader, string VIN, string openSessionCommand = "", string preWriteCommands = "", string afterWriteCommands = "", string atst = "96", string preReadCommands = "")
		{
			this.Address = Address;
			this.Title = Title;
			this.Type = Type;
			this.OldData = OldData;
			this.NewData = NewData;
			this.Password = Password;
			this.RequestHeader = RequestHeader;
			this.ResponseHeader = ResponseHeader;
			this.Timestamp = Timestamp;
			this.UserFriendlyValue = UserFriendlyValue;
			this.VIN = VIN;
			this.OpenSessionCommand = openSessionCommand;
			this.PreWriteCommands = preWriteCommands;
			this.PostWriteCommands = afterWriteCommands;
			this.PreReadCommands = preReadCommands;
			this.ATST = atst;
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x00376868 File Offset: 0x00374A68
		private void SetAdditionalInfo()
		{
			if (this.Type == CodingLogItem.CodingTypes.MQB)
			{
				int num = 0;
				if (this.OldData != null && this.NewData != null && this.OldData.Length == 32 && this.NewData.Length == 32 && int.TryParse(this.Address, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num) && num >= 1360 && num <= 1411)
				{
					try
					{
						this.AdditionalGeneratedInfo = MQB_LightConfiguration.GetTextDifference(this.Address, this.OldData, this.NewData);
						this.HasAdditionalGeneratedInfo = true;
					}
					catch (Exception)
					{
						this.HasAdditionalGeneratedInfo = false;
					}
				}
			}
		}

		// Token: 0x17001657 RID: 5719
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x0037691C File Offset: 0x00374B1C
		// (set) Token: 0x060048F3 RID: 18675 RVA: 0x00376924 File Offset: 0x00374B24
		public bool IsVisible
		{
			[CompilerGenerated]
			get
			{
				return this.<IsVisible>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsVisible>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001658 RID: 5720
		// (get) Token: 0x060048F4 RID: 18676 RVA: 0x0037692D File Offset: 0x00374B2D
		// (set) Token: 0x060048F5 RID: 18677 RVA: 0x00376935 File Offset: 0x00374B35
		public CodingLogItem.CodingTypes Type
		{
			[CompilerGenerated]
			get
			{
				return this.<Type>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Type>k__BackingField = value;
			}
		}

		// Token: 0x17001659 RID: 5721
		// (get) Token: 0x060048F6 RID: 18678 RVA: 0x0037693E File Offset: 0x00374B3E
		// (set) Token: 0x060048F7 RID: 18679 RVA: 0x00376946 File Offset: 0x00374B46
		public string VIN
		{
			[CompilerGenerated]
			get
			{
				return this.<VIN>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<VIN>k__BackingField = value;
			}
		}

		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x060048F8 RID: 18680 RVA: 0x0037694F File Offset: 0x00374B4F
		// (set) Token: 0x060048F9 RID: 18681 RVA: 0x00376957 File Offset: 0x00374B57
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x060048FA RID: 18682 RVA: 0x00376960 File Offset: 0x00374B60
		// (set) Token: 0x060048FB RID: 18683 RVA: 0x00376968 File Offset: 0x00374B68
		public long Timestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<Timestamp>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Timestamp>k__BackingField = value;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x060048FC RID: 18684 RVA: 0x00376974 File Offset: 0x00374B74
		[JsonIgnore]
		public string Date
		{
			get
			{
				return new DateTime(this.Timestamp).ToString();
			}
		}

		// Token: 0x1700165D RID: 5725
		// (get) Token: 0x060048FD RID: 18685 RVA: 0x00376994 File Offset: 0x00374B94
		// (set) Token: 0x060048FE RID: 18686 RVA: 0x0037699C File Offset: 0x00374B9C
		public string Address
		{
			[CompilerGenerated]
			get
			{
				return this.<Address>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Address>k__BackingField = value;
			}
		}

		// Token: 0x1700165E RID: 5726
		// (get) Token: 0x060048FF RID: 18687 RVA: 0x003769A5 File Offset: 0x00374BA5
		// (set) Token: 0x06004900 RID: 18688 RVA: 0x003769AD File Offset: 0x00374BAD
		public string OldData
		{
			[CompilerGenerated]
			get
			{
				return this.<OldData>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldData>k__BackingField = value;
			}
		}

		// Token: 0x1700165F RID: 5727
		// (get) Token: 0x06004901 RID: 18689 RVA: 0x003769B6 File Offset: 0x00374BB6
		// (set) Token: 0x06004902 RID: 18690 RVA: 0x003769BE File Offset: 0x00374BBE
		public string NewData
		{
			[CompilerGenerated]
			get
			{
				return this.<NewData>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NewData>k__BackingField = value;
			}
		}

		// Token: 0x17001660 RID: 5728
		// (get) Token: 0x06004903 RID: 18691 RVA: 0x003769C7 File Offset: 0x00374BC7
		// (set) Token: 0x06004904 RID: 18692 RVA: 0x003769CF File Offset: 0x00374BCF
		public string Password
		{
			[CompilerGenerated]
			get
			{
				return this.<Password>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Password>k__BackingField = value;
			}
		}

		// Token: 0x17001661 RID: 5729
		// (get) Token: 0x06004905 RID: 18693 RVA: 0x003769D8 File Offset: 0x00374BD8
		// (set) Token: 0x06004906 RID: 18694 RVA: 0x003769E0 File Offset: 0x00374BE0
		public string UserFriendlyValue
		{
			[CompilerGenerated]
			get
			{
				return this.<UserFriendlyValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UserFriendlyValue>k__BackingField = value;
			}
		}

		// Token: 0x17001662 RID: 5730
		// (get) Token: 0x06004907 RID: 18695 RVA: 0x003769E9 File Offset: 0x00374BE9
		// (set) Token: 0x06004908 RID: 18696 RVA: 0x003769F1 File Offset: 0x00374BF1
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
		}

		// Token: 0x17001663 RID: 5731
		// (get) Token: 0x06004909 RID: 18697 RVA: 0x003769FA File Offset: 0x00374BFA
		// (set) Token: 0x0600490A RID: 18698 RVA: 0x00376A02 File Offset: 0x00374C02
		public string ResponseHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseHeader>k__BackingField = value;
			}
		}

		// Token: 0x17001664 RID: 5732
		// (get) Token: 0x0600490B RID: 18699 RVA: 0x00376A0B File Offset: 0x00374C0B
		// (set) Token: 0x0600490C RID: 18700 RVA: 0x00376A13 File Offset: 0x00374C13
		public string PreReadCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<PreReadCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PreReadCommands>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001665 RID: 5733
		// (get) Token: 0x0600490D RID: 18701 RVA: 0x00376A1C File Offset: 0x00374C1C
		// (set) Token: 0x0600490E RID: 18702 RVA: 0x00376A24 File Offset: 0x00374C24
		public string PostWriteCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<PostWriteCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PostWriteCommands>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001666 RID: 5734
		// (get) Token: 0x0600490F RID: 18703 RVA: 0x00376A2D File Offset: 0x00374C2D
		// (set) Token: 0x06004910 RID: 18704 RVA: 0x00376A35 File Offset: 0x00374C35
		public string PreWriteCommands
		{
			[CompilerGenerated]
			get
			{
				return this.<PreWriteCommands>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PreWriteCommands>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001667 RID: 5735
		// (get) Token: 0x06004911 RID: 18705 RVA: 0x00376A3E File Offset: 0x00374C3E
		// (set) Token: 0x06004912 RID: 18706 RVA: 0x00376A46 File Offset: 0x00374C46
		public string OpenSessionCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<OpenSessionCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OpenSessionCommand>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06004913 RID: 18707 RVA: 0x00376A4F File Offset: 0x00374C4F
		// (set) Token: 0x06004914 RID: 18708 RVA: 0x00376A57 File Offset: 0x00374C57
		public string CloseSessionCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<CloseSessionCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CloseSessionCommand>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x00376A60 File Offset: 0x00374C60
		// (set) Token: 0x06004916 RID: 18710 RVA: 0x00376A68 File Offset: 0x00374C68
		public string ResetECUCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<ResetECUCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResetECUCommand>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06004917 RID: 18711 RVA: 0x00376A71 File Offset: 0x00374C71
		// (set) Token: 0x06004918 RID: 18712 RVA: 0x00376A79 File Offset: 0x00374C79
		public string ATST
		{
			[CompilerGenerated]
			get
			{
				return this.<ATST>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ATST>k__BackingField = value;
			}
		} = "96";

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06004919 RID: 18713 RVA: 0x00376A82 File Offset: 0x00374C82
		// (set) Token: 0x0600491A RID: 18714 RVA: 0x00376A8A File Offset: 0x00374C8A
		[JsonIgnore]
		public bool HasAdditionalGeneratedInfo
		{
			[CompilerGenerated]
			get
			{
				return this.<HasAdditionalGeneratedInfo>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HasAdditionalGeneratedInfo>k__BackingField = value;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x0600491B RID: 18715 RVA: 0x00376A93 File Offset: 0x00374C93
		// (set) Token: 0x0600491C RID: 18716 RVA: 0x00376A9B File Offset: 0x00374C9B
		[JsonIgnore]
		public string AdditionalGeneratedInfo
		{
			[CompilerGenerated]
			get
			{
				return this.<AdditionalGeneratedInfo>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AdditionalGeneratedInfo>k__BackingField = value;
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x0600491D RID: 18717 RVA: 0x00376AA4 File Offset: 0x00374CA4
		// (remove) Token: 0x0600491E RID: 18718 RVA: 0x00376ADC File Offset: 0x00374CDC
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

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x0600491F RID: 18719 RVA: 0x00376B11 File Offset: 0x00374D11
		// (set) Token: 0x06004920 RID: 18720 RVA: 0x00376B19 File Offset: 0x00374D19
		[JsonIgnore]
		public bool Selected
		{
			get
			{
				return this._Selected;
			}
			set
			{
				this._Selected = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Selected"));
			}
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x00376B40 File Offset: 0x00374D40
		public ICodingContainer ToMQBAdaptationTemplate()
		{
			if (string.IsNullOrEmpty(this.ResponseHeader))
			{
				this.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(this.RequestHeader, "Audi", null);
			}
			return new MQBEasyCodingItem(CodingGroup.Other, this.Title, "", this.Address, this.RequestHeader, this.ResponseHeader, this.Password, "", (byte[] data, string value, MQBEasyCodingItem codingItem) => BitHelpers.ConvertHexToBytesX(this.OldData), delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data != null)
				{
					return BitHelpers.ByteArrayToHexString(data);
				}
				return "";
			}, new MQBAdaptationOption[] { CodingLogItem.RestoreBackupOption });
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x00376BD8 File Offset: 0x00374DD8
		public static void SaveLogItems(IEnumerable<CodingLogItem> items)
		{
			object obj = CodingLogItem.lock_object;
			bool flag = false;
			try
			{
				Monitor.Enter(obj, ref flag);
				Random rnd = new Random();
				items = (from x in items.ToArray<CodingLogItem>()
					orderby rnd.Next()
					select x).ToArray<CodingLogItem>();
				try
				{
					using (FileStream fileStream = File.Open(FileSystemHelper.GetLocalFilePath("coding.bak"), FileMode.Create))
					{
						using (ShiftStream2 shiftStream = new ShiftStream2(fileStream))
						{
							using (BsonWriter bsonWriter = new BsonWriter(shiftStream))
							{
								new JsonSerializer().Serialize(bsonWriter, items);
								bsonWriter.Flush();
								shiftStream.Flush();
								fileStream.Flush();
							}
						}
					}
				}
				catch (Exception)
				{
				}
				if (PlatformHelper.IsAndroid)
				{
					try
					{
						string externalStoragePath = FileSystemHelper.ExternalStoragePath;
						if (externalStoragePath != null && Directory.Exists(externalStoragePath))
						{
							using (FileStream fileStream2 = File.Open(Path.Combine(externalStoragePath, "coding.bak"), FileMode.Create))
							{
								using (ShiftStream2 shiftStream2 = new ShiftStream2(fileStream2))
								{
									using (BsonWriter bsonWriter2 = new BsonWriter(shiftStream2))
									{
										new JsonSerializer().Serialize(bsonWriter2, items);
										bsonWriter2.Flush();
										shiftStream2.Flush();
										fileStream2.Flush();
									}
								}
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(obj);
				}
			}
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00376E14 File Offset: 0x00375014
		internal ICodingContainer ToMQBParametrizeBase()
		{
			MQBParametrizeBase mqbparametrizeBase = new MQBParametrizeBase
			{
				Name = this.Title,
				Description = "",
				InnerDescription = "",
				Translations = new ObservableCollection<TranslationItem>(),
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Address = int.Parse(this.Address, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat),
				ATST = this.ATST,
				Password = this.Password,
				RequestHeader = this.RequestHeader,
				ResponseHeader = this.ResponseHeader,
				PreReadCommands = this.OpenSessionCommand,
				PreWriteCommands = this.PreWriteCommands,
				PostWriteCommands = this.PostWriteCommands
			};
			if (this.Values != null)
			{
				mqbparametrizeBase.AddressFormatLength = int.Parse(this.Values["AddressFormatLength"], NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
				mqbparametrizeBase.DataLengthFormatLength = int.Parse(this.Values["DataLengthFormatLength"], NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
				mqbparametrizeBase.DataLength = int.Parse(this.Values["DataLength"], NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
			}
			return mqbparametrizeBase;
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00376F58 File Offset: 0x00375158
		internal ICodingContainer ToMQBCustomizableMode22Coding()
		{
			return new MQBCustomizableMode22Coding
			{
				Name = this.Title,
				Description = "",
				InnerDescription = "",
				Translations = new ObservableCollection<TranslationItem>(),
				ValueType = AdaptationValueTypes.InputHexDataType,
				Address = this.Address,
				ATST = this.ATST,
				Password = this.Password,
				RequestHeader = this.RequestHeader,
				ResponseHeader = this.ResponseHeader,
				PreReadCommands = this.OpenSessionCommand,
				PreWriteCommands = this.PreWriteCommands,
				PostWriteCommands = this.PostWriteCommands
			};
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00377000 File Offset: 0x00375200
		internal ICodingContainer ToCustomizableCodingTemplate()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate
			{
				UID = new Random().Next(),
				Name = this.Title,
				Description = "",
				InnerDescription = "",
				Translations = new ObservableCollection<TranslationItem>(),
				ValueType = AdaptationValueTypes.InputHexDataType,
				WriteModeAndAddress = this.Address,
				RequestHeader = this.RequestHeader,
				ResponseHeader = this.ResponseHeader,
				OpenSessionCommand = this.OpenSessionCommand,
				CloseSessionCommand = this.CloseSessionCommand,
				ATST = this.ATST,
				Password = this.Password,
				MakeChangesToInitialData = false,
				PreWriteCommands = this.PreWriteCommands,
				PostWriteCommands = this.PostWriteCommands,
				PreReadCommands = this.PreReadCommands
			};
			if (this.Values != null && this.Values.Count > 0)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this.Values)
				{
					try
					{
						string key = keyValuePair.Key;
						string value = keyValuePair.Value;
						PropertyInfo property = customizableCodingTemplate.GetType().GetProperty(key);
						if (property.PropertyType == typeof(string))
						{
							property.SetValue(customizableCodingTemplate, value);
						}
						else if (property.PropertyType == typeof(int))
						{
							int num = int.Parse(value);
							property.SetValue(customizableCodingTemplate, num);
						}
						else if (property.PropertyType == typeof(bool))
						{
							bool flag = bool.Parse(value);
							property.SetValue(customizableCodingTemplate, flag);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (string.IsNullOrEmpty(customizableCodingTemplate.OpenSessionCommand) && (selectedBrand == "Renault" || selectedBrand == "Dacia" || selectedBrand == "Lada" || selectedBrand == "VAZ" || selectedBrand == "Лада" || selectedBrand == "ВАЗ") && string.IsNullOrEmpty(customizableCodingTemplate.OpenSessionCommand))
			{
				customizableCodingTemplate.OpenSessionCommand = "10C0";
			}
			try
			{
				if (CAN11bitHelper.GetPossibleResponseHeader(this.RequestHeader, "Audi", null) == this.ResponseHeader && this.UserFriendlyValue == "BACKUP" && this.Values.Count == 0 && this.Address.Length == 4)
				{
					return this.ToMQBAdaptationTemplate();
				}
			}
			catch (Exception)
			{
			}
			return customizableCodingTemplate;
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x003772D8 File Offset: 0x003754D8
		internal ICodingContainer ToToyotaCoding()
		{
			return new ToyotaCoding(new Random().Next(), this.Title, "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, this.Address, this.RequestHeader, this.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[] { CodingLogItem.RestoreBackupOption });
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x00377348 File Offset: 0x00375548
		internal ICodingContainer ToKWP2000Coding()
		{
			return new KWP2000Coding(new Random().Next(), this.Title, "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, this.Address, this.RequestHeader, this.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[] { CodingLogItem.RestoreBackupOption });
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x003773B8 File Offset: 0x003755B8
		internal ICodingContainer ToUDSCoding()
		{
			return new UDSCoding(new Random().Next(), this.Title, "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, this.Address, this.RequestHeader, this.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[] { CodingLogItem.RestoreBackupOption });
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x00377428 File Offset: 0x00375628
		internal ICodingContainer ToHyundaiKiaUDSCoding()
		{
			return new HyundaiKiaUDSCoding(new Random().Next(), this.Title, "", "", "", new TranslationItem[0], AdaptationValueTypes.InputHexDataType, this.Address, this.RequestHeader, this.ResponseHeader, 0, 1, 1.0, 0.0, false, false, false, new MQBAdaptationOption[] { CodingLogItem.RestoreBackupOption });
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x0600492A RID: 18730 RVA: 0x00377497 File Offset: 0x00375697
		// (set) Token: 0x0600492B RID: 18731 RVA: 0x0037749F File Offset: 0x0037569F
		public Dictionary<string, string> Values
		{
			[CompilerGenerated]
			get
			{
				return this.<Values>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Values>k__BackingField = value;
			}
		} = new Dictionary<string, string>();

		// Token: 0x0600492C RID: 18732 RVA: 0x003774A8 File Offset: 0x003756A8
		public static void RecordToLog(IEnumerable<CodingLogItem> items)
		{
			List<CodingLogItem> list = CodingLogItem.LoadLogItems();
			List<CodingLogItem> list2 = items.ToList<CodingLogItem>();
			list2.AddRange(list);
			CodingLogItem.SaveLogItems(list2);
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x003774D0 File Offset: 0x003756D0
		public static void RecordToLog(string Title, string UserFriendlyValue, CodingLogItem.CodingTypes Type, string Address, string OldData, string NewData, string Password, string RequestHeader, string ResponseHeader, string openSessionCommand = "", string preWriteCommands = "", string afterWriteCommands = "", string atst = "96", Dictionary<string, string> dict = null, string preReadCommands = "")
		{
			string text = "";
			if (CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.IsVINAvailable && !string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
			{
				text = CarInfoViewModel.Instance.VIN;
			}
			CodingLogItem codingLogItem = new CodingLogItem(Type, DateTimeNowHelper.NowSafe.Ticks, Title, UserFriendlyValue, Address, OldData, NewData, Password, RequestHeader, ResponseHeader, text, openSessionCommand, preWriteCommands, afterWriteCommands, atst, preReadCommands);
			if (dict != null)
			{
				codingLogItem.Values = dict;
			}
			List<CodingLogItem> list = CodingLogItem.LoadLogItems();
			list.Insert(0, codingLogItem);
			CodingLogItem.SaveLogItems(list);
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x0037755C File Offset: 0x0037575C
		public static List<CodingLogItem> LoadLogItems()
		{
			HashSet<CodingLogItem> hashSet = new HashSet<CodingLogItem>();
			object obj = CodingLogItem.lock_object;
			lock (obj)
			{
				string localFilePath = FileSystemHelper.GetLocalFilePath("coding.bak");
				if (File.Exists(localFilePath))
				{
					using (Stream stream = File.Open(localFilePath, FileMode.Open))
					{
						using (ShiftStream2 shiftStream = new ShiftStream2(stream))
						{
							using (BsonReader bsonReader = new BsonReader(shiftStream))
							{
								try
								{
									bsonReader.ReadRootValueAsArray = true;
									List<CodingLogItem> list = new JsonSerializer().Deserialize<List<CodingLogItem>>(bsonReader);
									foreach (CodingLogItem codingLogItem in list)
									{
										if (codingLogItem.RequestHeader != null && codingLogItem.RequestHeader.Length == 3 && codingLogItem.Values != null && codingLogItem.Values.ContainsKey("Protocol") && codingLogItem.Values["Protocol"] == "7")
										{
											codingLogItem.Values["Protocol"] = "6";
										}
									}
									hashSet = new HashSet<CodingLogItem>(list);
								}
								catch (Exception)
								{
								}
							}
						}
					}
				}
				if (PlatformHelper.IsAndroid)
				{
					try
					{
						string externalStoragePath = FileSystemHelper.ExternalStoragePath;
						if (externalStoragePath != null && Directory.Exists(externalStoragePath) && PlatformHelper.DroidService.Android_OS_Environment_MediaMounted.Equals(PlatformHelper.DroidService.Android_OS_Environment_ExternalStorageState))
						{
							using (Stream stream2 = File.Open(Path.Combine(externalStoragePath, "coding.bak"), FileMode.Open))
							{
								using (ShiftStream2 shiftStream2 = new ShiftStream2(stream2))
								{
									using (BsonReader bsonReader2 = new BsonReader(shiftStream2))
									{
										try
										{
											bsonReader2.ReadRootValueAsArray = true;
											foreach (CodingLogItem codingLogItem2 in new JsonSerializer().Deserialize<List<CodingLogItem>>(bsonReader2))
											{
												if (!hashSet.Contains(codingLogItem2))
												{
													hashSet.Add(codingLogItem2);
												}
											}
										}
										catch (Exception)
										{
										}
									}
								}
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
			List<CodingLogItem> list2 = hashSet.OrderByDescending((CodingLogItem x) => x.Timestamp).ToList<CodingLogItem>();
			foreach (CodingLogItem codingLogItem3 in list2)
			{
				if (codingLogItem3.Type == CodingLogItem.CodingTypes.MQB)
				{
					if (codingLogItem3.Title != null && codingLogItem3.Address != null && !codingLogItem3.Title.Contains(codingLogItem3.Address))
					{
						codingLogItem3.Title = codingLogItem3.Title + " [$" + codingLogItem3.Address + "]";
					}
					if (codingLogItem3.Title != null && codingLogItem3.Title.Contains("[$0600]"))
					{
						codingLogItem3.Title = codingLogItem3.Title.Replace("[$0600]", "[$LC]");
					}
					if (codingLogItem3.Title != null && !string.IsNullOrEmpty(codingLogItem3.RequestHeader))
					{
						string unitIdFromRequestHeader = VagUnitHelper.GetUnitIdFromRequestHeader(codingLogItem3.RequestHeader);
						if (!string.IsNullOrEmpty(unitIdFromRequestHeader))
						{
							string text = "[U:" + unitIdFromRequestHeader + "]";
							if (!codingLogItem3.Title.Contains(text))
							{
								int num = codingLogItem3.Title.IndexOf("[$");
								if (num >= 0)
								{
									codingLogItem3.Title = codingLogItem3.Title.Substring(0, num) + text + codingLogItem3.Title.Substring(num);
								}
							}
						}
					}
					if (codingLogItem3.Title.IndexOf("[$LC]") >= 0 && Regex.Matches(codingLogItem3.Title, "[$LC]").Count > 1)
					{
						codingLogItem3.Title = codingLogItem3.Title.Replace("[$LC]", "").Trim() + " [$LC]";
					}
					codingLogItem3.SetAdditionalInfo();
				}
			}
			return list2;
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x00377AAC File Offset: 0x00375CAC
		public static async Task<List<CodingLogItem>> LoadLogItemsAsync()
		{
			return await Task.Run<List<CodingLogItem>>(() => CodingLogItem.LoadLogItems());
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x00377AE8 File Offset: 0x00375CE8
		private static void AddLightConfigurationDescription(List<CodingLogItem> items)
		{
			foreach (CodingLogItem codingLogItem in items)
			{
				if ((codingLogItem.Type == CodingLogItem.CodingTypes.MQB || codingLogItem.Type == CodingLogItem.CodingTypes.MQBCustomizableMode22Coding) && codingLogItem.OldData != null && codingLogItem.NewData != null)
				{
					int length = codingLogItem.OldData.Length;
					int length2 = codingLogItem.NewData.Length;
				}
			}
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x00377B68 File Offset: 0x00375D68
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			CodingLogItem codingLogItem = obj as CodingLogItem;
			return codingLogItem != null && this.Type == codingLogItem.Type && this.VIN == codingLogItem.VIN && this.Title == codingLogItem.Title && this.Timestamp == codingLogItem.Timestamp && this.Date == codingLogItem.Date && this.Address == codingLogItem.Address && this.OldData == codingLogItem.OldData && this.NewData == codingLogItem.NewData && this.Password == codingLogItem.Password && this.UserFriendlyValue == codingLogItem.UserFriendlyValue && this.RequestHeader == codingLogItem.RequestHeader && this.ResponseHeader == codingLogItem.ResponseHeader && this._Selected == codingLogItem._Selected && this.Selected == codingLogItem.Selected;
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x00377C94 File Offset: 0x00375E94
		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			hashCode.Add<CodingLogItem.CodingTypes>(this.Type);
			hashCode.Add<string>(this.VIN);
			hashCode.Add<string>(this.Title);
			hashCode.Add<long>(this.Timestamp);
			hashCode.Add<string>(this.Date);
			hashCode.Add<string>(this.Address);
			hashCode.Add<string>(this.OldData);
			hashCode.Add<string>(this.NewData);
			hashCode.Add<string>(this.Password);
			hashCode.Add<string>(this.UserFriendlyValue);
			hashCode.Add<string>(this.RequestHeader);
			hashCode.Add<string>(this.ResponseHeader);
			hashCode.Add<bool>(this._Selected);
			hashCode.Add<bool>(this.Selected);
			return hashCode.ToHashCode();
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00377D66 File Offset: 0x00375F66
		// Note: this type is marked as 'beforefieldinit'.
		static CodingLogItem()
		{
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x00377D8B File Offset: 0x00375F8B
		[CompilerGenerated]
		private byte[] <ToMQBAdaptationTemplate>b__98_0(byte[] data, string value, MQBEasyCodingItem codingItem)
		{
			return BitHelpers.ConvertHexToBytesX(this.OldData);
		}

		// Token: 0x04002A22 RID: 10786
		[CompilerGenerated]
		private bool <IsVisible>k__BackingField;

		// Token: 0x04002A23 RID: 10787
		[CompilerGenerated]
		private CodingLogItem.CodingTypes <Type>k__BackingField;

		// Token: 0x04002A24 RID: 10788
		[CompilerGenerated]
		private string <VIN>k__BackingField;

		// Token: 0x04002A25 RID: 10789
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x04002A26 RID: 10790
		[CompilerGenerated]
		private long <Timestamp>k__BackingField;

		// Token: 0x04002A27 RID: 10791
		[CompilerGenerated]
		private string <Address>k__BackingField;

		// Token: 0x04002A28 RID: 10792
		[CompilerGenerated]
		private string <OldData>k__BackingField;

		// Token: 0x04002A29 RID: 10793
		[CompilerGenerated]
		private string <NewData>k__BackingField;

		// Token: 0x04002A2A RID: 10794
		[CompilerGenerated]
		private string <Password>k__BackingField;

		// Token: 0x04002A2B RID: 10795
		[CompilerGenerated]
		private string <UserFriendlyValue>k__BackingField;

		// Token: 0x04002A2C RID: 10796
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04002A2D RID: 10797
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04002A2E RID: 10798
		[CompilerGenerated]
		private string <PreReadCommands>k__BackingField;

		// Token: 0x04002A2F RID: 10799
		[CompilerGenerated]
		private string <PostWriteCommands>k__BackingField;

		// Token: 0x04002A30 RID: 10800
		[CompilerGenerated]
		private string <PreWriteCommands>k__BackingField;

		// Token: 0x04002A31 RID: 10801
		[CompilerGenerated]
		private string <OpenSessionCommand>k__BackingField;

		// Token: 0x04002A32 RID: 10802
		[CompilerGenerated]
		private string <CloseSessionCommand>k__BackingField;

		// Token: 0x04002A33 RID: 10803
		[CompilerGenerated]
		private string <ResetECUCommand>k__BackingField;

		// Token: 0x04002A34 RID: 10804
		[CompilerGenerated]
		private string <ATST>k__BackingField;

		// Token: 0x04002A35 RID: 10805
		[CompilerGenerated]
		private bool <HasAdditionalGeneratedInfo>k__BackingField;

		// Token: 0x04002A36 RID: 10806
		[CompilerGenerated]
		private string <AdditionalGeneratedInfo>k__BackingField;

		// Token: 0x04002A37 RID: 10807
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002A38 RID: 10808
		private bool _Selected;

		// Token: 0x04002A39 RID: 10809
		public static MQBAdaptationOption RestoreBackupOption = new MQBAdaptationOption(Translate.GetString("coding_Restored"), "UNDO");

		// Token: 0x04002A3A RID: 10810
		[CompilerGenerated]
		private Dictionary<string, string> <Values>k__BackingField;

		// Token: 0x04002A3B RID: 10811
		private static object lock_object = new object();

		// Token: 0x0200085B RID: 2139
		public enum CodingTypes
		{
			// Token: 0x04002A3D RID: 10813
			MQB,
			// Token: 0x04002A3E RID: 10814
			KWP2000,
			// Token: 0x04002A3F RID: 10815
			TOYOTA,
			// Token: 0x04002A40 RID: 10816
			UDS,
			// Token: 0x04002A41 RID: 10817
			HyundaiKiaUDS,
			// Token: 0x04002A42 RID: 10818
			CustomizableCodingTemplate,
			// Token: 0x04002A43 RID: 10819
			MQBParametrizeBase,
			// Token: 0x04002A44 RID: 10820
			MQBCustomizableMode22Coding
		}

		// Token: 0x0200085C RID: 2140
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004935 RID: 18741 RVA: 0x00377D98 File Offset: 0x00375F98
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004936 RID: 18742 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004937 RID: 18743 RVA: 0x00377DA4 File Offset: 0x00375FA4
			internal string <ToMQBAdaptationTemplate>b__98_1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (data != null)
				{
					return BitHelpers.ByteArrayToHexString(data);
				}
				return "";
			}

			// Token: 0x06004938 RID: 18744 RVA: 0x00377DB5 File Offset: 0x00375FB5
			internal long <LoadLogItems>b__114_0(CodingLogItem x)
			{
				return x.Timestamp;
			}

			// Token: 0x06004939 RID: 18745 RVA: 0x00377DBD File Offset: 0x00375FBD
			internal List<CodingLogItem> <LoadLogItemsAsync>b__115_0()
			{
				return CodingLogItem.LoadLogItems();
			}

			// Token: 0x04002A45 RID: 10821
			public static readonly CodingLogItem.<>c <>9 = new CodingLogItem.<>c();

			// Token: 0x04002A46 RID: 10822
			public static Func<byte[], MQBEasyCodingItem, string> <>9__98_1;

			// Token: 0x04002A47 RID: 10823
			public static Func<CodingLogItem, long> <>9__114_0;

			// Token: 0x04002A48 RID: 10824
			public static Func<List<CodingLogItem>> <>9__115_0;
		}

		// Token: 0x0200085D RID: 2141
		[CompilerGenerated]
		private sealed class <>c__DisplayClass99_0
		{
			// Token: 0x0600493A RID: 18746 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass99_0()
			{
			}

			// Token: 0x0600493B RID: 18747 RVA: 0x00377DC4 File Offset: 0x00375FC4
			internal int <SaveLogItems>b__0(CodingLogItem x)
			{
				return this.rnd.Next();
			}

			// Token: 0x04002A49 RID: 10825
			public Random rnd;
		}

		// Token: 0x0200085E RID: 2142
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadLogItemsAsync>d__115 : IAsyncStateMachine
		{
			// Token: 0x0600493C RID: 18748 RVA: 0x00377DD4 File Offset: 0x00375FD4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				List<CodingLogItem> result;
				try
				{
					TaskAwaiter<List<CodingLogItem>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Task.Run<List<CodingLogItem>>(() => CodingLogItem.LoadLogItems()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<List<CodingLogItem>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CodingLogItem>>, CodingLogItem.<LoadLogItemsAsync>d__115>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<List<CodingLogItem>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<CodingLogItem>>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x0600493D RID: 18749 RVA: 0x00377EA0 File Offset: 0x003760A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A4A RID: 10826
			public int <>1__state;

			// Token: 0x04002A4B RID: 10827
			public AsyncTaskMethodBuilder<List<CodingLogItem>> <>t__builder;

			// Token: 0x04002A4C RID: 10828
			private TaskAwaiter<List<CodingLogItem>> <>u__1;
		}
	}
}
