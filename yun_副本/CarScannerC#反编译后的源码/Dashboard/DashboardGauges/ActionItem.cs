using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Dashboard.DashboardGauges
{
	// Token: 0x020007A8 RID: 1960
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\ActionItem.xaml")]
	public class ActionItem : ContentView
	{
		// Token: 0x060042F9 RID: 17145 RVA: 0x0033F1DD File Offset: 0x0033D3DD
		public ActionItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x060042FA RID: 17146 RVA: 0x0033F1EC File Offset: 0x0033D3EC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ActionItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/ActionItem.xaml",
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
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			FloatValueToColorConverter floatValueToColorConverter;
			VisualDiagnostics.RegisterSourceInfo(floatValueToColorConverter = new FloatValueToColorConverter(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			FloatItemToLineBreakModeConverter floatItemToLineBreakModeConverter;
			VisualDiagnostics.RegisterSourceInfo(floatItemToLineBreakModeConverter = new FloatItemToLineBreakModeConverter(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 49);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\ActionItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("bindingHackName", this);
			if (this.StyleId == null)
			{
				this.StyleId = "bindingHackName";
			}
			this.bindingHackName = this;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("FloatValueToColorConverter", floatValueToColorConverter);
			resourceDictionary.Add("FloatItemToLineBreakModeConverter", floatItemToLineBreakModeConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			this.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ShowDefaultBackground";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			gaugeBackground.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid.Children.Add(gaugeBackground);
			linkButton.SetValue(Grid.RowProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			bindingExtension2.Path = "Model.Action";
			bindingExtension2.TypedBinding = new TypedBinding<DashboardItem, ICommand>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<ICommand, bool>(model.Action, true);
					}
				}
				return default(ValueTuple<ICommand, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Action")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			linkButton.SetBinding(Button.CommandProperty, bindingBase2);
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "FontName";
			bindingExtension3.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.FontName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FontName")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			linkButton.SetBinding(Button.FontFamilyProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "ValueFontSize";
			bindingExtension4.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFontSize")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			linkButton.SetBinding(Button.FontSizeProperty, bindingBase4);
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			bindingExtension5.Path = "PIDName";
			bindingExtension5.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PIDName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PIDName")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			linkButton.SetBinding(Button.TextProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "ValueTextColor";
			bindingExtension6.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueTextColor")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			linkButton.SetBinding(Button.TextColorProperty, bindingBase6);
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			grid.Children.Add(linkButton);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x060042FB RID: 17147 RVA: 0x0033F9C2 File Offset: 0x0033DBC2
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ActionItem>(this, typeof(ActionItem));
			this.bindingHackName = NameScopeExtensions.FindByName<ContentView>(this, "bindingHackName");
		}

		// Token: 0x060042FC RID: 17148 RVA: 0x0033F9E8 File Offset: 0x0033DBE8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__263(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060042FD RID: 17149 RVA: 0x0033FA18 File Offset: 0x0033DC18
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__264(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060042FE RID: 17150 RVA: 0x0033FA28 File Offset: 0x0033DC28
		[CompilerGenerated]
		private static ValueTuple<ICommand, bool> <InitializeComponent>typedBindingsM__265(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<ICommand, bool>(model.Action, true);
				}
			}
			return default(ValueTuple<ICommand, bool>);
		}

		// Token: 0x060042FF RID: 17151 RVA: 0x0033FA60 File Offset: 0x0033DC60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__267(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x0033FA70 File Offset: 0x0033DC70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__268(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004301 RID: 17153 RVA: 0x0033FA84 File Offset: 0x0033DC84
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__269(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004302 RID: 17154 RVA: 0x0033FAB4 File Offset: 0x0033DCB4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__270(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x0033FAC4 File Offset: 0x0033DCC4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__271(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x0033FAF4 File Offset: 0x0033DCF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__272(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x0033FB04 File Offset: 0x0033DD04
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__273(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x0033FB34 File Offset: 0x0033DD34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__275(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0033FB44 File Offset: 0x0033DD44
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__276(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x0033FB74 File Offset: 0x0033DD74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__277(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0400289A RID: 10394
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView bindingHackName;
	}
}
