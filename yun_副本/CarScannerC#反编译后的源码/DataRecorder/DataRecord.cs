using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006D7 RID: 1751
	public class DataRecord : INotifyPropertyChanged
	{
		// Token: 0x06003B85 RID: 15237 RVA: 0x00314FD8 File Offset: 0x003131D8
		public DataRecord()
		{
			this.Elements = new List<DataRecordElement>(2048);
		}

		// Token: 0x170013A9 RID: 5033
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x00314FF0 File Offset: 0x003131F0
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x00314FF8 File Offset: 0x003131F8
		public int PID_Id
		{
			[CompilerGenerated]
			get
			{
				return this.<PID_Id>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PID_Id>k__BackingField = value;
			}
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x00315001 File Offset: 0x00313201
		// (set) Token: 0x06003B89 RID: 15241 RVA: 0x00315009 File Offset: 0x00313209
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06003B8A RID: 15242 RVA: 0x00315012 File Offset: 0x00313212
		// (set) Token: 0x06003B8B RID: 15243 RVA: 0x0031501A File Offset: 0x0031321A
		public string ShortName
		{
			[CompilerGenerated]
			get
			{
				return this.<ShortName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShortName>k__BackingField = value;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06003B8C RID: 15244 RVA: 0x00315023 File Offset: 0x00313223
		// (set) Token: 0x06003B8D RID: 15245 RVA: 0x0031502B File Offset: 0x0031322B
		public List<DataRecordElement> Elements
		{
			[CompilerGenerated]
			get
			{
				return this.<Elements>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Elements>k__BackingField = value;
			}
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06003B8E RID: 15246 RVA: 0x00315034 File Offset: 0x00313234
		// (set) Token: 0x06003B8F RID: 15247 RVA: 0x0031503C File Offset: 0x0031323C
		public UnitsHelper.Units Units
		{
			[CompilerGenerated]
			get
			{
				return this.<Units>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Units>k__BackingField = value;
			}
		}

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06003B90 RID: 15248 RVA: 0x00315048 File Offset: 0x00313248
		// (remove) Token: 0x06003B91 RID: 15249 RVA: 0x00315080 File Offset: 0x00313280
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

		// Token: 0x06003B92 RID: 15250 RVA: 0x003150B8 File Offset: 0x003132B8
		public void AddElement(double Value, double secondsFromStart)
		{
			DataRecordElement dataRecordElement = new DataRecordElement(Value, secondsFromStart);
			this.Elements.Add(dataRecordElement);
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x003150D9 File Offset: 0x003132D9
		// (set) Token: 0x06003B94 RID: 15252 RVA: 0x003150E4 File Offset: 0x003132E4
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			set
			{
				if (value != this._IsVisible)
				{
					this._IsVisible = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged != null)
					{
						propertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
					}
				}
			}
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x0031511C File Offset: 0x0031331C
		public void WriteToStreamDataRecord(JsonTextWriter writer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("PID_Id");
			writer.WriteValue(this.PID_Id);
			writer.WritePropertyName("Name");
			writer.WriteValue(this.Name);
			writer.WritePropertyName("ShortName");
			writer.WriteValue(this.ShortName);
			writer.WritePropertyName("Units");
			writer.WriteValue(this.Units);
			writer.WritePropertyName("Elements");
			writer.WriteStartArray();
			foreach (DataRecordElement dataRecordElement in this.Elements)
			{
				dataRecordElement.WriteToStreamDataElement(writer);
			}
			writer.WriteEndArray();
			writer.WriteEndObject();
		}

		// Token: 0x04002478 RID: 9336
		[CompilerGenerated]
		private int <PID_Id>k__BackingField;

		// Token: 0x04002479 RID: 9337
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x0400247A RID: 9338
		[CompilerGenerated]
		private string <ShortName>k__BackingField;

		// Token: 0x0400247B RID: 9339
		[CompilerGenerated]
		private List<DataRecordElement> <Elements>k__BackingField;

		// Token: 0x0400247C RID: 9340
		[CompilerGenerated]
		private UnitsHelper.Units <Units>k__BackingField;

		// Token: 0x0400247D RID: 9341
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400247E RID: 9342
		private bool _IsVisible;
	}
}
