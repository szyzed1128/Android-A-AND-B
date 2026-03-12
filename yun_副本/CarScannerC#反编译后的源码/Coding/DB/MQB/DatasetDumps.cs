using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000A99 RID: 2713
	internal static class DatasetDumps
	{
		// Token: 0x060055B2 RID: 21938 RVA: 0x0040D204 File Offset: 0x0040B404
		public static ICodingContainer DataSet_65_0x7E0800()
		{
			return new MQBParametrizeSilentDump
			{
				Address = 8259584,
				DataLength = 2048,
				DataLengthFormatLength = 2,
				AddressFormatLength = 4,
				Group = CodingGroup.DatasetDump,
				Name = "Dataset 65 0x7E0800",
				LogComment = "Dataset 65 0x7E0800",
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Password = "20103",
				PreReadCommands = "1003;1040;2704;22F182",
				PreWriteCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;22F182;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;",
				PostWriteCommands = "700:3E80;37;700:3E80;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF;",
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("65"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("65")
			};
		}

		// Token: 0x060055B3 RID: 21939 RVA: 0x0040D2AC File Offset: 0x0040B4AC
		public static ICodingContainer DataSet_44_061000()
		{
			return new MQBParametrizeSilentDump
			{
				Address = 1,
				DataLength = 8192,
				DataLengthFormatLength = 4,
				AddressFormatLength = 4,
				Group = CodingGroup.DatasetDump,
				Name = "Dataset 44 0x061000",
				LogComment = "Dataset 44 0x061000",
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Password = "19249",
				PreReadCommands = "1003;1040;2704;22F182",
				PreWriteCommands = "700:1083;1003;1040;22F1A0;22F1A1;22F1A4;22F182;2704;2EF198;2EF199;31010300030100;31030300;",
				PostWriteCommands = "37;310102EF030100;310302EF;2EF1A0;2EF1A1;2EF1A4;1102;1003;14FFFFFF;",
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("44"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("44")
			};
		}

		// Token: 0x060055B4 RID: 21940 RVA: 0x0040D350 File Offset: 0x0040B550
		public static ICodingContainer DataSet_44_0E7000()
		{
			return new MQBParametrizeSilentDump
			{
				Address = 1,
				DataLength = 8192,
				DataLengthFormatLength = 4,
				AddressFormatLength = 4,
				Group = CodingGroup.DatasetDump,
				Name = "Dataset 44 0x0E7000",
				LogComment = "Dataset 44 0x0E7000",
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Password = "44595",
				PreReadCommands = "1003;1040;2704;22F182",
				PreWriteCommands = "700:1083;1003;1040;22F1A0;22F1A1;22F1A4;22F182;2704;2EF198;2EF199;31010300030100;31030300;",
				PostWriteCommands = "37;310102EF030100;310302EF;2EF1A0;2EF1A1;2EF1A4;1102;1003;14FFFFFF;",
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("44"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("44")
			};
		}

		// Token: 0x060055B5 RID: 21941 RVA: 0x0040D3F4 File Offset: 0x0040B5F4
		public static ICodingContainer DatasetDump_5F(string name, int address, int dataLength)
		{
			return new MQBParametrizeSilentDump
			{
				Address = address,
				DataLength = dataLength,
				DataLengthFormatLength = 3,
				AddressFormatLength = 3,
				Group = CodingGroup.DatasetDump,
				Name = name,
				LogComment = name,
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Password = "20103",
				PreReadCommands = "1003;1040;2704;22F182",
				PreWriteCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;",
				PostWriteCommands = "700:3E80;37;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF;1902AF",
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("5F"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("5F")
			};
		}

		// Token: 0x060055B6 RID: 21942 RVA: 0x0040D48C File Offset: 0x0040B68C
		public static ICodingContainer DataSet_47()
		{
			return new MQBParametrizeSilentDump
			{
				Address = 851968,
				DataLength = 65536,
				DataLengthFormatLength = 4,
				AddressFormatLength = 4,
				Group = CodingGroup.DatasetDump,
				Name = "Dataset 47 0xD0000",
				LogComment = "Dataset 47 0xD0000",
				ValueType = AdaptationValueTypes.MQBParametrizeDump,
				Password = "20103",
				PreReadCommands = "700:1083;1003;1040;22F1A0;700:3E80;22F1A1;700:3E80;22F1A4;700:3E80;2704;2EF198;700:3E80;2EF199;700:3E80;31010300030100;700:3E80;31030300;700:3E80;",
				PreWriteCommands = "700:3E80;310102EF030100;700:3E80;310302EF;700:3E80;2EF1A0;700:3E80;2EF1A1;700:3E80;2EF1A4;700:3E80;1102;1003;14FFFFFF;1902AF",
				RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit("47"),
				ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit("47")
			};
		}

		// Token: 0x060055B7 RID: 21943 RVA: 0x0040D528 File Offset: 0x0040B728
		public static IEnumerable<ICodingContainer> Create5FDatasetDumps()
		{
			return new List<ICodingContainer>
			{
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x0240", 576, 30),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x3000", 12288, 1330),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x3F00", 16128, 404),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x7100", 28928, 2048),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x700", 1792, 1024),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x3B00", 15104, 55),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x3600", 13824, 633),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x200", 512, 35),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x280", 640, 22),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x2C00", 11264, 32),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0xDA0", 3488, 60),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x2D00", 11520, 389),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x2F00", 12032, 102),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x4500", 17664, 9220),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x7000", 28672, 254),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0xF00000", 15728640, 608),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x0440", 1088, 18),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x2C0", 704, 325),
				DatasetDumps.DatasetDump_5F("Dataset 5F 0x3900", 14592, 496)
			};
		}
	}
}
