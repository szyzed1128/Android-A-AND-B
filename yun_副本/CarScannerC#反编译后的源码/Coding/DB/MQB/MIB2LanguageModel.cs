using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AF2 RID: 2802
	internal class MIB2LanguageModel : ObservableCollection<LanguageItem>, INotifyPropertyChanged
	{
		// Token: 0x060057D2 RID: 22482 RVA: 0x0041DBE0 File Offset: 0x0041BDE0
		public MIB2LanguageModel(byte[] dataset280, byte[] dataset2d00)
		{
			base.Add(new LanguageItem("af_ZA", "Afrikaans (South Africa)"));
			base.Add(new LanguageItem("ar_AE", "Arabic (U.A.E.)"));
			base.Add(new LanguageItem("ar_BH", "Arabic (Bahrain)"));
			base.Add(new LanguageItem("ar_DZ", "Arabic (Algeria)"));
			base.Add(new LanguageItem("ar_EG", "Arabic (Egypt)"));
			base.Add(new LanguageItem("ar_IQ", "Arabic (Iraq)"));
			base.Add(new LanguageItem("ar_JO", "Arabic (Jordan)"));
			base.Add(new LanguageItem("ar_KW", "Arabic (Kuwait)"));
			base.Add(new LanguageItem("ar_LB", "Arabic (Lebanon)"));
			base.Add(new LanguageItem("ar_LY", "Arabic (Libya)"));
			base.Add(new LanguageItem("ar_MA", "Arabic (Morocco)"));
			base.Add(new LanguageItem("ar_OM", "Arabic (Oman)"));
			base.Add(new LanguageItem("ar_QA", "Arabic (Qatar)"));
			base.Add(new LanguageItem("ar_SA", "Arabic (Saudi Arabia)"));
			base.Add(new LanguageItem("ar_SY", "Arabic (Syria)"));
			base.Add(new LanguageItem("ar_TN", "Arabic (Tunisia)"));
			base.Add(new LanguageItem("ar_YE", "Arabic (Yemen)"));
			base.Add(new LanguageItem("az_AZ", "Azeri (Latin) (Azerbaijan)"));
			base.Add(new LanguageItem("az_AZ", "Azeri (Cyrillic) (Azerbaijan)"));
			base.Add(new LanguageItem("be_BY", "Belarusian (Belarus)"));
			base.Add(new LanguageItem("bg_BG", "Bulgarian (Bulgaria)"));
			base.Add(new LanguageItem("bs_BA", "Bosnian (Bosnia and Herzegovina)"));
			base.Add(new LanguageItem("ca_ES", "Catalan (Spain)"));
			base.Add(new LanguageItem("cs_CZ", "Czech (Czech Republic)"));
			base.Add(new LanguageItem("cy_GB", "Welsh (United Kingdom)"));
			base.Add(new LanguageItem("da_DK", "Danish (Denmark)"));
			base.Add(new LanguageItem("de_AT", "German (Austria)"));
			base.Add(new LanguageItem("de_CH", "German (Switzerland)"));
			base.Add(new LanguageItem("de_DE", "German (Germany)"));
			base.Add(new LanguageItem("de_LI", "German (Liechtenstein)"));
			base.Add(new LanguageItem("de_LU", "German (Luxembourg)"));
			base.Add(new LanguageItem("dv_MV", "Divehi (Maldives)"));
			base.Add(new LanguageItem("el_GR", "Greek (Greece)"));
			base.Add(new LanguageItem("en_AU", "English (Australia)"));
			base.Add(new LanguageItem("en_BZ", "English (Belize)"));
			base.Add(new LanguageItem("en_CA", "English (Canada)"));
			base.Add(new LanguageItem("en_CB", "English (Caribbean)"));
			base.Add(new LanguageItem("en_GB", "English (United Kingdom)"));
			base.Add(new LanguageItem("en_IE", "English (Ireland)"));
			base.Add(new LanguageItem("en_JM", "English (Jamaica)"));
			base.Add(new LanguageItem("en_NZ", "English (New Zealand)"));
			base.Add(new LanguageItem("en_PH", "English (Republic of the Philippines)"));
			base.Add(new LanguageItem("en_TT", "English (Trinidad and Tobago)"));
			base.Add(new LanguageItem("en_US", "English (United States)"));
			base.Add(new LanguageItem("en_ZA", "English (South Africa)"));
			base.Add(new LanguageItem("en_ZW", "English (Zimbabwe)"));
			base.Add(new LanguageItem("es_AR", "Spanish (Argentina)"));
			base.Add(new LanguageItem("es_BO", "Spanish (Bolivia)"));
			base.Add(new LanguageItem("es_CL", "Spanish (Chile)"));
			base.Add(new LanguageItem("es_CO", "Spanish (Colombia)"));
			base.Add(new LanguageItem("es_CR", "Spanish (Costa Rica)"));
			base.Add(new LanguageItem("es_DO", "Spanish (Dominican Republic)"));
			base.Add(new LanguageItem("es_EC", "Spanish (Ecuador)"));
			base.Add(new LanguageItem("es_ES", "Spanish (Castilian)"));
			base.Add(new LanguageItem("es_ES", "Spanish (Spain)"));
			base.Add(new LanguageItem("es_GT", "Spanish (Guatemala)"));
			base.Add(new LanguageItem("es_HN", "Spanish (Honduras)"));
			base.Add(new LanguageItem("es_MX", "Spanish (Mexico)"));
			base.Add(new LanguageItem("es_NI", "Spanish (Nicaragua)"));
			base.Add(new LanguageItem("es_PA", "Spanish (Panama)"));
			base.Add(new LanguageItem("es_PE", "Spanish (Peru)"));
			base.Add(new LanguageItem("es_PR", "Spanish (Puerto Rico)"));
			base.Add(new LanguageItem("es_PY", "Spanish (Paraguay)"));
			base.Add(new LanguageItem("es_SV", "Spanish (El Salvador)"));
			base.Add(new LanguageItem("es_UY", "Spanish (Uruguay)"));
			base.Add(new LanguageItem("es_VE", "Spanish (Venezuela)"));
			base.Add(new LanguageItem("et_EE", "Estonian (Estonia)"));
			base.Add(new LanguageItem("eu_ES", "Basque (Spain)"));
			base.Add(new LanguageItem("fa_IR", "Farsi (Iran)"));
			base.Add(new LanguageItem("fi_FI", "Finnish (Finland)"));
			base.Add(new LanguageItem("fo_FO", "Faroese (Faroe Islands)"));
			base.Add(new LanguageItem("fr_BE", "French (Belgium)"));
			base.Add(new LanguageItem("fr_CA", "French (Canada)"));
			base.Add(new LanguageItem("fr_CH", "French (Switzerland)"));
			base.Add(new LanguageItem("fr_FR", "French (France)"));
			base.Add(new LanguageItem("fr_LU", "French (Luxembourg)"));
			base.Add(new LanguageItem("fr_MC", "French (Principality of Monaco)"));
			base.Add(new LanguageItem("gl_ES", "Galician (Spain)"));
			base.Add(new LanguageItem("gu_IN", "Gujarati (India)"));
			base.Add(new LanguageItem("he_IL", "Hebrew (Israel)"));
			base.Add(new LanguageItem("hi_IN", "Hindi (India)"));
			base.Add(new LanguageItem("hr_BA", "Croatian (Bosnia and Herzegovina)"));
			base.Add(new LanguageItem("hr_HR", "Croatian (Croatia)"));
			base.Add(new LanguageItem("hu_HU", "Hungarian (Hungary)"));
			base.Add(new LanguageItem("hy_AM", "Armenian (Armenia)"));
			base.Add(new LanguageItem("id_ID", "Indonesian (Indonesia)"));
			base.Add(new LanguageItem("is_IS", "Icelandic (Iceland)"));
			base.Add(new LanguageItem("it_CH", "Italian (Switzerland)"));
			base.Add(new LanguageItem("it_IT", "Italian (Italy)"));
			base.Add(new LanguageItem("ja_JP", "Japanese (Japan)"));
			base.Add(new LanguageItem("ka_GE", "Georgian (Georgia)"));
			base.Add(new LanguageItem("kk_KZ", "Kazakh (Kazakhstan)"));
			base.Add(new LanguageItem("kn_IN", "Kannada (India)"));
			base.Add(new LanguageItem("ko_KR", "Korean (Korea)"));
			base.Add(new LanguageItem("kok_IN", "Konkani (India)"));
			base.Add(new LanguageItem("ky_KG", "Kyrgyz (Kyrgyzstan)"));
			base.Add(new LanguageItem("lt_LT", "Lithuanian (Lithuania)"));
			base.Add(new LanguageItem("lv_LV", "Latvian (Latvia)"));
			base.Add(new LanguageItem("mi_NZ", "Maori (New Zealand)"));
			base.Add(new LanguageItem("mk_MK", "FYRO Macedonian (Former Yugoslav Republic of Macedonia)"));
			base.Add(new LanguageItem("mn_MN", "Mongolian (Mongolia)"));
			base.Add(new LanguageItem("mr_IN", "Marathi (India)"));
			base.Add(new LanguageItem("ms_BN", "Malay (Brunei Darussalam)"));
			base.Add(new LanguageItem("ms_MY", "Malay (Malaysia)"));
			base.Add(new LanguageItem("mt_MT", "Maltese (Malta)"));
			base.Add(new LanguageItem("nb_NO", "Norwegian (Bokm?l) (Norway)"));
			base.Add(new LanguageItem("nl_BE", "Dutch (Belgium)"));
			base.Add(new LanguageItem("nl_NL", "Dutch (Netherlands)"));
			base.Add(new LanguageItem("nn_NO", "Norwegian (Nynorsk) (Norway)"));
			base.Add(new LanguageItem("ns_ZA", "Northern Sotho (South Africa)"));
			base.Add(new LanguageItem("pa_IN", "Punjabi (India)"));
			base.Add(new LanguageItem("pl_PL", "Polish (Poland)"));
			base.Add(new LanguageItem("ps_AR", "Pashto (Afghanistan)"));
			base.Add(new LanguageItem("pt_BR", "Portuguese (Brazil)"));
			base.Add(new LanguageItem("pt_PT", "Portuguese (Portugal)"));
			base.Add(new LanguageItem("qu_BO", "Quechua (Bolivia)"));
			base.Add(new LanguageItem("qu_EC", "Quechua (Ecuador)"));
			base.Add(new LanguageItem("qu_PE", "Quechua (Peru)"));
			base.Add(new LanguageItem("ro_RO", "Romanian (Romania)"));
			base.Add(new LanguageItem("ru_RU", "Russian (Russia)"));
			base.Add(new LanguageItem("sa_IN", "Sanskrit (India)"));
			base.Add(new LanguageItem("se_FI", "Sami (Northern) (Finland)"));
			base.Add(new LanguageItem("se_FI", "Sami (Skolt) (Finland)"));
			base.Add(new LanguageItem("se_FI", "Sami (Inari) (Finland)"));
			base.Add(new LanguageItem("se_NO", "Sami (Northern) (Norway)"));
			base.Add(new LanguageItem("se_NO", "Sami (Lule) (Norway)"));
			base.Add(new LanguageItem("se_NO", "Sami (Southern) (Norway)"));
			base.Add(new LanguageItem("se_SE", "Sami (Northern) (Sweden)"));
			base.Add(new LanguageItem("se_SE", "Sami (Lule) (Sweden)"));
			base.Add(new LanguageItem("se_SE", "Sami (Southern) (Sweden)"));
			base.Add(new LanguageItem("sk_SK", "Slovak (Slovakia)"));
			base.Add(new LanguageItem("sl_SI", "Slovenian (Slovenia)"));
			base.Add(new LanguageItem("sq_AL", "Albanian (Albania)"));
			base.Add(new LanguageItem("sr_BA", "Serbian (Latin) (Bosnia and Herzegovina)"));
			base.Add(new LanguageItem("sr_BA", "Serbian (Cyrillic) (Bosnia and Herzegovina)"));
			base.Add(new LanguageItem("sr_SP", "Serbian (Latin) (Serbia and Montenegro)"));
			base.Add(new LanguageItem("sr_SP", "Serbian (Cyrillic) (Serbia and Montenegro)"));
			base.Add(new LanguageItem("sv_FI", "Swedish (Finland)"));
			base.Add(new LanguageItem("sv_SE", "Swedish (Sweden)"));
			base.Add(new LanguageItem("sw_KE", "Swahili (Kenya)"));
			base.Add(new LanguageItem("syr_SY", "Syriac (Syria)"));
			base.Add(new LanguageItem("ta_IN", "Tamil (India)"));
			base.Add(new LanguageItem("te_IN", "Telugu (India)"));
			base.Add(new LanguageItem("th_TH", "Thai (Thailand)"));
			base.Add(new LanguageItem("tl_PH", "Tagalog (Philippines)"));
			base.Add(new LanguageItem("tn_ZA", "Tswana (South Africa)"));
			base.Add(new LanguageItem("tr_TR", "Turkish (Turkey)"));
			base.Add(new LanguageItem("tt_RU", "Tatar (Russia)"));
			base.Add(new LanguageItem("uk_UA", "Ukrainian (Ukraine)"));
			base.Add(new LanguageItem("ur_PK", "Urdu (Islamic Republic of Pakistan)"));
			base.Add(new LanguageItem("uz_UZ", "Uzbek (Latin) (Uzbekistan)"));
			base.Add(new LanguageItem("uz_UZ", "Uzbek (Cyrillic) (Uzbekistan)"));
			base.Add(new LanguageItem("vi_VN", "Vietnamese (Viet Nam)"));
			base.Add(new LanguageItem("xh_ZA", "Xhosa (South Africa)"));
			base.Add(new LanguageItem("zh_CN", "Chinese (S)"));
			base.Add(new LanguageItem("zh_HK", "Chinese (Hong Kong)"));
			base.Add(new LanguageItem("zh_MO", "Chinese (Macau)"));
			base.Add(new LanguageItem("zh_SG", "Chinese (Singapore)"));
			base.Add(new LanguageItem("zh_TW", "Chinese (T)"));
			base.Add(new LanguageItem("zu_ZA", "Zulu (South Africa)"));
			string[] array = this.Parse0x2D00(dataset2d00);
			for (int i = 0; i < array.Length; i++)
			{
				string code = array[i];
				LanguageItem languageItem = this.FirstOrDefault((LanguageItem x) => x.Code == code);
				if (languageItem != null)
				{
					languageItem.IsChecked = true;
				}
				else
				{
					languageItem = new LanguageItem(code, code)
					{
						IsChecked = true
					};
					base.Add(languageItem);
				}
			}
			this.Parse0x280(dataset280);
			foreach (LanguageItem languageItem2 in this)
			{
				languageItem2.PropertyChanged += this.Item_PropertyChanged;
			}
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x0041E9DC File Offset: 0x0041CBDC
		private string[] Parse0x280(byte[] data)
		{
			string[] array = new string[3];
			for (int i = 0; i < 3; i++)
			{
				int num = i * 6;
				string @string = Encoding.ASCII.GetString(data, num, 5);
				array[i] = @string;
			}
			return array;
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x0041EA14 File Offset: 0x0041CC14
		private string[] Parse0x2D00(byte[] data)
		{
			int num = (int)data[0];
			List<string> list = new List<string>(num);
			for (int i = 0; i < num; i++)
			{
				int num2 = 1 + i * 6;
				string @string = Encoding.ASCII.GetString(data, num2, 5);
				list.Add(@string);
			}
			return list.ToArray();
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x0041EA5C File Offset: 0x0041CC5C
		private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsChecked")
			{
				List<LanguageItem> list = this.Where((LanguageItem x) => x.IsChecked).ToList<LanguageItem>();
				if (list.Count > this.MaxItems)
				{
					(sender as LanguageItem).IsChecked = false;
				}
				this.SelectedItems = list;
				this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
				if (!this.SelectedItems.Contains(this.DefaultLanguage))
				{
					this.DefaultLanguage = this.SelectedItems.FirstOrDefault<LanguageItem>();
				}
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x060057D6 RID: 22486 RVA: 0x0041EAFE File Offset: 0x0041CCFE
		// (set) Token: 0x060057D7 RID: 22487 RVA: 0x0041EB06 File Offset: 0x0041CD06
		public LanguageItem DefaultLanguage
		{
			get
			{
				return this._DefaultLanguage;
			}
			set
			{
				if (value != this._DefaultLanguage)
				{
					this._DefaultLanguage = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("DefaultLanguage"));
				}
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x060057D8 RID: 22488 RVA: 0x0041EB28 File Offset: 0x0041CD28
		// (set) Token: 0x060057D9 RID: 22489 RVA: 0x0041EB30 File Offset: 0x0041CD30
		public List<LanguageItem> SelectedItems
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedItems>k__BackingField = value;
			}
		} = new List<LanguageItem>();

		// Token: 0x04003650 RID: 13904
		private int MaxItems = 64;

		// Token: 0x04003651 RID: 13905
		private LanguageItem _DefaultLanguage;

		// Token: 0x04003652 RID: 13906
		[CompilerGenerated]
		private List<LanguageItem> <SelectedItems>k__BackingField;

		// Token: 0x02000AF3 RID: 2803
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060057DA RID: 22490 RVA: 0x0041EB39 File Offset: 0x0041CD39
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060057DB RID: 22491 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060057DC RID: 22492 RVA: 0x0041EB45 File Offset: 0x0041CD45
			internal bool <Item_PropertyChanged>b__4_0(LanguageItem x)
			{
				return x.IsChecked;
			}

			// Token: 0x04003653 RID: 13907
			public static readonly MIB2LanguageModel.<>c <>9 = new MIB2LanguageModel.<>c();

			// Token: 0x04003654 RID: 13908
			public static Func<LanguageItem, bool> <>9__4_0;
		}

		// Token: 0x02000AF4 RID: 2804
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060057DD RID: 22493 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060057DE RID: 22494 RVA: 0x0041EB4D File Offset: 0x0041CD4D
			internal bool <.ctor>b__0(LanguageItem x)
			{
				return x.Code == this.code;
			}

			// Token: 0x04003655 RID: 13909
			public string code;
		}
	}
}
