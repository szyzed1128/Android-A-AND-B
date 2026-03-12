using System;
using System.ComponentModel;
using CarScannerXamarinForms.ABRP;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003FB RID: 1019
	public interface IPID : INotifyPropertyChanged
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x060028DC RID: 10460
		// (remove) Token: 0x060028DD RID: 10461
		event EventHandler<PID> ValueChanged;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x060028DE RID: 10462
		// (remove) Token: 0x060028DF RID: 10463
		event EventHandler<PID> IsAvailableChanged;

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x060028E0 RID: 10464
		string Name { get; }

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x060028E1 RID: 10465
		string ShortName { get; }

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x060028E2 RID: 10466
		string Command { get; }

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x060028E3 RID: 10467
		bool IsAvailable { get; }

		// Token: 0x060028E4 RID: 10468
		void Decode(byte[] data, TimeSpan timeStamp, string response_header);

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x060028E5 RID: 10469
		bool ShouldHideValue { get; }

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x060028E6 RID: 10470
		double Maximum { get; }

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x060028E7 RID: 10471
		double Minimum { get; }

		// Token: 0x170011D5 RID: 4565
		// (get) Token: 0x060028E8 RID: 10472
		bool HasAnnotation { get; }

		// Token: 0x170011D6 RID: 4566
		// (get) Token: 0x060028E9 RID: 10473
		int Id { get; }

		// Token: 0x170011D7 RID: 4567
		// (get) Token: 0x060028EA RID: 10474
		TimeSpan TimeStamp { get; }

		// Token: 0x170011D8 RID: 4568
		// (get) Token: 0x060028EB RID: 10475
		int intCommand { get; }

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x060028EC RID: 10476
		// (set) Token: 0x060028ED RID: 10477
		Roles Role { get; set; }

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x060028EE RID: 10478
		// (set) Token: 0x060028EF RID: 10479
		bool IsVisible { get; set; }

		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x060028F0 RID: 10480
		// (set) Token: 0x060028F1 RID: 10481
		int SkipCycles { get; set; }

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x060028F2 RID: 10482
		string OriginalName { get; }

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x060028F3 RID: 10483
		string OriginalShortName { get; }

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x060028F4 RID: 10484
		Roles OriginalRole { get; }

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x060028F5 RID: 10485
		int OriginalSkipCycles { get; }

		// Token: 0x170011E0 RID: 4576
		// (get) Token: 0x060028F6 RID: 10486
		ABRPRoles ABRPRole { get; }

		// Token: 0x170011E1 RID: 4577
		// (get) Token: 0x060028F7 RID: 10487
		// (set) Token: 0x060028F8 RID: 10488
		string Header { get; set; }

		// Token: 0x170011E2 RID: 4578
		// (get) Token: 0x060028F9 RID: 10489
		// (set) Token: 0x060028FA RID: 10490
		string CustomName { get; set; }

		// Token: 0x170011E3 RID: 4579
		// (get) Token: 0x060028FB RID: 10491
		// (set) Token: 0x060028FC RID: 10492
		string CustomShortName { get; set; }

		// Token: 0x170011E4 RID: 4580
		// (get) Token: 0x060028FD RID: 10493
		// (set) Token: 0x060028FE RID: 10494
		int CustomSkipCycles { get; set; }

		// Token: 0x170011E5 RID: 4581
		// (get) Token: 0x060028FF RID: 10495
		// (set) Token: 0x06002900 RID: 10496
		Roles CustomRole { get; set; }

		// Token: 0x170011E6 RID: 4582
		// (get) Token: 0x06002901 RID: 10497
		// (set) Token: 0x06002902 RID: 10498
		UnitsHelper.Units CustomUnit { get; set; }
	}
}
