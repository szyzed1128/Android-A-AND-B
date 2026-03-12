using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200019C RID: 412
	public class HorizontalList : Grid
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06001646 RID: 5702 RVA: 0x0009FA28 File Offset: 0x0009DC28
		// (remove) Token: 0x06001647 RID: 5703 RVA: 0x0009FA60 File Offset: 0x0009DC60
		public event EventHandler SelectedItemChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.SelectedItemChanged;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.SelectedItemChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.SelectedItemChanged;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.SelectedItemChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06001648 RID: 5704 RVA: 0x0009FA95 File Offset: 0x0009DC95
		// (set) Token: 0x06001649 RID: 5705 RVA: 0x0009FA9D File Offset: 0x0009DC9D
		public StackOrientation ListOrientation
		{
			[CompilerGenerated]
			get
			{
				return this.<ListOrientation>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ListOrientation>k__BackingField = value;
			}
		}

		// Token: 0x17000F64 RID: 3940
		// (get) Token: 0x0600164A RID: 5706 RVA: 0x0009FAA6 File Offset: 0x0009DCA6
		// (set) Token: 0x0600164B RID: 5707 RVA: 0x0009FAAE File Offset: 0x0009DCAE
		public double Spacing
		{
			[CompilerGenerated]
			get
			{
				return this.<Spacing>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Spacing>k__BackingField = value;
			}
		}

		// Token: 0x17000F65 RID: 3941
		// (get) Token: 0x0600164C RID: 5708 RVA: 0x0009FAB7 File Offset: 0x0009DCB7
		// (set) Token: 0x0600164D RID: 5709 RVA: 0x0009FAC9 File Offset: 0x0009DCC9
		public ICommand SelectedCommand
		{
			get
			{
				return (ICommand)base.GetValue(HorizontalList.SelectedCommandProperty);
			}
			set
			{
				base.SetValue(HorizontalList.SelectedCommandProperty, value);
			}
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x0600164E RID: 5710 RVA: 0x0009FAD7 File Offset: 0x0009DCD7
		// (set) Token: 0x0600164F RID: 5711 RVA: 0x0009FAE9 File Offset: 0x0009DCE9
		public IEnumerable ItemsSource
		{
			get
			{
				return (IEnumerable)base.GetValue(HorizontalList.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(HorizontalList.ItemsSourceProperty, value);
			}
		}

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x0009FAF7 File Offset: 0x0009DCF7
		// (set) Token: 0x06001651 RID: 5713 RVA: 0x0009FB04 File Offset: 0x0009DD04
		public object SelectedItem
		{
			get
			{
				return base.GetValue(HorizontalList.SelectedItemProperty);
			}
			set
			{
				base.SetValue(HorizontalList.SelectedItemProperty, value);
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06001652 RID: 5714 RVA: 0x0009FB12 File Offset: 0x0009DD12
		// (set) Token: 0x06001653 RID: 5715 RVA: 0x0009FB24 File Offset: 0x0009DD24
		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(HorizontalList.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(HorizontalList.ItemTemplateProperty, value);
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0009FB32 File Offset: 0x0009DD32
		private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((HorizontalList)bindable).SetItems();
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0009FB40 File Offset: 0x0009DD40
		public HorizontalList()
		{
			this._scrollView = new ScrollView();
			this._itemsStackLayout = new StackLayout
			{
				BackgroundColor = base.BackgroundColor,
				Padding = base.Padding,
				Spacing = this.Spacing,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			this._scrollView.BackgroundColor = base.BackgroundColor;
			this._scrollView.Content = this._itemsStackLayout;
			base.Children.Add(this._scrollView);
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0009FBCC File Offset: 0x0009DDCC
		protected virtual void SetItems()
		{
			this._itemsStackLayout.Children.Clear();
			this._itemsStackLayout.Spacing = this.Spacing;
			this._innerSelectedCommand = new Command<View>(delegate(View view)
			{
				this.SelectedItem = view.BindingContext;
				this.SelectedItem = null;
			});
			this._itemsStackLayout.Orientation = this.ListOrientation;
			this._scrollView.Orientation = ((this.ListOrientation == 1) ? 1 : 0);
			if (this.ItemsSource == null)
			{
				return;
			}
			foreach (object obj in this.ItemsSource)
			{
				this._itemsStackLayout.Children.Add(this.GetItemView(obj));
			}
			this._itemsStackLayout.BackgroundColor = base.BackgroundColor;
			this.SelectedItem = null;
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0009FCB4 File Offset: 0x0009DEB4
		protected virtual View GetItemView(object item)
		{
			View view = this.ItemTemplate.CreateContent() as View;
			if (view == null)
			{
				return null;
			}
			view.BindingContext = item;
			TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer
			{
				Command = this._innerSelectedCommand,
				CommandParameter = view
			};
			this.AddGesture(view, tapGestureRecognizer);
			return view;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0009FD00 File Offset: 0x0009DF00
		private void AddGesture(View view, TapGestureRecognizer gesture)
		{
			view.GestureRecognizers.Add(gesture);
			Layout<View> layout = view as Layout<View>;
			if (layout == null)
			{
				return;
			}
			foreach (View view2 in layout.Children)
			{
				this.AddGesture(view2, gesture);
			}
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0009FD68 File Offset: 0x0009DF68
		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
		{
			HorizontalList horizontalList = (HorizontalList)bindable;
			if (newValue == oldValue && newValue != null)
			{
				return;
			}
			EventHandler selectedItemChanged = horizontalList.SelectedItemChanged;
			if (selectedItemChanged != null)
			{
				selectedItemChanged(horizontalList, EventArgs.Empty);
			}
			ICommand selectedCommand = horizontalList.SelectedCommand;
			if (selectedCommand != null && selectedCommand.CanExecute(newValue))
			{
				ICommand selectedCommand2 = horizontalList.SelectedCommand;
				if (selectedCommand2 == null)
				{
					return;
				}
				selectedCommand2.Execute(newValue);
			}
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x0009FDC4 File Offset: 0x0009DFC4
		// Note: this type is marked as 'beforefieldinit'.
		static HorizontalList()
		{
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0009FE8F File Offset: 0x0009E08F
		[CompilerGenerated]
		private void <SetItems>b__32_0(View view)
		{
			this.SelectedItem = view.BindingContext;
			this.SelectedItem = null;
		}

		// Token: 0x04000689 RID: 1673
		private ICommand _innerSelectedCommand;

		// Token: 0x0400068A RID: 1674
		private readonly ScrollView _scrollView;

		// Token: 0x0400068B RID: 1675
		private readonly StackLayout _itemsStackLayout;

		// Token: 0x0400068C RID: 1676
		[CompilerGenerated]
		private EventHandler SelectedItemChanged;

		// Token: 0x0400068D RID: 1677
		[CompilerGenerated]
		private StackOrientation <ListOrientation>k__BackingField;

		// Token: 0x0400068E RID: 1678
		[CompilerGenerated]
		private double <Spacing>k__BackingField;

		// Token: 0x0400068F RID: 1679
		public static readonly BindableProperty SelectedCommandProperty = BindableProperty.Create("SelectedCommand", typeof(ICommand), typeof(HorizontalList), null, 2, null, null, null, null, null);

		// Token: 0x04000690 RID: 1680
		public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(HorizontalList), null, 1, null, new BindableProperty.BindingPropertyChangedDelegate(HorizontalList.ItemsSourceChanged), null, null, null);

		// Token: 0x04000691 RID: 1681
		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(HorizontalList), null, 1, null, new BindableProperty.BindingPropertyChangedDelegate(HorizontalList.OnSelectedItemChanged), null, null, null);

		// Token: 0x04000692 RID: 1682
		public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(HorizontalList), null, 2, null, null, null, null, null);
	}
}
