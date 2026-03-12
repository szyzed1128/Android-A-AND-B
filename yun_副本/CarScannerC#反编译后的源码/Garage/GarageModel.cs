using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Garage
{
	// Token: 0x020004AF RID: 1199
	public class GarageModel
	{
		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x0021328E File Offset: 0x0021148E
		// (set) Token: 0x06002FB8 RID: 12216 RVA: 0x00213296 File Offset: 0x00211496
		public ObservableCollection<MyCar> Cars
		{
			[CompilerGenerated]
			get
			{
				return this.<Cars>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Cars>k__BackingField = value;
			}
		} = new ObservableCollection<MyCar>();

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06002FB9 RID: 12217 RVA: 0x0021329F File Offset: 0x0021149F
		// (set) Token: 0x06002FBA RID: 12218 RVA: 0x002132A7 File Offset: 0x002114A7
		public MyCar CurrentCar
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentCar>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CurrentCar>k__BackingField = value;
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x002132B0 File Offset: 0x002114B0
		public void Initialize(bool saveCurrent = true)
		{
			this.LoadFromFile(saveCurrent);
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06002FBC RID: 12220 RVA: 0x002132B9 File Offset: 0x002114B9
		private string filepath
		{
			get
			{
				return Path.Combine(FileSystemHelper.LocalStoragePath, "garage.json");
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x002132CC File Offset: 0x002114CC
		private void LoadFromFile(bool saveCurrent)
		{
			string localStoragePath = FileSystemHelper.LocalStoragePath;
			if (File.Exists(this.filepath))
			{
				try
				{
					using (FileStream fileStream = File.OpenRead(this.filepath))
					{
						List<MyCar> list = this.DeserializeFromStream<List<MyCar>>(fileStream);
						this.Cars.Clear();
						foreach (MyCar myCar in list)
						{
							this.Cars.Add(myCar);
						}
					}
					if (saveCurrent)
					{
						MyCar myCar2 = this.Cars.FirstOrDefault((MyCar x) => x.Id == SharedSettings.Current.CurrentCarId);
						if (myCar2 != null)
						{
							myCar2.LoadFromCurrentSettings();
							this.CurrentCar = myCar2;
						}
					}
					else
					{
						MyCar car = new MyCar();
						car.Name = Translate.GetString("ios_MY_CAR");
						car.Id = DateTimeNowHelper.NowSafe.Ticks;
						SharedSettings.Current.CurrentCarId = car.Id;
						SharedSettings.Current.CurrentCarName = car.Name;
						if (this.Cars.Any((MyCar x) => x.Name == car.Name))
						{
							car.Name += " (1)";
						}
						car.LoadFromCurrentSettings();
						this.Cars.Add(car);
						this.CurrentCar = car;
					}
					if (saveCurrent)
					{
						this.SaveToFile();
					}
					return;
				}
				catch (Exception ex)
				{
					PCLDebugStream.CurrentInstance.WriteLineAsync(ex.ToString());
					this.CreateGarageWithDefaultCar(saveCurrent);
					return;
				}
			}
			this.CreateGarageWithDefaultCar(saveCurrent);
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x002134E8 File Offset: 0x002116E8
		private void CreateGarageWithDefaultCar(bool saveCurrent)
		{
			this.Cars.Clear();
			MyCar myCar = new MyCar();
			myCar.Id = DateTime.UtcNow.Ticks;
			myCar.Name = Translate.GetString("ios_MY_CAR");
			myCar.LoadFromCurrentSettings();
			this.Cars.Add(myCar);
			this.CurrentCar = myCar;
			SharedSettings.Current.CurrentCarId = myCar.Id;
			SharedSettings.Current.CurrentCarName = myCar.Name;
			if (saveCurrent)
			{
				this.SaveToFile();
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0021356C File Offset: 0x0021176C
		public bool SaveToFile()
		{
			bool flag;
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(this.filepath, false, Encoding.UTF8))
				{
					string text = JsonConvert.SerializeObject(this.Cars);
					streamWriter.Write(text);
					streamWriter.Flush();
				}
				flag = true;
			}
			catch (Exception ex)
			{
				PCLDebugStream.CurrentInstance.WriteLineAsync(ex.ToString());
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x002135E8 File Offset: 0x002117E8
		private T DeserializeFromStream<T>(Stream stream)
		{
			JsonSerializer jsonSerializer = new JsonSerializer();
			T t;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
				{
					t = jsonSerializer.Deserialize<T>(jsonTextReader);
				}
			}
			return t;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x00213644 File Offset: 0x00211844
		public bool RestoreFromBackup()
		{
			try
			{
				string localStoragePath = FileSystemHelper.LocalStoragePath;
				if (File.Exists(this.filepath))
				{
					using (FileStream fileStream = File.OpenRead(this.filepath))
					{
						MyCar myCar = this.DeserializeFromStream<List<MyCar>>(fileStream).FirstOrDefault<MyCar>();
						if (myCar != null)
						{
							myCar.ApplyToSettings();
						}
					}
					return true;
				}
			}
			catch (Exception)
			{
			}
			SimpleMainPage.Instance.UpdateMainButtons();
			return false;
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x002136C8 File Offset: 0x002118C8
		public GarageModel()
		{
		}

		// Token: 0x04001BBF RID: 7103
		[CompilerGenerated]
		private ObservableCollection<MyCar> <Cars>k__BackingField;

		// Token: 0x04001BC0 RID: 7104
		[CompilerGenerated]
		private MyCar <CurrentCar>k__BackingField;

		// Token: 0x04001BC1 RID: 7105
		private const string filename = "garage.json";

		// Token: 0x020004B0 RID: 1200
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002FC3 RID: 12227 RVA: 0x002136DB File Offset: 0x002118DB
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002FC4 RID: 12228 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002FC5 RID: 12229 RVA: 0x002136E7 File Offset: 0x002118E7
			internal bool <LoadFromFile>b__12_0(MyCar x)
			{
				return x.Id == SharedSettings.Current.CurrentCarId;
			}

			// Token: 0x04001BC2 RID: 7106
			public static readonly GarageModel.<>c <>9 = new GarageModel.<>c();

			// Token: 0x04001BC3 RID: 7107
			public static Func<MyCar, bool> <>9__12_0;
		}

		// Token: 0x020004B1 RID: 1201
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06002FC6 RID: 12230 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06002FC7 RID: 12231 RVA: 0x002136FB File Offset: 0x002118FB
			internal bool <LoadFromFile>b__1(MyCar x)
			{
				return x.Name == this.car.Name;
			}

			// Token: 0x04001BC4 RID: 7108
			public MyCar car;
		}
	}
}
