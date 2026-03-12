using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x020004A6 RID: 1190
	internal static class ScanXChecker
	{
		// Token: 0x06002F94 RID: 12180 RVA: 0x00211E54 File Offset: 0x00210054
		internal static async Task CheckAtDeviceSelection(string device_id, string device_name)
		{
			if (!SharedSettings.Current.AdsProductPurchased || SharedSettings.Current.WhitelistDeviceActivated)
			{
				if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
				{
					SharedSettings.Current.WhitelistDeviceActivated = false;
					SharedSettings.Current.WhitelistDeviceSN = "";
				}
				else if (!(device_name != "CAR2LS ScanX"))
				{
					ValueTuple<bool, string, string, string> valueTuple = await ScanXChecker.CheckOffline(device_id);
					string sn = valueTuple.Item2;
					string item = valueTuple.Item3;
					string item2 = valueTuple.Item4;
					if (!string.IsNullOrEmpty(sn) && !string.IsNullOrEmpty(item) && !string.IsNullOrEmpty(item2))
					{
						TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter = ScanXChecker.CheckOnline(sn, item, item2).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ScanXChecker.OnlineResult>);
						}
						if (taskAwaiter.GetResult() == ScanXChecker.OnlineResult.Valid)
						{
							SharedSettings.Current.WhitelistDeviceActivated = true;
							SharedSettings.Current.WhitelistDeviceSN = sn;
							SharedSettings.Current.AdsProductPurchased = true;
							ScanXChecker.UpdateInterface();
						}
					}
				}
			}
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00211EA0 File Offset: 0x002100A0
		private static void UpdateInterface()
		{
			SimpleMainPage instance = SimpleMainPage.Instance;
			if (instance != null)
			{
				instance.UpdateMainButtons();
			}
			if (App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				try
				{
					LiveDataPIDModel.SetShouldHideForSelectedPids();
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00211EEC File Offset: 0x002100EC
		internal static async Task CheckDeviceAtConnection(string device_id)
		{
			if (!SharedSettings.Current.AdsProductPurchased || SharedSettings.Current.WhitelistDeviceActivated)
			{
				Guid guid;
				if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
				{
					SharedSettings.Current.WhitelistDeviceActivated = false;
					SharedSettings.Current.WhitelistDeviceSN = "";
				}
				else if (!Guid.TryParse(device_id, out guid))
				{
					if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated)
					{
						SharedSettings.Current.AdsProductPurchased = false;
						SharedSettings.Current.WhitelistDeviceActivated = false;
						SharedSettings.Current.WhitelistDeviceSN = "";
						ScanXChecker.UpdateInterface();
					}
				}
				else
				{
					string text = "";
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
					{
						text = SharedSettings.Current.BTDeviceName;
					}
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE)
					{
						text = SharedSettings.Current.BTLEDeviceName;
					}
					if (text == "CAR2LS ScanX" && !SharedSettings.Current.AdsProductPurchased)
					{
						await ScanXChecker.CheckAtDeviceSelection(device_id, text);
					}
					else if (SharedSettings.Current.WhitelistDeviceActivated)
					{
						if (text != "CAR2LS ScanX")
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.WhitelistDeviceSN = "";
							SharedSettings.Current.AdsProductPurchased = false;
							ScanXChecker.UpdateInterface();
						}
						ValueTuple<bool, string, string, string> valueTuple = await ScanXChecker.CheckOffline(device_id);
						bool item = valueTuple.Item1;
						string sn = valueTuple.Item2;
						string item2 = valueTuple.Item3;
						string item3 = valueTuple.Item4;
						if (item)
						{
							long dtTicks = DateTimeNowHelper.NowSafe.Ticks;
							if (new TimeSpan(dtTicks - SharedSettings.Current.LastTimeLicenceChecked) >= SharedSettings.Current.LicenseCheckPeriod || SharedSettings.Current.LastTimeLicenceChecked > dtTicks)
							{
								ScanXChecker.OnlineResult onlineResult = await ScanXChecker.CheckOnline(sn, item2, item3);
								if (onlineResult != ScanXChecker.OnlineResult.ConnectionFail)
								{
									if (onlineResult == ScanXChecker.OnlineResult.Valid)
									{
										SharedSettings.Current.LastTimeLicenceChecked = dtTicks;
									}
									else
									{
										SharedSettings.Current.WhitelistDeviceActivated = false;
										SharedSettings.Current.WhitelistDeviceSN = "";
										SharedSettings.Current.AdsProductPurchased = false;
										ScanXChecker.UpdateInterface();
									}
								}
							}
						}
						else if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(item2) || string.IsNullOrEmpty(item3))
						{
							if (SharedSettings.Current.WhitelistDeviceActivated)
							{
								SharedSettings.Current.WhitelistDeviceActivated = false;
								SharedSettings.Current.WhitelistDeviceSN = "";
								SharedSettings.Current.AdsProductPurchased = false;
								ScanXChecker.UpdateInterface();
							}
						}
						else
						{
							TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter = ScanXChecker.CheckOnline(sn, item2, item3).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<ScanXChecker.OnlineResult>);
							}
							if (taskAwaiter.GetResult() == ScanXChecker.OnlineResult.Valid)
							{
								SharedSettings.Current.WhitelistDeviceActivated = true;
								SharedSettings.Current.WhitelistDeviceSN = sn;
								SharedSettings.Current.AdsProductPurchased = true;
								ScanXChecker.UpdateInterface();
							}
							else
							{
								SharedSettings.Current.WhitelistDeviceActivated = false;
								SharedSettings.Current.WhitelistDeviceSN = "";
								SharedSettings.Current.AdsProductPurchased = false;
								ScanXChecker.UpdateInterface();
							}
						}
					}
				}
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x00211F30 File Offset: 0x00210130
		private static async Task<ValueTuple<bool, string, string, string>> CheckOffline(string device_id)
		{
			ValueTuple<bool, string, string, string> valueTuple;
			if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
			{
				valueTuple = new ValueTuple<bool, string, string, string>(false, "", "", "");
			}
			else
			{
				new BTLEDeviceSelectorViewModel();
				ValueTuple<string, string> valueTuple2 = await ScanXChecker.ReadSNFromRokodil(device_id);
				string item = valueTuple2.Item1;
				string item2 = valueTuple2.Item2;
				if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(item2) || !item.Contains(" SN"))
				{
					SharedSettings.Current.WhitelistDeviceSN = "";
					if (SharedSettings.Current.WhitelistDeviceActivated)
					{
						SharedSettings.Current.WhitelistDeviceActivated = false;
						SharedSettings.Current.AdsProductPurchased = false;
						ScanXChecker.UpdateInterface();
					}
				}
				string text = item.Substring(item.IndexOf(" SN") + 3);
				string text2 = item.Substring(0, item.IndexOf(" "));
				if (SharedSettings.Current.WhitelistDeviceSN == text)
				{
					valueTuple = new ValueTuple<bool, string, string, string>(true, text, item2, text2);
				}
				else
				{
					valueTuple = new ValueTuple<bool, string, string, string>(false, text, item2, text2);
				}
			}
			return valueTuple;
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00211F74 File Offset: 0x00210174
		public static async Task<ValueTuple<string, string>> ReadSNFromRokodil(string device_guid)
		{
			Guid guid = Guid.Parse("(0000fee0-0000-1000-8000-00805f9b34fb)");
			Guid guid2 = Guid.Parse("(0000fee2-0000-1000-8000-00805f9b34fb)");
			Guid guid3 = Guid.Parse("(0000fee1-0000-1000-8000-00805f9b34fb)");
			BTLEConnection connection = new BTLEConnection(guid, guid2, guid3);
			TaskAwaiter<bool> taskAwaiter = connection.ConnectAsync(device_guid, PCLDebugStream.CurrentInstance).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			ValueTuple<string, string> valueTuple;
			if (taskAwaiter.GetResult())
			{
				int seed = new Random().Next(1000, 9999);
				await connection.WriteBytesAsync(Encoding.ASCII.GetBytes("ATI" + seed.ToString("0000")));
				await Task.Delay(1000);
				byte[] array = await connection.ReadBytesAsync();
				string @string = Encoding.ASCII.GetString(array);
				connection.Disconect();
				valueTuple = new ValueTuple<string, string>(@string, seed.ToString("0000"));
			}
			else
			{
				valueTuple = new ValueTuple<string, string>("", "");
			}
			return valueTuple;
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00211FB7 File Offset: 0x002101B7
		private static string sha(string str)
		{
			return ScanXChecker.sha1(str + ScanXChecker.sha1(str));
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x00211FCC File Offset: 0x002101CC
		private static string sha1(string input)
		{
			string text;
			using (SHA1Managed sha1Managed = new SHA1Managed())
			{
				byte[] array = sha1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
				foreach (byte b in array)
				{
					stringBuilder.Append(b.ToString("x2"));
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00212048 File Offset: 0x00210248
		private static string GetSignature(IDictionary<string, string> formData, string secretKey = "fndsiufne302fn2389vb*IVN#()gn902vn28vn20v02V10V0VN2V2=1==")
		{
			string[] array = (from x in formData
				where x.Value != ""
				where x.Key != "signature"
				where x.Value != null
				orderby x.Key
				select x.Value).ToArray<string>();
			string text = string.Join("", array.Select((string x) => x)) + secretKey;
			Console.WriteLine(text);
			return ScanXChecker.sha(ScanXChecker.sha(text));
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00212155 File Offset: 0x00210355
		private static string GetDeviceId()
		{
			return ScanXChecker.sha(Hardware.DeviceID + DeviceInfo.Manufacturer + DeviceInfo.Model);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00212170 File Offset: 0x00210370
		private static Dictionary<string, string> BuildRequest(string sn, string seed, string key)
		{
			Random random = new Random();
			byte[] array = new byte[12];
			random.NextBytes(array);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["sn"] = sn;
			dictionary["seed"] = seed;
			dictionary["key"] = key;
			dictionary["deviceId"] = ScanXChecker.GetDeviceId();
			dictionary["salt"] = BitHelpers.ByteArrayToHexString(array);
			string signature = ScanXChecker.GetSignature(dictionary, "fndsiufne302fn2389vb*IVN#()gn902vn28vn20v02V10V0VN2V2=1==");
			dictionary["signature"] = signature;
			return dictionary;
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x002121F4 File Offset: 0x002103F4
		public static async void Test()
		{
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00212224 File Offset: 0x00210424
		private static int hashFnv32a(string str, int seed)
		{
			int num = seed;
			for (int i = 0; i < str.Length; i++)
			{
				num = (num + (int)str[i]) ^ seed;
				if (i % 2 == 0)
				{
					num ^= i;
				}
				else
				{
					num -= i;
				}
			}
			return Math.Abs(num);
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x00212268 File Offset: 0x00210468
		private static int generateCheckSummForKeyAndDeviceID(string s_key, string s_devid, string s_salt)
		{
			DateTime lastUtcTime = OnlineTime.LastUtcTime;
			int num = lastUtcTime.Day * lastUtcTime.Month * (int)lastUtcTime.DayOfWeek;
			int num2 = ScanXChecker.hashFnv32a(s_key, num);
			int num3 = ScanXChecker.hashFnv32a(s_devid, num);
			int num4 = ScanXChecker.hashFnv32a(s_salt, num);
			return num2 ^ num3 ^ num4;
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x002122B0 File Offset: 0x002104B0
		private static bool CheckResponse(Dictionary<string, string> request, Dictionary<string, string> response)
		{
			return response.ContainsKey("sn") && response.ContainsKey("deviceId") && response.ContainsKey("sn") && response.ContainsKey("isValid") && response.ContainsKey("key") && response.ContainsKey("message") && response.ContainsKey("salt") && response.ContainsKey("secret") && response.ContainsKey("seed") && response.ContainsKey("signature") && !(ScanXChecker.generateCheckSummForKeyAndDeviceID(request["sn"] + request["seed"] + request["key"], request["deviceId"], response["salt"]).ToString(CultureInfo.InvariantCulture) != response["secret"]) && !(ScanXChecker.GetSignature(response, "fndsiufne302fn2389vb*IVN#()gn902vn28vn20v02V10V0VN2V2=1==") != response["signature"]);
		}

		// Token: 0x06002FA2 RID: 12194 RVA: 0x002123C4 File Offset: 0x002105C4
		private static async Task<ScanXChecker.OnlineResult> CheckOnline(string sn, string seed, string key)
		{
			ScanXChecker.OnlineResult onlineResult;
			if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
			{
				onlineResult = ScanXChecker.OnlineResult.NotValid;
			}
			else
			{
				Dictionary<string, string> requestDict = ScanXChecker.BuildRequest(sn, seed, key);
				HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestDict), Encoding.UTF8);
				string text = await HttpDownloader.Post("https://node4.carscanner.info/scanx", httpContent, 10);
				if (string.IsNullOrEmpty(text))
				{
					onlineResult = ScanXChecker.OnlineResult.ConnectionFail;
				}
				else
				{
					try
					{
						Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
						if (dictionary != null && ScanXChecker.CheckResponse(requestDict, dictionary) && dictionary["isValid"] == "true" && requestDict["sn"] == dictionary["sn"] && requestDict["seed"] == dictionary["seed"] && requestDict["key"] == dictionary["key"])
						{
							return ScanXChecker.OnlineResult.Valid;
						}
					}
					catch (Exception)
					{
					}
					onlineResult = ScanXChecker.OnlineResult.NotValid;
				}
			}
			return onlineResult;
		}

		// Token: 0x04001B8F RID: 7055
		public const string VALIDNAME = "CAR2LS ScanX";

		// Token: 0x020004A7 RID: 1191
		private enum OnlineResult
		{
			// Token: 0x04001B91 RID: 7057
			Valid,
			// Token: 0x04001B92 RID: 7058
			NotValid,
			// Token: 0x04001B93 RID: 7059
			ConnectionFail
		}

		// Token: 0x020004A8 RID: 1192
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002FA3 RID: 12195 RVA: 0x00212417 File Offset: 0x00210617
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002FA4 RID: 12196 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002FA5 RID: 12197 RVA: 0x00212423 File Offset: 0x00210623
			internal bool <GetSignature>b__8_0(KeyValuePair<string, string> x)
			{
				return x.Value != "";
			}

			// Token: 0x06002FA6 RID: 12198 RVA: 0x00212436 File Offset: 0x00210636
			internal bool <GetSignature>b__8_1(KeyValuePair<string, string> x)
			{
				return x.Key != "signature";
			}

			// Token: 0x06002FA7 RID: 12199 RVA: 0x00212449 File Offset: 0x00210649
			internal bool <GetSignature>b__8_2(KeyValuePair<string, string> x)
			{
				return x.Value != null;
			}

			// Token: 0x06002FA8 RID: 12200 RVA: 0x00212455 File Offset: 0x00210655
			internal string <GetSignature>b__8_3(KeyValuePair<string, string> x)
			{
				return x.Key;
			}

			// Token: 0x06002FA9 RID: 12201 RVA: 0x0021245E File Offset: 0x0021065E
			internal string <GetSignature>b__8_4(KeyValuePair<string, string> x)
			{
				return x.Value;
			}

			// Token: 0x06002FAA RID: 12202 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <GetSignature>b__8_5(string x)
			{
				return x;
			}

			// Token: 0x04001B94 RID: 7060
			public static readonly ScanXChecker.<>c <>9 = new ScanXChecker.<>c();

			// Token: 0x04001B95 RID: 7061
			public static Func<KeyValuePair<string, string>, bool> <>9__8_0;

			// Token: 0x04001B96 RID: 7062
			public static Func<KeyValuePair<string, string>, bool> <>9__8_1;

			// Token: 0x04001B97 RID: 7063
			public static Func<KeyValuePair<string, string>, bool> <>9__8_2;

			// Token: 0x04001B98 RID: 7064
			public static Func<KeyValuePair<string, string>, string> <>9__8_3;

			// Token: 0x04001B99 RID: 7065
			public static Func<KeyValuePair<string, string>, string> <>9__8_4;

			// Token: 0x04001B9A RID: 7066
			public static Func<string, string> <>9__8_5;
		}

		// Token: 0x020004A9 RID: 1193
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckAtDeviceSelection>d__1 : IAsyncStateMachine
		{
			// Token: 0x06002FAB RID: 12203 RVA: 0x00212468 File Offset: 0x00210668
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter3;
					TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter4;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<ScanXChecker.OnlineResult>);
							num2 = -1;
							goto IL_017D;
						}
						if (SharedSettings.Current.AdsProductPurchased && !SharedSettings.Current.WhitelistDeviceActivated)
						{
							goto IL_01D3;
						}
						if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.WhitelistDeviceSN = "";
							goto IL_01D3;
						}
						if (device_name != "CAR2LS ScanX")
						{
							goto IL_01D3;
						}
						taskAwaiter4 = ScanXChecker.CheckOffline(device_id).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string, string, string>>, ScanXChecker.<CheckAtDeviceSelection>d__1>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<ValueTuple<bool, string, string, string>>);
						num2 = -1;
					}
					ValueTuple<bool, string, string, string> result = taskAwaiter4.GetResult();
					sn = result.Item2;
					string item = result.Item3;
					string item2 = result.Item4;
					if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(item) || string.IsNullOrEmpty(item2))
					{
						goto IL_01D3;
					}
					taskAwaiter3 = ScanXChecker.CheckOnline(sn, item, item2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ScanXChecker.OnlineResult>, ScanXChecker.<CheckAtDeviceSelection>d__1>(ref taskAwaiter3, ref this);
						return;
					}
					IL_017D:
					if (taskAwaiter3.GetResult() == ScanXChecker.OnlineResult.Valid)
					{
						SharedSettings.Current.WhitelistDeviceActivated = true;
						SharedSettings.Current.WhitelistDeviceSN = sn;
						SharedSettings.Current.AdsProductPurchased = true;
						ScanXChecker.UpdateInterface();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					sn = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01D3:
				num2 = -2;
				sn = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002FAC RID: 12204 RVA: 0x00212680 File Offset: 0x00210880
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B9B RID: 7067
			public int <>1__state;

			// Token: 0x04001B9C RID: 7068
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B9D RID: 7069
			public string device_name;

			// Token: 0x04001B9E RID: 7070
			public string device_id;

			// Token: 0x04001B9F RID: 7071
			private string <sn>5__2;

			// Token: 0x04001BA0 RID: 7072
			private TaskAwaiter<ValueTuple<bool, string, string, string>> <>u__1;

			// Token: 0x04001BA1 RID: 7073
			private TaskAwaiter<ScanXChecker.OnlineResult> <>u__2;
		}

		// Token: 0x020004AA RID: 1194
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckDeviceAtConnection>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002FAD RID: 12205 RVA: 0x00212690 File Offset: 0x00210890
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter5;
					TaskAwaiter<ScanXChecker.OnlineResult> taskAwaiter7;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<ValueTuple<bool, string, string, string>>);
						num2 = -1;
						goto IL_0223;
					}
					case 2:
						taskAwaiter7 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<ScanXChecker.OnlineResult>);
						num2 = -1;
						goto IL_0305;
					case 3:
						taskAwaiter7 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<ScanXChecker.OnlineResult>);
						num2 = -1;
						goto IL_0413;
					default:
					{
						if (SharedSettings.Current.AdsProductPurchased && !SharedSettings.Current.WhitelistDeviceActivated)
						{
							goto IL_0495;
						}
						if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.WhitelistDeviceSN = "";
							goto IL_0495;
						}
						Guid guid;
						if (!Guid.TryParse(device_id, out guid))
						{
							if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated)
							{
								SharedSettings.Current.AdsProductPurchased = false;
								SharedSettings.Current.WhitelistDeviceActivated = false;
								SharedSettings.Current.WhitelistDeviceSN = "";
								ScanXChecker.UpdateInterface();
							}
							goto IL_0495;
						}
						string text = "";
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
						{
							text = SharedSettings.Current.BTDeviceName;
						}
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE)
						{
							text = SharedSettings.Current.BTLEDeviceName;
						}
						if (text == "CAR2LS ScanX" && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = ScanXChecker.CheckAtDeviceSelection(device_id, text).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ScanXChecker.<CheckDeviceAtConnection>d__3>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (!SharedSettings.Current.WhitelistDeviceActivated)
							{
								goto IL_0495;
							}
							if (text != "CAR2LS ScanX")
							{
								SharedSettings.Current.WhitelistDeviceActivated = false;
								SharedSettings.Current.WhitelistDeviceSN = "";
								SharedSettings.Current.AdsProductPurchased = false;
								ScanXChecker.UpdateInterface();
							}
							taskAwaiter5 = ScanXChecker.CheckOffline(device_id).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter<ValueTuple<bool, string, string, string>> taskAwaiter6 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string, string, string>>, ScanXChecker.<CheckDeviceAtConnection>d__3>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0223;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_0495;
					IL_0223:
					ValueTuple<bool, string, string, string> result = taskAwaiter5.GetResult();
					bool item = result.Item1;
					sn = result.Item2;
					string item2 = result.Item3;
					string item3 = result.Item4;
					if (item)
					{
						dtTicks = DateTimeNowHelper.NowSafe.Ticks;
						if (!(new TimeSpan(dtTicks - SharedSettings.Current.LastTimeLicenceChecked) >= SharedSettings.Current.LicenseCheckPeriod) && SharedSettings.Current.LastTimeLicenceChecked <= dtTicks)
						{
							goto IL_0353;
						}
						taskAwaiter7 = ScanXChecker.CheckOnline(sn, item2, item3).GetAwaiter();
						if (!taskAwaiter7.IsCompleted)
						{
							num2 = 2;
							taskAwaiter2 = taskAwaiter7;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ScanXChecker.OnlineResult>, ScanXChecker.<CheckDeviceAtConnection>d__3>(ref taskAwaiter7, ref this);
							return;
						}
					}
					else if (string.IsNullOrEmpty(sn) || string.IsNullOrEmpty(item2) || string.IsNullOrEmpty(item3))
					{
						if (SharedSettings.Current.WhitelistDeviceActivated)
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.WhitelistDeviceSN = "";
							SharedSettings.Current.AdsProductPurchased = false;
							ScanXChecker.UpdateInterface();
							goto IL_0473;
						}
						goto IL_0473;
					}
					else
					{
						taskAwaiter7 = ScanXChecker.CheckOnline(sn, item2, item3).GetAwaiter();
						if (!taskAwaiter7.IsCompleted)
						{
							num2 = 3;
							taskAwaiter2 = taskAwaiter7;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ScanXChecker.OnlineResult>, ScanXChecker.<CheckDeviceAtConnection>d__3>(ref taskAwaiter7, ref this);
							return;
						}
						goto IL_0413;
					}
					IL_0305:
					ScanXChecker.OnlineResult result2 = taskAwaiter7.GetResult();
					if (result2 != ScanXChecker.OnlineResult.ConnectionFail)
					{
						if (result2 == ScanXChecker.OnlineResult.Valid)
						{
							SharedSettings.Current.LastTimeLicenceChecked = dtTicks;
						}
						else
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.WhitelistDeviceSN = "";
							SharedSettings.Current.AdsProductPurchased = false;
							ScanXChecker.UpdateInterface();
						}
					}
					IL_0353:
					goto IL_0495;
					IL_0413:
					if (taskAwaiter7.GetResult() == ScanXChecker.OnlineResult.Valid)
					{
						SharedSettings.Current.WhitelistDeviceActivated = true;
						SharedSettings.Current.WhitelistDeviceSN = sn;
						SharedSettings.Current.AdsProductPurchased = true;
						ScanXChecker.UpdateInterface();
					}
					else
					{
						SharedSettings.Current.WhitelistDeviceActivated = false;
						SharedSettings.Current.WhitelistDeviceSN = "";
						SharedSettings.Current.AdsProductPurchased = false;
						ScanXChecker.UpdateInterface();
					}
					IL_0473:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					sn = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0495:
				num2 = -2;
				sn = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002FAE RID: 12206 RVA: 0x00212B68 File Offset: 0x00210D68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001BA2 RID: 7074
			public int <>1__state;

			// Token: 0x04001BA3 RID: 7075
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001BA4 RID: 7076
			public string device_id;

			// Token: 0x04001BA5 RID: 7077
			private string <sn>5__2;

			// Token: 0x04001BA6 RID: 7078
			private TaskAwaiter <>u__1;

			// Token: 0x04001BA7 RID: 7079
			private TaskAwaiter<ValueTuple<bool, string, string, string>> <>u__2;

			// Token: 0x04001BA8 RID: 7080
			private long <dtTicks>5__3;

			// Token: 0x04001BA9 RID: 7081
			private TaskAwaiter<ScanXChecker.OnlineResult> <>u__3;
		}

		// Token: 0x020004AB RID: 1195
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckOffline>d__4 : IAsyncStateMachine
		{
			// Token: 0x06002FAF RID: 12207 RVA: 0x00212B78 File Offset: 0x00210D78
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ValueTuple<bool, string, string, string> valueTuple;
				try
				{
					TaskAwaiter<ValueTuple<string, string>> taskAwaiter;
					if (num != 0)
					{
						if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
						{
							valueTuple = new ValueTuple<bool, string, string, string>(false, "", "", "");
							goto IL_0177;
						}
						new BTLEDeviceSelectorViewModel();
						taskAwaiter = ScanXChecker.ReadSNFromRokodil(device_id).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<ValueTuple<string, string>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<string, string>>, ScanXChecker.<CheckOffline>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<ValueTuple<string, string>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<ValueTuple<string, string>>);
						num2 = -1;
					}
					ValueTuple<string, string> result = taskAwaiter.GetResult();
					string item = result.Item1;
					string item2 = result.Item2;
					if (string.IsNullOrEmpty(item) || string.IsNullOrEmpty(item2) || !item.Contains(" SN"))
					{
						SharedSettings.Current.WhitelistDeviceSN = "";
						if (SharedSettings.Current.WhitelistDeviceActivated)
						{
							SharedSettings.Current.WhitelistDeviceActivated = false;
							SharedSettings.Current.AdsProductPurchased = false;
							ScanXChecker.UpdateInterface();
						}
					}
					string text = item.Substring(item.IndexOf(" SN") + 3);
					string text2 = item.Substring(0, item.IndexOf(" "));
					if (SharedSettings.Current.WhitelistDeviceSN == text)
					{
						valueTuple = new ValueTuple<bool, string, string, string>(true, text, item2, text2);
					}
					else
					{
						valueTuple = new ValueTuple<bool, string, string, string>(false, text, item2, text2);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0177:
				num2 = -2;
				this.<>t__builder.SetResult(valueTuple);
			}

			// Token: 0x06002FB0 RID: 12208 RVA: 0x00212D2C File Offset: 0x00210F2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001BAA RID: 7082
			public int <>1__state;

			// Token: 0x04001BAB RID: 7083
			public AsyncTaskMethodBuilder<ValueTuple<bool, string, string, string>> <>t__builder;

			// Token: 0x04001BAC RID: 7084
			public string device_id;

			// Token: 0x04001BAD RID: 7085
			private TaskAwaiter<ValueTuple<string, string>> <>u__1;
		}

		// Token: 0x020004AC RID: 1196
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckOnline>d__16 : IAsyncStateMachine
		{
			// Token: 0x06002FB1 RID: 12209 RVA: 0x00212D3C File Offset: 0x00210F3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ScanXChecker.OnlineResult onlineResult;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (PlatformHelper.AppMarket == Markets.RUS || PlatformHelper.AppMarket == Markets.Sideload || PlatformHelper.AppMarket == Markets.Rustore)
						{
							onlineResult = ScanXChecker.OnlineResult.NotValid;
							goto IL_01A4;
						}
						requestDict = ScanXChecker.BuildRequest(sn, seed, key);
						HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(requestDict), Encoding.UTF8);
						taskAwaiter = HttpDownloader.Post("https://node4.carscanner.info/scanx", httpContent, 10).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, ScanXChecker.<CheckOnline>d__16>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					if (string.IsNullOrEmpty(result))
					{
						onlineResult = ScanXChecker.OnlineResult.ConnectionFail;
					}
					else
					{
						try
						{
							Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
							if (dictionary != null && ScanXChecker.CheckResponse(requestDict, dictionary) && dictionary["isValid"] == "true" && requestDict["sn"] == dictionary["sn"] && requestDict["seed"] == dictionary["seed"] && requestDict["key"] == dictionary["key"])
							{
								onlineResult = ScanXChecker.OnlineResult.Valid;
								goto IL_01A4;
							}
						}
						catch (Exception)
						{
						}
						onlineResult = ScanXChecker.OnlineResult.NotValid;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					requestDict = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A4:
				num2 = -2;
				requestDict = null;
				this.<>t__builder.SetResult(onlineResult);
			}

			// Token: 0x06002FB2 RID: 12210 RVA: 0x00212F3C File Offset: 0x0021113C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001BAE RID: 7086
			public int <>1__state;

			// Token: 0x04001BAF RID: 7087
			public AsyncTaskMethodBuilder<ScanXChecker.OnlineResult> <>t__builder;

			// Token: 0x04001BB0 RID: 7088
			public string sn;

			// Token: 0x04001BB1 RID: 7089
			public string seed;

			// Token: 0x04001BB2 RID: 7090
			public string key;

			// Token: 0x04001BB3 RID: 7091
			private Dictionary<string, string> <requestDict>5__2;

			// Token: 0x04001BB4 RID: 7092
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020004AD RID: 1197
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadSNFromRokodil>d__5 : IAsyncStateMachine
		{
			// Token: 0x06002FB3 RID: 12211 RVA: 0x00212F4C File Offset: 0x0021114C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ValueTuple<string, string> valueTuple;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					ValueTaskAwaiter<byte[]> valueTaskAwaiter;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_015F;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C1;
					}
					case 3:
					{
						ValueTaskAwaiter<byte[]> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<byte[]>);
						num2 = -1;
						goto IL_0228;
					}
					default:
					{
						Guid guid = Guid.Parse("(0000fee0-0000-1000-8000-00805f9b34fb)");
						Guid guid2 = Guid.Parse("(0000fee2-0000-1000-8000-00805f9b34fb)");
						Guid guid3 = Guid.Parse("(0000fee1-0000-1000-8000-00805f9b34fb)");
						connection = new BTLEConnection(guid, guid2, guid3);
						taskAwaiter3 = connection.ConnectAsync(device_guid, PCLDebugStream.CurrentInstance).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ScanXChecker.<ReadSNFromRokodil>d__5>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						valueTuple = new ValueTuple<string, string>("", "");
						goto IL_0292;
					}
					Random random = new Random();
					seed = random.Next(1000, 9999);
					taskAwaiter4 = connection.WriteBytesAsync(Encoding.ASCII.GetBytes("ATI" + seed.ToString("0000"))).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ScanXChecker.<ReadSNFromRokodil>d__5>(ref taskAwaiter4, ref this);
						return;
					}
					IL_015F:
					taskAwaiter4.GetResult();
					taskAwaiter4 = Task.Delay(1000).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ScanXChecker.<ReadSNFromRokodil>d__5>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01C1:
					taskAwaiter4.GetResult();
					valueTaskAwaiter = connection.ReadBytesAsync().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 3;
						ValueTaskAwaiter<byte[]> valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<byte[]>, ScanXChecker.<ReadSNFromRokodil>d__5>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_0228:
					byte[] result = valueTaskAwaiter.GetResult();
					string @string = Encoding.ASCII.GetString(result);
					connection.Disconect();
					valueTuple = new ValueTuple<string, string>(@string, seed.ToString("0000"));
				}
				catch (Exception ex)
				{
					num2 = -2;
					connection = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0292:
				num2 = -2;
				connection = null;
				this.<>t__builder.SetResult(valueTuple);
			}

			// Token: 0x06002FB4 RID: 12212 RVA: 0x00213224 File Offset: 0x00211424
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001BB5 RID: 7093
			public int <>1__state;

			// Token: 0x04001BB6 RID: 7094
			public AsyncTaskMethodBuilder<ValueTuple<string, string>> <>t__builder;

			// Token: 0x04001BB7 RID: 7095
			public string device_guid;

			// Token: 0x04001BB8 RID: 7096
			private BTLEConnection <connection>5__2;

			// Token: 0x04001BB9 RID: 7097
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001BBA RID: 7098
			private int <seed>5__3;

			// Token: 0x04001BBB RID: 7099
			private TaskAwaiter <>u__2;

			// Token: 0x04001BBC RID: 7100
			private ValueTaskAwaiter<byte[]> <>u__3;
		}

		// Token: 0x020004AE RID: 1198
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Test>d__11 : IAsyncStateMachine
		{
			// Token: 0x06002FB5 RID: 12213 RVA: 0x00213234 File Offset: 0x00211434
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002FB6 RID: 12214 RVA: 0x00213280 File Offset: 0x00211480
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001BBD RID: 7101
			public int <>1__state;

			// Token: 0x04001BBE RID: 7102
			public AsyncVoidMethodBuilder <>t__builder;
		}
	}
}
