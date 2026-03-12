using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using org.mariuszgromada.math.mxparser;
using Xamarin.Forms.Internals;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003DE RID: 990
	public class CustomPID : PID, IPIDFloatValue, IPID, INotifyPropertyChanged, IPIDWithRequiredPids
	{
		// Token: 0x060027E7 RID: 10215 RVA: 0x001E96EC File Offset: 0x001E78EC
		[JsonConstructor]
		public CustomPID(string Name, string ShortName, string Command, string Header, string Formula, UnitsHelper.Units Units, double MinValue, double MaxValue, string BeforeCommand, string AfterCommand, bool IsAction, Roles Role, CustomPIDType Type, int StartByteId, int DataLength, double Multiplier, double Divider, double Offset, bool IsSigned, bool IsFormulaHidden, bool IsVisible = true, ObservableCollection<TranslationItem> Translations = null)
			: base(Name, Command)
		{
			base.ShortName = ShortName;
			this.Units = Units;
			base.Maximum = MaxValue;
			base.Minimum = MinValue;
			this.Formula = Formula;
			this.Header = Header;
			this.Type = Type;
			base.Name = CustomPID.ReplaceFromResource(Name);
			base.ShortName = CustomPID.ReplaceFromResource(ShortName);
			this.BeforeCommand = BeforeCommand;
			this.AfterCommand = AfterCommand;
			if (IsAction)
			{
				this.Type = CustomPIDType.Action;
			}
			this.HasAnnotation = false;
			base.Role = Role;
			this.StartByteId = StartByteId;
			this.DataLength = DataLength;
			this.Multiplier = Multiplier;
			this.Divider = Divider;
			this.Offset = Offset;
			this.IsSigned = IsSigned;
			this.IsFormulaHidden = IsFormulaHidden;
			if (Translations == null)
			{
				this.Translations = new ObservableCollection<TranslationItem>();
			}
			else
			{
				this.Translations = Translations;
			}
			if (Translations != null && this.Translations.Count > 0)
			{
				TranslationItem translationItem = Translations.FirstOrDefault((TranslationItem x) => x.Language == App.CurrentLanguageCode);
				if (translationItem != null)
				{
					if (!string.IsNullOrEmpty(translationItem.Name))
					{
						base.Name = translationItem.Name;
					}
					if (!string.IsNullOrEmpty(translationItem.ShortName))
					{
						base.ShortName = translationItem.ShortName;
					}
				}
			}
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x001E98D6 File Offset: 0x001E7AD6
		public void SetValue(double value)
		{
			base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
			this.Value = value;
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x001E98F4 File Offset: 0x001E7AF4
		public void SendNaN()
		{
			base.TimeStamp = App.OBDReader.stopwatch.Elapsed;
			this.Value = double.NaN;
		}

		// Token: 0x170011AA RID: 4522
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x001E991A File Offset: 0x001E7B1A
		// (set) Token: 0x060027EB RID: 10219 RVA: 0x001E9922 File Offset: 0x001E7B22
		public override string Command
		{
			get
			{
				return base.Command;
			}
			set
			{
				base.Command = value;
				this.NotifyPropertyChanged("VagGroup");
				this.NotifyPropertyChanged("VagUnit");
			}
		}

		// Token: 0x170011AB RID: 4523
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x001E9941 File Offset: 0x001E7B41
		// (set) Token: 0x060027ED RID: 10221 RVA: 0x001E9949 File Offset: 0x001E7B49
		[JsonProperty("SBI")]
		public int StartByteId
		{
			get
			{
				return this._StartByteId;
			}
			set
			{
				this._StartByteId = value;
				this.NotifyPropertyChanged("StartByteId");
			}
		}

		// Token: 0x170011AC RID: 4524
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x001E995D File Offset: 0x001E7B5D
		// (set) Token: 0x060027EF RID: 10223 RVA: 0x001E9965 File Offset: 0x001E7B65
		[JsonProperty("DL")]
		public int DataLength
		{
			get
			{
				return this._DataLength;
			}
			set
			{
				this._DataLength = value;
				this.NotifyPropertyChanged("DataLength");
			}
		}

		// Token: 0x170011AD RID: 4525
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x001E9979 File Offset: 0x001E7B79
		// (set) Token: 0x060027F1 RID: 10225 RVA: 0x001E9981 File Offset: 0x001E7B81
		[JsonProperty("MUL")]
		public double Multiplier
		{
			get
			{
				return this._Multiplier;
			}
			set
			{
				this._Multiplier = value;
				this.NotifyPropertyChanged("Multiplier");
			}
		}

		// Token: 0x170011AE RID: 4526
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x001E9995 File Offset: 0x001E7B95
		// (set) Token: 0x060027F3 RID: 10227 RVA: 0x001E99C5 File Offset: 0x001E7BC5
		[JsonProperty("DIV")]
		public double Divider
		{
			get
			{
				if (this._Divider == 0.0 || !double.IsFinite(this._Divider))
				{
					return 1.0;
				}
				return this._Divider;
			}
			set
			{
				this._Divider = value;
				this.NotifyPropertyChanged("Divider");
			}
		}

		// Token: 0x170011AF RID: 4527
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x001E99D9 File Offset: 0x001E7BD9
		// (set) Token: 0x060027F5 RID: 10229 RVA: 0x001E99E1 File Offset: 0x001E7BE1
		[JsonProperty("OFS")]
		public double Offset
		{
			get
			{
				return this._Offset;
			}
			set
			{
				this._Offset = value;
				this.NotifyPropertyChanged("Offset");
			}
		}

		// Token: 0x170011B0 RID: 4528
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x001E99F5 File Offset: 0x001E7BF5
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x001E99FD File Offset: 0x001E7BFD
		[JsonProperty("SIG")]
		public bool IsSigned
		{
			get
			{
				return this._IsSigned;
			}
			set
			{
				this._IsSigned = value;
				this.NotifyPropertyChanged("IsSigned");
			}
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x001E9A11 File Offset: 0x001E7C11
		// (set) Token: 0x060027F9 RID: 10233 RVA: 0x001E9A19 File Offset: 0x001E7C19
		[JsonProperty("TP")]
		public CustomPIDType Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				if (this._Type != value)
				{
					this._Type = value;
					this.NotifyPropertyChanged("Type");
					this.NotifyPropertyChanged("IsAction");
				}
			}
		}

		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x001E9A41 File Offset: 0x001E7C41
		// (set) Token: 0x060027FB RID: 10235 RVA: 0x001E9A49 File Offset: 0x001E7C49
		[JsonProperty("BIT")]
		public int Bit
		{
			get
			{
				return this._Bit;
			}
			set
			{
				this._Bit = value;
				this.NotifyPropertyChanged("Bit");
			}
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x001E9A5D File Offset: 0x001E7C5D
		// (set) Token: 0x060027FD RID: 10237 RVA: 0x001E9A65 File Offset: 0x001E7C65
		[JsonProperty("FHID")]
		public bool IsFormulaHidden
		{
			[CompilerGenerated]
			get
			{
				return this.<IsFormulaHidden>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsFormulaHidden>k__BackingField = value;
			}
		}

		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x001E9A6E File Offset: 0x001E7C6E
		// (set) Token: 0x060027FF RID: 10239 RVA: 0x001E9A76 File Offset: 0x001E7C76
		[JsonProperty("RBS")]
		public bool ReversedByteSet
		{
			get
			{
				return this._ReversedByteSet;
			}
			set
			{
				this._ReversedByteSet = value;
				this.NotifyPropertyChanged("ReversedByteSet");
			}
		}

		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x001E9A8A File Offset: 0x001E7C8A
		// (set) Token: 0x06002801 RID: 10241 RVA: 0x001E9A92 File Offset: 0x001E7C92
		[JsonProperty("ORD")]
		public int OrderId
		{
			get
			{
				return this._OrderId;
			}
			set
			{
				this._OrderId = value;
				this.NotifyPropertyChanged("OrderId");
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06002802 RID: 10242 RVA: 0x001E9AA6 File Offset: 0x001E7CA6
		// (set) Token: 0x06002803 RID: 10243 RVA: 0x001E9AAE File Offset: 0x001E7CAE
		[JsonProperty("TVV")]
		public string TextValueVariants
		{
			get
			{
				return this._TextValueVariants;
			}
			set
			{
				if (this._TextValueVariants != value)
				{
					this._TextValueVariants = value;
					this.NotifyPropertyChanged("TextValueVariants");
					CustomPID.UpdateTextValuesDict(this._TextValuesDict, value);
				}
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x001E9ADC File Offset: 0x001E7CDC
		public static void UpdateTextValuesDict(Dictionary<int, string> dict, string s)
		{
			if (dict == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(s))
			{
				return;
			}
			foreach (string text in s.Split(new char[] { '\r', '\n', ';' }, StringSplitOptions.RemoveEmptyEntries))
			{
				if (text != null)
				{
					int num = text.IndexOf('=');
					if (num >= 0)
					{
						string text2 = text.Substring(0, num).Trim();
						string text3 = text.Substring(num + 1).Trim();
						int num2;
						if (int.TryParse(text2, out num2))
						{
							dict[num2] = text3;
						}
					}
				}
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x001E9B68 File Offset: 0x001E7D68
		public string GetTextValueVariantOrNull(double value)
		{
			if (this._TextValuesDict == null || this._TextValuesDict.Count == 0)
			{
				return null;
			}
			int num = (int)value;
			string text;
			if (this._TextValuesDict.TryGetValue(num, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x001E9BA4 File Offset: 0x001E7DA4
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is CustomPID))
			{
				return false;
			}
			CustomPID customPID = (CustomPID)obj;
			return customPID.Command == this.Command && customPID.Name == base.Name && customPID.ShortName == base.ShortName && customPID.Units == this.Units && customPID.Formula == this.Formula && customPID.Command == this.Command && customPID.Header == this.Header && customPID.AfterCommand == this.AfterCommand && customPID.BeforeCommand == this.BeforeCommand && customPID.IsAction == this.IsAction && customPID.Type == this.Type && customPID.Bit == this.Bit && customPID.DataLength == this.DataLength && customPID.IsSigned == this.IsSigned && customPID.Multiplier == this.Multiplier && customPID.Offset == this.Offset && customPID.Role == base.Role && customPID.StartByteId == this.StartByteId;
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x06002807 RID: 10247 RVA: 0x001E9D10 File Offset: 0x001E7F10
		[JsonIgnore]
		public string VagGroup
		{
			get
			{
				if (this.Command != null && this.Command.StartsWith("VWTP:"))
				{
					string[] array = this.Command.Split(VWTPManager.CmdSplitter, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length >= 3)
					{
						return BitHelpers.ConvertHexToInt(array[2].Substring(2)).ToString();
					}
				}
				return "";
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x001E9D6C File Offset: 0x001E7F6C
		[JsonIgnore]
		public string VagUnit
		{
			get
			{
				if (this.Command != null && this.Command.StartsWith("VWTP:"))
				{
					string[] array = this.Command.Split(VWTPManager.CmdSplitter, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length >= 2)
					{
						return VWTPManager.CANAddressToUnit(array[1]);
					}
				}
				return "";
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x06002809 RID: 10249 RVA: 0x001E9DBC File Offset: 0x001E7FBC
		[JsonIgnore]
		public string VagItemInGroup
		{
			get
			{
				return (this.StartByteId + 1).ToString();
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x001E9DD9 File Offset: 0x001E7FD9
		// (set) Token: 0x0600280B RID: 10251 RVA: 0x001E9DE1 File Offset: 0x001E7FE1
		[JsonIgnore]
		public override bool IsAvailable
		{
			get
			{
				return this._IsAvailable;
			}
			set
			{
				this._IsAvailable = value;
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x001E9DEA File Offset: 0x001E7FEA
		// (set) Token: 0x0600280D RID: 10253 RVA: 0x001E9DF2 File Offset: 0x001E7FF2
		[JsonProperty("FR")]
		public string Formula
		{
			get
			{
				return this._Formula;
			}
			set
			{
				if (this._Formula != value)
				{
					this._Formula = value;
					this.ReloadFormula();
				}
				this.NotifyPropertyChanged("Formula");
			}
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x001E9E1C File Offset: 0x001E801C
		public void ReloadFormula()
		{
			if (!string.IsNullOrEmpty(this._Formula))
			{
				if (CustomPIDViewModel.CurrentProfile.Loaded && CustomPIDViewModel.CurrentCustom.Loaded)
				{
					try
					{
						foreach (IPID ipid in this.RequiredPIDs)
						{
							ipid.ValueChanged -= this.RequiredPid_ValueChanged;
						}
						this._RequiredPIDs.Clear();
						if (this._Formula.Contains("{" + base.Name + "}") || this._Formula.Contains(string.Format("PID({0})", base.Id)) || this._Formula.Contains(string.Format("pid({0})", base.Id)))
						{
							this.IsFormulaCorrect = false;
							return;
						}
						List<IPID> dependencyPidsFromPidFormula = CustomPID.GetDependencyPidsFromPidFormula(this._Formula);
						ValueTuple<string, List<IPID>> pidIdsDictFromFormulaString = CustomPID.GetPidIdsDictFromFormulaString(this._Formula);
						string item = pidIdsDictFromFormulaString.Item1;
						List<IPID> item2 = pidIdsDictFromFormulaString.Item2;
						if (dependencyPidsFromPidFormula != null && dependencyPidsFromPidFormula.Count > 0)
						{
							foreach (IPID ipid2 in dependencyPidsFromPidFormula)
							{
								this._RequiredPIDs.Add(ipid2);
							}
						}
						if (item2 != null && item2.Count > 0)
						{
							foreach (IPID ipid3 in item2)
							{
								this._RequiredPIDs.Add(ipid3);
							}
						}
						if (this._RequiredPIDs.Contains(this))
						{
							this._RequiredPIDs.Clear();
							this.IsFormulaCorrect = false;
							return;
						}
						this.engine.setExpressionString(item);
						foreach (IPID ipid4 in this.RequiredPIDs)
						{
							ipid4.ValueChanged -= this.RequiredPid_ValueChanged;
							ipid4.ValueChanged += this.RequiredPid_ValueChanged;
						}
						goto IL_022E;
					}
					catch (Exception)
					{
						this.IsFormulaCorrect = false;
						return;
					}
				}
				this.engine.setExpressionString(this._Formula);
				IL_022E:
				try
				{
					this.FormulaElements = this.GetFormulaElements(this.engine.getExpressionString());
					this.engine.removeAllArguments();
					this.arguments = new Argument[this.FormulaElements.Length];
					for (int i = 0; i < this.arguments.Length; i++)
					{
						this.arguments[i] = new Argument(CustomPID.DICT_LETTERS[this.FormulaElements[i]], 0.0);
					}
					this.engine.addArguments(this.arguments);
					this.IsFormulaCorrect = this.engine.checkSyntax();
					return;
				}
				catch (Exception)
				{
					this.IsFormulaCorrect = false;
					return;
				}
			}
			this.IsFormulaCorrect = false;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x001EA194 File Offset: 0x001E8394
		private void RequiredPid_ValueChanged(object sender, PID e)
		{
			if ((string.IsNullOrEmpty(this.Command) || this.lastData != null) && base.TimeStamp.Ticks != e.TimeStamp.Ticks)
			{
				object obj = this.lockobj;
				lock (obj)
				{
					this.Decode(this.lastData, e.TimeStamp, "");
				}
			}
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x001EA218 File Offset: 0x001E8418
		private int[] GetFormulaElements(string _formula)
		{
			char[] array = new char[]
			{
				' ', '(', ')', '*', '/', '+', '-', '>', '<', '@',
				'!', '^', '~', ',', ' ', ';', '=', '!', '&'
			};
			string[] array2 = _formula.Split(array, StringSplitOptions.RemoveEmptyEntries);
			List<int> list = new List<int>(array2.Length);
			foreach (string text in array2)
			{
				int num = Array.IndexOf<string>(CustomPID.DICT_LETTERS, text);
				if (num >= 0 && !list.Contains(num))
				{
					list.Add(num);
				}
			}
			return list.ToArray();
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x001EA288 File Offset: 0x001E8488
		// (set) Token: 0x06002812 RID: 10258 RVA: 0x001EA290 File Offset: 0x001E8490
		[JsonIgnore]
		public bool IsFormulaCorrect
		{
			get
			{
				return this._IsFormulaCorrect;
			}
			set
			{
				this._IsFormulaCorrect = value;
				this.NotifyPropertyChanged("IsFormulaCorrect");
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x001EA2A4 File Offset: 0x001E84A4
		private Expression engine
		{
			get
			{
				if (this._engine == null)
				{
					this.InitializeEngine();
				}
				return this._engine;
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x001EA2BC File Offset: 0x001E84BC
		private void InitializeEngine()
		{
			mXparser.removeBuiltinTokens(CustomPID.tokensToRemove);
			mXparser.removeBuiltinTokens(CustomPID.DICT_LETTERS);
			this._engine = new Expression("", Array.Empty<PrimitiveElement>());
			this._engine.setSilentMode();
			Function function = new Function("GetBit", new GetBitFunc());
			Function function2 = new Function("BIT", new BITFunc());
			Function function3 = new Function("SIGNED", new SignedFunc());
			Function function4 = new Function("signed", new SignedFunc());
			Function function5 = new Function("Signed", new SignedFunc());
			Function function6 = new Function("ShortSigned", new ShortSignedFunc());
			Function function7 = new Function("And", new BitAndFunc());
			Function function8 = new Function("Shr", new BitShrFunc());
			Function function9 = new Function("Shl", new BitShlFunc());
			Function function10 = new Function("MAX", new MaxFunc());
			Function function11 = new Function("MIN", new MinFunc());
			Function function12 = new Function("ABS", new AbsFunc());
			Function function13 = new Function("FLOAT32", new Float32Func());
			Function function14 = new Function("FLOAT64", new Float64Func());
			Function function15 = new Function("float32", new Float32Func());
			Function function16 = new Function("float64", new Float64Func());
			Function function17 = new Function("int24", new Int24Func());
			Function function18 = new Function("INT24", new Int24Func());
			Function function19 = new Function("int32", new Int32Func());
			Function function20 = new Function("INT32", new Int32Func());
			Function function21 = new Function("int16", new Int16Func());
			Function function22 = new Function("INT16", new Int16Func());
			Function function23 = new Function("IF", new IFCapitalFunc());
			Function function24 = new Function("PID", new GetPidFunc());
			Function function25 = new Function("pid", new GetPidFunc());
			Function function26 = new Function("SetVar", new SetVarFunc());
			Function function27 = new Function("SetVarOnce", new SetVarFunc());
			Function function28 = new Function("GetVar", new GetVarFunc());
			Function function29 = new Function("GetTimestampMs", new GetTimestampMsFunc());
			this.engine.addFunctions(new Function[]
			{
				function, function4, function3, function5, function6, function7, function8, function9, function10, function11,
				function12, function15, function16, function13, function14, function17, function18, function2, function21, function22,
				function19, function20, function23, function24, function25, function26, function27, function28, function29
			});
			byte[] array = new byte[CustomPID.DICT_LETTERS.Length];
			this.engine.addArguments(CustomPID.BuildDictionary(array));
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x001EA5B9 File Offset: 0x001E87B9
		private static double ShortSigned(double A, double B)
		{
			return (double)((short)((int)A * 256 + (int)B));
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x001EA5C8 File Offset: 0x001E87C8
		private static double GetBitDouble(double b, double bitNumber)
		{
			if (BitHelpers.GetBit_1_8((byte)b, (int)bitNumber))
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x001EA5E8 File Offset: 0x001E87E8
		private static double BitAnd(double b, double bitNumber)
		{
			return (double)((int)b & (int)bitNumber);
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x001EA5F0 File Offset: 0x001E87F0
		private static double BitShr(double b, double bitNumber)
		{
			return (double)((int)b >> (int)bitNumber);
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x001EA5FB File Offset: 0x001E87FB
		private static double BitShl(double b, double bitNumber)
		{
			return (double)((int)b << (int)bitNumber);
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x001EA606 File Offset: 0x001E8806
		// (set) Token: 0x0600281B RID: 10267 RVA: 0x001EA60E File Offset: 0x001E880E
		[JsonIgnore]
		public string ActionValue
		{
			[CompilerGenerated]
			get
			{
				return this.<ActionValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ActionValue>k__BackingField = value;
			}
		} = "";

		// Token: 0x0600281C RID: 10268 RVA: 0x001EA618 File Offset: 0x001E8818
		public override void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			base.TimeStamp = timeStamp;
			if (this.IsAction)
			{
				this.ActionValue = BitConverter.ToString(data).Replace("-", " ");
				this.OnValueChanged();
				return;
			}
			try
			{
				this.lastData = data;
				switch (this.Type)
				{
				case CustomPIDType.Formula:
					this.DecodeFormula(data);
					break;
				case CustomPIDType.ByteSet:
					if (this.ReversedByteSet)
					{
						this.DecodeReversedByteSet(data);
					}
					else
					{
						this.DecodeByteSet(data);
					}
					break;
				case CustomPIDType.BitValue:
					this.DecodeBitValue(data);
					break;
				case CustomPIDType.InternalOBD2:
					this.DecodeInternalOBDII(data);
					break;
				case CustomPIDType.VWTPGroupItem:
					this.DecodeVWTPGroupItem(data);
					break;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x001EA6D8 File Offset: 0x001E88D8
		private void DecodeVWTPGroupItem(byte[] data)
		{
			int num = this.StartByteId * 3 + 3;
			if (data.Length < num)
			{
				return;
			}
			ValueTuple<double, UnitsHelper.Units> result = VWTPFormulaManager.GetResult(data[this.StartByteId * 3], data[this.StartByteId * 3 + 1], data[this.StartByteId * 3 + 2]);
			double item = result.Item1;
			UnitsHelper.Units item2 = result.Item2;
			if (this.Units != item2)
			{
				this.Units = item2;
			}
			if (base.Role == Roles.Speed)
			{
				this.Value = item * SharedSettings.Current.SpeedCorrectionFactor;
				return;
			}
			this.Value = item;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x001EA75E File Offset: 0x001E895E
		public void SetInternalFormual(Func<byte[], double> decodeDelegate)
		{
			this.internalFormula = decodeDelegate;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x001EA768 File Offset: 0x001E8968
		private void DecodeInternalOBDII(byte[] data)
		{
			if (this.internalFormula != null)
			{
				double num = this.internalFormula(data);
				this.Value = num;
			}
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x001EA794 File Offset: 0x001E8994
		private void DecodeReversedByteSet(byte[] data)
		{
			if (data.Length < this.StartByteId + this.DataLength)
			{
				return;
			}
			double num = double.NaN;
			int num2 = this.DataLength;
			if (num2 == 0)
			{
				num2 = data.Length - this.StartByteId;
			}
			if (num2 > 4)
			{
				num2 = 4;
			}
			if (this.IsSigned)
			{
				switch (num2)
				{
				case 1:
					num = (double)((sbyte)data[this.StartByteId]);
					break;
				case 2:
					num = (double)((short)((int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId]));
					break;
				case 3:
					num = (double)((int)((sbyte)data[this.StartByteId + 2]) * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId]);
					break;
				case 4:
				{
					byte[] array = new byte[]
					{
						data[this.StartByteId],
						data[this.StartByteId + 1],
						data[this.StartByteId + 2],
						data[this.StartByteId + 3]
					};
					int num3 = 0;
					num = (double)(((int)array[num3++] << 24) | ((int)array[num3++] << 16) | ((int)array[num3++] << 8) | (int)array[num3++]);
					break;
				}
				}
			}
			else
			{
				switch (num2)
				{
				case 1:
					num = (double)data[this.StartByteId];
					break;
				case 2:
					num = (double)((int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId]);
					break;
				case 3:
					num = (double)((int)data[this.StartByteId + 2] * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId]);
					break;
				case 4:
				{
					byte[] array2 = new byte[4];
					Array.Copy(data, this.StartByteId, array2, 0, 4);
					if (!BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array2);
					}
					num = BitConverter.ToUInt32(array2, 0);
					break;
				}
				default:
					num = (double)((int)data[this.StartByteId + 3] * 256 * 256 * 256 + (int)data[this.StartByteId + 2] * 256 * 256 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId]);
					break;
				}
			}
			num *= this.Multiplier;
			num /= this.Divider;
			num += this.Offset;
			if (base.Role == Roles.Speed)
			{
				this.Value = num * SharedSettings.Current.SpeedCorrectionFactor;
				return;
			}
			this.Value = num;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x001EAA0C File Offset: 0x001E8C0C
		private void DecodeBitValue(byte[] data)
		{
			if (this.StartByteId >= data.Length)
			{
				return;
			}
			if (GetBitFunc.GetBit0_7(data[this.StartByteId], this.Bit))
			{
				this.Value = 1.0;
				return;
			}
			this.Value = 0.0;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x001EAA5C File Offset: 0x001E8C5C
		private void DecodeByteSet(byte[] data)
		{
			if (data.Length < this.StartByteId + this.DataLength)
			{
				return;
			}
			double num = double.NaN;
			int num2 = this.DataLength;
			if (num2 == 0)
			{
				num2 = data.Length - this.StartByteId;
			}
			if (num2 > 4)
			{
				num2 = 4;
			}
			if (this.IsSigned)
			{
				switch (num2)
				{
				case 1:
					num = (double)((sbyte)data[this.StartByteId]);
					break;
				case 2:
					num = (double)((short)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]));
					break;
				case 3:
					num = (double)((int)((sbyte)data[this.StartByteId]) * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					int startByteId = this.StartByteId;
					num = (double)(((int)data[startByteId++] << 24) | ((int)data[startByteId++] << 16) | ((int)data[startByteId++] << 8) | (int)data[startByteId++]);
					break;
				}
				}
			}
			else
			{
				switch (num2)
				{
				case 1:
					num = (double)data[this.StartByteId];
					break;
				case 2:
					num = (double)((int)data[this.StartByteId] * 256 + (int)data[this.StartByteId + 1]);
					break;
				case 3:
					num = (double)((int)data[this.StartByteId] * 65536 + (int)data[this.StartByteId + 1] * 256 + (int)data[this.StartByteId + 2]);
					break;
				case 4:
				{
					byte[] array = new byte[4];
					Array.Copy(data, this.StartByteId, array, 0, 4);
					if (BitConverter.IsLittleEndian)
					{
						Array.Reverse<byte>(array);
					}
					num = BitConverter.ToUInt32(array, 0);
					break;
				}
				default:
					num = (double)((int)data[this.StartByteId] * 256 * 256 * 256 + (int)data[this.StartByteId + 1] * 256 * 256 + (int)data[this.StartByteId + 2] * 256 + (int)data[this.StartByteId + 3]);
					break;
				}
			}
			num *= this.Multiplier;
			num /= this.Divider;
			num += this.Offset;
			if (base.Role == Roles.Speed)
			{
				this.Value = num * SharedSettings.Current.SpeedCorrectionFactor;
				return;
			}
			this.Value = num;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x001EAC98 File Offset: 0x001E8E98
		[DebuggerStepThrough]
		private void DecodeFormula(byte[] data)
		{
			try
			{
				if (this.engine == null)
				{
					this.InitializeEngine();
				}
				if (this.BuildDictionaryV2(data))
				{
					double num = this.engine.calculate();
					if (base.Role == Roles.Speed)
					{
						this.Value = num * SharedSettings.Current.SpeedCorrectionFactor;
					}
					else
					{
						this.Value = num;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x001EAD04 File Offset: 0x001E8F04
		public static string GetLetterFromPosition(int position)
		{
			return CustomPID.DICT_LETTERS[position];
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x001EAD0D File Offset: 0x001E8F0D
		public static int GetByteNumberFromLetter(string letter)
		{
			return EnumerableExtensions.IndexOf<string>(CustomPID.DICT_LETTERS, letter);
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x001EAD1C File Offset: 0x001E8F1C
		private bool BuildDictionaryV2(byte[] source_data)
		{
			bool flag = true;
			for (int i = 0; i < this.arguments.Length; i++)
			{
				int num = this.FormulaElements[i];
				if (source_data.Length > num)
				{
					byte b = source_data[num];
					this.arguments[i].setArgumentValue((double)b);
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x001EAD68 File Offset: 0x001E8F68
		private static Argument[] BuildDictionary(byte[] source_data)
		{
			Argument[] array = new Argument[source_data.Length];
			int num = 0;
			while (num < source_data.Length && num < CustomPID.DICT_LETTERS.Length)
			{
				if (array[num] == null)
				{
					array[num] = new Argument(CustomPID.DICT_LETTERS[num], (double)source_data[num]);
				}
				else
				{
					array[num].setArgumentValue((double)source_data[num]);
				}
				num++;
			}
			return array;
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x06002828 RID: 10280 RVA: 0x001EADBD File Offset: 0x001E8FBD
		// (set) Token: 0x06002829 RID: 10281 RVA: 0x001EADC5 File Offset: 0x001E8FC5
		[JsonProperty("UN")]
		public UnitsHelper.Units Units
		{
			get
			{
				return this._Units;
			}
			set
			{
				this._Units = value;
				this.NotifyPropertyChanged("Units");
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x0600282A RID: 10282 RVA: 0x001EADD9 File Offset: 0x001E8FD9
		// (set) Token: 0x0600282B RID: 10283 RVA: 0x001EADE1 File Offset: 0x001E8FE1
		[JsonIgnore]
		public double Value
		{
			get
			{
				return this._Value;
			}
			protected set
			{
				this._Value = value;
				this.OnValueChanged();
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x001EADF0 File Offset: 0x001E8FF0
		private static string ReplaceFromResource(string input)
		{
			string text;
			try
			{
				if (string.IsNullOrEmpty(input))
				{
					text = "";
				}
				else
				{
					int num = input.IndexOf("RES(", StringComparison.Ordinal);
					if (num < 0)
					{
						text = input;
					}
					else
					{
						string text2 = input.Substring(num);
						int num2 = text2.IndexOf(")", StringComparison.Ordinal);
						if (num2 < 0)
						{
							text = input;
						}
						else
						{
							text2 = text2.Substring(0, num2 + 1);
							string resourceString = PID.GetResourceString(text2.Substring(4, text2.Length - 1 - 4));
							if (resourceString == null)
							{
								text = input;
							}
							else
							{
								text = CustomPID.ReplaceFromResource(input.Replace(text2, resourceString));
							}
						}
					}
				}
			}
			catch (Exception)
			{
				text = input;
			}
			return text;
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x001EAE98 File Offset: 0x001E9098
		// (set) Token: 0x0600282E RID: 10286 RVA: 0x001EAEA0 File Offset: 0x001E90A0
		[JsonProperty("BCM")]
		public virtual string BeforeCommand
		{
			get
			{
				return this._BeforeCommand;
			}
			set
			{
				if (value == null)
				{
					this._BeforeCommand = string.Empty;
				}
				else
				{
					this._BeforeCommand = value.ToUpper();
				}
				this._RequiresSTCommands = null;
				this.NotifyPropertyChanged("BeforeCommand");
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x0600282F RID: 10287 RVA: 0x001EAED5 File Offset: 0x001E90D5
		// (set) Token: 0x06002830 RID: 10288 RVA: 0x001EAEDD File Offset: 0x001E90DD
		[JsonProperty("ACM")]
		public virtual string AfterCommand
		{
			get
			{
				return this._AfterCommand;
			}
			set
			{
				if (value == null)
				{
					this._AfterCommand = string.Empty;
				}
				else
				{
					this._AfterCommand = value.ToUpper();
				}
				this._RequiresSTCommands = null;
				this.NotifyPropertyChanged("AfterCommand");
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x06002831 RID: 10289 RVA: 0x001EAF12 File Offset: 0x001E9112
		// (set) Token: 0x06002832 RID: 10290 RVA: 0x000027D4 File Offset: 0x000009D4
		[JsonProperty("ACT")]
		public bool IsAction
		{
			get
			{
				return this.Type == CustomPIDType.Action;
			}
			set
			{
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06002833 RID: 10291 RVA: 0x001EAF1D File Offset: 0x001E911D
		// (set) Token: 0x06002834 RID: 10292 RVA: 0x001EAF25 File Offset: 0x001E9125
		[JsonProperty("TRNS")]
		public ObservableCollection<TranslationItem> Translations
		{
			[CompilerGenerated]
			get
			{
				return this.<Translations>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Translations>k__BackingField = value;
			}
		} = new ObservableCollection<TranslationItem>();

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06002835 RID: 10293 RVA: 0x001EAF30 File Offset: 0x001E9130
		[JsonIgnore]
		public bool RequiresSTCommands
		{
			get
			{
				if (this._RequiresSTCommands == null)
				{
					if (!string.IsNullOrEmpty(this.BeforeCommand))
					{
						foreach (string text in CustomPID.StringToCommands(this.BeforeCommand))
						{
							if (text.StartsWith("ST") || text.StartsWith("st"))
							{
								this._RequiresSTCommands = new bool?(true);
								return true;
							}
						}
					}
					if (!string.IsNullOrEmpty(this.AfterCommand))
					{
						foreach (string text2 in CustomPID.StringToCommands(this.AfterCommand))
						{
							if (text2.StartsWith("ST") || text2.StartsWith("st"))
							{
								this._RequiresSTCommands = new bool?(true);
								return true;
							}
						}
					}
					this._RequiresSTCommands = new bool?(false);
					return false;
				}
				return this._RequiresSTCommands.Value;
			}
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x001EB00C File Offset: 0x001E920C
		internal static string[] StringToCommands(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return new string[0];
			}
			return input.Trim().Split(new char[] { '\\', ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x001EB03C File Offset: 0x001E923C
		private static string[] GetDependencyPidNamesV2(string formula)
		{
			int num = 0;
			for (int i = 0; i < formula.Length; i++)
			{
				char c = formula[i];
				char c2 = '\0';
				if (i > 0)
				{
					c2 = formula[i - 1];
				}
				if ((c == '{' || c == '}') && c2 != '\\')
				{
					num++;
				}
			}
			if (num == 0)
			{
				return CustomPID.empty_string_array;
			}
			if (num % 2 != 0)
			{
				return null;
			}
			string[] array = new string[num / 2];
			int j = 0;
			int num2 = -1;
			int num3 = 0;
			while (j < formula.Length)
			{
				char c3 = '\0';
				if (j > 0)
				{
					c3 = formula[j - 1];
				}
				if (formula[j] == '{' && c3 != '\\')
				{
					if (num2 < 0)
					{
						num2 = j;
					}
				}
				else if (num2 >= 0 && formula[j] == '}' && c3 != '\\')
				{
					int num4 = j - num2;
					string text = formula.Substring(num2 + 1, num4 - 1);
					array[num3] = text;
					num3++;
					num2 = -1;
				}
				j++;
			}
			return array;
		}

		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x06002838 RID: 10296 RVA: 0x001EB126 File Offset: 0x001E9326
		[JsonIgnore]
		public IReadOnlyList<IPID> RequiredPIDs
		{
			get
			{
				return this._RequiredPIDs;
			}
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x001EB130 File Offset: 0x001E9330
		private static List<int> GetDependencyPidIdsFromPidFormula(string _formula)
		{
			List<int> list = null;
			int num = 0;
			for (;;)
			{
				int num2 = _formula.IndexOf("PID(", num, StringComparison.OrdinalIgnoreCase);
				if (num2 < 0)
				{
					return list;
				}
				int num3 = _formula.IndexOf(')', num2);
				if (num3 < 0)
				{
					break;
				}
				if (num3 > 0 && num3 > num)
				{
					int num4 = int.Parse(_formula.Substring(num2 + 4, num3 - (num2 + 4)));
					if (list == null)
					{
						list = new List<int> { num4 };
					}
					list.Add(num4);
				}
				num = num3 + 1;
			}
			throw new ArgumentException("Missing closing bracket in formula");
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x001EB1AC File Offset: 0x001E93AC
		private static List<IPID> GetDependencyPidsFromPidFormula(string _formula)
		{
			List<int> dependencyPidIdsFromPidFormula = CustomPID.GetDependencyPidIdsFromPidFormula(_formula);
			if (dependencyPidIdsFromPidFormula == null)
			{
				return null;
			}
			List<IPID> list = new List<IPID>(dependencyPidIdsFromPidFormula.Count);
			foreach (int num in dependencyPidIdsFromPidFormula)
			{
				IPID ipid = App.OBDReader.CurrentCarData.FindPIDById(num);
				if (ipid != null && ipid is IPIDFloatValue)
				{
					list.Add(ipid);
				}
			}
			return list;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x001EB234 File Offset: 0x001E9434
		private static ValueTuple<string, List<IPID>> GetPidIdsDictFromFormulaString(string formulaString)
		{
			string[] dependencyPidNamesV = CustomPID.GetDependencyPidNamesV2(formulaString);
			if (dependencyPidNamesV == null)
			{
				return new ValueTuple<string, List<IPID>>("", null);
			}
			if (dependencyPidNamesV.Length == 0)
			{
				return new ValueTuple<string, List<IPID>>(formulaString, null);
			}
			string text = formulaString;
			List<IPID> list = new List<IPID>(dependencyPidNamesV.Length);
			foreach (string text2 in dependencyPidNamesV)
			{
				string text3 = text2.Replace("\\{", "{").Replace("\\}", "}");
				IPID ipid = App.OBDReader.CurrentCarData.FindPIDByName(text3);
				if (!list.Contains(ipid))
				{
					list.Add(ipid);
				}
				string text4 = "PID(" + ipid.Id.ToString() + ")";
				text = text.Replace("{" + text2 + "}", text4);
			}
			return new ValueTuple<string, List<IPID>>(text, list);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x001EB318 File Offset: 0x001E9518
		public override void Dispose()
		{
			foreach (IPID ipid in this.RequiredPIDs)
			{
				ipid.ValueChanged -= this.RequiredPid_ValueChanged;
			}
			this._RequiredPIDs.Clear();
			base.Dispose();
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x001EB380 File Offset: 0x001E9580
		// Note: this type is marked as 'beforefieldinit'.
		static CustomPID()
		{
		}

		// Token: 0x04001664 RID: 5732
		private int _StartByteId;

		// Token: 0x04001665 RID: 5733
		private int _DataLength;

		// Token: 0x04001666 RID: 5734
		private double _Multiplier = 1.0;

		// Token: 0x04001667 RID: 5735
		private double _Divider = 1.0;

		// Token: 0x04001668 RID: 5736
		private double _Offset;

		// Token: 0x04001669 RID: 5737
		private bool _IsSigned;

		// Token: 0x0400166A RID: 5738
		private CustomPIDType _Type;

		// Token: 0x0400166B RID: 5739
		private int _Bit;

		// Token: 0x0400166C RID: 5740
		[CompilerGenerated]
		private bool <IsFormulaHidden>k__BackingField;

		// Token: 0x0400166D RID: 5741
		private bool _ReversedByteSet;

		// Token: 0x0400166E RID: 5742
		private int _OrderId = -1;

		// Token: 0x0400166F RID: 5743
		private string _TextValueVariants;

		// Token: 0x04001670 RID: 5744
		protected Dictionary<int, string> _TextValuesDict = new Dictionary<int, string>(0);

		// Token: 0x04001671 RID: 5745
		private bool _IsAvailable = true;

		// Token: 0x04001672 RID: 5746
		private string _Formula = "";

		// Token: 0x04001673 RID: 5747
		private object lockobj = new object();

		// Token: 0x04001674 RID: 5748
		private int[] FormulaElements = new int[0];

		// Token: 0x04001675 RID: 5749
		private bool _IsFormulaCorrect;

		// Token: 0x04001676 RID: 5750
		private Expression _engine;

		// Token: 0x04001677 RID: 5751
		[CompilerGenerated]
		private string <ActionValue>k__BackingField;

		// Token: 0x04001678 RID: 5752
		private byte[] lastData;

		// Token: 0x04001679 RID: 5753
		private Func<byte[], double> internalFormula;

		// Token: 0x0400167A RID: 5754
		public static string[] DICT_LETTERS = new string[]
		{
			"A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
			"K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
			"U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD",
			"AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN",
			"AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX",
			"AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH",
			"BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR",
			"BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ", "CA", "CB",
			"CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL",
			"CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV",
			"CW", "CX", "CY", "CZ", "DA", "DB", "DC", "DD", "DE", "DF",
			"DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP",
			"DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ",
			"EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ",
			"EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET",
			"EU", "EV", "EW", "EX", "EY", "EZ", "FA", "FB", "FC", "FD",
			"FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN",
			"FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX",
			"FY", "FZ"
		};

		// Token: 0x0400167B RID: 5755
		private Argument[] arguments = new Argument[0];

		// Token: 0x0400167C RID: 5756
		private UnitsHelper.Units _Units;

		// Token: 0x0400167D RID: 5757
		private double _Value;

		// Token: 0x0400167E RID: 5758
		private string _BeforeCommand = string.Empty;

		// Token: 0x0400167F RID: 5759
		private string _AfterCommand = string.Empty;

		// Token: 0x04001680 RID: 5760
		private bool _IsAction;

		// Token: 0x04001681 RID: 5761
		[CompilerGenerated]
		private ObservableCollection<TranslationItem> <Translations>k__BackingField;

		// Token: 0x04001682 RID: 5762
		internal static string[] tokensToRemove = new string[]
		{
			"sin", "cos", "tan", "tg", "cot", "ctg", "ctan", "sec", "cosec", "csc",
			"arcsin", "arsin", "asin", "arccos", "arcos", "acos", "arctg", "atg", "arctan", "atan",
			"arccot", "acot", "arcctg", "actg", "arcctan", "actan", "ln", "log2", "log10", "rad",
			"exp", "sqrt", "sinh", "cosh", "tanh", "tgh", "ctgh", "coth", "ctanh", "sech",
			"cosech", "csch", "deg", "abs", "sgn", "floor", "ceil", "not", "arcsinh", "arsinh",
			"asinh", "arccosh", "arcosh", "acosh", "arctgh", "atgh", "arctanh", "atanh", "arcctgh", "actgh",
			"arccoth", "arcoth", "acoth", "arcctanh", "actanh", "arcsech", "arsech", "asech", "arccosech", "arcosech",
			"acosech", "arccsch", "arcsch", "acsch", "Sa", "sinc", "Sinc", "Bell", "Luc", "Fib",
			"harm", "ispr", "Pi", "Ei", "li", "Li", "erf", "erfc", "erfInv", "erfcInv",
			"ulp", "log", "mod", "C", "Bern", "Stirl1", "Stirl2", "Worp", "Euler", "KDelta",
			"EulerPol", "Harm", "rUni", "rUnid", "round", "rNor", "chi", "CHi", "Chi", "cHi",
			"pUni", "cUni", "qUni", "pNor", "cNor", "qNor", "iff", "min", "max", "ConFrac",
			"ConPol", "gcd", "lcm", "add", "multi", "mean", "var", "std", "rList", "sum",
			"prod", "int", "der", "der-", "der+", "dern", "diff", "difb", "avg", "vari",
			"stdi", "mini", "maxi", "solve", "pi", "e", "[gam]", "[phi]", "[PN]", "[B*]",
			"[F'd]", "[F'a]", "[C2]", "[M1]", "[B2]", "[B4]", "[BN'L]", "[Kat]", "[K*]", "[K.]",
			"[B'L]", "[RS'm]", "[EB'e]", "[Bern]", "[GKW'l]", "[HSM's]", "[lm]", "[Cah]", "[Ll]", "[AG]",
			"[L*]", "[L.]", "[Dz3]", "[A3n]", "[Bh]", "[Pt]", "[L2]", "[Nv]", "[Ks]", "[Kh]",
			"[FR]", "[La]", "[P2]", "[Om]", "[MRB]", "[li2]", "[EG]", "[c]", "[G.]", "[g]",
			"[hP]", "[h-]", "[lP]", "[mP]", "[tP]", "[ly]", "[au]", "[pc]", "[kpc]", "[Earth-R-eq]",
			"[Earth-R-po]", "[Earth-R]", "[Earth-M]", "[Earth-D]", "[Moon-R]", "[Moon-M]", "[Moon-D]", "[Solar-R]", "[Solar-M]", "[Mercury-R]",
			"[Mercury-M]", "[Mercury-D]", "[Venus-R]", "[Venus-M]", "[Venus-D]", "[Mars-R]", "[Mars-M]", "[Mars-D]", "[Jupiter-R]", "[Jupiter-M]",
			"[Jupiter-D]", "[Saturn-R]", "[Saturn-M]", "[Saturn-D]", "[Uranus-R]", "[Uranus-M]", "[Uranus-D]", "[Neptune-R]", "[Neptune-M]", "[Neptune-D]",
			"[Uni]", "[Int]", "[Int1]", "[Int2]", "[Int3]", "[Int4]", "[Int5]", "[Int6]", "[Int7]", "[Int8]",
			"[Int9]", "[nat]", "[nat1]", "[nat2]", "[nat3]", "[nat4]", "[nat5]", "[nat6]", "[nat7]", "[nat8]",
			"[nat9]", "[Nat]", "[Nat1]", "[Nat2]", "[Nat3]", "[Nat4]", "[Nat5]", "[Nat6]", "[Nat7]", "[Nat8]",
			"[Nat9]", "[Nor]", "@~", "@&", "@^", "@|", "@<<", "@>>", "[%]", "[%%]",
			"[Y]", "[sept]", "[Z]", "[sext]", "[quint]", "[E]", "[quad]", "[P]", "[T]", "[tril]",
			"[bil]", "[G]", "[M]", "[mil]", "[th]", "[k]", "[hecto]", "[hund]", "[deca]", "[ten]",
			"[deci]", "[centi]", "[milli]", "[mic]", "[n]", "[p]", "[f]", "[a]", "[z]", "[y]",
			"[m]", "[km]", "[cm]", "[mm]", "[inch]", "[yd]", "[ft]", "[mile]", "[nmi]", "[m2]",
			"[cm2]", "[mm2]", "[are]", "[ha]", "[acre]", "[km2]", "[mm3]", "[cm3]", "[m3]", "[km3]",
			"[ml]", "[l]", "[gall]", "[pint]", "[s]", "[ms]", "[min]", "[h]", "[day]", "[week]",
			"[yearj]", "[kg]", "[gr]", "[mg]", "[dag]", "[t]", "[oz]", "[lb]", "[b]", "[kb]",
			"[Mb]", "[Gb]", "[Tb]", "[Pb]", "[Eb]", "[Zb]", "[Yb]", "[B]", "[kB]", "[MB]",
			"[GB]", "[TB]", "[PB]", "[EB]", "[ZB]", "[YB]", "[J]", "[eV]", "[keV]", "[MeV]",
			"[GeV]", "[TeV]", "[m/s]", "[km/h]", "[mi/h]", "[knot]", "[m/s2]", "[km/h2]", "[mi/h2]", "[rad]",
			"[deg]"
		};

		// Token: 0x04001683 RID: 5763
		private static char[] SPLITTER = new char[] { ';', '\\' };

		// Token: 0x04001684 RID: 5764
		private bool? _RequiresSTCommands;

		// Token: 0x04001685 RID: 5765
		private static string[] empty_string_array = new string[0];

		// Token: 0x04001686 RID: 5766
		private List<IPID> _RequiredPIDs = new List<IPID>();

		// Token: 0x020003DF RID: 991
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600283E RID: 10302 RVA: 0x001ECAA5 File Offset: 0x001EACA5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600283F RID: 10303 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002840 RID: 10304 RVA: 0x001ECAB1 File Offset: 0x001EACB1
			internal bool <.ctor>b__0_0(TranslationItem x)
			{
				return x.Language == App.CurrentLanguageCode;
			}

			// Token: 0x04001687 RID: 5767
			public static readonly CustomPID.<>c <>9 = new CustomPID.<>c();

			// Token: 0x04001688 RID: 5768
			public static Func<TranslationItem, bool> <>9__0_0;
		}
	}
}
