using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfRangeSlider.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006F9 RID: 1785
	[XamlCompilation(2)]
	[XamlFilePath("DataRecorder\\MapWithMultiChartV2.xaml")]
	public class MapWithMultiChartV2 : ContentPage
	{
		// Token: 0x06003CC4 RID: 15556 RVA: 0x0031F0E8 File Offset: 0x0031D2E8
		public MapWithMultiChartV2(IDataRecordContainer Recorder, List<PointWithTime> allPositions)
		{
			this.InitializeComponent();
			base.Title = Recorder.Title;
			if (Device.Idiom == 2)
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["tabletTemplate"];
			}
			else
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["mobileTemplate"];
			}
			base.ToolbarItems.Add(new ToolbarItem("list", "", delegate
			{
				this.chart.IsVisible = !this.chart.IsVisible;
				this.lv.IsVisible = !this.chart.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_list.png", delegate
			{
				this.Legend.IsVisible = !this.Legend.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			List<DataRecord> list = Recorder.Records.Where((DataRecord x) => x.IsVisible).ToList<DataRecord>();
			if (allPositions.Count == 0)
			{
				return;
			}
			this.recordsItemsWithElement = list.Select((DataRecord x) => new DataRecordElementCollectionWithContainerReference(x)).ToList<DataRecordElementCollectionWithContainerReference>();
			AutoUpdatingListForHidingItems<DataRecordElementCollectionWithContainerReference> autoUpdatingListForHidingItems = new AutoUpdatingListForHidingItems<DataRecordElementCollectionWithContainerReference>();
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in this.recordsItemsWithElement)
			{
				autoUpdatingListForHidingItems.Add(dataRecordElementCollectionWithContainerReference);
			}
			this.lv.ItemsSource = autoUpdatingListForHidingItems;
			this.allPositions = allPositions.ToList<PointWithTime>();
			this.filteredPositions = this.FilterPositionsV2(allPositions, list);
			this.DrawLineForAll();
			this.DrawLineStartingExistingAndSetPositionsForSelected();
			if (this.recordsItemsWithElement.Select((DataRecordElementCollectionWithContainerReference x) => x.DataRecord.Units).Distinct<UnitsHelper.Units>().Count<UnitsHelper.Units>() == 2)
			{
				this.BuildInterfaceFor2Units(Recorder, this.recordsItemsWithElement);
			}
			else
			{
				this.BuildInterfaceForManyUnits(Recorder, this.recordsItemsWithElement);
			}
			if (this.filteredPositions.Count > 0)
			{
				this.slider.Minimum = this.filteredPositions[0].TimeSeconds;
				this.slider.Maximum = this.filteredPositions[this.filteredPositions.Count - 1].TimeSeconds;
				this.lbStartTime.Text = TimeSpan.FromSeconds(this.filteredPositions[0].TimeSeconds).ToString("hh\\:mm\\:ss");
				this.lbFinishTime.Text = TimeSpan.FromSeconds(this.filteredPositions[this.filteredPositions.Count - 1].TimeSeconds).ToString("hh\\:mm\\:ss");
				this.UpdateForTime(this.PointStart.TimeSeconds);
			}
			List<DataRecordElementCollectionWithContainerReference> list2 = this.recordsItemsWithElement.ToList<DataRecordElementCollectionWithContainerReference>();
			DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference2 = new DataRecordElementCollectionWithContainerReference(new DataRecord
			{
				Name = Translate.GetString("pid_Empty")
			});
			list2.Insert(0, dataRecordElementCollectionWithContainerReference2);
			this.accentPicker.ItemsSource = list2;
			this.accentPicker.SelectedIndex = 0;
			this.accentPicker.SelectedIndexChanged += this.AccentPicker_SelectedIndexChanged;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0031F82C File Offset: 0x0031DA2C
		private async void MapWithMultiChartV2_SizeChanged(object sender, EventArgs e)
		{
			switch (DeviceDisplay.MainDisplayInfo.Rotation)
			{
			case 0:
			case 1:
			case 3:
				this.Legend.DockPosition = 3;
				this.Legend.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
				this.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width;
				break;
			case 2:
			case 4:
				this.Legend.DockPosition = 4;
				this.Legend.BackgroundColor = ((Color)Application.Current.Resources["BackgroundColor"]).MultiplyAlpha(0.5);
				this.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width * 0.25;
				break;
			}
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0031F864 File Offset: 0x0031DA64
		private async void AccentPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			int idx = this.accentPicker.SelectedIndex;
			await Task.Delay(500);
			if (this.accentPicker.SelectedIndex == idx)
			{
				this.activityFrame.IsVisible = true;
				await Task.Delay(100);
				this.DrawAccentLine((DataRecordElementCollectionWithContainerReference)this.accentPicker.SelectedItem);
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x0031F89B File Offset: 0x0031DA9B
		// (set) Token: 0x06003CC8 RID: 15560 RVA: 0x0031F8A4 File Offset: 0x0031DAA4
		public PointWithTime CurrentPoint
		{
			get
			{
				return this._CurrentPoint;
			}
			set
			{
				this._CurrentPoint = value;
				int index = this.CurrentPoint.Index;
				this.labelCurrentPointSb.Clear();
				this.labelCurrentPointSb.Append(TimeSpan.FromSeconds(this._CurrentPoint.TimeSeconds).ToString("hh\\:mm\\:ss"));
				this.labelCurrentPointSb.Append(" [");
				this.labelCurrentPointSb.Append(index.ToString());
				this.labelCurrentPointSb.Append("/");
				this.labelCurrentPointSb.Append(this.filteredPositions.Count.ToString());
				this.labelCurrentPointSb.Append("]");
				this.lbCurrentPoint.Text = this.labelCurrentPointSb.ToString();
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x0031F977 File Offset: 0x0031DB77
		// (set) Token: 0x06003CCA RID: 15562 RVA: 0x0031F97F File Offset: 0x0031DB7F
		public double[] SpeedArray
		{
			[CompilerGenerated]
			get
			{
				return this.<SpeedArray>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SpeedArray>k__BackingField = value;
			}
		} = new double[]
		{
			0.1, 0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 2.0, 3.0, 4.0,
			5.0, 8.0, 10.0, 15.0
		};

		// Token: 0x170013FD RID: 5117
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x0031F988 File Offset: 0x0031DB88
		// (set) Token: 0x06003CCC RID: 15564 RVA: 0x0031F990 File Offset: 0x0031DB90
		public double CurrentSpeed
		{
			get
			{
				return this._CurrentSpeed;
			}
			set
			{
				if (this._CurrentSpeed != value)
				{
					this._CurrentSpeed = value;
					this.OnPropertyChanged("CurrentSpeed");
					this.OnPropertyChanged("CurrentSpeedTitle");
				}
			}
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x0031F9B8 File Offset: 0x0031DBB8
		public string CurrentSpeedTitle
		{
			get
			{
				return this.CurrentSpeed.ToString() + "x";
			}
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0031F9E0 File Offset: 0x0031DBE0
		private void btnStepPrev_Clicked(object sender, EventArgs e)
		{
			if (!this.IsPlaying)
			{
				this.PrevStep();
				return;
			}
			int num = Array.IndexOf<double>(this.SpeedArray, this.CurrentSpeed);
			if (num == 0)
			{
				return;
			}
			this.CurrentSpeed = this.SpeedArray[num - 1];
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0031FA24 File Offset: 0x0031DC24
		private void btnStepNext_Clicked(object sender, EventArgs e)
		{
			if (!this.IsPlaying)
			{
				this.NextStep();
				return;
			}
			int num = Array.IndexOf<double>(this.SpeedArray, this.CurrentSpeed);
			if (num == this.SpeedArray.Length - 1)
			{
				return;
			}
			this.CurrentSpeed = this.SpeedArray[num + 1];
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0031FA70 File Offset: 0x0031DC70
		private void btnPlay_Clicked(object sender, EventArgs e)
		{
			this.IsPlaying = true;
			this.btnPlay.IsVisible = false;
			this.btnPause.IsVisible = true;
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x0031FA91 File Offset: 0x0031DC91
		private void btnPause_Clicked(object sender, EventArgs e)
		{
			this.IsPlaying = false;
			this.btnPlay.IsVisible = true;
			this.btnPause.IsVisible = false;
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x06003CD2 RID: 15570 RVA: 0x0031FAB2 File Offset: 0x0031DCB2
		// (set) Token: 0x06003CD3 RID: 15571 RVA: 0x0031FABA File Offset: 0x0031DCBA
		public bool IsPlaying
		{
			get
			{
				return this._IsPlaying;
			}
			set
			{
				if (this._IsPlaying != value)
				{
					this._IsPlaying = value;
					if (this._IsPlaying)
					{
						this.PlayLoop();
					}
				}
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0031FADC File Offset: 0x0031DCDC
		private async Task PlayLoop()
		{
			while (this.IsPlaying)
			{
				int num = this.CurrentPoint.Index + 1;
				if (num != this.filteredPositions.Count)
				{
					MapWithMultiChartV2.<>c__DisplayClass32_0 CS$<>8__locals1 = new MapWithMultiChartV2.<>c__DisplayClass32_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.nextpoint = this.filteredPositions[num];
					await Task.Delay(TimeSpan.FromSeconds((CS$<>8__locals1.nextpoint.TimeSeconds - this.CurrentPoint.TimeSeconds) / this.CurrentSpeed));
					if (this.IsPlaying)
					{
						await MainThread.InvokeOnMainThreadAsync(delegate
						{
							if (CS$<>8__locals1.<>4__this.Navigation.NavigationStack[CS$<>8__locals1.<>4__this.Navigation.NavigationStack.Count - 1] == CS$<>8__locals1.<>4__this)
							{
								CS$<>8__locals1.<>4__this.UpdateForPosition(CS$<>8__locals1.nextpoint);
							}
						});
						CS$<>8__locals1 = null;
						continue;
					}
				}
				return;
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x0031FB20 File Offset: 0x0031DD20
		public void NextStep()
		{
			int num = this.CurrentPoint.Index + 1;
			if (num < this.filteredPositions.Count)
			{
				PointWithTime pointWithTime = this.filteredPositions[num];
				this.UpdateForPosition(pointWithTime);
			}
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x0031FB60 File Offset: 0x0031DD60
		public void PrevStep()
		{
			int index = this.CurrentPoint.Index;
			if (index == 0)
			{
				return;
			}
			int num = index - 1;
			PointWithTime pointWithTime = this.filteredPositions[num];
			this.UpdateForPosition(pointWithTime);
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x0031FB98 File Offset: 0x0031DD98
		private List<PointWithTime> FilterPositionsV1(List<PointWithTime> allPositions, List<DataRecord> records)
		{
			List<PointWithTime> list = new List<PointWithTime>(allPositions.Count);
			using (List<PointWithTime>.Enumerator enumerator = allPositions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PointWithTime pos = enumerator.Current;
					pos.ToPoint();
					Func<DataRecordElement, bool> <>9__1;
					foreach (DataRecord dataRecord in records)
					{
						IEnumerable<DataRecordElement> enumerable = dataRecord.Elements.Where((DataRecordElement x) => !double.IsNaN(x.Value));
						Func<DataRecordElement, bool> func;
						if ((func = <>9__1) == null)
						{
							func = (<>9__1 = (DataRecordElement x) => pos.Equals(x.Position));
						}
						if (enumerable.Any(func))
						{
							list.Add(pos);
							break;
						}
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				PointWithTime pointWithTime = list[i];
				pointWithTime.Index = i;
				list[i] = pointWithTime;
			}
			list = new List<PointWithTime>(list);
			return list;
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x0031FCD8 File Offset: 0x0031DED8
		private List<PointWithTime> FilterPositionsV2(List<PointWithTime> allPositions, List<DataRecord> records)
		{
			List<PointWithTime> list = new List<PointWithTime>(allPositions.Count);
			foreach (DataRecord dataRecord in records)
			{
				list.AddRange(from x in dataRecord.Elements
					where !double.IsNaN(x.Value)
					select new PointWithTime(x.Position, x.Seconds));
			}
			list = list.OrderBy((PointWithTime x) => x.TimeSeconds).Distinct<PointWithTime>().ToList<PointWithTime>();
			for (int i = 0; i < list.Count; i++)
			{
				PointWithTime pointWithTime = list[i];
				pointWithTime.Index = i;
				list[i] = pointWithTime;
			}
			return list;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x0031FDDC File Offset: 0x0031DFDC
		private void Slider_ValueChanging(object sender, ValueEventArgs e)
		{
			this.UpdateForTime(this.slider.Value);
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Slider_DragStarted(object sender, DragThumbEventArgs e)
		{
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x0031FDDC File Offset: 0x0031DFDC
		private void Slider_DragCompleted(object sender, DragThumbEventArgs e)
		{
			this.UpdateForTime(this.slider.Value);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x0031FDF0 File Offset: 0x0031DFF0
		private PointWithTime GetClosestRecordedPositionToClickedPosition(Point pos)
		{
			return this.filteredPositions.OrderBy((PointWithTime x) => x.GetDistanceToPoint(pos)).First<PointWithTime>();
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x0031FE28 File Offset: 0x0031E028
		private void BuildInterfaceFor2Units(IDataRecordContainer Recorder, List<DataRecordElementCollectionWithContainerReference> visibleCollection)
		{
			UnitsHelper.Units[] array = visibleCollection.Select((DataRecordElementCollectionWithContainerReference x) => x.DataRecord.Units).Distinct<UnitsHelper.Units>().ToArray<UnitsHelper.Units>();
			UnitsHelper.Units leftUnit = array[0];
			UnitsHelper.Units rightUnit = array[1];
			List<DataRecordElementCollectionWithContainerReference> list = visibleCollection.Where((DataRecordElementCollectionWithContainerReference x) => x.DataRecord.Units == leftUnit).ToList<DataRecordElementCollectionWithContainerReference>();
			visibleCollection.Where((DataRecordElementCollectionWithContainerReference x) => x.DataRecord.Units == rightUnit).ToList<DataRecordElementCollectionWithContainerReference>();
			ChartAxisTitle chartAxisTitle = new ChartAxisTitle
			{
				Text = UnitsHelper.GetCaption(leftUnit),
				Margin = new Thickness(1.0, 0.0, 0.0, 0.0),
				FontAttributes = 1
			};
			chartAxisTitle.SetDynamicResource(ChartAxisTitle.TextColorProperty, "ButtonBackgroundColor");
			ChartAxisTitle chartAxisTitle2 = new ChartAxisTitle
			{
				Text = UnitsHelper.GetCaption(rightUnit),
				Margin = new Thickness(0.0, 0.0, 1.0, 0.0),
				FontAttributes = 1
			};
			chartAxisTitle2.SetDynamicResource(ChartAxisTitle.TextColorProperty, "ButtonBackgroundColor");
			NumericalAxis numericalAxis = new NumericalAxis
			{
				EdgeLabelsDrawingMode = 0,
				EdgeLabelsVisibilityMode = 0,
				LabelsIntersectAction = 0,
				RangePadding = 2,
				OpposedPosition = false,
				Title = chartAxisTitle,
				TickPosition = 0,
				LabelStyle = new ChartAxisLabelStyle
				{
					LabelFormat = "0.###"
				}
			};
			NumericalAxis numericalAxis2 = new NumericalAxis
			{
				EdgeLabelsDrawingMode = 0,
				EdgeLabelsVisibilityMode = 0,
				LabelsIntersectAction = 0,
				RangePadding = 2,
				OpposedPosition = true,
				Title = chartAxisTitle2,
				TickPosition = 0,
				LabelStyle = new ChartAxisLabelStyle
				{
					LabelFormat = "0.###"
				}
			};
			this.chart.SecondaryAxis = numericalAxis;
			this.chart.Axes.Add(numericalAxis2);
			if (this.chart.Series == null)
			{
				this.chart.Series = new ChartSeriesCollection();
			}
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in visibleCollection)
			{
				FastLineSeries fastLineSeries = new FastLineSeries
				{
					ItemsSource = dataRecordElementCollectionWithContainerReference,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					EnableDataPointSelection = true,
					Label = dataRecordElementCollectionWithContainerReference.DataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecordElementCollectionWithContainerReference.DataRecord.Units) + "]",
					IsVisibleOnLegend = true,
					ShowTrackballInfo = true
				};
				Binding binding = new Binding("LegendTitleWithValue", 2, null, null, null, dataRecordElementCollectionWithContainerReference);
				fastLineSeries.SetBinding(ChartSeries.LabelProperty, binding);
				if (list.Contains(dataRecordElementCollectionWithContainerReference))
				{
					fastLineSeries.YAxis = numericalAxis;
				}
				else
				{
					fastLineSeries.YAxis = numericalAxis2;
				}
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				this.chart.Series.Add(fastLineSeries);
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x00320160 File Offset: 0x0031E360
		private void BuildInterfaceForManyUnits(IDataRecordContainer Recorder, List<DataRecordElementCollectionWithContainerReference> visibleCollection)
		{
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in visibleCollection)
			{
				DataRecord dataRecord = dataRecordElementCollectionWithContainerReference.DataRecord;
				FastLineSeries fastLineSeries = new FastLineSeries
				{
					ItemsSource = dataRecordElementCollectionWithContainerReference,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					EnableDataPointSelection = true,
					Label = dataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecord.Units) + "]",
					IsVisibleOnLegend = true,
					ShowTrackballInfo = true
				};
				Binding binding = new Binding("LegendTitleWithValue", 2, null, null, null, dataRecordElementCollectionWithContainerReference);
				fastLineSeries.SetBinding(ChartSeries.LabelProperty, binding);
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				this.chart.Series.Add(fastLineSeries);
			}
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00320260 File Offset: 0x0031E460
		private void DrawLineForAll()
		{
			this.DrawLine(this.allPositions, Color.DarkGray, (double)((Device.RuntimePlatform == "iOS") ? 6 : 12));
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x0032028C File Offset: 0x0031E48C
		private Polyline DrawLine(List<PointWithTime> positions, Color color, double thickness)
		{
			if (positions.Count == 0)
			{
				return null;
			}
			Polyline polyline = new Polyline
			{
				StrokeColor = color,
				StrokeWidth = (float)thickness
			};
			foreach (PointWithTime pointWithTime in positions)
			{
				polyline.Geopath.Add(pointWithTime.ToPosition());
			}
			this.map.MapElements.Add(polyline);
			MapSpan.FromCenterAndRadius(positions[0].ToPosition(), Distance.FromMeters(300.0));
			return polyline;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x0032033C File Offset: 0x0031E53C
		private void DrawPointsAsPolylines(List<PointWithTime> positions, Color color, double thickness)
		{
			Distance distance = Distance.FromMeters(5.0);
			foreach (PointWithTime pointWithTime in positions)
			{
				Position position = pointWithTime.ToPosition();
				Circle circle = new Circle
				{
					StrokeColor = color,
					StrokeWidth = (float)thickness,
					Center = position,
					Radius = distance
				};
				this.map.MapElements.Add(circle);
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x003203D0 File Offset: 0x0031E5D0
		private void DrawLineStartingExistingAndSetPositionsForSelected()
		{
			if (this.filteredPositions == null || this.filteredPositions.Count == 0)
			{
				return;
			}
			this.DrawLine(this.filteredPositions, Color.FromHex("0165B9"), (double)((Device.RuntimePlatform == "iOS") ? 6 : 12));
			this.PointStart = this.filteredPositions[0];
			this.PointFinish = this.filteredPositions[this.filteredPositions.Count - 1];
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x00271490 File Offset: 0x0026F690
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DataViewer"), Translate.GetString("ios_DataViewerOne_Info"), "OK");
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00320454 File Offset: 0x0031E654
		private void ChangeZoomMode()
		{
			if (this.zoomBehave.ZoomMode == null)
			{
				this.zoomBehave.ZoomMode = 2;
				this.zoomModeButton.ImageSource = ImageSource.FromFile("resize_xy.png");
				return;
			}
			if (this.zoomBehave.ZoomMode == 2)
			{
				this.zoomBehave.ZoomMode = 1;
				this.zoomModeButton.ImageSource = ImageSource.FromFile("resize_y.png");
				return;
			}
			if (this.zoomBehave.ZoomMode == 1)
			{
				this.zoomBehave.ZoomMode = 0;
				this.zoomModeButton.ImageSource = ImageSource.FromFile("resize_x.png");
			}
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x003204EF File Offset: 0x0031E6EF
		private void zoomModeButton_Clicked(object sender, EventArgs e)
		{
			this.ChangeZoomMode();
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x003204F8 File Offset: 0x0031E6F8
		private void Map_MapClicked(object sender, MapClickedEventArgs e)
		{
			PointWithTime closestRecordedPositionToClickedPosition = this.GetClosestRecordedPositionToClickedPosition(new Point(e.Position.Latitude, e.Position.Longitude));
			this.UpdateForPosition(closestRecordedPositionToClickedPosition);
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x00320534 File Offset: 0x0031E734
		private void UpdateForPosition(PointWithTime selectedPoint)
		{
			this.chart.SuspendSeriesNotification();
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in this.recordsItemsWithElement)
			{
				dataRecordElementCollectionWithContainerReference.UpdateForLastTime(selectedPoint.TimeSeconds);
			}
			this.chart.ResumeSeriesNotification();
			PointWithTime currentPoint = this.CurrentPoint;
			this.CurrentPoint = selectedPoint;
			this.slider.Value = this.CurrentPoint.TimeSeconds;
			if (currentPoint.X != this.CurrentPoint.X && currentPoint.Y != this.CurrentPoint.Y)
			{
				this.UpdateMapMarker();
				this.UpdateCurrentGeoposition();
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00320604 File Offset: 0x0031E804
		private void UpdateForTime(double time)
		{
			PointWithTime pointWithTime = this.FindClosestPositionForTime(time);
			this.chart.SuspendSeriesNotification();
			foreach (ChartSeries chartSeries in this.chart.Series)
			{
				DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference = (DataRecordElementCollectionWithContainerReference)chartSeries.ItemsSource;
				chartSeries.ItemsSource = null;
				dataRecordElementCollectionWithContainerReference.UpdateForLastTime(pointWithTime.TimeSeconds);
				chartSeries.ItemsSource = dataRecordElementCollectionWithContainerReference;
			}
			this.chart.ResumeSeriesNotification();
			PointWithTime currentPoint = this.CurrentPoint;
			this.CurrentPoint = pointWithTime;
			this.slider.Value = this.CurrentPoint.TimeSeconds;
			if (currentPoint.X != this.CurrentPoint.X && currentPoint.Y != this.CurrentPoint.Y)
			{
				this.UpdateMapMarker();
				this.UpdateCurrentGeoposition();
			}
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x003206F8 File Offset: 0x0031E8F8
		private PointWithTime FindClosestPositionForTime(double time)
		{
			double num = double.MaxValue;
			int num2 = 0;
			for (int i = 0; i < this.filteredPositions.Count; i++)
			{
				double num3 = Math.Abs(this.filteredPositions[i].TimeSeconds - time);
				if (num3 < num)
				{
					num2 = i;
					num = num3;
				}
				if (num3 > num)
				{
					return this.filteredPositions[num2];
				}
			}
			return this.filteredPositions[this.filteredPositions.Count - 1];
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x00320778 File Offset: 0x0031E978
		private void UpdateMapMarker()
		{
			if (this.pin == null)
			{
				this.pin = new Pin
				{
					Type = 0
				};
			}
			Position position = this.CurrentPoint.ToPosition();
			this.pin.Position = position;
			this.pin.Label = TimeSpan.FromSeconds(this.CurrentPoint.TimeSeconds).ToString();
			if (!this.map.Pins.Contains(this.pin))
			{
				this.map.Pins.Add(this.pin);
			}
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x0032081C File Offset: 0x0031EA1C
		private void UpdateCurrentGeoposition()
		{
			MapSpan visibleRegion = this.map.VisibleRegion;
			MapSpan mapSpan = MapSpan.FromCenterAndRadius(this.CurrentPoint.ToPosition(), (visibleRegion == null) ? Distance.FromMeters(300.0) : visibleRegion.Radius);
			this.map.MoveToRegion(mapSpan);
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00320874 File Offset: 0x0031EA74
		private void numAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			double num;
			if (double.TryParse(e.LabelContent, out num))
			{
				double num2 = this.xaxis.VisibleMaximum - this.xaxis.VisibleMinimum;
				string text;
				if (num2 > 300.0)
				{
					text = "hh\\:mm";
				}
				else if (num2 <= 300.0 && num2 > 60.0)
				{
					text = "hh\\:mm\\:ss";
				}
				else if (num2 > 2.0 && num2 <= 60.0)
				{
					text = "mm\\:ss\\.ff";
				}
				else
				{
					text = "ss\\.fff";
				}
				TimeSpan.FromSeconds(num);
				string text2 = TimeSpan.FromSeconds(num).ToString(text);
				e.LabelContent = text2;
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00320924 File Offset: 0x0031EB24
		private void DrawAccentLine(DataRecordElementCollectionWithContainerReference rec)
		{
			if (this.accentLinesList != null)
			{
				foreach (Polyline polyline in this.accentLinesList)
				{
					this.map.MapElements.Remove(polyline);
				}
				this.accentLinesList.Clear();
				this.accentLinesList = null;
			}
			this.accentLinesList = this.DrawLineAsPolyLines(rec, (double)((Device.RuntimePlatform == "iOS") ? 4 : 8));
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x003209C0 File Offset: 0x0031EBC0
		private List<Polyline> DrawLineAsPolyLines(DataRecordElementCollectionWithContainerReference rec, double thickness)
		{
			if (rec.DataRecord.Elements.Count == 0)
			{
				return new List<Polyline>(0);
			}
			List<Polyline> list = new List<Polyline>(rec.DataRecord.Elements.Count);
			try
			{
				double num = rec.DataRecord.Elements.Where((DataRecordElement x) => double.IsFinite(x.Value)).Min((DataRecordElement x) => x.Value);
				double num2 = rec.DataRecord.Elements.Where((DataRecordElement x) => double.IsFinite(x.Value)).Max((DataRecordElement x) => x.Value);
				int num3 = 0;
				while (num3 < rec.DataRecord.Elements.Count - 1 && num3 + 1 < rec.DataRecord.Elements.Count)
				{
					DataRecordElement dataRecordElement = rec.DataRecord.Elements[num3];
					DataRecordElement dataRecordElement2 = rec.DataRecord.Elements[num3 + 1];
					if (double.IsFinite(dataRecordElement.Value) && !(dataRecordElement.Position == Point.Zero) && !(dataRecordElement2.Position == Point.Zero))
					{
						Color color = this.GetColor(dataRecordElement.Value, num, num2);
						Polyline polyline = new Polyline
						{
							StrokeColor = color,
							StrokeWidth = (float)thickness
						};
						list.Add(polyline);
						polyline.Geopath.Add(dataRecordElement.Position.ToPosition());
						polyline.Geopath.Add(dataRecordElement2.Position.ToPosition());
						for (int i = num3 + 1; i < rec.DataRecord.Elements.Count - 1; i++)
						{
							DataRecordElement dataRecordElement3 = rec.DataRecord.Elements[i];
							if (dataRecordElement3.Value != dataRecordElement.Value)
							{
								break;
							}
							Position position = dataRecordElement3.Position.ToPosition();
							if (polyline.Geopath[polyline.Geopath.Count - 1] != position)
							{
								polyline.Geopath.Add(dataRecordElement3.Position.ToPosition());
							}
							num3 = i - 1;
						}
						this.map.MapElements.Add(polyline);
					}
					num3++;
				}
			}
			catch (Exception)
			{
				return list;
			}
			return list;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x00320C78 File Offset: 0x0031EE78
		private Color GetColor(double value, double minValue, double maxValue)
		{
			Color[] array = this.colorSets20V2;
			int stepPercentFromValue = this.GetStepPercentFromValue(value, minValue, maxValue);
			int num = stepPercentFromValue / 5;
			int num2 = num + 1;
			if (num >= array.Length - 2)
			{
				num = array.Length - 2;
				num2 = array.Length - 1;
			}
			int stepPercentFromValue2 = this.GetStepPercentFromValue((double)(stepPercentFromValue % array.Length), 0.0, (double)array.Length);
			return this.GetGradientColorForStep(stepPercentFromValue2, array[num], array[num2]);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x00320CE4 File Offset: 0x0031EEE4
		private int GetStepPercentFromValue(double value, double minValue, double maxValue)
		{
			if (maxValue < minValue)
			{
				double num = maxValue;
				maxValue = minValue;
				minValue = num;
			}
			if (value < minValue)
			{
				return 0;
			}
			if (value > maxValue)
			{
				return 100;
			}
			double num2 = maxValue - minValue;
			double num3 = value - minValue;
			return (int)(100.0 * num3 / num2);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x00320D20 File Offset: 0x0031EF20
		private Color GetGradientColorForStep(int stepPercent, Color colorStart, Color colorFinish)
		{
			int num = (int)(colorStart.R * 255.0);
			int num2 = (int)(colorFinish.R * 255.0);
			int num3 = (int)(colorStart.G * 255.0);
			int num4 = (int)(colorFinish.G * 255.0);
			int num5 = (int)(colorStart.B * 255.0);
			int num6 = (int)(colorFinish.B * 255.0);
			int num7 = num + (num2 - num) * stepPercent / 100;
			int num8 = num3 + (num4 - num3) * stepPercent / 100;
			int num9 = num5 + (num6 - num5) * stepPercent / 100;
			return Color.FromRgb(num7, num8, num9);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x00320DCF File Offset: 0x0031EFCF
		private void expandButton_Clicked(object sender, EventArgs e)
		{
			this.gridProgressSlider.IsVisible = true;
			this.accentSelectorGrid.IsVisible = true;
			this.collapseButton.IsVisible = true;
			this.expandButton.IsVisible = false;
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00320E01 File Offset: 0x0031F001
		private void collapseButton_Clicked(object sender, EventArgs e)
		{
			this.gridProgressSlider.IsVisible = false;
			this.accentSelectorGrid.IsVisible = false;
			this.collapseButton.IsVisible = false;
			this.expandButton.IsVisible = true;
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00320E34 File Offset: 0x0031F034
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MapWithMultiChartV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/MapWithMultiChartV2.xaml",
				Instance = this
			}))
			{
				this.__InitComponentRuntime();
				return;
			}
			if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
			{
				this.__InitComponentRuntime();
				return;
			}
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			Color red = Color.Red;
			Color green = Color.Green;
			Color blue = Color.Blue;
			Color orange = Color.Orange;
			Color pink = Color.Pink;
			Color color = new Color(0.6000000238418579, 0.06666667014360428, 0.0941176488995552, 1.0);
			Color color2 = new Color(0.6000000238418579, 0.3294117748737335, 0.47058823704719543, 1.0);
			Color color3 = new Color(0.5882353186607361, 0.6000000238418579, 0.3294117748737335, 1.0);
			Color color4 = new Color(0.6000000238418579, 0.3607843220233917, 0.3294117748737335, 1.0);
			Color color5 = new Color(0.5803921818733215, 0.6000000238418579, 0.529411792755127, 1.0);
			Color color6 = new Color(0.08627451211214066, 0.6117647290229797, 0.800000011920929, 1.0);
			Color color7 = new Color(0.6823529601097107, 0.800000011920929, 0.08627451211214066, 1.0);
			Color color8 = new Color(0.08627451211214066, 0.800000011920929, 0.4901960790157318, 1.0);
			Color color9 = new Color(0.6470588445663452, 0.08627451211214066, 0.800000011920929, 1.0);
			Color color10 = new Color(0.08627451211214066, 0.7764706015586853, 0.800000011920929, 1.0);
			Color color11 = new Color(0.08627451211214066, 0.3607843220233917, 0.800000011920929, 1.0);
			Color color12 = new Color(0.800000011920929, 0.46666666865348816, 0.1764705926179886, 1.0);
			Color color13 = new Color(0.43921568989753723, 0.5372549295425415, 0.800000011920929, 1.0);
			Color color14 = new Color(0.800000011920929, 0.6549019813537598, 0.529411792755127, 1.0);
			Color color15 = new Color(0.7098039388656616, 0.529411792755127, 0.800000011920929, 1.0);
			Color color16 = new Color(1.0, 0.3176470696926117, 0.10980392247438431, 1.0);
			Color color17 = new Color(1.0, 0.10980392247438431, 0.3607843220233917, 1.0);
			Color color18 = new Color(0.10980392247438431, 0.24313725531101227, 1.0, 1.0);
			Color color19 = new Color(1.0, 0.21960784494876862, 0.7137255072593689, 1.0);
			Color color20 = new Color(0.41960784792900085, 1.0, 0.3294117748737335, 1.0);
			Color color21 = new Color(1.0, 0.8313725590705872, 0.43921568989753723, 1.0);
			Color color22 = new Color(1.0, 0.6549019813537598, 0.5490196347236633, 1.0);
			Color color23 = new Color(1.0, 0.5490196347236633, 0.6784313917160034, 1.0);
			Color color24 = new Color(1.0, 0.7686274647712708, 0.7803921699523926, 1.0);
			Color color25 = new Color(0.8274509906768799, 1.0, 0.7686274647712708, 1.0);
			Color color26 = new Color(0.9921568632125854, 1.0, 0.8784313797950745, 1.0);
			Color color27 = new Color(0.9921568632125854, 1.0, 0.9882352948188782, 1.0);
			Color darkOrange = Color.DarkOrange;
			Color darkGreen = Color.DarkGreen;
			Color darkRed = Color.DarkRed;
			ChartColorCollection chartColorCollection;
			VisualDiagnostics.RegisterSourceInfo(chartColorCollection = new ChartColorCollection(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			Map map;
			VisualDiagnostics.RegisterSourceInfo(map = new Map(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 25);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 22);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 25);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 22);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 25);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 25);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 22);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 22);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 25);
			LinkButton linkButton5;
			VisualDiagnostics.RegisterSourceInfo(linkButton5 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 22);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 25);
			LinkButton linkButton6;
			VisualDiagnostics.RegisterSourceInfo(linkButton6 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 22);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 25);
			LinkButton linkButton7;
			VisualDiagnostics.RegisterSourceInfo(linkButton7 = new LinkButton(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 18);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 22);
			SfRangeSlider sfRangeSlider;
			VisualDiagnostics.RegisterSourceInfo(sfRangeSlider = new SfRangeSlider(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 22);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 25);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 22);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 25);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 22);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 14);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 300, 17);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 44);
			ChartColorModel chartColorModel;
			VisualDiagnostics.RegisterSourceInfo(chartColorModel = new ChartColorModel(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 25);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 30);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 51);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 22);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 331, 25);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 30);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 51);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 356, 22);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 22);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 377, 30);
			ChartLegend chartLegend;
			VisualDiagnostics.RegisterSourceInfo(chartLegend = new ChartLegend(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 370, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 14);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 403, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 418, 14);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("layoutRoot", grid4);
			if (grid4.StyleId == null)
			{
				grid4.StyleId = "layoutRoot";
			}
			nameScope.RegisterName("map", map);
			if (map.StyleId == null)
			{
				map.StyleId = "map";
			}
			nameScope.RegisterName("controlsPanel", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "controlsPanel";
			}
			nameScope.RegisterName("buttonsGrid", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "buttonsGrid";
			}
			nameScope.RegisterName("btnPlay", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnPlay";
			}
			nameScope.RegisterName("btnPause", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnPause";
			}
			nameScope.RegisterName("btnStepPrev", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "btnStepPrev";
			}
			nameScope.RegisterName("btnStepNext", linkButton4);
			if (linkButton4.StyleId == null)
			{
				linkButton4.StyleId = "btnStepNext";
			}
			nameScope.RegisterName("lbSpeed", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbSpeed";
			}
			nameScope.RegisterName("collapseButton", linkButton5);
			if (linkButton5.StyleId == null)
			{
				linkButton5.StyleId = "collapseButton";
			}
			nameScope.RegisterName("expandButton", linkButton6);
			if (linkButton6.StyleId == null)
			{
				linkButton6.StyleId = "expandButton";
			}
			nameScope.RegisterName("zoomModeButton", linkButton7);
			if (linkButton7.StyleId == null)
			{
				linkButton7.StyleId = "zoomModeButton";
			}
			nameScope.RegisterName("gridProgressSlider", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridProgressSlider";
			}
			nameScope.RegisterName("lbStartTime", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "lbStartTime";
			}
			nameScope.RegisterName("slider", sfRangeSlider);
			if (sfRangeSlider.StyleId == null)
			{
				sfRangeSlider.StyleId = "slider";
			}
			nameScope.RegisterName("lbFinishTime", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "lbFinishTime";
			}
			nameScope.RegisterName("accentSelectorGrid", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "accentSelectorGrid";
			}
			nameScope.RegisterName("accentPicker", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "accentPicker";
			}
			nameScope.RegisterName("lbCurrentPoint", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "lbCurrentPoint";
			}
			nameScope.RegisterName("chart", sfChart);
			if (sfChart.StyleId == null)
			{
				sfChart.StyleId = "chart";
			}
			nameScope.RegisterName("xaxis", numericalAxis);
			if (numericalAxis.StyleId == null)
			{
				numericalAxis.StyleId = "xaxis";
			}
			nameScope.RegisterName("yaxis", numericalAxis2);
			if (numericalAxis2.StyleId == null)
			{
				numericalAxis2.StyleId = "yaxis";
			}
			nameScope.RegisterName("zoomBehave", chartZoomPanBehavior);
			if (chartZoomPanBehavior.StyleId == null)
			{
				chartZoomPanBehavior.StyleId = "zoomBehave";
			}
			nameScope.RegisterName("Legend", chartLegend);
			if (chartLegend.StyleId == null)
			{
				chartLegend.StyleId = "Legend";
			}
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.page = this;
			this.layoutRoot = grid4;
			this.map = map;
			this.controlsPanel = stackLayout;
			this.buttonsGrid = grid;
			this.btnPlay = linkButton;
			this.btnPause = linkButton2;
			this.btnStepPrev = linkButton3;
			this.btnStepNext = linkButton4;
			this.lbSpeed = label;
			this.collapseButton = linkButton5;
			this.expandButton = linkButton6;
			this.zoomModeButton = linkButton7;
			this.gridProgressSlider = grid2;
			this.lbStartTime = label2;
			this.slider = sfRangeSlider;
			this.lbFinishTime = label3;
			this.accentSelectorGrid = grid3;
			this.accentPicker = picker;
			this.lbCurrentPoint = label5;
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.yaxis = numericalAxis2;
			this.zoomBehave = chartZoomPanBehavior;
			this.Legend = chartLegend;
			this.lv = sfListView;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			chartColorCollection.Add(red);
			chartColorCollection.Add(green);
			chartColorCollection.Add(blue);
			chartColorCollection.Add(orange);
			chartColorCollection.Add(pink);
			chartColorCollection.Add(color);
			chartColorCollection.Add(color2);
			chartColorCollection.Add(color3);
			chartColorCollection.Add(color4);
			chartColorCollection.Add(color5);
			chartColorCollection.Add(color6);
			chartColorCollection.Add(color7);
			chartColorCollection.Add(color8);
			chartColorCollection.Add(color9);
			chartColorCollection.Add(color10);
			chartColorCollection.Add(color11);
			chartColorCollection.Add(color12);
			chartColorCollection.Add(color13);
			chartColorCollection.Add(color14);
			chartColorCollection.Add(color15);
			chartColorCollection.Add(color16);
			chartColorCollection.Add(color17);
			chartColorCollection.Add(color18);
			chartColorCollection.Add(color19);
			chartColorCollection.Add(color20);
			chartColorCollection.Add(color21);
			chartColorCollection.Add(color22);
			chartColorCollection.Add(color23);
			chartColorCollection.Add(color24);
			chartColorCollection.Add(color25);
			chartColorCollection.Add(color26);
			chartColorCollection.Add(color27);
			chartColorCollection.Add(darkOrange);
			chartColorCollection.Add(darkGreen);
			chartColorCollection.Add(darkRed);
			resourceDictionary.Add("Colors", chartColorCollection);
			IDataTemplate dataTemplate4 = dataTemplate;
			MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_22 <InitializeComponent>_anonXamlCDataTemplate_ = new MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_22();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("mobileTemplate", dataTemplate);
			IDataTemplate dataTemplate5 = dataTemplate2;
			MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_23 <InitializeComponent>_anonXamlCDataTemplate_2 = new MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_23();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			resourceDictionary.Add("tabletTemplate", dataTemplate2);
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.MapWithMultiChartV2_SizeChanged;
			this.Resources = resourceDictionary;
			grid4.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("0.4*, Auto,  0.6*"));
			map.SetValue(Grid.RowProperty, 0);
			map.SetValue(Map.HasScrollEnabledProperty, true);
			map.SetValue(Map.HasZoomEnabledProperty, true);
			map.SetValue(Map.IsShowingUserProperty, false);
			map.MapClicked += this.Map_MapClicked;
			map.SetValue(Map.MapTypeProperty, 0);
			grid4.Children.Add(map);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			grid.SetValue(Grid.RowProperty, 1);
			grid.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("auto,auto,auto,auto,*, auto, auto"));
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = stackLayout;
			array4[3] = grid4;
			array4[4] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver2.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 25)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			linkButton.Clicked += this.btnPlay_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			linkButton.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("play.png"));
			linkButton.SetValue(Button.TextProperty, " ");
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			linkButton2.SetValue(Grid.ColumnProperty, 0);
			linkButton2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = linkButton2;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = grid4;
			array5[4] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array5, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver3.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 25)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			linkButton2.Clicked += this.btnPause_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			linkButton2.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("pause.png"));
			linkButton2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			linkButton2.SetValue(Button.TextProperty, " ");
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton2);
			linkButton3.SetValue(Grid.ColumnProperty, 1);
			linkButton3.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension4.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = linkButton3;
			array6[1] = grid;
			array6[2] = stackLayout;
			array6[3] = grid4;
			array6[4] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver4.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 25)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource4.Key);
			linkButton3.Clicked += this.btnStepPrev_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			linkButton3.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("rewind.png"));
			linkButton3.SetValue(Button.TextProperty, " ");
			linkButton3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton3);
			linkButton4.SetValue(Grid.ColumnProperty, 2);
			linkButton4.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension5.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = linkButton4;
			array7[1] = grid;
			array7[2] = stackLayout;
			array7[3] = grid4;
			array7[4] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array7, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver5.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 25)));
			DynamicResource dynamicResource5 = markupExtension5.ProvideValue(xamlServiceProvider5);
			linkButton4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource5.Key);
			linkButton4.Clicked += this.btnStepNext_Clicked;
			linkButton4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			linkButton4.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("fast_forward.png"));
			linkButton4.SetValue(Button.TextProperty, " ");
			linkButton4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton4);
			label.SetValue(Grid.ColumnProperty, 3);
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension6 = referenceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = label;
			array8[1] = grid;
			array8[2] = stackLayout;
			array8[3] = grid4;
			array8[4] = this;
			object obj6;
			xamlServiceProvider6.Add(typeFromHandle11, obj6 = new SimpleValueTargetProvider(array8, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver6.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 25)));
			object obj7 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetValue(BindableObject.BindingContextProperty, obj7);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = label;
			array9[1] = grid;
			array9[2] = stackLayout;
			array9[3] = grid4;
			array9[4] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver7.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 25)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label.FontSize = (double)obj9;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "CurrentSpeedTitle";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			linkButton5.SetValue(Grid.ColumnProperty, 4);
			linkButton5.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension6.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = linkButton5;
			array10[1] = grid;
			array10[2] = stackLayout;
			array10[3] = grid4;
			array10[4] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array10, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver8.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(210, 25)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			linkButton5.Clicked += this.collapseButton_Clicked;
			linkButton5.SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			linkButton5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton5.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("collapse.png"));
			linkButton5.SetValue(Button.TextProperty, " ");
			linkButton5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton5);
			linkButton6.SetValue(Grid.ColumnProperty, 5);
			linkButton6.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension7.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = linkButton6;
			array11[1] = grid;
			array11[2] = stackLayout;
			array11[3] = grid4;
			array11[4] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array11, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver9.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 25)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			linkButton6.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource7.Key);
			linkButton6.Clicked += this.expandButton_Clicked;
			linkButton6.SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			linkButton6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton6.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("expand.png"));
			linkButton6.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			linkButton6.SetValue(Button.TextProperty, " ");
			linkButton6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton6);
			linkButton7.SetValue(Grid.ColumnProperty, 6);
			linkButton7.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension8.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = linkButton7;
			array12[1] = grid;
			array12[2] = stackLayout;
			array12[3] = grid4;
			array12[4] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array12, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver10.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(236, 25)));
			DynamicResource dynamicResource8 = markupExtension10.ProvideValue(xamlServiceProvider10);
			linkButton7.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			linkButton7.Clicked += this.zoomModeButton_Clicked;
			linkButton7.SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			linkButton7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton7.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("resize_x.png"));
			linkButton7.SetValue(Button.TextProperty, " ");
			linkButton7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton7);
			stackLayout.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 2);
			grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("auto, *, auto"));
			label2.SetValue(Grid.ColumnProperty, 0);
			grid2.Children.Add(label2);
			sfRangeSlider.SetValue(Grid.ColumnProperty, 1);
			sfRangeSlider.SetValue(View.MarginProperty, new Thickness(1.0, 0.0));
			sfRangeSlider.SetValue(SfRangeSlider.AllowDragRangeProperty, false);
			sfRangeSlider.DragCompleted += new SfRangeSlider.DragThumbEventHandler(this.Slider_DragCompleted);
			sfRangeSlider.SetValue(VisualElement.HeightRequestProperty, 18.0);
			sfRangeSlider.SetValue(SfRangeSlider.MaximumProperty, 100000.0);
			sfRangeSlider.SetValue(SfRangeSlider.MinimumProperty, 0.0);
			sfRangeSlider.SetValue(SfRangeSlider.OrientationProperty, 0);
			sfRangeSlider.SetValue(SfRangeSlider.ShowRangeProperty, false);
			sfRangeSlider.SetValue(SfRangeSlider.ShowValueLabelProperty, false);
			sfRangeSlider.SetValue(SfRangeSlider.SnapsToProperty, 2);
			sfRangeSlider.SetValue(SfRangeSlider.TickFrequencyProperty, 0.0);
			sfRangeSlider.SetValue(SfRangeSlider.ToolTipPlacementProperty, 2);
			sfRangeSlider.SetValue(SfRangeSlider.ValueChangeModeProperty, 1);
			sfRangeSlider.SetValue(SfRangeSlider.ValuePlacementProperty, 0);
			grid2.Children.Add(sfRangeSlider);
			label3.SetValue(Grid.ColumnProperty, 2);
			grid2.Children.Add(label3);
			stackLayout.Children.Add(grid2);
			grid3.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto, *, Auto"));
			label4.SetValue(Grid.ColumnProperty, 0);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			translate.Text = "records_Highlight";
			IMarkupExtension markupExtension11 = translate;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = label4;
			array13[1] = grid3;
			array13[2] = stackLayout;
			array13[3] = grid4;
			array13[4] = this;
			object obj13;
			xamlServiceProvider11.Add(typeFromHandle21, obj13 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver11.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(274, 25)));
			object obj14 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label4.Text = obj14;
			label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(label4);
			picker.SetValue(Grid.ColumnProperty, 1);
			bindingExtension3.Path = "Name";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.ItemDisplayBinding = bindingBase3;
			picker.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(picker);
			label5.SetValue(Grid.ColumnProperty, 2);
			label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(label5);
			stackLayout.Children.Add(grid3);
			grid4.Children.Add(stackLayout);
			sfChart.SetValue(Grid.RowProperty, 2);
			sfChart.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension9.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = sfChart;
			array14[1] = grid4;
			array14[2] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array14, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver12.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(300, 17)));
			DynamicResource dynamicResource9 = markupExtension12.ProvideValue(xamlServiceProvider12);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource9.Key);
			dynamicResourceExtension10.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = sfChart;
			array15[1] = grid4;
			array15[2] = this;
			object obj16;
			xamlServiceProvider13.Add(typeFromHandle25, obj16 = new SimpleValueTargetProvider(array15, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver13.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(301, 17)));
			DynamicResource dynamicResource10 = markupExtension13.ProvideValue(xamlServiceProvider13);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			staticResourceExtension2.Key = "Colors";
			IMarkupExtension markupExtension14 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = chartColorModel;
			array16[1] = sfChart;
			array16[2] = grid4;
			array16[3] = this;
			object obj17;
			xamlServiceProvider14.Add(typeFromHandle27, obj17 = new SimpleValueTargetProvider(array16, ChartColorModel.CustomBrushesProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver14.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(305, 44)));
			object obj18 = markupExtension14.ProvideValue(xamlServiceProvider14);
			chartColorModel.CustomBrushes = obj18;
			chartColorModel.SetValue(ChartColorModel.PaletteProperty, 5);
			sfChart.SetValue(SfChart.ColorModelProperty, chartColorModel);
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, true);
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			staticResourceExtension3.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension15 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = numericalAxis;
			array17[1] = sfChart;
			array17[2] = grid4;
			array17[3] = this;
			object obj19;
			xamlServiceProvider15.Add(typeFromHandle29, obj19 = new SimpleValueTargetProvider(array17, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver15.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(313, 25)));
			object obj20 = markupExtension15.ProvideValue(xamlServiceProvider15);
			numericalAxis.MajorGridLineStyle = obj20;
			staticResourceExtension4.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension16 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = numericalAxis;
			array18[1] = sfChart;
			array18[2] = grid4;
			array18[3] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle31, obj21 = new SimpleValueTargetProvider(array18, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver16.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 25)));
			object obj22 = markupExtension16.ProvideValue(xamlServiceProvider16);
			numericalAxis.MajorTickStyle = obj22;
			dynamicResourceExtension11.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = chartAxisLabelStyle;
			array19[1] = numericalAxis;
			array19[2] = sfChart;
			array19[3] = grid4;
			array19[4] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array19, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver17.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 56)));
			DynamicResource dynamicResource11 = markupExtension17.ProvideValue(xamlServiceProvider17);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource11.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension12.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = chartLineStyle;
			array20[1] = numericalAxis;
			array20[2] = sfChart;
			array20[3] = grid4;
			array20[4] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle35, obj24 = new SimpleValueTargetProvider(array20, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver18.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(319, 51)));
			DynamicResource dynamicResource12 = markupExtension18.ProvideValue(xamlServiceProvider18);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource12.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 0);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 0);
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension5.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension19 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = numericalAxis2;
			array21[1] = sfChart;
			array21[2] = grid4;
			array21[3] = this;
			object obj25;
			xamlServiceProvider19.Add(typeFromHandle37, obj25 = new SimpleValueTargetProvider(array21, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver19.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(330, 25)));
			object obj26 = markupExtension19.ProvideValue(xamlServiceProvider19);
			numericalAxis2.MajorGridLineStyle = obj26;
			staticResourceExtension6.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension20 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = numericalAxis2;
			array22[1] = sfChart;
			array22[2] = grid4;
			array22[3] = this;
			object obj27;
			xamlServiceProvider20.Add(typeFromHandle39, obj27 = new SimpleValueTargetProvider(array22, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver20.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(331, 25)));
			object obj28 = markupExtension20.ProvideValue(xamlServiceProvider20);
			numericalAxis2.MajorTickStyle = obj28;
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			dynamicResourceExtension13.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = chartAxisLabelStyle2;
			array23[1] = numericalAxis2;
			array23[2] = sfChart;
			array23[3] = grid4;
			array23[4] = this;
			object obj29;
			xamlServiceProvider21.Add(typeFromHandle41, obj29 = new SimpleValueTargetProvider(array23, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver21.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(334, 56)));
			DynamicResource dynamicResource13 = markupExtension21.ProvideValue(xamlServiceProvider21);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource13.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension14.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = chartLineStyle2;
			array24[1] = numericalAxis2;
			array24[2] = sfChart;
			array24[3] = grid4;
			array24[4] = this;
			object obj30;
			xamlServiceProvider22.Add(typeFromHandle43, obj30 = new SimpleValueTargetProvider(array24, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
			xmlNamespaceResolver22.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(MapWithMultiChartV2).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(337, 51)));
			DynamicResource dynamicResource14 = markupExtension22.ProvideValue(xamlServiceProvider22);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource14.Key);
			numericalAxis2.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableDoubleTapProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnablePanningProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableZoomingProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.ZoomModeProperty, 0);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartZoomPanBehavior);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.LabelDisplayModeProperty, 1);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLabelProperty, true);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLineProperty, true);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartTrackballBehavior);
			chartLegend.SetValue(ChartLegend.DockPositionProperty, 3);
			chartLegend.SetValue(ChartLegend.IsVisibleProperty, true);
			chartLegend.SetValue(ChartLegend.OrientationProperty, 2);
			chartLegend.SetValue(ChartLegend.ToggleSeriesVisibilityProperty, true);
			IDataTemplate dataTemplate6 = dataTemplate3;
			MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_24 <InitializeComponent>_anonXamlCDataTemplate_3 = new MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_24();
			object[] array25 = new object[0 + 5];
			array25[0] = dataTemplate3;
			array25[1] = chartLegend;
			array25[2] = sfChart;
			array25[3] = grid4;
			array25[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array25;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			chartLegend.SetValue(ChartLegend.ItemTemplateProperty, dataTemplate3);
			sfChart.SetValue(SfChart.LegendProperty, chartLegend);
			grid4.Children.Add(sfChart);
			sfListView.SetValue(Grid.RowProperty, 2);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			sfListView.SetValue(SfListView.SelectionModeProperty, 3);
			grid4.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(Grid.RowSpanProperty, 3);
			activityFrame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid4);
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x003251EE File Offset: 0x003233EE
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.chart.IsVisible = !this.chart.IsVisible;
			this.lv.IsVisible = !this.chart.IsVisible;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x00325222 File Offset: 0x00323422
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.Legend.IsVisible = !this.Legend.IsVisible;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0032523D File Offset: 0x0032343D
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x00325248 File Offset: 0x00323448
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MapWithMultiChartV2>(this, typeof(MapWithMultiChartV2));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.layoutRoot = NameScopeExtensions.FindByName<Grid>(this, "layoutRoot");
			this.map = NameScopeExtensions.FindByName<Map>(this, "map");
			this.controlsPanel = NameScopeExtensions.FindByName<StackLayout>(this, "controlsPanel");
			this.buttonsGrid = NameScopeExtensions.FindByName<Grid>(this, "buttonsGrid");
			this.btnPlay = NameScopeExtensions.FindByName<LinkButton>(this, "btnPlay");
			this.btnPause = NameScopeExtensions.FindByName<LinkButton>(this, "btnPause");
			this.btnStepPrev = NameScopeExtensions.FindByName<LinkButton>(this, "btnStepPrev");
			this.btnStepNext = NameScopeExtensions.FindByName<LinkButton>(this, "btnStepNext");
			this.lbSpeed = NameScopeExtensions.FindByName<Label>(this, "lbSpeed");
			this.collapseButton = NameScopeExtensions.FindByName<LinkButton>(this, "collapseButton");
			this.expandButton = NameScopeExtensions.FindByName<LinkButton>(this, "expandButton");
			this.zoomModeButton = NameScopeExtensions.FindByName<LinkButton>(this, "zoomModeButton");
			this.gridProgressSlider = NameScopeExtensions.FindByName<Grid>(this, "gridProgressSlider");
			this.lbStartTime = NameScopeExtensions.FindByName<Label>(this, "lbStartTime");
			this.slider = NameScopeExtensions.FindByName<SfRangeSlider>(this, "slider");
			this.lbFinishTime = NameScopeExtensions.FindByName<Label>(this, "lbFinishTime");
			this.accentSelectorGrid = NameScopeExtensions.FindByName<Grid>(this, "accentSelectorGrid");
			this.accentPicker = NameScopeExtensions.FindByName<Picker>(this, "accentPicker");
			this.lbCurrentPoint = NameScopeExtensions.FindByName<Label>(this, "lbCurrentPoint");
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.yaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "yaxis");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
			this.Legend = NameScopeExtensions.FindByName<ChartLegend>(this, "Legend");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002530 RID: 9520
		private Pin pin;

		// Token: 0x04002531 RID: 9521
		private StringBuilder labelCurrentPointSb = new StringBuilder(6);

		// Token: 0x04002532 RID: 9522
		private PointWithTime _CurrentPoint = PointWithTime.Zero;

		// Token: 0x04002533 RID: 9523
		private List<PointWithTime> allPositions;

		// Token: 0x04002534 RID: 9524
		private List<PointWithTime> filteredPositions;

		// Token: 0x04002535 RID: 9525
		private PointWithTime PointStart;

		// Token: 0x04002536 RID: 9526
		private PointWithTime PointFinish;

		// Token: 0x04002537 RID: 9527
		private List<DataRecordElementCollectionWithContainerReference> recordsItemsWithElement;

		// Token: 0x04002538 RID: 9528
		[CompilerGenerated]
		private double[] <SpeedArray>k__BackingField;

		// Token: 0x04002539 RID: 9529
		private double _CurrentSpeed = 1.0;

		// Token: 0x0400253A RID: 9530
		private bool _IsPlaying;

		// Token: 0x0400253B RID: 9531
		private List<Polyline> accentLinesList;

		// Token: 0x0400253C RID: 9532
		private Color[] colorSets20 = new Color[]
		{
			Color.FromHex("3713BB"),
			Color.FromHex("212CD3"),
			Color.FromHex("0744EA"),
			Color.FromHex("0064F0"),
			Color.FromHex("008FED"),
			Color.FromHex("00B8EA"),
			Color.FromHex("02CFD4"),
			Color.FromHex("05E1B5"),
			Color.FromHex("06F693"),
			Color.FromHex("14F863"),
			Color.FromHex("26FB2F"),
			Color.FromHex("40F603"),
			Color.FromHex("9EED06"),
			Color.FromHex("D3E805"),
			Color.FromHex("FAD000"),
			Color.FromHex("FBAA01"),
			Color.FromHex("FA8301"),
			Color.FromHex("FA5B00"),
			Color.FromHex("FC3401"),
			Color.FromHex("FB0D01")
		};

		// Token: 0x0400253D RID: 9533
		private Color[] colorSets10 = new Color[]
		{
			Color.FromHex("26FB2F"),
			Color.FromHex("40F603"),
			Color.FromHex("9EED06"),
			Color.FromHex("D3E805"),
			Color.FromHex("FAD000"),
			Color.FromHex("FBAA01"),
			Color.FromHex("FA8301"),
			Color.FromHex("FA5B00"),
			Color.FromHex("FC3401"),
			Color.FromHex("FB0D01")
		};

		// Token: 0x0400253E RID: 9534
		private Color[] colorSets20V2 = new Color[]
		{
			Color.FromHex("00A856"),
			Color.FromHex("15ae49"),
			Color.FromHex("3cba32"),
			Color.FromHex("56C222"),
			Color.FromHex("95d416"),
			Color.FromHex("cae40c"),
			Color.FromHex("FFF301"),
			Color.FromHex("ffda01"),
			Color.FromHex("ffc101"),
			Color.FromHex("FEA200"),
			Color.FromHex("fe9900"),
			Color.FromHex("fe8f00"),
			Color.FromHex("ff8600"),
			Color.FromHex("FF7C00"),
			Color.FromHex("ff660c"),
			Color.FromHex("ff4b1b"),
			Color.FromHex("FF2531"),
			Color.FromHex("ff1a26"),
			Color.FromHex("ff0f1a"),
			Color.FromHex("FF010B")
		};

		// Token: 0x0400253F RID: 9535
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002540 RID: 9536
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutRoot;

		// Token: 0x04002541 RID: 9537
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Map map;

		// Token: 0x04002542 RID: 9538
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout controlsPanel;

		// Token: 0x04002543 RID: 9539
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid buttonsGrid;

		// Token: 0x04002544 RID: 9540
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPlay;

		// Token: 0x04002545 RID: 9541
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPause;

		// Token: 0x04002546 RID: 9542
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnStepPrev;

		// Token: 0x04002547 RID: 9543
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnStepNext;

		// Token: 0x04002548 RID: 9544
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbSpeed;

		// Token: 0x04002549 RID: 9545
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton collapseButton;

		// Token: 0x0400254A RID: 9546
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton expandButton;

		// Token: 0x0400254B RID: 9547
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton zoomModeButton;

		// Token: 0x0400254C RID: 9548
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridProgressSlider;

		// Token: 0x0400254D RID: 9549
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbStartTime;

		// Token: 0x0400254E RID: 9550
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRangeSlider slider;

		// Token: 0x0400254F RID: 9551
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbFinishTime;

		// Token: 0x04002550 RID: 9552
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid accentSelectorGrid;

		// Token: 0x04002551 RID: 9553
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker accentPicker;

		// Token: 0x04002552 RID: 9554
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbCurrentPoint;

		// Token: 0x04002553 RID: 9555
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x04002554 RID: 9556
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x04002555 RID: 9557
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis yaxis;

		// Token: 0x04002556 RID: 9558
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x04002557 RID: 9559
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartLegend Legend;

		// Token: 0x04002558 RID: 9560
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x04002559 RID: 9561
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x020006FA RID: 1786
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003CF9 RID: 15609 RVA: 0x00325431 File Offset: 0x00323631
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003CFA RID: 15610 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003CFB RID: 15611 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <.ctor>b__0_3(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003CFC RID: 15612 RVA: 0x0031E049 File Offset: 0x0031C249
			internal DataRecordElementCollectionWithContainerReference <.ctor>b__0_4(DataRecord x)
			{
				return new DataRecordElementCollectionWithContainerReference(x);
			}

			// Token: 0x06003CFD RID: 15613 RVA: 0x0031E051 File Offset: 0x0031C251
			internal UnitsHelper.Units <.ctor>b__0_5(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units;
			}

			// Token: 0x06003CFE RID: 15614 RVA: 0x0031E05E File Offset: 0x0031C25E
			internal bool <FilterPositionsV1>b__35_0(DataRecordElement x)
			{
				return !double.IsNaN(x.Value);
			}

			// Token: 0x06003CFF RID: 15615 RVA: 0x0031E05E File Offset: 0x0031C25E
			internal bool <FilterPositionsV2>b__36_1(DataRecordElement x)
			{
				return !double.IsNaN(x.Value);
			}

			// Token: 0x06003D00 RID: 15616 RVA: 0x0032543D File Offset: 0x0032363D
			internal PointWithTime <FilterPositionsV2>b__36_2(DataRecordElement x)
			{
				return new PointWithTime(x.Position, x.Seconds);
			}

			// Token: 0x06003D01 RID: 15617 RVA: 0x00325450 File Offset: 0x00323650
			internal double <FilterPositionsV2>b__36_0(PointWithTime x)
			{
				return x.TimeSeconds;
			}

			// Token: 0x06003D02 RID: 15618 RVA: 0x0031E051 File Offset: 0x0031C251
			internal UnitsHelper.Units <BuildInterfaceFor2Units>b__41_0(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units;
			}

			// Token: 0x06003D03 RID: 15619 RVA: 0x00315BA4 File Offset: 0x00313DA4
			internal bool <DrawLineAsPolyLines>b__59_0(DataRecordElement x)
			{
				return double.IsFinite(x.Value);
			}

			// Token: 0x06003D04 RID: 15620 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <DrawLineAsPolyLines>b__59_1(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x06003D05 RID: 15621 RVA: 0x00315BA4 File Offset: 0x00313DA4
			internal bool <DrawLineAsPolyLines>b__59_2(DataRecordElement x)
			{
				return double.IsFinite(x.Value);
			}

			// Token: 0x06003D06 RID: 15622 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <DrawLineAsPolyLines>b__59_3(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x0400255A RID: 9562
			public static readonly MapWithMultiChartV2.<>c <>9 = new MapWithMultiChartV2.<>c();

			// Token: 0x0400255B RID: 9563
			public static Func<DataRecord, bool> <>9__0_3;

			// Token: 0x0400255C RID: 9564
			public static Func<DataRecord, DataRecordElementCollectionWithContainerReference> <>9__0_4;

			// Token: 0x0400255D RID: 9565
			public static Func<DataRecordElementCollectionWithContainerReference, UnitsHelper.Units> <>9__0_5;

			// Token: 0x0400255E RID: 9566
			public static Func<DataRecordElement, bool> <>9__35_0;

			// Token: 0x0400255F RID: 9567
			public static Func<DataRecordElement, bool> <>9__36_1;

			// Token: 0x04002560 RID: 9568
			public static Func<DataRecordElement, PointWithTime> <>9__36_2;

			// Token: 0x04002561 RID: 9569
			public static Func<PointWithTime, double> <>9__36_0;

			// Token: 0x04002562 RID: 9570
			public static Func<DataRecordElementCollectionWithContainerReference, UnitsHelper.Units> <>9__41_0;

			// Token: 0x04002563 RID: 9571
			public static Func<DataRecordElement, bool> <>9__59_0;

			// Token: 0x04002564 RID: 9572
			public static Func<DataRecordElement, double> <>9__59_1;

			// Token: 0x04002565 RID: 9573
			public static Func<DataRecordElement, bool> <>9__59_2;

			// Token: 0x04002566 RID: 9574
			public static Func<DataRecordElement, double> <>9__59_3;
		}

		// Token: 0x020006FB RID: 1787
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_0
		{
			// Token: 0x06003D07 RID: 15623 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_0()
			{
			}

			// Token: 0x06003D08 RID: 15624 RVA: 0x0032545C File Offset: 0x0032365C
			internal void <PlayLoop>b__0()
			{
				if (this.<>4__this.Navigation.NavigationStack[this.<>4__this.Navigation.NavigationStack.Count - 1] == this.<>4__this)
				{
					this.<>4__this.UpdateForPosition(this.nextpoint);
				}
			}

			// Token: 0x04002567 RID: 9575
			public PointWithTime nextpoint;

			// Token: 0x04002568 RID: 9576
			public MapWithMultiChartV2 <>4__this;
		}

		// Token: 0x020006FC RID: 1788
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_0
		{
			// Token: 0x06003D09 RID: 15625 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_0()
			{
			}

			// Token: 0x06003D0A RID: 15626 RVA: 0x003254AE File Offset: 0x003236AE
			internal bool <FilterPositionsV1>b__1(DataRecordElement x)
			{
				return this.pos.Equals(x.Position);
			}

			// Token: 0x04002569 RID: 9577
			public PointWithTime pos;

			// Token: 0x0400256A RID: 9578
			public Func<DataRecordElement, bool> <>9__1;
		}

		// Token: 0x020006FD RID: 1789
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x06003D0B RID: 15627 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x06003D0C RID: 15628 RVA: 0x003254CC File Offset: 0x003236CC
			internal double <GetClosestRecordedPositionToClickedPosition>b__0(PointWithTime x)
			{
				return x.GetDistanceToPoint(this.pos);
			}

			// Token: 0x0400256B RID: 9579
			public Point pos;
		}

		// Token: 0x020006FE RID: 1790
		[CompilerGenerated]
		private sealed class <>c__DisplayClass41_0
		{
			// Token: 0x06003D0D RID: 15629 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass41_0()
			{
			}

			// Token: 0x06003D0E RID: 15630 RVA: 0x003254DA File Offset: 0x003236DA
			internal bool <BuildInterfaceFor2Units>b__1(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units == this.leftUnit;
			}

			// Token: 0x06003D0F RID: 15631 RVA: 0x003254EF File Offset: 0x003236EF
			internal bool <BuildInterfaceFor2Units>b__2(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units == this.rightUnit;
			}

			// Token: 0x0400256C RID: 9580
			public UnitsHelper.Units leftUnit;

			// Token: 0x0400256D RID: 9581
			public UnitsHelper.Units rightUnit;
		}

		// Token: 0x020006FF RID: 1791
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AccentPicker_SelectedIndexChanged>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003D10 RID: 15632 RVA: 0x00325504 File Offset: 0x00323704
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MapWithMultiChartV2 mapWithMultiChartV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00FC;
						}
						idx = mapWithMultiChartV.accentPicker.SelectedIndex;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapWithMultiChartV2.<AccentPicker_SelectedIndexChanged>d__2>(ref taskAwaiter, ref this);
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
					if (mapWithMultiChartV.accentPicker.SelectedIndex != idx)
					{
						goto IL_0127;
					}
					mapWithMultiChartV.activityFrame.IsVisible = true;
					taskAwaiter = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapWithMultiChartV2.<AccentPicker_SelectedIndexChanged>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_00FC:
					taskAwaiter.GetResult();
					DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference = (DataRecordElementCollectionWithContainerReference)mapWithMultiChartV.accentPicker.SelectedItem;
					mapWithMultiChartV.DrawAccentLine(dataRecordElementCollectionWithContainerReference);
					mapWithMultiChartV.activityFrame.IsVisible = false;
					IL_0127:;
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

			// Token: 0x06003D11 RID: 15633 RVA: 0x00325684 File Offset: 0x00323884
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400256E RID: 9582
			public int <>1__state;

			// Token: 0x0400256F RID: 9583
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002570 RID: 9584
			public MapWithMultiChartV2 <>4__this;

			// Token: 0x04002571 RID: 9585
			private int <idx>5__2;

			// Token: 0x04002572 RID: 9586
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000700 RID: 1792
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <MapWithMultiChartV2_SizeChanged>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003D12 RID: 15634 RVA: 0x00325694 File Offset: 0x00323894
			void IAsyncStateMachine.MoveNext()
			{
				MapWithMultiChartV2 mapWithMultiChartV = this;
				try
				{
					switch (DeviceDisplay.MainDisplayInfo.Rotation)
					{
					case 0:
					case 1:
					case 3:
						mapWithMultiChartV.Legend.DockPosition = 3;
						mapWithMultiChartV.Legend.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
						mapWithMultiChartV.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width;
						break;
					case 2:
					case 4:
						mapWithMultiChartV.Legend.DockPosition = 4;
						mapWithMultiChartV.Legend.BackgroundColor = ((Color)Application.Current.Resources["BackgroundColor"]).MultiplyAlpha(0.5);
						mapWithMultiChartV.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width * 0.25;
						break;
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

			// Token: 0x06003D13 RID: 15635 RVA: 0x003257C0 File Offset: 0x003239C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002573 RID: 9587
			public int <>1__state;

			// Token: 0x04002574 RID: 9588
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002575 RID: 9589
			public MapWithMultiChartV2 <>4__this;
		}

		// Token: 0x02000701 RID: 1793
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PlayLoop>d__32 : IAsyncStateMachine
		{
			// Token: 0x06003D14 RID: 15636 RVA: 0x003257D0 File Offset: 0x003239D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MapWithMultiChartV2 mapWithMultiChartV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num != 1)
						{
							goto IL_0179;
						}
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_016B;
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					IL_00F3:
					taskAwaiter.GetResult();
					if (!mapWithMultiChartV.IsPlaying)
					{
						goto IL_019F;
					}
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						if (CS$<>8__locals1.<>4__this.Navigation.NavigationStack[CS$<>8__locals1.<>4__this.Navigation.NavigationStack.Count - 1] == CS$<>8__locals1.<>4__this)
						{
							CS$<>8__locals1.<>4__this.UpdateForPosition(CS$<>8__locals1.nextpoint);
						}
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapWithMultiChartV2.<PlayLoop>d__32>(ref taskAwaiter, ref this);
						return;
					}
					IL_016B:
					taskAwaiter.GetResult();
					CS$<>8__locals1 = null;
					IL_0179:
					if (mapWithMultiChartV.IsPlaying)
					{
						int num3 = mapWithMultiChartV.CurrentPoint.Index + 1;
						if (num3 != mapWithMultiChartV.filteredPositions.Count)
						{
							CS$<>8__locals1 = new MapWithMultiChartV2.<>c__DisplayClass32_0();
							CS$<>8__locals1.<>4__this = mapWithMultiChartV;
							CS$<>8__locals1.nextpoint = mapWithMultiChartV.filteredPositions[num3];
							taskAwaiter = Task.Delay(TimeSpan.FromSeconds((CS$<>8__locals1.nextpoint.TimeSeconds - mapWithMultiChartV.CurrentPoint.TimeSeconds) / mapWithMultiChartV.CurrentSpeed)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapWithMultiChartV2.<PlayLoop>d__32>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00F3;
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_019F:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003D15 RID: 15637 RVA: 0x003259AC File Offset: 0x00323BAC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002576 RID: 9590
			public int <>1__state;

			// Token: 0x04002577 RID: 9591
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002578 RID: 9592
			public MapWithMultiChartV2 <>4__this;

			// Token: 0x04002579 RID: 9593
			private MapWithMultiChartV2.<>c__DisplayClass32_0 <>8__1;

			// Token: 0x0400257A RID: 9594
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000702 RID: 1794
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_22
		{
			// Token: 0x06003D16 RID: 15638 RVA: 0x003259BC File Offset: 0x00323BBC
			public <InitializeComponent>_anonXamlCDataTemplate_22()
			{
			}

			// Token: 0x06003D17 RID: 15639 RVA: 0x003259D0 File Offset: 0x00323BD0
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 26);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				bindingExtension.Mode = 4;
				bindingExtension.Path = "DataRecord.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 4;
				bindingExtension3.Path = "AdaptedUnitsTitle";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				return grid;
			}

			// Token: 0x0400257B RID: 9595
			internal object[] parentValues;

			// Token: 0x0400257C RID: 9596
			internal MapWithMultiChartV2 root;
		}

		// Token: 0x02000703 RID: 1795
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_23
		{
			// Token: 0x06003D18 RID: 15640 RVA: 0x00325D80 File Offset: 0x00323F80
			public <InitializeComponent>_anonXamlCDataTemplate_23()
			{
			}

			// Token: 0x06003D19 RID: 15641 RVA: 0x00325D94 File Offset: 0x00323F94
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 26);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 26);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 25);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 25);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
				xmlNamespaceResolver.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_23).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 25)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
				xmlNamespaceResolver2.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_23).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 25)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				return grid;
			}

			// Token: 0x0400257D RID: 9597
			internal object[] parentValues;

			// Token: 0x0400257E RID: 9598
			internal MapWithMultiChartV2 root;
		}

		// Token: 0x02000704 RID: 1796
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_24
		{
			// Token: 0x06003D1A RID: 15642 RVA: 0x00326484 File Offset: 0x00324684
			public <InitializeComponent>_anonXamlCDataTemplate_24()
			{
			}

			// Token: 0x06003D1B RID: 15643 RVA: 0x00326498 File Offset: 0x00324698
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 381, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 385, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 388, 41);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 392, 41);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 390, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapWithMultiChartV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
				grid.SetValue(VisualElement.WidthRequestProperty, 200.0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				boxView.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Path = "IconColor";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				boxView.SetBinding(VisualElement.BackgroundColorProperty, bindingBase);
				boxView.SetValue(VisualElement.HeightRequestProperty, 10.0);
				boxView.SetValue(VisualElement.WidthRequestProperty, 10.0);
				bindingExtension2.Path = "IconColor";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				boxView.SetBinding(BoxView.ColorProperty, bindingBase2);
				grid.Children.Add(boxView);
				label.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension.Key = "BaseFontSize-";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("maps", "clr-namespace:Xamarin.Forms.Maps;assembly=Xamarin.Forms.Maps");
				xmlNamespaceResolver.Add("range", "clr-namespace:Syncfusion.SfRangeSlider.XForms;assembly=Syncfusion.SfRangeSlider.XForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapWithMultiChartV2.<InitializeComponent>_anonXamlCDataTemplate_24).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(392, 41)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.FontSize = (double)obj2;
				bindingExtension3.Mode = 4;
				bindingExtension3.Path = "Label";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase3);
				grid.Children.Add(label);
				return grid;
			}

			// Token: 0x0400257F RID: 9599
			internal object[] parentValues;

			// Token: 0x04002580 RID: 9600
			internal MapWithMultiChartV2 root;
		}
	}
}
