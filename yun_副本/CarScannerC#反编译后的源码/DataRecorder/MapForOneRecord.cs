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
using CarScannerXamarinForms.PlatformAdapters;
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
	// Token: 0x020006F0 RID: 1776
	[XamlCompilation(2)]
	[XamlFilePath("DataRecorder\\MapForOneRecord.xaml")]
	public class MapForOneRecord : ContentPage
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x00318FA8 File Offset: 0x003171A8
		public MapForOneRecord(IDataRecordContainer Recorder, List<PointWithTime> allPositions)
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
			this.filteredPositions = this.FilterPositions(allPositions, list);
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
		}

		// Token: 0x170013F6 RID: 5110
		// (get) Token: 0x06003C7C RID: 15484 RVA: 0x0031945C File Offset: 0x0031765C
		// (set) Token: 0x06003C7D RID: 15485 RVA: 0x00319464 File Offset: 0x00317664
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
				StringBuilder stringBuilder = new StringBuilder(6);
				stringBuilder.Append(TimeSpan.FromSeconds(this._CurrentPoint.TimeSeconds).ToString("hh\\:mm\\:ss"));
				stringBuilder.Append(" [");
				stringBuilder.Append(index.ToString());
				stringBuilder.Append("/");
				stringBuilder.Append(this.filteredPositions.Count.ToString());
				stringBuilder.Append("]");
				this.lbCurrentPoint.Text = stringBuilder.ToString();
			}
		}

		// Token: 0x170013F7 RID: 5111
		// (get) Token: 0x06003C7E RID: 15486 RVA: 0x00319510 File Offset: 0x00317710
		// (set) Token: 0x06003C7F RID: 15487 RVA: 0x00319518 File Offset: 0x00317718
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

		// Token: 0x170013F8 RID: 5112
		// (get) Token: 0x06003C80 RID: 15488 RVA: 0x00319521 File Offset: 0x00317721
		// (set) Token: 0x06003C81 RID: 15489 RVA: 0x00319529 File Offset: 0x00317729
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

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06003C82 RID: 15490 RVA: 0x00319554 File Offset: 0x00317754
		public string CurrentSpeedTitle
		{
			get
			{
				return this.CurrentSpeed.ToString() + "x";
			}
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x0031957C File Offset: 0x0031777C
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

		// Token: 0x06003C84 RID: 15492 RVA: 0x003195C0 File Offset: 0x003177C0
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

		// Token: 0x06003C85 RID: 15493 RVA: 0x0031960C File Offset: 0x0031780C
		private void btnPlay_Clicked(object sender, EventArgs e)
		{
			this.IsPlaying = true;
			this.btnPlay.IsVisible = false;
			this.btnPause.IsVisible = true;
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0031962D File Offset: 0x0031782D
		private void btnPause_Clicked(object sender, EventArgs e)
		{
			this.IsPlaying = false;
			this.btnPlay.IsVisible = true;
			this.btnPause.IsVisible = false;
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06003C87 RID: 15495 RVA: 0x0031964E File Offset: 0x0031784E
		// (set) Token: 0x06003C88 RID: 15496 RVA: 0x00319656 File Offset: 0x00317856
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

		// Token: 0x06003C89 RID: 15497 RVA: 0x00319678 File Offset: 0x00317878
		private async Task PlayLoop()
		{
			while (this.IsPlaying)
			{
				int num = this.CurrentPoint.Index + 1;
				if (num != this.filteredPositions.Count)
				{
					MapForOneRecord.<>c__DisplayClass29_0 CS$<>8__locals1 = new MapForOneRecord.<>c__DisplayClass29_0();
					CS$<>8__locals1.<>4__this = this;
					CS$<>8__locals1.nextpoint = this.filteredPositions[num];
					await Task.Delay(TimeSpan.FromSeconds((CS$<>8__locals1.nextpoint.TimeSeconds - this.CurrentPoint.TimeSeconds) / this.CurrentSpeed));
					if (this.IsPlaying)
					{
						await MainThread.InvokeOnMainThreadAsync(delegate
						{
							if (CS$<>8__locals1.<>4__this.Navigation.NavigationStack[CS$<>8__locals1.<>4__this.Navigation.NavigationStack.Count - 1] == CS$<>8__locals1.<>4__this)
							{
								CS$<>8__locals1.<>4__this.UpdateForTime(CS$<>8__locals1.nextpoint.TimeSeconds);
							}
						});
						CS$<>8__locals1 = null;
						continue;
					}
				}
				return;
			}
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x003196BC File Offset: 0x003178BC
		public void NextStep()
		{
			int num = this.CurrentPoint.Index + 1;
			if (num <= this.filteredPositions.Count)
			{
				this.UpdateForTime(this.filteredPositions[num].TimeSeconds);
			}
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x00319704 File Offset: 0x00317904
		public void PrevStep()
		{
			int index = this.CurrentPoint.Index;
			if (index == 0)
			{
				return;
			}
			int num = index - 1;
			this.UpdateForTime(this.filteredPositions[num].TimeSeconds);
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x00319744 File Offset: 0x00317944
		private List<PointWithTime> FilterPositions(List<PointWithTime> allPositions, List<DataRecord> records)
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
							func = (<>9__1 = (DataRecordElement x) => x.Position == pos.ToPoint());
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
			return list;
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x0031987C File Offset: 0x00317A7C
		private void Slider_ValueChanging(object sender, ValueEventArgs e)
		{
			this.UpdateForTime(this.slider.Value);
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Slider_DragStarted(object sender, DragThumbEventArgs e)
		{
		}

		// Token: 0x06003C8F RID: 15503 RVA: 0x0031987C File Offset: 0x00317A7C
		private void Slider_DragCompleted(object sender, DragThumbEventArgs e)
		{
			this.UpdateForTime(this.slider.Value);
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x00319890 File Offset: 0x00317A90
		private PointWithTime GetClosestRecordedPositionToClickedPosition(Point pos)
		{
			return this.filteredPositions.OrderBy((PointWithTime x) => x.GetDistanceToPoint(pos)).First<PointWithTime>();
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x003198C8 File Offset: 0x00317AC8
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
				if (PlatformHelper.IsAndroid)
				{
					fastLineSeries.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				}
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

		// Token: 0x06003C92 RID: 15506 RVA: 0x00319C28 File Offset: 0x00317E28
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
				if (PlatformHelper.IsAndroid)
				{
					fastLineSeries.StrokeDashArray = new double[] { 1.0, 2.0, 3.0 };
				}
				Binding binding = new Binding("LegendTitleWithValue", 2, null, null, null, dataRecordElementCollectionWithContainerReference);
				fastLineSeries.SetBinding(ChartSeries.LabelProperty, binding);
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				this.chart.Series.Add(fastLineSeries);
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00319D44 File Offset: 0x00317F44
		private void DrawLineForAll()
		{
			this.DrawLine(this.allPositions, Color.DarkGreen, 8.0);
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x00319D64 File Offset: 0x00317F64
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

		// Token: 0x06003C95 RID: 15509 RVA: 0x00319E14 File Offset: 0x00318014
		private void DrawPointsAsCircles(List<PointWithTime> positions, Color color, double thickness)
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

		// Token: 0x06003C96 RID: 15510 RVA: 0x00319EA8 File Offset: 0x003180A8
		private void DrawLineAsPolyLines(DataRecordElementCollectionWithContainerReference rec, double thickness)
		{
			double num = rec.DataRecord.Elements.Where((DataRecordElement x) => double.IsFinite(x.Value)).Min((DataRecordElement x) => x.Value);
			double num2 = rec.DataRecord.Elements.Where((DataRecordElement x) => double.IsFinite(x.Value)).Max((DataRecordElement x) => x.Value);
			for (int i = 0; i < rec.DataRecord.Elements.Count - 1; i++)
			{
				if (i + 1 < rec.DataRecord.Elements.Count)
				{
					DataRecordElement dataRecordElement = rec.DataRecord.Elements[i];
					if (double.IsFinite(dataRecordElement.Value))
					{
						DataRecordElement dataRecordElement2 = rec.DataRecord.Elements[i + 1];
						if (Distance.BetweenPositions(dataRecordElement.Position.ToPosition(), dataRecordElement2.Position.ToPosition()).Kilometers <= 100.0)
						{
							Color color = this.GetColor(dataRecordElement.Value, num, num2);
							Polyline polyline = new Polyline
							{
								StrokeColor = color,
								StrokeWidth = (float)thickness
							};
							polyline.Geopath.Add(dataRecordElement.Position.ToPosition());
							polyline.Geopath.Add(dataRecordElement2.Position.ToPosition());
							this.map.MapElements.Add(polyline);
						}
					}
				}
			}
		}

		// Token: 0x06003C97 RID: 15511 RVA: 0x0031A068 File Offset: 0x00318268
		private void DrawLineStartingExistingAndSetPositionsForSelected()
		{
			this.DrawLineAsPolyLines(this.recordsItemsWithElement[0], 8.0);
			this.PointStart = this.filteredPositions[0];
			this.PointFinish = this.filteredPositions[this.filteredPositions.Count - 1];
		}

		// Token: 0x06003C98 RID: 15512 RVA: 0x00271490 File Offset: 0x0026F690
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DataViewer"), Translate.GetString("ios_DataViewerOne_Info"), "OK");
		}

		// Token: 0x06003C99 RID: 15513 RVA: 0x0031A0C0 File Offset: 0x003182C0
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

		// Token: 0x06003C9A RID: 15514 RVA: 0x0031A15B File Offset: 0x0031835B
		private void zoomModeButton_Clicked(object sender, EventArgs e)
		{
			this.ChangeZoomMode();
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x0031A164 File Offset: 0x00318364
		private void Map_MapClicked(object sender, MapClickedEventArgs e)
		{
			PointWithTime closestRecordedPositionToClickedPosition = this.GetClosestRecordedPositionToClickedPosition(new Point(e.Position.Latitude, e.Position.Longitude));
			this.UpdateForPosition(closestRecordedPositionToClickedPosition);
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x0031A1A0 File Offset: 0x003183A0
		private void UpdateForPosition(PointWithTime selectedPoint)
		{
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in this.recordsItemsWithElement)
			{
				dataRecordElementCollectionWithContainerReference.UpdateForLastTime(selectedPoint.TimeSeconds);
			}
			this.CurrentPoint = selectedPoint;
			this.slider.Value = this.CurrentPoint.TimeSeconds;
			this.UpdateMapMarker();
			this.UpdateCurrentGeoposition();
		}

		// Token: 0x06003C9D RID: 15517 RVA: 0x0031A224 File Offset: 0x00318424
		private void UpdateForTime(double time)
		{
			PointWithTime pointWithTime = this.FindClosestPositionForTime(time);
			foreach (DataRecordElementCollectionWithContainerReference dataRecordElementCollectionWithContainerReference in this.recordsItemsWithElement)
			{
				dataRecordElementCollectionWithContainerReference.UpdateForLastTime(pointWithTime.TimeSeconds);
			}
			this.CurrentPoint = pointWithTime;
			this.slider.Value = this.CurrentPoint.TimeSeconds;
			this.UpdateMapMarker();
			this.UpdateCurrentGeoposition();
		}

		// Token: 0x06003C9E RID: 15518 RVA: 0x0031A2B0 File Offset: 0x003184B0
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

		// Token: 0x06003C9F RID: 15519 RVA: 0x0031A330 File Offset: 0x00318530
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

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0031A3D4 File Offset: 0x003185D4
		private void UpdateCurrentGeoposition()
		{
			MapSpan visibleRegion = this.map.VisibleRegion;
			MapSpan mapSpan = MapSpan.FromCenterAndRadius(this.CurrentPoint.ToPosition(), (visibleRegion == null) ? Distance.FromMeters(300.0) : visibleRegion.Radius);
			this.map.MoveToRegion(mapSpan);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x0031A42C File Offset: 0x0031862C
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

		// Token: 0x06003CA2 RID: 15522 RVA: 0x0031A4DC File Offset: 0x003186DC
		private Color GetColor(double value, double minValue, double maxValue)
		{
			int stepPercentFromValue = this.GetStepPercentFromValue(value, minValue, maxValue);
			int num = stepPercentFromValue / 5;
			int num2 = num + 1;
			if (num >= 19)
			{
				num = 18;
				num2 = 19;
			}
			int stepPercentFromValue2 = this.GetStepPercentFromValue((double)(stepPercentFromValue % 5), 0.0, 5.0);
			return this.GetGradientColorForStep(stepPercentFromValue2, this.colorSets[num], this.colorSets[num2]);
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0031A544 File Offset: 0x00318744
		private int GetStepPercentFromValue(double value, double minValue, double maxValue)
		{
			double num = Math.Abs(Math.Abs(maxValue) - Math.Abs(minValue));
			return (int)(Math.Abs(value) * 100.0 / num);
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x0031A578 File Offset: 0x00318778
		private Color GetGradientColorForStep(int stepPercent, Color colorStart, Color colorFinish)
		{
			int num = (int)(colorStart.R * 255.0);
			int num2 = (int)(colorFinish.R * 255.0);
			int num3 = (int)(colorStart.G * 255.0);
			int num4 = (int)(colorFinish.G * 255.0);
			int num5 = (int)(colorStart.B * 255.0);
			int num6 = (int)(colorFinish.B * 255.0);
			new List<Color>();
			int num7 = num + (num2 - num) * stepPercent / 100;
			int num8 = num3 + (num4 - num3) * stepPercent / 100;
			int num9 = num5 + (num6 - num5) * stepPercent / 100;
			return Color.FromRgb(num7, num8, num9);
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x0031A630 File Offset: 0x00318830
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MapForOneRecord).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/MapForOneRecord.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
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
			VisualDiagnostics.RegisterSourceInfo(chartColorCollection = new ChartColorCollection(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			Map map;
			VisualDiagnostics.RegisterSourceInfo(map = new Map(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 21);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 21);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 21);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 21);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 18);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 21);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 21);
			LinkButton linkButton5;
			VisualDiagnostics.RegisterSourceInfo(linkButton5 = new LinkButton(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 14);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			SfRangeSlider sfRangeSlider;
			VisualDiagnostics.RegisterSourceInfo(sfRangeSlider = new SfRangeSlider(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 18);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 14);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 14);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 17);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 44);
			ChartColorModel chartColorModel;
			VisualDiagnostics.RegisterSourceInfo(chartColorModel = new ChartColorModel(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 25);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 30);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 51);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 22);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 25);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 30);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 51);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 300, 22);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 307, 22);
			ChartLegend chartLegend;
			VisualDiagnostics.RegisterSourceInfo(chartLegend = new ChartLegend(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 14);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("map", map);
			if (map.StyleId == null)
			{
				map.StyleId = "map";
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
			nameScope.RegisterName("zoomModeButton", linkButton5);
			if (linkButton5.StyleId == null)
			{
				linkButton5.StyleId = "zoomModeButton";
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
			nameScope.RegisterName("lbCurrentPoint", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "lbCurrentPoint";
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
			this.page = this;
			this.map = map;
			this.buttonsGrid = grid;
			this.btnPlay = linkButton;
			this.btnPause = linkButton2;
			this.btnStepPrev = linkButton3;
			this.btnStepNext = linkButton4;
			this.lbSpeed = label;
			this.zoomModeButton = linkButton5;
			this.lbStartTime = label2;
			this.slider = sfRangeSlider;
			this.lbFinishTime = label3;
			this.lbCurrentPoint = label4;
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.yaxis = numericalAxis2;
			this.zoomBehave = chartZoomPanBehavior;
			this.Legend = chartLegend;
			this.lv = sfListView;
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
			IDataTemplate dataTemplate3 = dataTemplate;
			MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_20 <InitializeComponent>_anonXamlCDataTemplate_ = new MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_20();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("mobileTemplate", dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_21 <InitializeComponent>_anonXamlCDataTemplate_2 = new MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_21();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid3.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("0.4*, Auto, Auto, Auto, 0.6*"));
			map.SetValue(Grid.RowProperty, 0);
			map.SetValue(Map.HasScrollEnabledProperty, true);
			map.SetValue(Map.HasZoomEnabledProperty, true);
			map.SetValue(Map.IsShowingUserProperty, false);
			map.MapClicked += this.Map_MapClicked;
			map.SetValue(Map.MapTypeProperty, 0);
			grid3.Children.Add(map);
			grid.SetValue(Grid.RowProperty, 1);
			grid.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("auto,auto,auto,auto, *"));
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = grid3;
			array4[3] = this;
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 21)));
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
			object[] array5 = new object[0 + 4];
			array5[0] = linkButton2;
			array5[1] = grid;
			array5[2] = grid3;
			array5[3] = this;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 21)));
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
			object[] array6 = new object[0 + 4];
			array6[0] = linkButton3;
			array6[1] = grid;
			array6[2] = grid3;
			array6[3] = this;
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 21)));
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
			object[] array7 = new object[0 + 4];
			array7[0] = linkButton4;
			array7[1] = grid;
			array7[2] = grid3;
			array7[3] = this;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 21)));
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
			object[] array8 = new object[0 + 4];
			array8[0] = label;
			array8[1] = grid;
			array8[2] = grid3;
			array8[3] = this;
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 21)));
			object obj7 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetValue(BindableObject.BindingContextProperty, obj7);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = label;
			array9[1] = grid;
			array9[2] = grid3;
			array9[3] = this;
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(194, 21)));
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
			object[] array10 = new object[0 + 4];
			array10[0] = linkButton5;
			array10[1] = grid;
			array10[2] = grid3;
			array10[3] = this;
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(202, 21)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			linkButton5.Clicked += this.zoomModeButton_Clicked;
			linkButton5.SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			linkButton5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton5.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("resize_x.png"));
			linkButton5.SetValue(Button.TextProperty, " ");
			linkButton5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton5);
			grid3.Children.Add(grid);
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
			grid3.Children.Add(grid2);
			label4.SetValue(Grid.RowProperty, 3);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid3.Children.Add(label4);
			sfChart.SetValue(Grid.RowProperty, 4);
			sfChart.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension7.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = sfChart;
			array11[1] = grid3;
			array11[2] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array11, SfChart.AreaBackgroundColorProperty, nameScope));
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 17)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource7.Key);
			dynamicResourceExtension8.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = sfChart;
			array12[1] = grid3;
			array12[2] = this;
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 17)));
			DynamicResource dynamicResource8 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			staticResourceExtension2.Key = "Colors";
			IMarkupExtension markupExtension11 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = chartColorModel;
			array13[1] = sfChart;
			array13[2] = grid3;
			array13[3] = this;
			object obj13;
			xamlServiceProvider11.Add(typeFromHandle21, obj13 = new SimpleValueTargetProvider(array13, ChartColorModel.CustomBrushesProperty, nameScope));
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 44)));
			object obj14 = markupExtension11.ProvideValue(xamlServiceProvider11);
			chartColorModel.CustomBrushes = obj14;
			chartColorModel.SetValue(ChartColorModel.PaletteProperty, 5);
			sfChart.SetValue(SfChart.ColorModelProperty, chartColorModel);
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, true);
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			staticResourceExtension3.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension12 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = numericalAxis;
			array14[1] = sfChart;
			array14[2] = grid3;
			array14[3] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array14, ChartAxis.MajorGridLineStyleProperty, nameScope));
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(257, 25)));
			object obj16 = markupExtension12.ProvideValue(xamlServiceProvider12);
			numericalAxis.MajorGridLineStyle = obj16;
			staticResourceExtension4.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension13 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = numericalAxis;
			array15[1] = sfChart;
			array15[2] = grid3;
			array15[3] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array15, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 25)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			numericalAxis.MajorTickStyle = obj18;
			dynamicResourceExtension9.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = chartAxisLabelStyle;
			array16[1] = numericalAxis;
			array16[2] = sfChart;
			array16[3] = grid3;
			array16[4] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array16, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
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
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(260, 56)));
			DynamicResource dynamicResource9 = markupExtension14.ProvideValue(xamlServiceProvider14);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource9.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension10.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = chartLineStyle;
			array17[1] = numericalAxis;
			array17[2] = sfChart;
			array17[3] = grid3;
			array17[4] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle29, obj20 = new SimpleValueTargetProvider(array17, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
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
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 51)));
			DynamicResource dynamicResource10 = markupExtension15.ProvideValue(xamlServiceProvider15);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource10.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 0);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 0);
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension5.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension16 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = numericalAxis2;
			array18[1] = sfChart;
			array18[2] = grid3;
			array18[3] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle31, obj21 = new SimpleValueTargetProvider(array18, ChartAxis.MajorGridLineStyleProperty, nameScope));
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
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(274, 25)));
			object obj22 = markupExtension16.ProvideValue(xamlServiceProvider16);
			numericalAxis2.MajorGridLineStyle = obj22;
			staticResourceExtension6.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension17 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = numericalAxis2;
			array19[1] = sfChart;
			array19[2] = grid3;
			array19[3] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array19, ChartAxis.MajorTickStyleProperty, nameScope));
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
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 25)));
			object obj24 = markupExtension17.ProvideValue(xamlServiceProvider17);
			numericalAxis2.MajorTickStyle = obj24;
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			dynamicResourceExtension11.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = chartAxisLabelStyle2;
			array20[1] = numericalAxis2;
			array20[2] = sfChart;
			array20[3] = grid3;
			array20[4] = this;
			object obj25;
			xamlServiceProvider18.Add(typeFromHandle35, obj25 = new SimpleValueTargetProvider(array20, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj25);
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
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(278, 56)));
			DynamicResource dynamicResource11 = markupExtension18.ProvideValue(xamlServiceProvider18);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource11.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension12.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 5];
			array21[0] = chartLineStyle2;
			array21[1] = numericalAxis2;
			array21[2] = sfChart;
			array21[3] = grid3;
			array21[4] = this;
			object obj26;
			xamlServiceProvider19.Add(typeFromHandle37, obj26 = new SimpleValueTargetProvider(array21, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj26);
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
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(MapForOneRecord).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(281, 51)));
			DynamicResource dynamicResource12 = markupExtension19.ProvideValue(xamlServiceProvider19);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource12.Key);
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
			sfChart.SetValue(SfChart.LegendProperty, chartLegend);
			grid3.Children.Add(sfChart);
			sfListView.SetValue(Grid.RowProperty, 4);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			sfListView.SetValue(SfListView.SelectionModeProperty, 3);
			grid3.Children.Add(sfListView);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0031DE80 File Offset: 0x0031C080
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.chart.IsVisible = !this.chart.IsVisible;
			this.lv.IsVisible = !this.chart.IsVisible;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0031DEB4 File Offset: 0x0031C0B4
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.Legend.IsVisible = !this.Legend.IsVisible;
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x0031DECF File Offset: 0x0031C0CF
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x0031DEDC File Offset: 0x0031C0DC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MapForOneRecord>(this, typeof(MapForOneRecord));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.map = NameScopeExtensions.FindByName<Map>(this, "map");
			this.buttonsGrid = NameScopeExtensions.FindByName<Grid>(this, "buttonsGrid");
			this.btnPlay = NameScopeExtensions.FindByName<LinkButton>(this, "btnPlay");
			this.btnPause = NameScopeExtensions.FindByName<LinkButton>(this, "btnPause");
			this.btnStepPrev = NameScopeExtensions.FindByName<LinkButton>(this, "btnStepPrev");
			this.btnStepNext = NameScopeExtensions.FindByName<LinkButton>(this, "btnStepNext");
			this.lbSpeed = NameScopeExtensions.FindByName<Label>(this, "lbSpeed");
			this.zoomModeButton = NameScopeExtensions.FindByName<LinkButton>(this, "zoomModeButton");
			this.lbStartTime = NameScopeExtensions.FindByName<Label>(this, "lbStartTime");
			this.slider = NameScopeExtensions.FindByName<SfRangeSlider>(this, "slider");
			this.lbFinishTime = NameScopeExtensions.FindByName<Label>(this, "lbFinishTime");
			this.lbCurrentPoint = NameScopeExtensions.FindByName<Label>(this, "lbCurrentPoint");
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.yaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "yaxis");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
			this.Legend = NameScopeExtensions.FindByName<ChartLegend>(this, "Legend");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
		}

		// Token: 0x040024F8 RID: 9464
		private Pin pin;

		// Token: 0x040024F9 RID: 9465
		private PointWithTime _CurrentPoint = PointWithTime.Zero;

		// Token: 0x040024FA RID: 9466
		private List<PointWithTime> allPositions;

		// Token: 0x040024FB RID: 9467
		private List<PointWithTime> filteredPositions;

		// Token: 0x040024FC RID: 9468
		private PointWithTime PointStart;

		// Token: 0x040024FD RID: 9469
		private PointWithTime PointFinish;

		// Token: 0x040024FE RID: 9470
		private List<DataRecordElementCollectionWithContainerReference> recordsItemsWithElement;

		// Token: 0x040024FF RID: 9471
		[CompilerGenerated]
		private double[] <SpeedArray>k__BackingField;

		// Token: 0x04002500 RID: 9472
		private double _CurrentSpeed = 1.0;

		// Token: 0x04002501 RID: 9473
		private bool _IsPlaying;

		// Token: 0x04002502 RID: 9474
		private Color[] colorSets = new Color[]
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

		// Token: 0x04002503 RID: 9475
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002504 RID: 9476
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Map map;

		// Token: 0x04002505 RID: 9477
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid buttonsGrid;

		// Token: 0x04002506 RID: 9478
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPlay;

		// Token: 0x04002507 RID: 9479
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPause;

		// Token: 0x04002508 RID: 9480
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnStepPrev;

		// Token: 0x04002509 RID: 9481
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnStepNext;

		// Token: 0x0400250A RID: 9482
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbSpeed;

		// Token: 0x0400250B RID: 9483
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton zoomModeButton;

		// Token: 0x0400250C RID: 9484
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbStartTime;

		// Token: 0x0400250D RID: 9485
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRangeSlider slider;

		// Token: 0x0400250E RID: 9486
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbFinishTime;

		// Token: 0x0400250F RID: 9487
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbCurrentPoint;

		// Token: 0x04002510 RID: 9488
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x04002511 RID: 9489
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x04002512 RID: 9490
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis yaxis;

		// Token: 0x04002513 RID: 9491
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x04002514 RID: 9492
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartLegend Legend;

		// Token: 0x04002515 RID: 9493
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x020006F1 RID: 1777
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003CAA RID: 15530 RVA: 0x0031E03D File Offset: 0x0031C23D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003CAB RID: 15531 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003CAC RID: 15532 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <.ctor>b__0_3(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003CAD RID: 15533 RVA: 0x0031E049 File Offset: 0x0031C249
			internal DataRecordElementCollectionWithContainerReference <.ctor>b__0_4(DataRecord x)
			{
				return new DataRecordElementCollectionWithContainerReference(x);
			}

			// Token: 0x06003CAE RID: 15534 RVA: 0x0031E051 File Offset: 0x0031C251
			internal UnitsHelper.Units <.ctor>b__0_5(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units;
			}

			// Token: 0x06003CAF RID: 15535 RVA: 0x0031E05E File Offset: 0x0031C25E
			internal bool <FilterPositions>b__32_0(DataRecordElement x)
			{
				return !double.IsNaN(x.Value);
			}

			// Token: 0x06003CB0 RID: 15536 RVA: 0x0031E051 File Offset: 0x0031C251
			internal UnitsHelper.Units <BuildInterfaceFor2Units>b__37_0(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units;
			}

			// Token: 0x06003CB1 RID: 15537 RVA: 0x00315BA4 File Offset: 0x00313DA4
			internal bool <DrawLineAsPolyLines>b__42_0(DataRecordElement x)
			{
				return double.IsFinite(x.Value);
			}

			// Token: 0x06003CB2 RID: 15538 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <DrawLineAsPolyLines>b__42_1(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x06003CB3 RID: 15539 RVA: 0x00315BA4 File Offset: 0x00313DA4
			internal bool <DrawLineAsPolyLines>b__42_2(DataRecordElement x)
			{
				return double.IsFinite(x.Value);
			}

			// Token: 0x06003CB4 RID: 15540 RVA: 0x00318870 File Offset: 0x00316A70
			internal double <DrawLineAsPolyLines>b__42_3(DataRecordElement x)
			{
				return x.Value;
			}

			// Token: 0x04002516 RID: 9494
			public static readonly MapForOneRecord.<>c <>9 = new MapForOneRecord.<>c();

			// Token: 0x04002517 RID: 9495
			public static Func<DataRecord, bool> <>9__0_3;

			// Token: 0x04002518 RID: 9496
			public static Func<DataRecord, DataRecordElementCollectionWithContainerReference> <>9__0_4;

			// Token: 0x04002519 RID: 9497
			public static Func<DataRecordElementCollectionWithContainerReference, UnitsHelper.Units> <>9__0_5;

			// Token: 0x0400251A RID: 9498
			public static Func<DataRecordElement, bool> <>9__32_0;

			// Token: 0x0400251B RID: 9499
			public static Func<DataRecordElementCollectionWithContainerReference, UnitsHelper.Units> <>9__37_0;

			// Token: 0x0400251C RID: 9500
			public static Func<DataRecordElement, bool> <>9__42_0;

			// Token: 0x0400251D RID: 9501
			public static Func<DataRecordElement, double> <>9__42_1;

			// Token: 0x0400251E RID: 9502
			public static Func<DataRecordElement, bool> <>9__42_2;

			// Token: 0x0400251F RID: 9503
			public static Func<DataRecordElement, double> <>9__42_3;
		}

		// Token: 0x020006F2 RID: 1778
		[CompilerGenerated]
		private sealed class <>c__DisplayClass29_0
		{
			// Token: 0x06003CB5 RID: 15541 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass29_0()
			{
			}

			// Token: 0x06003CB6 RID: 15542 RVA: 0x0031E070 File Offset: 0x0031C270
			internal void <PlayLoop>b__0()
			{
				if (this.<>4__this.Navigation.NavigationStack[this.<>4__this.Navigation.NavigationStack.Count - 1] == this.<>4__this)
				{
					this.<>4__this.UpdateForTime(this.nextpoint.TimeSeconds);
				}
			}

			// Token: 0x04002520 RID: 9504
			public PointWithTime nextpoint;

			// Token: 0x04002521 RID: 9505
			public MapForOneRecord <>4__this;
		}

		// Token: 0x020006F3 RID: 1779
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_0
		{
			// Token: 0x06003CB7 RID: 15543 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_0()
			{
			}

			// Token: 0x06003CB8 RID: 15544 RVA: 0x0031E0C7 File Offset: 0x0031C2C7
			internal bool <FilterPositions>b__1(DataRecordElement x)
			{
				return x.Position == this.pos.ToPoint();
			}

			// Token: 0x04002522 RID: 9506
			public PointWithTime pos;

			// Token: 0x04002523 RID: 9507
			public Func<DataRecordElement, bool> <>9__1;
		}

		// Token: 0x020006F4 RID: 1780
		[CompilerGenerated]
		private sealed class <>c__DisplayClass36_0
		{
			// Token: 0x06003CB9 RID: 15545 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass36_0()
			{
			}

			// Token: 0x06003CBA RID: 15546 RVA: 0x0031E0DF File Offset: 0x0031C2DF
			internal double <GetClosestRecordedPositionToClickedPosition>b__0(PointWithTime x)
			{
				return x.GetDistanceToPoint(this.pos);
			}

			// Token: 0x04002524 RID: 9508
			public Point pos;
		}

		// Token: 0x020006F5 RID: 1781
		[CompilerGenerated]
		private sealed class <>c__DisplayClass37_0
		{
			// Token: 0x06003CBB RID: 15547 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass37_0()
			{
			}

			// Token: 0x06003CBC RID: 15548 RVA: 0x0031E0ED File Offset: 0x0031C2ED
			internal bool <BuildInterfaceFor2Units>b__1(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units == this.leftUnit;
			}

			// Token: 0x06003CBD RID: 15549 RVA: 0x0031E102 File Offset: 0x0031C302
			internal bool <BuildInterfaceFor2Units>b__2(DataRecordElementCollectionWithContainerReference x)
			{
				return x.DataRecord.Units == this.rightUnit;
			}

			// Token: 0x04002525 RID: 9509
			public UnitsHelper.Units leftUnit;

			// Token: 0x04002526 RID: 9510
			public UnitsHelper.Units rightUnit;
		}

		// Token: 0x020006F6 RID: 1782
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PlayLoop>d__29 : IAsyncStateMachine
		{
			// Token: 0x06003CBE RID: 15550 RVA: 0x0031E118 File Offset: 0x0031C318
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MapForOneRecord mapForOneRecord = this;
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
					if (!mapForOneRecord.IsPlaying)
					{
						goto IL_019F;
					}
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						if (CS$<>8__locals1.<>4__this.Navigation.NavigationStack[CS$<>8__locals1.<>4__this.Navigation.NavigationStack.Count - 1] == CS$<>8__locals1.<>4__this)
						{
							CS$<>8__locals1.<>4__this.UpdateForTime(CS$<>8__locals1.nextpoint.TimeSeconds);
						}
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapForOneRecord.<PlayLoop>d__29>(ref taskAwaiter, ref this);
						return;
					}
					IL_016B:
					taskAwaiter.GetResult();
					CS$<>8__locals1 = null;
					IL_0179:
					if (mapForOneRecord.IsPlaying)
					{
						int num3 = mapForOneRecord.CurrentPoint.Index + 1;
						if (num3 != mapForOneRecord.filteredPositions.Count)
						{
							CS$<>8__locals1 = new MapForOneRecord.<>c__DisplayClass29_0();
							CS$<>8__locals1.<>4__this = mapForOneRecord;
							CS$<>8__locals1.nextpoint = mapForOneRecord.filteredPositions[num3];
							taskAwaiter = Task.Delay(TimeSpan.FromSeconds((CS$<>8__locals1.nextpoint.TimeSeconds - mapForOneRecord.CurrentPoint.TimeSeconds) / mapForOneRecord.CurrentSpeed)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MapForOneRecord.<PlayLoop>d__29>(ref taskAwaiter, ref this);
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

			// Token: 0x06003CBF RID: 15551 RVA: 0x0031E2F4 File Offset: 0x0031C4F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002527 RID: 9511
			public int <>1__state;

			// Token: 0x04002528 RID: 9512
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002529 RID: 9513
			public MapForOneRecord <>4__this;

			// Token: 0x0400252A RID: 9514
			private MapForOneRecord.<>c__DisplayClass29_0 <>8__1;

			// Token: 0x0400252B RID: 9515
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006F7 RID: 1783
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_20
		{
			// Token: 0x06003CC0 RID: 15552 RVA: 0x0031E304 File Offset: 0x0031C504
			public <InitializeComponent>_anonXamlCDataTemplate_20()
			{
			}

			// Token: 0x06003CC1 RID: 15553 RVA: 0x0031E318 File Offset: 0x0031C518
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 39);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 34);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 34);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 30);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
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
				label3.SetValue(Grid.RowProperty, 0);
				label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				BindableObject bindableObject = label3;
				BindableProperty fontSizeProperty = Label.FontSizeProperty;
				IExtendedTypeConverter extendedTypeConverter = new FontSizeConverter();
				string text = "7";
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label3;
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_20).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 25)));
				bindableObject.SetValue(fontSizeProperty, extendedTypeConverter.ConvertFromInvariantString(text, xamlServiceProvider));
				label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "Settings.ShowPing";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				label3.SetValue(Label.TextColorProperty, Color.Red);
				label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "Ping";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				span4.SetBinding(Span.TextProperty, bindingBase5);
				formattedString2.Spans.Add(span4);
				span5.SetValue(Span.TextProperty, "ms");
				formattedString2.Spans.Add(span5);
				label3.SetValue(Label.FormattedTextProperty, formattedString2);
				grid.Children.Add(label3);
				return grid;
			}

			// Token: 0x0400252C RID: 9516
			internal object[] parentValues;

			// Token: 0x0400252D RID: 9517
			internal MapForOneRecord root;
		}

		// Token: 0x020006F8 RID: 1784
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_21
		{
			// Token: 0x06003CC2 RID: 15554 RVA: 0x0031EA58 File Offset: 0x0031CC58
			public <InitializeComponent>_anonXamlCDataTemplate_21()
			{
			}

			// Token: 0x06003CC3 RID: 15555 RVA: 0x0031EA6C File Offset: 0x0031CC6C
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 26);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 26);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 22);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\MapForOneRecord.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				BindableObject bindableObject = label;
				BindableProperty fontSizeProperty = Label.FontSizeProperty;
				IExtendedTypeConverter extendedTypeConverter = new FontSizeConverter();
				string text = "Medium";
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_21).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 25)));
				bindableObject.SetValue(fontSizeProperty, extendedTypeConverter.ConvertFromInvariantString(text, xamlServiceProvider));
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				BindableObject bindableObject2 = label2;
				BindableProperty fontSizeProperty2 = Label.FontSizeProperty;
				IExtendedTypeConverter extendedTypeConverter2 = new FontSizeConverter();
				string text2 = "Medium";
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
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MapForOneRecord.<InitializeComponent>_anonXamlCDataTemplate_21).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(110, 25)));
				bindableObject2.SetValue(fontSizeProperty2, extendedTypeConverter2.ConvertFromInvariantString(text2, xamlServiceProvider2));
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

			// Token: 0x0400252E RID: 9518
			internal object[] parentValues;

			// Token: 0x0400252F RID: 9519
			internal MapForOneRecord root;
		}
	}
}
