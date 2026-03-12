using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000738 RID: 1848
	public class CustomPIDViewModel
	{
		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x06003ECE RID: 16078 RVA: 0x0032DD4E File Offset: 0x0032BF4E
		// (set) Token: 0x06003ECF RID: 16079 RVA: 0x0032DD56 File Offset: 0x0032BF56
		public bool Loaded
		{
			[CompilerGenerated]
			get
			{
				return this.<Loaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Loaded>k__BackingField = value;
			}
		}

		// Token: 0x06003ED0 RID: 16080 RVA: 0x0032DD60 File Offset: 0x0032BF60
		public static void Reload()
		{
			CustomPIDViewModel._CurrentProfile = null;
			CustomPIDViewModel._CurrentCustom = null;
			CustomPIDViewModel._DisabledProfile = null;
			CustomPIDViewModel.CurrentProfile.Load();
			CustomPIDViewModel.CurrentCustom.Load();
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentCustom.PidCollection)
			{
				customPID.ReloadFormula();
			}
			try
			{
				foreach (CustomPID customPID2 in CustomPIDViewModel.CurrentProfile.PidCollection)
				{
					customPID2.ReloadFormula();
				}
			}
			catch (Exception)
			{
			}
			CustomPIDViewModel.CheckIdIntegrity();
		}

		// Token: 0x06003ED1 RID: 16081 RVA: 0x0032DE28 File Offset: 0x0032C028
		public static void CheckIdIntegrity()
		{
			List<CustomPID> list = CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Id < 10000000).ToList<CustomPID>();
			if (list.Count > 0)
			{
				list.Max((CustomPID x) => x.Id);
			}
			if (CustomPIDViewModel.CurrentCustom.PidCollection.Count > 0)
			{
				CustomPIDViewModel.CurrentCustom.PidCollection.Max((CustomPID x) => x.Id);
			}
			if (SharedSettings.Current.CustomPidLastId >= 10000000)
			{
				SharedSettings.Current.CustomPidLastId = 100000;
			}
			bool flag = false;
			CustomPID[] array = CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.Id >= 10000000).ToArray<CustomPID>();
			if (array.Length != 0)
			{
				CustomPIDViewModel.FixCustomPidTooHighIds(array);
				flag = true;
			}
			List<CustomPID> list2 = list.Concat(CustomPIDViewModel.CurrentCustom.PidCollection).ToList<CustomPID>();
			for (int i = 0; i < list2.Count; i++)
			{
				int id = list2[i].Id;
				for (int j = 0; j < list2.Count; j++)
				{
					if (i != j)
					{
						int id2 = list2[j].Id;
						if (id == id2 || id2 < 1001)
						{
							SharedSettings sharedSettings = SharedSettings.Current;
							int customPidLastId = sharedSettings.CustomPidLastId;
							sharedSettings.CustomPidLastId = customPidLastId + 1;
							list2[j].Id = SharedSettings.Current.CustomPidLastId;
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				CustomPIDViewModel.CurrentProfile.Save();
				CustomPIDViewModel.CurrentCustom.Save();
			}
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0032E010 File Offset: 0x0032C210
		private static void FixCustomPidTooHighIds(CustomPID[] user_pids_with_high_id)
		{
			if (user_pids_with_high_id == null || user_pids_with_high_id.Length == 0)
			{
				return;
			}
			string dashboard = SharedSettings.Current.Dashboard;
			List<ProxyPage> list = null;
			try
			{
				list = Json.DeserializeObject<List<ProxyPage>>(dashboard);
			}
			catch (Exception)
			{
			}
			string[] array = SharedSettings.Current.LastCarAvailableSensors.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			LiveDataPageSettings liveDataPageSettings = LiveDataPageSettings.Load();
			string[] array2 = SharedSettings.Current.MultiPidsSelected.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (CustomPID customPID in user_pids_with_high_id)
			{
				int id = customPID.Id;
				SharedSettings sharedSettings = SharedSettings.Current;
				int num = sharedSettings.CustomPidLastId + 1;
				sharedSettings.CustomPidLastId = num;
				int num2 = num;
				string text = id.ToString();
				string text2 = num2.ToString();
				customPID.Id = num2;
				if (list != null)
				{
					foreach (ProxyPage proxyPage in list)
					{
						foreach (ProxyItem proxyItem in proxyPage.Items)
						{
							if (proxyItem.PID_Id == id)
							{
								proxyItem.PID_Id = num2;
							}
						}
					}
				}
				if (liveDataPageSettings.PIDId0 == id)
				{
					liveDataPageSettings.PIDId0 = num2;
				}
				if (liveDataPageSettings.PIDId1 == id)
				{
					liveDataPageSettings.PIDId1 = num2;
				}
				if (liveDataPageSettings.PIDId2 == id)
				{
					liveDataPageSettings.PIDId2 = num2;
				}
				if (liveDataPageSettings.PIDId3 == id)
				{
					liveDataPageSettings.PIDId3 = num2;
				}
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] == text)
					{
						array[j] = text2;
					}
				}
				for (int k = 0; k < array2.Length; k++)
				{
					if (array2[k] == text)
					{
						array2[k] = text2;
					}
				}
			}
			if (list != null)
			{
				SharedSettings.Current.Dashboard = JsonConvert.SerializeObject(list);
			}
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			foreach (string text3 in array)
			{
				stringBuilder.Append(text3);
				stringBuilder.Append(';');
			}
			SharedSettings.Current.LastCarAvailableSensors = stringBuilder.ToString();
			new StringBuilder(array2.Length * 2);
			foreach (string text4 in array2)
			{
				stringBuilder.Append(text4);
				stringBuilder.Append(';');
			}
			SharedSettings.Current.MultiPidsSelected = stringBuilder.ToString();
			liveDataPageSettings.Save();
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x06003ED3 RID: 16083 RVA: 0x0032E2C0 File Offset: 0x0032C4C0
		public static CustomPIDViewModel DisabledProfile
		{
			get
			{
				if (CustomPIDViewModel._DisabledProfile == null)
				{
					CustomPIDViewModel._DisabledProfile = new CustomPIDViewModel(CustomPIDViewModel.CustomModelType.DisabledProfile);
				}
				return CustomPIDViewModel._DisabledProfile;
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x06003ED4 RID: 16084 RVA: 0x0032E2D9 File Offset: 0x0032C4D9
		public static CustomPIDViewModel CurrentCustom
		{
			get
			{
				if (CustomPIDViewModel._CurrentCustom == null)
				{
					CustomPIDViewModel._CurrentCustom = new CustomPIDViewModel(CustomPIDViewModel.CustomModelType.Custom);
				}
				return CustomPIDViewModel._CurrentCustom;
			}
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x0032E2F2 File Offset: 0x0032C4F2
		public static CustomPIDViewModel CurrentProfile
		{
			get
			{
				if (CustomPIDViewModel._CurrentProfile == null)
				{
					CustomPIDViewModel._CurrentProfile = new CustomPIDViewModel(CustomPIDViewModel.CustomModelType.Profile);
				}
				return CustomPIDViewModel._CurrentProfile;
			}
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x0032E30B File Offset: 0x0032C50B
		public ObservableCollection<CustomPID> PidCollection
		{
			get
			{
				return this._PidCollection;
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0032E313 File Offset: 0x0032C513
		public CustomPIDViewModel(CustomPIDViewModel.CustomModelType modelType)
		{
			this._PidCollection = new ObservableCollection<CustomPID>();
			if (modelType == CustomPIDViewModel.CustomModelType.Custom)
			{
				this.PidCollection.CollectionChanged += this.CustomPidCollection_CollectionChanged;
			}
			this.CurrentModelType = modelType;
		}

		// Token: 0x06003ED8 RID: 16088 RVA: 0x0032E348 File Offset: 0x0032C548
		private void CustomPidCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if ((e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace) && e.NewItems != null)
			{
				foreach (object obj in e.NewItems)
				{
					CustomPID customPID = obj as CustomPID;
					if (customPID.Id < 1001)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int customPidLastId = sharedSettings.CustomPidLastId;
						sharedSettings.CustomPidLastId = customPidLastId + 1;
						customPID.Id = SharedSettings.Current.CustomPidLastId;
					}
				}
			}
		}

		// Token: 0x06003ED9 RID: 16089 RVA: 0x0032E3E4 File Offset: 0x0032C5E4
		public void Save()
		{
			string text = Json.SerializeObject(this.PidCollection);
			if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.Custom)
			{
				SharedSettings.Current.CustomPIDsCollection = text;
			}
			else if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.Profile)
			{
				SharedSettings.Current.ProfilePIDsCollection = text;
			}
			else if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.DisabledProfile)
			{
				SharedSettings.Current.DisabledProfilePIDsCollection = text;
			}
			OBDDataReader obdreader = App.OBDReader;
			if (obdreader == null)
			{
				return;
			}
			CarData currentCarData = obdreader.CurrentCarData;
			if (currentCarData == null)
			{
				return;
			}
			currentCarData.ClearFindCache();
		}

		// Token: 0x06003EDA RID: 16090 RVA: 0x0032E458 File Offset: 0x0032C658
		public void Load()
		{
			try
			{
				this.PidCollection.Clear();
				string text = string.Empty;
				if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.Custom)
				{
					text = SharedSettings.Current.CustomPIDsCollection;
				}
				else if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.Profile)
				{
					text = SharedSettings.Current.ProfilePIDsCollection;
				}
				else
				{
					text = SharedSettings.Current.DisabledProfilePIDsCollection;
				}
				if (!string.IsNullOrEmpty(text))
				{
					foreach (CustomPID customPID in Json.DeserializeObject<ObservableCollection<CustomPID>>(text))
					{
						try
						{
							if (customPID != null)
							{
								if (this.CurrentModelType == CustomPIDViewModel.CustomModelType.Profile)
								{
									customPID.IsFormulaHidden = true;
								}
								this.PidCollection.Add(customPID);
								customPID.IsAvailable = true;
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				PCLDebugStream currentInstance = PCLDebugStream.CurrentInstance;
				if (currentInstance != null)
				{
					currentInstance.WriteLineAsync("Load CPVM Error: " + ex.ToString());
				}
			}
			this.Loaded = true;
		}

		// Token: 0x06003EDB RID: 16091 RVA: 0x0032E560 File Offset: 0x0032C760
		public void LoadFromString(string jsondata)
		{
			this.PidCollection.Clear();
			try
			{
				foreach (CustomPID customPID in Json.DeserializeObject<ObservableCollection<CustomPID>>(jsondata))
				{
					this.PidCollection.Add(customPID);
				}
			}
			catch
			{
			}
			this.Loaded = true;
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x0032E5D8 File Offset: 0x0032C7D8
		public void AddFromString(string jsondata)
		{
			if (string.IsNullOrEmpty(jsondata))
			{
				return;
			}
			try
			{
				foreach (CustomPID customPID in Json.DeserializeObject<ObservableCollection<CustomPID>>(jsondata))
				{
					this.PidCollection.Add(customPID);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0032E644 File Offset: 0x0032C844
		public IEnumerable<PID_SupportedPids> CreateCheckPIDs()
		{
			List<PID_SupportedPids> list = new List<PID_SupportedPids>();
			List<int> list2 = new List<int>(2);
			foreach (CustomPID customPID in this.PidCollection)
			{
				int length = customPID.Command.Length;
				if (!list2.Contains(length))
				{
					list2.Add(length);
				}
			}
			using (List<int>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int singLength = enumerator2.Current;
					List<string> list3 = new List<string>();
					IEnumerable<CustomPID> enumerable = this.PidCollection.Where((CustomPID x) => x.Command.Length == singLength);
					foreach (CustomPID customPID2 in enumerable)
					{
						string header2 = customPID2.Header;
						if (!list3.Contains(header2))
						{
							list3.Add(header2);
						}
					}
					using (List<string>.Enumerator enumerator3 = list3.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string header = enumerator3.Current;
							foreach (PID pid in from x in enumerable
								where x.Header == header
								orderby x.Command
								select x)
							{
								string checkCmd = (BitHelpers.ConvertHexToInt(pid.Command) / 32 * 32).ToString("X" + singLength.ToString(), CultureInfo.InvariantCulture);
								if (!list.Any((PID_SupportedPids x) => x.Command == checkCmd && x.Header == header))
								{
									list.Add(new PID_SupportedPids(checkCmd, this.PidCollection.Select((CustomPID x) => x).ToList<PID>(), header));
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06003EDE RID: 16094 RVA: 0x0032E908 File Offset: 0x0032CB08
		private string FormulaFix(string s)
		{
			string text;
			try
			{
				s = this.FormulaFix_ReplaceTorqueGetBit(s);
				s = this.FormulaFix_ReplaceSigned(s);
				text = s;
			}
			catch
			{
				text = s;
			}
			return text;
		}

		// Token: 0x06003EDF RID: 16095 RVA: 0x0032E944 File Offset: 0x0032CB44
		private string FormulaFix_ReplaceSigned(string s)
		{
			string text;
			try
			{
				text = s.Replace("signed", "Signed");
			}
			catch
			{
				text = s;
			}
			return text;
		}

		// Token: 0x06003EE0 RID: 16096 RVA: 0x0032E97C File Offset: 0x0032CB7C
		private string FormulaFix_ReplaceTorqueGetBit(string s)
		{
			string text;
			try
			{
				int num = s.IndexOf('{');
				if (num < 0)
				{
					text = s;
				}
				else
				{
					int num2 = s.IndexOf(':', num);
					if (num2 < 0)
					{
						text = s;
					}
					else
					{
						int num3 = s.IndexOf('}', num2);
						if (num3 < 0)
						{
							text = s;
						}
						else if (num >= 0 && num2 >= 0 && num3 >= 0)
						{
							string text2 = s.Substring(num + 1, num2 - (num + 1));
							string text3 = s.Substring(num2 + 1, num3 - 1 - num2);
							string text4 = string.Concat(new string[] { "GetBit(", text2, ",", text3, ")" });
							if (num > 0)
							{
								text4 = s.Substring(0, num) + text4;
							}
							if (num3 < s.Length - 1)
							{
								text4 += s.Substring(num3 + 1);
							}
							s = this.FormulaFix_ReplaceTorqueGetBit(text4);
							text = s;
						}
						else
						{
							text = s;
						}
					}
				}
			}
			catch
			{
				text = s;
			}
			return text;
		}

		// Token: 0x06003EE1 RID: 16097 RVA: 0x0032EA88 File Offset: 0x0032CC88
		internal void Unload()
		{
			this.Loaded = false;
		}

		// Token: 0x0400268A RID: 9866
		[CompilerGenerated]
		private bool <Loaded>k__BackingField;

		// Token: 0x0400268B RID: 9867
		private CustomPIDViewModel.CustomModelType CurrentModelType;

		// Token: 0x0400268C RID: 9868
		private const int PROFILE_IDS = 10000000;

		// Token: 0x0400268D RID: 9869
		private static CustomPIDViewModel _DisabledProfile;

		// Token: 0x0400268E RID: 9870
		private static CustomPIDViewModel _CurrentCustom;

		// Token: 0x0400268F RID: 9871
		private static CustomPIDViewModel _CurrentProfile;

		// Token: 0x04002690 RID: 9872
		private ObservableCollection<CustomPID> _PidCollection;

		// Token: 0x02000739 RID: 1849
		public enum CustomModelType
		{
			// Token: 0x04002692 RID: 9874
			Custom,
			// Token: 0x04002693 RID: 9875
			Profile,
			// Token: 0x04002694 RID: 9876
			DisabledProfile
		}

		// Token: 0x0200073A RID: 1850
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003EE2 RID: 16098 RVA: 0x0032EA91 File Offset: 0x0032CC91
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003EE3 RID: 16099 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003EE4 RID: 16100 RVA: 0x0032EA9D File Offset: 0x0032CC9D
			internal bool <CheckIdIntegrity>b__8_0(CustomPID x)
			{
				return x.Id < 10000000;
			}

			// Token: 0x06003EE5 RID: 16101 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <CheckIdIntegrity>b__8_1(CustomPID x)
			{
				return x.Id;
			}

			// Token: 0x06003EE6 RID: 16102 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <CheckIdIntegrity>b__8_2(CustomPID x)
			{
				return x.Id;
			}

			// Token: 0x06003EE7 RID: 16103 RVA: 0x0032EAAC File Offset: 0x0032CCAC
			internal bool <CheckIdIntegrity>b__8_3(CustomPID x)
			{
				return x.Id >= 10000000;
			}

			// Token: 0x06003EE8 RID: 16104 RVA: 0x001C6E3D File Offset: 0x001C503D
			internal string <CreateCheckPIDs>b__28_2(CustomPID x)
			{
				return x.Command;
			}

			// Token: 0x06003EE9 RID: 16105 RVA: 0x00016849 File Offset: 0x00014A49
			internal PID <CreateCheckPIDs>b__28_4(CustomPID x)
			{
				return x;
			}

			// Token: 0x04002695 RID: 9877
			public static readonly CustomPIDViewModel.<>c <>9 = new CustomPIDViewModel.<>c();

			// Token: 0x04002696 RID: 9878
			public static Func<CustomPID, bool> <>9__8_0;

			// Token: 0x04002697 RID: 9879
			public static Func<CustomPID, int> <>9__8_1;

			// Token: 0x04002698 RID: 9880
			public static Func<CustomPID, int> <>9__8_2;

			// Token: 0x04002699 RID: 9881
			public static Func<CustomPID, bool> <>9__8_3;

			// Token: 0x0400269A RID: 9882
			public static Func<CustomPID, string> <>9__28_2;

			// Token: 0x0400269B RID: 9883
			public static Func<CustomPID, PID> <>9__28_4;
		}

		// Token: 0x0200073B RID: 1851
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_0
		{
			// Token: 0x06003EEA RID: 16106 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_0()
			{
			}

			// Token: 0x06003EEB RID: 16107 RVA: 0x0032EABE File Offset: 0x0032CCBE
			internal bool <CreateCheckPIDs>b__0(CustomPID x)
			{
				return x.Command.Length == this.singLength;
			}

			// Token: 0x0400269C RID: 9884
			public int singLength;
		}

		// Token: 0x0200073C RID: 1852
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_1
		{
			// Token: 0x06003EEC RID: 16108 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_1()
			{
			}

			// Token: 0x06003EED RID: 16109 RVA: 0x0032EAD3 File Offset: 0x0032CCD3
			internal bool <CreateCheckPIDs>b__1(CustomPID x)
			{
				return x.Header == this.header;
			}

			// Token: 0x0400269D RID: 9885
			public string header;
		}

		// Token: 0x0200073D RID: 1853
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_2
		{
			// Token: 0x06003EEE RID: 16110 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_2()
			{
			}

			// Token: 0x06003EEF RID: 16111 RVA: 0x0032EAE6 File Offset: 0x0032CCE6
			internal bool <CreateCheckPIDs>b__3(PID_SupportedPids x)
			{
				return x.Command == this.checkCmd && x.Header == this.CS$<>8__locals1.header;
			}

			// Token: 0x0400269E RID: 9886
			public string checkCmd;

			// Token: 0x0400269F RID: 9887
			public CustomPIDViewModel.<>c__DisplayClass28_1 CS$<>8__locals1;
		}
	}
}
