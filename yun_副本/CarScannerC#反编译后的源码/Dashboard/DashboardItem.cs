using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Dashboard.DashboardGauges;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using FFImageLoading.Forms;
using MR.Gestures;
using Sounds.FormsPlugin.Abstractions;
using Syncfusion.XForms.Border;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard
{
	// Token: 0x02000769 RID: 1897
	public class DashboardItem : SfBorder, IDashboardItem, INotifyPropertyChanged
	{
		// Token: 0x0600401A RID: 16410 RVA: 0x00335A3C File Offset: 0x00333C3C
		public DashboardItem()
		{
			DashboardItem.ApplyThemeToItem(this);
			base.BindingContext = this;
			base.SetBinding(SfBorder.BorderWidthProperty, new Binding("FrameSize", 2, null, null, null, null));
			base.SetBinding(SfBorder.BorderColorProperty, new Binding("FrameColor", 2, null, null, null, null));
			base.Padding = 0.0;
			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.NumberOfTapsRequired = 2;
			tapGestureRecognizer.Tapped += this.TapGestureRecognizer_Tapped;
			base.GestureRecognizers.Add(tapGestureRecognizer);
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.NumberOfTapsRequired = 1;
			tapGestureRecognizer.Tapped += this.TapSingleGestureRecognizer_Tapped;
			base.GestureRecognizers.Add(tapGestureRecognizer);
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00335E04 File Offset: 0x00334004
		private void TapSingleGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			DashboardXamlPage dashboardXamlPage = App.GetCurrentPage() as DashboardXamlPage;
			if (dashboardXamlPage != null && dashboardXamlPage != null)
			{
				dashboardXamlPage.ShowTopGrid();
			}
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x00335E2C File Offset: 0x0033402C
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			if (base.Parent is AbsoluteLayout)
			{
				this.ShowPopup();
				return;
			}
			if (DashboardXamlPage.Instance != null && App.GetCurrentPage() == DashboardXamlPage.Instance)
			{
				Page page = new DashboardItemEditorV3(this, base.Width, base.Height);
				if (App.UseLegacyUI && PlatformHelper.IsiOS)
				{
					page = new DashboardItemEditor(this, base.Width, base.Height);
				}
				DashboardXamlPage.Instance.Navigation.PushAsync(page, true);
			}
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x00335EA8 File Offset: 0x003340A8
		private void double_Tapped(object sender, EventArgs e)
		{
			if (base.Navigation.NavigationStack.LastOrDefault<Page>() is DashboardXamlPage)
			{
				Page page = new DashboardItemEditorV3(this, base.Width, base.Height);
				if (App.UseLegacyUI && PlatformHelper.IsiOS)
				{
					page = new DashboardItemEditor(this, base.Width, base.Height);
				}
				base.Navigation.PushAsync(page, true);
			}
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x00335F10 File Offset: 0x00334110
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			try
			{
				this.OnPropertyChanged(PropertyName);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x170014CA RID: 5322
		// (get) Token: 0x0600401F RID: 16415 RVA: 0x00335F3C File Offset: 0x0033413C
		// (set) Token: 0x06004020 RID: 16416 RVA: 0x00335F44 File Offset: 0x00334144
		public bool GaugeShowRedLine
		{
			get
			{
				return this._GaugeShowRedLine;
			}
			set
			{
				this._GaugeShowRedLine = value;
				if (!value)
				{
					this.GaugeRedLineStart = this.Maximum;
					this.GaugeRedLineFinish = this.Maximum;
				}
				this.NotifyPropertyChanged("GaugeShowRedLine");
			}
		}

		// Token: 0x170014CB RID: 5323
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x00335F73 File Offset: 0x00334173
		// (set) Token: 0x06004022 RID: 16418 RVA: 0x00335F7B File Offset: 0x0033417B
		public double GaugeRedLineStart
		{
			get
			{
				return this._GaugeRedLineStart;
			}
			set
			{
				this._GaugeRedLineStart = value;
				this.NotifyPropertyChanged("GaugeRedLineStart");
			}
		}

		// Token: 0x170014CC RID: 5324
		// (get) Token: 0x06004023 RID: 16419 RVA: 0x00335F8F File Offset: 0x0033418F
		// (set) Token: 0x06004024 RID: 16420 RVA: 0x00335F97 File Offset: 0x00334197
		public double GaugeRedLineFinish
		{
			get
			{
				return this._GaugeRedLineFinish;
			}
			set
			{
				this._GaugeRedLineFinish = value;
				this.NotifyPropertyChanged("GaugeRedLineFinish");
			}
		}

		// Token: 0x170014CD RID: 5325
		// (get) Token: 0x06004025 RID: 16421 RVA: 0x00335FAB File Offset: 0x003341AB
		// (set) Token: 0x06004026 RID: 16422 RVA: 0x00335FB3 File Offset: 0x003341B3
		public Color GaugeRedLineColor
		{
			get
			{
				return this._GaugeRedLineColor;
			}
			set
			{
				this._GaugeRedLineColor = value;
				this.NotifyPropertyChanged("GaugeRedLineColor");
			}
		}

		// Token: 0x170014CE RID: 5326
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x00335FC7 File Offset: 0x003341C7
		// (set) Token: 0x06004028 RID: 16424 RVA: 0x00335FCF File Offset: 0x003341CF
		public bool ShowDefaultBackground
		{
			get
			{
				return this._ShowDefaultBackground;
			}
			set
			{
				this._ShowDefaultBackground = value;
				this.NotifyPropertyChanged("ShowDefaultBackground");
			}
		}

		// Token: 0x170014CF RID: 5327
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x00335FE3 File Offset: 0x003341E3
		// (set) Token: 0x0600402A RID: 16426 RVA: 0x00335FEB File Offset: 0x003341EB
		public double FrameSize
		{
			get
			{
				return this._FrameSize;
			}
			set
			{
				this._FrameSize = value;
				this.NotifyPropertyChanged("FrameSize");
			}
		}

		// Token: 0x170014D0 RID: 5328
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x00335FFF File Offset: 0x003341FF
		// (set) Token: 0x0600402C RID: 16428 RVA: 0x00336007 File Offset: 0x00334207
		public Color FrameColor
		{
			get
			{
				return this._FrameColor;
			}
			set
			{
				this._FrameColor = value;
				this.NotifyPropertyChanged("FrameColor");
			}
		}

		// Token: 0x170014D1 RID: 5329
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x0033601B File Offset: 0x0033421B
		// (set) Token: 0x0600402E RID: 16430 RVA: 0x00336023 File Offset: 0x00334223
		public Color TitleTextColor
		{
			get
			{
				return this._TitleTextColor;
			}
			set
			{
				this._TitleTextColor = value;
				this.NotifyPropertyChanged("TitleTextColor");
			}
		}

		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x0600402F RID: 16431 RVA: 0x00336037 File Offset: 0x00334237
		// (set) Token: 0x06004030 RID: 16432 RVA: 0x0033603F File Offset: 0x0033423F
		public double TitleFontSize
		{
			get
			{
				return this._TitleFontSize;
			}
			set
			{
				this._TitleFontSize = value;
				this.NotifyPropertyChanged("TitleFontSize");
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06004031 RID: 16433 RVA: 0x00336053 File Offset: 0x00334253
		// (set) Token: 0x06004032 RID: 16434 RVA: 0x0033605B File Offset: 0x0033425B
		public Color ValueNormalTextColor
		{
			get
			{
				return this._ValueNormalTextColor;
			}
			set
			{
				if (this._ValueNormalTextColor == value)
				{
					return;
				}
				this._ValueNormalTextColor = value;
				this.NotifyPropertyChanged("ValueNormalTextColor");
				this.UpdateValueTextColor();
			}
		}

		// Token: 0x06004033 RID: 16435 RVA: 0x00336084 File Offset: 0x00334284
		private void UpdateValueTextColor()
		{
			if (this.Model == null)
			{
				this.ValueTextColor = this.ValueNormalTextColor;
				return;
			}
			if (this.ShowLowWarning && this.Model.FloatValue < this.LowWarningStart)
			{
				this.ValueTextColor = this.LowWarningColor;
				return;
			}
			if (this.ShowHighWarning && this.Model.FloatValue > this.HighWarningStart)
			{
				this.ValueTextColor = this.HighWarningColor;
				return;
			}
			this.ValueTextColor = this.ValueNormalTextColor;
		}

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x00336102 File Offset: 0x00334302
		// (set) Token: 0x06004035 RID: 16437 RVA: 0x0033610A File Offset: 0x0033430A
		public Color ValueTextColor
		{
			get
			{
				return this._ValueTextColor;
			}
			set
			{
				if (value == this._ValueTextColor)
				{
					return;
				}
				this._ValueTextColor = value;
				this.OnPropertyChanged("ValueTextColor");
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06004036 RID: 16438 RVA: 0x0033612D File Offset: 0x0033432D
		// (set) Token: 0x06004037 RID: 16439 RVA: 0x00336135 File Offset: 0x00334335
		public double ValueFontSize
		{
			get
			{
				return this._ValueFontSize;
			}
			set
			{
				this._ValueFontSize = value;
				this.NotifyPropertyChanged("ValueFontSize");
				if (PlatformHelper.IsiOS && this.ItemType == DashboardItemTypes.Gauge && this.ValueUseLCDFont)
				{
					this.NotifyPropertyChanged("FontName");
				}
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x0033616C File Offset: 0x0033436C
		// (set) Token: 0x06004039 RID: 16441 RVA: 0x00336174 File Offset: 0x00334374
		public Color UnitsTextColor
		{
			get
			{
				return this._UnitsTextColor;
			}
			set
			{
				this._UnitsTextColor = value;
				this.NotifyPropertyChanged("UnitsTextColor");
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x0600403A RID: 16442 RVA: 0x00336188 File Offset: 0x00334388
		// (set) Token: 0x0600403B RID: 16443 RVA: 0x00336190 File Offset: 0x00334390
		public double UnitsFontSize
		{
			get
			{
				return this._UnitsFontSize;
			}
			set
			{
				this._UnitsFontSize = value;
				this.NotifyPropertyChanged("UnitsFontSize");
			}
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x0600403C RID: 16444 RVA: 0x003361A4 File Offset: 0x003343A4
		// (set) Token: 0x0600403D RID: 16445 RVA: 0x003361AC File Offset: 0x003343AC
		public Color GaugeLabelColor
		{
			get
			{
				return this._GaugeLabelColor;
			}
			set
			{
				this._GaugeLabelColor = value;
				this.NotifyPropertyChanged("GaugeLabelColor");
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x0600403E RID: 16446 RVA: 0x003361C0 File Offset: 0x003343C0
		// (set) Token: 0x0600403F RID: 16447 RVA: 0x003361C8 File Offset: 0x003343C8
		public Color GaugeRimColor
		{
			get
			{
				return this._GaugeRimColor;
			}
			set
			{
				this._GaugeRimColor = value;
				this.NotifyPropertyChanged("GaugeRimColor");
			}
		}

		// Token: 0x170014DA RID: 5338
		// (get) Token: 0x06004040 RID: 16448 RVA: 0x003361DC File Offset: 0x003343DC
		// (set) Token: 0x06004041 RID: 16449 RVA: 0x003361EA File Offset: 0x003343EA
		public Color GaugeTickColor
		{
			get
			{
				bool isiOS = PlatformHelper.IsiOS;
				return this._GaugeTickColor;
			}
			set
			{
				this._GaugeTickColor = value;
				this.NotifyPropertyChanged("GaugeTickColor");
				if (!PlatformHelper.IsiOS)
				{
					this.NotifyPropertyChanged("GaugeTickColor");
				}
			}
		}

		// Token: 0x170014DB RID: 5339
		// (get) Token: 0x06004042 RID: 16450 RVA: 0x00336210 File Offset: 0x00334410
		// (set) Token: 0x06004043 RID: 16451 RVA: 0x00336218 File Offset: 0x00334418
		public Color GaugePointerColor
		{
			get
			{
				return this._GaugePointerColor;
			}
			set
			{
				this._GaugePointerColor = value;
				this.NotifyPropertyChanged("GaugePointerColor");
			}
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x06004044 RID: 16452 RVA: 0x0033622C File Offset: 0x0033442C
		// (set) Token: 0x06004045 RID: 16453 RVA: 0x00336234 File Offset: 0x00334434
		public Color ChartLineColor
		{
			get
			{
				return this._ChartLineColor;
			}
			set
			{
				this._ChartLineColor = value;
			}
		}

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x06004046 RID: 16454 RVA: 0x0033623D File Offset: 0x0033443D
		// (set) Token: 0x06004047 RID: 16455 RVA: 0x00336245 File Offset: 0x00334445
		public bool ChartValuePositionCenter
		{
			get
			{
				return this._ChartValuePositionCenter;
			}
			set
			{
				this._ChartValuePositionCenter = value;
				this.OnPropertyChanged("ChartValuePositionCenter");
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x06004048 RID: 16456 RVA: 0x00336259 File Offset: 0x00334459
		// (set) Token: 0x06004049 RID: 16457 RVA: 0x00336261 File Offset: 0x00334461
		public int ChartLineWidth
		{
			get
			{
				return this._ChartLineWidth;
			}
			set
			{
				this._ChartLineWidth = value;
				this.OnPropertyChanged("ChartLineWidth");
			}
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x0600404A RID: 16458 RVA: 0x00336275 File Offset: 0x00334475
		// (set) Token: 0x0600404B RID: 16459 RVA: 0x0033627D File Offset: 0x0033447D
		public Color GaugeKnobColor
		{
			get
			{
				return this._GaugeKnobColor;
			}
			set
			{
				this._GaugeKnobColor = value;
				this.NotifyPropertyChanged("GaugeKnobColor");
			}
		}

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x0600404C RID: 16460 RVA: 0x00336291 File Offset: 0x00334491
		// (set) Token: 0x0600404D RID: 16461 RVA: 0x00336299 File Offset: 0x00334499
		public double LinearScaleSize
		{
			get
			{
				return this._LinearScaleSize;
			}
			set
			{
				this._LinearScaleSize = value;
				this.NotifyPropertyChanged("LinearScaleSize");
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x003362AD File Offset: 0x003344AD
		// (set) Token: 0x0600404F RID: 16463 RVA: 0x003362B5 File Offset: 0x003344B5
		public bool ShowValue
		{
			get
			{
				return this._ShowValue;
			}
			set
			{
				this._ShowValue = value;
				this.OnPropertyChanged("ShowValue");
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x06004050 RID: 16464 RVA: 0x003362C9 File Offset: 0x003344C9
		// (set) Token: 0x06004051 RID: 16465 RVA: 0x003362D1 File Offset: 0x003344D1
		public bool LinearOrientationHorizontal
		{
			get
			{
				return this._LinearOrientationHorizontal;
			}
			set
			{
				this._LinearOrientationHorizontal = value;
				this.OnPropertyChanged("LinearOrientationHorizontal");
			}
		}

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x06004052 RID: 16466 RVA: 0x003362E5 File Offset: 0x003344E5
		// (set) Token: 0x06004053 RID: 16467 RVA: 0x003362ED File Offset: 0x003344ED
		public bool OverrideName
		{
			get
			{
				return this._OverrideName;
			}
			set
			{
				this._OverrideName = value;
				this.OnPropertyChanged("OverrideName");
				this.OnPropertyChanged("PIDName");
			}
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x06004054 RID: 16468 RVA: 0x0033630C File Offset: 0x0033450C
		// (set) Token: 0x06004055 RID: 16469 RVA: 0x00336322 File Offset: 0x00334522
		public string CustomName
		{
			get
			{
				if (this._CustomName == null)
				{
					return "";
				}
				return this._CustomName;
			}
			set
			{
				this._CustomName = value;
				this.OnPropertyChanged("CustomName");
				this.OnPropertyChanged("PIDName");
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x06004056 RID: 16470 RVA: 0x00336341 File Offset: 0x00334541
		public string PIDName
		{
			get
			{
				if (this.OverrideName)
				{
					return this.CustomName;
				}
				if (this.Model != null && this.Model.SelectedPID != null)
				{
					return this.Model.SelectedPID.ShortName;
				}
				return "";
			}
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x0033637D File Offset: 0x0033457D
		// (set) Token: 0x06004058 RID: 16472 RVA: 0x00336385 File Offset: 0x00334585
		public bool PlaySound
		{
			get
			{
				return this._PlaySound;
			}
			set
			{
				this._PlaySound = value;
				this.NotifyPropertyChanged("PlaySound");
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x00336399 File Offset: 0x00334599
		// (set) Token: 0x0600405A RID: 16474 RVA: 0x003363A1 File Offset: 0x003345A1
		public string SoundName
		{
			get
			{
				return this._SoundName;
			}
			set
			{
				this._SoundName = value;
				if (this.player != null)
				{
					this.player.Filename = value;
				}
				this.NotifyPropertyChanged("SoundName");
			}
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x003363C9 File Offset: 0x003345C9
		// (set) Token: 0x0600405C RID: 16476 RVA: 0x003363D1 File Offset: 0x003345D1
		public double SoundStart
		{
			get
			{
				return this._SoundStart;
			}
			set
			{
				this._SoundStart = value;
				this.NotifyPropertyChanged("SoundStart");
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x003363E5 File Offset: 0x003345E5
		// (set) Token: 0x0600405E RID: 16478 RVA: 0x003363ED File Offset: 0x003345ED
		public bool PlaySoundLow
		{
			get
			{
				return this._PlaySoundLow;
			}
			set
			{
				this._PlaySoundLow = value;
				this.NotifyPropertyChanged("PlaySoundLow");
			}
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x00336401 File Offset: 0x00334601
		// (set) Token: 0x06004060 RID: 16480 RVA: 0x00336409 File Offset: 0x00334609
		public string SoundNameLow
		{
			get
			{
				return this._SoundNameLow;
			}
			set
			{
				this._SoundNameLow = value;
				if (this.playerLow != null)
				{
					this.playerLow.Filename = value;
				}
				this.NotifyPropertyChanged("SoundNameLow");
			}
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x06004061 RID: 16481 RVA: 0x00336431 File Offset: 0x00334631
		// (set) Token: 0x06004062 RID: 16482 RVA: 0x00336439 File Offset: 0x00334639
		public double SoundStartLow
		{
			get
			{
				return this._SoundStartLow;
			}
			set
			{
				this._SoundStartLow = value;
				this.NotifyPropertyChanged("SoundStartLow");
			}
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x06004063 RID: 16483 RVA: 0x0033644D File Offset: 0x0033464D
		// (set) Token: 0x06004064 RID: 16484 RVA: 0x00336458 File Offset: 0x00334658
		public DashboardItemTypes ItemType
		{
			get
			{
				return this._ItemType;
			}
			set
			{
				this._ItemType = value;
				this.NotifyPropertyChanged("ItemType");
				this.NotifyPropertyChanged("CanChangeFont");
				if (this.Model != null)
				{
					if (this.Model is MultiplePIDViewModel && this._ItemType != DashboardItemTypes.MultiPidBarChart && this._ItemType != DashboardItemTypes.MultiChart)
					{
						this.Model.Unsubscribe();
						this.Model = new LiveDataPIDModel();
						this.Start();
					}
					else if (!(this.Model is MultiplePIDViewModel) && (this._ItemType == DashboardItemTypes.MultiPidBarChart || this._ItemType == DashboardItemTypes.MultiChart))
					{
						this.Model.Unsubscribe();
						MultiplePIDViewModel multiplePIDViewModel = new MultiplePIDViewModel();
						multiplePIDViewModel.SetPIDs(this.PID_IDs.ToList<int>());
						this.Model = multiplePIDViewModel;
					}
				}
				this.SelectAndAddControl();
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06004065 RID: 16485 RVA: 0x0033651A File Offset: 0x0033471A
		// (set) Token: 0x06004066 RID: 16486 RVA: 0x00336547 File Offset: 0x00334747
		public double Maximum
		{
			get
			{
				if (this.ItemType == DashboardItemTypes.Chart && !this.UseCustomMinMax && this.Model != null)
				{
					return this.Model.Maximum;
				}
				return this._Maximum;
			}
			set
			{
				this._Maximum = value;
				this.NotifyPropertyChanged("Maximum");
				this.NotifyPropertyChanged("Interval");
				this.NotifyPropertyChanged("GaugeLabelNumberOfDecimalDigits");
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06004067 RID: 16487 RVA: 0x00336571 File Offset: 0x00334771
		// (set) Token: 0x06004068 RID: 16488 RVA: 0x0033659E File Offset: 0x0033479E
		public double Minimum
		{
			get
			{
				if (this.ItemType == DashboardItemTypes.Chart && !this.UseCustomMinMax && this.Model != null)
				{
					return this.Model.Minimum;
				}
				return this._Minimum;
			}
			set
			{
				this._Minimum = value;
				this.NotifyPropertyChanged("Minimum");
				this.NotifyPropertyChanged("Interval");
				this.NotifyPropertyChanged("GaugeLabelNumberOfDecimalDigits");
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06004069 RID: 16489 RVA: 0x003365C8 File Offset: 0x003347C8
		// (set) Token: 0x0600406A RID: 16490 RVA: 0x003365D0 File Offset: 0x003347D0
		public bool UseCustomMinMax
		{
			get
			{
				return this._UseCustomMinMax;
			}
			set
			{
				this._UseCustomMinMax = value;
				this.NotifyPropertyChanged("UseCustomMinMax");
				this.NotifyPropertyChanged("Minimum");
				this.NotifyPropertyChanged("Maximum");
				this.NotifyPropertyChanged("Interval");
			}
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x0600406B RID: 16491 RVA: 0x00336605 File Offset: 0x00334805
		// (set) Token: 0x0600406C RID: 16492 RVA: 0x00336610 File Offset: 0x00334810
		public LiveDataPIDModel Model
		{
			get
			{
				return this._Model;
			}
			set
			{
				if (this._Model != null)
				{
					this._Model.PropertyChanged -= this.ModelPropertyChanged;
				}
				this._Model = value;
				if (value == null)
				{
					return;
				}
				this._Model.SetMinMaxAvgOnlyVisibleArea = this.SetMinMaxAvgOnlyVisibleArea;
				this._Model.PropertyChanged -= this.ModelPropertyChanged;
				this._Model.PropertyChanged += this.ModelPropertyChanged;
				this._Model.LiveDataShowTime = this.LiveDataShowTime;
				this.OnPropertyChanged("Model");
				this.OnPropertyChanged("Minimum");
				this.OnPropertyChanged("Maximum");
			}
		}

		// Token: 0x0600406D RID: 16493 RVA: 0x003366B8 File Offset: 0x003348B8
		private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!(e.PropertyName == "FloatValue"))
			{
				if (this.ItemType == DashboardItemTypes.Chart && !this.UseCustomMinMax)
				{
					if (e.PropertyName == "Minimum")
					{
						this.Minimum = this.Model.Minimum;
					}
					if (e.PropertyName == "Maximum")
					{
						this.Maximum = this.Model.Maximum;
					}
				}
				return;
			}
			this.float_change_counter += 1U;
			if (double.IsNaN(this.last_float_value))
			{
				this.last_float_value = this.Model.FloatValue;
				return;
			}
			this.UpdateIndicatorBackgroundColor();
			this.UpdateValueTextColor();
			if (this.PlaySound && this.last_float_value < this.SoundStart && this.Model.FloatValue >= this.SoundStart)
			{
				if (this.player == null)
				{
					this.player = PlatformHelper.CommonService.SoundManager;
					this.player.Filename = this.SoundName;
				}
				if (!this.player.IsPlaying)
				{
					this.player.Play();
				}
			}
			if (this.PlaySoundLow && this.last_float_value > this.SoundStartLow && this.Model.FloatValue <= this.SoundStartLow)
			{
				if (this.playerLow == null)
				{
					this.player = PlatformHelper.CommonService.SoundManager;
					this.playerLow.Filename = this.SoundNameLow;
				}
				if (!this.playerLow.IsPlaying)
				{
					this.playerLow.Play();
				}
			}
			this.last_float_value = this.Model.FloatValue;
		}

		// Token: 0x170014F1 RID: 5361
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x00336852 File Offset: 0x00334A52
		// (set) Token: 0x0600406F RID: 16495 RVA: 0x0033685C File Offset: 0x00334A5C
		public int PID_Id
		{
			get
			{
				return this._PID_Id;
			}
			set
			{
				if (this._PID_Id == 0)
				{
					this._PID_Id = value;
					if (this._PID_Id < 1001)
					{
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => this.PID_Id == x.Id);
					}
					else if (CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id) == null)
					{
						CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id);
					}
				}
				else
				{
					if (this._PID_Id == value)
					{
						if (Math.Abs(this.Maximum - this.Minimum) < 1E-05)
						{
							this._PID_Id = value;
							PID pid;
							if (this._PID_Id < 1001)
							{
								pid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => this.PID_Id == x.Id);
							}
							else
							{
								pid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id);
								if (pid == null)
								{
									pid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id);
								}
							}
							if (pid != null)
							{
								this.Maximum = pid.Maximum;
								this.Minimum = pid.Minimum;
							}
						}
						return;
					}
					this._PID_Id = value;
					PID pid2;
					if (this._PID_Id < 1001)
					{
						pid2 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => this.PID_Id == x.Id);
					}
					else
					{
						pid2 = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id);
						if (pid2 == null)
						{
							pid2 = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => this.PID_Id == x.Id);
						}
					}
					if (pid2 != null)
					{
						this.Maximum = pid2.Maximum;
						this.Minimum = pid2.Minimum;
						if (this.GaugeShowRedLine)
						{
							double num = this.Maximum - this.Minimum;
							this.GaugeRedLineStart = 0.85 * num + this.Minimum;
							this.GaugeRedLineFinish = this.Maximum;
						}
					}
				}
				this.NotifyPropertyChanged("PID_Id");
			}
		}

		// Token: 0x170014F2 RID: 5362
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x00336A78 File Offset: 0x00334C78
		// (set) Token: 0x06004071 RID: 16497 RVA: 0x00336A80 File Offset: 0x00334C80
		public List<int> PID_IDs
		{
			get
			{
				return this._PID_IDs;
			}
			set
			{
				this._PID_IDs = value;
				if (this.Model != null)
				{
					MultiplePIDViewModel multiplePIDViewModel = this.Model as MultiplePIDViewModel;
					if (multiplePIDViewModel != null)
					{
						multiplePIDViewModel.SetPIDs(this._PID_IDs);
					}
				}
			}
		}

		// Token: 0x170014F3 RID: 5363
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x00336AB7 File Offset: 0x00334CB7
		public double Interval
		{
			get
			{
				if (this.UseCustomInterval)
				{
					return this.CustomInterval;
				}
				return this.GetInterval();
			}
		}

		// Token: 0x170014F4 RID: 5364
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x00336ACE File Offset: 0x00334CCE
		// (set) Token: 0x06004074 RID: 16500 RVA: 0x00336AD6 File Offset: 0x00334CD6
		public bool ValueUseLCDFont
		{
			get
			{
				return this._ValueUseLCDFont;
			}
			set
			{
				if (this._ValueUseLCDFont != value)
				{
					this._ValueUseLCDFont = value;
					this.NotifyPropertyChanged("ValueUseLCDFont");
					this.NotifyPropertyChanged("FontName");
				}
			}
		}

		// Token: 0x170014F5 RID: 5365
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x00336AFE File Offset: 0x00334CFE
		// (set) Token: 0x06004076 RID: 16502 RVA: 0x00336B06 File Offset: 0x00334D06
		public int ValueFormat
		{
			get
			{
				return this._ValueFormat;
			}
			set
			{
				this._ValueFormat = value;
				if (this.Model != null)
				{
					this.Model.DoubleFormat = value;
				}
			}
		}

		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06004077 RID: 16503 RVA: 0x00336B23 File Offset: 0x00334D23
		public bool CanChangeFont
		{
			get
			{
				return this.ItemType != DashboardItemTypes.Gauge;
			}
		}

		// Token: 0x170014F7 RID: 5367
		// (get) Token: 0x06004078 RID: 16504 RVA: 0x00336B34 File Offset: 0x00334D34
		public string FontName
		{
			get
			{
				if (!this.ValueUseLCDFont)
				{
					return Font.Default.FontFamily;
				}
				if (Device.RuntimePlatform == "iOS")
				{
					return "LCD2";
				}
				if (!(Device.RuntimePlatform == "Android"))
				{
					return Font.Default.FontFamily;
				}
				if (this.ItemType == DashboardItemTypes.Gauge || this.ItemType == DashboardItemTypes.GaugeVar2 || this.ItemType == DashboardItemTypes.GaugeVar3)
				{
					return "lcd2b.ttf";
				}
				return "lcd2b.ttf#LCD2";
			}
		}

		// Token: 0x170014F8 RID: 5368
		// (get) Token: 0x06004079 RID: 16505 RVA: 0x00336BB3 File Offset: 0x00334DB3
		// (set) Token: 0x0600407A RID: 16506 RVA: 0x00336BBB File Offset: 0x00334DBB
		public double DesiredWidth
		{
			get
			{
				return this._DesiredWidth;
			}
			set
			{
				this._DesiredWidth = value;
				this.NotifyPropertyChanged("DesiredWidth");
			}
		}

		// Token: 0x170014F9 RID: 5369
		// (get) Token: 0x0600407B RID: 16507 RVA: 0x00336BCF File Offset: 0x00334DCF
		// (set) Token: 0x0600407C RID: 16508 RVA: 0x00336BD7 File Offset: 0x00334DD7
		public double DesiredHeight
		{
			get
			{
				return this._DesiredHeight;
			}
			set
			{
				this._DesiredHeight = value;
				this.NotifyPropertyChanged("DesiredHeight");
			}
		}

		// Token: 0x170014FA RID: 5370
		// (get) Token: 0x0600407D RID: 16509 RVA: 0x00336BEB File Offset: 0x00334DEB
		// (set) Token: 0x0600407E RID: 16510 RVA: 0x00336BF3 File Offset: 0x00334DF3
		public double PositionX
		{
			get
			{
				return this._PositionX;
			}
			set
			{
				this._PositionX = value;
				this.NotifyPropertyChanged("PositionX");
			}
		}

		// Token: 0x170014FB RID: 5371
		// (get) Token: 0x0600407F RID: 16511 RVA: 0x00336C07 File Offset: 0x00334E07
		// (set) Token: 0x06004080 RID: 16512 RVA: 0x00336C0F File Offset: 0x00334E0F
		public double PositionY
		{
			get
			{
				return this._PositionY;
			}
			set
			{
				this._PositionY = value;
				this.NotifyPropertyChanged("PositionY");
			}
		}

		// Token: 0x170014FC RID: 5372
		// (get) Token: 0x06004081 RID: 16513 RVA: 0x00336C23 File Offset: 0x00334E23
		public double GaugeLabelNumberOfDecimalDigits
		{
			get
			{
				if (Math.Abs(this.Minimum - this.Maximum) <= 2.0)
				{
					return 1.0;
				}
				return 0.0;
			}
		}

		// Token: 0x170014FD RID: 5373
		// (get) Token: 0x06004082 RID: 16514 RVA: 0x00336C55 File Offset: 0x00334E55
		// (set) Token: 0x06004083 RID: 16515 RVA: 0x00336C5D File Offset: 0x00334E5D
		public double MinMaxAvgFontSize
		{
			get
			{
				return this._MinMaxAvgFontSize;
			}
			set
			{
				this._MinMaxAvgFontSize = value;
				this.NotifyPropertyChanged("MinMaxAvgFontSize");
			}
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x06004084 RID: 16516 RVA: 0x00336C71 File Offset: 0x00334E71
		// (set) Token: 0x06004085 RID: 16517 RVA: 0x00336C79 File Offset: 0x00334E79
		public bool ShowMinMax
		{
			get
			{
				return this._ShowMinMax;
			}
			set
			{
				if (value != this._ShowMinMax)
				{
					this._ShowMinMax = value;
					this.NotifyPropertyChanged("ShowMinMax");
					this.NotifyPropertyChanged("MinMaxAvgColor");
				}
			}
		}

		// Token: 0x170014FF RID: 5375
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x00336CA1 File Offset: 0x00334EA1
		// (set) Token: 0x06004087 RID: 16519 RVA: 0x00336CA9 File Offset: 0x00334EA9
		public bool ShowMinMaxPointers
		{
			get
			{
				return this._ShowMinMaxPointers;
			}
			set
			{
				if (value != this._ShowMinMaxPointers)
				{
					this._ShowMinMaxPointers = value;
					this.NotifyPropertyChanged("ShowMinMaxPointers");
					this.NotifyPropertyChanged("MinMaxPointersColor");
				}
			}
		}

		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06004088 RID: 16520 RVA: 0x00336CD1 File Offset: 0x00334ED1
		// (set) Token: 0x06004089 RID: 16521 RVA: 0x00336CD9 File Offset: 0x00334ED9
		public Color MinMaxPointersColor
		{
			get
			{
				return this._MinMaxPointersColor;
			}
			set
			{
				this._MinMaxPointersColor = value;
				this.NotifyPropertyChanged("MinMaxPointersColor");
			}
		}

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x0600408A RID: 16522 RVA: 0x00336CED File Offset: 0x00334EED
		// (set) Token: 0x0600408B RID: 16523 RVA: 0x00336CF5 File Offset: 0x00334EF5
		public bool ShowAvg
		{
			get
			{
				return this._ShowAvg;
			}
			set
			{
				if (value != this._ShowAvg)
				{
					this._ShowAvg = value;
					this.NotifyPropertyChanged("ShowAvg");
				}
			}
		}

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x00336D12 File Offset: 0x00334F12
		// (set) Token: 0x0600408D RID: 16525 RVA: 0x00336D1A File Offset: 0x00334F1A
		public Color MinMaxAvgColor
		{
			get
			{
				return this._MinMaxAvgColor;
			}
			set
			{
				this._MinMaxAvgColor = value;
				this.NotifyPropertyChanged("MinMaxAvgColor");
			}
		}

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600408E RID: 16526 RVA: 0x00336D2E File Offset: 0x00334F2E
		// (set) Token: 0x0600408F RID: 16527 RVA: 0x00336D36 File Offset: 0x00334F36
		public bool SetMinMaxAvgOnlyVisibleArea
		{
			get
			{
				return this._SetMinMaxAvgOnlyVisibleArea;
			}
			set
			{
				if (this._SetMinMaxAvgOnlyVisibleArea != value)
				{
					this._SetMinMaxAvgOnlyVisibleArea = value;
					this.NotifyPropertyChanged("SetMinMaxAvgOnlyVisibleArea");
					if (this.Model != null)
					{
						this.Model.SetMinMaxAvgOnlyVisibleArea = value;
					}
				}
			}
		}

		// Token: 0x06004090 RID: 16528 RVA: 0x00336D68 File Offset: 0x00334F68
		public void Start()
		{
			PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x != null && x.Id == this.PID_Id);
			if (string.IsNullOrEmpty(this.CustomName) && pid == null)
			{
				this.CustomName = PID.Empty.Name;
			}
			this.Model.Unsubscribe();
			if (this.Model.SelectedPID == pid)
			{
				this.Model.Unsubscribe();
				this.Model.Subscribe();
			}
			else
			{
				this.Model.SelectedPID = pid;
			}
			this.float_change_counter = 0U;
			if (this.ItemType == DashboardItemTypes.Chart)
			{
				this.Model.Mode = LiveDataModes.DashboardChart;
			}
			else
			{
				this.Model.Mode = LiveDataModes.Dashboard;
			}
			base.Content.BindingContext = this;
			this.OnPropertyChanged("PIDName");
			this.UpdateValueTextColor();
			this.UpdateIndicatorBackgroundColor();
		}

		// Token: 0x06004091 RID: 16529 RVA: 0x00336E36 File Offset: 0x00335036
		public void Stop()
		{
			this.Model.Unsubscribe();
		}

		// Token: 0x06004092 RID: 16530 RVA: 0x00336E44 File Offset: 0x00335044
		private double GetInterval()
		{
			double num = Math.Abs(this.Minimum - this.Maximum);
			double num2;
			if (0.0 < num && num <= 1.0)
			{
				num2 = 0.1;
			}
			else if (1.0 < num && num <= 2.0)
			{
				num2 = 0.2;
			}
			else if (2.0 < num && num <= 10.0)
			{
				num2 = 1.0;
			}
			else if (10.0 < num && num <= 20.0)
			{
				num2 = 2.0;
			}
			else if (20.0 < num && num <= 50.0)
			{
				num2 = 5.0;
			}
			else if (50.0 < num && num <= 100.0)
			{
				num2 = 10.0;
			}
			else if (100.0 < num && num <= 220.0)
			{
				num2 = 20.0;
			}
			else if (220.0 < num && num <= 500.0)
			{
				num2 = 50.0;
			}
			else if (500.0 < num && num <= 1000.0)
			{
				num2 = 100.0;
			}
			else if (1000.0 < num && num <= 2000.0)
			{
				num2 = 200.0;
			}
			else if (2000.0 < num && num <= 10000.0)
			{
				num2 = 1000.0;
			}
			else if (10000.0 < num && num <= 20000.0)
			{
				num2 = 2000.0;
			}
			else if (20000.0 < num && num <= 100000.0)
			{
				num2 = 10000.0;
			}
			else
			{
				num2 = num / 10.0;
			}
			if (this.ItemType == DashboardItemTypes.LinearGauge)
			{
				num2 *= 2.0;
			}
			return num2;
		}

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x00337075 File Offset: 0x00335275
		// (set) Token: 0x06004094 RID: 16532 RVA: 0x00337080 File Offset: 0x00335280
		public ChartItemTypes ChartItemType
		{
			get
			{
				return this._ChartItemType;
			}
			set
			{
				if (value != this._ChartItemType)
				{
					this._ChartItemType = value;
					ChartItem chartItem = base.Content as ChartItem;
					if (chartItem != null)
					{
						chartItem.UpdateSeries(value);
					}
					this.OnPropertyChanged("ChartItemType");
				}
			}
		}

		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x003370BE File Offset: 0x003352BE
		// (set) Token: 0x06004096 RID: 16534 RVA: 0x003370C8 File Offset: 0x003352C8
		public int LiveDataShowTime
		{
			get
			{
				return this._LiveDataShowTime;
			}
			set
			{
				this._LiveDataShowTime = value;
				this.OnPropertyChanged("LiveDataShowTime");
				LiveDataPIDModel model = this.Model;
				if (model != null)
				{
					model.LiveDataShowTime = value;
				}
			}
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x003370F8 File Offset: 0x003352F8
		public void SelectAndAddControl()
		{
			try
			{
				switch (this.ItemType)
				{
				case DashboardItemTypes.Text:
					base.Content = new SimpleTextItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.Chart:
					base.Content = new ChartItem(this.ChartItemType);
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.DashboardChart;
					}
					break;
				case DashboardItemTypes.Gauge:
					if (this.Minimum == this.Maximum)
					{
						this.Minimum = 0.0;
						this.Maximum = 100.0;
					}
					base.Content = new GaugeItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.Action:
					base.Content = new ActionItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.LinearGauge:
					base.Content = new LinearProgressItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.TextHorizontal:
					base.Content = new TextItemHorizontal();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.GaugeVar2:
					if (this.Minimum == this.Maximum)
					{
						this.Minimum = 0.0;
						this.Maximum = 100.0;
					}
					base.Content = new GaugeItemVar2();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.GaugeVar3:
					if (this.Minimum == this.Maximum)
					{
						this.Minimum = 0.0;
						this.Maximum = 100.0;
					}
					base.Content = new GaugeItemVar3();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.MultiPidBarChart:
					base.Content = new MultiPidBarChart();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.DashboardChart;
					}
					break;
				case DashboardItemTypes.ColorIndicator:
					base.Content = new IndicatorItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.Dashboard;
					}
					break;
				case DashboardItemTypes.MultiChart:
					base.Content = new MultiChartItem();
					if (this.Model != null)
					{
						this.Model.Mode = LiveDataModes.DashboardChart;
					}
					break;
				}
				base.Content.BindingContext = this;
			}
			catch (Exception ex)
			{
				App.OBDReader.DebugWrite(ex.ToString());
				this.ItemType = DashboardItemTypes.Text;
				base.Content = new SimpleTextItem();
				if (this.Model != null)
				{
					this.Model.Mode = LiveDataModes.Dashboard;
				}
				base.Content.BindingContext = this;
			}
			if (PlatformHelper.IsAndroid)
			{
				base.Content.Margin = new Thickness(2.0);
			}
		}

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x06004098 RID: 16536 RVA: 0x003373F0 File Offset: 0x003355F0
		// (set) Token: 0x06004099 RID: 16537 RVA: 0x003373F8 File Offset: 0x003355F8
		public int Place
		{
			get
			{
				return this._Place;
			}
			set
			{
				this._Place = value;
				this.NotifyPropertyChanged("PlacePlusOne");
			}
		}

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x0033740C File Offset: 0x0033560C
		public int PlacePlusOne
		{
			get
			{
				return this.Place + 1;
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x00337418 File Offset: 0x00335618
		public string ProxyPidName
		{
			get
			{
				int id = this.PID_Id;
				PID pid;
				if (id < 1001)
				{
					pid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == id);
				}
				else
				{
					pid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == id);
					if (pid == null)
					{
						pid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Id == id);
					}
				}
				if (pid == null)
				{
					return PID.Empty.ShortName;
				}
				return pid.ShortName;
			}
		}

		// Token: 0x17001509 RID: 5385
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x003374B6 File Offset: 0x003356B6
		// (set) Token: 0x0600409D RID: 16541 RVA: 0x003374BE File Offset: 0x003356BE
		public bool ShowLowWarning
		{
			get
			{
				return this._ShowLowWarning;
			}
			set
			{
				this._ShowLowWarning = value;
				if (!value)
				{
					this.LowWarningStart = this.Maximum;
				}
				this.NotifyPropertyChanged("ShowLowWarning");
			}
		}

		// Token: 0x1700150A RID: 5386
		// (get) Token: 0x0600409E RID: 16542 RVA: 0x003374E1 File Offset: 0x003356E1
		// (set) Token: 0x0600409F RID: 16543 RVA: 0x003374E9 File Offset: 0x003356E9
		public double LowWarningStart
		{
			get
			{
				return this._LowWarningStart;
			}
			set
			{
				this._LowWarningStart = value;
				this.NotifyPropertyChanged("LowWarningStart");
			}
		}

		// Token: 0x1700150B RID: 5387
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x003374FD File Offset: 0x003356FD
		// (set) Token: 0x060040A1 RID: 16545 RVA: 0x00337505 File Offset: 0x00335705
		public Color LowWarningColor
		{
			get
			{
				return this._LowWarningColor;
			}
			set
			{
				this._LowWarningColor = value;
				this.NotifyPropertyChanged("LowWarningColor");
				this.UpdateValueTextColor();
			}
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x060040A2 RID: 16546 RVA: 0x0033751F File Offset: 0x0033571F
		// (set) Token: 0x060040A3 RID: 16547 RVA: 0x00337527 File Offset: 0x00335727
		public int SegmentCount
		{
			get
			{
				return this._SegmentCount;
			}
			set
			{
				this._SegmentCount = value;
				this.NotifyPropertyChanged("SegmentCount");
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x060040A4 RID: 16548 RVA: 0x0033753B File Offset: 0x0033573B
		// (set) Token: 0x060040A5 RID: 16549 RVA: 0x00337543 File Offset: 0x00335743
		public double CustomInterval
		{
			get
			{
				return this._CustomInterval;
			}
			set
			{
				this._CustomInterval = value;
				this.NotifyPropertyChanged("CustomInterval");
				this.NotifyPropertyChanged("Interval");
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x060040A6 RID: 16550 RVA: 0x00337562 File Offset: 0x00335762
		// (set) Token: 0x060040A7 RID: 16551 RVA: 0x0033756A File Offset: 0x0033576A
		public bool UseCustomInterval
		{
			get
			{
				return this._UseCustomInterval;
			}
			set
			{
				this._UseCustomInterval = value;
				this.NotifyPropertyChanged("UseCustomInterval");
				this.NotifyPropertyChanged("Interval");
			}
		}

		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x00337589 File Offset: 0x00335789
		// (set) Token: 0x060040A9 RID: 16553 RVA: 0x00337591 File Offset: 0x00335791
		public double GradientOffsetPoint1
		{
			get
			{
				return this._GradientOffsetPoint1;
			}
			set
			{
				this._GradientOffsetPoint1 = value;
				this.NotifyPropertyChanged("GradientOffsetPoint1");
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x003375A5 File Offset: 0x003357A5
		// (set) Token: 0x060040AB RID: 16555 RVA: 0x003375AD File Offset: 0x003357AD
		public double GradientOffsetPoint2
		{
			get
			{
				return this._GradientOffsetPoint2;
			}
			set
			{
				this._GradientOffsetPoint2 = value;
				this.NotifyPropertyChanged("GradientOffsetPoint2");
			}
		}

		// Token: 0x17001511 RID: 5393
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x003375C1 File Offset: 0x003357C1
		// (set) Token: 0x060040AD RID: 16557 RVA: 0x003375C9 File Offset: 0x003357C9
		public Color GradientColor1
		{
			get
			{
				return this._GradientColor1;
			}
			set
			{
				this._GradientColor1 = value;
				this.NotifyPropertyChanged("GradientColor1");
			}
		}

		// Token: 0x17001512 RID: 5394
		// (get) Token: 0x060040AE RID: 16558 RVA: 0x003375DD File Offset: 0x003357DD
		// (set) Token: 0x060040AF RID: 16559 RVA: 0x003375E5 File Offset: 0x003357E5
		public Color GradientColor2
		{
			get
			{
				return this._GradientColor2;
			}
			set
			{
				this._GradientColor2 = value;
				this.NotifyPropertyChanged("GradientColor2");
			}
		}

		// Token: 0x17001513 RID: 5395
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x003375F9 File Offset: 0x003357F9
		// (set) Token: 0x060040B1 RID: 16561 RVA: 0x00337601 File Offset: 0x00335801
		public bool GaugeShowMinMaxMarkers
		{
			get
			{
				return this._GaugeShowMinMaxMarkers;
			}
			set
			{
				this._GaugeShowMinMaxMarkers = value;
				this.NotifyPropertyChanged("GaugeShowMinMaxMarkers");
			}
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x060040B2 RID: 16562 RVA: 0x00337615 File Offset: 0x00335815
		// (set) Token: 0x060040B3 RID: 16563 RVA: 0x0033761D File Offset: 0x0033581D
		public Point GradientStartPoint
		{
			get
			{
				return this._GradientStartPoint;
			}
			set
			{
				this._GradientStartPoint = value;
				this.NotifyPropertyChanged("GradientStartPoint");
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x060040B4 RID: 16564 RVA: 0x00337631 File Offset: 0x00335831
		// (set) Token: 0x060040B5 RID: 16565 RVA: 0x00337639 File Offset: 0x00335839
		public Point GradientEndPoint
		{
			get
			{
				return this._GradientEndPoint;
			}
			set
			{
				this._GradientEndPoint = value;
				this.NotifyPropertyChanged("GradientEndPoint");
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060040B6 RID: 16566 RVA: 0x0033764D File Offset: 0x0033584D
		// (set) Token: 0x060040B7 RID: 16567 RVA: 0x00337655 File Offset: 0x00335855
		public double CiruclarGaugeWidth
		{
			get
			{
				return this._CiruclarGaugeWidth;
			}
			set
			{
				this._CiruclarGaugeWidth = value;
				this.NotifyPropertyChanged("FrameSize");
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x00337669 File Offset: 0x00335869
		// (set) Token: 0x060040B9 RID: 16569 RVA: 0x00337671 File Offset: 0x00335871
		public bool GaugeShowBlueLine
		{
			get
			{
				return this._GaugeShowBlueLine;
			}
			set
			{
				this._GaugeShowBlueLine = value;
				if (!value)
				{
					this.GaugeBlueLineStart = this.Minimum;
					this.GaugeBlueLineFinish = this.Minimum;
				}
				this.NotifyPropertyChanged("GaugeShowBlueLine");
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x060040BA RID: 16570 RVA: 0x003376A0 File Offset: 0x003358A0
		// (set) Token: 0x060040BB RID: 16571 RVA: 0x003376A8 File Offset: 0x003358A8
		public double GaugeBlueLineStart
		{
			get
			{
				return this._GaugeBlueLineStart;
			}
			set
			{
				this._GaugeBlueLineStart = value;
				this.NotifyPropertyChanged("GaugeBlueLineStart");
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x060040BC RID: 16572 RVA: 0x003376BC File Offset: 0x003358BC
		// (set) Token: 0x060040BD RID: 16573 RVA: 0x003376C4 File Offset: 0x003358C4
		public double GaugeBlueLineFinish
		{
			get
			{
				return this._GaugeBlueLineFinish;
			}
			set
			{
				this._GaugeBlueLineFinish = value;
				this.NotifyPropertyChanged("GaugeBlueLineFinish");
			}
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x060040BE RID: 16574 RVA: 0x003376D8 File Offset: 0x003358D8
		// (set) Token: 0x060040BF RID: 16575 RVA: 0x003376E0 File Offset: 0x003358E0
		public Color GaugeBlueLineColor
		{
			get
			{
				return this._GaugeBlueLineColor;
			}
			set
			{
				this._GaugeBlueLineColor = value;
				this.NotifyPropertyChanged("GaugeBlueLineColor");
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x060040C0 RID: 16576 RVA: 0x003376F4 File Offset: 0x003358F4
		// (set) Token: 0x060040C1 RID: 16577 RVA: 0x003376FC File Offset: 0x003358FC
		public bool ShowHighWarning
		{
			get
			{
				return this._ShowHighWarning;
			}
			set
			{
				this._ShowHighWarning = value;
				if (!value)
				{
					this.HighWarningStart = this.Maximum;
				}
				this.NotifyPropertyChanged("ShowHighWarning");
			}
		}

		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x060040C2 RID: 16578 RVA: 0x0033771F File Offset: 0x0033591F
		// (set) Token: 0x060040C3 RID: 16579 RVA: 0x00337727 File Offset: 0x00335927
		public double HighWarningStart
		{
			get
			{
				return this._HighWarningStart;
			}
			set
			{
				this._HighWarningStart = value;
				this.NotifyPropertyChanged("HighWarningStart");
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x060040C4 RID: 16580 RVA: 0x0033773B File Offset: 0x0033593B
		// (set) Token: 0x060040C5 RID: 16581 RVA: 0x00337743 File Offset: 0x00335943
		public Color HighWarningColor
		{
			get
			{
				return this._HighWarningColor;
			}
			set
			{
				this._HighWarningColor = value;
				this.NotifyPropertyChanged("HighWarningColor");
				this.UpdateValueTextColor();
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x060040C6 RID: 16582 RVA: 0x0033775D File Offset: 0x0033595D
		// (set) Token: 0x060040C7 RID: 16583 RVA: 0x00337765 File Offset: 0x00335965
		public Color IndicatorBackgroundLowColor
		{
			get
			{
				return this._IndicatorBackgroundLowColor;
			}
			set
			{
				if (value == this._IndicatorBackgroundLowColor)
				{
					return;
				}
				this._IndicatorBackgroundLowColor = value;
				this.OnPropertyChanged("IndicatorBackgroundLowColor");
				this.OnPropertyChanged("IndicatorBackgroundColor");
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x060040C8 RID: 16584 RVA: 0x00337793 File Offset: 0x00335993
		// (set) Token: 0x060040C9 RID: 16585 RVA: 0x0033779B File Offset: 0x0033599B
		public Color IndicatorBackgroundHighColor
		{
			get
			{
				return this._IndicatorBackgroundHighColor;
			}
			set
			{
				if (value == this._IndicatorBackgroundHighColor)
				{
					return;
				}
				this._IndicatorBackgroundHighColor = value;
				this.OnPropertyChanged("IndicatorBackgroundHighColor");
				this.OnPropertyChanged("IndicatorBackgroundColor");
			}
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x003377CC File Offset: 0x003359CC
		private void UpdateIndicatorBackgroundColor()
		{
			if (this.Model == null)
			{
				this.IndicatorBackgroundColor = Color.Transparent;
				return;
			}
			if (this.ShowLowWarning && this.Model.FloatValue < this.LowWarningStart)
			{
				this.IndicatorBackgroundColor = this.IndicatorBackgroundLowColor;
				return;
			}
			if (this.ShowHighWarning && this.Model.FloatValue > this.HighWarningStart)
			{
				this.IndicatorBackgroundColor = this.IndicatorBackgroundHighColor;
				return;
			}
			this.IndicatorBackgroundColor = Color.Transparent;
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x060040CB RID: 16587 RVA: 0x00337848 File Offset: 0x00335A48
		// (set) Token: 0x060040CC RID: 16588 RVA: 0x00337850 File Offset: 0x00335A50
		public Color IndicatorBackgroundColor
		{
			get
			{
				return this._IndicatorBackgroundColor;
			}
			set
			{
				if (value == this._IndicatorBackgroundColor)
				{
					return;
				}
				this._IndicatorBackgroundColor = value;
				this.OnPropertyChanged("IndicatorBackgroundColor");
			}
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00337874 File Offset: 0x00335A74
		public void ShowPopup()
		{
			try
			{
				if (App.GetCurrentPage() is DashboardXamlPage)
				{
					this.CreatePopupLayout().ShowRelativeToView(this, 0, 0.0, this.DesiredHeight / 2.0);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060040CE RID: 16590 RVA: 0x003378C8 File Offset: 0x00335AC8
		public void ShowBtnOkOverlay()
		{
			Grid firstGrid = this.GetFirstGrid(base.Content);
			if (firstGrid != null)
			{
				this.btnOkFrameOverlay = new Frame
				{
					BackgroundColor = Color.Gray,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(0.0),
					Margin = new Thickness(10.0),
					Opacity = 0.75
				};
				Label label = new Label
				{
					BackgroundColor = Color.Gray,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					Text = "OK",
					FontAttributes = 1,
					Margin = new Thickness(5.0),
					TextColor = Color.White
				};
				label.FontSize = (double)Application.Current.Resources["BaseFontSize++"];
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				this.btnOkFrameOverlay.Content = label;
				this.btnOkFrameOverlay.GestureRecognizers.Add(tapGestureRecognizer);
				Frame frameLink = this.btnOkFrameOverlay;
				tapGestureRecognizer.Tapped += delegate(object sender, EventArgs e)
				{
					Dash_CustomPage dash_CustomPage = (Dash_CustomPage)this.GetCurrentPage();
					dash_CustomPage.GestureMode = Dash_CustomPage.GestureModes.None;
					dash_CustomPage.DraggingControl = null;
					if (frameLink != null && firstGrid.Children.Contains(frameLink))
					{
						firstGrid.Children.Remove(frameLink);
						frameLink = null;
					}
					this.btnOkFrameOverlay = null;
				};
				Grid.SetRow(this.btnOkFrameOverlay, 0);
				Grid.SetColumn(this.btnOkFrameOverlay, 0);
				if (firstGrid.RowDefinitions.Count > 0)
				{
					Grid.SetRowSpan(this.btnOkFrameOverlay, firstGrid.RowDefinitions.Count);
				}
				if (firstGrid.ColumnDefinitions.Count > 0)
				{
					Grid.SetColumnSpan(this.btnOkFrameOverlay, firstGrid.ColumnDefinitions.Count);
				}
				firstGrid.Children.Add(this.btnOkFrameOverlay);
			}
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x00337AA4 File Offset: 0x00335CA4
		public void HideBtnOkOverlay()
		{
			if (this.btnOkFrameOverlay != null)
			{
				try
				{
					Grid grid = (Grid)this.btnOkFrameOverlay.Parent;
					if (grid != null && grid.Children.Contains(this.btnOkFrameOverlay) && grid != null)
					{
						grid.Children.Remove(this.btnOkFrameOverlay);
					}
					this.btnOkFrameOverlay = null;
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060040D0 RID: 16592 RVA: 0x00337B14 File Offset: 0x00335D14
		private Grid GetFirstGrid(View v)
		{
			if (v == null)
			{
				return null;
			}
			if (v is Grid)
			{
				return (Grid)v;
			}
			if (!(v is ContentView))
			{
				return null;
			}
			ContentView contentView = (ContentView)v;
			if (contentView.Content is Grid)
			{
				return (Grid)contentView.Content;
			}
			return this.GetFirstGrid(contentView.Content);
		}

		// Token: 0x060040D1 RID: 16593 RVA: 0x00337B6C File Offset: 0x00335D6C
		private async void ShowDragHintLayout()
		{
			if (!SharedSettings.Current.DashboardResizeHintDisplayed)
			{
				PopupWithDontShowAgain.DisplayPopupWithDontShowAgain("", Translate.GetString("dash_ItemMoveAndSizeHint"), "DashboardResizeHintDisplayed", "OK", false);
			}
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00337B9C File Offset: 0x00335D9C
		private SfPopupLayout CreatePopupLayout()
		{
			SfPopupLayout sfPopupLayout = new SfPopupLayout();
			sfPopupLayout.StaysOpen = false;
			sfPopupLayout.PopupView.AutoSizeMode = 2;
			sfPopupLayout.PopupView.AppearanceMode = 0;
			sfPopupLayout.PopupView.ShowFooter = false;
			sfPopupLayout.PopupView.ShowCloseButton = true;
			sfPopupLayout.PopupView.AnimationMode = 0;
			sfPopupLayout.PopupView.ShowHeader = false;
			DataTemplate dataTemplate = new DataTemplate(delegate
			{
				Grid grid = new Grid();
				grid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Auto
				});
				grid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Auto
				});
				grid.ColumnDefinitions.Add(new ColumnDefinition
				{
					Width = GridLength.Auto
				});
				grid.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Auto
				});
				grid.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Auto
				});
				CachedImage cachedImage = new CachedImage
				{
					Source = ImageSource.FromFile("dash_move.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnSize_Clicked;
				cachedImage.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage2 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_settings.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnSet_Clicked;
				cachedImage2.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage3 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_up.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnUp_Clicked;
				cachedImage3.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage4 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_down.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnDown_Clicked;
				cachedImage4.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage5 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_del.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnDel_Clicked;
				cachedImage5.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage6 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_resize_height.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnSizeH_Clicked;
				cachedImage6.GestureRecognizers.Add(tapGestureRecognizer);
				CachedImage cachedImage7 = new CachedImage
				{
					Source = ImageSource.FromFile("dash_resize_width.png"),
					WidthRequest = 60.0,
					HeightRequest = 60.0
				};
				tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.BtnSizeW_Clicked;
				cachedImage7.GestureRecognizers.Add(tapGestureRecognizer);
				Grid.SetColumn(cachedImage2, 0);
				Grid.SetColumn(cachedImage5, 2);
				Grid.SetColumn(cachedImage3, 3);
				Grid.SetColumn(cachedImage, 1);
				Grid.SetColumn(cachedImage7, 0);
				Grid.SetColumn(cachedImage6, 1);
				Grid.SetColumn(cachedImage4, 3);
				Grid.SetRow(cachedImage2, 0);
				Grid.SetRow(cachedImage5, 0);
				Grid.SetRow(cachedImage3, 0);
				Grid.SetRow(cachedImage, 0);
				Grid.SetRow(cachedImage7, 1);
				Grid.SetRow(cachedImage6, 1);
				Grid.SetRow(cachedImage4, 1);
				grid.Children.Add(cachedImage2);
				grid.Children.Add(cachedImage5);
				grid.Children.Add(cachedImage3);
				grid.Children.Add(cachedImage);
				grid.Children.Add(cachedImage7);
				grid.Children.Add(cachedImage6);
				grid.Children.Add(cachedImage4);
				grid.BindingContext = this;
				return grid;
			});
			sfPopupLayout.PopupView.ContentTemplate = dataTemplate;
			return sfPopupLayout;
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x00337C1C File Offset: 0x00335E1C
		private void BtnSizeW_Clicked(object sender, EventArgs e)
		{
			try
			{
				DashboardPage currentPage = this.GetCurrentPage();
				if (currentPage != null)
				{
					Dash_CustomPage dash_CustomPage = currentPage as Dash_CustomPage;
					if (dash_CustomPage != null)
					{
						dash_CustomPage.DraggingControl = this;
						dash_CustomPage.GestureMode = Dash_CustomPage.GestureModes.ResizeWidth;
						View view = (View)sender;
						if (view != null)
						{
							Element parent = view.Parent;
							if (parent != null)
							{
								SfPopupLayout sfPopupLayout = (SfPopupLayout)parent.Parent;
								if (sfPopupLayout != null)
								{
									sfPopupLayout.IsOpen = false;
									this.ShowBtnOkOverlay();
									this.ShowDragHintLayout();
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x00337C9C File Offset: 0x00335E9C
		private void BtnSizeH_Clicked(object sender, EventArgs e)
		{
			try
			{
				DashboardPage currentPage = this.GetCurrentPage();
				if (currentPage != null)
				{
					Dash_CustomPage dash_CustomPage = currentPage as Dash_CustomPage;
					if (dash_CustomPage != null)
					{
						dash_CustomPage.DraggingControl = this;
						dash_CustomPage.GestureMode = Dash_CustomPage.GestureModes.ResizeHeight;
						if (sender != null)
						{
							Element parent = ((View)sender).Parent;
							if (parent != null)
							{
								SfPopupLayout sfPopupLayout = (SfPopupLayout)parent.Parent;
								if (sfPopupLayout != null)
								{
									sfPopupLayout.IsOpen = false;
									this.ShowBtnOkOverlay();
									this.ShowDragHintLayout();
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00337D14 File Offset: 0x00335F14
		private void BtnSize_Clicked(object sender, EventArgs e)
		{
			try
			{
				DashboardPage currentPage = this.GetCurrentPage();
				if (currentPage != null)
				{
					Dash_CustomPage dash_CustomPage = currentPage as Dash_CustomPage;
					if (dash_CustomPage != null)
					{
						dash_CustomPage.DraggingControl = this;
						dash_CustomPage.GestureMode = Dash_CustomPage.GestureModes.ResizeBoth;
						View view = (View)sender;
						if (view != null)
						{
							Element parent = view.Parent;
							if (parent != null)
							{
								SfPopupLayout sfPopupLayout = (SfPopupLayout)parent.Parent;
								if (sfPopupLayout != null)
								{
									sfPopupLayout.IsOpen = false;
									this.ShowBtnOkOverlay();
									this.ShowDragHintLayout();
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x00337D94 File Offset: 0x00335F94
		private async void BtnDel_Clicked(object sender, EventArgs e)
		{
			Page currentPage = App.GetCurrentPage();
			bool flag = currentPage != null;
			if (flag)
			{
				flag = await currentPage.DisplayAlert(Translate.GetString("dash_DeleteSensor"), this.PIDName, "OK", Translate.GetString("ios_Cancel"));
			}
			if (flag)
			{
				try
				{
					Dash_CustomPage dash_CustomPage = (Dash_CustomPage)this.GetCurrentPage();
					if (dash_CustomPage != null)
					{
						dash_CustomPage.RemoveItem(this);
					}
					View view = (View)sender;
					Element element = ((view != null) ? view.Parent : null);
					SfPopupLayout sfPopupLayout = (SfPopupLayout)((element != null) ? element.Parent : null);
					if (sfPopupLayout != null)
					{
						sfPopupLayout.IsOpen = false;
					}
					DashboardListViewModel.Current.SaveDashboardToSettings();
				}
				catch
				{
				}
			}
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x00337DD4 File Offset: 0x00335FD4
		private void BtnDown_Clicked(object sender, EventArgs e)
		{
			try
			{
				(base.Parent as Layout).LowerChild(this);
				Dash_CustomPage dash_CustomPage = (Dash_CustomPage)this.GetCurrentPage();
				int num = dash_CustomPage.Items.IndexOf(this);
				dash_CustomPage.Items.Move(num, 0);
				DashboardListViewModel.Current.SaveDashboardToSettings();
				((SfPopupLayout)((View)sender).Parent.Parent).IsOpen = false;
			}
			catch
			{
			}
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x00337E50 File Offset: 0x00336050
		private void BtnUp_Clicked(object sender, EventArgs e)
		{
			try
			{
				(base.Parent as Layout).RaiseChild(this);
				Dash_CustomPage dash_CustomPage = (Dash_CustomPage)this.GetCurrentPage();
				int num = dash_CustomPage.Items.IndexOf(this);
				dash_CustomPage.Items.Move(num, dash_CustomPage.Items.Count - 1);
				DashboardListViewModel.Current.SaveDashboardToSettings();
				((SfPopupLayout)((View)sender).Parent.Parent).IsOpen = false;
			}
			catch
			{
			}
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00337EDC File Offset: 0x003360DC
		private async void BtnSet_Clicked(object sender, EventArgs e)
		{
			(sender as View).IsEnabled = false;
			if (DashboardXamlPage.Instance != null && App.GetCurrentPage() == DashboardXamlPage.Instance)
			{
				if (App.UseLegacyUI)
				{
					if (PlatformHelper.IsiOS)
					{
						Page page = new DashboardItemEditor(this, base.Width, base.Height);
						await DashboardXamlPage.Instance.Navigation.PushAsync(page, true);
					}
				}
				else
				{
					Page page2 = new DashboardItemEditorV3(this, base.Width, base.Height);
					await DashboardXamlPage.Instance.Navigation.PushAsync(page2, true);
				}
			}
			(sender as View).IsEnabled = true;
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x00337F1B File Offset: 0x0033611B
		private DashboardPage GetCurrentPage()
		{
			return DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this));
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x00337F38 File Offset: 0x00336138
		public static void RegisterDashboardItemTypes()
		{
			StaticLists.DashboardItemTypesList.Clear();
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_TextIndicator.Content"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_ChartIndicator.Content"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_GaugeIndicator.Content"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("ios_CustomPIDEditor_IsAction"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_GaugeLinearIndicator.Content"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_TextIndicatorHorizontal"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_GaugeIndicator.Content") + " 2");
			StaticLists.RegisterDashboardItemType(Translate.GetString("Dashboard2ItemEditor_GaugeIndicator.Content") + " 3");
			StaticLists.RegisterDashboardItemType(Translate.GetString("dash_MultiPidBarChart"));
			StaticLists.RegisterDashboardItemType(Translate.GetString("dash_ColorIndicator"));
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x00337FFC File Offset: 0x003361FC
		public static void ApplyThemeToItem(DashboardItem item)
		{
			if (item == null)
			{
				return;
			}
			switch (SharedSettings.Current.DashboardTheme)
			{
			case 0:
				item._FrameColor = Color.White;
				item.BackgroundColor = Color.Black;
				item._GaugeRimColor = Color.White;
				item._GaugeTickColor = item.GaugeRimColor;
				item._GaugeLabelColor = item.GaugeRimColor;
				item._GaugePointerColor = Color.White;
				item._ChartLineColor = Color.White;
				item._TitleTextColor = Color.White;
				item._UnitsTextColor = Color.White;
				item._ValueNormalTextColor = Color.White;
				item._ShowDefaultBackground = false;
				item.GaugePointerColor = Color.FromHex("DB0000");
				item.FrameSize = 0.0;
				return;
			case 1:
				item.FrameColor = Color.Black;
				item.BackgroundColor = Color.White;
				item.TitleTextColor = Color.Black;
				item.UnitsTextColor = Color.Black;
				item.ValueNormalTextColor = Color.Black;
				item.GaugeRimColor = Color.Black;
				item.GaugeTickColor = item.GaugeRimColor;
				item.GaugeLabelColor = item.GaugeRimColor;
				item.GaugePointerColor = Color.DarkRed;
				item.ChartLineColor = Color.FromHex("007aff");
				item.ShowDefaultBackground = false;
				item.FrameSize = 0.0;
				return;
			case 2:
				item.FrameColor = Color.Black;
				item.BackgroundColor = Color.Black;
				item.GaugeRimColor = Color.White;
				item.GaugeTickColor = item.GaugeRimColor;
				item.GaugeLabelColor = item.GaugeRimColor;
				item.GaugePointerColor = Color.FromHex("DB0000");
				item.ChartLineColor = Color.White;
				item.TitleTextColor = Color.White;
				item.UnitsTextColor = Color.White;
				item.ValueNormalTextColor = Color.White;
				item.ShowDefaultBackground = true;
				item.FrameSize = 1.0;
				return;
			default:
				return;
			}
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_0(PID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040DE RID: 16606 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_1(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040DF RID: 16607 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_2(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_3(PID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E1 RID: 16609 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_4(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E2 RID: 16610 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_5(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_6(PID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_7(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x003381D9 File Offset: 0x003363D9
		[CompilerGenerated]
		private bool <set_PID_Id>b__167_8(CustomPID x)
		{
			return this.PID_Id == x.Id;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x003381E9 File Offset: 0x003363E9
		[CompilerGenerated]
		private bool <Start>b__233_0(PID x)
		{
			return x != null && x.Id == this.PID_Id;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x00338200 File Offset: 0x00336400
		[CompilerGenerated]
		private object <CreatePopupLayout>b__356_0()
		{
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Auto
			});
			grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Auto
			});
			CachedImage cachedImage = new CachedImage
			{
				Source = ImageSource.FromFile("dash_move.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnSize_Clicked;
			cachedImage.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage2 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_settings.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnSet_Clicked;
			cachedImage2.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage3 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_up.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnUp_Clicked;
			cachedImage3.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage4 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_down.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnDown_Clicked;
			cachedImage4.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage5 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_del.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnDel_Clicked;
			cachedImage5.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage6 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_resize_height.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnSizeH_Clicked;
			cachedImage6.GestureRecognizers.Add(tapGestureRecognizer);
			CachedImage cachedImage7 = new CachedImage
			{
				Source = ImageSource.FromFile("dash_resize_width.png"),
				WidthRequest = 60.0,
				HeightRequest = 60.0
			};
			tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += this.BtnSizeW_Clicked;
			cachedImage7.GestureRecognizers.Add(tapGestureRecognizer);
			Grid.SetColumn(cachedImage2, 0);
			Grid.SetColumn(cachedImage5, 2);
			Grid.SetColumn(cachedImage3, 3);
			Grid.SetColumn(cachedImage, 1);
			Grid.SetColumn(cachedImage7, 0);
			Grid.SetColumn(cachedImage6, 1);
			Grid.SetColumn(cachedImage4, 3);
			Grid.SetRow(cachedImage2, 0);
			Grid.SetRow(cachedImage5, 0);
			Grid.SetRow(cachedImage3, 0);
			Grid.SetRow(cachedImage, 0);
			Grid.SetRow(cachedImage7, 1);
			Grid.SetRow(cachedImage6, 1);
			Grid.SetRow(cachedImage4, 1);
			grid.Children.Add(cachedImage2);
			grid.Children.Add(cachedImage5);
			grid.Children.Add(cachedImage3);
			grid.Children.Add(cachedImage);
			grid.Children.Add(cachedImage7);
			grid.Children.Add(cachedImage6);
			grid.Children.Add(cachedImage4);
			grid.BindingContext = this;
			return grid;
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x003385D2 File Offset: 0x003367D2
		[CompilerGenerated]
		private bool <GetCurrentPage>b__364_0(DashboardPage x)
		{
			return x.Items.Contains(this);
		}

		// Token: 0x0400276F RID: 10095
		private double last_float_value = double.NaN;

		// Token: 0x04002770 RID: 10096
		private bool _GaugeShowRedLine;

		// Token: 0x04002771 RID: 10097
		private double _GaugeRedLineStart;

		// Token: 0x04002772 RID: 10098
		private double _GaugeRedLineFinish;

		// Token: 0x04002773 RID: 10099
		private Color _GaugeRedLineColor = Color.Red;

		// Token: 0x04002774 RID: 10100
		private bool _ShowDefaultBackground = true;

		// Token: 0x04002775 RID: 10101
		private double _FrameSize = 1.0;

		// Token: 0x04002776 RID: 10102
		private Color _FrameColor = Color.Black;

		// Token: 0x04002777 RID: 10103
		private Color _TitleTextColor = Color.White;

		// Token: 0x04002778 RID: 10104
		private double _TitleFontSize = (double)Application.Current.Resources["BaseFontSize++"];

		// Token: 0x04002779 RID: 10105
		private Color _ValueNormalTextColor = Color.White;

		// Token: 0x0400277A RID: 10106
		private Color _ValueTextColor;

		// Token: 0x0400277B RID: 10107
		private double _ValueFontSize = (double)Application.Current.Resources["BaseFontSize+++"];

		// Token: 0x0400277C RID: 10108
		private Color _UnitsTextColor = Color.White;

		// Token: 0x0400277D RID: 10109
		private double _UnitsFontSize = (double)Application.Current.Resources["BaseFontSize++"];

		// Token: 0x0400277E RID: 10110
		private Color _GaugeLabelColor = Color.White;

		// Token: 0x0400277F RID: 10111
		private Color _GaugeRimColor = Color.White;

		// Token: 0x04002780 RID: 10112
		private Color _GaugeTickColor = Color.White;

		// Token: 0x04002781 RID: 10113
		private Color _GaugePointerColor = Color.White;

		// Token: 0x04002782 RID: 10114
		private Color _ChartLineColor = Color.FromHex("007aff");

		// Token: 0x04002783 RID: 10115
		private bool _ChartValuePositionCenter;

		// Token: 0x04002784 RID: 10116
		private int _ChartLineWidth = 1;

		// Token: 0x04002785 RID: 10117
		private Color _GaugeKnobColor = Color.SlateGray;

		// Token: 0x04002786 RID: 10118
		private double _LinearScaleSize = 25.0;

		// Token: 0x04002787 RID: 10119
		private bool _ShowValue = true;

		// Token: 0x04002788 RID: 10120
		private bool _LinearOrientationHorizontal = true;

		// Token: 0x04002789 RID: 10121
		private bool _OverrideName;

		// Token: 0x0400278A RID: 10122
		private string _CustomName = "";

		// Token: 0x0400278B RID: 10123
		private bool _PlaySound;

		// Token: 0x0400278C RID: 10124
		private string _SoundName = StaticLists.SoundsList[0];

		// Token: 0x0400278D RID: 10125
		private double _SoundStart;

		// Token: 0x0400278E RID: 10126
		private bool _PlaySoundLow;

		// Token: 0x0400278F RID: 10127
		private string _SoundNameLow = StaticLists.SoundsList[0];

		// Token: 0x04002790 RID: 10128
		private double _SoundStartLow;

		// Token: 0x04002791 RID: 10129
		private DashboardItemTypes _ItemType;

		// Token: 0x04002792 RID: 10130
		private double _Maximum;

		// Token: 0x04002793 RID: 10131
		private double _Minimum;

		// Token: 0x04002794 RID: 10132
		private bool _UseCustomMinMax;

		// Token: 0x04002795 RID: 10133
		private LiveDataPIDModel _Model;

		// Token: 0x04002796 RID: 10134
		private uint float_change_counter;

		// Token: 0x04002797 RID: 10135
		private ISoundManager player;

		// Token: 0x04002798 RID: 10136
		private ISoundManager playerLow;

		// Token: 0x04002799 RID: 10137
		private int _PID_Id;

		// Token: 0x0400279A RID: 10138
		private List<int> _PID_IDs = new List<int>(0);

		// Token: 0x0400279B RID: 10139
		private bool _ValueUseLCDFont;

		// Token: 0x0400279C RID: 10140
		private int _ValueFormat = 2;

		// Token: 0x0400279D RID: 10141
		private double _DesiredWidth = 100.0;

		// Token: 0x0400279E RID: 10142
		private double _DesiredHeight = 100.0;

		// Token: 0x0400279F RID: 10143
		private double _PositionX = 25.0;

		// Token: 0x040027A0 RID: 10144
		private double _PositionY = 25.0;

		// Token: 0x040027A1 RID: 10145
		private double _MinMaxAvgFontSize = (double)Application.Current.Resources["BaseFontSize--"];

		// Token: 0x040027A2 RID: 10146
		private bool _ShowMinMax = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x040027A3 RID: 10147
		private bool _ShowMinMaxPointers = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x040027A4 RID: 10148
		private Color _MinMaxPointersColor = Color.FromHex("FFA500");

		// Token: 0x040027A5 RID: 10149
		private bool _ShowAvg;

		// Token: 0x040027A6 RID: 10150
		private Color _MinMaxAvgColor = Color.FromHex("FFA500");

		// Token: 0x040027A7 RID: 10151
		private bool _SetMinMaxAvgOnlyVisibleArea = SharedSettings.Current.SetChartMinMaxOnlyVisibleArea;

		// Token: 0x040027A8 RID: 10152
		private ChartItemTypes _ChartItemType;

		// Token: 0x040027A9 RID: 10153
		private int _LiveDataShowTime = SharedSettings.Current.LiveDataShowTime;

		// Token: 0x040027AA RID: 10154
		private int _Place;

		// Token: 0x040027AB RID: 10155
		private bool _ShowLowWarning;

		// Token: 0x040027AC RID: 10156
		private double _LowWarningStart;

		// Token: 0x040027AD RID: 10157
		private Color _LowWarningColor = Color.LightBlue;

		// Token: 0x040027AE RID: 10158
		private int _SegmentCount;

		// Token: 0x040027AF RID: 10159
		private double _CustomInterval = 5.0;

		// Token: 0x040027B0 RID: 10160
		private bool _UseCustomInterval;

		// Token: 0x040027B1 RID: 10161
		private double _GradientOffsetPoint1 = 0.153;

		// Token: 0x040027B2 RID: 10162
		private double _GradientOffsetPoint2 = 0.984;

		// Token: 0x040027B3 RID: 10163
		private Color _GradientColor1 = Color.FromHex("FF124AA0");

		// Token: 0x040027B4 RID: 10164
		private Color _GradientColor2 = Color.FromHex("FF072A72");

		// Token: 0x040027B5 RID: 10165
		private bool _GaugeShowMinMaxMarkers = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x040027B6 RID: 10166
		private Point _GradientStartPoint = new Point(0.0, 0.0);

		// Token: 0x040027B7 RID: 10167
		private Point _GradientEndPoint = new Point(1.0, 1.0);

		// Token: 0x040027B8 RID: 10168
		private double _CiruclarGaugeWidth = 12.0;

		// Token: 0x040027B9 RID: 10169
		private bool _GaugeShowBlueLine;

		// Token: 0x040027BA RID: 10170
		private double _GaugeBlueLineStart;

		// Token: 0x040027BB RID: 10171
		private double _GaugeBlueLineFinish;

		// Token: 0x040027BC RID: 10172
		private Color _GaugeBlueLineColor = Color.Blue;

		// Token: 0x040027BD RID: 10173
		private bool _ShowHighWarning;

		// Token: 0x040027BE RID: 10174
		private double _HighWarningStart;

		// Token: 0x040027BF RID: 10175
		private Color _HighWarningColor = Color.LightBlue;

		// Token: 0x040027C0 RID: 10176
		private Color _IndicatorBackgroundLowColor = Color.Blue;

		// Token: 0x040027C1 RID: 10177
		private Color _IndicatorBackgroundHighColor = Color.Red;

		// Token: 0x040027C2 RID: 10178
		private Color _IndicatorBackgroundColor = Color.Transparent;

		// Token: 0x040027C3 RID: 10179
		private Frame btnOkFrameOverlay;

		// Token: 0x040027C4 RID: 10180
		private const int THEME_DARK = 0;

		// Token: 0x040027C5 RID: 10181
		private const int THEME_LIGHT = 1;

		// Token: 0x040027C6 RID: 10182
		private const int THEME_CARSCANNER = 2;

		// Token: 0x0200076A RID: 1898
		[CompilerGenerated]
		private sealed class <>c__DisplayClass252_0
		{
			// Token: 0x060040E9 RID: 16617 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass252_0()
			{
			}

			// Token: 0x060040EA RID: 16618 RVA: 0x003385E0 File Offset: 0x003367E0
			internal bool <get_ProxyPidName>b__0(PID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x060040EB RID: 16619 RVA: 0x003385E0 File Offset: 0x003367E0
			internal bool <get_ProxyPidName>b__1(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x060040EC RID: 16620 RVA: 0x003385E0 File Offset: 0x003367E0
			internal bool <get_ProxyPidName>b__2(CustomPID x)
			{
				return x.Id == this.id;
			}

			// Token: 0x040027C7 RID: 10183
			public int id;
		}

		// Token: 0x0200076B RID: 1899
		[CompilerGenerated]
		private sealed class <>c__DisplayClass352_0
		{
			// Token: 0x060040ED RID: 16621 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass352_0()
			{
			}

			// Token: 0x060040EE RID: 16622 RVA: 0x003385F0 File Offset: 0x003367F0
			internal void <ShowBtnOkOverlay>b__0(object sender, EventArgs e)
			{
				Dash_CustomPage dash_CustomPage = (Dash_CustomPage)this.<>4__this.GetCurrentPage();
				dash_CustomPage.GestureMode = Dash_CustomPage.GestureModes.None;
				dash_CustomPage.DraggingControl = null;
				if (this.frameLink != null && this.firstGrid.Children.Contains(this.frameLink))
				{
					this.firstGrid.Children.Remove(this.frameLink);
					this.frameLink = null;
				}
				this.<>4__this.btnOkFrameOverlay = null;
			}

			// Token: 0x040027C8 RID: 10184
			public DashboardItem <>4__this;

			// Token: 0x040027C9 RID: 10185
			public Grid firstGrid;

			// Token: 0x040027CA RID: 10186
			public Frame frameLink;
		}

		// Token: 0x0200076C RID: 1900
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnDel_Clicked>d__360 : IAsyncStateMachine
		{
			// Token: 0x060040EF RID: 16623 RVA: 0x00338664 File Offset: 0x00336864
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItem dashboardItem = this;
				try
				{
					bool flag;
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						Page currentPage = App.GetCurrentPage();
						flag = currentPage != null;
						if (!flag)
						{
							goto IL_009D;
						}
						taskAwaiter = currentPage.DisplayAlert(Translate.GetString("dash_DeleteSensor"), dashboardItem.PIDName, "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardItem.<BtnDel_Clicked>d__360>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					flag = taskAwaiter.GetResult();
					IL_009D:
					if (flag)
					{
						try
						{
							Dash_CustomPage dash_CustomPage = (Dash_CustomPage)dashboardItem.GetCurrentPage();
							if (dash_CustomPage != null)
							{
								dash_CustomPage.RemoveItem(dashboardItem);
							}
							View view = (View)sender;
							Element element = ((view != null) ? view.Parent : null);
							SfPopupLayout sfPopupLayout = (SfPopupLayout)((element != null) ? element.Parent : null);
							if (sfPopupLayout != null)
							{
								sfPopupLayout.IsOpen = false;
							}
							DashboardListViewModel.Current.SaveDashboardToSettings();
						}
						catch
						{
						}
					}
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

			// Token: 0x060040F0 RID: 16624 RVA: 0x003387B8 File Offset: 0x003369B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040027CB RID: 10187
			public int <>1__state;

			// Token: 0x040027CC RID: 10188
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040027CD RID: 10189
			public DashboardItem <>4__this;

			// Token: 0x040027CE RID: 10190
			public object sender;

			// Token: 0x040027CF RID: 10191
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200076D RID: 1901
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnSet_Clicked>d__363 : IAsyncStateMachine
		{
			// Token: 0x060040F1 RID: 16625 RVA: 0x003387C8 File Offset: 0x003369C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItem dashboardItem = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							(sender as View).IsEnabled = false;
							if (DashboardXamlPage.Instance == null || App.GetCurrentPage() != DashboardXamlPage.Instance)
							{
								goto IL_014C;
							}
							if (App.UseLegacyUI)
							{
								if (!PlatformHelper.IsiOS)
								{
									goto IL_014C;
								}
								Page page = new DashboardItemEditor(dashboardItem, dashboardItem.Width, dashboardItem.Height);
								taskAwaiter = DashboardXamlPage.Instance.Navigation.PushAsync(page, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItem.<BtnSet_Clicked>d__363>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00CB;
							}
							else
							{
								Page page2 = new DashboardItemEditorV3(dashboardItem, dashboardItem.Width, dashboardItem.Height);
								taskAwaiter = DashboardXamlPage.Instance.Navigation.PushAsync(page2, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItem.<BtnSet_Clicked>d__363>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_014C;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00CB:
					taskAwaiter.GetResult();
					IL_014C:
					(sender as View).IsEnabled = true;
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

			// Token: 0x060040F2 RID: 16626 RVA: 0x0033897C File Offset: 0x00336B7C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040027D0 RID: 10192
			public int <>1__state;

			// Token: 0x040027D1 RID: 10193
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040027D2 RID: 10194
			public object sender;

			// Token: 0x040027D3 RID: 10195
			public DashboardItem <>4__this;

			// Token: 0x040027D4 RID: 10196
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200076E RID: 1902
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowDragHintLayout>d__355 : IAsyncStateMachine
		{
			// Token: 0x060040F3 RID: 16627 RVA: 0x0033898C File Offset: 0x00336B8C
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					if (!SharedSettings.Current.DashboardResizeHintDisplayed)
					{
						PopupWithDontShowAgain.DisplayPopupWithDontShowAgain("", Translate.GetString("dash_ItemMoveAndSizeHint"), "DashboardResizeHintDisplayed", "OK", false);
					}
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

			// Token: 0x060040F4 RID: 16628 RVA: 0x00338A00 File Offset: 0x00336C00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040027D5 RID: 10197
			public int <>1__state;

			// Token: 0x040027D6 RID: 10198
			public AsyncVoidMethodBuilder <>t__builder;
		}
	}
}
