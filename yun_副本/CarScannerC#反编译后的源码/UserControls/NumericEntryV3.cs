using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005CF RID: 1487
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\NumericEntryV3.xaml")]
	public class NumericEntryV3 : EntryWithNumericKeyboard
	{
		// Token: 0x0600357E RID: 13694 RVA: 0x002659C5 File Offset: 0x00263BC5
		public NumericEntryV3()
		{
			this.InitializeComponent();
			this.UpdateEntryFromValue();
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x002659D9 File Offset: 0x00263BD9
		private void Entry_Unfocused(object sender, FocusEventArgs e)
		{
			this.UpdateEntryFromValue();
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x06003580 RID: 13696 RVA: 0x002659E4 File Offset: 0x00263BE4
		// (remove) Token: 0x06003581 RID: 13697 RVA: 0x00265A1C File Offset: 0x00263C1C
		public event EventHandler Completed
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.Completed;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Completed, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.Completed;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Completed, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06003582 RID: 13698 RVA: 0x00265A51 File Offset: 0x00263C51
		private void Entry_Completed(object sender, EventArgs e)
		{
			this.UpdateEntryFromValue();
			EventHandler completed = this.Completed;
			if (completed == null)
			{
				return;
			}
			completed(this, e);
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00265A6C File Offset: 0x00263C6C
		private void UpdateEntryFromValue()
		{
			string text = this.Value.ToString(this.DoubleFormat, CultureInfo.InvariantCulture);
			if (this.entry.Text != text)
			{
				this.entry.Text = text;
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x00265AB4 File Offset: 0x00263CB4
		private static void ValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			NumericEntryV3 numericEntryV = (NumericEntryV3)bindable;
			double num = (double)newValue;
			if (!double.IsFinite(num))
			{
				num = 0.0;
			}
			string text = num.ToString((string)numericEntryV.GetValue(NumericEntryV3.DoubleFormatProperty), CultureInfo.InvariantCulture);
			if (numericEntryV.entry.Text != text)
			{
				numericEntryV.entry.Text = text;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x00265B1D File Offset: 0x00263D1D
		// (set) Token: 0x06003586 RID: 13702 RVA: 0x00265B30 File Offset: 0x00263D30
		public double Value
		{
			get
			{
				return (double)base.GetValue(NumericEntryV3.ValueProperty);
			}
			set
			{
				double value2 = this.Value;
				if (value != value2)
				{
					if (double.IsFinite(value))
					{
						base.SetValue(NumericEntryV3.ValueProperty, value);
						return;
					}
					base.SetValue(NumericEntryV3.ValueProperty, 0.0);
				}
			}
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x00265B7C File Offset: 0x00263D7C
		private static void DoubleFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			NumericEntryV3 numericEntryV = (NumericEntryV3)bindable;
			NumericEntryV3.ValuePropertyChanged(numericEntryV, null, numericEntryV.Value);
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x00265BA2 File Offset: 0x00263DA2
		// (set) Token: 0x06003589 RID: 13705 RVA: 0x00265BB4 File Offset: 0x00263DB4
		public string DoubleFormat
		{
			get
			{
				return (string)base.GetValue(NumericEntryV3.DoubleFormatProperty);
			}
			set
			{
				base.SetValue(NumericEntryV3.DoubleFormatProperty, value);
			}
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x00265BC4 File Offset: 0x00263DC4
		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue == null || e.NewTextValue == "" || e.NewTextValue == "-")
			{
				return;
			}
			if (e.NewTextValue.Any((char X) => !NumericEntryV3.allowedChars.Contains(X)))
			{
				this.entry.Text = e.OldTextValue;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(e.NewTextValue.Length);
			foreach (char c in e.NewTextValue)
			{
				if (NumericEntryV3.allowedChars.Contains(c))
				{
					stringBuilder.Append(c);
				}
			}
			string text = stringBuilder.ToString().Replace(',', '.');
			double naN = double.NaN;
			if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out naN))
			{
				this.entry.Text = e.OldTextValue;
				return;
			}
			if (naN > this.Maximum)
			{
				this.Value = this.Maximum;
				this.UpdateEntryFromValue();
				return;
			}
			if (naN < this.Minimum)
			{
				this.Value = this.Minimum;
				this.UpdateEntryFromValue();
				return;
			}
			this.Value = naN;
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x00265D04 File Offset: 0x00263F04
		private static void MinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			NumericEntryV3 numericEntryV = (NumericEntryV3)bindable;
			if (numericEntryV.Value < numericEntryV.Minimum)
			{
				numericEntryV.Value = numericEntryV.Minimum;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x0600358C RID: 13708 RVA: 0x00265D32 File Offset: 0x00263F32
		// (set) Token: 0x0600358D RID: 13709 RVA: 0x00265D44 File Offset: 0x00263F44
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(NumericEntryV3.MinimumProperty);
			}
			set
			{
				base.SetValue(NumericEntryV3.MinimumProperty, value);
			}
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00265D58 File Offset: 0x00263F58
		private static void MaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			NumericEntryV3 numericEntryV = (NumericEntryV3)bindable;
			if (numericEntryV.Value > numericEntryV.Maximum)
			{
				numericEntryV.Value = numericEntryV.Maximum;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x0600358F RID: 13711 RVA: 0x00265D86 File Offset: 0x00263F86
		// (set) Token: 0x06003590 RID: 13712 RVA: 0x00265D98 File Offset: 0x00263F98
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(NumericEntryV3.MaximumProperty);
			}
			set
			{
				base.SetValue(NumericEntryV3.MaximumProperty, value);
			}
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x00265DAC File Offset: 0x00263FAC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(NumericEntryV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/NumericEntryV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\NumericEntryV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\NumericEntryV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\NumericEntryV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\NumericEntryV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("entry", this);
			if (this.StyleId == null)
			{
				this.StyleId = "entry";
			}
			this.entry = this;
			dynamicResourceExtension.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(NumericEntryV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(8, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Completed += this.Entry_Completed;
			this.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			this.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			dynamicResourceExtension2.Key = "GrayedTextColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Entry.PlaceholderColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(NumericEntryV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(Entry.PlaceholderColorProperty, dynamicResource2.Key);
			this.TextChanged += this.Entry_TextChanged;
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Entry.TextColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(NumericEntryV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			this.SetDynamicResource(Entry.TextColorProperty, dynamicResource3.Key);
			this.Unfocused += this.Entry_Unfocused;
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x00266204 File Offset: 0x00264404
		// Note: this type is marked as 'beforefieldinit'.
		static NumericEntryV3()
		{
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x0026630D File Offset: 0x0026450D
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<NumericEntryV3>(this, typeof(NumericEntryV3));
			this.entry = NameScopeExtensions.FindByName<EntryWithNumericKeyboard>(this, "entry");
		}

		// Token: 0x04001FCF RID: 8143
		[CompilerGenerated]
		private EventHandler Completed;

		// Token: 0x04001FD0 RID: 8144
		public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(double), typeof(NumericEntryV3), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(NumericEntryV3.ValuePropertyChanged), null, null, null);

		// Token: 0x04001FD1 RID: 8145
		public static readonly BindableProperty DoubleFormatProperty = BindableProperty.Create("DoubleFormat", typeof(string), typeof(NumericEntryV3), "", 2, null, new BindableProperty.BindingPropertyChangedDelegate(NumericEntryV3.DoubleFormatPropertyChanged), null, null, null);

		// Token: 0x04001FD2 RID: 8146
		private static string allowedChars = "0123456789-.,";

		// Token: 0x04001FD3 RID: 8147
		public static readonly BindableProperty MinimumProperty = BindableProperty.Create("Minimum", typeof(double), typeof(NumericEntryV3), double.MinValue, 2, null, new BindableProperty.BindingPropertyChangedDelegate(NumericEntryV3.MinimumPropertyChanged), null, null, null);

		// Token: 0x04001FD4 RID: 8148
		public static readonly BindableProperty MaximumProperty = BindableProperty.Create("Maximum", typeof(double), typeof(NumericEntryV3), double.MaxValue, 2, null, new BindableProperty.BindingPropertyChangedDelegate(NumericEntryV3.MaximumPropertyChanged), null, null, null);

		// Token: 0x04001FD5 RID: 8149
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private EntryWithNumericKeyboard entry;

		// Token: 0x020005D0 RID: 1488
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003594 RID: 13716 RVA: 0x00266331 File Offset: 0x00264531
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003595 RID: 13717 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003596 RID: 13718 RVA: 0x0026633D File Offset: 0x0026453D
			internal bool <Entry_TextChanged>b__18_0(char X)
			{
				return !NumericEntryV3.allowedChars.Contains(X);
			}

			// Token: 0x04001FD6 RID: 8150
			public static readonly NumericEntryV3.<>c <>9 = new NumericEntryV3.<>c();

			// Token: 0x04001FD7 RID: 8151
			public static Func<char, bool> <>9__18_0;
		}
	}
}
