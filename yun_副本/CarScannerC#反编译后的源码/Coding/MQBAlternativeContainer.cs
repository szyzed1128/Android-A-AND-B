using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200089B RID: 2203
	internal class MQBAlternativeContainer : INotifyPropertyChanged
	{
		// Token: 0x06004B06 RID: 19206 RVA: 0x00381CBE File Offset: 0x0037FEBE
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x00381CD8 File Offset: 0x0037FED8
		public MQBAlternativeContainer(string Address, string Password, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
		{
			this.Address = Address;
			this.Password = Password;
			this.ChangeDataDelegate = ChangeDataDelegate;
			this.GetCurrentStateDelegate = GetCurrentStateDelegate;
			if (this.GetCurrentStateDelegate == null)
			{
				this.GetCurrentStateDelegate = new Func<byte[], MQBAlternativeCoding, string>(this.DefaultGetStateDelegate);
			}
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x00381D44 File Offset: 0x0037FF44
		public MQBAlternativeContainer(string Address, Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate, Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate)
		{
			this.Address = Address;
			this.ChangeDataDelegate = ChangeDataDelegate;
			this.GetCurrentStateDelegate = GetCurrentStateDelegate;
			if (this.GetCurrentStateDelegate == null)
			{
				this.GetCurrentStateDelegate = new Func<byte[], MQBAlternativeCoding, string>(this.DefaultGetStateDelegate);
			}
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x00381DA8 File Offset: 0x0037FFA8
		protected string DefaultGetStateDelegate(byte[] data, MQBAlternativeCoding coding)
		{
			if (coding.IsMIB3() && coding.RequestHeader == "773" && string.IsNullOrEmpty(coding.Password) && string.IsNullOrEmpty(this.Password))
			{
				this.Password = "20103";
				coding.Password = "20103";
			}
			foreach (MQBAdaptationOption mqbadaptationOption in coding.Options)
			{
				if (ArrayHelpers.ArrayEquals<byte>(this.ChangeDataDelegate(data, mqbadaptationOption.Value, coding), data))
				{
					return mqbadaptationOption.Title;
				}
			}
			return MQBAdaptationTemplate.DisableOption.Title;
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06004B0A RID: 19210 RVA: 0x00381E68 File Offset: 0x00380068
		// (set) Token: 0x06004B0B RID: 19211 RVA: 0x00381E70 File Offset: 0x00380070
		protected bool InputType
		{
			[CompilerGenerated]
			get
			{
				return this.<InputType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InputType>k__BackingField = value;
			}
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x00381E79 File Offset: 0x00380079
		protected string GetCurrentStateForInputValue(byte[] data, MQBAlternativeCoding coding)
		{
			return this.ConvertRawDataToInputValue(data);
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x00381E84 File Offset: 0x00380084
		protected byte[] ChangeDataForInputValue(byte[] data, string value, MQBAlternativeCoding coding)
		{
			byte[] array = this.ConvertInputValueToByteArray(value);
			if (array.Length == 0)
			{
				throw new ArgumentException("wrong format", "value");
			}
			byte[] array2 = new byte[data.Length];
			try
			{
				Array.Copy(data, 0, array2, 0, array2.Length);
				Array.Copy(array, 0, array2, this.StartByteId, this.DataLength);
			}
			catch (Exception)
			{
				throw new ArgumentException("wrong format", "data");
			}
			return array2;
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06004B0E RID: 19214 RVA: 0x00381EFC File Offset: 0x003800FC
		// (set) Token: 0x06004B0F RID: 19215 RVA: 0x00381F04 File Offset: 0x00380104
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

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06004B10 RID: 19216 RVA: 0x00381F0D File Offset: 0x0038010D
		// (set) Token: 0x06004B11 RID: 19217 RVA: 0x00381F15 File Offset: 0x00380115
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				this.OnPropertyChanged("Password");
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x06004B12 RID: 19218 RVA: 0x00381F29 File Offset: 0x00380129
		// (set) Token: 0x06004B13 RID: 19219 RVA: 0x00381F31 File Offset: 0x00380131
		public Func<byte[], string, MQBAlternativeCoding, byte[]> ChangeDataDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<ChangeDataDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChangeDataDelegate>k__BackingField = value;
			}
		}

		// Token: 0x170016EF RID: 5871
		// (get) Token: 0x06004B14 RID: 19220 RVA: 0x00381F3A File Offset: 0x0038013A
		// (set) Token: 0x06004B15 RID: 19221 RVA: 0x00381F42 File Offset: 0x00380142
		public Func<byte[], MQBAlternativeCoding, string> GetCurrentStateDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<GetCurrentStateDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetCurrentStateDelegate>k__BackingField = value;
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06004B16 RID: 19222 RVA: 0x00381F4C File Offset: 0x0038014C
		// (remove) Token: 0x06004B17 RID: 19223 RVA: 0x00381F84 File Offset: 0x00380184
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

		// Token: 0x06004B18 RID: 19224 RVA: 0x00381FBC File Offset: 0x003801BC
		public virtual async Task<bool> CheckIsSupported(string requestHeader, string responseHeader, MQBAlternativeCoding coding)
		{
			Tuple<byte[], CodingRequestResult> tuple = await new MQBAdaptationTemplate
			{
				Address = this.Address,
				RequestHeader = requestHeader,
				ResponseHeader = responseHeader,
				Password = "",
				ValueType = AdaptationValueTypes.InputHexDataType
			}.GetCurrentStateRawData("");
			bool flag;
			if (tuple.Item2 == CodingRequestResult.Success && tuple.Item1 != null && tuple.Item1.Length != 0)
			{
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x00382010 File Offset: 0x00380210
		public MQBAlternativeContainer(string Address, string Password, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned = false, bool ReversedByteSet = false)
		{
			this.Password = Password;
			this.Address = Address;
			this.DataLength = DataLength;
			this.StartByteId = StartByteId;
			this.Multiplier = Multiplier;
			this.Offset = Offset;
			this.IsSigned = IsSigned;
			this.ReversedByteSet = ReversedByteSet;
			this.InputType = true;
			this.GetCurrentStateDelegate = new Func<byte[], MQBAlternativeCoding, string>(this.GetCurrentStateForInputValue);
			this.ChangeDataDelegate = new Func<byte[], string, MQBAlternativeCoding, byte[]>(this.ChangeDataForInputValue);
		}

		// Token: 0x170016F0 RID: 5872
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x003820AC File Offset: 0x003802AC
		// (set) Token: 0x06004B1B RID: 19227 RVA: 0x003820B4 File Offset: 0x003802B4
		public int DataLength
		{
			get
			{
				return this._DataLength;
			}
			set
			{
				this._DataLength = value;
				this.OnPropertyChanged("DataLength");
			}
		}

		// Token: 0x170016F1 RID: 5873
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x003820C8 File Offset: 0x003802C8
		// (set) Token: 0x06004B1D RID: 19229 RVA: 0x003820D0 File Offset: 0x003802D0
		public double Multiplier
		{
			get
			{
				return this._Multiplier;
			}
			set
			{
				this._Multiplier = value;
				this.OnPropertyChanged("Multiplier");
			}
		}

		// Token: 0x170016F2 RID: 5874
		// (get) Token: 0x06004B1E RID: 19230 RVA: 0x003820E4 File Offset: 0x003802E4
		// (set) Token: 0x06004B1F RID: 19231 RVA: 0x003820EC File Offset: 0x003802EC
		public bool ReversedByteSet
		{
			get
			{
				return this._ReversedByteSet;
			}
			set
			{
				this._ReversedByteSet = value;
				this.OnPropertyChanged("ReversedByteSet");
			}
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06004B20 RID: 19232 RVA: 0x00382100 File Offset: 0x00380300
		// (set) Token: 0x06004B21 RID: 19233 RVA: 0x00382108 File Offset: 0x00380308
		public double Offset
		{
			get
			{
				return this._Offset;
			}
			set
			{
				this._Offset = value;
				this.OnPropertyChanged("Offset");
			}
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06004B22 RID: 19234 RVA: 0x0038211C File Offset: 0x0038031C
		// (set) Token: 0x06004B23 RID: 19235 RVA: 0x00382124 File Offset: 0x00380324
		public int StartByteId
		{
			get
			{
				return this._StartByteId;
			}
			set
			{
				this._StartByteId = value;
				this.OnPropertyChanged("StartByteId");
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06004B24 RID: 19236 RVA: 0x00382138 File Offset: 0x00380338
		// (set) Token: 0x06004B25 RID: 19237 RVA: 0x00382140 File Offset: 0x00380340
		public bool IsSigned
		{
			get
			{
				return this._IsSigned;
			}
			set
			{
				this._IsSigned = value;
				this.OnPropertyChanged("IsSigned");
			}
		}

		// Token: 0x06004B26 RID: 19238 RVA: 0x00382154 File Offset: 0x00380354
		protected byte[] ConvertInputValueToByteArray(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return new byte[0];
			}
			double num;
			if (!double.TryParse(input.Trim().Replace(" ", "").Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out num))
			{
				return new byte[0];
			}
			byte[] bytes = BitConverter.GetBytes((int)((num - this.Offset) / this.Multiplier));
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse<byte>(bytes);
			}
			if (this.DataLength == 4)
			{
				return bytes;
			}
			byte[] array = new byte[this.DataLength];
			Array.Copy(bytes, 4 - this.DataLength, array, 0, array.Length);
			return array;
		}

		// Token: 0x06004B27 RID: 19239 RVA: 0x00382204 File Offset: 0x00380404
		protected string ConvertRawDataToInputValue(byte[] data)
		{
			if (data.Length < this.StartByteId + this.DataLength)
			{
				return MQBAdaptationTemplate.UNSUPPORTED_TITLE;
			}
			double num = double.NaN;
			int num2 = this.DataLength;
			if (num2 == 0)
			{
				num2 = data.Length - this.StartByteId;
			}
			if (num2 > 4)
			{
				num2 = 4;
			}
			if (this.IsSigned)
			{
				switch (num2)
				{
				case 1:
					num = (double)((sbyte)data[this.StartByteId]);
					break;
				case 2:
					num = (double)((short)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]));
					break;
				case 3:
					num = (double)((int)((sbyte)data[this.StartByteId]) * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					int startByteId = this.StartByteId;
					num = (double)(((int)data[startByteId++] << 24) | ((int)data[startByteId++] << 16) | ((int)data[startByteId++] << 8) | (int)data[startByteId++]);
					break;
				}
				}
			}
			else
			{
				switch (num2)
				{
				case 1:
					num = (double)data[this.StartByteId];
					break;
				case 2:
					num = (double)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]);
					break;
				case 3:
					num = (double)((int)data[this.StartByteId] * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					byte[] array = new byte[4];
					Array.Copy(data, this.StartByteId, array, 0, 4);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					num = BitConverter.ToUInt32(array, 0);
					break;
				}
				default:
					num = (double)((int)data[this.StartByteId] * 256 * 256 * 256 + (int)data[this.StartByteId + 1] * 256 * 256 + (int)data[this.StartByteId + 2] * 256 + (int)data[this.StartByteId + 3]);
					break;
				}
			}
			num *= this.Multiplier;
			return (num + this.Offset).ToString("0.####");
		}

		// Token: 0x04002BCD RID: 11213
		[CompilerGenerated]
		private bool <InputType>k__BackingField;

		// Token: 0x04002BCE RID: 11214
		[CompilerGenerated]
		private string <Address>k__BackingField;

		// Token: 0x04002BCF RID: 11215
		private string _Password = "";

		// Token: 0x04002BD0 RID: 11216
		[CompilerGenerated]
		private Func<byte[], string, MQBAlternativeCoding, byte[]> <ChangeDataDelegate>k__BackingField;

		// Token: 0x04002BD1 RID: 11217
		[CompilerGenerated]
		private Func<byte[], MQBAlternativeCoding, string> <GetCurrentStateDelegate>k__BackingField;

		// Token: 0x04002BD2 RID: 11218
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002BD3 RID: 11219
		private int _DataLength = 1;

		// Token: 0x04002BD4 RID: 11220
		private double _Multiplier = 1.0;

		// Token: 0x04002BD5 RID: 11221
		private bool _ReversedByteSet;

		// Token: 0x04002BD6 RID: 11222
		private double _Offset;

		// Token: 0x04002BD7 RID: 11223
		private int _StartByteId;

		// Token: 0x04002BD8 RID: 11224
		private bool _IsSigned;

		// Token: 0x0200089C RID: 2204
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsSupported>d__29 : IAsyncStateMachine
		{
			// Token: 0x06004B28 RID: 19240 RVA: 0x00382424 File Offset: 0x00380624
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBAlternativeContainer mqbalternativeContainer = this;
				bool flag;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = new MQBAdaptationTemplate
						{
							Address = mqbalternativeContainer.Address,
							RequestHeader = requestHeader,
							ResponseHeader = responseHeader,
							Password = "",
							ValueType = AdaptationValueTypes.InputHexDataType
						}.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBAlternativeContainer.<CheckIsSupported>d__29>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					if (result.Item2 == CodingRequestResult.Success && result.Item1 != null && result.Item1.Length != 0)
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06004B29 RID: 19241 RVA: 0x0038253C File Offset: 0x0038073C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BD9 RID: 11225
			public int <>1__state;

			// Token: 0x04002BDA RID: 11226
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04002BDB RID: 11227
			public MQBAlternativeContainer <>4__this;

			// Token: 0x04002BDC RID: 11228
			public string requestHeader;

			// Token: 0x04002BDD RID: 11229
			public string responseHeader;

			// Token: 0x04002BDE RID: 11230
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
