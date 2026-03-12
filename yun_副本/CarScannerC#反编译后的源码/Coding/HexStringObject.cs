using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000879 RID: 2169
	internal class HexStringObject : ObservableCollection<ByteObject>
	{
		// Token: 0x06004A00 RID: 18944 RVA: 0x0037C1C4 File Offset: 0x0037A3C4
		public HexStringObject(byte[] data)
			: base(data.Select((byte x) => new ByteObject(x)))
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].PropertyChanged -= this.Bo_PropertyChanged;
				base[i].PropertyChanged += this.Bo_PropertyChanged;
				base[i].ByteIdx = i;
			}
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0037C24A File Offset: 0x0037A44A
		public HexStringObject(string hex)
			: this(BitHelpers.ConvertHexToBytesX(hex))
		{
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x0037C258 File Offset: 0x0037A458
		private void Bo_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs("Hex"));
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x0037C26A File Offset: 0x0037A46A
		public byte[] GetValue()
		{
			return this.Select((ByteObject bo) => bo.B).ToArray<byte>();
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x0037C298 File Offset: 0x0037A498
		// (set) Token: 0x06004A05 RID: 18949 RVA: 0x0037C310 File Offset: 0x0037A510
		public string Hex
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ByteObject byteObject in this)
				{
					stringBuilder.Append(byteObject.B.ToString("X2") + " ");
				}
				return stringBuilder.ToString().Trim();
			}
			set
			{
				try
				{
					value = value.Trim().Replace(" ", "");
					value = OBDDataReader.FilterHexAndNewLineOnly(value);
					List<byte> list = BitHelpers.ConvertHexToBytesX(value).ToList<byte>();
					if (this.VariableDataLength)
					{
						base.Clear();
						using (List<byte>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								byte b = enumerator.Current;
								base.Add(new ByteObject(b));
							}
							goto IL_009C;
						}
					}
					int num = 0;
					while (num < list.Count && num < base.Count)
					{
						base[num].B = list[num];
						num++;
					}
					IL_009C:;
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0037C3DC File Offset: 0x0037A5DC
		internal void ApplyChanges(string hex_data)
		{
			try
			{
				byte[] array = BitHelpers.ConvertHexToBytesX(OBDDataReader.FilterHexAndNewLineOnly(hex_data).Replace("\r", "").Replace("\n", ""));
				this.ApplyChanges(array);
			}
			catch (Exception)
			{
				this.OnPropertyChanged(new PropertyChangedEventArgs("Hex"));
			}
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0037C440 File Offset: 0x0037A640
		internal void ApplyChanges(byte[] new_data)
		{
			if (this.VariableDataLength)
			{
				base.Clear();
				foreach (byte b in new_data)
				{
					base.Add(new ByteObject(b));
				}
			}
			else
			{
				if (new_data == null)
				{
					return;
				}
				int num = 0;
				while (num < new_data.Length && num < base.Count)
				{
					base[num].B = new_data[num];
					num++;
				}
			}
			this.OnPropertyChanged(new PropertyChangedEventArgs("Hex"));
		}

		// Token: 0x04002AD3 RID: 10963
		public bool VariableDataLength;

		// Token: 0x0200087A RID: 2170
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004A08 RID: 18952 RVA: 0x0037C4B7 File Offset: 0x0037A6B7
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004A09 RID: 18953 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004A0A RID: 18954 RVA: 0x0037C4C3 File Offset: 0x0037A6C3
			internal ByteObject <.ctor>b__0_0(byte x)
			{
				return new ByteObject(x);
			}

			// Token: 0x06004A0B RID: 18955 RVA: 0x0037C4CB File Offset: 0x0037A6CB
			internal byte <GetValue>b__4_0(ByteObject bo)
			{
				return bo.B;
			}

			// Token: 0x04002AD4 RID: 10964
			public static readonly HexStringObject.<>c <>9 = new HexStringObject.<>c();

			// Token: 0x04002AD5 RID: 10965
			public static Func<byte, ByteObject> <>9__0_0;

			// Token: 0x04002AD6 RID: 10966
			public static Func<ByteObject, byte> <>9__4_0;
		}
	}
}
