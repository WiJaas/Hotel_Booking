using System;
using System.Collections.Generic;

namespace Hotel_Booking.Models;

public partial class ChambreNew
{
    public int IdChambre { get; set; }

    public int? Capacite { get; set; }

    public string? DescriptionChambre { get; set; }

    public string? StatutChambre { get; set; }

    public double? TarifParNuit { get; set; }

    public string? TypeChambre { get; set; }

    public string? PhotoUrl{ get; set; }

    public virtual ICollection<ReservationNew> ReservationNews { get; set; } = new List<ReservationNew>();
}
