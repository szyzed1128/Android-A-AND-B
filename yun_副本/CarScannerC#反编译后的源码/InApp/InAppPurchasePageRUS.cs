using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
	// Token: 0x02000473 RID: 1139
	[XamlCompilation(2)]
	[XamlFilePath("InApp\\InAppPurchasePageRUS.xaml")]
	public class InAppPurchasePageRUS : ContentPage
	{
		// Token: 0x06002EE4 RID: 12004 RVA: 0x0020779E File Offset: 0x0020599E
		public InAppPurchasePageRUS()
		{
			this.InitializeComponent();
			this.LoadProducts();
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x002077B4 File Offset: 0x002059B4
		private void EulaSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			foreach (View view in this.panelNewStylePurchaseButtons.Children)
			{
				view.IsEnabled = this.EulaSwitch.IsToggled;
			}
			this.btnRestore.IsEnabled = this.EulaSwitch.IsToggled;
		}

		// Token: 0x06002EE6 RID: 12006 RVA: 0x00207824 File Offset: 0x00205A24
		private async void btnPurchase_Clicked(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			btn.IsEnabled = false;
			string price_level = btn.ClassId;
			string text = await base.DisplayPromptAsync(Translate.GetString("rus_PurchaseRequestTitle"), Translate.GetString("rus_PurchaseRequestText"), "OK", "Отмена", null, -1, Keyboard.Email, "");
			string email = text;
			if (email != null)
			{
				email = email.Trim();
				if (this.IsValidEmail(email))
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("rus_ConfirmEmailTitle"), string.Format(Translate.GetString("rus_ConfirmEmailText"), email), "Да", "Нет").GetAwaiter();
					TaskAwaiter<bool> taskAwaiter2;
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						taskAwaiter = Launcher.TryOpenAsync("https://ru.carscanner.info/purchase/?action=purchasev2&" + ("client_email=" + email + "&price_level=" + price_level)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							await base.DisplayAlert("Ошибка!", "Ошибка при попытке запустить браузер для оплаты.\nПожалуйста, убедитесь, что у вас установлен браузер.", "ОК");
						}
					}
				}
				else
				{
					await base.DisplayAlert("Ошибка!", Translate.GetString("rus_WrongEmailFormatText"), "OK");
				}
			}
			btn.IsEnabled = true;
		}

		// Token: 0x06002EE7 RID: 12007 RVA: 0x00207864 File Offset: 0x00205A64
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

		// Token: 0x06002EE8 RID: 12008 RVA: 0x002078B4 File Offset: 0x00205AB4
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

		// Token: 0x06002EE9 RID: 12009 RVA: 0x00207908 File Offset: 0x00205B08
		private async void btnRestore_Clicked(object sender, EventArgs e)
		{
			try
			{
				string text = await base.DisplayPromptAsync(Translate.GetString("rus_RestoreKeyTitle"), Translate.GetString("rus_RestoreKeyText"), "OK", "Отмена", null, -1, null, "");
				if (text != null)
				{
					if (text.Length == 5)
					{
						if (text.All((char x) => char.IsDigit(x)))
						{
							await App.GetCurrentPage().DisplayAlert("Это не ключ!", "Вы ввели код для подтверждения электронной почты. ЭТО НЕ КЛЮЧ АКТИВАЦИИ!\nКлюч активации отправляется автоматически после оплаты отдельным письмом с адреса admin@carscanner.info\nЕсли вы не получили письмо с ключом, то проверьте в вашей почте папки \"Спам\" и \"Нежелательная почта\", скорей всего письмо в одной из этих папок.\nЕсли письма нет и в этих папках, то напишите мне на почту admin@carscanner.info с той почты, которую вы указали при покупке и я продублирую вам ключ в ручном режиме.", "OK");
							return;
						}
					}
					text = text.Replace('O', '0');
					string text2 = this.filterKeyForWrongSymbols(text);
					ValueTuple<ActivationRequestResult, int> valueTuple = await PlatformHelper.DroidService.RuKeyActivator_CheckKeyAsync(text2, true, "");
					ActivationRequestResult item = valueTuple.Item1;
					int item2 = valueTuple.Item2;
					switch (item)
					{
					case ActivationRequestResult.NotValid:
						if (item2 == 0)
						{
							await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorKeyNoActivationsLeft"), "OK");
						}
						else
						{
							await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorKeyNotValid"), "OK");
						}
						break;
					case ActivationRequestResult.Valid:
						await App.GetCurrentPage().DisplayAlert("Успешно!", string.Format("Благодарим вас за покупку {0}!\nОставшееся количество активаций: {1}", App.AppTitle, item2), "OK");
						this.btnBack_Clicked(sender, e);
						break;
					case ActivationRequestResult.ConnectionFail:
						await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK");
						break;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x00207950 File Offset: 0x00205B50
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			try
			{
				if (App.GetCurrentPage() == this)
				{
					await base.Navigation.PopAsync();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x00207988 File Offset: 0x00205B88
		private async Task LoadProducts()
		{
			await Task.Run(async delegate
			{
				try
				{
					CS$<>8__locals1 = new InAppPurchasePageRUS.<>c__DisplayClass7_0();
					CS$<>8__locals1.<>4__this = this;
					string text = "https://node2.carscanner.info/p/ru.json";
					string text2 = "https://node3.carscanner.info/p/ru.json";
					Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(await HttpDownloader.Get(new string[] { text, text2 }, 15, null, true));
					string text3 = dictionary["price"];
					CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
					CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
					string text4 = dictionary["price_list"];
					CS$<>8__locals1.prices = text4.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					MainThread.BeginInvokeOnMainThread(delegate
					{
						CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Clear();
						for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
						{
							string text5 = CS$<>8__locals1.prices[i];
							string text6 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text5);
							Button button = new Button
							{
								Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
								ClassId = (i + 1).ToString(),
								Text = text6,
								IsEnabled = false
							};
							button.Clicked += CS$<>8__locals1.<>4__this.btnPurchase_Clicked;
							CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Add(button);
						}
						CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
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

		// Token: 0x06002EEC RID: 12012 RVA: 0x002079CC File Offset: 0x00205BCC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "InApp/InAppPurchasePageRUS.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 52);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Button)), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 53);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			HyperLinkLabel hyperLinkLabel;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel = new HyperLinkLabel(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 25);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 22);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 53);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 34);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 34);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 34);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 30);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("InApp\\InAppPurchasePageRUS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			nameScope.RegisterName("contentStack", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "contentStack";
			}
			nameScope.RegisterName("lbPurchaseDetailsShort", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "lbPurchaseDetailsShort";
			}
			nameScope.RegisterName("EulaSwitch", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "EulaSwitch";
			}
			nameScope.RegisterName("btnRestore", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnRestore";
			}
			nameScope.RegisterName("panelNewStylePurchaseButtons", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelNewStylePurchaseButtons";
			}
			this.contentStack = stackLayout2;
			this.lbPurchaseDetailsShort = label3;
			this.EulaSwitch = labelSwitch;
			this.btnRestore = button;
			this.panelNewStylePurchaseButtons = stackLayout;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 52)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 17)));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension4.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			label.SetValue(Label.TextProperty, "Car Scanner Rus Pro");
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(View.MarginProperty, new Thickness(10.0, 0.0, 10.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			stackLayout2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(Grid.RowProperty, 0);
			image.SetValue(Image.AspectProperty, 0);
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension5.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = image;
			array6[1] = stackLayout2;
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 25)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource5.Key);
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout2.Children.Add(image);
			label2.SetValue(Label.LineBreakModeProperty, 1);
			translate2.Text = "ios_CarScannerProAdvantages";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = label2;
			array7[1] = stackLayout2;
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 53)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label2.Text = obj9;
			stackLayout2.Children.Add(label2);
			label3.SetValue(Label.LineBreakModeProperty, 1);
			stackLayout2.Children.Add(label3);
			hyperLinkLabel.SetValue(HyperLinkLabel.NavigateUriProperty, "https://ru.carscanner.info/eula/");
			hyperLinkLabel.SetValue(Label.TextProperty, "Лицензионное соглашение");
			stackLayout2.Children.Add(hyperLinkLabel);
			labelSwitch.SetValue(LabelSwitch.TextProperty, "Условия лицензионного соглашения прочитаны. Оплата является подтверждением принятия условий лицензионного соглашения");
			labelSwitch.Toggled += this.EulaSwitch_Toggled;
			stackLayout2.Children.Add(labelSwitch);
			button.SetValue(Grid.RowProperty, 7);
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = button;
			array8[1] = stackLayout2;
			array8[2] = scrollView;
			array8[3] = grid2;
			array8[4] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 25)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			button.Clicked += this.btnRestore_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			button.SetValue(VisualElement.IsEnabledProperty, false);
			translate3.Text = "rus_ActivateKey";
			IMarkupExtension markupExtension9 = translate3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = button;
			array9[1] = stackLayout2;
			array9[2] = scrollView;
			array9[3] = grid2;
			array9[4] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 25)));
			object obj12 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button.Text = obj12;
			button.SetValue(Button.TextColorProperty, Color.White);
			stackLayout2.Children.Add(button);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout2.Children.Add(stackLayout);
			span.SetValue(Span.TextProperty, "ВАЖНО!");
			dynamicResourceExtension7.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = span;
			array10[1] = formattedString;
			array10[2] = label4;
			array10[3] = stackLayout2;
			array10[4] = scrollView;
			array10[5] = grid2;
			array10[6] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Span.TextColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(InAppPurchasePageRUS).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 53)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			span.SetDynamicResource(Span.TextColorProperty, dynamicResource7.Key);
			span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " ЕСЛИ ВЫ НЕ ПОЛУЧИЛИ КЛЮЧ В ТЕЧЕНИЕ 5 МИНУТ ПОСЛЕ ОПЛАТЫ:");
			formattedString.Spans.Add(span2);
			span3.SetValue(Span.TextProperty, "1) Проверьте папку «Спам» или «Нежелательная почта» в вашем почтовом ящике, в 99% случаев письмо с ключом и инструкцией по активации именно там.\n2) Если Вам так и не поступило письмо с ключом активации, просто напишите мне на адрес электронной почты admin@carscanner.info с той почты, которую вы указали при покупке.");
			formattedString.Spans.Add(span3);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout2.Children.Add(label4);
			scrollView.Content = stackLayout2;
			grid2.Children.Add(scrollView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x002091F4 File Offset: 0x002073F4
		[CompilerGenerated]
		private async Task <LoadProducts>b__7_0()
		{
			try
			{
				InAppPurchasePageRUS.<>c__DisplayClass7_0 CS$<>8__locals1 = new InAppPurchasePageRUS.<>c__DisplayClass7_0();
				CS$<>8__locals1.<>4__this = this;
				string text = "https://node2.carscanner.info/p/ru.json";
				string text2 = "https://node3.carscanner.info/p/ru.json";
				Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(await HttpDownloader.Get(new string[] { text, text2 }, 15, null, true));
				string text3 = dictionary["price"];
				CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
				CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
				string text4 = dictionary["price_list"];
				CS$<>8__locals1.prices = text4.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				MainThread.BeginInvokeOnMainThread(delegate
				{
					CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Clear();
					for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
					{
						string text5 = CS$<>8__locals1.prices[i];
						string text6 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text5);
						Button button = new Button
						{
							Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
							ClassId = (i + 1).ToString(),
							Text = text6,
							IsEnabled = false
						};
						button.Clicked += CS$<>8__locals1.<>4__this.btnPurchase_Clicked;
						CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Add(button);
					}
					CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
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

		// Token: 0x06002EEE RID: 12014 RVA: 0x00209238 File Offset: 0x00207438
		[CompilerGenerated]
		private async void <LoadProducts>b__7_2()
		{
			await base.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK");
			this.btnBack_Clicked(null, null);
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x00209270 File Offset: 0x00207470
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<InAppPurchasePageRUS>(this, typeof(InAppPurchasePageRUS));
			this.contentStack = NameScopeExtensions.FindByName<StackLayout>(this, "contentStack");
			this.lbPurchaseDetailsShort = NameScopeExtensions.FindByName<Label>(this, "lbPurchaseDetailsShort");
			this.EulaSwitch = NameScopeExtensions.FindByName<LabelSwitch>(this, "EulaSwitch");
			this.btnRestore = NameScopeExtensions.FindByName<Button>(this, "btnRestore");
			this.panelNewStylePurchaseButtons = NameScopeExtensions.FindByName<StackLayout>(this, "panelNewStylePurchaseButtons");
		}

		// Token: 0x04001AA3 RID: 6819
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout contentStack;

		// Token: 0x04001AA4 RID: 6820
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbPurchaseDetailsShort;

		// Token: 0x04001AA5 RID: 6821
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch EulaSwitch;

		// Token: 0x04001AA6 RID: 6822
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore;

		// Token: 0x04001AA7 RID: 6823
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNewStylePurchaseButtons;

		// Token: 0x02000474 RID: 1140
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<LoadProducts>b__7_0>d : IAsyncStateMachine
		{
			// Token: 0x06002EF0 RID: 12016 RVA: 0x002092E4 File Offset: 0x002074E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							CS$<>8__locals1 = new InAppPurchasePageRUS.<>c__DisplayClass7_0();
							CS$<>8__locals1.<>4__this = inAppPurchasePageRUS;
							string text = "https://node2.carscanner.info/p/ru.json";
							string text2 = "https://node3.carscanner.info/p/ru.json";
							taskAwaiter = HttpDownloader.Get(new string[] { text, text2 }, 15, null, true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageRUS.<<LoadProducts>b__7_0>d>(ref taskAwaiter, ref this);
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
						string text3 = dictionary["price"];
						CS$<>8__locals1.purchaseDetailsShort = dictionary["details"];
						CS$<>8__locals1.purchaseBtnLabel = dictionary["purchaseButton"];
						string text4 = dictionary["price_list"];
						CS$<>8__locals1.prices = text4.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
						MainThread.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Clear();
							for (int i = 0; i < CS$<>8__locals1.prices.Length; i++)
							{
								string text5 = CS$<>8__locals1.prices[i];
								string text6 = string.Format(CS$<>8__locals1.purchaseBtnLabel, text5);
								Button button = new Button
								{
									Style = (Style)CS$<>8__locals1.<>4__this.Resources["PurchaseButtonStyle"],
									ClassId = (i + 1).ToString(),
									Text = text6,
									IsEnabled = false
								};
								button.Clicked += CS$<>8__locals1.<>4__this.btnPurchase_Clicked;
								CS$<>8__locals1.<>4__this.panelNewStylePurchaseButtons.Children.Add(button);
							}
							CS$<>8__locals1.<>4__this.lbPurchaseDetailsShort.Text = CS$<>8__locals1.purchaseDetailsShort;
						});
						CS$<>8__locals1 = null;
					}
					catch (Exception)
					{
						MainThread.BeginInvokeOnMainThread(delegate
						{
							InAppPurchasePageRUS.<<LoadProducts>b__7_2>d <<LoadProducts>b__7_2>d;
							<<LoadProducts>b__7_2>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<LoadProducts>b__7_2>d.<>4__this = inAppPurchasePageRUS;
							<<LoadProducts>b__7_2>d.<>1__state = -1;
							<<LoadProducts>b__7_2>d.<>t__builder.Start<InAppPurchasePageRUS.<<LoadProducts>b__7_2>d>(ref <<LoadProducts>b__7_2>d);
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

			// Token: 0x06002EF1 RID: 12017 RVA: 0x0020949C File Offset: 0x0020769C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AA8 RID: 6824
			public int <>1__state;

			// Token: 0x04001AA9 RID: 6825
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001AAA RID: 6826
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001AAB RID: 6827
			private InAppPurchasePageRUS.<>c__DisplayClass7_0 <>8__1;

			// Token: 0x04001AAC RID: 6828
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000475 RID: 1141
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<LoadProducts>b__7_2>d : IAsyncStateMachine
		{
			// Token: 0x06002EF2 RID: 12018 RVA: 0x002094AC File Offset: 0x002076AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = inAppPurchasePageRUS.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<<LoadProducts>b__7_2>d>(ref taskAwaiter, ref this);
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
					inAppPurchasePageRUS.btnBack_Clicked(null, null);
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

			// Token: 0x06002EF3 RID: 12019 RVA: 0x0020957C File Offset: 0x0020777C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AAD RID: 6829
			public int <>1__state;

			// Token: 0x04001AAE RID: 6830
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001AAF RID: 6831
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001AB0 RID: 6832
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000476 RID: 1142
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002EF4 RID: 12020 RVA: 0x0020958A File Offset: 0x0020778A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002EF5 RID: 12021 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002EF6 RID: 12022 RVA: 0x00209596 File Offset: 0x00207796
			internal bool <btnRestore_Clicked>b__5_0(char x)
			{
				return char.IsDigit(x);
			}

			// Token: 0x04001AB1 RID: 6833
			public static readonly InAppPurchasePageRUS.<>c <>9 = new InAppPurchasePageRUS.<>c();

			// Token: 0x04001AB2 RID: 6834
			public static Func<char, bool> <>9__5_0;
		}

		// Token: 0x02000477 RID: 1143
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06002EF7 RID: 12023 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06002EF8 RID: 12024 RVA: 0x002095A0 File Offset: 0x002077A0
			internal void <LoadProducts>b__1()
			{
				this.<>4__this.panelNewStylePurchaseButtons.Children.Clear();
				for (int i = 0; i < this.prices.Length; i++)
				{
					string text = this.prices[i];
					string text2 = string.Format(this.purchaseBtnLabel, text);
					Button button = new Button
					{
						Style = (Style)this.<>4__this.Resources["PurchaseButtonStyle"],
						ClassId = (i + 1).ToString(),
						Text = text2,
						IsEnabled = false
					};
					button.Clicked += this.<>4__this.btnPurchase_Clicked;
					this.<>4__this.panelNewStylePurchaseButtons.Children.Add(button);
				}
				this.<>4__this.lbPurchaseDetailsShort.Text = this.purchaseDetailsShort;
			}

			// Token: 0x04001AB3 RID: 6835
			public string[] prices;

			// Token: 0x04001AB4 RID: 6836
			public string purchaseBtnLabel;

			// Token: 0x04001AB5 RID: 6837
			public string purchaseDetailsShort;

			// Token: 0x04001AB6 RID: 6838
			public InAppPurchasePageRUS <>4__this;
		}

		// Token: 0x02000478 RID: 1144
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadProducts>d__7 : IAsyncStateMachine
		{
			// Token: 0x06002EF9 RID: 12025 RVA: 0x0020967C File Offset: 0x0020787C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Task.Run(delegate
						{
							InAppPurchasePageRUS.<<LoadProducts>b__7_0>d <<LoadProducts>b__7_0>d;
							<<LoadProducts>b__7_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<LoadProducts>b__7_0>d.<>4__this = inAppPurchasePageRUS;
							<<LoadProducts>b__7_0>d.<>1__state = -1;
							<<LoadProducts>b__7_0>d.<>t__builder.Start<InAppPurchasePageRUS.<<LoadProducts>b__7_0>d>(ref <<LoadProducts>b__7_0>d);
							return <<LoadProducts>b__7_0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<LoadProducts>d__7>(ref taskAwaiter, ref this);
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

			// Token: 0x06002EFA RID: 12026 RVA: 0x0020973C File Offset: 0x0020793C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AB7 RID: 6839
			public int <>1__state;

			// Token: 0x04001AB8 RID: 6840
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001AB9 RID: 6841
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001ABA RID: 6842
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000479 RID: 1145
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06002EFB RID: 12027 RVA: 0x0020974C File Offset: 0x0020794C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							if (App.GetCurrentPage() != inAppPurchasePageRUS)
							{
								goto IL_007A;
							}
							taskAwaiter = inAppPurchasePageRUS.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InAppPurchasePageRUS.<btnBack_Clicked>d__6>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						IL_007A:;
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

			// Token: 0x06002EFC RID: 12028 RVA: 0x00209820 File Offset: 0x00207A20
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ABB RID: 6843
			public int <>1__state;

			// Token: 0x04001ABC RID: 6844
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001ABD RID: 6845
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001ABE RID: 6846
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x0200047A RID: 1146
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPurchase_Clicked>d__2 : IAsyncStateMachine
		{
			// Token: 0x06002EFD RID: 12029 RVA: 0x00209830 File Offset: 0x00207A30
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					TaskAwaiter<string> taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter taskAwaiter6;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0190;
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_021F;
					case 3:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0291;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0305;
					}
					default:
						btn = (Button)sender;
						btn.IsEnabled = false;
						price_level = btn.ClassId;
						taskAwaiter3 = inAppPurchasePageRUS.DisplayPromptAsync(Translate.GetString("rus_PurchaseRequestTitle"), Translate.GetString("rus_PurchaseRequestText"), "OK", "Отмена", null, -1, Keyboard.Email, "").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageRUS.<btnPurchase_Clicked>d__2>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					string result = taskAwaiter3.GetResult();
					email = result;
					if (email == null)
					{
						goto IL_030C;
					}
					email = email.Trim();
					if (inAppPurchasePageRUS.IsValidEmail(email))
					{
						taskAwaiter5 = inAppPurchasePageRUS.DisplayAlert(Translate.GetString("rus_ConfirmEmailTitle"), string.Format(Translate.GetString("rus_ConfirmEmailText"), email), "Да", "Нет").GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 1;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageRUS.<btnPurchase_Clicked>d__2>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter6 = inAppPurchasePageRUS.DisplayAlert("Ошибка!", Translate.GetString("rus_WrongEmailFormatText"), "OK").GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnPurchase_Clicked>d__2>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0305;
					}
					IL_0190:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_030C;
					}
					string text = "client_email=" + email + "&price_level=" + price_level;
					text = "https://ru.carscanner.info/purchase/?action=purchasev2&" + text;
					taskAwaiter5 = Launcher.TryOpenAsync(text).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InAppPurchasePageRUS.<btnPurchase_Clicked>d__2>(ref taskAwaiter5, ref this);
						return;
					}
					IL_021F:
					if (taskAwaiter5.GetResult())
					{
						goto IL_030C;
					}
					taskAwaiter6 = inAppPurchasePageRUS.DisplayAlert("Ошибка!", "Ошибка при попытке запустить браузер для оплаты.\nПожалуйста, убедитесь, что у вас установлен браузер.", "ОК").GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnPurchase_Clicked>d__2>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0291:
					taskAwaiter6.GetResult();
					goto IL_030C;
					IL_0305:
					taskAwaiter6.GetResult();
					IL_030C:
					btn.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					btn = null;
					price_level = null;
					email = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				btn = null;
				price_level = null;
				email = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002EFE RID: 12030 RVA: 0x00209BCC File Offset: 0x00207DCC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001ABF RID: 6847
			public int <>1__state;

			// Token: 0x04001AC0 RID: 6848
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001AC1 RID: 6849
			public object sender;

			// Token: 0x04001AC2 RID: 6850
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001AC3 RID: 6851
			private Button <btn>5__2;

			// Token: 0x04001AC4 RID: 6852
			private string <price_level>5__3;

			// Token: 0x04001AC5 RID: 6853
			private string <email>5__4;

			// Token: 0x04001AC6 RID: 6854
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001AC7 RID: 6855
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04001AC8 RID: 6856
			private TaskAwaiter <>u__3;
		}

		// Token: 0x0200047B RID: 1147
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06002EFF RID: 12031 RVA: 0x00209BDC File Offset: 0x00207DDC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InAppPurchasePageRUS inAppPurchasePageRUS = this;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						TaskAwaiter<ValueTuple<ActivationRequestResult, int>> taskAwaiter5;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_015A;
						}
						case 2:
						{
							TaskAwaiter<ValueTuple<ActivationRequestResult, int>> taskAwaiter6;
							taskAwaiter5 = taskAwaiter6;
							taskAwaiter6 = default(TaskAwaiter<ValueTuple<ActivationRequestResult, int>>);
							num2 = -1;
							goto IL_01DD;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0276;
						}
						case 4:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_02F1;
						}
						case 5:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0368;
						}
						case 6:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_03EC;
						}
						default:
							taskAwaiter = inAppPurchasePageRUS.DisplayPromptAsync(Translate.GetString("rus_RestoreKeyTitle"), Translate.GetString("rus_RestoreKeyText"), "OK", "Отмена", null, -1, null, "").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						string text = taskAwaiter.GetResult();
						if (text == null)
						{
							goto IL_0405;
						}
						if (text.Length == 5)
						{
							if (text.All((char x) => char.IsDigit(x)))
							{
								taskAwaiter3 = App.GetCurrentPage().DisplayAlert("Это не ключ!", "Вы ввели код для подтверждения электронной почты. ЭТО НЕ КЛЮЧ АКТИВАЦИИ!\nКлюч активации отправляется автоматически после оплаты отдельным письмом с адреса admin@carscanner.info\nЕсли вы не получили письмо с ключом, то проверьте в вашей почте папки \"Спам\" и \"Нежелательная почта\", скорей всего письмо в одной из этих папок.\nЕсли письма нет и в этих папках, то напишите мне на почту admin@carscanner.info с той почты, которую вы указали при покупке и я продублирую вам ключ в ручном режиме.", "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_015A;
							}
						}
						text = text.Replace('O', '0');
						string text2 = inAppPurchasePageRUS.filterKeyForWrongSymbols(text);
						taskAwaiter5 = PlatformHelper.DroidService.RuKeyActivator_CheckKeyAsync(text2, true, "").GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<ValueTuple<ActivationRequestResult, int>> taskAwaiter6 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<ActivationRequestResult, int>>, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_01DD;
						IL_015A:
						taskAwaiter3.GetResult();
						goto IL_0425;
						IL_01DD:
						ValueTuple<ActivationRequestResult, int> result = taskAwaiter5.GetResult();
						ActivationRequestResult item = result.Item1;
						int item2 = result.Item2;
						switch (item)
						{
						case ActivationRequestResult.NotValid:
							if (item2 == 0)
							{
								taskAwaiter3 = inAppPurchasePageRUS.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorKeyNoActivationsLeft"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 4;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_02F1;
							}
							else
							{
								taskAwaiter3 = inAppPurchasePageRUS.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorKeyNotValid"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 5;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0368;
							}
							break;
						case ActivationRequestResult.Valid:
							taskAwaiter3 = App.GetCurrentPage().DisplayAlert("Успешно!", string.Format("Благодарим вас за покупку {0}!\nОставшееся количество активаций: {1}", App.AppTitle, item2), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 6;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_03EC;
						case ActivationRequestResult.ConnectionFail:
							taskAwaiter3 = inAppPurchasePageRUS.DisplayAlert("Ошибка!", Translate.GetString("rus_ErrorConnectionFail"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 3;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InAppPurchasePageRUS.<btnRestore_Clicked>d__5>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						default:
							goto IL_0405;
						}
						IL_0276:
						taskAwaiter3.GetResult();
						goto IL_0405;
						IL_02F1:
						taskAwaiter3.GetResult();
						goto IL_0405;
						IL_0368:
						taskAwaiter3.GetResult();
						goto IL_0405;
						IL_03EC:
						taskAwaiter3.GetResult();
						inAppPurchasePageRUS.btnBack_Clicked(sender, e);
						IL_0405:;
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
				IL_0425:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002F00 RID: 12032 RVA: 0x0020A058 File Offset: 0x00208258
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001AC9 RID: 6857
			public int <>1__state;

			// Token: 0x04001ACA RID: 6858
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001ACB RID: 6859
			public InAppPurchasePageRUS <>4__this;

			// Token: 0x04001ACC RID: 6860
			public object sender;

			// Token: 0x04001ACD RID: 6861
			public EventArgs e;

			// Token: 0x04001ACE RID: 6862
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001ACF RID: 6863
			private TaskAwaiter <>u__2;

			// Token: 0x04001AD0 RID: 6864
			private TaskAwaiter<ValueTuple<ActivationRequestResult, int>> <>u__3;
		}
	}
}
