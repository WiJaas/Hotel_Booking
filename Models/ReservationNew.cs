using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hotel_Booking.Models;

public partial class ReservationNew
{
    public int IdReservation { get; set; }

    public int? IdChambre { get; set; }

    public DateTime?DateDebut { get; set; }

    public DateTime? DateFin { get; set; }

    [HiddenInput(DisplayValue = false)]
    public DateTime? DateReservation { get; set; }

    public string? StatutReservation { get; set; }

    public string? TypeReservation { get; set; }

    public string? UserId { get; set; }

    public virtual ChambreNew? IdChambreNavigation { get; set; }
}
