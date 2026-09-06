using System;
using System.Collections.Generic;

namespace ElectricityMeterportal.Models;

public partial class Meter
{
    public int MeterId { get; set; }

    public string MeterNumber { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int RequestId { get; set; }
    public virtual ElectricityRequest? Request { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
}
