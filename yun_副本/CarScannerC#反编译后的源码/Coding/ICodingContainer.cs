using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200087F RID: 2175
	public interface ICodingContainer : INotifyPropertyChanged
	{
		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x06004A13 RID: 18963
		// (set) Token: 0x06004A14 RID: 18964
		CodingGroup Group { get; set; }

		// Token: 0x170016A5 RID: 5797
		// (get) Token: 0x06004A15 RID: 18965
		// (set) Token: 0x06004A16 RID: 18966
		string Name { get; set; }

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x06004A17 RID: 18967
		string Description { get; }

		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x06004A18 RID: 18968
		string InnerDescription { get; }

		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x06004A19 RID: 18969
		string CurrentState { get; }

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x06004A1A RID: 18970
		bool PasswordVisible { get; }

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x06004A1B RID: 18971
		// (set) Token: 0x06004A1C RID: 18972
		bool HasCurrentState { get; set; }

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06004A1D RID: 18973
		// (set) Token: 0x06004A1E RID: 18974
		string Password { get; set; }

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06004A1F RID: 18975
		string PasswordHint { get; }

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06004A20 RID: 18976
		ObservableCollection<MQBAdaptationOption> Options { get; }

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06004A21 RID: 18977
		// (set) Token: 0x06004A22 RID: 18978
		bool RequiresPro { get; set; }

		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06004A23 RID: 18979
		AdaptationValueTypes ValueType { get; }

		// Token: 0x06004A24 RID: 18980
		Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null);

		// Token: 0x06004A25 RID: 18981
		Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false);
	}
}
