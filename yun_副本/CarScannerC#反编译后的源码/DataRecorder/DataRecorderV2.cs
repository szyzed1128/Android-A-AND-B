using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.SpeedTest;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006E4 RID: 1764
	public class DataRecorderV2
	{
		// Token: 0x06003C1D RID: 15389 RVA: 0x00317C60 File Offset: 0x00315E60
		public void Record(IPIDFloatValue pid, double TimeStampSeconds)
		{
			if (pid == null)
			{
				return;
			}
			object obj = this.write_lock;
			lock (obj)
			{
				if (this.bw == null)
				{
					this.Start((CarInfoViewModel.Instance != null) ? CarInfoViewModel.Instance.VIN : "");
				}
				if (!this.pids.ContainsKey(pid.Id))
				{
					BinaryWriter binaryWriter = this.bw;
					if (binaryWriter != null)
					{
						binaryWriter.Write(13984144);
					}
					BinaryWriter binaryWriter2 = this.bw;
					if (binaryWriter2 != null)
					{
						binaryWriter2.Write(pid.Id);
					}
					BinaryWriter binaryWriter3 = this.bw;
					if (binaryWriter3 != null)
					{
						binaryWriter3.Write(pid.Name ?? "");
					}
					BinaryWriter binaryWriter4 = this.bw;
					if (binaryWriter4 != null)
					{
						binaryWriter4.Write(pid.ShortName ?? "");
					}
					BinaryWriter binaryWriter5 = this.bw;
					if (binaryWriter5 != null)
					{
						binaryWriter5.Write((int)pid.Units);
					}
					this.pids.Add(pid.Id, new KeyValuePair<string, string>(pid.Name ?? "", pid.ShortName ?? ""));
				}
				BinaryWriter binaryWriter6 = this.bw;
				if (binaryWriter6 != null)
				{
					binaryWriter6.Write(19350352);
				}
				BinaryWriter binaryWriter7 = this.bw;
				if (binaryWriter7 != null)
				{
					binaryWriter7.Write(TimeStampSeconds);
				}
				BinaryWriter binaryWriter8 = this.bw;
				if (binaryWriter8 != null)
				{
					binaryWriter8.Write(pid.Id);
				}
				BinaryWriter binaryWriter9 = this.bw;
				if (binaryWriter9 != null)
				{
					binaryWriter9.Write(pid.Value);
				}
				if (!this.hasData && double.IsFinite(pid.Value))
				{
					this.hasData = true;
				}
			}
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x00317E10 File Offset: 0x00316010
		public void RecordNaN(IPIDFloatValue pid, double TimeStampSeconds)
		{
			if (pid == null)
			{
				return;
			}
			object obj = this.write_lock;
			lock (obj)
			{
				string text = "";
				if (CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.VIN != null)
				{
					text = CarInfoViewModel.Instance.VIN;
				}
				if (this.bw == null)
				{
					this.Start(text);
				}
				if (this.pids != null && !this.pids.ContainsKey(pid.Id))
				{
					this.bw.Write(13984144);
					this.bw.Write(pid.Id);
					this.bw.Write(pid.Name ?? "");
					this.bw.Write(pid.ShortName ?? "");
					this.bw.Write((int)pid.Units);
					this.pids.Add(pid.Id, new KeyValuePair<string, string>(pid.Name ?? "", pid.ShortName ?? ""));
				}
				this.bw.Write(19350352);
				this.bw.Write(TimeStampSeconds);
				this.bw.Write(pid.Id);
				this.bw.Write(double.NaN);
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x00317F8C File Offset: 0x0031618C
		public void Record(DTCItemV2 dtc)
		{
			if (dtc == null)
			{
				return;
			}
			object obj = this.write_lock;
			lock (obj)
			{
				this.hasData = true;
				if (this.bw == null)
				{
					this.Start(CarInfoViewModel.Instance.VIN ?? "");
				}
				string text = JsonConvert.SerializeObject(dtc);
				BinaryWriter binaryWriter = this.bw;
				if (binaryWriter != null)
				{
					binaryWriter.Write(383857192);
				}
				BinaryWriter binaryWriter2 = this.bw;
				if (binaryWriter2 != null)
				{
					binaryWriter2.Write(DateTimeNowHelper.NowSafe.Ticks);
				}
				BinaryWriter binaryWriter3 = this.bw;
				if (binaryWriter3 != null)
				{
					binaryWriter3.Write(text);
				}
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x00318040 File Offset: 0x00316240
		public void Record(List<DTCItemV2> dtcList)
		{
			object obj = this.write_lock;
			lock (obj)
			{
				this.hasData = true;
				if (this.bw == null)
				{
					this.Start(CarInfoViewModel.Instance.VIN);
				}
				foreach (DTCItemV2 dtcitemV in dtcList)
				{
					string text = JsonConvert.SerializeObject(dtcitemV);
					this.bw.Write(383857192);
					this.bw.Write(DateTimeNowHelper.NowSafe.Ticks);
					this.bw.Write(text);
				}
				this.Save();
			}
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x00318110 File Offset: 0x00316310
		public void Record(double latitude, double longtitude)
		{
			if (latitude == 0.0 && longtitude == 0.0)
			{
				return;
			}
			object obj = this.write_lock;
			lock (obj)
			{
				this.hasData = true;
				if (this.bw == null)
				{
					this.Start(CarInfoViewModel.Instance.VIN);
				}
				this.bw.Write(527891292);
				this.bw.Write(App.OBDReader.stopwatch.Elapsed.TotalSeconds);
				this.bw.Write(latitude);
				this.bw.Write(longtitude);
			}
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x003181CC File Offset: 0x003163CC
		public void Record(ISpeedTestBase speedTest)
		{
			object obj = this.write_lock;
			lock (obj)
			{
				this.hasData = true;
				if (this.bw == null)
				{
					this.Start(CarInfoViewModel.Instance.VIN);
				}
				this.bw.Write(59201502);
				TimeSpan elapsed = App.OBDReader.stopwatch.Elapsed;
				this.bw.Write(elapsed.TotalSeconds);
				ProxyTest proxyTest = ProxyTest.GetProxyTest(speedTest);
				proxyTest.Timestamp = this.TimeStarted.Add(elapsed);
				string text = JsonConvert.SerializeObject(proxyTest);
				this.bw.Write(text);
			}
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x00318284 File Offset: 0x00316484
		public static void StartRecording()
		{
			DataRecorderV2 dataRecorderV = new DataRecorderV2();
			dataRecorderV.Start(CarInfoViewModel.Instance.IsVINAvailable ? CarInfoViewModel.Instance.VIN : string.Empty);
			App.OBDReader.CurrentCarData.Recorder = dataRecorderV;
			App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
			App.OBDReader.StatusChanged += DataRecorderV2.StopRecording;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x003182F8 File Offset: 0x003164F8
		public static async void StopRecording(OBDDataReaderStatus NewStatus)
		{
			if (NewStatus == OBDDataReaderStatus.Disconnected)
			{
				App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder != null)
				{
					if (recorder != null)
					{
						recorder.Stop();
					}
					App.OBDReader.CurrentCarData.Recorder = null;
					SimpleMainPage.ShowActivityFrame();
					await Task.Delay(200);
					SimpleMainPage.HideActivityFrame();
				}
			}
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x00318330 File Offset: 0x00316530
		private void Start(string VIN)
		{
			this.TimeStarted = DateTimeNowHelper.NowSafe;
			string text = this.TimeStarted.ToString("yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture);
			this.FileName = text + ".brc";
			try
			{
				BufferedStream bufferedStream = new BufferedStream(File.Open(FileSystemHelper.GetLocalFilePath(this.FileName), FileMode.Create, FileAccess.ReadWrite), 16384);
				this.bw = new BinaryWriter(bufferedStream, Encoding.UTF8, true);
				this.bw.Write("CARSCANNERRECORD");
				this.bw.Write(2);
				this.bw.Write(VIN);
				this.bw.Write(SharedSettings.Current.CurrentCarName);
				this.bw.Write(SharedSettings.Current.BrandAndProfile);
				string text2 = "";
				switch (SharedSettings.Current.ConnectionType)
				{
				case ConnectionTypes.WiFi:
					text2 = "Wi-Fi " + SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
					break;
				case ConnectionTypes.BluetoothLE:
					text2 = "BTLE " + SharedSettings.Current.BTLEDeviceName;
					break;
				case ConnectionTypes.Bluetooth:
					text2 = string.Concat(new string[]
					{
						"BT ",
						SharedSettings.Current.BTDeviceName,
						" [",
						SharedSettings.Current.BTLEDeviceID,
						"]"
					});
					break;
				}
				this.bw.Write(text2);
				this.bw.Write(this.TimeStarted.Ticks);
				GPSDataCollector.StartUpdates();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x003184D8 File Offset: 0x003166D8
		public void Stop()
		{
			BinaryWriter binaryWriter = this.bw;
			this.bw = null;
			if (binaryWriter != null)
			{
				try
				{
					binaryWriter.Flush();
					BufferedStream bufferedStream = (BufferedStream)binaryWriter.BaseStream;
					Stream underlyingStream = bufferedStream.UnderlyingStream;
					bufferedStream.Flush();
					underlyingStream.Flush();
					binaryWriter.Close();
					bufferedStream.Close();
					underlyingStream.Close();
					if (!this.hasData)
					{
						try
						{
							App.OBDReader.DebugWrite("\r\n[Recorder hasData=false]");
						}
						catch (Exception)
						{
						}
						try
						{
							File.Delete(FileSystemHelper.GetLocalFilePath(this.FileName));
						}
						catch (Exception)
						{
						}
					}
				}
				catch (Exception)
				{
				}
			}
			GPSDataCollector.StopUpdates();
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x00318590 File Offset: 0x00316790
		public void Save()
		{
			BinaryWriter binaryWriter = this.bw;
			if (binaryWriter != null)
			{
				binaryWriter.Flush();
			}
			BinaryWriter binaryWriter2 = this.bw;
			if (binaryWriter2 == null)
			{
				return;
			}
			Stream baseStream = binaryWriter2.BaseStream;
			if (baseStream == null)
			{
				return;
			}
			baseStream.Flush();
		}

		// Token: 0x170013DA RID: 5082
		// (get) Token: 0x06003C28 RID: 15400 RVA: 0x003185BD File Offset: 0x003167BD
		// (set) Token: 0x06003C29 RID: 15401 RVA: 0x003185C5 File Offset: 0x003167C5
		public string FileName
		{
			[CompilerGenerated]
			get
			{
				return this.<FileName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FileName>k__BackingField = value;
			}
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x003185CE File Offset: 0x003167CE
		public DataRecorderV2()
		{
		}

		// Token: 0x040024C8 RID: 9416
		public const string RECORDED_MARKER = "CARSCANNERRECORD";

		// Token: 0x040024C9 RID: 9417
		public const int PID_ID_MARKER = 13984144;

		// Token: 0x040024CA RID: 9418
		public const int PID_VALUE_MARKER = 19350352;

		// Token: 0x040024CB RID: 9419
		private object write_lock = new object();

		// Token: 0x040024CC RID: 9420
		public const int DTC_MARKER = 383857192;

		// Token: 0x040024CD RID: 9421
		public const int GEO_MARKER = 527891292;

		// Token: 0x040024CE RID: 9422
		public const int SPEED_TEST_MARKER = 59201502;

		// Token: 0x040024CF RID: 9423
		private bool hasData;

		// Token: 0x040024D0 RID: 9424
		private DateTime TimeStarted;

		// Token: 0x040024D1 RID: 9425
		private Dictionary<int, KeyValuePair<string, string>> pids = new Dictionary<int, KeyValuePair<string, string>>();

		// Token: 0x040024D2 RID: 9426
		private BinaryWriter bw;

		// Token: 0x040024D3 RID: 9427
		[CompilerGenerated]
		private string <FileName>k__BackingField;

		// Token: 0x020006E5 RID: 1765
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StopRecording>d__15 : IAsyncStateMachine
		{
			// Token: 0x06003C2B RID: 15403 RVA: 0x003185EC File Offset: 0x003167EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (NewStatus != OBDDataReaderStatus.Disconnected)
						{
							goto IL_00C0;
						}
						App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
						DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
						if (recorder == null)
						{
							goto IL_00C0;
						}
						if (recorder != null)
						{
							recorder.Stop();
						}
						App.OBDReader.CurrentCarData.Recorder = null;
						SimpleMainPage.ShowActivityFrame();
						taskAwaiter = Task.Delay(200).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderV2.<StopRecording>d__15>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					SimpleMainPage.HideActivityFrame();
					IL_00C0:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003C2C RID: 15404 RVA: 0x003186F8 File Offset: 0x003168F8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040024D4 RID: 9428
			public int <>1__state;

			// Token: 0x040024D5 RID: 9429
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040024D6 RID: 9430
			public OBDDataReaderStatus NewStatus;

			// Token: 0x040024D7 RID: 9431
			private TaskAwaiter <>u__1;
		}
	}
}
