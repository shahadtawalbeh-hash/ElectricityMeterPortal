using System;
using System.Collections.Generic;

namespace ElectricityMeterportal.Models;

public partial class Bill
{
    public int BillId { get; set; }

    public int MeterId { get; set; }

    public decimal PreviousReading { get; set; }

    public decimal CurrentReading { get; set; }

    public decimal Consumption { get; set; }

    public decimal Amount { get; set; }

    public DateTime? BillDate { get; set; }

    public virtual Meter Meter { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
