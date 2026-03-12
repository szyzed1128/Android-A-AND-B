using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200085F RID: 2143
	internal class CodingLogItemGroup : List<CodingLogItem>
	{
		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x00377EAE File Offset: 0x003760AE
		// (set) Token: 0x0600493F RID: 18751 RVA: 0x00377EB6 File Offset: 0x003760B6
		public string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		}

		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x00377EBF File Offset: 0x003760BF
		// (set) Token: 0x06004941 RID: 18753 RVA: 0x00377EC7 File Offset: 0x003760C7
		public string Address
		{
			[CompilerGenerated]
			get
			{
				return this.<Address>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Address>k__BackingField = value;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x06004942 RID: 18754 RVA: 0x00377ED0 File Offset: 0x003760D0
		// (set) Token: 0x06004943 RID: 18755 RVA: 0x00377ED8 File Offset: 0x003760D8
		public string VIN
		{
			[CompilerGenerated]
			get
			{
				return this.<VIN>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<VIN>k__BackingField = value;
			}
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x00377EE1 File Offset: 0x003760E1
		public CodingLogItemGroup(CodingLogItem firstItem)
		{
			base.Add(firstItem);
			this.RequestHeader = firstItem.RequestHeader;
			this.Address = firstItem.Address;
			this.VIN = firstItem.VIN;
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00377F14 File Offset: 0x00376114
		public bool CanAdd(CodingLogItem item)
		{
			return item.RequestHeader == this.RequestHeader && item.Address == this.Address && item.VIN == this.VIN;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00377F54 File Offset: 0x00376154
		public CodingLogItem GetItemForRestore()
		{
			CodingLogItem[] array = this.OrderBy((CodingLogItem x) => x.Timestamp).ToArray<CodingLogItem>();
			base.Clear();
			base.AddRange(array);
			if (base.Count == 1)
			{
				if (base[0].OldData == base[0].NewData)
				{
					return null;
				}
				if (string.IsNullOrEmpty(base[0].OldData))
				{
					return null;
				}
				if (string.IsNullOrEmpty(base[0].NewData))
				{
					return null;
				}
				return base[0];
			}
			else
			{
				CodingLogItem codingLogItem = this.FirstOrDefault((CodingLogItem x) => !string.IsNullOrEmpty(x.OldData));
				if (codingLogItem == null)
				{
					return null;
				}
				CodingLogItem codingLogItem2 = this.LastOrDefault((CodingLogItem x) => !string.IsNullOrEmpty(x.NewData) || (string.IsNullOrEmpty(x.NewData) && !string.IsNullOrEmpty(x.OldData) && x.UserFriendlyValue != null && x.UserFriendlyValue.Contains("BACKUP")));
				if (codingLogItem2 == null)
				{
					return null;
				}
				string oldData = codingLogItem.OldData;
				string text = ((!string.IsNullOrEmpty(codingLogItem2.NewData)) ? codingLogItem2.NewData : codingLogItem2.OldData);
				if (string.IsNullOrEmpty(oldData))
				{
					return null;
				}
				if (string.IsNullOrEmpty(text))
				{
					return null;
				}
				if (oldData == text)
				{
					return null;
				}
				return base[0];
			}
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x00378098 File Offset: 0x00376298
		public string GetGroupReport()
		{
			CodingLogItem[] array = this.OrderBy((CodingLogItem x) => x.Timestamp).ToArray<CodingLogItem>();
			base.Clear();
			base.AddRange(array);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("\r\n");
			stringBuilder.Append("VIN=" + this.VIN + "; ");
			stringBuilder.Append(string.Concat(new string[] { "ECU: ", this.RequestHeader, "; ADDRESS=", this.Address, "\n" }));
			if (base.Count == 1)
			{
				stringBuilder.AppendLine("DATA DIFFERENT ");
				stringBuilder.AppendLine("Old data=" + base[0].OldData + "; Time=" + base[0].Date);
				stringBuilder.AppendLine("New data=" + base[0].NewData + "; Time=" + base[0].Date);
			}
			else
			{
				string oldData = base[0].OldData;
				string newData = base[base.Count - 1].NewData;
				if (oldData == newData)
				{
					stringBuilder.AppendLine("DATA EQUALS");
					stringBuilder.AppendLine("Data=" + newData);
				}
				else
				{
					stringBuilder.AppendLine("DATA DIFFERENT");
					stringBuilder.AppendLine("Old data=" + oldData + "; Time=" + base[0].Date);
					stringBuilder.AppendLine("New data=" + newData + "; Time=" + base[base.Count - 1].Date);
				}
			}
			stringBuilder.AppendLine("=================================");
			return stringBuilder.ToString();
		}

		// Token: 0x04002A4D RID: 10829
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x04002A4E RID: 10830
		[CompilerGenerated]
		private string <Address>k__BackingField;

		// Token: 0x04002A4F RID: 10831
		[CompilerGenerated]
		private string <VIN>k__BackingField;

		// Token: 0x02000860 RID: 2144
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004948 RID: 18760 RVA: 0x00378272 File Offset: 0x00376472
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004949 RID: 18761 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600494A RID: 18762 RVA: 0x00377DB5 File Offset: 0x00375FB5
			internal long <GetItemForRestore>b__14_0(CodingLogItem x)
			{
				return x.Timestamp;
			}

			// Token: 0x0600494B RID: 18763 RVA: 0x0037827E File Offset: 0x0037647E
			internal bool <GetItemForRestore>b__14_1(CodingLogItem x)
			{
				return !string.IsNullOrEmpty(x.OldData);
			}

			// Token: 0x0600494C RID: 18764 RVA: 0x00378290 File Offset: 0x00376490
			internal bool <GetItemForRestore>b__14_2(CodingLogItem x)
			{
				return !string.IsNullOrEmpty(x.NewData) || (string.IsNullOrEmpty(x.NewData) && !string.IsNullOrEmpty(x.OldData) && x.UserFriendlyValue != null && x.UserFriendlyValue.Contains("BACKUP"));
			}

			// Token: 0x0600494D RID: 18765 RVA: 0x00377DB5 File Offset: 0x00375FB5
			internal long <GetGroupReport>b__15_0(CodingLogItem x)
			{
				return x.Timestamp;
			}

			// Token: 0x04002A50 RID: 10832
			public static readonly CodingLogItemGroup.<>c <>9 = new CodingLogItemGroup.<>c();

			// Token: 0x04002A51 RID: 10833
			public static Func<CodingLogItem, long> <>9__14_0;

			// Token: 0x04002A52 RID: 10834
			public static Func<CodingLogItem, bool> <>9__14_1;

			// Token: 0x04002A53 RID: 10835
			public static Func<CodingLogItem, bool> <>9__14_2;

			// Token: 0x04002A54 RID: 10836
			public static Func<CodingLogItem, long> <>9__15_0;
		}
	}
}
