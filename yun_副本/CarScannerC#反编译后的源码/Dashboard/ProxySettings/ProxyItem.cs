using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.ProxySettings
{
	// Token: 0x02000778 RID: 1912
	public class ProxyItem
	{
		// Token: 0x0600413E RID: 16702 RVA: 0x0033964C File Offset: 0x0033784C
		public ProxyItem()
		{
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00339858 File Offset: 0x00337A58
		public ProxyItem(DashboardItem item)
		{
			this.ApplySettingsFromRealItem(item);
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00339A6C File Offset: 0x00337C6C
		public void ApplySettingsToRealItem(DashboardItem realItem, bool ExcludePIDId = false)
		{
			if (!double.IsFinite(this.Minimum))
			{
				this.Minimum = 0.0;
			}
			if (!double.IsFinite(this.Maximum))
			{
				this.Maximum = 0.0;
			}
			realItem.BackgroundColor = this.BackgroundColor;
			realItem.ChartLineColor = this.ChartLineColor;
			realItem.FrameColor = this.FrameColor;
			realItem.FrameSize = this.FrameSize;
			realItem.GaugeLabelColor = this.GaugeLabelColor;
			realItem.GaugePointerColor = this.GaugePointerColor;
			realItem.GaugeRedLineColor = this.GaugeRedLineColor;
			realItem.GaugeRedLineFinish = this.GaugeRedLineFinish;
			realItem.GaugeRedLineStart = this.GaugeRedLineStart;
			realItem.GaugeShowRedLine = this.GaugeShowRedLine;
			realItem.GaugeRimColor = this.GaugeRimColor;
			realItem.GaugeTickColor = this.GaugeTickColor;
			realItem.ShowValue = this.ShowValue;
			realItem.ItemType = this.ItemType;
			realItem.Maximum = this.Maximum;
			realItem.Minimum = this.Minimum;
			if (!ExcludePIDId)
			{
				realItem.PID_Id = this.PID_Id;
				realItem.OverrideName = this.OverrideName;
				realItem.CustomName = this.CustomName;
				realItem.DesiredHeight = this.DesiredHeight;
				realItem.DesiredWidth = this.DesiredWidth;
				realItem.PositionX = this.PositionX;
				realItem.PositionY = this.PositionY;
				realItem.PID_IDs = this.PID_IDs;
			}
			realItem.ShowDefaultBackground = this.ShowDefaultBackground;
			realItem.TitleFontSize = this.TitleFontSize;
			realItem.TitleTextColor = this.TitleTextColor;
			realItem.UnitsFontSize = this.UnitsFontSize;
			realItem.UnitsTextColor = this.UnitsTextColor;
			realItem.ValueFontSize = this.ValueFontSize;
			realItem.ValueNormalTextColor = this.ValueNormalTextColor;
			realItem.ValueUseLCDFont = this.ValueUseLCDFont;
			realItem.SoundName = this.SoundName ?? StaticLists.SoundsList[0];
			realItem.SoundStart = this.SoundStart;
			realItem.PlaySound = this.PlaySound;
			realItem.ValueFormat = this.ValueFormat;
			realItem.LowWarningColor = this.LowWarningColor;
			realItem.LowWarningStart = this.LowWarningStart;
			realItem.ShowLowWarning = this.ShowLowWarning;
			realItem.PlaySoundLow = this.PlaySoundLow;
			realItem.SoundNameLow = this.SoundNameLow ?? StaticLists.SoundsList[0];
			realItem.SoundStartLow = this.SoundStartLow;
			realItem.GaugeKnobColor = this.GaugeKnobColor;
			realItem.UseCustomMinMax = this.UseCustomMinMax;
			realItem.ShowValue = this.ShowValue;
			realItem.LinearOrientationHorizontal = this.LinearOrientationHorizontal;
			realItem.LinearScaleSize = this.LinearScaleSize;
			realItem.SegmentCount = this.SegmentCount;
			realItem.CustomInterval = this.CustomInterval;
			realItem.UseCustomInterval = this.UseCustomInterval;
			realItem.CornerRadius = this.CornerRadius;
			realItem.MinMaxAvgFontSize = this.MinMaxAvgFontSize;
			realItem.ShowMinMax = this.ShowMinMax;
			realItem.ShowAvg = this.ShowAvg;
			realItem.MinMaxAvgColor = this.MinMaxAvgColor;
			realItem.SetMinMaxAvgOnlyVisibleArea = this.SetMinMaxAvgOnlyVisibleArea;
			realItem.GradientOffsetPoint1 = this.GradientOffsetPoint1;
			realItem.GradientOffsetPoint2 = this.GradientOffsetPoint2;
			realItem.GradientColor1 = this.GradientColor1;
			realItem.GradientColor2 = this.GradientColor2;
			realItem.GaugeShowMinMaxMarkers = this.GaugeShowMinMaxMarkers;
			realItem.GradientStartPoint = this.GradientStartPoint;
			realItem.GradientEndPoint = this.GradientEndPoint;
			realItem.CiruclarGaugeWidth = this.CiruclarGaugeWidth;
			realItem.GaugeBlueLineColor = this.GaugeBlueLineColor;
			realItem.GaugeBlueLineFinish = this.GaugeBlueLineFinish;
			realItem.GaugeBlueLineStart = this.GaugeBlueLineStart;
			realItem.GaugeShowBlueLine = this.GaugeShowBlueLine;
			realItem.MinMaxPointersColor = this.MinMaxPointersColor;
			realItem.ShowMinMaxPointers = this.ShowMinMaxPointers;
			realItem.ChartItemType = this.ChartItemType;
			realItem.LiveDataShowTime = this.LiveDataShowTime;
			realItem.ChartValuePositionCenter = this.ChartValuePositionCenter;
			realItem.ChartLineWidth = this.ChartLineWidth;
			realItem.HighWarningColor = this.HighWarningColor;
			realItem.IndicatorBackgroundHighColor = this.IndicatorBackgroundHighColor;
			realItem.IndicatorBackgroundLowColor = this.IndicatorBackgroundLowColor;
			realItem.HighWarningStart = this.HighWarningStart;
			realItem.ShowHighWarning = this.ShowHighWarning;
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00339E88 File Offset: 0x00338088
		public void ApplyColorSettingsToProxyItem(ProxyItem targetItem)
		{
			PropertyInfo[] properties = base.GetType().GetProperties();
			Type typeFromHandle = typeof(Color);
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.PropertyType == typeFromHandle || propertyInfo.Name == "ShowDefaultBackground" || propertyInfo.Name == "GradientOffsetPoint1" || propertyInfo.Name == "GradientOffsetPoint2" || propertyInfo.Name == "GradientStartPoint" || propertyInfo.Name == "GradientEndPoint")
				{
					object value = propertyInfo.GetValue(this);
					propertyInfo.SetValue(targetItem, value);
				}
			}
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00339F40 File Offset: 0x00338140
		public void ApplySettingsToProxyItem(ProxyItem targetItem, bool ExcludePIDId = false)
		{
			targetItem.BackgroundColor = this.BackgroundColor;
			targetItem.ChartLineColor = this.ChartLineColor;
			targetItem.FrameColor = this.FrameColor;
			targetItem.FrameSize = this.FrameSize;
			targetItem.GaugeLabelColor = this.GaugeLabelColor;
			targetItem.GaugePointerColor = this.GaugePointerColor;
			targetItem.GaugeRedLineColor = this.GaugeRedLineColor;
			targetItem.GaugeRedLineFinish = this.GaugeRedLineFinish;
			targetItem.GaugeRedLineStart = this.GaugeRedLineStart;
			targetItem.GaugeShowRedLine = this.GaugeShowRedLine;
			targetItem.GaugeRimColor = this.GaugeRimColor;
			targetItem.GaugeTickColor = this.GaugeTickColor;
			targetItem.ShowValue = this.ShowValue;
			targetItem.ItemType = this.ItemType;
			targetItem.Maximum = this.Maximum;
			targetItem.Minimum = this.Minimum;
			if (!ExcludePIDId)
			{
				targetItem.PID_Id = this.PID_Id;
				targetItem.OverrideName = this.OverrideName;
				targetItem.CustomName = this.CustomName;
				targetItem.DesiredHeight = this.DesiredHeight;
				targetItem.DesiredWidth = this.DesiredWidth;
				targetItem.PositionX = this.PositionX;
				targetItem.PositionY = this.PositionY;
				targetItem.PID_IDs = this.PID_IDs;
			}
			targetItem.ShowDefaultBackground = this.ShowDefaultBackground;
			targetItem.TitleFontSize = this.TitleFontSize;
			targetItem.TitleTextColor = this.TitleTextColor;
			targetItem.UnitsFontSize = this.UnitsFontSize;
			targetItem.UnitsTextColor = this.UnitsTextColor;
			targetItem.ValueFontSize = this.ValueFontSize;
			targetItem.ValueNormalTextColor = this.ValueNormalTextColor;
			targetItem.ValueUseLCDFont = this.ValueUseLCDFont;
			targetItem.SoundName = this.SoundName ?? StaticLists.SoundsList[0];
			targetItem.SoundStart = this.SoundStart;
			targetItem.PlaySound = this.PlaySound;
			targetItem.ValueFormat = this.ValueFormat;
			targetItem.LowWarningColor = this.LowWarningColor;
			targetItem.LowWarningStart = this.LowWarningStart;
			targetItem.ShowLowWarning = this.ShowLowWarning;
			targetItem.PlaySoundLow = this.PlaySoundLow;
			targetItem.SoundNameLow = this.SoundNameLow ?? StaticLists.SoundsList[0];
			targetItem.SoundStartLow = this.SoundStartLow;
			targetItem.GaugeKnobColor = this.GaugeKnobColor;
			targetItem.UseCustomMinMax = this.UseCustomMinMax;
			targetItem.ShowValue = this.ShowValue;
			targetItem.LinearOrientationHorizontal = this.LinearOrientationHorizontal;
			targetItem.LinearScaleSize = this.LinearScaleSize;
			targetItem.SegmentCount = this.SegmentCount;
			targetItem.CustomInterval = this.CustomInterval;
			targetItem.UseCustomInterval = this.UseCustomInterval;
			targetItem.CornerRadius = this.CornerRadius;
			targetItem.MinMaxAvgFontSize = this.MinMaxAvgFontSize;
			targetItem.ShowMinMax = this.ShowMinMax;
			targetItem.ShowAvg = this.ShowAvg;
			targetItem.MinMaxAvgColor = this.MinMaxAvgColor;
			targetItem.SetMinMaxAvgOnlyVisibleArea = this.SetMinMaxAvgOnlyVisibleArea;
			targetItem.GradientOffsetPoint1 = this.GradientOffsetPoint1;
			targetItem.GradientOffsetPoint2 = this.GradientOffsetPoint2;
			targetItem.GradientColor1 = this.GradientColor1;
			targetItem.GradientColor2 = this.GradientColor2;
			targetItem.GaugeShowMinMaxMarkers = this.GaugeShowMinMaxMarkers;
			targetItem.GradientStartPoint = this.GradientStartPoint;
			targetItem.GradientEndPoint = this.GradientEndPoint;
			targetItem.CiruclarGaugeWidth = this.CiruclarGaugeWidth;
			targetItem.GaugeBlueLineColor = this.GaugeBlueLineColor;
			targetItem.GaugeBlueLineFinish = this.GaugeBlueLineFinish;
			targetItem.GaugeBlueLineStart = this.GaugeBlueLineStart;
			targetItem.GaugeShowBlueLine = this.GaugeShowBlueLine;
			targetItem.MinMaxPointersColor = this.MinMaxPointersColor;
			targetItem.ShowMinMaxPointers = this.ShowMinMaxPointers;
			targetItem.ChartItemType = this.ChartItemType;
			targetItem.LiveDataShowTime = this.LiveDataShowTime;
			targetItem.ChartValuePositionCenter = this.ChartValuePositionCenter;
			targetItem.ChartLineWidth = this.ChartLineWidth;
			targetItem.HighWarningColor = this.HighWarningColor;
			targetItem.HighWarningStart = this.HighWarningStart;
			targetItem.IndicatorBackgroundHighColor = this.IndicatorBackgroundHighColor;
			targetItem.IndicatorBackgroundLowColor = this.IndicatorBackgroundLowColor;
			targetItem.ShowHighWarning = this.ShowHighWarning;
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x0033A324 File Offset: 0x00338524
		public void ApplySettingsFromRealItem(DashboardItem realItem)
		{
			Type type = realItem.GetType();
			foreach (PropertyInfo propertyInfo in base.GetType().GetProperties())
			{
				PropertyInfo property = type.GetProperty(propertyInfo.Name);
				if (property != null)
				{
					object value = property.GetValue(realItem);
					propertyInfo.SetValue(this, value);
				}
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06004144 RID: 16708 RVA: 0x0033A381 File Offset: 0x00338581
		// (set) Token: 0x06004145 RID: 16709 RVA: 0x0033A389 File Offset: 0x00338589
		public DashboardItemTypes ItemType
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ItemType>k__BackingField = value;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x0033A392 File Offset: 0x00338592
		// (set) Token: 0x06004147 RID: 16711 RVA: 0x0033A39A File Offset: 0x0033859A
		public double Maximum
		{
			[CompilerGenerated]
			get
			{
				return this.<Maximum>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Maximum>k__BackingField = value;
			}
		} = 100.0;

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06004148 RID: 16712 RVA: 0x0033A3A3 File Offset: 0x003385A3
		// (set) Token: 0x06004149 RID: 16713 RVA: 0x0033A3AB File Offset: 0x003385AB
		public double Minimum
		{
			[CompilerGenerated]
			get
			{
				return this.<Minimum>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Minimum>k__BackingField = value;
			}
		}

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x0600414A RID: 16714 RVA: 0x0033A3B4 File Offset: 0x003385B4
		// (set) Token: 0x0600414B RID: 16715 RVA: 0x0033A3BC File Offset: 0x003385BC
		public int PID_Id
		{
			get
			{
				return this._PID_Id;
			}
			set
			{
				if (value == 544)
				{
					this._PID_Id = 231;
					return;
				}
				if (value == 545)
				{
					this._PID_Id = 234;
					return;
				}
				this._PID_Id = value;
			}
		}

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x0600414C RID: 16716 RVA: 0x0033A3ED File Offset: 0x003385ED
		// (set) Token: 0x0600414D RID: 16717 RVA: 0x0033A3F5 File Offset: 0x003385F5
		public bool ShowDefaultBackground
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowDefaultBackground>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowDefaultBackground>k__BackingField = value;
			}
		}

		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x0600414E RID: 16718 RVA: 0x0033A3FE File Offset: 0x003385FE
		// (set) Token: 0x0600414F RID: 16719 RVA: 0x0033A406 File Offset: 0x00338606
		public double FrameSize
		{
			[CompilerGenerated]
			get
			{
				return this.<FrameSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FrameSize>k__BackingField = value;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x0033A40F File Offset: 0x0033860F
		// (set) Token: 0x06004151 RID: 16721 RVA: 0x0033A417 File Offset: 0x00338617
		[JsonConverter(typeof(JsonColorConverter))]
		public Color BackgroundColor
		{
			[CompilerGenerated]
			get
			{
				return this.<BackgroundColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BackgroundColor>k__BackingField = value;
			}
		}

		// Token: 0x17001540 RID: 5440
		// (get) Token: 0x06004152 RID: 16722 RVA: 0x0033A420 File Offset: 0x00338620
		// (set) Token: 0x06004153 RID: 16723 RVA: 0x0033A428 File Offset: 0x00338628
		[JsonConverter(typeof(JsonColorConverter))]
		public Color FrameColor
		{
			[CompilerGenerated]
			get
			{
				return this.<FrameColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FrameColor>k__BackingField = value;
			}
		}

		// Token: 0x17001541 RID: 5441
		// (get) Token: 0x06004154 RID: 16724 RVA: 0x0033A431 File Offset: 0x00338631
		// (set) Token: 0x06004155 RID: 16725 RVA: 0x0033A439 File Offset: 0x00338639
		[JsonConverter(typeof(JsonColorConverter))]
		public Color TitleTextColor
		{
			[CompilerGenerated]
			get
			{
				return this.<TitleTextColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TitleTextColor>k__BackingField = value;
			}
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x06004156 RID: 16726 RVA: 0x0033A442 File Offset: 0x00338642
		// (set) Token: 0x06004157 RID: 16727 RVA: 0x0033A44A File Offset: 0x0033864A
		public double TitleFontSize
		{
			[CompilerGenerated]
			get
			{
				return this.<TitleFontSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TitleFontSize>k__BackingField = value;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06004158 RID: 16728 RVA: 0x0033A453 File Offset: 0x00338653
		// (set) Token: 0x06004159 RID: 16729 RVA: 0x0033A45B File Offset: 0x0033865B
		[JsonConverter(typeof(JsonColorConverter))]
		public Color ValueNormalTextColor
		{
			[CompilerGenerated]
			get
			{
				return this.<ValueNormalTextColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValueNormalTextColor>k__BackingField = value;
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x0600415A RID: 16730 RVA: 0x0033A464 File Offset: 0x00338664
		// (set) Token: 0x0600415B RID: 16731 RVA: 0x0033A46C File Offset: 0x0033866C
		public double ValueFontSize
		{
			[CompilerGenerated]
			get
			{
				return this.<ValueFontSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValueFontSize>k__BackingField = value;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x0600415C RID: 16732 RVA: 0x0033A475 File Offset: 0x00338675
		// (set) Token: 0x0600415D RID: 16733 RVA: 0x0033A47D File Offset: 0x0033867D
		[JsonConverter(typeof(JsonColorConverter))]
		public Color UnitsTextColor
		{
			[CompilerGenerated]
			get
			{
				return this.<UnitsTextColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UnitsTextColor>k__BackingField = value;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x0033A486 File Offset: 0x00338686
		// (set) Token: 0x0600415F RID: 16735 RVA: 0x0033A48E File Offset: 0x0033868E
		public double UnitsFontSize
		{
			[CompilerGenerated]
			get
			{
				return this.<UnitsFontSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UnitsFontSize>k__BackingField = value;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x0033A497 File Offset: 0x00338697
		// (set) Token: 0x06004161 RID: 16737 RVA: 0x0033A49F File Offset: 0x0033869F
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeLabelColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeLabelColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeLabelColor>k__BackingField = value;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06004162 RID: 16738 RVA: 0x0033A4A8 File Offset: 0x003386A8
		// (set) Token: 0x06004163 RID: 16739 RVA: 0x0033A4B0 File Offset: 0x003386B0
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeRimColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeRimColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeRimColor>k__BackingField = value;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06004164 RID: 16740 RVA: 0x0033A4B9 File Offset: 0x003386B9
		// (set) Token: 0x06004165 RID: 16741 RVA: 0x0033A4C1 File Offset: 0x003386C1
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeTickColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeTickColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeTickColor>k__BackingField = value;
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x0033A4CA File Offset: 0x003386CA
		// (set) Token: 0x06004167 RID: 16743 RVA: 0x0033A4D2 File Offset: 0x003386D2
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugePointerColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugePointerColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugePointerColor>k__BackingField = value;
			}
		}

		// Token: 0x1700154B RID: 5451
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x0033A4DB File Offset: 0x003386DB
		// (set) Token: 0x06004169 RID: 16745 RVA: 0x0033A4E3 File Offset: 0x003386E3
		[JsonConverter(typeof(JsonColorConverter))]
		public Color ChartLineColor
		{
			[CompilerGenerated]
			get
			{
				return this.<ChartLineColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChartLineColor>k__BackingField = value;
			}
		}

		// Token: 0x1700154C RID: 5452
		// (get) Token: 0x0600416A RID: 16746 RVA: 0x0033A4EC File Offset: 0x003386EC
		// (set) Token: 0x0600416B RID: 16747 RVA: 0x0033A4F4 File Offset: 0x003386F4
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeRedLineColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeRedLineColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeRedLineColor>k__BackingField = value;
			}
		} = Color.Red;

		// Token: 0x1700154D RID: 5453
		// (get) Token: 0x0600416C RID: 16748 RVA: 0x0033A4FD File Offset: 0x003386FD
		// (set) Token: 0x0600416D RID: 16749 RVA: 0x0033A505 File Offset: 0x00338705
		public bool GaugeShowRedLine
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeShowRedLine>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeShowRedLine>k__BackingField = value;
			}
		}

		// Token: 0x1700154E RID: 5454
		// (get) Token: 0x0600416E RID: 16750 RVA: 0x0033A50E File Offset: 0x0033870E
		// (set) Token: 0x0600416F RID: 16751 RVA: 0x0033A516 File Offset: 0x00338716
		public double GaugeRedLineStart
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeRedLineStart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeRedLineStart>k__BackingField = value;
			}
		}

		// Token: 0x1700154F RID: 5455
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x0033A51F File Offset: 0x0033871F
		// (set) Token: 0x06004171 RID: 16753 RVA: 0x0033A527 File Offset: 0x00338727
		public double GaugeRedLineFinish
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeRedLineFinish>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeRedLineFinish>k__BackingField = value;
			}
		}

		// Token: 0x17001550 RID: 5456
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x0033A530 File Offset: 0x00338730
		// (set) Token: 0x06004173 RID: 16755 RVA: 0x0033A538 File Offset: 0x00338738
		public bool ValueUseLCDFont
		{
			[CompilerGenerated]
			get
			{
				return this.<ValueUseLCDFont>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValueUseLCDFont>k__BackingField = value;
			}
		}

		// Token: 0x17001551 RID: 5457
		// (get) Token: 0x06004174 RID: 16756 RVA: 0x0033A541 File Offset: 0x00338741
		// (set) Token: 0x06004175 RID: 16757 RVA: 0x0033A549 File Offset: 0x00338749
		public double PositionX
		{
			[CompilerGenerated]
			get
			{
				return this.<PositionX>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PositionX>k__BackingField = value;
			}
		}

		// Token: 0x17001552 RID: 5458
		// (get) Token: 0x06004176 RID: 16758 RVA: 0x0033A552 File Offset: 0x00338752
		// (set) Token: 0x06004177 RID: 16759 RVA: 0x0033A55A File Offset: 0x0033875A
		public double PositionY
		{
			[CompilerGenerated]
			get
			{
				return this.<PositionY>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PositionY>k__BackingField = value;
			}
		}

		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06004178 RID: 16760 RVA: 0x0033A563 File Offset: 0x00338763
		// (set) Token: 0x06004179 RID: 16761 RVA: 0x0033A56B File Offset: 0x0033876B
		public double DesiredWidth
		{
			[CompilerGenerated]
			get
			{
				return this.<DesiredWidth>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DesiredWidth>k__BackingField = value;
			}
		}

		// Token: 0x17001554 RID: 5460
		// (get) Token: 0x0600417A RID: 16762 RVA: 0x0033A574 File Offset: 0x00338774
		// (set) Token: 0x0600417B RID: 16763 RVA: 0x0033A57C File Offset: 0x0033877C
		public double DesiredHeight
		{
			[CompilerGenerated]
			get
			{
				return this.<DesiredHeight>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DesiredHeight>k__BackingField = value;
			}
		}

		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x0600417C RID: 16764 RVA: 0x0033A585 File Offset: 0x00338785
		// (set) Token: 0x0600417D RID: 16765 RVA: 0x0033A58D File Offset: 0x0033878D
		public double LinearScaleSize
		{
			[CompilerGenerated]
			get
			{
				return this.<LinearScaleSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LinearScaleSize>k__BackingField = value;
			}
		} = 25.0;

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x0600417E RID: 16766 RVA: 0x0033A596 File Offset: 0x00338796
		// (set) Token: 0x0600417F RID: 16767 RVA: 0x0033A59E File Offset: 0x0033879E
		public bool PlaySound
		{
			[CompilerGenerated]
			get
			{
				return this.<PlaySound>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PlaySound>k__BackingField = value;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06004180 RID: 16768 RVA: 0x0033A5A7 File Offset: 0x003387A7
		// (set) Token: 0x06004181 RID: 16769 RVA: 0x0033A5AF File Offset: 0x003387AF
		public string SoundName
		{
			[CompilerGenerated]
			get
			{
				return this.<SoundName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SoundName>k__BackingField = value;
			}
		} = StaticLists.SoundsList[0];

		// Token: 0x17001558 RID: 5464
		// (get) Token: 0x06004182 RID: 16770 RVA: 0x0033A5B8 File Offset: 0x003387B8
		// (set) Token: 0x06004183 RID: 16771 RVA: 0x0033A5C0 File Offset: 0x003387C0
		public double SoundStart
		{
			[CompilerGenerated]
			get
			{
				return this.<SoundStart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SoundStart>k__BackingField = value;
			}
		}

		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06004184 RID: 16772 RVA: 0x0033A5C9 File Offset: 0x003387C9
		// (set) Token: 0x06004185 RID: 16773 RVA: 0x0033A5D1 File Offset: 0x003387D1
		public int ValueFormat
		{
			[CompilerGenerated]
			get
			{
				return this.<ValueFormat>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ValueFormat>k__BackingField = value;
			}
		} = 2;

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06004186 RID: 16774 RVA: 0x0033A5DA File Offset: 0x003387DA
		// (set) Token: 0x06004187 RID: 16775 RVA: 0x0033A5E2 File Offset: 0x003387E2
		[JsonConverter(typeof(JsonColorConverter))]
		public Color LowWarningColor
		{
			[CompilerGenerated]
			get
			{
				return this.<LowWarningColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LowWarningColor>k__BackingField = value;
			}
		} = Color.Blue;

		// Token: 0x1700155B RID: 5467
		// (get) Token: 0x06004188 RID: 16776 RVA: 0x0033A5EB File Offset: 0x003387EB
		// (set) Token: 0x06004189 RID: 16777 RVA: 0x0033A5F3 File Offset: 0x003387F3
		public bool ShowLowWarning
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowLowWarning>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowLowWarning>k__BackingField = value;
			}
		}

		// Token: 0x1700155C RID: 5468
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x0033A5FC File Offset: 0x003387FC
		// (set) Token: 0x0600418B RID: 16779 RVA: 0x0033A604 File Offset: 0x00338804
		public double LowWarningStart
		{
			[CompilerGenerated]
			get
			{
				return this.<LowWarningStart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LowWarningStart>k__BackingField = value;
			}
		}

		// Token: 0x1700155D RID: 5469
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x0033A60D File Offset: 0x0033880D
		// (set) Token: 0x0600418D RID: 16781 RVA: 0x0033A615 File Offset: 0x00338815
		public bool PlaySoundLow
		{
			[CompilerGenerated]
			get
			{
				return this.<PlaySoundLow>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PlaySoundLow>k__BackingField = value;
			}
		}

		// Token: 0x1700155E RID: 5470
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0033A61E File Offset: 0x0033881E
		// (set) Token: 0x0600418F RID: 16783 RVA: 0x0033A626 File Offset: 0x00338826
		public string SoundNameLow
		{
			[CompilerGenerated]
			get
			{
				return this.<SoundNameLow>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SoundNameLow>k__BackingField = value;
			}
		} = StaticLists.SoundsList[0];

		// Token: 0x1700155F RID: 5471
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x0033A62F File Offset: 0x0033882F
		// (set) Token: 0x06004191 RID: 16785 RVA: 0x0033A637 File Offset: 0x00338837
		public double SoundStartLow
		{
			[CompilerGenerated]
			get
			{
				return this.<SoundStartLow>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SoundStartLow>k__BackingField = value;
			}
		}

		// Token: 0x17001560 RID: 5472
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x0033A640 File Offset: 0x00338840
		// (set) Token: 0x06004193 RID: 16787 RVA: 0x0033A648 File Offset: 0x00338848
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeKnobColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeKnobColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeKnobColor>k__BackingField = value;
			}
		} = Color.SlateGray;

		// Token: 0x17001561 RID: 5473
		// (get) Token: 0x06004194 RID: 16788 RVA: 0x0033A651 File Offset: 0x00338851
		// (set) Token: 0x06004195 RID: 16789 RVA: 0x0033A659 File Offset: 0x00338859
		public string CustomName
		{
			[CompilerGenerated]
			get
			{
				return this.<CustomName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CustomName>k__BackingField = value;
			}
		} = "";

		// Token: 0x17001562 RID: 5474
		// (get) Token: 0x06004196 RID: 16790 RVA: 0x0033A662 File Offset: 0x00338862
		// (set) Token: 0x06004197 RID: 16791 RVA: 0x0033A66A File Offset: 0x0033886A
		public bool OverrideName
		{
			[CompilerGenerated]
			get
			{
				return this.<OverrideName>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OverrideName>k__BackingField = value;
			}
		}

		// Token: 0x17001563 RID: 5475
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x0033A673 File Offset: 0x00338873
		// (set) Token: 0x06004199 RID: 16793 RVA: 0x0033A67B File Offset: 0x0033887B
		public bool UseCustomMinMax
		{
			[CompilerGenerated]
			get
			{
				return this.<UseCustomMinMax>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseCustomMinMax>k__BackingField = value;
			}
		}

		// Token: 0x17001564 RID: 5476
		// (get) Token: 0x0600419A RID: 16794 RVA: 0x0033A684 File Offset: 0x00338884
		// (set) Token: 0x0600419B RID: 16795 RVA: 0x0033A68C File Offset: 0x0033888C
		public bool ShowValue
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowValue>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001565 RID: 5477
		// (get) Token: 0x0600419C RID: 16796 RVA: 0x0033A695 File Offset: 0x00338895
		// (set) Token: 0x0600419D RID: 16797 RVA: 0x0033A69D File Offset: 0x0033889D
		public bool LinearOrientationHorizontal
		{
			[CompilerGenerated]
			get
			{
				return this.<LinearOrientationHorizontal>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LinearOrientationHorizontal>k__BackingField = value;
			}
		} = true;

		// Token: 0x17001566 RID: 5478
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x0033A6A6 File Offset: 0x003388A6
		// (set) Token: 0x0600419F RID: 16799 RVA: 0x0033A6AE File Offset: 0x003388AE
		public int SegmentCount
		{
			[CompilerGenerated]
			get
			{
				return this.<SegmentCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SegmentCount>k__BackingField = value;
			}
		}

		// Token: 0x17001567 RID: 5479
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x0033A6B7 File Offset: 0x003388B7
		// (set) Token: 0x060041A1 RID: 16801 RVA: 0x0033A6BF File Offset: 0x003388BF
		public double CustomInterval
		{
			[CompilerGenerated]
			get
			{
				return this.<CustomInterval>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CustomInterval>k__BackingField = value;
			}
		} = 5.0;

		// Token: 0x17001568 RID: 5480
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x0033A6C8 File Offset: 0x003388C8
		// (set) Token: 0x060041A3 RID: 16803 RVA: 0x0033A6D0 File Offset: 0x003388D0
		public bool UseCustomInterval
		{
			[CompilerGenerated]
			get
			{
				return this.<UseCustomInterval>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UseCustomInterval>k__BackingField = value;
			}
		}

		// Token: 0x17001569 RID: 5481
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0033A6D9 File Offset: 0x003388D9
		// (set) Token: 0x060041A5 RID: 16805 RVA: 0x0033A6E1 File Offset: 0x003388E1
		public Thickness CornerRadius
		{
			[CompilerGenerated]
			get
			{
				return this.<CornerRadius>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CornerRadius>k__BackingField = value;
			}
		} = new Thickness(0.0);

		// Token: 0x1700156A RID: 5482
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x0033A6EA File Offset: 0x003388EA
		// (set) Token: 0x060041A7 RID: 16807 RVA: 0x0033A6F2 File Offset: 0x003388F2
		public double MinMaxAvgFontSize
		{
			[CompilerGenerated]
			get
			{
				return this.<MinMaxAvgFontSize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MinMaxAvgFontSize>k__BackingField = value;
			}
		} = 9.0;

		// Token: 0x1700156B RID: 5483
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x0033A6FB File Offset: 0x003388FB
		// (set) Token: 0x060041A9 RID: 16809 RVA: 0x0033A703 File Offset: 0x00338903
		public bool ShowMinMax
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowMinMax>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowMinMax>k__BackingField = value;
			}
		} = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x1700156C RID: 5484
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x0033A70C File Offset: 0x0033890C
		// (set) Token: 0x060041AB RID: 16811 RVA: 0x0033A714 File Offset: 0x00338914
		public bool ShowAvg
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowAvg>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowAvg>k__BackingField = value;
			}
		}

		// Token: 0x1700156D RID: 5485
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x0033A71D File Offset: 0x0033891D
		// (set) Token: 0x060041AD RID: 16813 RVA: 0x0033A725 File Offset: 0x00338925
		[JsonConverter(typeof(JsonColorConverter))]
		public Color MinMaxAvgColor
		{
			[CompilerGenerated]
			get
			{
				return this.<MinMaxAvgColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MinMaxAvgColor>k__BackingField = value;
			}
		} = Color.FromHex("FFA500");

		// Token: 0x1700156E RID: 5486
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x0033A72E File Offset: 0x0033892E
		// (set) Token: 0x060041AF RID: 16815 RVA: 0x0033A736 File Offset: 0x00338936
		public bool SetMinMaxAvgOnlyVisibleArea
		{
			[CompilerGenerated]
			get
			{
				return this.<SetMinMaxAvgOnlyVisibleArea>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SetMinMaxAvgOnlyVisibleArea>k__BackingField = value;
			}
		} = SharedSettings.Current.SetChartMinMaxOnlyVisibleArea;

		// Token: 0x1700156F RID: 5487
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x0033A73F File Offset: 0x0033893F
		// (set) Token: 0x060041B1 RID: 16817 RVA: 0x0033A747 File Offset: 0x00338947
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GradientColor1
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientColor1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientColor1>k__BackingField = value;
			}
		} = Color.FromHex("FF124AA0");

		// Token: 0x17001570 RID: 5488
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x0033A750 File Offset: 0x00338950
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x0033A758 File Offset: 0x00338958
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GradientColor2
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientColor2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientColor2>k__BackingField = value;
			}
		} = Color.FromHex("FF072A72");

		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x0033A761 File Offset: 0x00338961
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x0033A769 File Offset: 0x00338969
		public double GradientOffsetPoint1
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientOffsetPoint1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientOffsetPoint1>k__BackingField = value;
			}
		} = 0.153;

		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x0033A772 File Offset: 0x00338972
		// (set) Token: 0x060041B7 RID: 16823 RVA: 0x0033A77A File Offset: 0x0033897A
		public double GradientOffsetPoint2
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientOffsetPoint2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientOffsetPoint2>k__BackingField = value;
			}
		} = 0.984;

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x0033A783 File Offset: 0x00338983
		// (set) Token: 0x060041B9 RID: 16825 RVA: 0x0033A78B File Offset: 0x0033898B
		public bool GaugeShowMinMaxMarkers
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeShowMinMaxMarkers>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeShowMinMaxMarkers>k__BackingField = value;
			}
		} = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x0033A794 File Offset: 0x00338994
		// (set) Token: 0x060041BB RID: 16827 RVA: 0x0033A79C File Offset: 0x0033899C
		public Point GradientStartPoint
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientStartPoint>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientStartPoint>k__BackingField = value;
			}
		} = new Point(0.0, 0.0);

		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x0033A7A5 File Offset: 0x003389A5
		// (set) Token: 0x060041BD RID: 16829 RVA: 0x0033A7AD File Offset: 0x003389AD
		public Point GradientEndPoint
		{
			[CompilerGenerated]
			get
			{
				return this.<GradientEndPoint>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GradientEndPoint>k__BackingField = value;
			}
		} = new Point(1.0, 1.0);

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x0033A7B6 File Offset: 0x003389B6
		// (set) Token: 0x060041BF RID: 16831 RVA: 0x0033A7BE File Offset: 0x003389BE
		public double CiruclarGaugeWidth
		{
			[CompilerGenerated]
			get
			{
				return this.<CiruclarGaugeWidth>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CiruclarGaugeWidth>k__BackingField = value;
			}
		} = 12.0;

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x0033A7C7 File Offset: 0x003389C7
		// (set) Token: 0x060041C1 RID: 16833 RVA: 0x0033A7CF File Offset: 0x003389CF
		[JsonConverter(typeof(JsonColorConverter))]
		public Color GaugeBlueLineColor
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeBlueLineColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeBlueLineColor>k__BackingField = value;
			}
		} = Color.Blue;

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x0033A7D8 File Offset: 0x003389D8
		// (set) Token: 0x060041C3 RID: 16835 RVA: 0x0033A7E0 File Offset: 0x003389E0
		public bool GaugeShowBlueLine
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeShowBlueLine>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeShowBlueLine>k__BackingField = value;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x0033A7E9 File Offset: 0x003389E9
		// (set) Token: 0x060041C5 RID: 16837 RVA: 0x0033A7F1 File Offset: 0x003389F1
		public double GaugeBlueLineStart
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeBlueLineStart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeBlueLineStart>k__BackingField = value;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x0033A7FA File Offset: 0x003389FA
		// (set) Token: 0x060041C7 RID: 16839 RVA: 0x0033A802 File Offset: 0x00338A02
		public double GaugeBlueLineFinish
		{
			[CompilerGenerated]
			get
			{
				return this.<GaugeBlueLineFinish>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GaugeBlueLineFinish>k__BackingField = value;
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x0033A80B File Offset: 0x00338A0B
		// (set) Token: 0x060041C9 RID: 16841 RVA: 0x0033A813 File Offset: 0x00338A13
		public bool ShowMinMaxPointers
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowMinMaxPointers>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowMinMaxPointers>k__BackingField = value;
			}
		} = SharedSettings.Current.ShowMinMaxValues;

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x0033A81C File Offset: 0x00338A1C
		// (set) Token: 0x060041CB RID: 16843 RVA: 0x0033A824 File Offset: 0x00338A24
		[JsonConverter(typeof(JsonColorConverter))]
		public Color MinMaxPointersColor
		{
			[CompilerGenerated]
			get
			{
				return this.<MinMaxPointersColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MinMaxPointersColor>k__BackingField = value;
			}
		} = Color.FromHex("FFA500");

		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x0033A82D File Offset: 0x00338A2D
		// (set) Token: 0x060041CD RID: 16845 RVA: 0x0033A835 File Offset: 0x00338A35
		public ChartItemTypes ChartItemType
		{
			[CompilerGenerated]
			get
			{
				return this.<ChartItemType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChartItemType>k__BackingField = value;
			}
		}

		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x0033A83E File Offset: 0x00338A3E
		// (set) Token: 0x060041CF RID: 16847 RVA: 0x0033A846 File Offset: 0x00338A46
		public int LiveDataShowTime
		{
			[CompilerGenerated]
			get
			{
				return this.<LiveDataShowTime>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LiveDataShowTime>k__BackingField = value;
			}
		} = SharedSettings.Current.LiveDataShowTime;

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x0033A84F File Offset: 0x00338A4F
		// (set) Token: 0x060041D1 RID: 16849 RVA: 0x0033A857 File Offset: 0x00338A57
		public bool ChartValuePositionCenter
		{
			[CompilerGenerated]
			get
			{
				return this.<ChartValuePositionCenter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChartValuePositionCenter>k__BackingField = value;
			}
		}

		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x0033A860 File Offset: 0x00338A60
		// (set) Token: 0x060041D3 RID: 16851 RVA: 0x0033A868 File Offset: 0x00338A68
		public int ChartLineWidth
		{
			[CompilerGenerated]
			get
			{
				return this.<ChartLineWidth>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ChartLineWidth>k__BackingField = value;
			}
		} = 1;

		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x0033A871 File Offset: 0x00338A71
		// (set) Token: 0x060041D5 RID: 16853 RVA: 0x0033A879 File Offset: 0x00338A79
		public List<int> PID_IDs
		{
			[CompilerGenerated]
			get
			{
				return this.<PID_IDs>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PID_IDs>k__BackingField = value;
			}
		} = new List<int>(0);

		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x0033A882 File Offset: 0x00338A82
		// (set) Token: 0x060041D7 RID: 16855 RVA: 0x0033A88A File Offset: 0x00338A8A
		[JsonConverter(typeof(JsonColorConverter))]
		public Color HighWarningColor
		{
			[CompilerGenerated]
			get
			{
				return this.<HighWarningColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HighWarningColor>k__BackingField = value;
			}
		} = Color.Red;

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x0033A893 File Offset: 0x00338A93
		// (set) Token: 0x060041D9 RID: 16857 RVA: 0x0033A89B File Offset: 0x00338A9B
		public double HighWarningStart
		{
			[CompilerGenerated]
			get
			{
				return this.<HighWarningStart>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HighWarningStart>k__BackingField = value;
			}
		} = 6000.0;

		// Token: 0x17001584 RID: 5508
		// (get) Token: 0x060041DA RID: 16858 RVA: 0x0033A8A4 File Offset: 0x00338AA4
		// (set) Token: 0x060041DB RID: 16859 RVA: 0x0033A8AC File Offset: 0x00338AAC
		public bool ShowHighWarning
		{
			[CompilerGenerated]
			get
			{
				return this.<ShowHighWarning>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShowHighWarning>k__BackingField = value;
			}
		}

		// Token: 0x17001585 RID: 5509
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x0033A8B5 File Offset: 0x00338AB5
		// (set) Token: 0x060041DD RID: 16861 RVA: 0x0033A8BD File Offset: 0x00338ABD
		[JsonConverter(typeof(JsonColorConverter))]
		public Color IndicatorBackgroundLowColor
		{
			[CompilerGenerated]
			get
			{
				return this.<IndicatorBackgroundLowColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IndicatorBackgroundLowColor>k__BackingField = value;
			}
		} = Color.Transparent;

		// Token: 0x17001586 RID: 5510
		// (get) Token: 0x060041DE RID: 16862 RVA: 0x0033A8C6 File Offset: 0x00338AC6
		// (set) Token: 0x060041DF RID: 16863 RVA: 0x0033A8CE File Offset: 0x00338ACE
		[JsonConverter(typeof(JsonColorConverter))]
		public Color IndicatorBackgroundHighColor
		{
			[CompilerGenerated]
			get
			{
				return this.<IndicatorBackgroundHighColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IndicatorBackgroundHighColor>k__BackingField = value;
			}
		} = Color.Transparent;

		// Token: 0x04002820 RID: 10272
		[CompilerGenerated]
		private DashboardItemTypes <ItemType>k__BackingField;

		// Token: 0x04002821 RID: 10273
		[CompilerGenerated]
		private double <Maximum>k__BackingField;

		// Token: 0x04002822 RID: 10274
		[CompilerGenerated]
		private double <Minimum>k__BackingField;

		// Token: 0x04002823 RID: 10275
		private int _PID_Id;

		// Token: 0x04002824 RID: 10276
		[CompilerGenerated]
		private bool <ShowDefaultBackground>k__BackingField;

		// Token: 0x04002825 RID: 10277
		[CompilerGenerated]
		private double <FrameSize>k__BackingField;

		// Token: 0x04002826 RID: 10278
		[CompilerGenerated]
		private Color <BackgroundColor>k__BackingField;

		// Token: 0x04002827 RID: 10279
		[CompilerGenerated]
		private Color <FrameColor>k__BackingField;

		// Token: 0x04002828 RID: 10280
		[CompilerGenerated]
		private Color <TitleTextColor>k__BackingField;

		// Token: 0x04002829 RID: 10281
		[CompilerGenerated]
		private double <TitleFontSize>k__BackingField;

		// Token: 0x0400282A RID: 10282
		[CompilerGenerated]
		private Color <ValueNormalTextColor>k__BackingField;

		// Token: 0x0400282B RID: 10283
		[CompilerGenerated]
		private double <ValueFontSize>k__BackingField;

		// Token: 0x0400282C RID: 10284
		[CompilerGenerated]
		private Color <UnitsTextColor>k__BackingField;

		// Token: 0x0400282D RID: 10285
		[CompilerGenerated]
		private double <UnitsFontSize>k__BackingField;

		// Token: 0x0400282E RID: 10286
		[CompilerGenerated]
		private Color <GaugeLabelColor>k__BackingField;

		// Token: 0x0400282F RID: 10287
		[CompilerGenerated]
		private Color <GaugeRimColor>k__BackingField;

		// Token: 0x04002830 RID: 10288
		[CompilerGenerated]
		private Color <GaugeTickColor>k__BackingField;

		// Token: 0x04002831 RID: 10289
		[CompilerGenerated]
		private Color <GaugePointerColor>k__BackingField;

		// Token: 0x04002832 RID: 10290
		[CompilerGenerated]
		private Color <ChartLineColor>k__BackingField;

		// Token: 0x04002833 RID: 10291
		[CompilerGenerated]
		private Color <GaugeRedLineColor>k__BackingField;

		// Token: 0x04002834 RID: 10292
		[CompilerGenerated]
		private bool <GaugeShowRedLine>k__BackingField;

		// Token: 0x04002835 RID: 10293
		[CompilerGenerated]
		private double <GaugeRedLineStart>k__BackingField;

		// Token: 0x04002836 RID: 10294
		[CompilerGenerated]
		private double <GaugeRedLineFinish>k__BackingField;

		// Token: 0x04002837 RID: 10295
		[CompilerGenerated]
		private bool <ValueUseLCDFont>k__BackingField;

		// Token: 0x04002838 RID: 10296
		[CompilerGenerated]
		private double <PositionX>k__BackingField;

		// Token: 0x04002839 RID: 10297
		[CompilerGenerated]
		private double <PositionY>k__BackingField;

		// Token: 0x0400283A RID: 10298
		[CompilerGenerated]
		private double <DesiredWidth>k__BackingField;

		// Token: 0x0400283B RID: 10299
		[CompilerGenerated]
		private double <DesiredHeight>k__BackingField;

		// Token: 0x0400283C RID: 10300
		[CompilerGenerated]
		private double <LinearScaleSize>k__BackingField;

		// Token: 0x0400283D RID: 10301
		[CompilerGenerated]
		private bool <PlaySound>k__BackingField;

		// Token: 0x0400283E RID: 10302
		[CompilerGenerated]
		private string <SoundName>k__BackingField;

		// Token: 0x0400283F RID: 10303
		[CompilerGenerated]
		private double <SoundStart>k__BackingField;

		// Token: 0x04002840 RID: 10304
		[CompilerGenerated]
		private int <ValueFormat>k__BackingField;

		// Token: 0x04002841 RID: 10305
		[CompilerGenerated]
		private Color <LowWarningColor>k__BackingField;

		// Token: 0x04002842 RID: 10306
		[CompilerGenerated]
		private bool <ShowLowWarning>k__BackingField;

		// Token: 0x04002843 RID: 10307
		[CompilerGenerated]
		private double <LowWarningStart>k__BackingField;

		// Token: 0x04002844 RID: 10308
		[CompilerGenerated]
		private bool <PlaySoundLow>k__BackingField;

		// Token: 0x04002845 RID: 10309
		[CompilerGenerated]
		private string <SoundNameLow>k__BackingField;

		// Token: 0x04002846 RID: 10310
		[CompilerGenerated]
		private double <SoundStartLow>k__BackingField;

		// Token: 0x04002847 RID: 10311
		[CompilerGenerated]
		private Color <GaugeKnobColor>k__BackingField;

		// Token: 0x04002848 RID: 10312
		[CompilerGenerated]
		private string <CustomName>k__BackingField;

		// Token: 0x04002849 RID: 10313
		[CompilerGenerated]
		private bool <OverrideName>k__BackingField;

		// Token: 0x0400284A RID: 10314
		[CompilerGenerated]
		private bool <UseCustomMinMax>k__BackingField;

		// Token: 0x0400284B RID: 10315
		[CompilerGenerated]
		private bool <ShowValue>k__BackingField;

		// Token: 0x0400284C RID: 10316
		[CompilerGenerated]
		private bool <LinearOrientationHorizontal>k__BackingField;

		// Token: 0x0400284D RID: 10317
		[CompilerGenerated]
		private int <SegmentCount>k__BackingField;

		// Token: 0x0400284E RID: 10318
		[CompilerGenerated]
		private double <CustomInterval>k__BackingField;

		// Token: 0x0400284F RID: 10319
		[CompilerGenerated]
		private bool <UseCustomInterval>k__BackingField;

		// Token: 0x04002850 RID: 10320
		[CompilerGenerated]
		private Thickness <CornerRadius>k__BackingField;

		// Token: 0x04002851 RID: 10321
		[CompilerGenerated]
		private double <MinMaxAvgFontSize>k__BackingField;

		// Token: 0x04002852 RID: 10322
		[CompilerGenerated]
		private bool <ShowMinMax>k__BackingField;

		// Token: 0x04002853 RID: 10323
		[CompilerGenerated]
		private bool <ShowAvg>k__BackingField;

		// Token: 0x04002854 RID: 10324
		[CompilerGenerated]
		private Color <MinMaxAvgColor>k__BackingField;

		// Token: 0x04002855 RID: 10325
		[CompilerGenerated]
		private bool <SetMinMaxAvgOnlyVisibleArea>k__BackingField;

		// Token: 0x04002856 RID: 10326
		[CompilerGenerated]
		private Color <GradientColor1>k__BackingField;

		// Token: 0x04002857 RID: 10327
		[CompilerGenerated]
		private Color <GradientColor2>k__BackingField;

		// Token: 0x04002858 RID: 10328
		[CompilerGenerated]
		private double <GradientOffsetPoint1>k__BackingField;

		// Token: 0x04002859 RID: 10329
		[CompilerGenerated]
		private double <GradientOffsetPoint2>k__BackingField;

		// Token: 0x0400285A RID: 10330
		[CompilerGenerated]
		private bool <GaugeShowMinMaxMarkers>k__BackingField;

		// Token: 0x0400285B RID: 10331
		[CompilerGenerated]
		private Point <GradientStartPoint>k__BackingField;

		// Token: 0x0400285C RID: 10332
		[CompilerGenerated]
		private Point <GradientEndPoint>k__BackingField;

		// Token: 0x0400285D RID: 10333
		[CompilerGenerated]
		private double <CiruclarGaugeWidth>k__BackingField;

		// Token: 0x0400285E RID: 10334
		[CompilerGenerated]
		private Color <GaugeBlueLineColor>k__BackingField;

		// Token: 0x0400285F RID: 10335
		[CompilerGenerated]
		private bool <GaugeShowBlueLine>k__BackingField;

		// Token: 0x04002860 RID: 10336
		[CompilerGenerated]
		private double <GaugeBlueLineStart>k__BackingField;

		// Token: 0x04002861 RID: 10337
		[CompilerGenerated]
		private double <GaugeBlueLineFinish>k__BackingField;

		// Token: 0x04002862 RID: 10338
		[CompilerGenerated]
		private bool <ShowMinMaxPointers>k__BackingField;

		// Token: 0x04002863 RID: 10339
		[CompilerGenerated]
		private Color <MinMaxPointersColor>k__BackingField;

		// Token: 0x04002864 RID: 10340
		[CompilerGenerated]
		private ChartItemTypes <ChartItemType>k__BackingField;

		// Token: 0x04002865 RID: 10341
		[CompilerGenerated]
		private int <LiveDataShowTime>k__BackingField;

		// Token: 0x04002866 RID: 10342
		[CompilerGenerated]
		private bool <ChartValuePositionCenter>k__BackingField;

		// Token: 0x04002867 RID: 10343
		[CompilerGenerated]
		private int <ChartLineWidth>k__BackingField;

		// Token: 0x04002868 RID: 10344
		[CompilerGenerated]
		private List<int> <PID_IDs>k__BackingField;

		// Token: 0x04002869 RID: 10345
		[CompilerGenerated]
		private Color <HighWarningColor>k__BackingField;

		// Token: 0x0400286A RID: 10346
		[CompilerGenerated]
		private double <HighWarningStart>k__BackingField;

		// Token: 0x0400286B RID: 10347
		[CompilerGenerated]
		private bool <ShowHighWarning>k__BackingField;

		// Token: 0x0400286C RID: 10348
		[CompilerGenerated]
		private Color <IndicatorBackgroundLowColor>k__BackingField;

		// Token: 0x0400286D RID: 10349
		[CompilerGenerated]
		private Color <IndicatorBackgroundHighColor>k__BackingField;
	}
}
