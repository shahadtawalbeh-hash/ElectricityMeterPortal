using System;
using System.Collections.Generic;

namespace ElectricityMeterportal.Models;

public partial class MeterReading
{
    public int ReadingId { get; set; }

    public int MeterId { get; set; }

    public decimal ReadingValue { get; set; }

    public DateTime? ReadingDate { get; set; }

    public virtual Meter Meter { get; set; } = null!;
}
