using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200070F RID: 1807
	public class RecorderedTracksViewModel : INotifyPropertyChanged
	{
		// Token: 0x06003D4B RID: 15691 RVA: 0x00327A92 File Offset: 0x00325C92
		public RecorderedTracksViewModel()
		{
		}

		// Token: 0x17001406 RID: 5126
		// (get) Token: 0x06003D4C RID: 15692 RVA: 0x00327AA5 File Offset: 0x00325CA5
		// (set) Token: 0x06003D4D RID: 15693 RVA: 0x00327AAD File Offset: 0x00325CAD
		public bool IsSelectionMode
		{
			get
			{
				return this._IsSelectionMode;
			}
			set
			{
				if (value != this._IsSelectionMode)
				{
					this._IsSelectionMode = value;
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged(this, new PropertyChangedEventArgs("IsSelectionMode"));
				}
			}
		}

		// Token: 0x17001407 RID: 5127
		// (get) Token: 0x06003D4E RID: 15694 RVA: 0x00327ADA File Offset: 0x00325CDA
		public ObservableCollection<FileToRecordProxy> RecorderedTracks
		{
			get
			{
				return this._RecorderedTracks;
			}
		}

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06003D4F RID: 15695 RVA: 0x00327AE4 File Offset: 0x00325CE4
		// (remove) Token: 0x06003D50 RID: 15696 RVA: 0x00327B1C File Offset: 0x00325D1C
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

		// Token: 0x06003D51 RID: 15697 RVA: 0x00327B54 File Offset: 0x00325D54
		private void LoadFilesFromFolder(string folder_path)
		{
			string[] array = Directory.GetFiles(folder_path);
			IEnumerable<string> enumerable = array.Where((string x) => Path.GetExtension(x).ToLowerInvariant() == ".br2");
			bool flag = false;
			foreach (string text in enumerable)
			{
				try
				{
					string text2 = text.Substring(0, text.Length - 2) + "rc";
					File.Move(text, text2);
					flag = true;
				}
				catch (Exception)
				{
				}
			}
			foreach (string text3 in array.Where((string x) => Path.GetExtension(x).ToLowerInvariant() == ".bc"))
			{
				try
				{
					string text4 = text3.Replace(".bc", ".brc");
					File.Move(text3, text4);
					flag = true;
				}
				catch (Exception)
				{
				}
			}
			if (flag)
			{
				array = Directory.GetFiles(folder_path);
			}
			List<string> list = (from x in array.Where(delegate(string x)
				{
					string text5 = Path.GetExtension(x).ToLowerInvariant();
					return text5 == ".rec" || text5 == ".brc";
				})
				orderby x descending
				select x).ToList<string>();
			List<FileToRecordProxy> proxyList = (from x in list
				where x != null
				select new FileToRecordProxy(x)).ToList<FileToRecordProxy>();
			foreach (FileToRecordProxy fileToRecordProxy in proxyList)
			{
				try
				{
					fileToRecordProxy.LoadSize();
				}
				catch (Exception)
				{
				}
			}
			if (!SharedSettings.Current.ShowExperimental)
			{
				foreach (FileToRecordProxy fileToRecordProxy2 in proxyList.Where((FileToRecordProxy x) => x.SizeLoaded && x.Size == "0 B").ToList<FileToRecordProxy>())
				{
					proxyList.Remove(fileToRecordProxy2);
				}
			}
			int num = 20;
			if (proxyList.Count < 20)
			{
				num = proxyList.Count;
			}
			for (int i = 0; i < num; i++)
			{
				try
				{
					proxyList[i].LoadMeta();
				}
				catch (Exception)
				{
				}
			}
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				foreach (FileToRecordProxy fileToRecordProxy3 in proxyList)
				{
					this.RecorderedTracks.Add(fileToRecordProxy3);
				}
			});
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00327E80 File Offset: 0x00326080
		public void LoadList()
		{
			try
			{
				if (MainThread.IsMainThread)
				{
					this.RecorderedTracks.Clear();
				}
				else
				{
					MainThread.InvokeOnMainThreadAsync(delegate
					{
						this.RecorderedTracks.Clear();
					});
					Task.Delay(1000).Wait();
				}
			}
			catch (Exception)
			{
			}
			if (PlatformHelper.IsAndroid)
			{
				try
				{
					PlatformHelper.DroidService.FileMigrator_MigrateRecords(null);
				}
				catch (Exception)
				{
				}
			}
			try
			{
				this.LoadFilesFromFolder(FileSystemHelper.LocalStoragePath);
			}
			catch (Exception)
			{
			}
			try
			{
				string text = Path.Combine(FileSystemHelper.LocalStoragePath, "TrackRecords");
				if (Directory.Exists(text))
				{
					this.LoadFilesFromFolder(text);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				string externalStoragePath = FileSystemHelper.ExternalStoragePath;
				if (externalStoragePath != null)
				{
					string text2 = Path.Combine(externalStoragePath, "records");
					if (Directory.Exists(text2))
					{
						this.LoadFilesFromFolder(text2);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x00327F80 File Offset: 0x00326180
		public static DataRecorder LoadRecorderFromFile(string filepath)
		{
			DataRecorder dataRecorder2;
			try
			{
				using (FileStream fileStream = File.OpenRead(filepath))
				{
					JsonSerializer jsonSerializer = new JsonSerializer();
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
						{
							jsonSerializer.Culture = CultureInfo.InvariantCulture;
							DataRecorder dataRecorder = jsonSerializer.Deserialize<DataRecorder>(jsonTextReader);
							dataRecorder.Records = dataRecorder.Records.OrderBy((DataRecord x) => x.Name).ToList<DataRecord>();
							dataRecorder.Synchronize();
							dataRecorder2 = dataRecorder;
						}
					}
				}
			}
			catch (Exception)
			{
				dataRecorder2 = null;
			}
			return dataRecorder2;
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x00328054 File Offset: 0x00326254
		[CompilerGenerated]
		private void <LoadList>b__12_0()
		{
			this.RecorderedTracks.Clear();
		}

		// Token: 0x0400259B RID: 9627
		private bool _IsSelectionMode;

		// Token: 0x0400259C RID: 9628
		private ObservableCollection<FileToRecordProxy> _RecorderedTracks = new ObservableCollection<FileToRecordProxy>();

		// Token: 0x0400259D RID: 9629
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x02000710 RID: 1808
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003D55 RID: 15701 RVA: 0x00328061 File Offset: 0x00326261
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003D56 RID: 15702 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003D57 RID: 15703 RVA: 0x0032806D File Offset: 0x0032626D
			internal bool <LoadFilesFromFolder>b__11_0(string x)
			{
				return Path.GetExtension(x).ToLowerInvariant() == ".br2";
			}

			// Token: 0x06003D58 RID: 15704 RVA: 0x00328084 File Offset: 0x00326284
			internal bool <LoadFilesFromFolder>b__11_1(string x)
			{
				return Path.GetExtension(x).ToLowerInvariant() == ".bc";
			}

			// Token: 0x06003D59 RID: 15705 RVA: 0x0032809C File Offset: 0x0032629C
			internal bool <LoadFilesFromFolder>b__11_2(string x)
			{
				string text = Path.GetExtension(x).ToLowerInvariant();
				return text == ".rec" || text == ".brc";
			}

			// Token: 0x06003D5A RID: 15706 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <LoadFilesFromFolder>b__11_3(string x)
			{
				return x;
			}

			// Token: 0x06003D5B RID: 15707 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <LoadFilesFromFolder>b__11_4(string x)
			{
				return x != null;
			}

			// Token: 0x06003D5C RID: 15708 RVA: 0x003280D2 File Offset: 0x003262D2
			internal FileToRecordProxy <LoadFilesFromFolder>b__11_5(string x)
			{
				return new FileToRecordProxy(x);
			}

			// Token: 0x06003D5D RID: 15709 RVA: 0x003280DA File Offset: 0x003262DA
			internal bool <LoadFilesFromFolder>b__11_7(FileToRecordProxy x)
			{
				return x.SizeLoaded && x.Size == "0 B";
			}

			// Token: 0x06003D5E RID: 15710 RVA: 0x00315BB1 File Offset: 0x00313DB1
			internal string <LoadRecorderFromFile>b__13_0(DataRecord x)
			{
				return x.Name;
			}

			// Token: 0x0400259E RID: 9630
			public static readonly RecorderedTracksViewModel.<>c <>9 = new RecorderedTracksViewModel.<>c();

			// Token: 0x0400259F RID: 9631
			public static Func<string, bool> <>9__11_0;

			// Token: 0x040025A0 RID: 9632
			public static Func<string, bool> <>9__11_1;

			// Token: 0x040025A1 RID: 9633
			public static Func<string, bool> <>9__11_2;

			// Token: 0x040025A2 RID: 9634
			public static Func<string, string> <>9__11_3;

			// Token: 0x040025A3 RID: 9635
			public static Func<string, bool> <>9__11_4;

			// Token: 0x040025A4 RID: 9636
			public static Func<string, FileToRecordProxy> <>9__11_5;

			// Token: 0x040025A5 RID: 9637
			public static Func<FileToRecordProxy, bool> <>9__11_7;

			// Token: 0x040025A6 RID: 9638
			public static Func<DataRecord, string> <>9__13_0;
		}

		// Token: 0x02000711 RID: 1809
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x06003D5F RID: 15711 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x06003D60 RID: 15712 RVA: 0x003280F8 File Offset: 0x003262F8
			internal void <LoadFilesFromFolder>b__6()
			{
				foreach (FileToRecordProxy fileToRecordProxy in this.proxyList)
				{
					this.<>4__this.RecorderedTracks.Add(fileToRecordProxy);
				}
			}

			// Token: 0x040025A7 RID: 9639
			public List<FileToRecordProxy> proxyList;

			// Token: 0x040025A8 RID: 9640
			public RecorderedTracksViewModel <>4__this;
		}
	}
}
