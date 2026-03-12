using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x02000573 RID: 1395
	internal static class ReportGenerator
	{
		// Token: 0x0600337C RID: 13180 RVA: 0x00242054 File Offset: 0x00240254
		public static string CreateReport(IEnumerable<IECU> ecus)
		{
			ecus.ToList<IECU>();
			string appMarketTag = PlatformHelper.AppMarketTag;
			DTCCodeToStringConverter dtccodeToStringConverter = new DTCCodeToStringConverter();
			DTCStatusCollectionToStringConverter dtcstatusCollectionToStringConverter = new DTCStatusCollectionToStringConverter();
			DTCDescriptionCollectionToStringConverter dtcdescriptionCollectionToStringConverter = new DTCDescriptionCollectionToStringConverter();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				"Car Scanner ELM OBD2\nVersion: ",
				App.Version,
				"/",
				App.Build,
				"/",
				appMarketTag,
				"\nDTC report\nConnection profile: ",
				SharedSettings.Current.BrandAndProfile
			}));
			stringBuilder.Append("Date: " + DateTimeNowHelper.NowSafe.ToString());
			if (CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.IsVINAvailable && !string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
			{
				stringBuilder.Append("\nVIN: " + CarInfoViewModel.Instance.VIN);
			}
			foreach (IECU iecu in ecus)
			{
				stringBuilder.Append("\n============================\n");
				stringBuilder.Append(iecu.Name);
				if (iecu.DTCCollection == null || iecu.DTCCollection.Count == 0)
				{
					stringBuilder.Append('\n');
					stringBuilder.Append(Translate.GetString("ios_NoDTC"));
				}
				else
				{
					stringBuilder.Append(iecu.Name);
					stringBuilder.Append("\nDTCs: " + iecu.DTCCollection.Count.ToString());
					foreach (DTCItemV2 dtcitemV in iecu.DTCCollection)
					{
						stringBuilder.Append("\n----------------------------");
						stringBuilder.Append("\n" + (string)dtccodeToStringConverter.Convert(dtcitemV.Code, null, null, null));
						stringBuilder.Append(" [0x" + dtcitemV.RawCode + "]");
						if (dtcitemV.IsArchive)
						{
							stringBuilder.Append(" " + Translate.GetString("ios_Archive"));
						}
						if (dtcitemV.DescriptionsVisible)
						{
							stringBuilder.Append("\n");
							foreach (Span span in ((FormattedString)dtcdescriptionCollectionToStringConverter.Convert(dtcitemV.Descriptions, null, null, null)).Spans)
							{
								stringBuilder.Append(span.Text);
							}
						}
						if (!string.IsNullOrEmpty(dtcitemV.Payload))
						{
							stringBuilder.Append("\n");
							stringBuilder.Append(dtcitemV.Payload);
						}
						if (dtcitemV.StatusVisible && dtcitemV.Statuses != null && dtcitemV.Statuses.Count > 0)
						{
							stringBuilder.Append("\n");
							foreach (Span span2 in ((FormattedString)dtcstatusCollectionToStringConverter.Convert(dtcitemV.Statuses, null, null, null)).Spans)
							{
								stringBuilder.Append(span2.Text);
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x00242424 File Offset: 0x00240624
		public static string CreateReport(IEnumerable<DTCItemV2> dtcs)
		{
			if (dtcs == null)
			{
				return "";
			}
			List<DTCItemV2> list = dtcs.ToList<DTCItemV2>();
			if (list.Count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Car Scanner ELM OBD2\nDTC report\nSelected brand: " + list[0].SelectedBrand);
			if (CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.IsVINAvailable && !string.IsNullOrEmpty(CarInfoViewModel.Instance.VIN))
			{
				stringBuilder.Append("\nVIN: " + CarInfoViewModel.Instance.VIN);
			}
			string text = "============{0}==============";
			stringBuilder.Append("\n\n");
			DTCCodeToStringConverter dtccodeToStringConverter = new DTCCodeToStringConverter();
			DTCStatusCollectionToStringConverter dtcstatusCollectionToStringConverter = new DTCStatusCollectionToStringConverter();
			DTCDescriptionCollectionToStringConverter dtcdescriptionCollectionToStringConverter = new DTCDescriptionCollectionToStringConverter();
			for (int i = 0; i < list.Count; i++)
			{
				DTCItemV2 dtcitemV = list[i];
				stringBuilder.Append(string.Format(text, (i + 1).ToString()));
				stringBuilder.Append("\n" + (string)dtccodeToStringConverter.Convert(dtcitemV.Code, null, null, null));
				stringBuilder.Append("\nRaw code: " + dtcitemV.RawCode);
				stringBuilder.Append("\nECU: " + dtcitemV.ECU);
				if (dtcitemV.IsArchive)
				{
					stringBuilder.Append("[" + Translate.GetString("ios_Archive") + "]");
				}
				if (dtcitemV.StatusVisible && dtcitemV.Statuses != null && dtcitemV.Statuses.Count > 0)
				{
					stringBuilder.Append("\n");
					foreach (Span span in ((FormattedString)dtcstatusCollectionToStringConverter.Convert(dtcitemV.Statuses, null, null, null)).Spans)
					{
						stringBuilder.Append(span.Text);
					}
				}
				if (dtcitemV.DescriptionsVisible)
				{
					stringBuilder.Append("\n");
					foreach (Span span2 in ((FormattedString)dtcdescriptionCollectionToStringConverter.Convert(dtcitemV.Descriptions, null, null, null)).Spans)
					{
						stringBuilder.Append(span2.Text);
					}
				}
				stringBuilder.Append("\n");
				if (!string.IsNullOrEmpty(dtcitemV.Payload))
				{
					stringBuilder.Append(dtcitemV.Payload);
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}
	}
}
