using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using MR.Gestures;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Dashboard.DashboardPages
{
	// Token: 0x0200079D RID: 1949
	public class Dash_CustomPage : DashboardPage
	{
		// Token: 0x060042C4 RID: 17092 RVA: 0x0033D998 File Offset: 0x0033BB98
		public Dash_CustomPage()
		{
			this.Title = Translate.GetString("dash_CustomizablePage");
			this.absLayout = new AbsoluteLayout();
			this.lbHint = new Label
			{
				Text = Translate.GetString("dash_DoubleTapToAddNew"),
				InputTransparent = true,
				VerticalTextAlignment = 1,
				HorizontalTextAlignment = 1,
				TextColor = Color.Red,
				FontAttributes = 1,
				LineBreakMode = 1,
				IsVisible = true,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};
			this.lbHint.FontSize = (double)Application.Current.Resources["BaseFontSize+++"];
			this.absLayout.ChildAdded += this.AbsLayout_ChildAdded;
			this.absLayout.ChildRemoved += this.AbsLayout_ChildRemoved;
			this.absLayout.Tapped += this.AbsLayout_Tapped;
			this.absLayout.DoubleTapped += this.AbsLayout_DoubleTapped;
			this.grid.RowDefinitions.Add(new RowDefinition
			{
				Height = GridLength.Star
			});
			this.grid.ColumnDefinitions.Add(new ColumnDefinition
			{
				Width = GridLength.Star
			});
			Grid.SetRow(this.absLayout, 0);
			Grid.SetColumn(this.absLayout, 0);
			this.grid.Children.Add(this.absLayout);
			Grid.SetRow(this.lbHint, 0);
			Grid.SetColumn(this.lbHint, 0);
			this.grid.Children.Add(this.lbHint);
			base.SizeChanged += this.Dash_CustomPage_SizeChanged;
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x0033DB69 File Offset: 0x0033BD69
		private void AbsLayout_ChildRemoved(object sender, ElementEventArgs e)
		{
			this.UpdateLabelHint();
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x0033DB69 File Offset: 0x0033BD69
		private void AbsLayout_ChildAdded(object sender, ElementEventArgs e)
		{
			this.UpdateLabelHint();
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x0033DB74 File Offset: 0x0033BD74
		private void UpdateLabelHint()
		{
			if (this.absLayout.Children.Any((View x) => x is DashboardItem))
			{
				this.lbHint.IsVisible = false;
				return;
			}
			this.lbHint.IsVisible = true;
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0033DBCC File Offset: 0x0033BDCC
		private void CreateAndAddItemFromPIDAndItemType(IPID pid, DashboardItemTypes itemType, Point position)
		{
			double num = Math.Round(((DeviceDisplay.MainDisplayInfo.Width > DeviceDisplay.MainDisplayInfo.Height) ? DeviceDisplay.MainDisplayInfo.Height : DeviceDisplay.MainDisplayInfo.Width) / DeviceDisplay.MainDisplayInfo.Density / 2.5, 0);
			DashboardItem dashboardItem = new DashboardItem
			{
				PID_Id = pid.Id,
				ItemType = itemType,
				PositionX = position.X,
				PositionY = position.Y,
				Minimum = pid.Minimum,
				Maximum = pid.Maximum,
				CustomName = pid.ShortName,
				DesiredHeight = num,
				DesiredWidth = num,
				WidthRequest = num,
				HeightRequest = num,
				ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5,
				UnitsFontSize = Device.GetNamedSize(4, typeof(Label))
			};
			this.AddNewItem(dashboardItem);
			dashboardItem.Model = new LiveDataPIDModel();
			dashboardItem.SelectAndAddControl();
			dashboardItem.Start();
			DashboardListViewModel.Current.SaveDashboardToSettings();
			try
			{
				DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Stop();
				DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Start();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x0033DD54 File Offset: 0x0033BF54
		private async void AbsLayout_DoubleTapped(object sender, TapEventArgs e)
		{
			if (!this.absLayout.Children.Any((View x) => x.Bounds.Contains(e.Center)))
			{
				if (e.NumberOfTaps == 2)
				{
					await this.AskUserToAddNewItemAsync(e.Center);
				}
			}
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x0033DD94 File Offset: 0x0033BF94
		public async Task AskUserToAddNewItemAsync(Point position)
		{
			if (SharedSettings.Current.PIDSelectorWithValuePreview)
			{
				IPID ipid = await PIDSelector.SelectPIDAsync(null, null);
				IPID pid = ipid;
				if (pid != null)
				{
					CustomPID customPID = pid as CustomPID;
					if (customPID != null && customPID.IsAction)
					{
						this.CreateAndAddItemFromPIDAndItemType(pid, DashboardItemTypes.Action, position);
					}
					else
					{
						DashboardItemTypes? dashboardItemTypes = await DashboardItemTypeSelector.SelectDashboardItemType(pid);
						if (dashboardItemTypes != null)
						{
							this.CreateAndAddItemFromPIDAndItemType(pid, dashboardItemTypes.Value, position);
						}
					}
				}
				pid = null;
			}
			else
			{
				DashboardItemAdderPage dashboardItemAdderPage = new DashboardItemAdderPage(this, position);
				App.GetCurrentPage().Navigation.PushAsync(dashboardItemAdderPage);
			}
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x0033DDE0 File Offset: 0x0033BFE0
		private void Dash_CustomPage_SizeChanged(object sender, EventArgs e)
		{
			foreach (DashboardItem dashboardItem in base.Items)
			{
				this.UpdateItem(dashboardItem);
			}
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x0033DE30 File Offset: 0x0033C030
		private void AbsLayout_Tapped(object sender, TapEventArgs e)
		{
			if (this.absLayout.Children.Any((View x) => x.Bounds.Contains(e.Center)))
			{
				return;
			}
			if (this.DraggingControl != null)
			{
				this.DraggingControl.HideBtnOkOverlay();
				this.DraggingControl = null;
			}
			if (e.NumberOfTaps == 1)
			{
				DashboardXamlPage dashboardXamlPage = App.GetCurrentPage() as DashboardXamlPage;
				if (dashboardXamlPage != null && dashboardXamlPage != null)
				{
					dashboardXamlPage.ShowTopGrid();
				}
			}
		}

		// Token: 0x170015F5 RID: 5621
		// (get) Token: 0x060042CD RID: 17101 RVA: 0x0033DEA9 File Offset: 0x0033C0A9
		// (set) Token: 0x060042CE RID: 17102 RVA: 0x0033DEB1 File Offset: 0x0033C0B1
		public DashboardItem DraggingControl
		{
			get
			{
				return this._DraggingControl;
			}
			set
			{
				if (value == this._DraggingControl)
				{
					return;
				}
				DashboardItem draggingControl = this._DraggingControl;
				if (draggingControl != null)
				{
					draggingControl.HideBtnOkOverlay();
				}
				this.GestureMode = Dash_CustomPage.GestureModes.None;
				this._DraggingControl = value;
			}
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x0033DEDC File Offset: 0x0033C0DC
		public override void Stop()
		{
			base.Stop();
			if (this._DraggingControl != null)
			{
				DashboardItem draggingControl = this._DraggingControl;
				if (draggingControl != null)
				{
					draggingControl.HideBtnOkOverlay();
				}
				this.DraggingControl = null;
				this.GestureMode = Dash_CustomPage.GestureModes.None;
			}
		}

		// Token: 0x170015F6 RID: 5622
		// (get) Token: 0x060042D0 RID: 17104 RVA: 0x0033DF0B File Offset: 0x0033C10B
		// (set) Token: 0x060042D1 RID: 17105 RVA: 0x0033DF14 File Offset: 0x0033C114
		public Dash_CustomPage.GestureModes GestureMode
		{
			get
			{
				return this._GestureMode;
			}
			set
			{
				try
				{
					this._GestureMode = value;
					this.OnPropertyChanged("GestureMode");
					switch (value)
					{
					case Dash_CustomPage.GestureModes.None:
						this.absLayout.Panned -= this.AbsLayout_Panned;
						this.absLayout.Panning -= this.AbsLayout_Panning;
						this.absLayout.Pinching -= this.AbsLayout_Pinching;
						this.absLayout.Pinched -= this.AbsLayout_Pinched;
						break;
					case Dash_CustomPage.GestureModes.Move:
						this.absLayout.Panned -= this.AbsLayout_Panned;
						this.absLayout.Panning -= this.AbsLayout_Panning;
						this.absLayout.Panned += this.AbsLayout_Panned;
						this.absLayout.Panning += this.AbsLayout_Panning;
						break;
					case Dash_CustomPage.GestureModes.ResizeWidth:
					case Dash_CustomPage.GestureModes.ResizeHeight:
					case Dash_CustomPage.GestureModes.ResizeBoth:
						this.absLayout.Pinching -= this.AbsLayout_Pinching;
						this.absLayout.Pinched -= this.AbsLayout_Pinched;
						this.absLayout.Pinching += this.AbsLayout_Pinching;
						this.absLayout.Pinched += this.AbsLayout_Pinched;
						this.absLayout.Panned -= this.AbsLayout_Panned;
						this.absLayout.Panning -= this.AbsLayout_Panning;
						this.absLayout.Panned += this.AbsLayout_Panned;
						this.absLayout.Panning += this.AbsLayout_Panning;
						break;
					case Dash_CustomPage.GestureModes.MoveAndResize:
						this.absLayout.Panned -= this.AbsLayout_Panned;
						this.absLayout.Panning -= this.AbsLayout_Panning;
						this.absLayout.Panned += this.AbsLayout_Panned;
						this.absLayout.Panning += this.AbsLayout_Panning;
						this.absLayout.Pinching -= this.AbsLayout_Pinching;
						this.absLayout.Pinched -= this.AbsLayout_Pinched;
						this.absLayout.Pinching += this.AbsLayout_Pinching;
						this.absLayout.Pinched += this.AbsLayout_Pinched;
						break;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x0033E1B4 File Offset: 0x0033C3B4
		private void AbsLayout_Pinched(object sender, PinchEventArgs e)
		{
			if (this.DraggingControl != null)
			{
				if (SharedSettings.Current.DashboardAlignItemsToGrid)
				{
					this.DraggingControl.DesiredWidth = Math.Round(this.DraggingControl.DesiredWidth / 10.0) * 10.0;
					this.DraggingControl.DesiredHeight = Math.Round(this.DraggingControl.DesiredHeight / 10.0) * 10.0;
					this.UpdateItem(this.DraggingControl);
				}
				DashboardListViewModel.Current.SaveDashboardToSettings();
			}
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x0033E24C File Offset: 0x0033C44C
		private void AbsLayout_Pinching(object sender, PinchEventArgs e)
		{
			if (this.GestureMode == Dash_CustomPage.GestureModes.None)
			{
				return;
			}
			if (e.Touches == null || e.Touches.Length != 2)
			{
				return;
			}
			if (this.DraggingControl != null && double.IsFinite(e.DeltaScale))
			{
				if (this.GestureMode == Dash_CustomPage.GestureModes.ResizeWidth)
				{
					double num = base.Bounds.Width - this.DraggingControl.X;
					double num2 = this.DraggingControl.DesiredWidth * e.DeltaScale;
					if (num2 < this.min_size)
					{
						num2 = this.min_size;
					}
					if (num2 > num)
					{
						num2 = num;
					}
					this.DraggingControl.DesiredWidth = Math.Round(num2, 0);
				}
				if (this.GestureMode == Dash_CustomPage.GestureModes.ResizeHeight)
				{
					double num3 = base.Bounds.Height - this.DraggingControl.Y;
					double num4 = this.DraggingControl.DesiredHeight * e.DeltaScale;
					if (num4 < this.min_size)
					{
						num4 = this.min_size;
					}
					if (num4 > num3)
					{
						num4 = num3;
					}
					this.DraggingControl.DesiredHeight = Math.Round(num4, 0);
				}
				if (this.GestureMode == Dash_CustomPage.GestureModes.ResizeBoth || this.GestureMode == Dash_CustomPage.GestureModes.MoveAndResize)
				{
					bool flag = false;
					double num5 = base.Bounds.Height - this.DraggingControl.Y;
					double num6 = base.Bounds.Width - this.DraggingControl.X;
					double num7 = this.DraggingControl.DesiredHeight * e.DeltaScale;
					if (num7 < this.min_size)
					{
						num7 = this.min_size;
						flag = true;
					}
					if (num7 > num5)
					{
						num7 = num5;
						flag = true;
					}
					double num8 = this.DraggingControl.DesiredWidth * e.DeltaScale;
					if (num8 < this.min_size)
					{
						num8 = this.min_size;
						flag = true;
					}
					if (num8 > num6)
					{
						num8 = num6;
						flag = true;
					}
					if (!flag)
					{
						this.DraggingControl.DesiredHeight = Math.Round(num7, 0);
						this.DraggingControl.DesiredWidth = Math.Round(num8, 0);
					}
				}
				if (this.GestureMode == Dash_CustomPage.GestureModes.ResizeWidthOrHeight)
				{
					double num9 = Math.Abs(e.DeltaScaleX);
					double num10 = Math.Abs(e.DeltaScaleY);
					double num11 = Math.Abs(num10 - num9);
					if (num9 == num10)
					{
						return;
					}
					if (num11 < 0.02)
					{
						return;
					}
					if (Math.Abs(e.DeltaScaleX) > Math.Abs(e.DeltaScaleY))
					{
						this.GestureMode = Dash_CustomPage.GestureModes.ResizeWidth;
					}
					else
					{
						this.GestureMode = Dash_CustomPage.GestureModes.ResizeHeight;
					}
				}
				this.UpdateItem(this.DraggingControl);
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x0033E4B4 File Offset: 0x0033C6B4
		public void UpdateItem(DashboardItem item)
		{
			double num = item.PositionX;
			double num2 = item.PositionY;
			if (this.Orientation == Dash_CustomPage.Orientations.Horizontal)
			{
				num2 = item.PositionX;
				num = item.PositionY;
			}
			if (num2 + item.DesiredHeight > base.Height)
			{
				num2 = base.Height - item.DesiredHeight;
			}
			if (num + item.DesiredWidth > base.Width)
			{
				num = base.Width - item.DesiredWidth;
			}
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			if (num < 0.0)
			{
				num = 0.0;
			}
			AbsoluteLayout.SetLayoutBounds(item, new Rect(num, num2, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			item.WidthRequest = item.DesiredWidth;
			item.HeightRequest = item.DesiredHeight;
		}

		// Token: 0x060042D5 RID: 17109 RVA: 0x0033E584 File Offset: 0x0033C784
		private void AbsLayout_Panned(object sender, PanEventArgs e)
		{
			if (this.DraggingControl != null)
			{
				if (SharedSettings.Current.DashboardAlignItemsToGrid)
				{
					this.DraggingControl.PositionX = Math.Round(this.DraggingControl.PositionX / 10.0) * 10.0;
					this.DraggingControl.PositionY = Math.Round(this.DraggingControl.PositionY / 10.0) * 10.0;
					this.UpdateItem(this.DraggingControl);
				}
				DashboardListViewModel.Current.SaveDashboardToSettings();
			}
		}

		// Token: 0x060042D6 RID: 17110 RVA: 0x0033E61C File Offset: 0x0033C81C
		private async void AbsLayout_Panning(object sender, PanEventArgs e)
		{
			if (e.Touches != null && e.Touches.Length == 1)
			{
				if (this.DraggingControl != null && double.IsFinite(e.DeltaDistance.X) && double.IsFinite(e.DeltaDistance.Y))
				{
					Point point = e.Touches[0];
					if (this.DraggingControl.Bounds.Contains(point) > false)
					{
						if (this.Orientation == Dash_CustomPage.Orientations.Vertical)
						{
							double num = this.DraggingControl.PositionX + e.DeltaDistance.X;
							double num2 = this.DraggingControl.PositionY + e.DeltaDistance.Y;
							if (num < 0.0)
							{
								num = 0.0;
							}
							if (num2 < 0.0)
							{
								num2 = 0.0;
							}
							double num3 = base.Width - this.DraggingControl.Width;
							double num4 = base.Height - this.DraggingControl.Height;
							if (num > num3)
							{
								num = num3;
							}
							if (num2 > num4)
							{
								num2 = num4;
							}
							num = Math.Round(num, 0);
							num2 = Math.Round(num2, 0);
							this.DraggingControl.PositionX = num;
							this.DraggingControl.PositionY = num2;
						}
						else
						{
							double num5 = this.DraggingControl.PositionX + e.DeltaDistance.Y;
							double num6 = this.DraggingControl.PositionY + e.DeltaDistance.X;
							if (num5 < 0.0)
							{
								num5 = 0.0;
							}
							if (num6 < 0.0)
							{
								num6 = 0.0;
							}
							double num7 = base.Height - this.DraggingControl.Height;
							double num8 = base.Width - this.DraggingControl.Width;
							if (num5 > num7)
							{
								num5 = num7;
							}
							if (num6 > num8)
							{
								num6 = num8;
							}
							num5 = Math.Round(num5, 0);
							num6 = Math.Round(num6, 0);
							this.DraggingControl.PositionX = num5;
							this.DraggingControl.PositionY = num6;
						}
						this.UpdateItem(this.DraggingControl);
					}
				}
			}
		}

		// Token: 0x170015F7 RID: 5623
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x0020019E File Offset: 0x001FE39E
		public override DashboardTypes DashboardType
		{
			get
			{
				return DashboardTypes.Custom;
			}
		}

		// Token: 0x060042D8 RID: 17112 RVA: 0x0033E65C File Offset: 0x0033C85C
		protected override void RotateVertical()
		{
			this.Orientation = Dash_CustomPage.Orientations.Vertical;
			for (int i = 0; i < base.Items.Count; i++)
			{
				this.UpdateItem(base.Items[i]);
			}
		}

		// Token: 0x060042D9 RID: 17113 RVA: 0x0033E698 File Offset: 0x0033C898
		protected override void RotateHorizontal()
		{
			this.Orientation = Dash_CustomPage.Orientations.Horizontal;
			for (int i = 0; i < base.Items.Count; i++)
			{
				this.UpdateItem(base.Items[i]);
			}
		}

		// Token: 0x060042DA RID: 17114 RVA: 0x0033E6D4 File Offset: 0x0033C8D4
		public override void CreateGrid()
		{
			this.models = new ObservableCollection<LiveDataPIDModel>();
			this.absLayout.Children.Clear();
			for (int i = 0; i < base.Items.Count; i++)
			{
				DashboardItem dashboardItem = base.Items[i];
				dashboardItem.Model = new LiveDataPIDModel
				{
					DoubleFormat = dashboardItem.ValueFormat
				};
				dashboardItem.SelectAndAddControl();
				this.absLayout.Children.Add(dashboardItem);
			}
			if (base.Height >= base.Width)
			{
				this.RotateVertical();
				return;
			}
			this.RotateHorizontal();
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x0033E76D File Offset: 0x0033C96D
		public void AddNewItem(DashboardItem item)
		{
			base.Items.Add(item);
			this.absLayout.Children.Add(item);
			this.UpdateItem(item);
		}

		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x060042DC RID: 17116 RVA: 0x0033E793 File Offset: 0x0033C993
		public override string PreviewFile
		{
			get
			{
				return "dash_custom.png";
			}
		}

		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x060042DD RID: 17117 RVA: 0x0033E79A File Offset: 0x0033C99A
		// (set) Token: 0x060042DE RID: 17118 RVA: 0x0033E7A7 File Offset: 0x0033C9A7
		public override int ItemsCount
		{
			get
			{
				return base.Items.Count;
			}
			set
			{
				this.OnPropertyChanged("ItemsCount");
			}
		}

		// Token: 0x060042DF RID: 17119 RVA: 0x0033E7B4 File Offset: 0x0033C9B4
		internal void RemoveItem(DashboardItem dashboardItem)
		{
			if (dashboardItem == null)
			{
				return;
			}
			if (!this.absLayout.Children.Contains(dashboardItem))
			{
				return;
			}
			this.absLayout.Children.Remove(dashboardItem);
			base.Items.Remove(dashboardItem);
		}

		// Token: 0x04002874 RID: 10356
		private const double AlignStep = 10.0;

		// Token: 0x04002875 RID: 10357
		private Label lbHint;

		// Token: 0x04002876 RID: 10358
		private Dash_CustomPage.Orientations Orientation;

		// Token: 0x04002877 RID: 10359
		private double min_size = 50.0;

		// Token: 0x04002878 RID: 10360
		private DashboardItem _DraggingControl;

		// Token: 0x04002879 RID: 10361
		private Dash_CustomPage.GestureModes _GestureMode;

		// Token: 0x0400287A RID: 10362
		private AbsoluteLayout absLayout;

		// Token: 0x0200079E RID: 1950
		private enum Orientations
		{
			// Token: 0x0400287C RID: 10364
			Vertical,
			// Token: 0x0400287D RID: 10365
			Horizontal
		}

		// Token: 0x0200079F RID: 1951
		public enum GestureModes
		{
			// Token: 0x0400287F RID: 10367
			None,
			// Token: 0x04002880 RID: 10368
			Move,
			// Token: 0x04002881 RID: 10369
			ResizeWidth,
			// Token: 0x04002882 RID: 10370
			ResizeHeight,
			// Token: 0x04002883 RID: 10371
			ResizeBoth,
			// Token: 0x04002884 RID: 10372
			ResizeWidthOrHeight,
			// Token: 0x04002885 RID: 10373
			MoveAndResize
		}

		// Token: 0x020007A0 RID: 1952
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060042E0 RID: 17120 RVA: 0x0033E7ED File Offset: 0x0033C9ED
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060042E1 RID: 17121 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060042E2 RID: 17122 RVA: 0x0033E7F9 File Offset: 0x0033C9F9
			internal bool <UpdateLabelHint>b__5_0(View x)
			{
				return x is DashboardItem;
			}

			// Token: 0x04002886 RID: 10374
			public static readonly Dash_CustomPage.<>c <>9 = new Dash_CustomPage.<>c();

			// Token: 0x04002887 RID: 10375
			public static Func<View, bool> <>9__5_0;
		}

		// Token: 0x020007A1 RID: 1953
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x060042E3 RID: 17123 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x060042E4 RID: 17124 RVA: 0x0033E804 File Offset: 0x0033CA04
			internal bool <AbsLayout_Tapped>b__0(View x)
			{
				return x.Bounds.Contains(this.e.Center);
			}

			// Token: 0x04002888 RID: 10376
			public TapEventArgs e;
		}

		// Token: 0x020007A2 RID: 1954
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x060042E5 RID: 17125 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x060042E6 RID: 17126 RVA: 0x0033E82C File Offset: 0x0033CA2C
			internal bool <AbsLayout_DoubleTapped>b__0(View x)
			{
				return x.Bounds.Contains(this.e.Center);
			}

			// Token: 0x04002889 RID: 10377
			public TapEventArgs e;
		}

		// Token: 0x020007A3 RID: 1955
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AbsLayout_DoubleTapped>d__7 : IAsyncStateMachine
		{
			// Token: 0x060042E7 RID: 17127 RVA: 0x0033E854 File Offset: 0x0033CA54
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dash_CustomPage dash_CustomPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						Dash_CustomPage.<>c__DisplayClass7_0 CS$<>8__locals1 = new Dash_CustomPage.<>c__DisplayClass7_0();
						CS$<>8__locals1.e = e;
						if (dash_CustomPage.absLayout.Children.Any((View x) => x.Bounds.Contains(CS$<>8__locals1.e.Center)))
						{
							goto IL_00D5;
						}
						if (CS$<>8__locals1.e.NumberOfTaps != 2)
						{
							goto IL_00BA;
						}
						taskAwaiter = dash_CustomPage.AskUserToAddNewItemAsync(CS$<>8__locals1.e.Center).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Dash_CustomPage.<AbsLayout_DoubleTapped>d__7>(ref taskAwaiter, ref this);
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
					IL_00BA:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00D5:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060042E8 RID: 17128 RVA: 0x0033E95C File Offset: 0x0033CB5C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400288A RID: 10378
			public int <>1__state;

			// Token: 0x0400288B RID: 10379
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400288C RID: 10380
			public TapEventArgs e;

			// Token: 0x0400288D RID: 10381
			public Dash_CustomPage <>4__this;

			// Token: 0x0400288E RID: 10382
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020007A4 RID: 1956
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AbsLayout_Panning>d__28 : IAsyncStateMachine
		{
			// Token: 0x060042E9 RID: 17129 RVA: 0x0033E96C File Offset: 0x0033CB6C
			void IAsyncStateMachine.MoveNext()
			{
				Dash_CustomPage dash_CustomPage = this;
				try
				{
					if (e.Touches != null && e.Touches.Length == 1)
					{
						if (dash_CustomPage.DraggingControl != null && double.IsFinite(e.DeltaDistance.X) && double.IsFinite(e.DeltaDistance.Y))
						{
							Point point = e.Touches[0];
							if (dash_CustomPage.DraggingControl.Bounds.Contains(point) > false)
							{
								if (dash_CustomPage.Orientation == Dash_CustomPage.Orientations.Vertical)
								{
									double num = dash_CustomPage.DraggingControl.PositionX + e.DeltaDistance.X;
									double num2 = dash_CustomPage.DraggingControl.PositionY + e.DeltaDistance.Y;
									if (num < 0.0)
									{
										num = 0.0;
									}
									if (num2 < 0.0)
									{
										num2 = 0.0;
									}
									double num3 = dash_CustomPage.Width - dash_CustomPage.DraggingControl.Width;
									double num4 = dash_CustomPage.Height - dash_CustomPage.DraggingControl.Height;
									if (num > num3)
									{
										num = num3;
									}
									if (num2 > num4)
									{
										num2 = num4;
									}
									num = Math.Round(num, 0);
									num2 = Math.Round(num2, 0);
									dash_CustomPage.DraggingControl.PositionX = num;
									dash_CustomPage.DraggingControl.PositionY = num2;
								}
								else
								{
									double num5 = dash_CustomPage.DraggingControl.PositionX + e.DeltaDistance.Y;
									double num6 = dash_CustomPage.DraggingControl.PositionY + e.DeltaDistance.X;
									if (num5 < 0.0)
									{
										num5 = 0.0;
									}
									if (num6 < 0.0)
									{
										num6 = 0.0;
									}
									double num7 = dash_CustomPage.Height - dash_CustomPage.DraggingControl.Height;
									double num8 = dash_CustomPage.Width - dash_CustomPage.DraggingControl.Width;
									if (num5 > num7)
									{
										num5 = num7;
									}
									if (num6 > num8)
									{
										num6 = num8;
									}
									num5 = Math.Round(num5, 0);
									num6 = Math.Round(num6, 0);
									dash_CustomPage.DraggingControl.PositionX = num5;
									dash_CustomPage.DraggingControl.PositionY = num6;
								}
								dash_CustomPage.UpdateItem(dash_CustomPage.DraggingControl);
							}
						}
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

			// Token: 0x060042EA RID: 17130 RVA: 0x0033EC34 File Offset: 0x0033CE34
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400288F RID: 10383
			public int <>1__state;

			// Token: 0x04002890 RID: 10384
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002891 RID: 10385
			public PanEventArgs e;

			// Token: 0x04002892 RID: 10386
			public Dash_CustomPage <>4__this;
		}

		// Token: 0x020007A5 RID: 1957
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskUserToAddNewItemAsync>d__8 : IAsyncStateMachine
		{
			// Token: 0x060042EB RID: 17131 RVA: 0x0033EC44 File Offset: 0x0033CE44
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Dash_CustomPage dash_CustomPage = this;
				try
				{
					TaskAwaiter<DashboardItemTypes?> taskAwaiter;
					TaskAwaiter<IPID> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<DashboardItemTypes?> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<DashboardItemTypes?>);
							num2 = -1;
							goto IL_0124;
						}
						if (!SharedSettings.Current.PIDSelectorWithValuePreview)
						{
							DashboardItemAdderPage dashboardItemAdderPage = new DashboardItemAdderPage(dash_CustomPage, position);
							App.GetCurrentPage().Navigation.PushAsync(dashboardItemAdderPage);
							goto IL_0178;
						}
						taskAwaiter3 = PIDSelector.SelectPIDAsync(null, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<IPID> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, Dash_CustomPage.<AskUserToAddNewItemAsync>d__8>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<IPID> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<IPID>);
						num2 = -1;
					}
					IPID result = taskAwaiter3.GetResult();
					pid = result;
					if (pid == null)
					{
						goto IL_014F;
					}
					CustomPID customPID = pid as CustomPID;
					if (customPID != null && customPID.IsAction)
					{
						dash_CustomPage.CreateAndAddItemFromPIDAndItemType(pid, DashboardItemTypes.Action, position);
						goto IL_014F;
					}
					taskAwaiter = DashboardItemTypeSelector.SelectDashboardItemType(pid).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<DashboardItemTypes?> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DashboardItemTypes?>, Dash_CustomPage.<AskUserToAddNewItemAsync>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_0124:
					DashboardItemTypes? result2 = taskAwaiter.GetResult();
					if (result2 != null)
					{
						dash_CustomPage.CreateAndAddItemFromPIDAndItemType(pid, result2.Value, position);
					}
					IL_014F:
					pid = null;
					IL_0178:;
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

			// Token: 0x060042EC RID: 17132 RVA: 0x0033EE14 File Offset: 0x0033D014
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002893 RID: 10387
			public int <>1__state;

			// Token: 0x04002894 RID: 10388
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002895 RID: 10389
			public Dash_CustomPage <>4__this;

			// Token: 0x04002896 RID: 10390
			public Point position;

			// Token: 0x04002897 RID: 10391
			private IPID <pid>5__2;

			// Token: 0x04002898 RID: 10392
			private TaskAwaiter<IPID> <>u__1;

			// Token: 0x04002899 RID: 10393
			private TaskAwaiter<DashboardItemTypes?> <>u__2;
		}
	}
}
