using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200026C RID: 620
	public class SettingsV2
	{
		// Token: 0x06001C24 RID: 7204 RVA: 0x00143D30 File Offset: 0x00141F30
		public static void Reset()
		{
			string appDataPath = SettingsV2.GetAppDataPath();
			string text = Path.Combine(appDataPath, "strings");
			string text2 = Path.Combine(appDataPath, "bools");
			string text3 = Path.Combine(appDataPath, "longs");
			string text4 = Path.Combine(appDataPath, "doubles");
			string text5 = Path.Combine(appDataPath, "decimals");
			string text6 = Path.Combine(appDataPath, "ints");
			File.Delete(text);
			File.Delete(text2);
			File.Delete(text3);
			File.Delete(text4);
			File.Delete(text5);
			File.Delete(text6);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x00143DB0 File Offset: 0x00141FB0
		private static string GetAppDataPath()
		{
			string text;
			if (PlatformHelper.IsAndroid)
			{
				text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			else
			{
				if (!PlatformHelper.IsiOS)
				{
					throw new NotImplementedException("SettingsV2.GetAppDataPath PlatformUnknown=" + Device.RuntimePlatform);
				}
				text = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
			}
			return text;
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x00143DFC File Offset: 0x00141FFC
		public SettingsV2()
		{
			string appDataPath = SettingsV2.GetAppDataPath();
			this.fnStrings = Path.Combine(appDataPath, "strings");
			this.fnBools = Path.Combine(appDataPath, "bools");
			this.fnLongs = Path.Combine(appDataPath, "longs");
			this.fnDoubles = Path.Combine(appDataPath, "doubles");
			this.fnDecimals = Path.Combine(appDataPath, "decimals");
			this.fnInts = Path.Combine(appDataPath, "ints");
			this.Load();
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x00143ED0 File Offset: 0x001420D0
		public void PrintKeys()
		{
			foreach (string text in this.Bools.Keys.Concat(this.Ints.Keys).Concat(this.Decimals.Keys).Concat(this.Doubles.Keys)
				.Concat(this.Strings.Keys)
				.Concat(this.Longs.Keys))
			{
			}
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x00143F6C File Offset: 0x0014216C
		public void Load()
		{
			object obj = this.locker;
			lock (obj)
			{
				try
				{
					if (File.Exists(this.fnStrings))
					{
						using (FileStream fileStream = File.Open(this.fnStrings, FileMode.Open))
						{
							using (ShiftStream shiftStream = new ShiftStream(fileStream))
							{
								using (StreamReader streamReader = new StreamReader(shiftStream))
								{
									using (JsonReader jsonReader = new JsonTextReader(streamReader))
									{
										Dictionary<string, string> dictionary = new JsonSerializer().Deserialize<Dictionary<string, string>>(jsonReader);
										this.Strings = dictionary ?? new Dictionary<string, string>();
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					if (File.Exists(this.fnBools))
					{
						using (FileStream fileStream2 = File.Open(this.fnBools, FileMode.Open))
						{
							using (ShiftStream shiftStream2 = new ShiftStream(fileStream2))
							{
								using (StreamReader streamReader2 = new StreamReader(shiftStream2))
								{
									using (JsonReader jsonReader2 = new JsonTextReader(streamReader2))
									{
										Dictionary<string, bool> dictionary2 = new JsonSerializer().Deserialize<Dictionary<string, bool>>(jsonReader2);
										this.Bools = dictionary2 ?? new Dictionary<string, bool>();
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					if (File.Exists(this.fnInts))
					{
						using (FileStream fileStream3 = File.Open(this.fnInts, FileMode.Open))
						{
							using (ShiftStream shiftStream3 = new ShiftStream(fileStream3))
							{
								using (StreamReader streamReader3 = new StreamReader(shiftStream3))
								{
									using (JsonReader jsonReader3 = new JsonTextReader(streamReader3))
									{
										Dictionary<string, int> dictionary3 = new JsonSerializer().Deserialize<Dictionary<string, int>>(jsonReader3);
										this.Ints = dictionary3 ?? new Dictionary<string, int>();
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					if (File.Exists(this.fnLongs))
					{
						using (FileStream fileStream4 = File.Open(this.fnLongs, FileMode.Open))
						{
							using (ShiftStream shiftStream4 = new ShiftStream(fileStream4))
							{
								using (StreamReader streamReader4 = new StreamReader(shiftStream4))
								{
									using (JsonReader jsonReader4 = new JsonTextReader(streamReader4))
									{
										Dictionary<string, long> dictionary4 = new JsonSerializer().Deserialize<Dictionary<string, long>>(jsonReader4);
										this.Longs = dictionary4 ?? new Dictionary<string, long>();
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					if (File.Exists(this.fnDoubles))
					{
						using (FileStream fileStream5 = File.Open(this.fnDoubles, FileMode.Open))
						{
							using (ShiftStream shiftStream5 = new ShiftStream(fileStream5))
							{
								using (StreamReader streamReader5 = new StreamReader(shiftStream5))
								{
									using (JsonReader jsonReader5 = new JsonTextReader(streamReader5))
									{
										Dictionary<string, double> dictionary5 = new JsonSerializer().Deserialize<Dictionary<string, double>>(jsonReader5);
										this.Doubles = dictionary5 ?? new Dictionary<string, double>();
									}
								}
							}
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					if (File.Exists(this.fnDecimals))
					{
						using (FileStream fileStream6 = File.Open(this.fnDecimals, FileMode.Open))
						{
							using (ShiftStream shiftStream6 = new ShiftStream(fileStream6))
							{
								using (StreamReader streamReader6 = new StreamReader(shiftStream6))
								{
									using (JsonReader jsonReader6 = new JsonTextReader(streamReader6))
									{
										Dictionary<string, decimal> dictionary6 = new JsonSerializer().Deserialize<Dictionary<string, decimal>>(jsonReader6);
										this.Decimals = dictionary6 ?? new Dictionary<string, decimal>();
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

		// Token: 0x06001C29 RID: 7209 RVA: 0x001445F8 File Offset: 0x001427F8
		public bool Contains(string key)
		{
			object obj = this.locker;
			bool flag2;
			lock (obj)
			{
				if (this.Strings.ContainsKey(key))
				{
					flag2 = true;
				}
				else if (this.Bools.ContainsKey(key))
				{
					flag2 = true;
				}
				else if (this.Ints.ContainsKey(key))
				{
					flag2 = true;
				}
				else if (this.Longs.ContainsKey(key))
				{
					flag2 = true;
				}
				else if (this.Doubles.ContainsKey(key))
				{
					flag2 = true;
				}
				else if (this.Decimals.ContainsKey(key))
				{
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x001446A4 File Offset: 0x001428A4
		public void Remove(string key)
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.Strings.ContainsKey(key))
				{
					this.Strings.Remove(key);
					this.SaveToFile(TypeCode.String);
				}
				if (this.Bools.ContainsKey(key))
				{
					this.Bools.Remove(key);
					this.SaveToFile(TypeCode.Boolean);
				}
				if (this.Ints.ContainsKey(key))
				{
					this.Ints.Remove(key);
					this.SaveToFile(TypeCode.Int32);
				}
				if (this.Longs.ContainsKey(key))
				{
					this.Longs.Remove(key);
					this.SaveToFile(TypeCode.Int64);
				}
				if (this.Doubles.ContainsKey(key))
				{
					this.Doubles.Remove(key);
					this.SaveToFile(TypeCode.Double);
				}
				if (this.Decimals.ContainsKey(key))
				{
					this.Decimals.Remove(key);
					this.SaveToFile(TypeCode.Decimal);
				}
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x001447B0 File Offset: 0x001429B0
		private void SaveToFile(TypeCode dataType)
		{
			string text = null;
			string text2 = null;
			if (dataType != TypeCode.Boolean)
			{
				switch (dataType)
				{
				case TypeCode.Int32:
					text = JsonConvert.SerializeObject(this.Ints);
					text2 = this.fnInts;
					break;
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				case TypeCode.Single:
					break;
				case TypeCode.Int64:
					text = JsonConvert.SerializeObject(this.Longs);
					text2 = this.fnLongs;
					break;
				case TypeCode.Double:
					text = JsonConvert.SerializeObject(this.Doubles);
					text2 = this.fnDoubles;
					break;
				case TypeCode.Decimal:
					text = JsonConvert.SerializeObject(this.Decimals);
					text2 = this.fnDecimals;
					break;
				default:
					if (dataType == TypeCode.String)
					{
						text = JsonConvert.SerializeObject(this.Strings);
						text2 = this.fnStrings;
					}
					break;
				}
			}
			else
			{
				text = JsonConvert.SerializeObject(this.Bools);
				text2 = this.fnBools;
			}
			try
			{
				using (FileStream fileStream = File.Open(text2, FileMode.OpenOrCreate))
				{
					using (ShiftStream shiftStream = new ShiftStream(fileStream))
					{
						using (StreamWriter streamWriter = new StreamWriter(shiftStream))
						{
							streamWriter.Write(text);
							streamWriter.Flush();
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x001448F4 File Offset: 0x00142AF4
		public void AddOrUpdateValue(string key, string value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Strings[key] = value;
				this.SaveToFile(TypeCode.String);
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00144944 File Offset: 0x00142B44
		public void AddOrUpdateValue(string key, int value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Ints[key] = value;
				this.SaveToFile(TypeCode.Int32);
			}
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00144994 File Offset: 0x00142B94
		public void AddOrUpdateValue(string key, bool value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Bools[key] = value;
				this.SaveToFile(TypeCode.Boolean);
			}
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x001449E4 File Offset: 0x00142BE4
		public void AddOrUpdateValue(string key, decimal value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Decimals[key] = value;
				this.SaveToFile(TypeCode.Decimal);
			}
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x00144A34 File Offset: 0x00142C34
		public void AddOrUpdateValue(string key, double value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Doubles[key] = value;
				this.SaveToFile(TypeCode.Double);
			}
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x00144A84 File Offset: 0x00142C84
		public void AddOrUpdateValue(string key, long value, string file = null)
		{
			object obj = this.locker;
			lock (obj)
			{
				this.Longs[key] = value;
				this.SaveToFile(TypeCode.Int64);
			}
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x00144AD4 File Offset: 0x00142CD4
		public T GetValueOrDefault<T>(string key, T def = default(T), string file = null)
		{
			object obj = this.locker;
			T t;
			lock (obj)
			{
				Type type = typeof(T);
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type = Nullable.GetUnderlyingType(type);
				}
				TypeCode typeCode = Type.GetTypeCode(type);
				if (typeCode != TypeCode.Boolean)
				{
					switch (typeCode)
					{
					case TypeCode.Int32:
					{
						int num;
						if (this.Ints.TryGetValue(key, out num))
						{
							return (T)((object)Convert.ChangeType(num, typeof(T)));
						}
						return def;
					}
					case TypeCode.UInt32:
					case TypeCode.UInt64:
					case TypeCode.Single:
						break;
					case TypeCode.Int64:
					{
						long num2;
						if (this.Longs.TryGetValue(key, out num2))
						{
							return (T)((object)Convert.ChangeType(num2, typeof(T)));
						}
						return def;
					}
					case TypeCode.Double:
					{
						double num3;
						if (this.Doubles.TryGetValue(key, out num3))
						{
							return (T)((object)Convert.ChangeType(num3, typeof(T)));
						}
						return def;
					}
					case TypeCode.Decimal:
					{
						decimal num4;
						if (this.Decimals.TryGetValue(key, out num4))
						{
							return (T)((object)Convert.ChangeType(num4, typeof(T)));
						}
						return def;
					}
					default:
						if (typeCode == TypeCode.String)
						{
							string text;
							if (this.Strings.TryGetValue(key, out text))
							{
								return (T)((object)Convert.ChangeType(text, typeof(T)));
							}
							return def;
						}
						break;
					}
					throw new ArgumentException("Type " + typeof(T).ToString() + " not supported");
				}
				bool flag2;
				if (this.Bools.TryGetValue(key, out flag2))
				{
					t = (T)((object)Convert.ChangeType(flag2, typeof(T)));
				}
				else
				{
					t = def;
				}
			}
			return t;
		}

		// Token: 0x04000D75 RID: 3445
		private string fnStrings;

		// Token: 0x04000D76 RID: 3446
		private string fnBools;

		// Token: 0x04000D77 RID: 3447
		private string fnLongs;

		// Token: 0x04000D78 RID: 3448
		private string fnDoubles;

		// Token: 0x04000D79 RID: 3449
		private string fnDecimals;

		// Token: 0x04000D7A RID: 3450
		private string fnInts;

		// Token: 0x04000D7B RID: 3451
		private readonly object locker = new object();

		// Token: 0x04000D7C RID: 3452
		private Dictionary<string, string> Strings = new Dictionary<string, string>();

		// Token: 0x04000D7D RID: 3453
		private Dictionary<string, int> Ints = new Dictionary<string, int>();

		// Token: 0x04000D7E RID: 3454
		private Dictionary<string, bool> Bools = new Dictionary<string, bool>();

		// Token: 0x04000D7F RID: 3455
		private Dictionary<string, double> Doubles = new Dictionary<string, double>();

		// Token: 0x04000D80 RID: 3456
		private Dictionary<string, decimal> Decimals = new Dictionary<string, decimal>();

		// Token: 0x04000D81 RID: 3457
		private Dictionary<string, long> Longs = new Dictionary<string, long>();
	}
}
