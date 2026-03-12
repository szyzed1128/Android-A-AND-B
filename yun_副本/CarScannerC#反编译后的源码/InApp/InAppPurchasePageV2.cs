using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.InApp
{
	// Token: 0x0200047C RID: 1148
	[XamlFilePath("InApp\\InAppPurchasePageV2.xaml")]
	public class InAppPurchasePageV2 : ContentPage
	{
		// Token: 0x06002F01 RID: 12033 RVA: 0x0020A068 File Offset: 0x00208268
		public InAppPurchasePageV2()
		{
			this.InitializeComponent();
			if (RuDetector.IsRuLanguage())
			{
				this.faqLabelRus.NavigateUri = "https://www.carscanner.info/ru/purchase-restore/";
				this.faqLabel.NavigateUri = "https://www.carscanner.info/ru/purchase-restore/";
			}
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
			if (PlatformHelper.AppMarket == Markets.HMS)
			{
				this.lbSubscriptionHint.Text = Translate.GetString("ios_SubscriptionHintHuawei");
			}
			if (PlatformHelper.AppMarket == Markets.AppStore)
			{
				this.Platform = InAppPurchasePageV2.Platforms.iOS;
				return;
			}
			if (PlatformHelper.AppMarket == Markets.HMS)
			{
				this.Platform = InAppPurchasePageV2.Platforms.DroidHuawei;
				return;
			}
			if (PlatformHelper.AppMarket == Markets.GooglePlay)
			{
				this.Platform = InAppPurchasePageV2.Platforms.DroidGP;
				return;
			}
			if (PlatformHelper.AppMarket == Markets.Rustore)
			{
				this.Platform = InAppPurchasePageV2.Platforms.RuStore;
				this.lbRustore.IsVisible = true;
			}
		}

		// Token: 0x06002F02 RID: 12034 RVA: 0x0020A20C File Offset: 0x0020840C
		private async void Handle_Appearing(object sender, EventArgs e)
		{
			if (!this.WasLoaded)
			{
				this.activityFrame.IsVisible = true;
				switch (this.Platform)
				{
				case InAppPurchasePageV2.Platforms.DroidGP:
				{
					bool flag = this.forceNoRussia;
					if (!flag)
					{
						TaskAwaiter<bool> taskAwaiter = this.CheckIsRusAndSetInterface().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						flag = !taskAwaiter.GetResult();
					}
					if (flag)
					{
						this.contentStack.IsVisible = true;
						this.rusContentStack.IsVisible = false;
						await this.RequestProducts();
					}
					break;
				}
				case InAppPurchasePageV2.Platforms.DroidHuawei:
					this.contentStack.IsVisible = true;
					this.rusContentStack.IsVisible = false;
					await this.RequestProducts();
					break;
				case InAppPurchasePageV2.Platforms.iOS:
					this.contentStack.IsVisible = true;
					this.rusContentStack.IsVisible = false;
					await this.RequestProducts();
					await this.SetRusPurchase();
					break;
				case InAppPurchasePageV2.Platforms.RuStore:
					this.contentStack.IsVisible = true;
					this.rusContentStack.IsVisible = false;
					await this.RequestProducts();
					break;
				}
			}
		}

		// Token: 0x06002F03 RID: 12035 RVA: 0x0020A244 File Offset: 0x00208444
		private void btnBack_Clicked(object sender, EventArgs e)
		{
			this.manager.OnProductsReceived -= this.OnProductsReceived;
			SharedSettings.Current.PropertyChanged -= this.Settings_PropertyChanged;
			this.manager.ActionFinished -= this.Manager_ActionFinished;
			this.manager.Error -= this.Manager_Error;
			this.manager.FreeResources();
			base.Navigation.PopAsync();
		}

		// Token: 0x1700125A RID: 4698
		// (get) Token: 0x06002F04 RID: 12036 RVA: 0x00019544 File Offset: 0x00017744
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

		// Token: 0x1700125B RID: 4699
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x00019574 File Offset: 0x00017774
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

		// Token: 0x1700125C RID: 4700
		// (get) Token: 0x06002F06 RID: 12038 RVA: 0x000195A4 File Offset: 0x000177A4
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

		// Token: 0x06002F07 RID: 12039 RVA: 0x0020A2C4 File Offset: 0x002084C4
		private async void Manager_Error(object sender, string ErrorMessage)
		{
			string text = Translate.GetString("ios_PurchaseSomethingWrongText") + "\r\n" + ErrorMessage;
			if (PlatformHelper.IsiOS)
			{
				text += Translate.GetString("ios_IAP_Restrictions");
			}
			if (PlatformHelper.IsAndroid)
			{
				text += Translate.GetString("droid_IAP_Restore");
			}
			if (PlatformHelper.AppMarket == Markets.HMS && ErrorMessage != null && ErrorMessage.Contains("60050: account not logged in"))
			{
				text = Translate.GetString("huawei_NotLoggedIn") + "\n" + text;
			}
			if (PlatformHelper.IsAndroid)
			{
				if (PlatformHelper.AppMarket == Markets.GooglePlay)
				{
					if (this.ruDetected && !this.forceNoRussia)
					{
						text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "Google Play");
					}
				}
				else if (PlatformHelper.AppMarket == Markets.Rustore)
				{
					text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "RuStore");
				}
				else if (PlatformHelper.AppMarket == Markets.HMS && this.ruDetected && !this.forceNoRussia)
				{
					text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "Huawei AppGallery");
				}
			}
			await base.DisplayAlert(Translate.GetString("ios_PurchaseSomethingWrongTitle"), text, "OK");
			if (this.ruDetected && !this.forceNoRussia)
			{
				this.rusContentStack.IsVisible = true;
			}
			else
			{
				this.contentStack.IsVisible = true;
			}
			this.activityFrame.IsVisible = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x0020A304 File Offset: 0x00208504
		private void Manager_ActionFinished(object sender, EventArgs e)
		{
			if (this.ruDetected && !this.forceNoRussia)
			{
				this.rusContentStack.IsVisible = true;
			}
			else
			{
				this.contentStack.IsVisible = true;
			}
			this.activityFrame.IsVisible = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.LastUserAction = InAppPurchasePageV2.UserActions.None;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x0020A360 File Offset: 0x00208560
		private async void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdsProductPurchased" || e.PropertyName == "LicenseFinishDate")
			{
				Device.BeginInvokeOnMainThread(async delegate
				{
					this.contentStack.IsVisible = true;
					this.activityFrame.IsVisible = false;
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

		// Token: 0x06002F0A RID: 12042 RVA: 0x0020A3A0 File Offset: 0x002085A0
		private async Task RequestRating()
		{
			string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), this.GetStoreName());
			bool flag = !this.rating_requested;
			if (flag)
			{
				bool flag2 = Device.RuntimePlatform == "iOS";
				if (!flag2)
				{
					flag2 = await base.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content"));
				}
				flag = flag2;
			}
			if (flag)
			{
				this.rating_requested = true;
				DependencyService.Get<IRequestReview>(0).Request(false);
			}
		}

		// Token: 0x1700125D RID: 4701
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x0020A3E3 File Offset: 0x002085E3
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x0020A3EB File Offset: 0x002085EB
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

		// Token: 0x06002F0D RID: 12045 RVA: 0x0020A3F4 File Offset: 0x002085F4
		private async Task RequestProducts()
		{
			this.product_ids.Clear();
			await Task.Run(async delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\nLoading IDs...";
				string[] array = new string[] { "https://node4.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
				if (PlatformHelper.AppMarket == Markets.HMS)
				{
					array = new string[] { "https://node3.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://www.carscanner.info/p/", "https://node4.carscanner.info/p/" };
				}
				for (int i = 0; i < array.Length; i++)
				{
					string text;
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
					case Markets.Rustore:
						text = "rustore.json";
						break;
					case Markets.RUS:
						text = "ru.json";
						break;
					case Markets.Sideload:
						text = "sl.json";
						break;
					default:
						throw new NotImplementedException("Unknown market");
					}
					array[i] += text;
				}
				Func<string, string, bool> func = delegate(string uri, string input)
				{
					bool flag2;
					try
					{
						if (string.IsNullOrEmpty(input) || input == "[]")
						{
							throw new ArgumentException("input");
						}
						JsonConvert.DeserializeObject<List<string>>(input);
						flag2 = true;
					}
					catch (Exception)
					{
						flag2 = false;
					}
					return flag2;
				};
				string text2 = await HttpDownloader.Get(array, 7, func, true);
				bool flag = false;
				if (!string.IsNullOrEmpty(text2))
				{
					try
					{
						this.product_ids = JsonConvert.DeserializeObject<List<string>>(text2);
						flag = true;
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					ValueTaskAwaiter<bool> valueTaskAwaiter3 = RuDetector.IsUA().GetAwaiter();
					if (!valueTaskAwaiter3.IsCompleted)
					{
						await valueTaskAwaiter3;
						valueTaskAwaiter3 = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
					}
					if (valueTaskAwaiter3.GetResult())
					{
						switch (PlatformHelper.AppMarket)
						{
						case Markets.AppStore:
							this.product_ids = new List<string> { "ovz.CarScanner.ProS6M", "ovz.CarScanner.ProS1Y", "ovz.CarScanner.ProL4" };
							break;
						case Markets.GooglePlay:
							this.product_ids = new List<string> { "ovz.carscanner.s3m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
							break;
						case Markets.HMS:
							this.product_ids = new List<string> { "ovz.carscanner.s1m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
							break;
						case Markets.Rustore:
							this.product_ids = new List<string> { "ovz.carscanner.pro3" };
							break;
						case Markets.Sideload:
							this.product_ids = new List<string>();
							break;
						}
					}
				}
			});
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + string.Format("Loading prices... ({0})", this.product_ids.Count);
			this.manager.RequestProductData(this.product_ids);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x0020A438 File Offset: 0x00208638
		private async Task<bool> CheckIsCSRusPurchaseAllowed()
		{
			bool result = false;
			try
			{
				TaskAwaiter<string> taskAwaiter = HttpDownloader.Get(new string[]
				{
					"https://node2.carscanner.info/droidruspurchasehint/" + App.Version,
					"https://node3.carscanner.info/droidruspurchasehint/" + App.Version
				}, 5, null, true).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<string> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<string>);
				}
				if (taskAwaiter.GetResult() == "1")
				{
					result = true;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x0020A474 File Offset: 0x00208674
		private async Task SetRusPurchase()
		{
			InAppPurchasePageV2.<>c__DisplayClass30_0 CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			if (PlatformHelper.IsiOS)
			{
				bool flag = RuDetector.IsRuLanguage() && RuDetector.IsRuLocale();
				if (flag)
				{
					flag = await RuDetector.IsRuIP();
				}
				if (flag)
				{
					this.lbiOSRusHint.IsVisible = true;
				}
			}
			if (PlatformHelper.IsAndroid && PlatformHelper.DroidService.Resources_Configuration_Locale_Country == "RU" && App.CurrentLanguageCode == "ru")
			{
				CS$<>8__locals1.showButton = false;
				await Task.Run(delegate
				{
					InAppPurchasePageV2.<>c__DisplayClass30_0.<<SetRusPurchase>b__0>d <<SetRusPurchase>b__0>d;
					<<SetRusPurchase>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<SetRusPurchase>b__0>d.<>4__this = CS$<>8__locals1;
					<<SetRusPurchase>b__0>d.<>1__state = -1;
					<<SetRusPurchase>b__0>d.<>t__builder.Start<InAppPurchasePageV2.<>c__DisplayClass30_0.<<SetRusPurchase>b__0>d>(ref <<SetRusPurchase>b__0>d);
					return <<SetRusPurchase>b__0>d.<>t__builder.Task;
				});
				if (PlatformHelper.AppMarket == Markets.HMS)
				{
					MainThread.InvokeOnMainThreadAsync<bool>(() => CS$<>8__locals1.<>4__this.btnDroidRusPurchase.IsVisible = CS$<>8__locals1.showButton);
				}
			}
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x0020A4B7 File Offset: 0x002086B7
		private void btnDroidRusPurchase_Clicked(object sender, EventArgs e)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/ru/payment-options-in-russia/");
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x0020A4C4 File Offset: 0x002086C4
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
				this.CreatePurchaseButtons(_products);
				this.WasLoaded = true;
				this.contentStack.IsVisible = true;
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				if (this.RestoreOnAppear)
				{
					this.RestoreOnAppear = false;
					this.btnRestore_Clicked(this.btnRestore2, EventArgs.Empty);
				}
			}
			else
			{
				string text = Translate.GetString("ios_PurchaseCantGetProductsTitle");
				text = string.Format(text, this.GetStoreName());
				if (PlatformHelper.AppMarket == Markets.HMS && DebugString != null && DebugString.Contains("60050: account not logged in"))
				{
					text = Translate.GetString("huawei_NotLoggedIn") + "\n" + text;
				}
				if (PlatformHelper.AppMarket == Markets.Rustore && DebugString != null && DebugString.Contains("RuStore User Not Authorized", StringComparison.OrdinalIgnoreCase))
				{
					DebugString = "Сначала зайдите в свою учетную запись (авторизуйтесь) в приложении RuStore!";
				}
				this.WasLoaded = false;
				await base.DisplayAlert(text, Translate.GetString("ios_PurchaseCantGetProductsText") + "\n" + DebugString, "OK");
				await base.Navigation.PopAsync();
			}
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x0020A50C File Offset: 0x0020870C
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
				await base.DisplayAlert(string.Format(Translate.GetString("ios_StoreUnavailable_Title"), this.GetStoreName()), PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK");
			}
			else
			{
				this.LastUserAction = InAppPurchasePageV2.UserActions.RestorePurchases;
				this.manager.Restore();
			}
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x0020A544 File Offset: 0x00208744
		private async void btnNotRus_Clicked(object sender, EventArgs e)
		{
			this.WasLoaded = false;
			this.forceNoRussia = true;
			this.Handle_Appearing(this, EventArgs.Empty);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x0020A57C File Offset: 0x0020877C
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
				if (productID == null)
				{
					goto IL_0374;
				}
				switch (productID.Length)
				{
				case 18:
					switch (productID[16])
					{
					case '1':
						if (!(productID == "ovz.carscanner.s1m"))
						{
							if (!(productID == "ovz.carscanner.s1y"))
							{
								goto IL_0374;
							}
							goto IL_02E9;
						}
						break;
					case '2':
					case '4':
					case '5':
						goto IL_0374;
					case '3':
						if (!(productID == "ovz.carscanner.s3m"))
						{
							goto IL_0374;
						}
						goto IL_029F;
					case '6':
						if (!(productID == "ovz.carscanner.s6m"))
						{
							goto IL_0374;
						}
						goto IL_02C4;
					case '7':
						if (!(productID == "ovz.carscanner.s7d"))
						{
							goto IL_0374;
						}
						button.Text = product.LocalizedPriceString + " / 7 days";
						goto IL_0390;
					default:
						goto IL_0374;
					}
					break;
				case 19:
					switch (productID[18])
					{
					case '1':
						if (!(productID == "ovz.carscanner.pro1"))
						{
							goto IL_0374;
						}
						goto IL_0352;
					case '2':
						if (!(productID == "ovz.carscanner.pro2"))
						{
							goto IL_0374;
						}
						goto IL_0352;
					case '3':
						if (!(productID == "ovz.carscanner.pro3"))
						{
							goto IL_0374;
						}
						goto IL_0352;
					default:
						goto IL_0374;
					}
					break;
				case 20:
					switch (productID[19])
					{
					case '2':
						if (!(productID == "ovz.CarScanner.ProL2"))
						{
							goto IL_0374;
						}
						button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
						goto IL_0390;
					case '3':
						if (!(productID == "ovz.CarScanner.ProL3"))
						{
							goto IL_0374;
						}
						button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
						goto IL_0390;
					case '4':
						if (!(productID == "ovz.CarScanner.ProL4"))
						{
							goto IL_0374;
						}
						goto IL_0352;
					default:
						goto IL_0374;
					}
					break;
				case 21:
				{
					char c = productID[19];
					if (c != '1')
					{
						if (c != '3')
						{
							if (c != '6')
							{
								goto IL_0374;
							}
							if (!(productID == "ovz.CarScanner.ProS6M"))
							{
								goto IL_0374;
							}
							goto IL_02C4;
						}
						else
						{
							if (!(productID == "ovz.CarScanner.ProS3M"))
							{
								goto IL_0374;
							}
							goto IL_029F;
						}
					}
					else if (!(productID == "ovz.CarScanner.ProS1M"))
					{
						if (!(productID == "ovz.CarScanner.ProS1Y"))
						{
							goto IL_0374;
						}
						goto IL_02E9;
					}
					break;
				}
				default:
					goto IL_0374;
				}
				button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle1m");
				goto IL_0390;
				IL_029F:
				button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle3m");
				goto IL_0390;
				IL_02C4:
				button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle6m");
				goto IL_0390;
				IL_02E9:
				button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_SubscriptionTitle1y");
				goto IL_0390;
				IL_0352:
				button.Text = product.LocalizedPriceString + " / " + Translate.GetString("ios_Forever");
				IL_0390:
				button.Clicked += this.PurchaseBtn_Clicked;
				this.panelNewStylePurchaseButtons.Children.Add(button);
				continue;
				IL_0374:
				button.Text = product.LocalizedPriceString + " / " + product.ProductName;
				goto IL_0390;
			}
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x0020A974 File Offset: 0x00208B74
		private async void PurchaseBtn_Clicked(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string productId = button.ClassId;
			if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Please confirm Car Scanner Pro purchase", string.Format("Hello! You already have all Pro version features, because you're using {0} device.\nYou can purchase Pro version if you want to support developer or if you want to us all features with another device.\nDo you want to purchase Car Scanner Pro?", SharedSettings.Current.BTLEDeviceName), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					return;
				}
			}
			this.PurchaseWithProduct(productId);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x0020A9B4 File Offset: 0x00208BB4
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
				await base.DisplayAlert(string.Format(Translate.GetString("ios_StoreUnavailable_Title"), this.GetStoreName()), PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK");
			}
			else
			{
				this.LastUserAction = InAppPurchasePageV2.UserActions.StartPurchase;
				this.contentStack.IsVisible = false;
				this.activityFrame.IsVisible = true;
				this.manager.Purchase(productId);
			}
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x0020A9F3 File Offset: 0x00208BF3
		private string GetStoreName()
		{
			return PlatformHelper.AppMarketTitle;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x0020A9FC File Offset: 0x00208BFC
		private async Task<bool> CheckIsRusAndSetInterface()
		{
			if (RuDetector.IsRuLanguage() && RuDetector.IsRuLocale())
			{
				TaskAwaiter<bool> taskAwaiter = this.CheckIsCSRusPurchaseAllowed().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					bool flag = SharedSettings.Current.DroidGPCheckCurrencyR;
					if (flag)
					{
						flag = (await RuDetector.IsRuCurrency()).GetValueOrDefault();
					}
					bool flag2 = flag;
					if (!flag2)
					{
						flag2 = await RuDetector.IsRuIP();
					}
					if (flag2)
					{
						MainThreadHelper.InvokeOnMainThread(async delegate
						{
							this.ruDetected = true;
							this.contentStack.IsVisible = false;
							this.rusContentStack.IsVisible = true;
							await this.LoadProductsRus();
						});
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x0020AA40 File Offset: 0x00208C40
		private async Task LoadProductsRus()
		{
			if (PlatformHelper.IsAndroid && (PlatformHelper.AppMarket == Markets.GooglePlay || PlatformHelper.AppMarket == Markets.RUS))
			{
				await Task.Run(async delegate
				{
					try
					{
						CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass40_0();
						CS$<>8__locals1.<>4__this = this;
						Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(await HttpDownloader.Get("https://www.carscanner.info/p/ru.json", 10));
						string text = dictionary["price"];
						CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
						CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
						string text2 = dictionary["price_list"];
						CS$<>8__locals1.prices = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
						MainThread.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Clear();
							for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
							{
								string text3 = CS$<>8__locals1.prices[i];
								string text4 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text3);
								Button button = new Button
								{
									Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
									ClassId = (i + 1).ToString(),
									Text = text4,
									IsEnabled = true
								};
								button.Clicked += CS$<>8__locals1.<>4__this.btnPurchaseRus_Clicked;
								CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Add(button);
							}
							CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
							CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
							CS$<>8__locals1.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						});
						CS$<>8__locals1 = null;
					}
					catch (Exception)
					{
						MainThread.BeginInvokeOnMainThread(async delegate
						{
							await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK");
							this.btnBack_Clicked(null, null);
						});
					}
				});
			}
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x0020AA84 File Offset: 0x00208C84
		private void EulaSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			foreach (View view in this.panelNewStylePurchaseButtons.Children)
			{
				view.IsEnabled = this.EulaSwitch.IsToggled;
			}
			this.btnRestoreRus.IsEnabled = this.EulaSwitch.IsToggled;
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x0020AAF4 File Offset: 0x00208CF4
		private bool IsValidEmail(string email)
		{
			string text = email.Trim();
			if (text.EndsWith("."))
			{
				return false;
			}
			bool flag;
			try
			{
				flag = new MailAddress(email).Address == text;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x0020AB44 File Offset: 0x00208D44
		private string filterKeyForWrongSymbols(string input)
		{
			string text = "ABCDEFGHJKLMNPQRSUVWXYZ0123456789";
			StringBuilder stringBuilder = new StringBuilder(25);
			foreach (char c in input)
			{
				if (text.IndexOf(c) >= 0)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x0020AB98 File Offset: 0x00208D98
		private async void btnPurchaseRus_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = Launcher.TryOpenAsync("https://ru.carscanner.info/buy/").GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				await base.DisplayAlert("Ошибка!", "Ошибка при попытке запустить браузер для оплаты.\nПожалуйста, убедитесь, что у вас установлен браузер.", "ОК");
			}
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x0020ABD0 File Offset: 0x00208DD0
		private async void btnRestoreRus_Clicked(object sender, EventArgs e)
		{
		}

		// Token: 0x06002F1F RID: 12063 RVA: 0x0020AC00 File Offset: 0x00208E00
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(InAppPurchasePageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "InApp/InAppPurchasePageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 52);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Button)), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 29);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 26);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 29);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 36);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 78);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 30);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 33);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 30);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 33);
			HyperLinkLabel hyperLinkLabel;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel = new HyperLinkLabel(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 30);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 33);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 33);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 30);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 33);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 33);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 30);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 29);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 26);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 57);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 26);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 26);
			HyperLinkLabel hyperLinkLabel2;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel2 = new HyperLinkLabel(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 26);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 26);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 29);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 29);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 26);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 32);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 26);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 26);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 32);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 26);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 29);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 29);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 26);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 29);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 29);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 26);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 29);
			HyperLinkLabel hyperLinkLabel3;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel3 = new HyperLinkLabel(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 26);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 22);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("InApp\\InAppPurchasePageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			nameScope.RegisterName("contentStack", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "contentStack";
			}
			nameScope.RegisterName("lbRustore", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "lbRustore";
			}
			nameScope.RegisterName("stackNewStyle", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "stackNewStyle";
			}
			nameScope.RegisterName("btnDroidRusPurchase", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnDroidRusPurchase";
			}
			nameScope.RegisterName("panelNewStylePurchaseButtons", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelNewStylePurchaseButtons";
			}
			nameScope.RegisterName("btnRestore2", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnRestore2";
			}
			nameScope.RegisterName("faqLabel", hyperLinkLabel);
			if (hyperLinkLabel.StyleId == null)
			{
				hyperLinkLabel.StyleId = "faqLabel";
			}
			nameScope.RegisterName("lbiOSRusHint", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "lbiOSRusHint";
			}
			nameScope.RegisterName("lbSubscriptionHint", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "lbSubscriptionHint";
			}
			nameScope.RegisterName("rusContentStack", stackLayout5);
			if (stackLayout5.StyleId == null)
			{
				stackLayout5.StyleId = "rusContentStack";
			}
			nameScope.RegisterName("lbPurchaseDetailsShort", label7);
			if (label7.StyleId == null)
			{
				label7.StyleId = "lbPurchaseDetailsShort";
			}
			nameScope.RegisterName("EulaSwitch", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "EulaSwitch";
			}
			nameScope.RegisterName("btnRestoreRus", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnRestoreRus";
			}
			nameScope.RegisterName("panelNewStylePurchaseButtonsRus", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "panelNewStylePurchaseButtonsRus";
			}
			nameScope.RegisterName("btnRestore3", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnRestore3";
			}
			nameScope.RegisterName("btnNotRus", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnNotRus";
			}
			nameScope.RegisterName("faqLabelRus", hyperLinkLabel3);
			if (hyperLinkLabel3.StyleId == null)
			{
				hyperLinkLabel3.StyleId = "faqLabelRus";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.contentStack = stackLayout3;
			this.lbRustore = label2;
			this.stackNewStyle = stackLayout2;
			this.btnDroidRusPurchase = button;
			this.panelNewStylePurchaseButtons = stackLayout;
			this.btnRestore2 = button2;
			this.faqLabel = hyperLinkLabel;
			this.lbiOSRusHint = label4;
			this.lbSubscriptionHint = label5;
			this.rusContentStack = stackLayout5;
			this.lbPurchaseDetailsShort = label7;
			this.EulaSwitch = labelSwitch;
			this.btnRestoreRus = button3;
			this.panelNewStylePurchaseButtonsRus = stackLayout4;
			this.btnRestore3 = button4;
			this.btnNotRus = button5;
			this.faqLabelRus = hyperLinkLabel3;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(20, 52)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 17)));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension4.Key = "NavigationBarNonScalableLabel";
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			nonScalableLabel.SetValue(Label.TextProperty, "Car Scanner Pro");
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(nonScalableLabel);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid3.SetValue(View.MarginProperty, new Thickness(10.0, 0.0, 10.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(Grid.RowProperty, 0);
			image.SetValue(Image.AspectProperty, 0);
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension5.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = image;
			array6[1] = stackLayout3;
			array6[2] = grid2;
			array6[3] = scrollView;
			array6[4] = grid3;
			array6[5] = this;
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 29)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource5.Key);
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout3.Children.Add(image);
			label.SetValue(Grid.RowProperty, 1);
			label.SetValue(Label.LineBreakModeProperty, 1);
			translate2.Text = "ios_CarScannerProAdvantages";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = label;
			array7[1] = stackLayout3;
			array7[2] = grid2;
			array7[3] = scrollView;
			array7[4] = grid3;
			array7[5] = this;
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 29)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label.Text = obj9;
			stackLayout3.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label2.SetValue(Label.TextProperty, "Вы покупаете лицензию через магазин RuStore. Ваша покупка будет связана с вашей учетной записью в магазине RuStore и активируется автоматически при установке из RuStore. Никакого ключа активации для версии Car Scanner из RuStore не существует.");
			dynamicResourceExtension6.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label2;
			array8[1] = stackLayout3;
			array8[2] = grid2;
			array8[3] = scrollView;
			array8[4] = grid3;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 29)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label2.SetDynamicResource(Label.TextColorProperty, dynamicResource6.Key);
			stackLayout3.Children.Add(label2);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = label3;
			array9[1] = stackLayout2;
			array9[2] = stackLayout3;
			array9[3] = grid2;
			array9[4] = scrollView;
			array9[5] = grid3;
			array9[6] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 36)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			translate3.Text = "ios_ChoosePriceWithSubscriptions";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = label3;
			array10[1] = stackLayout2;
			array10[2] = stackLayout3;
			array10[3] = grid2;
			array10[4] = scrollView;
			array10[5] = grid3;
			array10[6] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 78)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label3.Text = obj13;
			stackLayout2.Children.Add(label3);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			button.Clicked += this.btnDroidRusPurchase_Clicked;
			button.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate4.Text = "droid_RusPaymentButton";
			IMarkupExtension markupExtension11 = translate4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = button;
			array11[1] = stackLayout2;
			array11[2] = stackLayout3;
			array11[3] = grid2;
			array11[4] = scrollView;
			array11[5] = grid3;
			array11[6] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 33)));
			object obj15 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button.Text = obj15;
			button.SetValue(Button.TextColorProperty, Color.White);
			stackLayout2.Children.Add(button);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout2.Children.Add(stackLayout);
			button2.SetValue(Grid.RowProperty, 7);
			dynamicResourceExtension8.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 7];
			array12[0] = button2;
			array12[1] = stackLayout2;
			array12[2] = stackLayout3;
			array12[3] = grid2;
			array12[4] = scrollView;
			array12[5] = grid3;
			array12[6] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 33)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			button2.Clicked += this.btnRestore_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			translate5.Text = "ios_RestorePurchases";
			IMarkupExtension markupExtension13 = translate5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = button2;
			array13[1] = stackLayout2;
			array13[2] = stackLayout3;
			array13[3] = grid2;
			array13[4] = scrollView;
			array13[5] = grid3;
			array13[6] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 33)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button2.Text = obj18;
			button2.SetValue(Button.TextColorProperty, Color.White);
			stackLayout2.Children.Add(button2);
			hyperLinkLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel.SetValue(HyperLinkLabel.NavigateUriProperty, "https://www.carscanner.info/purchase-restore/");
			translate6.Text = "purchase_FAQLabel";
			IMarkupExtension markupExtension14 = translate6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 7];
			array14[0] = hyperLinkLabel;
			array14[1] = stackLayout2;
			array14[2] = stackLayout3;
			array14[3] = grid2;
			array14[4] = scrollView;
			array14[5] = grid3;
			array14[6] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 33)));
			object obj20 = markupExtension14.ProvideValue(xamlServiceProvider14);
			hyperLinkLabel.Text = obj20;
			stackLayout2.Children.Add(hyperLinkLabel);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension9.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = label4;
			array15[1] = stackLayout2;
			array15[2] = stackLayout3;
			array15[3] = grid2;
			array15[4] = scrollView;
			array15[5] = grid3;
			array15[6] = this;
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
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 33)));
			DynamicResource dynamicResource9 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource9.Key);
			label4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate7.Text = "ios_RusPaymentsHint";
			IMarkupExtension markupExtension16 = translate7;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = label4;
			array16[1] = stackLayout2;
			array16[2] = stackLayout3;
			array16[3] = grid2;
			array16[4] = scrollView;
			array16[5] = grid3;
			array16[6] = this;
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
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 33)));
			object obj23 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label4.Text = obj23;
			label4.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout2.Children.Add(label4);
			dynamicResourceExtension10.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 7];
			array17[0] = label5;
			array17[1] = stackLayout2;
			array17[2] = stackLayout3;
			array17[3] = grid2;
			array17[4] = scrollView;
			array17[5] = grid3;
			array17[6] = this;
			object obj24;
			xamlServiceProvider17.Add(typeFromHandle33, obj24 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 33)));
			DynamicResource dynamicResource10 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource10.Key);
			label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate8.Text = "ios_SubscriptionHint";
			IMarkupExtension markupExtension18 = translate8;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = label5;
			array18[1] = stackLayout2;
			array18[2] = stackLayout3;
			array18[3] = grid2;
			array18[4] = scrollView;
			array18[5] = grid3;
			array18[6] = this;
			object obj25;
			xamlServiceProvider18.Add(typeFromHandle35, obj25 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 33)));
			object obj26 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label5.Text = obj26;
			stackLayout2.Children.Add(label5);
			stackLayout3.Children.Add(stackLayout2);
			grid2.Children.Add(stackLayout3);
			stackLayout5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			image2.SetValue(Grid.RowProperty, 0);
			image2.SetValue(Image.AspectProperty, 0);
			image2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension11.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 6];
			array19[0] = image2;
			array19[1] = stackLayout5;
			array19[2] = grid2;
			array19[3] = scrollView;
			array19[4] = grid3;
			array19[5] = this;
			object obj27;
			xamlServiceProvider19.Add(typeFromHandle37, obj27 = new SimpleValueTargetProvider(array19, Image.SourceProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 29)));
			DynamicResource dynamicResource11 = markupExtension19.ProvideValue(xamlServiceProvider19);
			image2.SetDynamicResource(Image.SourceProperty, dynamicResource11.Key);
			image2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout5.Children.Add(image2);
			label6.SetValue(Label.LineBreakModeProperty, 1);
			translate9.Text = "ios_CarScannerProAdvantages";
			IMarkupExtension markupExtension20 = translate9;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = label6;
			array20[1] = stackLayout5;
			array20[2] = grid2;
			array20[3] = scrollView;
			array20[4] = grid3;
			array20[5] = this;
			object obj28;
			xamlServiceProvider20.Add(typeFromHandle39, obj28 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 57)));
			object obj29 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label6.Text = obj29;
			stackLayout5.Children.Add(label6);
			label7.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label7.SetValue(Label.LineBreakModeProperty, 1);
			stackLayout5.Children.Add(label7);
			hyperLinkLabel2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			hyperLinkLabel2.SetValue(HyperLinkLabel.NavigateUriProperty, "http://ru.carscanner.info/eula/");
			hyperLinkLabel2.SetValue(Label.TextProperty, "Лицензионное соглашение");
			stackLayout5.Children.Add(hyperLinkLabel2);
			labelSwitch.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			labelSwitch.SetValue(LabelSwitch.TextProperty, "Условия лицензионного соглашения прочитаны. Оплата является подтверждением принятия условий лицензионного соглашения");
			labelSwitch.Toggled += this.EulaSwitch_Toggled;
			stackLayout5.Children.Add(labelSwitch);
			button3.SetValue(Grid.RowProperty, 7);
			dynamicResourceExtension12.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = button3;
			array21[1] = stackLayout5;
			array21[2] = grid2;
			array21[3] = scrollView;
			array21[4] = grid3;
			array21[5] = this;
			object obj30;
			xamlServiceProvider21.Add(typeFromHandle41, obj30 = new SimpleValueTargetProvider(array21, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 29)));
			DynamicResource dynamicResource12 = markupExtension21.ProvideValue(xamlServiceProvider21);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource12.Key);
			button3.Clicked += this.btnRestoreRus_Clicked;
			button3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			button3.SetValue(VisualElement.IsEnabledProperty, false);
			button3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate10.Text = "rus_ActivateKey";
			IMarkupExtension markupExtension22 = translate10;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 6];
			array22[0] = button3;
			array22[1] = stackLayout5;
			array22[2] = grid2;
			array22[3] = scrollView;
			array22[4] = grid3;
			array22[5] = this;
			object obj31;
			xamlServiceProvider22.Add(typeFromHandle43, obj31 = new SimpleValueTargetProvider(array22, Button.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 29)));
			object obj32 = markupExtension22.ProvideValue(xamlServiceProvider22);
			button3.Text = obj32;
			button3.SetValue(Button.TextColorProperty, Color.White);
			stackLayout5.Children.Add(button3);
			translate11.Text = "ru_GP_Text";
			IMarkupExtension markupExtension23 = translate11;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = label8;
			array23[1] = stackLayout5;
			array23[2] = grid2;
			array23[3] = scrollView;
			array23[4] = grid3;
			array23[5] = this;
			object obj33;
			xamlServiceProvider23.Add(typeFromHandle45, obj33 = new SimpleValueTargetProvider(array23, Label.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 32)));
			object obj34 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label8.Text = obj34;
			stackLayout5.Children.Add(label8);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout5.Children.Add(stackLayout4);
			translate12.Text = "ru_GP_restoreOldGooglePurchase";
			IMarkupExtension markupExtension24 = translate12;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = label9;
			array24[1] = stackLayout5;
			array24[2] = grid2;
			array24[3] = scrollView;
			array24[4] = grid3;
			array24[5] = this;
			object obj35;
			xamlServiceProvider24.Add(typeFromHandle47, obj35 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(177, 32)));
			object obj36 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label9.Text = obj36;
			stackLayout5.Children.Add(label9);
			button4.SetValue(Grid.RowProperty, 7);
			dynamicResourceExtension13.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = button4;
			array25[1] = stackLayout5;
			array25[2] = grid2;
			array25[3] = scrollView;
			array25[4] = grid3;
			array25[5] = this;
			object obj37;
			xamlServiceProvider25.Add(typeFromHandle49, obj37 = new SimpleValueTargetProvider(array25, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(181, 29)));
			DynamicResource dynamicResource13 = markupExtension25.ProvideValue(xamlServiceProvider25);
			button4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource13.Key);
			button4.Clicked += this.btnRestore_Clicked;
			button4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			translate13.Text = "ios_RestorePurchases";
			IMarkupExtension markupExtension26 = translate13;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = button4;
			array26[1] = stackLayout5;
			array26[2] = grid2;
			array26[3] = scrollView;
			array26[4] = grid3;
			array26[5] = this;
			object obj38;
			xamlServiceProvider26.Add(typeFromHandle51, obj38 = new SimpleValueTargetProvider(array26, Button.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 29)));
			object obj39 = markupExtension26.ProvideValue(xamlServiceProvider26);
			button4.Text = obj39;
			button4.SetValue(Button.TextColorProperty, Color.White);
			stackLayout5.Children.Add(button4);
			dynamicResourceExtension14.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = button5;
			array27[1] = stackLayout5;
			array27[2] = grid2;
			array27[3] = scrollView;
			array27[4] = grid3;
			array27[5] = this;
			object obj40;
			xamlServiceProvider27.Add(typeFromHandle53, obj40 = new SimpleValueTargetProvider(array27, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 29)));
			DynamicResource dynamicResource14 = markupExtension27.ProvideValue(xamlServiceProvider27);
			button5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource14.Key);
			button5.Clicked += this.btnNotRus_Clicked;
			button5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			translate14.Text = "ru_notRu";
			IMarkupExtension markupExtension28 = translate14;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 6];
			array28[0] = button5;
			array28[1] = stackLayout5;
			array28[2] = grid2;
			array28[3] = scrollView;
			array28[4] = grid3;
			array28[5] = this;
			object obj41;
			xamlServiceProvider28.Add(typeFromHandle55, obj41 = new SimpleValueTargetProvider(array28, Button.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 29)));
			object obj42 = markupExtension28.ProvideValue(xamlServiceProvider28);
			button5.Text = obj42;
			button5.SetValue(Button.TextColorProperty, Color.White);
			stackLayout5.Children.Add(button5);
			hyperLinkLabel3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel3.SetValue(HyperLinkLabel.NavigateUriProperty, "https://www.carscanner.info/purchase-restore/");
			translate15.Text = "purchase_FAQLabel";
			IMarkupExtension markupExtension29 = translate15;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 6];
			array29[0] = hyperLinkLabel3;
			array29[1] = stackLayout5;
			array29[2] = grid2;
			array29[3] = scrollView;
			array29[4] = grid3;
			array29[5] = this;
			object obj43;
			xamlServiceProvider29.Add(typeFromHandle57, obj43 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(InAppPurchasePageV2).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 29)));
			object obj44 = markupExtension29.ProvideValue(xamlServiceProvider29);
			hyperLinkLabel3.Text = obj44;
			stackLayout5.Children.Add(hyperLinkLabel3);
			grid2.Children.Add(stackLayout5);
			scrollView.Content = grid2;
			grid3.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid3.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06002F20 RID: 12064 RVA: 0x0020E928 File Offset: 0x0020CB28
		[CompilerGenerated]
		private async void <Settings_PropertyChanged>b__21_0()
		{
			this.contentStack.IsVisible = true;
			this.activityFrame.IsVisible = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

		// Token: 0x06002F21 RID: 12065 RVA: 0x0020E960 File Offset: 0x0020CB60
		[CompilerGenerated]
		private async Task <RequestProducts>b__28_0()
		{
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\nLoading IDs...";
			string[] array = new string[] { "https://node4.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
			if (PlatformHelper.AppMarket == Markets.HMS)
			{
				array = new string[] { "https://node3.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://www.carscanner.info/p/", "https://node4.carscanner.info/p/" };
			}
			for (int i = 0; i < array.Length; i++)
			{
				string text;
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
				case Markets.Rustore:
					text = "rustore.json";
					break;
				case Markets.RUS:
					text = "ru.json";
					break;
				case Markets.Sideload:
					text = "sl.json";
					break;
				default:
					throw new NotImplementedException("Unknown market");
				}
				array[i] += text;
			}
			Func<string, string, bool> func = delegate(string uri, string input)
			{
				bool flag2;
				try
				{
					if (string.IsNullOrEmpty(input) || input == "[]")
					{
						throw new ArgumentException("input");
					}
					JsonConvert.DeserializeObject<List<string>>(input);
					flag2 = true;
				}
				catch (Exception)
				{
					flag2 = false;
				}
				return flag2;
			};
			string text2 = await HttpDownloader.Get(array, 7, func, true);
			bool flag = false;
			if (!string.IsNullOrEmpty(text2))
			{
				try
				{
					this.product_ids = JsonConvert.DeserializeObject<List<string>>(text2);
					flag = true;
				}
				catch (Exception)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				ValueTaskAwaiter<bool> valueTaskAwaiter = RuDetector.IsUA().GetAwaiter();
				if (!valueTaskAwaiter.IsCompleted)
				{
					await valueTaskAwaiter;
					ValueTaskAwaiter<bool> valueTaskAwaiter2;
					valueTaskAwaiter = valueTaskAwaiter2;
					valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
				}
				if (valueTaskAwaiter.GetResult())
				{
					switch (PlatformHelper.AppMarket)
					{
					case Markets.AppStore:
						this.product_ids = new List<string> { "ovz.CarScanner.ProS6M", "ovz.CarScanner.ProS1Y", "ovz.CarScanner.ProL4" };
						break;
					case Markets.GooglePlay:
						this.product_ids = new List<string> { "ovz.carscanner.s3m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
						break;
					case Markets.HMS:
						this.product_ids = new List<string> { "ovz.carscanner.s1m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
						break;
					case Markets.Rustore:
						this.product_ids = new List<string> { "ovz.carscanner.pro3" };
						break;
					case Markets.Sideload:
						this.product_ids = new List<string>();
						break;
					}
				}
			}
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x0020E9A4 File Offset: 0x0020CBA4
		[CompilerGenerated]
		private async void <CheckIsRusAndSetInterface>b__39_0()
		{
			this.ruDetected = true;
			this.contentStack.IsVisible = false;
			this.rusContentStack.IsVisible = true;
			await this.LoadProductsRus();
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x0020E9DC File Offset: 0x0020CBDC
		[CompilerGenerated]
		private async Task <LoadProductsRus>b__40_0()
		{
			try
			{
				InAppPurchasePageV2.<>c__DisplayClass40_0 CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass40_0();
				CS$<>8__locals1.<>4__this = this;
				Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(await HttpDownloader.Get("https://www.carscanner.info/p/ru.json", 10));
				string text = dictionary["price"];
				CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
				CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
				string text2 = dictionary["price_list"];
				CS$<>8__locals1.prices = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				MainThread.BeginInvokeOnMainThread(delegate
				{
					CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Clear();
					for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
					{
						string text3 = CS$<>8__locals1.prices[i];
						string text4 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text3);
						Button button = new Button
						{
							Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
							ClassId = (i + 1).ToString(),
							Text = text4,
							IsEnabled = true
						};
						button.Clicked += CS$<>8__locals1.<>4__this.btnPurchaseRus_Clicked;
						CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Add(button);
					}
					CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
					CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
					CS$<>8__locals1.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				});
				CS$<>8__locals1 = null;
			}
			catch (Exception)
			{
				MainThread.BeginInvokeOnMainThread(async delegate
				{
					await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK");
					this.btnBack_Clicked(null, null);
				});
			}
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x0020EA20 File Offset: 0x0020CC20
		[CompilerGenerated]
		private async void <LoadProductsRus>b__40_2()
		{
			await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK");
			this.btnBack_Clicked(null, null);
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x0020EA58 File Offset: 0x0020CC58
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<InAppPurchasePageV2>(this, typeof(InAppPurchasePageV2));
			this.contentStack = NameScopeExtensions.FindByName<StackLayout>(this, "contentStack");
			this.lbRustore = NameScopeExtensions.FindByName<Label>(this, "lbRustore");
			this.stackNewStyle = NameScopeExtensions.FindByName<StackLayout>(this, "stackNewStyle");
			this.btnDroidRusPurchase = NameScopeExtensions.FindByName<Button>(this, "btnDroidRusPurchase");
			this.panelNewStylePurchaseButtons = NameScopeExtensions.FindByName<StackLayout>(this, "panelNewStylePurchaseButtons");
			this.btnRestore2 = NameScopeExtensions.FindByName<Button>(this, "btnRestore2");
			this.faqLabel = NameScopeExtensions.FindByName<HyperLinkLabel>(this, "faqLabel");
			this.lbiOSRusHint = NameScopeExtensions.FindByName<Label>(this, "lbiOSRusHint");
			this.lbSubscriptionHint = NameScopeExtensions.FindByName<Label>(this, "lbSubscriptionHint");
			this.rusContentStack = NameScopeExtensions.FindByName<StackLayout>(this, "rusContentStack");
			this.lbPurchaseDetailsShort = NameScopeExtensions.FindByName<Label>(this, "lbPurchaseDetailsShort");
			this.EulaSwitch = NameScopeExtensions.FindByName<LabelSwitch>(this, "EulaSwitch");
			this.btnRestoreRus = NameScopeExtensions.FindByName<Button>(this, "btnRestoreRus");
			this.panelNewStylePurchaseButtonsRus = NameScopeExtensions.FindByName<StackLayout>(this, "panelNewStylePurchaseButtonsRus");
			this.btnRestore3 = NameScopeExtensions.FindByName<Button>(this, "btnRestore3");
			this.btnNotRus = NameScopeExtensions.FindByName<Button>(this, "btnNotRus");
			this.faqLabelRus = NameScopeExtensions.FindByName<HyperLinkLabel>(this, "faqLabelRus");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04001AD1 RID: 6865
		private InAppPurchasePageV2.Platforms Platform;

		// Token: 0x04001AD2 RID: 6866
		private bool ruDetected;

		// Token: 0x04001AD3 RID: 6867
		private ICustomInAppManager manager;

		// Token: 0x04001AD4 RID: 6868
		private bool WasLoaded;

		// Token: 0x04001AD5 RID: 6869
		private bool forceNoRussia;

		// Token: 0x04001AD6 RID: 6870
		public List<string> product_ids = new List<string>();

		// Token: 0x04001AD7 RID: 6871
		public List<IProduct> products;

		// Token: 0x04001AD8 RID: 6872
		private InAppPurchasePageV2.UserActions LastUserAction;

		// Token: 0x04001AD9 RID: 6873
		private bool rating_requested;

		// Token: 0x04001ADA RID: 6874
		private bool _RestoreOnAppear;

		// Token: 0x04001ADB RID: 6875
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout contentStack;

		// Token: 0x04001ADC RID: 6876
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbRustore;

		// Token: 0x04001ADD RID: 6877
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackNewStyle;

		// Token: 0x04001ADE RID: 6878
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDroidRusPurchase;

		// Token: 0x04001ADF RID: 6879
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNewStylePurchaseButtons;

		// Token: 0x04001AE0 RID: 6880
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore2;

		// Token: 0x04001AE1 RID: 6881
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private HyperLinkLabel faqLabel;

		// Token: 0x04001AE2 RID: 6882
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbiOSRusHint;

		// Token: 0x04001AE3 RID: 6883
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbSubscriptionHint;

		// Token: 0x04001AE4 RID: 6884
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout rusContentStack;

		// Token: 0x04001AE5 RID: 6885
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbPurchaseDetailsShort;

		// Token: 0x04001AE6 RID: 6886
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch EulaSwitch;

		// Token: 0x04001AE7 RID: 6887
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestoreRus;

		// Token: 0x04001AE8 RID: 6888
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNewStylePurchaseButtonsRus;

		// Token: 0x04001AE9 RID: 6889
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore3;

		// Token: 0x04001AEA RID: 6890
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnNotRus;

		// Token: 0x04001AEB RID: 6891
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private HyperLinkLabel faqLabelRus;

		// Token: 0x04001AEC RID: 6892
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0200047D RID: 1149
		private enum Platforms
		{
			// Token: 0x04001AEE RID: 6894
			DroidGP,
			// Token: 0x04001AEF RID: 6895
			DroidHuawei,
			// Token: 0x04001AF0 RID: 6896
			iOS,
			// Token: 0x04001AF1 RID: 6897
			RuStore
		}

		// Token: 0x0200047E RID: 1150
		private enum UserActions
		{
			// Token: 0x04001AF3 RID: 6899
			None,
			// Token: 0x04001AF4 RID: 6900
			RequestProducts,
			// Token: 0x04001AF5 RID: 6901
			StartPurchase,
			// Token: 0x04001AF6 RID: 6902
			RestorePurchases
		}

		// Token: 0x0200047F RID: 1151
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<CheckIsRusAndSetInterface>b__39_0>d : IAsyncStateMachine
		{
			// Token: 0x06002F26 RID: 12070 RVA: 0x0020EBA8 File Offset: 0x0020CDA8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						inAppPurchasePageV.ruDetected = true;
						inAppPurchasePageV.contentStack.IsVisible = false;
						inAppPurchasePageV.rusContentStack.IsVisible = true;
						taskAwaiter = inAppPurchasePageV.LoadProductsRus().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<<CheckIsRusAndSetInterface>b__39_0>d>(ref taskAwaiter, ref this);
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

			// Token: 0x06002F27 RID: 12071 RVA: 0x0020EC7C File Offset: 0x0020CE7C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AF7 RID: 6903
			public int <>1__state;

			// Token: 0x04001AF8 RID: 6904
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001AF9 RID: 6905
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001AFA RID: 6906
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000480 RID: 1152
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<LoadProductsRus>b__40_0>d : IAsyncStateMachine
		{
			// Token: 0x06002F28 RID: 12072 RVA: 0x0020EC8C File Offset: 0x0020CE8C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass40_0();
							CS$<>8__locals1.<>4__this = inAppPurchasePageV;
							taskAwaiter = HttpDownloader.Get("https://www.carscanner.info/p/ru.json", 10).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageV2.<<LoadProductsRus>b__40_0>d>(ref taskAwaiter, ref this);
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
						Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(taskAwaiter.GetResult());
						string text = dictionary["price"];
						CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
						CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
						string text2 = dictionary["price_list"];
						CS$<>8__locals1.prices = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
						MainThread.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Clear();
							for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
							{
								string text3 = CS$<>8__locals1.prices[i];
								string text4 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text3);
								Button button = new Button
								{
									Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
									ClassId = (i + 1).ToString(),
									Text = text4,
									IsEnabled = true
								};
								button.Clicked += CS$<>8__locals1.<>4__this.btnPurchaseRus_Clicked;
								CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtonsRus.Children.Add(button);
							}
							CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
							CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
							CS$<>8__locals1.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						});
						CS$<>8__locals1 = null;
					}
					catch (Exception)
					{
						MainThread.BeginInvokeOnMainThread(delegate
						{
							InAppPurchasePageV2.<<LoadProductsRus>b__40_2>d <<LoadProductsRus>b__40_2>d;
							<<LoadProductsRus>b__40_2>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<LoadProductsRus>b__40_2>d.<>4__this = inAppPurchasePageV;
							<<LoadProductsRus>b__40_2>d.<>1__state = -1;
							<<LoadProductsRus>b__40_2>d.<>t__builder.Start<InAppPurchasePageV2.<<LoadProductsRus>b__40_2>d>(ref <<LoadProductsRus>b__40_2>d);
						});
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

			// Token: 0x06002F29 RID: 12073 RVA: 0x0020EE24 File Offset: 0x0020D024
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AFB RID: 6907
			public int <>1__state;

			// Token: 0x04001AFC RID: 6908
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001AFD RID: 6909
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001AFE RID: 6910
			private InAppPurchasePageV2.<>c__DisplayClass40_0 <>8__1;

			// Token: 0x04001AFF RID: 6911
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000481 RID: 1153
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<LoadProductsRus>b__40_2>d : IAsyncStateMachine
		{
			// Token: 0x06002F2A RID: 12074 RVA: 0x0020EE34 File Offset: 0x0020D034
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = inAppPurchasePageV.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<<LoadProductsRus>b__40_2>d>(ref taskAwaiter, ref this);
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
					inAppPurchasePageV.btnBack_Clicked(null, null);
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

			// Token: 0x06002F2B RID: 12075 RVA: 0x0020EF04 File Offset: 0x0020D104
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B00 RID: 6912
			public int <>1__state;

			// Token: 0x04001B01 RID: 6913
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B02 RID: 6914
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B03 RID: 6915
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000482 RID: 1154
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<RequestProducts>b__28_0>d : IAsyncStateMachine
		{
			// Token: 0x06002F2C RID: 12076 RVA: 0x0020EF14 File Offset: 0x0020D114
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					ValueTaskAwaiter<bool> valueTaskAwaiter3;
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num2 = -1;
							goto IL_0218;
						}
						inAppPurchasePageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\nLoading IDs...";
						string[] array = new string[] { "https://node4.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://node3.carscanner.info/p/" };
						if (PlatformHelper.AppMarket == Markets.HMS)
						{
							array = new string[] { "https://node3.carscanner.info/p/", "https://node2.carscanner.info/p/", "https://www.carscanner.info/p/", "https://node4.carscanner.info/p/" };
						}
						for (int i = 0; i < array.Length; i++)
						{
							string text;
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
							case Markets.Rustore:
								text = "rustore.json";
								break;
							case Markets.RUS:
								text = "ru.json";
								break;
							case Markets.Sideload:
								text = "sl.json";
								break;
							default:
								throw new NotImplementedException("Unknown market");
							}
							array[i] += text;
						}
						Func<string, string, bool> func = delegate(string uri, string input)
						{
							bool flag2;
							try
							{
								if (string.IsNullOrEmpty(input) || input == "[]")
								{
									throw new ArgumentException("input");
								}
								JsonConvert.DeserializeObject<List<string>>(input);
								flag2 = true;
							}
							catch (Exception)
							{
								flag2 = false;
							}
							return flag2;
						};
						taskAwaiter = HttpDownloader.Get(array, 7, func, true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageV2.<<RequestProducts>b__28_0>d>(ref taskAwaiter, ref this);
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
					bool flag = false;
					if (!string.IsNullOrEmpty(result))
					{
						try
						{
							inAppPurchasePageV.product_ids = JsonConvert.DeserializeObject<List<string>>(result);
							flag = true;
						}
						catch (Exception)
						{
							flag = false;
						}
					}
					if (flag)
					{
						goto IL_02FC;
					}
					valueTaskAwaiter3 = RuDetector.IsUA().GetAwaiter();
					if (!valueTaskAwaiter3.IsCompleted)
					{
						num2 = 1;
						valueTaskAwaiter2 = valueTaskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, InAppPurchasePageV2.<<RequestProducts>b__28_0>d>(ref valueTaskAwaiter3, ref this);
						return;
					}
					IL_0218:
					if (valueTaskAwaiter3.GetResult())
					{
						switch (PlatformHelper.AppMarket)
						{
						case Markets.AppStore:
							inAppPurchasePageV.product_ids = new List<string> { "ovz.CarScanner.ProS6M", "ovz.CarScanner.ProS1Y", "ovz.CarScanner.ProL4" };
							break;
						case Markets.GooglePlay:
							inAppPurchasePageV.product_ids = new List<string> { "ovz.carscanner.s3m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
							break;
						case Markets.HMS:
							inAppPurchasePageV.product_ids = new List<string> { "ovz.carscanner.s1m", "ovz.carscanner.s1y", "ovz.carscanner.pro3" };
							break;
						case Markets.Rustore:
							inAppPurchasePageV.product_ids = new List<string> { "ovz.carscanner.pro3" };
							break;
						case Markets.Sideload:
							inAppPurchasePageV.product_ids = new List<string>();
							break;
						}
					}
					IL_02FC:;
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

			// Token: 0x06002F2D RID: 12077 RVA: 0x0020F280 File Offset: 0x0020D480
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B04 RID: 6916
			public int <>1__state;

			// Token: 0x04001B05 RID: 6917
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B06 RID: 6918
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B07 RID: 6919
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001B08 RID: 6920
			private ValueTaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000483 RID: 1155
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<Settings_PropertyChanged>b__21_0>d : IAsyncStateMachine
		{
			// Token: 0x06002F2E RID: 12078 RVA: 0x0020F290 File Offset: 0x0020D490
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					if (num > 2)
					{
						inAppPurchasePageV.contentStack.IsVisible = true;
						inAppPurchasePageV.activityFrame.IsVisible = false;
						inAppPurchasePageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						if (!SharedSettings.Current.AdsProductPurchased)
						{
							goto IL_01B9;
						}
						SharedSettings.Current.PropertyChanged -= inAppPurchasePageV.Settings_PropertyChanged;
					}
					try
					{
						if (num <= 2 || App.GetCurrentPage() == inAppPurchasePageV)
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
									goto IL_014A;
								}
								case 2:
								{
									TaskAwaiter<Page> taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter<Page>);
									num2 = -1;
									goto IL_01A7;
								}
								default:
									taskAwaiter = inAppPurchasePageV.DisplayAlert(Translate.GetString("ios_ThankYou"), Translate.GetString("RMA_TEXT"), "OK").GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num2 = 0;
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<<Settings_PropertyChanged>b__21_0>d>(ref taskAwaiter, ref this);
										return;
									}
									break;
								}
								taskAwaiter.GetResult();
								taskAwaiter = inAppPurchasePageV.RequestRating().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<<Settings_PropertyChanged>b__21_0>d>(ref taskAwaiter, ref this);
									return;
								}
								IL_014A:
								taskAwaiter.GetResult();
								taskAwaiter3 = inAppPurchasePageV.Navigation.PopAsync().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InAppPurchasePageV2.<<Settings_PropertyChanged>b__21_0>d>(ref taskAwaiter3, ref this);
									return;
								}
								IL_01A7:
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
					IL_01B9:;
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

			// Token: 0x06002F2F RID: 12079 RVA: 0x0020F4D0 File Offset: 0x0020D6D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B09 RID: 6921
			public int <>1__state;

			// Token: 0x04001B0A RID: 6922
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B0B RID: 6923
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B0C RID: 6924
			private TaskAwaiter <>u__1;

			// Token: 0x04001B0D RID: 6925
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x02000484 RID: 1156
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002F30 RID: 12080 RVA: 0x0020F4DE File Offset: 0x0020D6DE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002F31 RID: 12081 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002F32 RID: 12082 RVA: 0x0020F4EC File Offset: 0x0020D6EC
			internal bool <RequestProducts>b__28_1(string uri, string input)
			{
				bool flag;
				try
				{
					if (string.IsNullOrEmpty(input) || input == "[]")
					{
						throw new ArgumentException("input");
					}
					JsonConvert.DeserializeObject<List<string>>(input);
					flag = true;
				}
				catch (Exception)
				{
					flag = false;
				}
				return flag;
			}

			// Token: 0x04001B0E RID: 6926
			public static readonly InAppPurchasePageV2.<>c <>9 = new InAppPurchasePageV2.<>c();

			// Token: 0x04001B0F RID: 6927
			public static Func<string, string, bool> <>9__28_1;
		}

		// Token: 0x02000485 RID: 1157
		[CompilerGenerated]
		private sealed class <>c__DisplayClass30_0
		{
			// Token: 0x06002F33 RID: 12083 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass30_0()
			{
			}

			// Token: 0x06002F34 RID: 12084 RVA: 0x0020F53C File Offset: 0x0020D73C
			internal async Task <SetRusPurchase>b__0()
			{
				try
				{
					TaskAwaiter<string> taskAwaiter = HttpDownloader.Get(new string[]
					{
						"https://node2.carscanner.info/droidruspurchasehint/" + App.Version,
						"https://node3.carscanner.info/droidruspurchasehint/" + App.Version
					}, 15, null, true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
					}
					if (taskAwaiter.GetResult() == "1")
					{
						this.showButton = true;
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x06002F35 RID: 12085 RVA: 0x0020F580 File Offset: 0x0020D780
			internal bool <SetRusPurchase>b__1()
			{
				return this.<>4__this.btnDroidRusPurchase.IsVisible = this.showButton;
			}

			// Token: 0x04001B10 RID: 6928
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B11 RID: 6929
			public bool showButton;

			// Token: 0x02000486 RID: 1158
			[StructLayout(LayoutKind.Auto)]
			private struct <<SetRusPurchase>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06002F36 RID: 12086 RVA: 0x0020F5A8 File Offset: 0x0020D7A8
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					InAppPurchasePageV2.<>c__DisplayClass30_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<string> taskAwaiter3;
							if (num != 0)
							{
								taskAwaiter3 = HttpDownloader.Get(new string[]
								{
									"https://node2.carscanner.info/droidruspurchasehint/" + App.Version,
									"https://node3.carscanner.info/droidruspurchasehint/" + App.Version
								}, 15, null, true).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageV2.<>c__DisplayClass30_0.<<SetRusPurchase>b__0>d>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
							}
							if (taskAwaiter3.GetResult() == "1")
							{
								CS$<>8__locals1.showButton = true;
							}
						}
						catch (Exception)
						{
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

				// Token: 0x06002F37 RID: 12087 RVA: 0x0020F6B0 File Offset: 0x0020D8B0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001B12 RID: 6930
				public int <>1__state;

				// Token: 0x04001B13 RID: 6931
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04001B14 RID: 6932
				public InAppPurchasePageV2.<>c__DisplayClass30_0 <>4__this;

				// Token: 0x04001B15 RID: 6933
				private TaskAwaiter<string> <>u__1;
			}
		}

		// Token: 0x02000487 RID: 1159
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_0
		{
			// Token: 0x06002F38 RID: 12088 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_0()
			{
			}

			// Token: 0x06002F39 RID: 12089 RVA: 0x0020F6BE File Offset: 0x0020D8BE
			internal bool <OnProductsReceived>b__0(string x)
			{
				return x == this.p.ProductID;
			}

			// Token: 0x04001B16 RID: 6934
			public IProduct p;
		}

		// Token: 0x02000488 RID: 1160
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x06002F3A RID: 12090 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x06002F3B RID: 12091 RVA: 0x0020F6D4 File Offset: 0x0020D8D4
			internal void <LoadProductsRus>b__1()
			{
				this.<>4__this.panelNewStylePurchaseButtonsRus.Children.Clear();
				for (int i = 0; i < this.prices.Length; i++)
				{
					string text = this.prices[i];
					string text2 = string.Format(this.purchaseBtnLabel, text);
					Button button = new Button
					{
						Style = (Style)this.<>4__this.Resources["PurchaseButtonStyle"],
						ClassId = (i + 1).ToString(),
						Text = text2,
						IsEnabled = true
					};
					button.Clicked += this.<>4__this.btnPurchaseRus_Clicked;
					this.<>4__this.panelNewStylePurchaseButtonsRus.Children.Add(button);
				}
				this.<>4__this.lbPurchaseDetailsShort.Text = this.purchaseDetailsShort;
				this.<>4__this.activityFrame.IsVisible = false;
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			}

			// Token: 0x04001B17 RID: 6935
			public string[] prices;

			// Token: 0x04001B18 RID: 6936
			public string purchaseBtnLabel;

			// Token: 0x04001B19 RID: 6937
			public string purchaseDetailsShort;

			// Token: 0x04001B1A RID: 6938
			public InAppPurchasePageV2 <>4__this;
		}

		// Token: 0x02000489 RID: 1161
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsCSRusPurchaseAllowed>d__29 : IAsyncStateMachine
		{
			// Token: 0x06002F3C RID: 12092 RVA: 0x0020F7D4 File Offset: 0x0020D9D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					if (num != 0)
					{
						result = false;
					}
					try
					{
						TaskAwaiter<string> taskAwaiter3;
						if (num != 0)
						{
							taskAwaiter3 = HttpDownloader.Get(new string[]
							{
								"https://node2.carscanner.info/droidruspurchasehint/" + App.Version,
								"https://node3.carscanner.info/droidruspurchasehint/" + App.Version
							}, 5, null, true).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageV2.<CheckIsCSRusPurchaseAllowed>d__29>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
						}
						if (taskAwaiter3.GetResult() == "1")
						{
							result = true;
						}
					}
					catch (Exception)
					{
					}
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002F3D RID: 12093 RVA: 0x0020F8E4 File Offset: 0x0020DAE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B1B RID: 6939
			public int <>1__state;

			// Token: 0x04001B1C RID: 6940
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B1D RID: 6941
			private bool <result>5__2;

			// Token: 0x04001B1E RID: 6942
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x0200048A RID: 1162
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsRusAndSetInterface>d__39 : IAsyncStateMachine
		{
			// Token: 0x06002F3E RID: 12094 RVA: 0x0020F8F4 File Offset: 0x0020DAF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				bool flag3;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<bool?> taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter<bool?> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<bool?>);
						num2 = -1;
						goto IL_00FA;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0164;
					default:
						if (!RuDetector.IsRuLanguage() || !RuDetector.IsRuLocale())
						{
							goto IL_0186;
						}
						taskAwaiter3 = inAppPurchasePageV.CheckIsCSRusPurchaseAllowed().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<CheckIsRusAndSetInterface>d__39>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0186;
					}
					bool flag = SharedSettings.Current.DroidGPCheckCurrencyR;
					if (!flag)
					{
						goto IL_010C;
					}
					taskAwaiter4 = RuDetector.IsRuCurrency().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool?> taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool?>, InAppPurchasePageV2.<CheckIsRusAndSetInterface>d__39>(ref taskAwaiter4, ref this);
						return;
					}
					IL_00FA:
					flag = taskAwaiter4.GetResult().GetValueOrDefault();
					IL_010C:
					bool flag2 = flag;
					if (flag2)
					{
						goto IL_016D;
					}
					taskAwaiter3 = RuDetector.IsRuIP().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<CheckIsRusAndSetInterface>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0164:
					flag2 = taskAwaiter3.GetResult();
					IL_016D:
					if (flag2)
					{
						MainThreadHelper.InvokeOnMainThread(delegate
						{
							InAppPurchasePageV2.<<CheckIsRusAndSetInterface>b__39_0>d <<CheckIsRusAndSetInterface>b__39_0>d;
							<<CheckIsRusAndSetInterface>b__39_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<CheckIsRusAndSetInterface>b__39_0>d.<>4__this = inAppPurchasePageV;
							<<CheckIsRusAndSetInterface>b__39_0>d.<>1__state = -1;
							<<CheckIsRusAndSetInterface>b__39_0>d.<>t__builder.Start<InAppPurchasePageV2.<<CheckIsRusAndSetInterface>b__39_0>d>(ref <<CheckIsRusAndSetInterface>b__39_0>d);
						});
						flag3 = true;
						goto IL_01A3;
					}
					IL_0186:
					flag3 = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01A3:
				num2 = -2;
				this.<>t__builder.SetResult(flag3);
			}

			// Token: 0x06002F3F RID: 12095 RVA: 0x0020FAD4 File Offset: 0x0020DCD4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B1F RID: 6943
			public int <>1__state;

			// Token: 0x04001B20 RID: 6944
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001B21 RID: 6945
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B22 RID: 6946
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B23 RID: 6947
			private TaskAwaiter<bool?> <>u__2;
		}

		// Token: 0x0200048B RID: 1163
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Appearing>d__4 : IAsyncStateMachine
		{
			// Token: 0x06002F40 RID: 12096 RVA: 0x0020FAE4 File Offset: 0x0020DCE4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					bool flag;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0146;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C1;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_021F;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_029A;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_030F;
					}
					default:
						if (inAppPurchasePageV.WasLoaded)
						{
							goto IL_0316;
						}
						inAppPurchasePageV.activityFrame.IsVisible = true;
						switch (inAppPurchasePageV.Platform)
						{
						case InAppPurchasePageV2.Platforms.DroidGP:
							flag = inAppPurchasePageV.forceNoRussia;
							if (flag)
							{
								goto IL_00D1;
							}
							taskAwaiter3 = inAppPurchasePageV.CheckIsRusAndSetInterface().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						case InAppPurchasePageV2.Platforms.DroidHuawei:
							inAppPurchasePageV.contentStack.IsVisible = true;
							inAppPurchasePageV.rusContentStack.IsVisible = false;
							taskAwaiter4 = inAppPurchasePageV.RequestProducts().GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_029A;
						case InAppPurchasePageV2.Platforms.iOS:
							inAppPurchasePageV.contentStack.IsVisible = true;
							inAppPurchasePageV.rusContentStack.IsVisible = false;
							taskAwaiter4 = inAppPurchasePageV.RequestProducts().GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_01C1;
						case InAppPurchasePageV2.Platforms.RuStore:
							inAppPurchasePageV.contentStack.IsVisible = true;
							inAppPurchasePageV.rusContentStack.IsVisible = false;
							taskAwaiter4 = inAppPurchasePageV.RequestProducts().GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 5;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_030F;
						default:
							goto IL_0316;
						}
						break;
					}
					flag = !taskAwaiter3.GetResult();
					IL_00D1:
					if (!flag)
					{
						goto IL_0316;
					}
					inAppPurchasePageV.contentStack.IsVisible = true;
					inAppPurchasePageV.rusContentStack.IsVisible = false;
					taskAwaiter4 = inAppPurchasePageV.RequestProducts().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0146:
					taskAwaiter4.GetResult();
					goto IL_0316;
					IL_01C1:
					taskAwaiter4.GetResult();
					taskAwaiter4 = inAppPurchasePageV.SetRusPurchase().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Handle_Appearing>d__4>(ref taskAwaiter4, ref this);
						return;
					}
					IL_021F:
					taskAwaiter4.GetResult();
					goto IL_0316;
					IL_029A:
					taskAwaiter4.GetResult();
					goto IL_0316;
					IL_030F:
					taskAwaiter4.GetResult();
					IL_0316:;
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

			// Token: 0x06002F41 RID: 12097 RVA: 0x0020FE54 File Offset: 0x0020E054
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B24 RID: 6948
			public int <>1__state;

			// Token: 0x04001B25 RID: 6949
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B26 RID: 6950
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B27 RID: 6951
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B28 RID: 6952
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200048C RID: 1164
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadProductsRus>d__40 : IAsyncStateMachine
		{
			// Token: 0x06002F42 RID: 12098 RVA: 0x0020FE64 File Offset: 0x0020E064
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid || (PlatformHelper.AppMarket != Markets.GooglePlay && PlatformHelper.AppMarket != Markets.RUS))
						{
							goto IL_008B;
						}
						taskAwaiter = Task.Run(delegate
						{
							InAppPurchasePageV2.<<LoadProductsRus>b__40_0>d <<LoadProductsRus>b__40_0>d;
							<<LoadProductsRus>b__40_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<LoadProductsRus>b__40_0>d.<>4__this = inAppPurchasePageV;
							<<LoadProductsRus>b__40_0>d.<>1__state = -1;
							<<LoadProductsRus>b__40_0>d.<>t__builder.Start<InAppPurchasePageV2.<<LoadProductsRus>b__40_0>d>(ref <<LoadProductsRus>b__40_0>d);
							return <<LoadProductsRus>b__40_0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<LoadProductsRus>d__40>(ref taskAwaiter, ref this);
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
					IL_008B:;
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

			// Token: 0x06002F43 RID: 12099 RVA: 0x0020FF38 File Offset: 0x0020E138
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B29 RID: 6953
			public int <>1__state;

			// Token: 0x04001B2A RID: 6954
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B2B RID: 6955
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B2C RID: 6956
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200048D RID: 1165
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Manager_Error>d__19 : IAsyncStateMachine
		{
			// Token: 0x06002F44 RID: 12100 RVA: 0x0020FF48 File Offset: 0x0020E148
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
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
						if (PlatformHelper.IsAndroid)
						{
							text += Translate.GetString("droid_IAP_Restore");
						}
						if (PlatformHelper.AppMarket == Markets.HMS && ErrorMessage != null && ErrorMessage.Contains("60050: account not logged in"))
						{
							text = Translate.GetString("huawei_NotLoggedIn") + "\n" + text;
						}
						if (PlatformHelper.IsAndroid)
						{
							if (PlatformHelper.AppMarket == Markets.GooglePlay)
							{
								if (inAppPurchasePageV.ruDetected && !inAppPurchasePageV.forceNoRussia)
								{
									text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "Google Play");
								}
							}
							else if (PlatformHelper.AppMarket == Markets.Rustore)
							{
								text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "RuStore");
							}
							else if (PlatformHelper.AppMarket == Markets.HMS && inAppPurchasePageV.ruDetected && !inAppPurchasePageV.forceNoRussia)
							{
								text = text + "\n" + string.Format(Translate.GetString("ru_WrongVersionRestorePurchases"), "Huawei AppGallery");
							}
						}
						taskAwaiter = inAppPurchasePageV.DisplayAlert(Translate.GetString("ios_PurchaseSomethingWrongTitle"), text, "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<Manager_Error>d__19>(ref taskAwaiter, ref this);
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
					if (inAppPurchasePageV.ruDetected && !inAppPurchasePageV.forceNoRussia)
					{
						inAppPurchasePageV.rusContentStack.IsVisible = true;
					}
					else
					{
						inAppPurchasePageV.contentStack.IsVisible = true;
					}
					inAppPurchasePageV.activityFrame.IsVisible = false;
					inAppPurchasePageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

			// Token: 0x06002F45 RID: 12101 RVA: 0x00210194 File Offset: 0x0020E394
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B2D RID: 6957
			public int <>1__state;

			// Token: 0x04001B2E RID: 6958
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B2F RID: 6959
			public string ErrorMessage;

			// Token: 0x04001B30 RID: 6960
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B31 RID: 6961
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200048E RID: 1166
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnProductsReceived>d__32 : IAsyncStateMachine
		{
			// Token: 0x06002F46 RID: 12102 RVA: 0x002101A4 File Offset: 0x0020E3A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
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
							goto IL_0264;
						}
						if (_products.Count > 0)
						{
							inAppPurchasePageV.products.Clear();
							List<IProduct>.Enumerator enumerator = _products.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									InAppPurchasePageV2.<>c__DisplayClass32_0 CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass32_0();
									CS$<>8__locals1.p = enumerator.Current;
									if (inAppPurchasePageV.product_ids.Any((string x) => x == CS$<>8__locals1.p.ProductID))
									{
										inAppPurchasePageV.products.Add(CS$<>8__locals1.p);
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
							inAppPurchasePageV.CreatePurchaseButtons(_products);
							inAppPurchasePageV.WasLoaded = true;
							inAppPurchasePageV.contentStack.IsVisible = true;
							inAppPurchasePageV.activityFrame.IsVisible = false;
							inAppPurchasePageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							if (inAppPurchasePageV.RestoreOnAppear)
							{
								inAppPurchasePageV.RestoreOnAppear = false;
								inAppPurchasePageV.btnRestore_Clicked(inAppPurchasePageV.btnRestore2, EventArgs.Empty);
								goto IL_026C;
							}
							goto IL_026C;
						}
						else
						{
							string text = Translate.GetString("ios_PurchaseCantGetProductsTitle");
							text = string.Format(text, inAppPurchasePageV.GetStoreName());
							if (PlatformHelper.AppMarket == Markets.HMS && DebugString != null && DebugString.Contains("60050: account not logged in"))
							{
								text = Translate.GetString("huawei_NotLoggedIn") + "\n" + text;
							}
							if (PlatformHelper.AppMarket == Markets.Rustore && DebugString != null && DebugString.Contains("RuStore User Not Authorized", StringComparison.OrdinalIgnoreCase))
							{
								DebugString = "Сначала зайдите в свою учетную запись (авторизуйтесь) в приложении RuStore!";
							}
							inAppPurchasePageV.WasLoaded = false;
							taskAwaiter3 = inAppPurchasePageV.DisplayAlert(text, Translate.GetString("ios_PurchaseCantGetProductsText") + "\n" + DebugString, "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<OnProductsReceived>d__32>(ref taskAwaiter3, ref this);
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
					taskAwaiter = inAppPurchasePageV.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InAppPurchasePageV2.<OnProductsReceived>d__32>(ref taskAwaiter, ref this);
						return;
					}
					IL_0264:
					taskAwaiter.GetResult();
					IL_026C:;
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

			// Token: 0x06002F47 RID: 12103 RVA: 0x00210480 File Offset: 0x0020E680
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B32 RID: 6962
			public int <>1__state;

			// Token: 0x04001B33 RID: 6963
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B34 RID: 6964
			public List<IProduct> _products;

			// Token: 0x04001B35 RID: 6965
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B36 RID: 6966
			public string DebugString;

			// Token: 0x04001B37 RID: 6967
			private TaskAwaiter <>u__1;

			// Token: 0x04001B38 RID: 6968
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x0200048F RID: 1167
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PurchaseBtn_Clicked>d__36 : IAsyncStateMachine
		{
			// Token: 0x06002F48 RID: 12104 RVA: 0x00210490 File Offset: 0x0020E690
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						Button button = (Button)sender;
						productId = button.ClassId;
						if (!SharedSettings.Current.AdsProductPurchased || !SharedSettings.Current.WhitelistDeviceActivated)
						{
							goto IL_00CE;
						}
						taskAwaiter3 = inAppPurchasePageV.DisplayAlert("Please confirm Car Scanner Pro purchase", string.Format("Hello! You already have all Pro version features, because you're using {0} device.\nYou can purchase Pro version if you want to support developer or if you want to us all features with another device.\nDo you want to purchase Car Scanner Pro?", SharedSettings.Current.BTLEDeviceName), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<PurchaseBtn_Clicked>d__36>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_00FC;
					}
					IL_00CE:
					inAppPurchasePageV.PurchaseWithProduct(productId);
				}
				catch (Exception ex)
				{
					num2 = -2;
					productId = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00FC:
				num2 = -2;
				productId = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002F49 RID: 12105 RVA: 0x002105C4 File Offset: 0x0020E7C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B39 RID: 6969
			public int <>1__state;

			// Token: 0x04001B3A RID: 6970
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B3B RID: 6971
			public object sender;

			// Token: 0x04001B3C RID: 6972
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B3D RID: 6973
			private string <productId>5__2;

			// Token: 0x04001B3E RID: 6974
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000490 RID: 1168
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PurchaseWithProduct>d__37 : IAsyncStateMachine
		{
			// Token: 0x06002F4A RID: 12106 RVA: 0x002105D4 File Offset: 0x0020E7D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
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
							goto IL_0121;
						}
						taskAwaiter5 = inAppPurchasePageV.manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<PurchaseWithProduct>d__37>(ref taskAwaiter5, ref this);
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
						inAppPurchasePageV.LastUserAction = InAppPurchasePageV2.UserActions.StartPurchase;
						inAppPurchasePageV.contentStack.IsVisible = false;
						inAppPurchasePageV.activityFrame.IsVisible = true;
						inAppPurchasePageV.manager.Purchase(productId);
						goto IL_0175;
					}
					string text = string.Format(Translate.GetString("ios_StoreUnavailable_Title"), inAppPurchasePageV.GetStoreName());
					taskAwaiter3 = inAppPurchasePageV.DisplayAlert(text, PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<PurchaseWithProduct>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0121:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0175:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002F4B RID: 12107 RVA: 0x00210788 File Offset: 0x0020E988
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B3F RID: 6975
			public int <>1__state;

			// Token: 0x04001B40 RID: 6976
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B41 RID: 6977
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B42 RID: 6978
			public string productId;

			// Token: 0x04001B43 RID: 6979
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B44 RID: 6980
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000491 RID: 1169
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestProducts>d__28 : IAsyncStateMachine
		{
			// Token: 0x06002F4C RID: 12108 RVA: 0x00210798 File Offset: 0x0020E998
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						inAppPurchasePageV.product_ids.Clear();
						taskAwaiter = Task.Run(delegate
						{
							InAppPurchasePageV2.<<RequestProducts>b__28_0>d <<RequestProducts>b__28_0>d;
							<<RequestProducts>b__28_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<RequestProducts>b__28_0>d.<>4__this = inAppPurchasePageV;
							<<RequestProducts>b__28_0>d.<>1__state = -1;
							<<RequestProducts>b__28_0>d.<>t__builder.Start<InAppPurchasePageV2.<<RequestProducts>b__28_0>d>(ref <<RequestProducts>b__28_0>d);
							return <<RequestProducts>b__28_0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<RequestProducts>d__28>(ref taskAwaiter, ref this);
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
					inAppPurchasePageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + string.Format("Loading prices... ({0})", inAppPurchasePageV.product_ids.Count);
					inAppPurchasePageV.manager.RequestProductData(inAppPurchasePageV.product_ids);
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

			// Token: 0x06002F4D RID: 12109 RVA: 0x002108A8 File Offset: 0x0020EAA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B45 RID: 6981
			public int <>1__state;

			// Token: 0x04001B46 RID: 6982
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B47 RID: 6983
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B48 RID: 6984
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000492 RID: 1170
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestRating>d__23 : IAsyncStateMachine
		{
			// Token: 0x06002F4E RID: 12110 RVA: 0x002108B8 File Offset: 0x0020EAB8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					bool flag;
					bool flag2;
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), inAppPurchasePageV.GetStoreName());
						flag = !inAppPurchasePageV.rating_requested;
						if (!flag)
						{
							goto IL_00C9;
						}
						flag2 = Device.RuntimePlatform == "iOS";
						if (flag2)
						{
							goto IL_00C6;
						}
						taskAwaiter = inAppPurchasePageV.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<RequestRating>d__23>(ref taskAwaiter, ref this);
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
					IL_00C6:
					flag = flag2;
					IL_00C9:
					if (flag)
					{
						inAppPurchasePageV.rating_requested = true;
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

			// Token: 0x06002F4F RID: 12111 RVA: 0x002109E4 File Offset: 0x0020EBE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B49 RID: 6985
			public int <>1__state;

			// Token: 0x04001B4A RID: 6986
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B4B RID: 6987
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B4C RID: 6988
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000493 RID: 1171
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetRusPurchase>d__30 : IAsyncStateMachine
		{
			// Token: 0x06002F50 RID: 12112 RVA: 0x002109F4 File Offset: 0x0020EBF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					bool flag;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_015F;
						}
						CS$<>8__locals1 = new InAppPurchasePageV2.<>c__DisplayClass30_0();
						CS$<>8__locals1.<>4__this = this;
						if (!PlatformHelper.IsiOS)
						{
							goto IL_00B8;
						}
						flag = RuDetector.IsRuLanguage() && RuDetector.IsRuLocale();
						if (!flag)
						{
							goto IL_00A9;
						}
						taskAwaiter3 = RuDetector.IsRuIP().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<SetRusPurchase>d__30>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					flag = taskAwaiter3.GetResult();
					IL_00A9:
					if (flag)
					{
						inAppPurchasePageV.lbiOSRusHint.IsVisible = true;
					}
					IL_00B8:
					if (!PlatformHelper.IsAndroid || !(PlatformHelper.DroidService.Resources_Configuration_Locale_Country == "RU") || !(App.CurrentLanguageCode == "ru"))
					{
						goto IL_0185;
					}
					CS$<>8__locals1.showButton = false;
					taskAwaiter = Task.Run(delegate
					{
						InAppPurchasePageV2.<>c__DisplayClass30_0.<<SetRusPurchase>b__0>d <<SetRusPurchase>b__0>d;
						<<SetRusPurchase>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<SetRusPurchase>b__0>d.<>4__this = CS$<>8__locals1;
						<<SetRusPurchase>b__0>d.<>1__state = -1;
						<<SetRusPurchase>b__0>d.<>t__builder.Start<InAppPurchasePageV2.<>c__DisplayClass30_0.<<SetRusPurchase>b__0>d>(ref <<SetRusPurchase>b__0>d);
						return <<SetRusPurchase>b__0>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<SetRusPurchase>d__30>(ref taskAwaiter, ref this);
						return;
					}
					IL_015F:
					taskAwaiter.GetResult();
					if (PlatformHelper.AppMarket == Markets.HMS)
					{
						MainThread.InvokeOnMainThreadAsync<bool>(() => CS$<>8__locals1.<>4__this.btnDroidRusPurchase.IsVisible = CS$<>8__locals1.showButton);
					}
					IL_0185:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002F51 RID: 12113 RVA: 0x00210BE0 File Offset: 0x0020EDE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B4D RID: 6989
			public int <>1__state;

			// Token: 0x04001B4E RID: 6990
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001B4F RID: 6991
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B50 RID: 6992
			private InAppPurchasePageV2.<>c__DisplayClass30_0 <>8__1;

			// Token: 0x04001B51 RID: 6993
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B52 RID: 6994
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000494 RID: 1172
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Settings_PropertyChanged>d__21 : IAsyncStateMachine
		{
			// Token: 0x06002F52 RID: 12114 RVA: 0x00210BF0 File Offset: 0x0020EDF0
			void IAsyncStateMachine.MoveNext()
			{
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					if (e.PropertyName == "AdsProductPurchased" || e.PropertyName == "LicenseFinishDate")
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							InAppPurchasePageV2.<<Settings_PropertyChanged>b__21_0>d <<Settings_PropertyChanged>b__21_0>d;
							<<Settings_PropertyChanged>b__21_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<Settings_PropertyChanged>b__21_0>d.<>4__this = inAppPurchasePageV;
							<<Settings_PropertyChanged>b__21_0>d.<>1__state = -1;
							<<Settings_PropertyChanged>b__21_0>d.<>t__builder.Start<InAppPurchasePageV2.<<Settings_PropertyChanged>b__21_0>d>(ref <<Settings_PropertyChanged>b__21_0>d);
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

			// Token: 0x06002F53 RID: 12115 RVA: 0x00210C80 File Offset: 0x0020EE80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B53 RID: 6995
			public int <>1__state;

			// Token: 0x04001B54 RID: 6996
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B55 RID: 6997
			public PropertyChangedEventArgs e;

			// Token: 0x04001B56 RID: 6998
			public InAppPurchasePageV2 <>4__this;
		}

		// Token: 0x02000495 RID: 1173
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnNotRus_Clicked>d__34 : IAsyncStateMachine
		{
			// Token: 0x06002F54 RID: 12116 RVA: 0x00210C90 File Offset: 0x0020EE90
			void IAsyncStateMachine.MoveNext()
			{
				InAppPurchasePageV2 inAppPurchasePageV = this;
				try
				{
					inAppPurchasePageV.WasLoaded = false;
					inAppPurchasePageV.forceNoRussia = true;
					inAppPurchasePageV.Handle_Appearing(inAppPurchasePageV, EventArgs.Empty);
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

			// Token: 0x06002F55 RID: 12117 RVA: 0x00210CFC File Offset: 0x0020EEFC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B57 RID: 6999
			public int <>1__state;

			// Token: 0x04001B58 RID: 7000
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B59 RID: 7001
			public InAppPurchasePageV2 <>4__this;
		}

		// Token: 0x02000496 RID: 1174
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPurchaseRus_Clicked>d__44 : IAsyncStateMachine
		{
			// Token: 0x06002F56 RID: 12118 RVA: 0x00210D0C File Offset: 0x0020EF0C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
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
							goto IL_00D9;
						}
						taskAwaiter5 = Launcher.TryOpenAsync("https://ru.carscanner.info/buy/").GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<btnPurchaseRus_Clicked>d__44>(ref taskAwaiter5, ref this);
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
						goto IL_00E0;
					}
					taskAwaiter3 = inAppPurchasePageV.DisplayAlert("Ошибка!", "Ошибка при попытке запустить браузер для оплаты.\nПожалуйста, убедитесь, что у вас установлен браузер.", "ОК").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<btnPurchaseRus_Clicked>d__44>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00D9:
					taskAwaiter3.GetResult();
					IL_00E0:;
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

			// Token: 0x06002F57 RID: 12119 RVA: 0x00210E38 File Offset: 0x0020F038
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B5A RID: 7002
			public int <>1__state;

			// Token: 0x04001B5B RID: 7003
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B5C RID: 7004
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B5D RID: 7005
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B5E RID: 7006
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000497 RID: 1175
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestoreRus_Clicked>d__45 : IAsyncStateMachine
		{
			// Token: 0x06002F58 RID: 12120 RVA: 0x00210E48 File Offset: 0x0020F048
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06002F59 RID: 12121 RVA: 0x00210E94 File Offset: 0x0020F094
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B5F RID: 7007
			public int <>1__state;

			// Token: 0x04001B60 RID: 7008
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x02000498 RID: 1176
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__33 : IAsyncStateMachine
		{
			// Token: 0x06002F5A RID: 12122 RVA: 0x00210EA4 File Offset: 0x0020F0A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageV2 inAppPurchasePageV = this;
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
							goto IL_0136;
						}
						inAppPurchasePageV.contentStack.IsVisible = false;
						inAppPurchasePageV.activityFrame.IsVisible = true;
						taskAwaiter5 = inAppPurchasePageV.manager.CanMakePayments().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageV2.<btnRestore_Clicked>d__33>(ref taskAwaiter5, ref this);
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
						inAppPurchasePageV.LastUserAction = InAppPurchasePageV2.UserActions.RestorePurchases;
						inAppPurchasePageV.manager.Restore();
						goto IL_016C;
					}
					string text = string.Format(Translate.GetString("ios_StoreUnavailable_Title"), inAppPurchasePageV.GetStoreName());
					taskAwaiter3 = inAppPurchasePageV.DisplayAlert(text, PlatformHelper.IsiOS ? (Translate.GetString("ios_StoreUnavailable_Text") + "\n" + Translate.GetString("ios_IAP_Restrictions")) : Translate.GetString("ios_StoreUnavailable_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageV2.<btnRestore_Clicked>d__33>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0136:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_016C:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002F5B RID: 12123 RVA: 0x0021104C File Offset: 0x0020F24C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001B61 RID: 7009
			public int <>1__state;

			// Token: 0x04001B62 RID: 7010
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001B63 RID: 7011
			public InAppPurchasePageV2 <>4__this;

			// Token: 0x04001B64 RID: 7012
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001B65 RID: 7013
			private TaskAwaiter <>u__2;
		}
	}
}
