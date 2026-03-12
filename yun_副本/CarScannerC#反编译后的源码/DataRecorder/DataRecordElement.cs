using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006DB RID: 1755
	public class DataRecordElement
	{
		// Token: 0x06003BC2 RID: 15298 RVA: 0x00002050 File Offset: 0x00000250
		public DataRecordElement()
		{
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x00315BC9 File Offset: 0x00313DC9
		public DataRecordElement(double Value, double secondsFromStart)
		{
			this.Value = Value;
			this.Seconds = secondsFromStart;
		}

		// Token: 0x170013C0 RID: 5056
		// (get) Token: 0x06003BC4 RID: 15300 RVA: 0x00315BDF File Offset: 0x00313DDF
		// (set) Token: 0x06003BC5 RID: 15301 RVA: 0x00315BE7 File Offset: 0x00313DE7
		public Point Position
		{
			[CompilerGenerated]
			get
			{
				return this.<Position>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Position>k__BackingField = value;
			}
		}

		// Token: 0x170013C1 RID: 5057
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x00315BF0 File Offset: 0x00313DF0
		// (set) Token: 0x06003BC7 RID: 15303 RVA: 0x00315BF8 File Offset: 0x00313DF8
		public double Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x00315C01 File Offset: 0x00313E01
		// (set) Token: 0x06003BC9 RID: 15305 RVA: 0x00315C09 File Offset: 0x00313E09
		public double Seconds
		{
			[CompilerGenerated]
			get
			{
				return this.<Seconds>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Seconds>k__BackingField = value;
			}
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00315C12 File Offset: 0x00313E12
		public void WriteToStreamDataElement(JsonTextWriter writer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("Value");
			writer.WriteValue(this.Value);
			writer.WritePropertyName("Seconds");
			writer.WriteValue(this.Seconds);
			writer.WriteEndObject();
		}

		// Token: 0x04002492 RID: 9362
		[CompilerGenerated]
		private Point <Position>k__BackingField;

		// Token: 0x04002493 RID: 9363
		[CompilerGenerated]
		private double <Value>k__BackingField;

		// Token: 0x04002494 RID: 9364
		[CompilerGenerated]
		private double <Seconds>k__BackingField;
	}
}
