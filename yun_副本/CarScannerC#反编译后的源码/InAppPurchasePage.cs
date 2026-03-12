using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000091 RID: 145
	[XamlFilePath("InApp\\InAppPurchasePage.xaml")]
	public class InAppPurchasePage : ContentPage
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00019544 File Offset: 0x00017744
		private string productLowPrice
		{
			get
			{
				if (PlatformHelper.IsiOS)
				{
					return "ovz.CarScanner.ProS6M";
				}
				if (PlatformHelper.IsAndroid)
				{
					return "ovz.carscanner.pro1";
				}
				throw new NotImplementedException("productId UnknownPlatform=" + Device.RuntimePlatform);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x00019574 File Offset: 0x00017774
		private string productMediumPrice
		{
			get
			{
				if (PlatformHelper.IsiOS)
				{
					return "ovz.CarScanner.ProL3";
				}
				if (PlatformHelper.IsAndroid)
				{
					return "ovz.carscanner.pro2";
				}
				throw new NotImplementedException("productId UnknownPlatform=" + Device.RuntimePlatform);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x000195A4 File Offset: 0x000177A4
		private string productHighPrice
		{
			get
			{
				if (PlatformHelper.IsiOS)
				{
					return "ovz.CarScanner.ProL4";
				}
				if (PlatformHelper.IsAndroid)
				{
					return "ovz.carscanner.pro3";
				}
				throw new NotImplementedException("productId UnknownPlatform=" + Device.RuntimePlatform);
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000195D4 File Offset: 0x000177D4
		public InAppPurchasePage()
		{
			this.InitializeComponent();
			this.contentStack.IsVisible = false;
			this.activityFrame.IsVisible = true;
			this.products = new List<IProduct>();
			this.manager = InAppManager.GetInstance();
			this.manager.Initialize();
			this.manager.OnProductsReceived -= this.OnProductsReceived;
			this.manager.OnProductsReceived += this.OnProductsReceived;
			this.manager.Error -= this.Manager_Error;
			this.manager.Error += this.Manager_Error;
			this.manager.ActionFinished -= this.Manager_ActionFinished;
			this.manager.ActionFinished += this.Manager_ActionFinished;
			SharedSettings.Current.PropertyChanged -= this.Settings_PropertyChanged;
			SharedSettings.Current.PropertyChanged += this.Settings_PropertyChanged;
			this.contentStack.Children.Remove(this.btnTest);
			this.btnTest.IsVisible = false;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001970A File Offset: 0x0001790A
		private void Manager_ActionFinished(object sender, EventArgs e)
		{
			this.contentStack.IsVisible = true;
			this.activityFrame.IsVisible = false;
			this.LastUserAction = InAppPurchasePage.UserActions.None;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001972C File Offset: 0x0001792C
		private async void Manager_Error(object sender, string ErrorMessage)
		{
			string text = Translate.GetString("ios_PurchaseSomethingWrongText") + "\r\n" + ErrorMessage;
			if (PlatformHelper.IsiOS)
			{
				text += Translate.GetString("ios_IAP_Restrictions");
			}
			else if (PlatformHelper.IsAndroid)
			{
				text += Translate.GetString("droid_IAP_Restore");
			}
			await base.DisplayAlert(Translate.GetString("ios_PurchaseSomethingWrongTitle"), text, "OK");
			this.contentStack.IsVisible = true;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001976C File Offset: 0x0001796C
		private async void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdsProductPurchased" || e.PropertyName == "LicenseFinishDate")
			{
				Device.BeginInvokeOnMainThread(async delegate
				{
					this.contentStack.IsVisible = true;
					this.activityFrame.IsVisible = false;
					if (SharedSettings.Current.AdsProductPurchased)
					{
						SharedSettings.Current.PropertyChanged -= this.Settings_PropertyChanged;
						try
						{
							if (App.GetCurrentPage() == this)
							{
								try
								{
									await base.DisplayAlert(Translate.GetString("ios_ThankYou"), Translate.GetString("RMA_TEXT"), "OK");
									await this.RequestRating();
									await base.Navigation.PopAsync();
								}
								catch
								{
								}
							}
						}
						catch (Exception)
						{
						}
					}
				});
			}
		}

		// Token: 0x060002EE RID: 750 RVA: 0x000197AC File Offset: 0x000179AC
		private async Task RequestRating()
		{
			string text = "";
			if (PlatformHelper.IsAndroid)
			{
				text = "Google Play";
			}
			else if (PlatformHelper.IsiOS)
			{
				text = "App Store";
			}
			string text2 = string.Format(Translate.GetString("ios_AboutRatingsTitle"), text);
			bool flag = !this.rating_requested;
			if (flag)
			{
				bool flag2 = Device.RuntimePlatform == "iOS";
				if (!flag2)
				{
					flag2 = await base.DisplayAlert(text2, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content"));
				}
				flag = flag2;
			}
			if (flag)
			{
				this.rating_requested = true;
				DependencyService.Get<IRequestReview>(0).Request(false);
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000197F0 File Offset: 0x000179F0
		private void btnBack_Clicked(object sender, EventArgs e)
		{
			this.manager.OnProductsReceived -= this.OnProductsReceived;
			SharedSettings.Current.PropertyChanged -= this.Settings_PropertyChanged;
			this.manager.ActionFinished -= this.Manager_ActionFinished;
			this.manager.Error -= this.Manager_Error;
			this.manager.FreeResources();
			base.Navigation.PopAsync();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00019870 File Offset: 0x00017A70
		private void CreatePurchaseButtons(List<IProduct> _products)
		{
			this.panelNewStylePurchaseButtons.Children.Clear();
			foreach (IProduct product in _products)
			{
				Button button = new Button
				{
					Style = (Style)base.Resources["PurchaseButtonStyle"],
					ClassId = product.ProductID
				};
				string productID = product.ProductID;
				if (productID != null)
				{
					int length = productID.Length;
					if (length != 20)
					{
						if (length == 21)
						{
							char c = productID[19];
							if (c != '1')
							{
								if (c != '3')
								{
									if (c == '6')
									{
										if (productID == "ovz.CarScanner.ProS6M")
										{
											button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle6m");
										}
									}
								}
								else if (productID == "ovz.CarScanner.ProS3M")
								{
									button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle6m");
								}
							}
							else if (!(productID == "ovz.CarScanner.ProS1M"))
							{
								if (productID == "ovz.CarScanner.ProS1Y")
								{
									button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle1y");
								}
							}
							else
							{
								button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle1m");
							}
						}
					}
					else
					{
						switch (productID[19])
						{
						case '2':
							if (productID == "ovz.CarScanner.ProL2")
							{
								button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
							}
							break;
						case '3':
							if (productID == "ovz.CarScanner.ProL3")
							{
								button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
							}
							break;
						case '4':
							if (productID == "ovz.CarScanner.ProL4")
							{
								button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
							}
							break;
						}
					}
				}
				button.Clicked += this.PurchaseBtn_Clicked;
				this.panelNewStylePurchaseButtons.Children.Add(button);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00019B10 File Offset: 0x00017D10
		private void PurchaseBtn_Clicked(object sender, EventArgs e)
		{
			string classId = ((Button)sender).ClassId;
			this.PurchaseWithProduct(classId);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00019B30 File Offset: 0x00017D30
		private async void OnProductsReceived(List<IProduct> _products, string DebugString)
		{
			if (_products.Count > 0)
			{
				this.products.Clear();
				using (List<IProduct>.Enumerator enumerator = _products.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IProduct p = enumerator.Current;
						if (this.product_ids.Any((string x) => x == p.ProductID))
						{
							this.products.Add(p);
						}
					}
				}
				if (PlatformHelper.IsiOS)
				{
					this.CreatePurchaseButtons(_products);
				}
				this.productSlider.Value = 1.0;
				this.productSlider_ValueChanged(this.productSlider, new ValueChangedEventArgs(0.0, 1.0));
				this.WasLoaded = true;
				this.contentStack.IsVisible = true;
				this.activityFrame.IsVisible = false;
				if (this.RestoreOnAppear)
				{
					this.RestoreOnAppear = false;
					this.btnRestore_Clicked(this.btnRestore, EventArgs.Empty);
				}
			}
			else
			{
				string text = Translate.GetString("ios_PurchaseCantGetProductsTitle");
				string text2 = "";
				if (PlatformHelper.IsiOS)
				{
					text2 = "App Store";
				}
				else if (PlatformHelper.IsAndroid)
				{
					text2 = "Google Play";
				}
				text = string.Format(text, text2);
				this.WasLoaded = false;
				await base.DisplayAlert(text, Translate.GetString("ios_PurchaseCantGetProductsText") + "\n" + DebugString, "OK");
				await base.Navigation.PopAsync();
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00019B77 File Offset: 0x00017D77
		private void Handle_Appearing(object sender, EventArgs e)
		{
			if (!this.WasLoaded)
			{
				this.RequestProducts();
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00019B88 File Offset: 0x00017D88
		private async Task RequestProducts()
		{
			this.product_ids.Clear();
			await Task.Run(async delegate
			{
				string[] array = new string[] { "https://node4.carscanner.info", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
				for (int i = 0; i < array.Length; i++)
				{
					string text = "";
					switch (PlatformHelper.AppMarket)
					{
					case Markets.AppStore:
						text = "iosv2.json";
						break;
					case Markets.GooglePlay:
						text = "droidv2.json";
						break;
					case Markets.HMS:
						text = "hms.json";
						break;
					}
					array[i] += text;
				}
				string text2 = await HttpDownloader.Get(array, 7, null, true);
				if (!string.IsNullOrEmpty(text2))
				{
					this.product_ids = JsonConvert.DeserializeObject<List<string>>(text2);
				}
			});
			this.manager.RequestProductData(this.product_ids);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00019BCC File Offset: 0x00017DCC
		private async void btnBuy_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = this.manager.CanMakePayments().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				string text = "";
				if (PlatformHelper.IsiOS)
				{
					text = "AppStore";
				}
				else if (PlatformHelper.IsAndroid)
				{
					text = "Google Play";
				}
				await base.DisplayAlert(string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text), PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK");
			}
			else
			{
				this.LastUserAction = InAppPurchasePage.UserActions.StartPurchase;
				this.contentStack.IsVisible = false;
				this.activityFrame.IsVisible = true;
				IProduct product = this.products[(int)this.productSlider.Value];
				this.manager.Purchase(product.ProductID);
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00019C04 File Offset: 0x00017E04
		private async void btnRestore_Clicked(object sender, EventArgs e)
		{
			this.contentStack.IsVisible = false;
			this.activityFrame.IsVisible = true;
			TaskAwaiter<bool> taskAwaiter = this.manager.CanMakePayments().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				string text = "";
				if (PlatformHelper.IsiOS)
				{
					text = "AppStore";
				}
				else if (PlatformHelper.IsAndroid)
				{
					text = "Google Play";
				}
				await base.DisplayAlert(string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text), PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK");
			}
			else
			{
				this.LastUserAction = InAppPurchasePage.UserActions.RestorePurchases;
				this.manager.Restore();
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00019C3B File Offset: 0x00017E3B
		private void SetPurchased()
		{
			SharedSettings.Current.AdsProductPurchased = true;
			LiveDataPIDModel.SetShouldHideForSelectedPids();
			this.btnBuy.IsVisible = false;
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00019C59 File Offset: 0x00017E59
		// (set) Token: 0x060002FA RID: 762 RVA: 0x00019C61 File Offset: 0x00017E61
		public bool RestoreOnAppear
		{
			get
			{
				return this._RestoreOnAppear;
			}
			set
			{
				this._RestoreOnAppear = value;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00019C6A File Offset: 0x00017E6A
		private void btnTest_Clicked(object sender, EventArgs e)
		{
			this.SetPurchased();
			this.RequestRating();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00019C7C File Offset: 0x00017E7C
		private void productSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			int num = (int)Math.Round(e.NewValue);
			if (this.products != null && this.products.Count > 0 && num < this.products.Count)
			{
				this.tbTitle.Text = this.products[num].ProductName;
				this.tbDescription.Text = this.products[num].ProductDescription;
				string text = Translate.GetString("ios_Buy") + " (" + this.products[num].LocalizedPriceString + ")";
				this.btnBuy.Text = text;
				this.btnBuy.IsVisible = true;
			}
			this.productSlider.Value = (double)num;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00019D48 File Offset: 0x00017F48
		private async void PurchaseWithProduct(string productId)
		{
			TaskAwaiter<bool> taskAwaiter = this.manager.CanMakePayments().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				string text = "";
				if (PlatformHelper.IsiOS)
				{
					text = "AppStore";
				}
				else if (PlatformHelper.IsAndroid)
				{
					text = "Google Play";
				}
				await base.DisplayAlert(string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text), PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK");
			}
			else
			{
				this.LastUserAction = InAppPurchasePage.UserActions.StartPurchase;
				this.contentStack.IsVisible = false;
				this.activityFrame.IsVisible = true;
				this.manager.Purchase(productId);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00019D88 File Offset: 0x00017F88
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(InAppPurchasePage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "InApp/InAppPurchasePage.xaml",
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
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 52);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Button)), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 25);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 34);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 34);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 26);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 26);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 29);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 26);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 29);
			Slider slider;
			VisualDiagnostics.RegisterSourceInfo(slider = new Slider(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 26);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 29);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 30);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 32);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 74);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 26);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 29);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 29);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 26);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 29);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 29);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 22);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 22);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("InApp\\InAppPurchasePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			nameScope.RegisterName("contentStack", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "contentStack";
			}
			nameScope.RegisterName("stackOldStyle", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "stackOldStyle";
			}
			nameScope.RegisterName("tbTitle", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "tbTitle";
			}
			nameScope.RegisterName("tbDescription", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "tbDescription";
			}
			nameScope.RegisterName("tbChoosePrice", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "tbChoosePrice";
			}
			nameScope.RegisterName("productSlider", slider);
			if (slider.StyleId == null)
			{
				slider.StyleId = "productSlider";
			}
			nameScope.RegisterName("btnBuy", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnBuy";
			}
			nameScope.RegisterName("btnRestore", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnRestore";
			}
			nameScope.RegisterName("stackNewStyle", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "stackNewStyle";
			}
			nameScope.RegisterName("panelNewStylePurchaseButtons", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelNewStylePurchaseButtons";
			}
			nameScope.RegisterName("btnRestore2", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnRestore2";
			}
			nameScope.RegisterName("btnTest", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnTest";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.contentStack = stackLayout4;
			this.stackOldStyle = stackLayout;
			this.tbTitle = label2;
			this.tbDescription = label3;
			this.tbChoosePrice = label4;
			this.productSlider = slider;
			this.btnBuy = button;
			this.btnRestore = button2;
			this.stackNewStyle = stackLayout3;
			this.panelNewStylePurchaseButtons = stackLayout2;
			this.btnRestore2 = button3;
			this.btnTest = button4;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			setter.Property = Button.TextColorProperty;
			setter.Value = "White";
			setter.Value = Color.White;
			style.Setters.Add(setter);
			setter2.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = setter2;
			array[1] = style;
			array[2] = resourceDictionary;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(Setter).GetRuntimeProperty("Value"), nameScope3));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(21, 52)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			setter2.Value = dynamicResource;
			style.Setters.Add(setter2);
			setter3.Property = View.MarginProperty;
			setter3.Value = "0";
			setter3.Value = new Thickness(0.0);
			style.Setters.Add(setter3);
			setter4.Property = View.HorizontalOptionsProperty;
			setter4.Value = "FillAndExpand";
			setter4.Value = LayoutOptions.FillAndExpand;
			style.Setters.Add(setter4);
			resourceDictionary.Add("PurchaseButtonStyle", style);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension3.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension4 = translate;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension4.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			nonScalableLabel.SetValue(Label.TextProperty, "Car Scanner Pro");
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(nonScalableLabel);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(View.MarginProperty, new Thickness(10.0, 0.0, 10.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(Grid.RowProperty, 0);
			image.SetValue(Image.AspectProperty, 0);
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension5.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = image;
			array6[1] = stackLayout4;
			array6[2] = scrollView;
			array6[3] = grid2;
			array6[4] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Image.SourceProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 25)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource5.Key);
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout4.Children.Add(image);
			label.SetValue(Grid.RowProperty, 1);
			label.SetValue(Label.LineBreakModeProperty, 1);
			translate2.Text = "ios_CarScannerProAdvantages";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = label;
			array7[1] = stackLayout4;
			array7[2] = scrollView;
			array7[3] = grid2;
			array7[4] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 25)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label.Text = obj9;
			stackLayout4.Children.Add(label);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform.Platforms.Add(on2);
			stackLayout.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			label2.SetValue(Grid.RowProperty, 2);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label2.SetValue(Label.LineBreakModeProperty, 1);
			stackLayout.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 3);
			label3.SetValue(Label.LineBreakModeProperty, 1);
			stackLayout.Children.Add(label3);
			label4.SetValue(Grid.RowProperty, 4);
			label4.SetValue(Label.LineBreakModeProperty, 1);
			translate3.Text = "ios_ChoosePrice";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label4;
			array8[1] = stackLayout;
			array8[2] = stackLayout4;
			array8[3] = scrollView;
			array8[4] = grid2;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 29)));
			object obj11 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.Text = obj11;
			stackLayout.Children.Add(label4);
			slider.SetValue(Grid.RowProperty, 5);
			slider.SetValue(View.MarginProperty, new Thickness(10.0, 0.0, 10.0, 0.0));
			slider.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			slider.SetValue(Slider.MaximumProperty, 2.0);
			slider.SetValue(Slider.MinimumProperty, 0.0);
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = slider;
			array9[1] = stackLayout;
			array9[2] = stackLayout4;
			array9[3] = scrollView;
			array9[4] = grid2;
			array9[5] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, Slider.ThumbColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 29)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			slider.SetDynamicResource(Slider.ThumbColorProperty, dynamicResource6.Key);
			slider.ValueChanged += this.productSlider_ValueChanged;
			stackLayout.Children.Add(slider);
			button.SetValue(Grid.RowProperty, 6);
			button.Clicked += this.btnBuy_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			stackLayout.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 7);
			button2.Clicked += this.btnRestore_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate4.Text = "ios_RestorePurchases";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = button2;
			array10[1] = stackLayout;
			array10[2] = stackLayout4;
			array10[3] = scrollView;
			array10[4] = grid2;
			array10[5] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 29)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button2.Text = obj14;
			stackLayout.Children.Add(button2);
			stackLayout4.Children.Add(stackLayout);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "true";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "false";
			onPlatform2.Platforms.Add(on4);
			stackLayout3.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label5;
			array11[1] = stackLayout3;
			array11[2] = stackLayout4;
			array11[3] = scrollView;
			array11[4] = grid2;
			array11[5] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, Label.FontSizeProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 32)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			translate5.Text = "ios_ChoosePriceWithSubscriptions";
			IMarkupExtension markupExtension12 = translate5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label5;
			array12[1] = stackLayout3;
			array12[2] = stackLayout4;
			array12[3] = scrollView;
			array12[4] = grid2;
			array12[5] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 74)));
			object obj17 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label5.Text = obj17;
			stackLayout3.Children.Add(label5);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout3.Children.Add(stackLayout2);
			button3.SetValue(Grid.RowProperty, 7);
			dynamicResourceExtension8.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = button3;
			array13[1] = stackLayout3;
			array13[2] = stackLayout4;
			array13[3] = scrollView;
			array13[4] = grid2;
			array13[5] = this;
			object obj18;
			xamlServiceProvider13.Add(typeFromHandle25, obj18 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 29)));
			DynamicResource dynamicResource8 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			button3.Clicked += this.btnRestore_Clicked;
			button3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			translate6.Text = "ios_RestorePurchases";
			IMarkupExtension markupExtension14 = translate6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = button3;
			array14[1] = stackLayout3;
			array14[2] = stackLayout4;
			array14[3] = scrollView;
			array14[4] = grid2;
			array14[5] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, Button.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 29)));
			object obj20 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button3.Text = obj20;
			button3.SetValue(Button.TextColorProperty, Color.White);
			stackLayout3.Children.Add(button3);
			dynamicResourceExtension9.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = label6;
			array15[1] = stackLayout3;
			array15[2] = stackLayout4;
			array15[3] = scrollView;
			array15[4] = grid2;
			array15[5] = this;
			object obj21;
			xamlServiceProvider15.Add(typeFromHandle29, obj21 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 29)));
			DynamicResource dynamicResource9 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource9.Key);
			label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate7.Text = "ios_SubscriptionHint";
			IMarkupExtension markupExtension16 = translate7;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = label6;
			array16[1] = stackLayout3;
			array16[2] = stackLayout4;
			array16[3] = scrollView;
			array16[4] = grid2;
			array16[5] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(InAppPurchasePage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 29)));
			object obj23 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label6.Text = obj23;
			stackLayout3.Children.Add(label6);
			stackLayout4.Children.Add(stackLayout3);
			button4.SetValue(Grid.RowProperty, 8);
			button4.Clicked += this.btnTest_Clicked;
			button4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			button4.SetValue(Button.TextProperty, "Test purchase");
			stackLayout4.Children.Add(button4);
			scrollView.Content = stackLayout4;
			grid2.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
		[CompilerGenerated]
		private async void <Settings_PropertyChanged>b__11_0()
		{
			this.contentStack.IsVisible = true;
			this.activityFrame.IsVisible = false;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				SharedSettings.Current.PropertyChanged -= this.Settings_PropertyChanged;
				try
				{
					if (App.GetCurrentPage() == this)
					{
						try
						{
							await base.DisplayAlert(Translate.GetString("ios_ThankYou"), Translate.GetString("RMA_TEXT"), "OK");
							await this.RequestRating();
							await base.Navigation.PopAsync();
						}
						catch
						{
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0001C610 File Offset: 0x0001A810
		[CompilerGenerated]
		private async Task <RequestProducts>b__22_0()
		{
			string[] array = new string[] { "https://node4.carscanner.info", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
			for (int i = 0; i < array.Length; i++)
			{
				string text = "";
				switch (PlatformHelper.AppMarket)
				{
				case Markets.AppStore:
					text = "iosv2.json";
					break;
				case Markets.GooglePlay:
					text = "droidv2.json";
					break;
				case Markets.HMS:
					text = "hms.json";
					break;
				}
				array[i] += text;
			}
			string text2 = await HttpDownloader.Get(array, 7, null, true);
			if (!string.IsNullOrEmpty(text2))
			{
				this.product_ids = JsonConvert.DeserializeObject<List<string>>(text2);
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0001C654 File Offset: 0x0001A854
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<InAppPurchasePage>(this, typeof(InAppPurchasePage));
			this.contentStack = NameScopeExtensions.FindByName<StackLayout>(this, "contentStack");
			this.stackOldStyle = NameScopeExtensions.FindByName<StackLayout>(this, "stackOldStyle");
			this.tbTitle = NameScopeExtensions.FindByName<Label>(this, "tbTitle");
			this.tbDescription = NameScopeExtensions.FindByName<Label>(this, "tbDescription");
			this.tbChoosePrice = NameScopeExtensions.FindByName<Label>(this, "tbChoosePrice");
			this.productSlider = NameScopeExtensions.FindByName<Slider>(this, "productSlider");
			this.btnBuy = NameScopeExtensions.FindByName<Button>(this, "btnBuy");
			this.btnRestore = NameScopeExtensions.FindByName<Button>(this, "btnRestore");
			this.stackNewStyle = NameScopeExtensions.FindByName<StackLayout>(this, "stackNewStyle");
			this.panelNewStylePurchaseButtons = NameScopeExtensions.FindByName<StackLayout>(this, "panelNewStylePurchaseButtons");
			this.btnRestore2 = NameScopeExtensions.FindByName<Button>(this, "btnRestore2");
			this.btnTest = NameScopeExtensions.FindByName<Button>(this, "btnTest");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x040001BB RID: 443
		private InAppPurchasePage.UserActions LastUserAction;

		// Token: 0x040001BC RID: 444
		private bool rating_requested;

		// Token: 0x040001BD RID: 445
		private ICustomInAppManager manager;

		// Token: 0x040001BE RID: 446
		private bool WasLoaded;

		// Token: 0x040001BF RID: 447
		public List<string> product_ids = new List<string>();

		// Token: 0x040001C0 RID: 448
		public List<IProduct> products;

		// Token: 0x040001C1 RID: 449
		private bool _RestoreOnAppear;

		// Token: 0x040001C2 RID: 450
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout contentStack;

		// Token: 0x040001C3 RID: 451
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackOldStyle;

		// Token: 0x040001C4 RID: 452
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbTitle;

		// Token: 0x040001C5 RID: 453
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbDescription;

		// Token: 0x040001C6 RID: 454
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbChoosePrice;

		// Token: 0x040001C7 RID: 455
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Slider productSlider;

		// Token: 0x040001C8 RID: 456
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBuy;

		// Token: 0x040001C9 RID: 457
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore;

		// Token: 0x040001CA RID: 458
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackNewStyle;

		// Token: 0x040001CB RID: 459
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNewStylePurchaseButtons;

		// Token: 0x040001CC RID: 460
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore2;

		// Token: 0x040001CD RID: 461
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnTest;

		// Token: 0x040001CE RID: 462
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000092 RID: 146
		private enum UserActions
		{
			// Token: 0x040001D0 RID: 464
			None,
			// Token: 0x040001D1 RID: 465
			RequestProducts,
			// Token: 0x040001D2 RID: 466
			StartPurchase,
			// Token: 0x040001D3 RID: 467
			RestorePurchases
		}

		// Token: 0x02000093 RID: 147
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<RequestProducts>b__22_0>d : IAsyncStateMachine
		{
			// Token: 0x06000302 RID: 770 RVA: 0x0001C750 File Offset: 0x0001A950
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						string[] array = new string[] { "https://node4.carscanner.info", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
						for (int i = 0; i < array.Length; i++)
						{
							string text = "";
							switch (PlatformHelper.AppMarket)
							{
							case Markets.AppStore:
								text = "iosv2.json";
								break;
							case Markets.GooglePlay:
								text = "droidv2.json";
								break;
							case Markets.HMS:
								text = "hms.json";
								break;
							}
							array[i] += text;
						}
						taskAwaiter = HttpDownloader.Get(array, 7, null, true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePage.<<RequestProducts>b__22_0>d>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					if (!string.IsNullOrEmpty(result))
					{
						inAppPurchasePage.product_ids = JsonConvert.DeserializeObject<List<string>>(result);
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

			// Token: 0x06000303 RID: 771 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001D4 RID: 468
			public int <>1__state;

			// Token: 0x040001D5 RID: 469
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040001D6 RID: 470
			public InAppPurchasePage <>4__this;

			// Token: 0x040001D7 RID: 471
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000094 RID: 148
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<Settings_PropertyChanged>b__11_0>d : IAsyncStateMachine
		{
			// Token: 0x06000304 RID: 772 RVA: 0x0001C8B0 File Offset: 0x0001AAB0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					if (num > 2)
					{
						inAppPurchasePage.contentStack.IsVisible = true;
						inAppPurchasePage.activityFrame.IsVisible = false;
						if (!SharedSettings.Current.AdsProductPurchased)
						{
							goto IL_01A9;
						}
						SharedSettings.Current.PropertyChanged -= inAppPurchasePage.Settings_PropertyChanged;
					}
					try
					{
						if (num <= 2 || App.GetCurrentPage() == inAppPurchasePage)
						{
							try
							{
								TaskAwaiter taskAwaiter;
								TaskAwaiter<Page> taskAwaiter3;
								switch (num)
								{
								case 0:
								{
									TaskAwaiter taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num2 = -1;
									break;
								}
								case 1:
								{
									TaskAwaiter taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num2 = -1;
									goto IL_013A;
								}
								case 2:
								{
									TaskAwaiter<Page> taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter<Page>);
									num2 = -1;
									goto IL_0197;
								}
								default:
									taskAwaiter = inAppPurchasePage.DisplayAlert(Translate.GetString("ios_ThankYou"), Translate.GetString("RMA_TEXT"), "OK").GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num2 = 0;
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<<Settings_PropertyChanged>b__11_0>d>(ref taskAwaiter, ref this);
										return;
									}
									break;
								}
								taskAwaiter.GetResult();
								taskAwaiter = inAppPurchasePage.RequestRating().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<<Settings_PropertyChanged>b__11_0>d>(ref taskAwaiter, ref this);
									return;
								}
								IL_013A:
								taskAwaiter.GetResult();
								taskAwaiter3 = inAppPurchasePage.Navigation.PopAsync().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InAppPurchasePage.<<Settings_PropertyChanged>b__11_0>d>(ref taskAwaiter3, ref this);
									return;
								}
								IL_0197:
								taskAwaiter3.GetResult();
							}
							catch
							{
							}
						}
					}
					catch (Exception)
					{
					}
					IL_01A9:;
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

			// Token: 0x06000305 RID: 773 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001D8 RID: 472
			public int <>1__state;

			// Token: 0x040001D9 RID: 473
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001DA RID: 474
			public InAppPurchasePage <>4__this;

			// Token: 0x040001DB RID: 475
			private TaskAwaiter <>u__1;

			// Token: 0x040001DC RID: 476
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x02000095 RID: 149
		[CompilerGenerated]
		private sealed class <>c__DisplayClass20_0
		{
			// Token: 0x06000306 RID: 774 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass20_0()
			{
			}

			// Token: 0x06000307 RID: 775 RVA: 0x0001CAEE File Offset: 0x0001ACEE
			internal bool <OnProductsReceived>b__0(string x)
			{
				return x == this.p.ProductID;
			}

			// Token: 0x040001DD RID: 477
			public IProduct p;
		}

		// Token: 0x02000096 RID: 150
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Manager_Error>d__10 : IAsyncStateMachine
		{
			// Token: 0x06000308 RID: 776 RVA: 0x0001CB04 File Offset: 0x0001AD04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						string text = Translate.GetString("ios_PurchaseSomethingWrongText") + "\r\n" + ErrorMessage;
						if (PlatformHelper.IsiOS)
						{
							text += Translate.GetString("ios_IAP_Restrictions");
						}
						else if (PlatformHelper.IsAndroid)
						{
							text += Translate.GetString("droid_IAP_Restore");
						}
						taskAwaiter = inAppPurchasePage.DisplayAlert(Translate.GetString("ios_PurchaseSomethingWrongTitle"), text, "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<Manager_Error>d__10>(ref taskAwaiter, ref this);
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
					inAppPurchasePage.contentStack.IsVisible = true;
					inAppPurchasePage.activityFrame.IsVisible = false;
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

			// Token: 0x06000309 RID: 777 RVA: 0x0001CC30 File Offset: 0x0001AE30
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001DE RID: 478
			public int <>1__state;

			// Token: 0x040001DF RID: 479
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001E0 RID: 480
			public string ErrorMessage;

			// Token: 0x040001E1 RID: 481
			public InAppPurchasePage <>4__this;

			// Token: 0x040001E2 RID: 482
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000097 RID: 151
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnProductsReceived>d__20 : IAsyncStateMachine
		{
			// Token: 0x0600030A RID: 778 RVA: 0x0001CC40 File Offset: 0x0001AE40
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num = (num2 = -1);
							goto IL_024B;
						}
						if (_products.Count > 0)
						{
							inAppPurchasePage.products.Clear();
							List<IProduct>.Enumerator enumerator = _products.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									InAppPurchasePage.<>c__DisplayClass20_0 CS$<>8__locals1 = new InAppPurchasePage.<>c__DisplayClass20_0();
									CS$<>8__locals1.p = enumerator.Current;
									if (inAppPurchasePage.product_ids.Any((string x) => x == CS$<>8__locals1.p.ProductID))
									{
										inAppPurchasePage.products.Add(CS$<>8__locals1.p);
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							if (PlatformHelper.IsiOS)
							{
								inAppPurchasePage.CreatePurchaseButtons(_products);
							}
							inAppPurchasePage.productSlider.Value = 1.0;
							inAppPurchasePage.productSlider_ValueChanged(inAppPurchasePage.productSlider, new ValueChangedEventArgs(0.0, 1.0));
							inAppPurchasePage.WasLoaded = true;
							inAppPurchasePage.contentStack.IsVisible = true;
							inAppPurchasePage.activityFrame.IsVisible = false;
							if (inAppPurchasePage.RestoreOnAppear)
							{
								inAppPurchasePage.RestoreOnAppear = false;
								inAppPurchasePage.btnRestore_Clicked(inAppPurchasePage.btnRestore, EventArgs.Empty);
								goto IL_0253;
							}
							goto IL_0253;
						}
						else
						{
							string text = Translate.GetString("ios_PurchaseCantGetProductsTitle");
							string text2 = "";
							if (PlatformHelper.IsiOS)
							{
								text2 = "App Store";
							}
							else if (PlatformHelper.IsAndroid)
							{
								text2 = "Google Play";
							}
							text = string.Format(text, text2);
							inAppPurchasePage.WasLoaded = false;
							taskAwaiter3 = inAppPurchasePage.DisplayAlert(text, Translate.GetString("ios_PurchaseCantGetProductsText") + "\n" + DebugString, "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<OnProductsReceived>d__20>(ref taskAwaiter3, ref this);
								return;
							}
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter3.GetResult();
					taskAwaiter = inAppPurchasePage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InAppPurchasePage.<OnProductsReceived>d__20>(ref taskAwaiter, ref this);
						return;
					}
					IL_024B:
					taskAwaiter.GetResult();
					IL_0253:;
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

			// Token: 0x0600030B RID: 779 RVA: 0x0001CF04 File Offset: 0x0001B104
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001E3 RID: 483
			public int <>1__state;

			// Token: 0x040001E4 RID: 484
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001E5 RID: 485
			public List<IProduct> _products;

			// Token: 0x040001E6 RID: 486
			public InAppPurchasePage <>4__this;

			// Token: 0x040001E7 RID: 487
			public string DebugString;

			// Token: 0x040001E8 RID: 488
			private TaskAwaiter <>u__1;

			// Token: 0x040001E9 RID: 489
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x02000098 RID: 152
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PurchaseWithProduct>d__34 : IAsyncStateMachine
		{
			// Token: 0x0600030C RID: 780 RVA: 0x0001CF14 File Offset: 0x0001B114
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0140;
						}
						taskAwaiter5 = inAppPurchasePage.manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePage.<PurchaseWithProduct>d__34>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter5.GetResult())
					{
						inAppPurchasePage.LastUserAction = InAppPurchasePage.UserActions.StartPurchase;
						inAppPurchasePage.contentStack.IsVisible = false;
						inAppPurchasePage.activityFrame.IsVisible = true;
						inAppPurchasePage.manager.Purchase(productId);
						goto IL_0194;
					}
					string text = "";
					if (PlatformHelper.IsiOS)
					{
						text = "AppStore";
					}
					else if (PlatformHelper.IsAndroid)
					{
						text = "Google Play";
					}
					string text2 = string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text);
					taskAwaiter3 = inAppPurchasePage.DisplayAlert(text2, PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<PurchaseWithProduct>d__34>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0140:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0194:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600030D RID: 781 RVA: 0x0001D0E4 File Offset: 0x0001B2E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001EA RID: 490
			public int <>1__state;

			// Token: 0x040001EB RID: 491
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001EC RID: 492
			public InAppPurchasePage <>4__this;

			// Token: 0x040001ED RID: 493
			public string productId;

			// Token: 0x040001EE RID: 494
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040001EF RID: 495
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000099 RID: 153
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestProducts>d__22 : IAsyncStateMachine
		{
			// Token: 0x0600030E RID: 782 RVA: 0x0001D0F4 File Offset: 0x0001B2F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						inAppPurchasePage.product_ids.Clear();
						taskAwaiter = Task.Run(delegate
						{
							InAppPurchasePage.<<RequestProducts>b__22_0>d <<RequestProducts>b__22_0>d;
							<<RequestProducts>b__22_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<RequestProducts>b__22_0>d.<>4__this = inAppPurchasePage;
							<<RequestProducts>b__22_0>d.<>1__state = -1;
							<<RequestProducts>b__22_0>d.<>t__builder.Start<InAppPurchasePage.<<RequestProducts>b__22_0>d>(ref <<RequestProducts>b__22_0>d);
							return <<RequestProducts>b__22_0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<RequestProducts>d__22>(ref taskAwaiter, ref this);
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
					inAppPurchasePage.manager.RequestProductData(inAppPurchasePage.product_ids);
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

			// Token: 0x0600030F RID: 783 RVA: 0x0001D1D0 File Offset: 0x0001B3D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001F0 RID: 496
			public int <>1__state;

			// Token: 0x040001F1 RID: 497
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040001F2 RID: 498
			public InAppPurchasePage <>4__this;

			// Token: 0x040001F3 RID: 499
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200009A RID: 154
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestRating>d__13 : IAsyncStateMachine
		{
			// Token: 0x06000310 RID: 784 RVA: 0x0001D1E0 File Offset: 0x0001B3E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					bool flag;
					bool flag2;
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						string text = "";
						if (PlatformHelper.IsAndroid)
						{
							text = "Google Play";
						}
						else if (PlatformHelper.IsiOS)
						{
							text = "App Store";
						}
						string text2 = string.Format(Translate.GetString("ios_AboutRatingsTitle"), text);
						flag = !inAppPurchasePage.rating_requested;
						if (!flag)
						{
							goto IL_00E9;
						}
						flag2 = Device.RuntimePlatform == "iOS";
						if (flag2)
						{
							goto IL_00E5;
						}
						taskAwaiter = inAppPurchasePage.DisplayAlert(text2, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePage.<RequestRating>d__13>(ref taskAwaiter, ref this);
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
					flag2 = taskAwaiter.GetResult();
					IL_00E5:
					flag = flag2;
					IL_00E9:
					if (flag)
					{
						inAppPurchasePage.rating_requested = true;
						DependencyService.Get<IRequestReview>(0).Request(false);
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

			// Token: 0x06000311 RID: 785 RVA: 0x0001D32C File Offset: 0x0001B52C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001F4 RID: 500
			public int <>1__state;

			// Token: 0x040001F5 RID: 501
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040001F6 RID: 502
			public InAppPurchasePage <>4__this;

			// Token: 0x040001F7 RID: 503
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200009B RID: 155
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Settings_PropertyChanged>d__11 : IAsyncStateMachine
		{
			// Token: 0x06000312 RID: 786 RVA: 0x0001D33C File Offset: 0x0001B53C
			void IAsyncStateMachine.MoveNext()
			{
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					if (e.PropertyName == "AdsProductPurchased" || e.PropertyName == "LicenseFinishDate")
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							InAppPurchasePage.<<Settings_PropertyChanged>b__11_0>d <<Settings_PropertyChanged>b__11_0>d;
							<<Settings_PropertyChanged>b__11_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<Settings_PropertyChanged>b__11_0>d.<>4__this = inAppPurchasePage;
							<<Settings_PropertyChanged>b__11_0>d.<>1__state = -1;
							<<Settings_PropertyChanged>b__11_0>d.<>t__builder.Start<InAppPurchasePage.<<Settings_PropertyChanged>b__11_0>d>(ref <<Settings_PropertyChanged>b__11_0>d);
						});
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

			// Token: 0x06000313 RID: 787 RVA: 0x0001D3CC File Offset: 0x0001B5CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001F8 RID: 504
			public int <>1__state;

			// Token: 0x040001F9 RID: 505
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001FA RID: 506
			public PropertyChangedEventArgs e;

			// Token: 0x040001FB RID: 507
			public InAppPurchasePage <>4__this;
		}

		// Token: 0x0200009C RID: 156
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBuy_Clicked>d__23 : IAsyncStateMachine
		{
			// Token: 0x06000314 RID: 788 RVA: 0x0001D3DC File Offset: 0x0001B5DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0144;
						}
						taskAwaiter5 = inAppPurchasePage.manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePage.<btnBuy_Clicked>d__23>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter5.GetResult())
					{
						inAppPurchasePage.LastUserAction = InAppPurchasePage.UserActions.StartPurchase;
						inAppPurchasePage.contentStack.IsVisible = false;
						inAppPurchasePage.activityFrame.IsVisible = true;
						IProduct product = inAppPurchasePage.products[(int)inAppPurchasePage.productSlider.Value];
						inAppPurchasePage.manager.Purchase(product.ProductID);
						goto IL_01B0;
					}
					string text = "";
					if (PlatformHelper.IsiOS)
					{
						text = "AppStore";
					}
					else if (PlatformHelper.IsAndroid)
					{
						text = "Google Play";
					}
					string text2 = string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text);
					taskAwaiter3 = inAppPurchasePage.DisplayAlert(text2, PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<btnBuy_Clicked>d__23>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0144:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01B0:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000315 RID: 789 RVA: 0x0001D5C8 File Offset: 0x0001B7C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040001FC RID: 508
			public int <>1__state;

			// Token: 0x040001FD RID: 509
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040001FE RID: 510
			public InAppPurchasePage <>4__this;

			// Token: 0x040001FF RID: 511
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000200 RID: 512
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200009D RID: 157
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__24 : IAsyncStateMachine
		{
			// Token: 0x06000316 RID: 790 RVA: 0x0001D5D8 File Offset: 0x0001B7D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePage inAppPurchasePage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0155;
						}
						inAppPurchasePage.contentStack.IsVisible = false;
						inAppPurchasePage.activityFrame.IsVisible = true;
						taskAwaiter5 = inAppPurchasePage.manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePage.<btnRestore_Clicked>d__24>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter5.GetResult())
					{
						inAppPurchasePage.LastUserAction = InAppPurchasePage.UserActions.RestorePurchases;
						inAppPurchasePage.manager.Restore();
						goto IL_018B;
					}
					string text = "";
					if (PlatformHelper.IsiOS)
					{
						text = "AppStore";
					}
					else if (PlatformHelper.IsAndroid)
					{
						text = "Google Play";
					}
					string text2 = string.Format(Translate.GetString("ios_StoreUnavailable_Title"), text);
					taskAwaiter3 = inAppPurchasePage.DisplayAlert(text2, PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePage.<btnRestore_Clicked>d__24>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0155:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_018B:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000317 RID: 791 RVA: 0x0001D7A0 File Offset: 0x0001B9A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000201 RID: 513
			public int <>1__state;

			// Token: 0x04000202 RID: 514
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000203 RID: 515
			public InAppPurchasePage <>4__this;

			// Token: 0x04000204 RID: 516
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000205 RID: 517
			private TaskAwaiter <>u__2;
		}
	}
}
