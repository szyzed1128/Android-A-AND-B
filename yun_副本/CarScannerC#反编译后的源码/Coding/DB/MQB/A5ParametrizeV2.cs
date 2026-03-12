using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A7F RID: 2687
	internal class A5ParametrizeV2 : A5Parametrize
	{
		// Token: 0x060054C5 RID: 21701 RVA: 0x004053C0 File Offset: 0x004035C0
		public A5ParametrizeV2()
		{
			base.Name = Translate.GetString("codingDB_a5_datasetcustomization");
			base.Description = Translate.GetString("codingDB_FPAEditor_Description") + "\n" + base.Description;
			base.ValueType = AdaptationValueTypes.MQBA5Customization;
			this.DefaultMaxBlockSize = 258U;
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x00405437 File Offset: 0x00403637
		// (set) Token: 0x060054C7 RID: 21703 RVA: 0x0040543F File Offset: 0x0040363F
		public ObservableCollection<ProxyFile> AvailableDatasets
		{
			[CompilerGenerated]
			get
			{
				return this.<AvailableDatasets>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<AvailableDatasets>k__BackingField = value;
			}
		} = new ObservableCollection<ProxyFile>();

		// Token: 0x060054C8 RID: 21704 RVA: 0x00405448 File Offset: 0x00403648
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			base.Options.Clear();
			try
			{
				this.AvailableDatasets.Clear();
			}
			catch (Exception)
			{
			}
			await base.GetIdents();
			string partNumber = this.PartNumber;
			if (partNumber != null)
			{
				int length = partNumber.Length;
				if (length != 9)
				{
					if (length != 10)
					{
						goto IL_0201;
					}
					char c = partNumber[9];
					if (c != 'A')
					{
						switch (c)
						{
						case 'F':
							if (!(partNumber == "3Q0980653F"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.F;
							goto IL_0201;
						case 'G':
							if (!(partNumber == "3Q0980654G"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.G;
							goto IL_0201;
						case 'H':
							if (!(partNumber == "3Q0980654H"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.H;
							goto IL_0201;
						case 'I':
						case 'J':
						case 'K':
							goto IL_0201;
						case 'L':
							if (!(partNumber == "3Q0980654L"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.L;
							goto IL_0201;
						case 'M':
							if (!(partNumber == "3Q0980654M"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.M;
							goto IL_0201;
						default:
							if (c != 'S')
							{
								goto IL_0201;
							}
							if (!(partNumber == "3Q0980654S"))
							{
								goto IL_0201;
							}
							this.CameraVersion = A5DatasetCustomizer.CameraVersions.S;
							goto IL_0201;
						}
					}
					else if (!(partNumber == "3QD980654A"))
					{
						goto IL_0201;
					}
				}
				else if (!(partNumber == "3QD980654"))
				{
					goto IL_0201;
				}
				string firmwareVersion = this.FirmwareVersion;
				if (!(firmwareVersion == "1272"))
				{
					if (firmwareVersion == "1611")
					{
						this.CameraVersion = A5DatasetCustomizer.CameraVersions.L;
					}
				}
				else
				{
					this.CameraVersion = A5DatasetCustomizer.CameraVersions.H;
				}
			}
			IL_0201:
			this.LoadDatasetsList();
			CodingRequestResult codingRequestResult;
			if (this.CameraVersion == A5DatasetCustomizer.CameraVersions.Unknown)
			{
				base.CurrentState = string.Concat(new string[] { "Detected HW: ", this.PartNumber, " SW: ", this.FirmwareVersion, " NOT SUPPORTED YET!" });
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				base.CurrentState = "Detected HW: " + this.PartNumber + " SW: " + this.FirmwareVersion;
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x060054C9 RID: 21705 RVA: 0x0040548C File Offset: 0x0040368C
		private void LoadDatasetsList()
		{
			this.AvailableDatasets.Clear();
			switch (this.CameraVersion)
			{
			case A5DatasetCustomizer.CameraVersions.H:
				this.path = "vag.a5_3q0_h";
				break;
			case A5DatasetCustomizer.CameraVersions.L:
				this.path = "vag.a5_3q0_l";
				break;
			case A5DatasetCustomizer.CameraVersions.F:
				this.path = "vag.a5_3q0_f";
				break;
			case A5DatasetCustomizer.CameraVersions.G:
				this.path = "vag.a5_3q0_g";
				break;
			case A5DatasetCustomizer.CameraVersions.S:
				this.path = "vag.a5_3q0_s";
				break;
			case A5DatasetCustomizer.CameraVersions.M:
				this.path = "vag.a5_3q0_m";
				break;
			}
			if (string.IsNullOrEmpty(this.path))
			{
				return;
			}
			foreach (string text in PackageFileReader.GetFilesInDirectory(this.path))
			{
				ProxyFile proxyFile = new ProxyFile
				{
					Filepath = this.path + "." + text,
					Title = Path.GetFileNameWithoutExtension(text.Replace('_', ' '))
				};
				this.AvailableDatasets.Add(proxyFile);
			}
			this.SelectedDataset = this.AvailableDatasets[0];
			base.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Apply"), MQBAdaptationTemplate.EnableOption.Value));
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x060054CA RID: 21706 RVA: 0x004055BB File Offset: 0x004037BB
		// (set) Token: 0x060054CB RID: 21707 RVA: 0x004055C3 File Offset: 0x004037C3
		public ProxyFile SelectedDataset
		{
			get
			{
				return this._SelectedDataset;
			}
			set
			{
				if (value != this._SelectedDataset)
				{
					this._SelectedDataset = value;
					if (value != null)
					{
						this.LoadDataset(value.Filepath);
					}
					base.OnPropertyChanged("SelectedDataset");
				}
			}
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x004055F0 File Offset: 0x004037F0
		private void LoadDataset(string filepath)
		{
			using (Stream stream = PackageFileReader.OpenFileStream(filepath))
			{
				byte[] array = new byte[stream.Length];
				stream.Read(array, 0, array.Length);
				this.Customizer.Load(array, this.CameraVersion);
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x060054CD RID: 21709 RVA: 0x0040564C File Offset: 0x0040384C
		// (set) Token: 0x060054CE RID: 21710 RVA: 0x00405654 File Offset: 0x00403854
		public A5DatasetCustomizer Customizer
		{
			[CompilerGenerated]
			get
			{
				return this.<Customizer>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Customizer>k__BackingField = value;
			}
		} = new A5DatasetCustomizer();

		// Token: 0x060054CF RID: 21711 RVA: 0x00405660 File Offset: 0x00403860
		public override Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			string text = BitHelpers.ByteArrayToHexString(this.Customizer.Build());
			return base.Execute(password, text, UserFriendlyValue, progress, originalData, skipIfTheSameData);
		}

		// Token: 0x040033E7 RID: 13287
		private A5DatasetCustomizer.CameraVersions CameraVersion;

		// Token: 0x040033E8 RID: 13288
		[CompilerGenerated]
		private ObservableCollection<ProxyFile> <AvailableDatasets>k__BackingField;

		// Token: 0x040033E9 RID: 13289
		private string path = "";

		// Token: 0x040033EA RID: 13290
		private ProxyFile _SelectedDataset;

		// Token: 0x040033EB RID: 13291
		[CompilerGenerated]
		private A5DatasetCustomizer <Customizer>k__BackingField;

		// Token: 0x02000A80 RID: 2688
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__6 : IAsyncStateMachine
		{
			// Token: 0x060054D0 RID: 21712 RVA: 0x00405690 File Offset: 0x00403890
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				A5ParametrizeV2 a5ParametrizeV = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						a5ParametrizeV.Options.Clear();
						try
						{
							a5ParametrizeV.AvailableDatasets.Clear();
						}
						catch (Exception)
						{
						}
						taskAwaiter = a5ParametrizeV.GetIdents().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, A5ParametrizeV2.<UpdateCurrentState>d__6>(ref taskAwaiter, ref this);
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
					string partNumber = a5ParametrizeV.PartNumber;
					if (partNumber != null)
					{
						int length = partNumber.Length;
						if (length != 9)
						{
							if (length != 10)
							{
								goto IL_0201;
							}
							char c = partNumber[9];
							if (c != 'A')
							{
								switch (c)
								{
								case 'F':
									if (!(partNumber == "3Q0980653F"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.F;
									goto IL_0201;
								case 'G':
									if (!(partNumber == "3Q0980654G"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.G;
									goto IL_0201;
								case 'H':
									if (!(partNumber == "3Q0980654H"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.H;
									goto IL_0201;
								case 'I':
								case 'J':
								case 'K':
									goto IL_0201;
								case 'L':
									if (!(partNumber == "3Q0980654L"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.L;
									goto IL_0201;
								case 'M':
									if (!(partNumber == "3Q0980654M"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.M;
									goto IL_0201;
								default:
									if (c != 'S')
									{
										goto IL_0201;
									}
									if (!(partNumber == "3Q0980654S"))
									{
										goto IL_0201;
									}
									a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.S;
									goto IL_0201;
								}
							}
							else if (!(partNumber == "3QD980654A"))
							{
								goto IL_0201;
							}
						}
						else if (!(partNumber == "3QD980654"))
						{
							goto IL_0201;
						}
						string firmwareVersion = a5ParametrizeV.FirmwareVersion;
						if (!(firmwareVersion == "1272"))
						{
							if (firmwareVersion == "1611")
							{
								a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.L;
							}
						}
						else
						{
							a5ParametrizeV.CameraVersion = A5DatasetCustomizer.CameraVersions.H;
						}
					}
					IL_0201:
					a5ParametrizeV.LoadDatasetsList();
					if (a5ParametrizeV.CameraVersion == A5DatasetCustomizer.CameraVersions.Unknown)
					{
						a5ParametrizeV.CurrentState = string.Concat(new string[] { "Detected HW: ", a5ParametrizeV.PartNumber, " SW: ", a5ParametrizeV.FirmwareVersion, " NOT SUPPORTED YET!" });
						codingRequestResult = CodingRequestResult.NotSupported;
					}
					else
					{
						a5ParametrizeV.CurrentState = "Detected HW: " + a5ParametrizeV.PartNumber + " SW: " + a5ParametrizeV.FirmwareVersion;
						codingRequestResult = CodingRequestResult.Success;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060054D1 RID: 21713 RVA: 0x00405974 File Offset: 0x00403B74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040033EC RID: 13292
			public int <>1__state;

			// Token: 0x040033ED RID: 13293
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040033EE RID: 13294
			public A5ParametrizeV2 <>4__this;

			// Token: 0x040033EF RID: 13295
			private TaskAwaiter <>u__1;
		}
	}
}
