using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008AD RID: 2221
	internal class MQBEasyLightCoding : MQBEasyCodingItem
	{
		// Token: 0x06004B7A RID: 19322 RVA: 0x00384056 File Offset: 0x00382256
		public MQBEasyLightCoding(string Address, Action<MQB_LightConfiguration> enableAction, Action<MQB_LightConfiguration> disableAction, Func<MQB_LightConfiguration, bool> checkState)
			: this("", "", Address, enableAction, disableAction, checkState)
		{
		}

		// Token: 0x06004B7B RID: 19323 RVA: 0x00384070 File Offset: 0x00382270
		public MQBEasyLightCoding(string Name, string Description, string Address, Action<MQB_LightConfiguration> enableAction, Action<MQB_LightConfiguration> disableAction, Func<MQB_LightConfiguration, bool> checkState)
			: base(CodingGroup.ExteriorLights, Name, Description, Address, "70E", "778", "31347", "", delegate(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
				mqb_LightConfiguration.LoadFromData(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					enableAction(mqb_LightConfiguration);
				}
				else
				{
					disableAction(mqb_LightConfiguration);
				}
				return mqb_LightConfiguration.ApplyToData();
			}, delegate(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (checkState == null)
				{
					return codingItem.DefaultGetStateDelegate(data, codingItem);
				}
				MQB_LightConfiguration mqb_LightConfiguration2 = new MQB_LightConfiguration(data);
				if (checkState(mqb_LightConfiguration2))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			})
		{
		}

		// Token: 0x020008AE RID: 2222
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06004B7C RID: 19324 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06004B7D RID: 19325 RVA: 0x003840E8 File Offset: 0x003822E8
			internal byte[] <.ctor>b__0(byte[] data, string value, MQBEasyCodingItem coding)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration();
				mqb_LightConfiguration.LoadFromData(array);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					this.enableAction(mqb_LightConfiguration);
				}
				else
				{
					this.disableAction(mqb_LightConfiguration);
				}
				return mqb_LightConfiguration.ApplyToData();
			}

			// Token: 0x06004B7E RID: 19326 RVA: 0x0038414C File Offset: 0x0038234C
			internal string <.ctor>b__1(byte[] data, MQBEasyCodingItem codingItem)
			{
				if (this.checkState == null)
				{
					return codingItem.DefaultGetStateDelegate(data, codingItem);
				}
				MQB_LightConfiguration mqb_LightConfiguration = new MQB_LightConfiguration(data);
				if (this.checkState(mqb_LightConfiguration))
				{
					return MQBAdaptationTemplate.EnableOption.Title;
				}
				return MQBAdaptationTemplate.DisableOption.Title;
			}

			// Token: 0x04002C27 RID: 11303
			public Action<MQB_LightConfiguration> enableAction;

			// Token: 0x04002C28 RID: 11304
			public Action<MQB_LightConfiguration> disableAction;

			// Token: 0x04002C29 RID: 11305
			public Func<MQB_LightConfiguration, bool> checkState;
		}
	}
}
