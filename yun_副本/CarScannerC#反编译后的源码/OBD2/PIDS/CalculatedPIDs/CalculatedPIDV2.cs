using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200043C RID: 1084
	public class CalculatedPIDV2 : PID, IPID, INotifyPropertyChanged, IPIDFloatValue, IPIDWithRequiredPids
	{
		// Token: 0x06002DCC RID: 11724 RVA: 0x00200ACC File Offset: 0x001FECCC
		protected CalculatedPIDV2(string name, string command = "", UnitsHelper.Units units = UnitsHelper.Units.None, double min = 0.0, double max = 1.0, Roles role = Roles.None)
			: base(name, command)
		{
			this.Units = units;
			this.Command = command;
			base.Minimum = min;
			base.Maximum = max;
			base.Role = role;
		}

		// Token: 0x17001252 RID: 4690
		// (get) Token: 0x06002DCD RID: 11725 RVA: 0x00200B30 File Offset: 0x001FED30
		public IReadOnlyList<IPID> LowPriorityRequiredPIDs
		{
			get
			{
				return this.lowPriorityRequiredPIDs;
			}
		}

		// Token: 0x17001253 RID: 4691
		// (get) Token: 0x06002DCE RID: 11726 RVA: 0x00200B38 File Offset: 0x001FED38
		public IReadOnlyList<IPID> RequiredPIDs
		{
			get
			{
				return this.requiredPIDs;
			}
		}

		// Token: 0x17001254 RID: 4692
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x00200B40 File Offset: 0x001FED40
		// (set) Token: 0x06002DD0 RID: 11728 RVA: 0x00200B48 File Offset: 0x001FED48
		protected IPIDFloatValue DependencyPID
		{
			get
			{
				return this._DependancyPID;
			}
			set
			{
				if (this._DependancyPID != null)
				{
					this._DependancyPID.ValueChanged -= this.DependencyPID_ValueChanged;
				}
				if (value != null)
				{
					this._DependancyPID = value;
					this._DependancyPID.ValueChanged -= this.DependencyPID_ValueChanged;
					this._DependancyPID.ValueChanged += this.DependencyPID_ValueChanged;
					if (!this.requiredPIDs.Contains(value))
					{
						this.requiredPIDs.Add(value);
						return;
					}
				}
				else
				{
					this.IsAvailable = false;
				}
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x00200BD0 File Offset: 0x001FEDD0
		private void DependencyPID_ValueChanged(object sender, PID e)
		{
			try
			{
				if (e != null)
				{
					if (!(e.TimeStamp == base.TimeStamp))
					{
						base.TimeStamp = e.TimeStamp;
						bool flag = true;
						foreach (IPID ipid in this.requiredPIDs)
						{
							if (ipid == null)
							{
								flag = false;
								break;
							}
							TimeSpan timeStamp = ipid.TimeStamp;
							if (Math.Abs(base.TimeStamp.Ticks - timeStamp.Ticks) > 50000000L)
							{
								if (!(this is PID_CalculatedInstantFuelRate) || ipid.Role != Roles.LAMBDA)
								{
									flag = false;
									break;
								}
								break;
							}
						}
						double num;
						if (flag && this.Calculate((IPIDFloatValue)e, out num))
						{
							this.Value = num;
						}
						if (this.LowPriorityCounter == 0)
						{
							foreach (IPID ipid2 in this.lowPriorityRequiredPIDs)
							{
								if (ipid2 != null && ipid2.IsAvailable)
								{
									App.OBDReader.AddRequestToQueue(new OBDRequest(ipid2.Command, false, (PID)ipid2));
								}
							}
						}
						this.LowPriorityCounter++;
						if (this.LowPriorityCounter > this.LowPriorityInterval)
						{
							this.LowPriorityCounter = 0;
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x00200D74 File Offset: 0x001FEF74
		protected virtual bool Calculate(IPIDFloatValue pid, out double result)
		{
			throw new NotImplementedException("Calculate not implemented in " + base.Name + " " + this.Command);
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x00200D96 File Offset: 0x001FEF96
		protected void SetValue(double val)
		{
			this.Value = val;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x00200D9F File Offset: 0x001FEF9F
		public virtual void Initialize()
		{
			this.CheckAndSetAvailable();
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x00200DA8 File Offset: 0x001FEFA8
		protected virtual void CheckAndSetAvailable()
		{
			IPID[] array = this.requiredPIDs.Concat(this.lowPriorityRequiredPIDs).ToArray<IPID>();
			if (array.Length == 0)
			{
				this.IsAvailable = false;
				return;
			}
			foreach (IPID ipid in array)
			{
				if (ipid == null)
				{
					this.IsAvailable = false;
					return;
				}
				if (!ipid.IsAvailable)
				{
					this.IsAvailable = false;
					return;
				}
			}
			this.IsAvailable = true;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x00200E0E File Offset: 0x001FF00E
		public virtual void ResetValues()
		{
			this.Value = 0.0;
		}

		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x00200E1F File Offset: 0x001FF01F
		// (set) Token: 0x06002DD8 RID: 11736 RVA: 0x00200E27 File Offset: 0x001FF027
		public double Value
		{
			get
			{
				return this._Value;
			}
			private set
			{
				this._Value = value;
				this.NotifyPropertyChanged("Value");
				this.OnValueChanged();
			}
		}

		// Token: 0x17001256 RID: 4694
		// (get) Token: 0x06002DD9 RID: 11737 RVA: 0x00200E41 File Offset: 0x001FF041
		// (set) Token: 0x06002DDA RID: 11738 RVA: 0x00200E49 File Offset: 0x001FF049
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

		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06002DDB RID: 11739 RVA: 0x00200E5D File Offset: 0x001FF05D
		// (set) Token: 0x06002DDC RID: 11740 RVA: 0x00200E65 File Offset: 0x001FF065
		public string TextValueVariants
		{
			[CompilerGenerated]
			get
			{
				return this.<TextValueVariants>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TextValueVariants>k__BackingField = value;
			}
		} = "";

		// Token: 0x06002DDD RID: 11741 RVA: 0x00200E70 File Offset: 0x001FF070
		public virtual void RequestLowPriorityPIDs(bool ForceRequest = false)
		{
			this.LowPriorityCounter++;
			if (this.LowPriorityCounter >= this.LowPriorityInterval || ForceRequest)
			{
				this.LowPriorityCounter = 0;
				foreach (IPID ipid in this.lowPriorityRequiredPIDs)
				{
					if (ipid.IsAvailable)
					{
						if (ipid is CalculatedPIDV2)
						{
							List<OBDRequest> list = new List<OBDRequest>(2);
							LiveDataPIDModel.GetRequests(ipid, list, null, "");
							using (List<OBDRequest>.Enumerator enumerator2 = list.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									OBDRequest obdrequest = enumerator2.Current;
									obdrequest.Repeat = false;
									App.OBDReader.AddRequestToQueue(obdrequest);
								}
								continue;
							}
						}
						App.OBDReader.AddRequestToQueue(ipid.Command);
					}
				}
			}
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000027D4 File Offset: 0x000009D4
		void IPIDFloatValue.SetValue(double value)
		{
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001FED0B File Offset: 0x001FCF0B
		public string GetTextValueVariantOrNull(double value)
		{
			return null;
		}

		// Token: 0x040019C8 RID: 6600
		protected int LowPriorityInterval = 15;

		// Token: 0x040019C9 RID: 6601
		private int LowPriorityCounter;

		// Token: 0x040019CA RID: 6602
		protected List<IPID> lowPriorityRequiredPIDs = new List<IPID>();

		// Token: 0x040019CB RID: 6603
		protected List<IPID> requiredPIDs = new List<IPID>();

		// Token: 0x040019CC RID: 6604
		private IPIDFloatValue _DependancyPID;

		// Token: 0x040019CD RID: 6605
		private double _Value;

		// Token: 0x040019CE RID: 6606
		[CompilerGenerated]
		private string <TextValueVariants>k__BackingField;

		// Token: 0x040019CF RID: 6607
		private UnitsHelper.Units _Units;
	}
}
