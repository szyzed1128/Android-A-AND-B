using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005DA RID: 1498
	public static class PopupWithDontShowAgain
	{
		// Token: 0x060035B9 RID: 13753 RVA: 0x00269278 File Offset: 0x00267478
		public static void DisplayPopupWithDontShowAgain(string title, string text, string dontShowPropertyName, string acceptText = "OK", bool revertResult = false)
		{
			SfPopupLayout sfPopupLayout = new SfPopupLayout();
			sfPopupLayout.BackgroundColor = Color.White;
			sfPopupLayout.PopupView.AcceptButtonText = acceptText;
			sfPopupLayout.PopupView.AppearanceMode = 0;
			EventHandler<CheckedChangedEventArgs> <>9__1;
			sfPopupLayout.PopupView.ContentTemplate = new DataTemplate(delegate
			{
				Label label = new Label
				{
					Text = text,
					TextColor = Color.Black
				};
				CheckBox cb = new CheckBox
				{
					VerticalOptions = LayoutOptions.Center,
					Color = (Color)Application.Current.Resources["ButtonAccentColor"]
				};
				CheckBox cb2 = cb;
				EventHandler<CheckedChangedEventArgs> eventHandler;
				if ((eventHandler = <>9__1) == null)
				{
					eventHandler = (<>9__1 = delegate(object sender, CheckedChangedEventArgs e)
					{
						bool flag = e.Value;
						if (revertResult)
						{
							flag = !flag;
						}
						SharedSettings.Current.GetType().GetProperty(dontShowPropertyName).SetValue(SharedSettings.Current, flag);
					});
				}
				cb2.CheckedChanged += eventHandler;
				Label label2 = new Label
				{
					Text = Translate.GetString("hint_DontShowAgain"),
					VerticalTextAlignment = 1,
					TextColor = Color.Black
				};
				StackLayout stackLayout = new StackLayout
				{
					Orientation = 1
				};
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += delegate(object sender, EventArgs e)
				{
					cb.IsChecked = !cb.IsChecked;
				};
				label2.GestureRecognizers.Add(tapGestureRecognizer);
				stackLayout.Children.Add(cb);
				stackLayout.Children.Add(label2);
				ScrollView scrollView = new ScrollView();
				scrollView.Orientation = 0;
				StackLayout stackLayout2 = new StackLayout
				{
					Orientation = 0,
					Margin = new Thickness(5.0, 5.0)
				};
				scrollView.Content = stackLayout2;
				stackLayout2.Children.Add(label);
				stackLayout2.Children.Add(stackLayout);
				return scrollView;
			});
			sfPopupLayout.PopupView.HeaderTitle = title;
			sfPopupLayout.PopupView.AnimationMode = 0;
			sfPopupLayout.PopupView.ShowCloseButton = true;
			sfPopupLayout.PopupView.AutoSizeMode = 2;
			sfPopupLayout.ClosePopupOnBackButtonPressed = true;
			if (string.IsNullOrEmpty(title))
			{
				sfPopupLayout.PopupView.ShowHeader = false;
			}
			sfPopupLayout.Show(false);
		}

		// Token: 0x020005DB RID: 1499
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060035BA RID: 13754 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060035BB RID: 13755 RVA: 0x00269338 File Offset: 0x00267538
			internal object <DisplayPopupWithDontShowAgain>b__0()
			{
				PopupWithDontShowAgain.<>c__DisplayClass0_1 CS$<>8__locals1 = new PopupWithDontShowAgain.<>c__DisplayClass0_1();
				Label label = new Label
				{
					Text = this.text,
					TextColor = Color.Black
				};
				CS$<>8__locals1.cb = new CheckBox
				{
					VerticalOptions = LayoutOptions.Center,
					Color = (Color)Application.Current.Resources["ButtonAccentColor"]
				};
				CheckBox cb = CS$<>8__locals1.cb;
				EventHandler<CheckedChangedEventArgs> eventHandler;
				if ((eventHandler = this.<>9__1) == null)
				{
					eventHandler = (this.<>9__1 = delegate(object sender, CheckedChangedEventArgs e)
					{
						bool flag = e.Value;
						if (this.revertResult)
						{
							flag = !flag;
						}
						SharedSettings.Current.GetType().GetProperty(this.dontShowPropertyName).SetValue(SharedSettings.Current, flag);
					});
				}
				cb.CheckedChanged += eventHandler;
				Label label2 = new Label
				{
					Text = Translate.GetString("hint_DontShowAgain"),
					VerticalTextAlignment = 1,
					TextColor = Color.Black
				};
				StackLayout stackLayout = new StackLayout
				{
					Orientation = 1
				};
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += delegate(object sender, EventArgs e)
				{
					CS$<>8__locals1.cb.IsChecked = !CS$<>8__locals1.cb.IsChecked;
				};
				label2.GestureRecognizers.Add(tapGestureRecognizer);
				stackLayout.Children.Add(CS$<>8__locals1.cb);
				stackLayout.Children.Add(label2);
				ScrollView scrollView = new ScrollView();
				scrollView.Orientation = 0;
				StackLayout stackLayout2 = new StackLayout
				{
					Orientation = 0,
					Margin = new Thickness(5.0, 5.0)
				};
				scrollView.Content = stackLayout2;
				stackLayout2.Children.Add(label);
				stackLayout2.Children.Add(stackLayout);
				return scrollView;
			}

			// Token: 0x060035BC RID: 13756 RVA: 0x0026949C File Offset: 0x0026769C
			internal void <DisplayPopupWithDontShowAgain>b__1(object sender, CheckedChangedEventArgs e)
			{
				bool flag = e.Value;
				if (this.revertResult)
				{
					flag = !flag;
				}
				SharedSettings.Current.GetType().GetProperty(this.dontShowPropertyName).SetValue(SharedSettings.Current, flag);
			}

			// Token: 0x04002005 RID: 8197
			public string text;

			// Token: 0x04002006 RID: 8198
			public bool revertResult;

			// Token: 0x04002007 RID: 8199
			public string dontShowPropertyName;

			// Token: 0x04002008 RID: 8200
			public EventHandler<CheckedChangedEventArgs> <>9__1;
		}

		// Token: 0x020005DC RID: 1500
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_1
		{
			// Token: 0x060035BD RID: 13757 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_1()
			{
			}

			// Token: 0x060035BE RID: 13758 RVA: 0x002694E2 File Offset: 0x002676E2
			internal void <DisplayPopupWithDontShowAgain>b__2(object sender, EventArgs e)
			{
				this.cb.IsChecked = !this.cb.IsChecked;
			}

			// Token: 0x04002009 RID: 8201
			public CheckBox cb;
		}
	}
}
