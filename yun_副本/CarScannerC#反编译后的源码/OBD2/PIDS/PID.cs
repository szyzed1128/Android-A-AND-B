using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.ABRP;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x02000406 RID: 1030
	public class PID : IDisposable, IPID, INotifyPropertyChanged
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06002A2B RID: 10795 RVA: 0x001F1C8D File Offset: 0x001EFE8D
		// (remove) Token: 0x06002A2C RID: 10796 RVA: 0x001F1CB8 File Offset: 0x001EFEB8
		public event EventHandler<PID> ValueChanged
		{
			add
			{
				if (this._ValueChanged == null || !this._ValueChanged.GetInvocationList().Contains(value))
				{
					this._ValueChanged += value;
					this.ValueChangedSubscriptionAdded();
				}
			}
			remove
			{
				try
				{
					this._ValueChanged -= value;
					if (this._ValueChanged == null || (this._ValueChanged != null && this._ValueChanged.GetInvocationList().Length == 0))
					{
						this.ValueChangedSubscriptionRemoved();
					}
				}
				catch (NullReferenceException)
				{
				}
			}
		}

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06002A2D RID: 10797 RVA: 0x001F1D08 File Offset: 0x001EFF08
		// (remove) Token: 0x06002A2E RID: 10798 RVA: 0x001F1D40 File Offset: 0x001EFF40
		protected event EventHandler<PID> _ValueChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler<PID> eventHandler = this._ValueChanged;
				EventHandler<PID> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PID> eventHandler3 = (EventHandler<PID>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<PID>>(ref this._ValueChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<PID> eventHandler = this._ValueChanged;
				EventHandler<PID> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PID> eventHandler3 = (EventHandler<PID>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<PID>>(ref this._ValueChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x06002A2F RID: 10799 RVA: 0x001F1D78 File Offset: 0x001EFF78
		// (remove) Token: 0x06002A30 RID: 10800 RVA: 0x001F1DB0 File Offset: 0x001EFFB0
		public event EventHandler<PID> IsAvailableChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler<PID> eventHandler = this.IsAvailableChanged;
				EventHandler<PID> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PID> eventHandler3 = (EventHandler<PID>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<PID>>(ref this.IsAvailableChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<PID> eventHandler = this.IsAvailableChanged;
				EventHandler<PID> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PID> eventHandler3 = (EventHandler<PID>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<PID>>(ref this.IsAvailableChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000028 RID: 40
		// (add) Token: 0x06002A31 RID: 10801 RVA: 0x001F1DE8 File Offset: 0x001EFFE8
		// (remove) Token: 0x06002A32 RID: 10802 RVA: 0x001F1E20 File Offset: 0x001F0020
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void ValueChangedSubscriptionAdded()
		{
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void ValueChangedSubscriptionRemoved()
		{
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x001F1E58 File Offset: 0x001F0058
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				if (MainThread.IsMainThread)
				{
					propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
					return;
				}
				Device.BeginInvokeOnMainThread(delegate
				{
					propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
				});
			}
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x001F1EBD File Offset: 0x001F00BD
		public virtual void OnValueChanged()
		{
			EventHandler<PID> valueChanged = this._ValueChanged;
			if (valueChanged == null)
			{
				return;
			}
			valueChanged(this, this);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x001F1ED4 File Offset: 0x001F00D4
		public void OnIsAvailableChanged()
		{
			EventHandler<PID> isAvailableChanged = this.IsAvailableChanged;
			if (isAvailableChanged != null)
			{
				isAvailableChanged(this, this);
			}
		}

		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06002A38 RID: 10808 RVA: 0x001F1EF3 File Offset: 0x001F00F3
		// (set) Token: 0x06002A39 RID: 10809 RVA: 0x001F1EFB File Offset: 0x001F00FB
		[JsonIgnore]
		public TimeSpan TimeStamp
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeStamp>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<TimeStamp>k__BackingField = value;
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06002A3A RID: 10810 RVA: 0x001F1F04 File Offset: 0x001F0104
		// (set) Token: 0x06002A3B RID: 10811 RVA: 0x001F1F28 File Offset: 0x001F0128
		[JsonProperty("NM")]
		public string Name
		{
			get
			{
				string customName = this.CustomName;
				if (!string.IsNullOrEmpty(customName))
				{
					return customName;
				}
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.NotifyPropertyChanged("Name");
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x001F1F3C File Offset: 0x001F013C
		// (set) Token: 0x06002A3D RID: 10813 RVA: 0x001F1F6F File Offset: 0x001F016F
		[JsonProperty("SNM")]
		public string ShortName
		{
			get
			{
				string customShortName = this.CustomShortName;
				if (!string.IsNullOrEmpty(customShortName))
				{
					return customShortName;
				}
				if (this._ShortName == null)
				{
					return this.Name;
				}
				return this._ShortName;
			}
			set
			{
				this._ShortName = value;
				this.NotifyPropertyChanged("ShortName");
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x06002A3E RID: 10814 RVA: 0x001F1F83 File Offset: 0x001F0183
		// (set) Token: 0x06002A3F RID: 10815 RVA: 0x001F1F8B File Offset: 0x001F018B
		[JsonProperty("CMD")]
		public virtual string Command
		{
			get
			{
				return this._Command;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this._Command = value.ToUpper();
				this.NotifyPropertyChanged("Command");
			}
		}

		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x001F1FAE File Offset: 0x001F01AE
		// (set) Token: 0x06002A41 RID: 10817 RVA: 0x001F1FB6 File Offset: 0x001F01B6
		[JsonProperty("HDR")]
		public virtual string Header
		{
			get
			{
				return this._Header;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				this._Header = value.ToUpper();
				this.NotifyPropertyChanged("Header");
			}
		}

		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x001F1FDC File Offset: 0x001F01DC
		// (set) Token: 0x06002A43 RID: 10819 RVA: 0x001F2009 File Offset: 0x001F0209
		[JsonIgnore]
		public string CustomName
		{
			get
			{
				PIDOverride pidoverride;
				if (PIDOverrideDictionary.Instance.TryGetValue(this.Id, out pidoverride))
				{
					return pidoverride.Name;
				}
				return "";
			}
			set
			{
				if (value != this.CustomName)
				{
					PIDOverrideDictionary.Instance.SetProperty(this.Id, "Name", value);
					this.NotifyPropertyChanged(this.CustomName);
					this.NotifyPropertyChanged(this.Name);
				}
			}
		}

		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06002A44 RID: 10820 RVA: 0x001F2048 File Offset: 0x001F0248
		// (set) Token: 0x06002A45 RID: 10821 RVA: 0x001F2075 File Offset: 0x001F0275
		[JsonIgnore]
		public string CustomShortName
		{
			get
			{
				PIDOverride pidoverride;
				if (PIDOverrideDictionary.Instance.TryGetValue(this.Id, out pidoverride))
				{
					return pidoverride.ShortName;
				}
				return "";
			}
			set
			{
				if (value != this.CustomShortName)
				{
					PIDOverrideDictionary.Instance.SetProperty(this.Id, "ShortName", value);
					this.NotifyPropertyChanged(this.CustomShortName);
					this.NotifyPropertyChanged(this.ShortName);
				}
			}
		}

		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06002A46 RID: 10822 RVA: 0x001F20B4 File Offset: 0x001F02B4
		// (set) Token: 0x06002A47 RID: 10823 RVA: 0x001F20E2 File Offset: 0x001F02E2
		[JsonIgnore]
		public int CustomSkipCycles
		{
			get
			{
				PIDOverride pidoverride;
				if (PIDOverrideDictionary.Instance.TryGetValue(this.Id, out pidoverride))
				{
					return pidoverride.SkipCycles;
				}
				return this._SkipCycles;
			}
			set
			{
				if (value != this.CustomSkipCycles)
				{
					PIDOverrideDictionary.Instance.SetProperty(this.Id, "SkipCycles", value);
					this.NotifyPropertyChanged("CustomSkipCycles");
					this.NotifyPropertyChanged("SkipCycles");
				}
			}
		}

		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06002A48 RID: 10824 RVA: 0x001F211C File Offset: 0x001F031C
		// (set) Token: 0x06002A49 RID: 10825 RVA: 0x001F2146 File Offset: 0x001F0346
		[JsonIgnore]
		public Roles CustomRole
		{
			get
			{
				PIDOverride pidoverride;
				if (PIDOverrideDictionary.Instance.TryGetValue(this.Id, out pidoverride))
				{
					return pidoverride.Role;
				}
				return Roles.UNDEFINED;
			}
			set
			{
				if (value != this.CustomRole)
				{
					PIDOverrideDictionary.Instance.SetProperty(this.Id, "Role", value);
					this.NotifyPropertyChanged("CustomRole");
					this.NotifyPropertyChanged("Role");
				}
			}
		}

		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x001F2180 File Offset: 0x001F0380
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x001F21B3 File Offset: 0x001F03B3
		[JsonIgnore]
		public UnitsHelper.Units CustomUnit
		{
			get
			{
				if (!(this is IPIDFloatValue))
				{
					return UnitsHelper.Units.None;
				}
				PIDOverride pidoverride;
				if (PIDOverrideDictionary.Instance.TryGetValue(this.Id, out pidoverride))
				{
					return pidoverride.Unit;
				}
				return UnitsHelper.Units.None;
			}
			set
			{
				if (value != this.CustomUnit)
				{
					PIDOverrideDictionary.Instance.SetProperty(this.Id, "Unit", value);
					this.NotifyPropertyChanged("CustomUnit");
					this.NotifyPropertyChanged("Units");
				}
			}
		}

		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x001F21EA File Offset: 0x001F03EA
		public static PID Empty
		{
			get
			{
				return PID._Empty;
			}
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06002A4D RID: 10829 RVA: 0x001F21F1 File Offset: 0x001F03F1
		// (set) Token: 0x06002A4E RID: 10830 RVA: 0x001F21F9 File Offset: 0x001F03F9
		public virtual bool IsAvailable
		{
			get
			{
				return this._IsAvailable;
			}
			set
			{
				this._IsAvailable = value;
				this.OnIsAvailableChanged();
				this.NotifyPropertyChanged("IsAvailable");
			}
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x001F2213 File Offset: 0x001F0413
		public virtual void Decode(byte[] data, TimeSpan timeStamp, string response_header)
		{
			this.TimeStamp = timeStamp;
			this.OnValueChanged();
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x001F2222 File Offset: 0x001F0422
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x001F222A File Offset: 0x001F042A
		public virtual void Dispose()
		{
			this.IsAvailableChanged = null;
			this.PropertyChanged = null;
			this._ValueChanged = null;
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x000027D4 File Offset: 0x000009D4
		[JsonIgnore]
		public virtual bool HasAnnotation
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x001F2241 File Offset: 0x001F0441
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x001F2249 File Offset: 0x001F0449
		[JsonIgnore]
		public int intCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<intCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<intCommand>k__BackingField = value;
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x001F2254 File Offset: 0x001F0454
		public PID(string Name, string Command)
		{
			this.Name = Name;
			this.Command = Command;
			string text;
			if (Translate.HasString("PID_" + Command + "_Short", out text))
			{
				this.ShortName = text;
			}
			else
			{
				this.ShortName = Name;
			}
			int num = 0;
			if (int.TryParse(Command, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
			{
				this.intCommand = num;
			}
			else
			{
				this.intCommand = -1;
			}
			this.IsAvailable = false;
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x001F22EC File Offset: 0x001F04EC
		// (set) Token: 0x06002A58 RID: 10840 RVA: 0x001F230D File Offset: 0x001F050D
		[JsonProperty("RL")]
		public Roles Role
		{
			get
			{
				Roles customRole = this.CustomRole;
				if (customRole != Roles.UNDEFINED)
				{
					return customRole;
				}
				return this._Role;
			}
			set
			{
				if (this._Role != value)
				{
					this._Role = value;
					this.NotifyPropertyChanged("Role");
				}
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06002A59 RID: 10841 RVA: 0x001F232A File Offset: 0x001F052A
		// (set) Token: 0x06002A5A RID: 10842 RVA: 0x001F2332 File Offset: 0x001F0532
		[JsonProperty("MAX")]
		public double Maximum
		{
			get
			{
				return this._Maximum;
			}
			set
			{
				this._Maximum = value;
				this.NotifyPropertyChanged("Maximum");
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x001F2346 File Offset: 0x001F0546
		// (set) Token: 0x06002A5C RID: 10844 RVA: 0x001F234E File Offset: 0x001F054E
		[JsonProperty("MIN")]
		public double Minimum
		{
			get
			{
				return this._Minimum;
			}
			set
			{
				this._Minimum = value;
				this.NotifyPropertyChanged("Minimum");
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06002A5D RID: 10845 RVA: 0x001F2362 File Offset: 0x001F0562
		// (set) Token: 0x06002A5E RID: 10846 RVA: 0x001F236A File Offset: 0x001F056A
		public int Id
		{
			get
			{
				return this._Id;
			}
			set
			{
				this._Id = value;
				this.NotifyPropertyChanged("Id");
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06002A5F RID: 10847 RVA: 0x001F237E File Offset: 0x001F057E
		// (set) Token: 0x06002A60 RID: 10848 RVA: 0x001F2386 File Offset: 0x001F0586
		[JsonIgnore]
		public bool ShouldHideValue
		{
			get
			{
				return this._ShouldHideValue;
			}
			set
			{
				this._ShouldHideValue = value;
				this.NotifyPropertyChanged("ShouldHideValue");
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06002A61 RID: 10849 RVA: 0x001F239A File Offset: 0x001F059A
		// (set) Token: 0x06002A62 RID: 10850 RVA: 0x001F23A2 File Offset: 0x001F05A2
		[JsonProperty("VIS")]
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			set
			{
				this._IsVisible = value;
				this.NotifyPropertyChanged("IsVisible");
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x001F23B6 File Offset: 0x001F05B6
		// (set) Token: 0x06002A64 RID: 10852 RVA: 0x001F23BE File Offset: 0x001F05BE
		public int SkipCycles
		{
			get
			{
				return this.CustomSkipCycles;
			}
			set
			{
				this._SkipCycles = value;
				this.NotifyPropertyChanged("SkipCycles");
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06002A65 RID: 10853 RVA: 0x001F23D2 File Offset: 0x001F05D2
		[JsonIgnore]
		public string OriginalName
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x001F23DA File Offset: 0x001F05DA
		[JsonIgnore]
		public string OriginalShortName
		{
			get
			{
				return this._ShortName;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x001F23E2 File Offset: 0x001F05E2
		[JsonIgnore]
		public Roles OriginalRole
		{
			get
			{
				return this._Role;
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x001F23EA File Offset: 0x001F05EA
		[JsonIgnore]
		public int OriginalSkipCycles
		{
			get
			{
				return this._SkipCycles;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x001F23F2 File Offset: 0x001F05F2
		// (set) Token: 0x06002A6A RID: 10858 RVA: 0x001F23FA File Offset: 0x001F05FA
		public ABRPRoles ABRPRole
		{
			get
			{
				return this._ABRPRole;
			}
			set
			{
				this._ABRPRole = value;
				this.NotifyPropertyChanged("ABRPRole");
			}
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x001F240E File Offset: 0x001F060E
		public static string GetResourceString(string key)
		{
			return Translate.GetString(key);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x001F2416 File Offset: 0x001F0616
		// Note: this type is marked as 'beforefieldinit'.
		static PID()
		{
		}

		// Token: 0x040017D4 RID: 6100
		[CompilerGenerated]
		private EventHandler<PID> _ValueChanged;

		// Token: 0x040017D5 RID: 6101
		[CompilerGenerated]
		private EventHandler<PID> IsAvailableChanged;

		// Token: 0x040017D6 RID: 6102
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040017D7 RID: 6103
		[CompilerGenerated]
		private TimeSpan <TimeStamp>k__BackingField;

		// Token: 0x040017D8 RID: 6104
		private string _Name;

		// Token: 0x040017D9 RID: 6105
		private string _ShortName;

		// Token: 0x040017DA RID: 6106
		private string _Command;

		// Token: 0x040017DB RID: 6107
		private string _Header = string.Empty;

		// Token: 0x040017DC RID: 6108
		private static PID _Empty = new PID(Translate.GetString("pid_Empty"), string.Empty)
		{
			Id = -1
		};

		// Token: 0x040017DD RID: 6109
		private bool _IsAvailable;

		// Token: 0x040017DE RID: 6110
		[CompilerGenerated]
		private int <intCommand>k__BackingField;

		// Token: 0x040017DF RID: 6111
		private Roles _Role;

		// Token: 0x040017E0 RID: 6112
		private double _Maximum = 100.0;

		// Token: 0x040017E1 RID: 6113
		private double _Minimum;

		// Token: 0x040017E2 RID: 6114
		private int _Id;

		// Token: 0x040017E3 RID: 6115
		private ABRPRoles _ABRPRole;

		// Token: 0x040017E4 RID: 6116
		private int _SkipCycles;

		// Token: 0x040017E5 RID: 6117
		private bool _IsVisible = true;

		// Token: 0x040017E6 RID: 6118
		private bool _ShouldHideValue;

		// Token: 0x02000407 RID: 1031
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			// Token: 0x06002A6D RID: 10861 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass14_0()
			{
			}

			// Token: 0x06002A6E RID: 10862 RVA: 0x001F2438 File Offset: 0x001F0638
			internal void <NotifyPropertyChanged>b__0()
			{
				this.propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.PropertyName));
			}

			// Token: 0x040017E7 RID: 6119
			public PropertyChangedEventHandler propertyChanged;

			// Token: 0x040017E8 RID: 6120
			public PID <>4__this;

			// Token: 0x040017E9 RID: 6121
			public string PropertyName;
		}
	}
}
