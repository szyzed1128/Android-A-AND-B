using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000010 RID: 16
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\ColorMixer\\ColorPickerEntry.xaml")]
	public class ColorPickerEntry : ContentView
	{
		// Token: 0x06000049 RID: 73 RVA: 0x0000316C File Offset: 0x0000136C
		public ColorPickerEntry()
		{
			this.InitializeComponent();
			this.eColor.BindingContext = this.ColorVal;
			this.fEdit.BindingContext = this.ColorVal;
			this.ColorVal.PropertyChanged += this.ColorVal_PropertyChanged;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004A RID: 74 RVA: 0x000031DB File Offset: 0x000013DB
		public Entry Editor
		{
			get
			{
				return this.eColor;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000031E3 File Offset: 0x000013E3
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000031EB File Offset: 0x000013EB
		public string DialogTitle
		{
			[CompilerGenerated]
			get
			{
				return this.<DialogTitle>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DialogTitle>k__BackingField = value;
			}
		} = "";

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000031F4 File Offset: 0x000013F4
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000031FC File Offset: 0x000013FC
		public Layout<View> RootContainer
		{
			[CompilerGenerated]
			get
			{
				return this.<RootContainer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RootContainer>k__BackingField = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003205 File Offset: 0x00001405
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003212 File Offset: 0x00001412
		public bool ShowColorPreview
		{
			get
			{
				return this.bEdit.IsVisible;
			}
			set
			{
				this.bEdit.IsVisible = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003220 File Offset: 0x00001420
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000322D File Offset: 0x0000142D
		public double ColorPreviewButtonWidth
		{
			get
			{
				return this.fEdit.WidthRequest;
			}
			set
			{
				this.fEdit.WidthRequest = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000323B File Offset: 0x0000143B
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003248 File Offset: 0x00001448
		public Color ColorPreviewButtonBorder
		{
			get
			{
				return this.fEdit.BorderColor;
			}
			set
			{
				this.fEdit.BorderColor = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x00003256 File Offset: 0x00001456
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003263 File Offset: 0x00001463
		public double SpaceBetweenEditorAndButton
		{
			get
			{
				return this.sLayout.Spacing;
			}
			set
			{
				this.sLayout.Spacing = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003271 File Offset: 0x00001471
		// (set) Token: 0x06000058 RID: 88 RVA: 0x00003279 File Offset: 0x00001479
		public bool AllowPickerDialog
		{
			[CompilerGenerated]
			get
			{
				return this.<AllowPickerDialog>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AllowPickerDialog>k__BackingField = value;
			}
		} = true;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000059 RID: 89 RVA: 0x00003282 File Offset: 0x00001482
		// (set) Token: 0x0600005A RID: 90 RVA: 0x0000328F File Offset: 0x0000148F
		public bool EditAlfa
		{
			get
			{
				return this.ColorVal.EditAlpha;
			}
			set
			{
				this.ColorVal.EditAlpha = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600005B RID: 91 RVA: 0x0000329D File Offset: 0x0000149D
		// (set) Token: 0x0600005C RID: 92 RVA: 0x000032A5 File Offset: 0x000014A5
		public ColorDialogSettings DialogSettings
		{
			[CompilerGenerated]
			get
			{
				return this.<DialogSettings>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DialogSettings>k__BackingField = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600005D RID: 93 RVA: 0x000032B0 File Offset: 0x000014B0
		// (remove) Token: 0x0600005E RID: 94 RVA: 0x000032E8 File Offset: 0x000014E8
		public event PreviewButtonClickedDelegate PreviewButtonClicked
		{
			[CompilerGenerated]
			add
			{
				PreviewButtonClickedDelegate previewButtonClickedDelegate = this.PreviewButtonClicked;
				PreviewButtonClickedDelegate previewButtonClickedDelegate2;
				do
				{
					previewButtonClickedDelegate2 = previewButtonClickedDelegate;
					PreviewButtonClickedDelegate previewButtonClickedDelegate3 = (PreviewButtonClickedDelegate)Delegate.Combine(previewButtonClickedDelegate2, value);
					previewButtonClickedDelegate = Interlocked.CompareExchange<PreviewButtonClickedDelegate>(ref this.PreviewButtonClicked, previewButtonClickedDelegate3, previewButtonClickedDelegate2);
				}
				while (previewButtonClickedDelegate != previewButtonClickedDelegate2);
			}
			[CompilerGenerated]
			remove
			{
				PreviewButtonClickedDelegate previewButtonClickedDelegate = this.PreviewButtonClicked;
				PreviewButtonClickedDelegate previewButtonClickedDelegate2;
				do
				{
					previewButtonClickedDelegate2 = previewButtonClickedDelegate;
					PreviewButtonClickedDelegate previewButtonClickedDelegate3 = (PreviewButtonClickedDelegate)Delegate.Remove(previewButtonClickedDelegate2, value);
					previewButtonClickedDelegate = Interlocked.CompareExchange<PreviewButtonClickedDelegate>(ref this.PreviewButtonClicked, previewButtonClickedDelegate3, previewButtonClickedDelegate2);
				}
				while (previewButtonClickedDelegate != previewButtonClickedDelegate2);
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000331D File Offset: 0x0000151D
		private void ColorVal_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Value")
			{
				this.Color = this.ColorVal.Value;
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003342 File Offset: 0x00001542
		private static void ColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((Color)oldValue != (Color)newValue)
			{
				((ColorPickerEntry)bindable).ColorVal.Value = (Color)newValue;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000336D File Offset: 0x0000156D
		// (set) Token: 0x06000062 RID: 98 RVA: 0x0000337F File Offset: 0x0000157F
		public Color Color
		{
			get
			{
				return (Color)base.GetValue(ColorPickerEntry.ColorProperty);
			}
			set
			{
				if (this.Color != value)
				{
					base.SetValue(ColorPickerEntry.ColorProperty, value);
				}
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000033A0 File Offset: 0x000015A0
		private static void DialogColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((Color)oldValue != (Color)newValue)
			{
				((ColorPickerEntry)bindable).DialogColor = (Color)newValue;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000033C6 File Offset: 0x000015C6
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000033D8 File Offset: 0x000015D8
		public Color DialogColor
		{
			get
			{
				return (Color)base.GetValue(ColorPickerEntry.DialogColorProperty);
			}
			set
			{
				if (this.DialogColor != value)
				{
					base.SetValue(ColorPickerEntry.DialogColorProperty, value);
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000033F9 File Offset: 0x000015F9
		private static void TextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((Color)oldValue != (Color)newValue)
			{
				((ColorPickerEntry)bindable).TextColor = (Color)newValue;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000067 RID: 103 RVA: 0x0000341F File Offset: 0x0000161F
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00003431 File Offset: 0x00001631
		public Color TextColor
		{
			get
			{
				return (Color)base.GetValue(ColorPickerEntry.TextColorProperty);
			}
			set
			{
				if (this.TextColor != value)
				{
					base.SetValue(ColorPickerEntry.TextColorProperty, value);
				}
				this.eColor.TextColor = value;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003460 File Offset: 0x00001660
		private async void OpenDialog_Clicked(object sender, EventArgs e)
		{
			if (this.PreviewButtonClicked != null)
			{
				PreviewButtonClickedEventArgs eva = new PreviewButtonClickedEventArgs
				{
					Color = this.Color,
					Handled = false
				};
				await this.PreviewButtonClicked(this, eva);
				this.Color = eva.Color;
				if (eva.Handled)
				{
					return;
				}
				eva = null;
			}
			if (this.AllowPickerDialog)
			{
				if (this.RootContainer == null)
				{
					this.RootContainer = ColorPickerUtils.GetRootParent<Layout<View>>(this);
				}
				if (this.RootContainer != null)
				{
					ColorDialogSettings colorDialogSettings;
					if ((colorDialogSettings = this.DialogSettings) == null)
					{
						ColorDialogSettings colorDialogSettings2 = new ColorDialogSettings();
						colorDialogSettings2.DialogColor = this.DialogColor;
						colorDialogSettings = colorDialogSettings2;
						colorDialogSettings2.TextColor = this.TextColor;
					}
					ColorDialogSettings colorDialogSettings3 = colorDialogSettings;
					colorDialogSettings3.EditAlfa = this.EditAlfa;
					ColorValue colorValue = this.ColorVal;
					colorValue.Value = await ColorPickerDialog.Show(this.RootContainer, this.DialogTitle, this.ColorVal.Value, colorDialogSettings3);
					colorValue = null;
				}
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003498 File Offset: 0x00001698
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ColorPickerEntry).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/ColorMixer/ColorPickerEntry.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 13);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 11);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 13);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 16);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 16);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 11);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 7, 8);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\ColorMixer\\ColorPickerEntry.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("sLayout", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "sLayout";
			}
			nameScope.RegisterName("eColor", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "eColor";
			}
			nameScope.RegisterName("fEdit", frame);
			if (frame.StyleId == null)
			{
				frame.StyleId = "fEdit";
			}
			nameScope.RegisterName("bEdit", button);
			if (button.StyleId == null)
			{
				button.StyleId = "bEdit";
			}
			this.sLayout = stackLayout;
			this.eColor = entry;
			this.fEdit = frame;
			this.bEdit = button;
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(StackLayout.SpacingProperty, 5.0);
			entry.SetValue(View.MarginProperty, new Thickness(0.0));
			entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			bindingExtension.Path = "Hexa";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase);
			entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout.Children.Add(entry);
			frame.SetValue(View.MarginProperty, new Thickness(0.0));
			frame.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			frame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension2.Path = "Value";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			frame.SetBinding(Frame.OutlineColorProperty, bindingBase2);
			frame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			frame.SetValue(VisualElement.WidthRequestProperty, 30.0);
			button.SetValue(View.MarginProperty, new Thickness(-1.0));
			bindingExtension3.Path = "Value";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			button.SetBinding(VisualElement.BackgroundColorProperty, bindingBase3);
			bindingExtension4.Path = "Value";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			button.SetBinding(Button.BorderColorProperty, bindingBase4);
			button.Clicked += this.OpenDialog_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			button.SetValue(VisualElement.WidthRequestProperty, 75.0);
			frame.SetValue(ContentView.ContentProperty, button);
			stackLayout.Children.Add(frame);
			this.SetValue(ContentView.ContentProperty, stackLayout);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003984 File Offset: 0x00001B84
		// Note: this type is marked as 'beforefieldinit'.
		static ColorPickerEntry()
		{
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00003A4C File Offset: 0x00001C4C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ColorPickerEntry>(this, typeof(ColorPickerEntry));
			this.sLayout = NameScopeExtensions.FindByName<StackLayout>(this, "sLayout");
			this.eColor = NameScopeExtensions.FindByName<Entry>(this, "eColor");
			this.fEdit = NameScopeExtensions.FindByName<Frame>(this, "fEdit");
			this.bEdit = NameScopeExtensions.FindByName<Button>(this, "bEdit");
		}

		// Token: 0x0400003B RID: 59
		private ColorValue ColorVal = new ColorValue();

		// Token: 0x0400003C RID: 60
		[CompilerGenerated]
		private string <DialogTitle>k__BackingField;

		// Token: 0x0400003D RID: 61
		[CompilerGenerated]
		private Layout<View> <RootContainer>k__BackingField;

		// Token: 0x0400003E RID: 62
		[CompilerGenerated]
		private bool <AllowPickerDialog>k__BackingField;

		// Token: 0x0400003F RID: 63
		[CompilerGenerated]
		private ColorDialogSettings <DialogSettings>k__BackingField;

		// Token: 0x04000040 RID: 64
		[CompilerGenerated]
		private PreviewButtonClickedDelegate PreviewButtonClicked;

		// Token: 0x04000041 RID: 65
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(ColorPickerEntry), Color.White, 1, null, new BindableProperty.BindingPropertyChangedDelegate(ColorPickerEntry.ColorChanged), null, null, null);

		// Token: 0x04000042 RID: 66
		public static readonly BindableProperty DialogColorProperty = BindableProperty.Create("DialogColor", typeof(Color), typeof(ColorPickerEntry), Color.White, 1, null, new BindableProperty.BindingPropertyChangedDelegate(ColorPickerEntry.DialogColorChanged), null, null, null);

		// Token: 0x04000043 RID: 67
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(ColorPickerEntry), Color.Gray, 1, null, new BindableProperty.BindingPropertyChangedDelegate(ColorPickerEntry.TextColorChanged), null, null, null);

		// Token: 0x04000044 RID: 68
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout sLayout;

		// Token: 0x04000045 RID: 69
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry eColor;

		// Token: 0x04000046 RID: 70
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Frame fEdit;

		// Token: 0x04000047 RID: 71
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button bEdit;

		// Token: 0x02000011 RID: 17
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OpenDialog_Clicked>d__54 : IAsyncStateMachine
		{
			// Token: 0x0600006D RID: 109 RVA: 0x00003AB0 File Offset: 0x00001CB0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ColorPickerEntry colorPickerEntry = this;
				try
				{
					TaskAwaiter<Color> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Color> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Color>);
							num2 = -1;
							goto IL_01A9;
						}
						if (colorPickerEntry.PreviewButtonClicked == null)
						{
							goto IL_00D2;
						}
						eva = new PreviewButtonClickedEventArgs
						{
							Color = colorPickerEntry.Color,
							Handled = false
						};
						taskAwaiter3 = colorPickerEntry.PreviewButtonClicked(colorPickerEntry, eva).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ColorPickerEntry.<OpenDialog_Clicked>d__54>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					colorPickerEntry.Color = eva.Color;
					if (eva.Handled)
					{
						goto IL_01E1;
					}
					eva = null;
					IL_00D2:
					if (!colorPickerEntry.AllowPickerDialog)
					{
						goto IL_01E1;
					}
					if (colorPickerEntry.RootContainer == null)
					{
						colorPickerEntry.RootContainer = ColorPickerUtils.GetRootParent<Layout<View>>(colorPickerEntry);
					}
					if (colorPickerEntry.RootContainer == null)
					{
						goto IL_01C6;
					}
					ColorDialogSettings colorDialogSettings;
					if ((colorDialogSettings = colorPickerEntry.DialogSettings) == null)
					{
						ColorDialogSettings colorDialogSettings2 = new ColorDialogSettings();
						colorDialogSettings2.DialogColor = colorPickerEntry.DialogColor;
						colorDialogSettings = colorDialogSettings2;
						colorDialogSettings2.TextColor = colorPickerEntry.TextColor;
					}
					ColorDialogSettings colorDialogSettings3 = colorDialogSettings;
					colorDialogSettings3.EditAlfa = colorPickerEntry.EditAlfa;
					colorValue = colorPickerEntry.ColorVal;
					taskAwaiter = ColorPickerDialog.Show(colorPickerEntry.RootContainer, colorPickerEntry.DialogTitle, colorPickerEntry.ColorVal.Value, colorDialogSettings3).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Color> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Color>, ColorPickerEntry.<OpenDialog_Clicked>d__54>(ref taskAwaiter, ref this);
						return;
					}
					IL_01A9:
					Color result = taskAwaiter.GetResult();
					colorValue.Value = result;
					colorValue = null;
					IL_01C6:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01E1:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600006E RID: 110 RVA: 0x00003CD0 File Offset: 0x00001ED0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000048 RID: 72
			public int <>1__state;

			// Token: 0x04000049 RID: 73
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400004A RID: 74
			public ColorPickerEntry <>4__this;

			// Token: 0x0400004B RID: 75
			private PreviewButtonClickedEventArgs <eva>5__2;

			// Token: 0x0400004C RID: 76
			private TaskAwaiter <>u__1;

			// Token: 0x0400004D RID: 77
			private ColorValue <>7__wrap2;

			// Token: 0x0400004E RID: 78
			private TaskAwaiter<Color> <>u__2;
		}
	}
}
