using System;
using System.Collections.Generic;

namespace ElectricityMeterportal.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BillId { get; set; }

    public decimal AmountPaid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public virtual Bill Bill { get; set; } = null!;
}
