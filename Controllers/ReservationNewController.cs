using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_Booking.Models;
using System.Security.Claims;

namespace Hotel_Booking.Controllers
{
    public class ReservationNewController : Controller
    {
        private readonly HotelBookingContext _context;

        public ReservationNewController(HotelBookingContext context)
        {
            _context = context;
        }

        // GET: ReservationNew
        // GET: ReservationNew
        public async Task<IActionResult> Index()
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filter reservations by the logged-in user's ID
            var hotelBookingContext = _context.ReservationNews
                .Include(r => r.IdChambreNavigation)
                .Where(r => r.UserId == userId);

            return View(await hotelBookingContext.ToListAsync());
        }


        // GET: ReservationNew/Create
     // GET: ReservationNew/Create
public IActionResult Create(int? idChambre)
{
    // Ensure the room ID is provided
    if (idChambre == null)
    {
        return NotFound("Room ID is not provided.");
    }

    // Check if the room exists in the database
    var room = _context.ChambreNews
        .Where(r => r.IdChambre == idChambre)
        .Select(r => new
        {
            r.DescriptionChambre,
            r.PhotoUrl
        })
        .FirstOrDefault();

    if (room == null)
    {
        return NotFound("The selected room does not exist.");
    }

    // Create a new reservation object and assign the room ID
    var reservation = new ReservationNew
    {
        IdChambre = idChambre.Value
    };

    // Populate the TypeReservation dropdown list
    var reservationTypes = new List<SelectListItem>
    {
        new SelectListItem { Text = "Single Room (For 1 person)", Value = "Single" },
        new SelectListItem { Text = "Double Room (For 2 people)", Value = "Double" },
        new SelectListItem { Text = "Suite (More luxurious with added amenities)", Value = "Suite" }
    };
    ViewData["TypeReservationList"] = reservationTypes;

    // Pass room details to the view
    ViewData["RoomDetails"] = room;

    return View(reservation);
}


        // POST: ReservationNew/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int idChambre, [Bind("IdReservation,DateDebut,DateFin,StatutReservation,TypeReservation")] ReservationNew reservationNew)
        {
            if (ModelState.IsValid)
            {
                // Validate that DateDebut is less than DateFin
                if (reservationNew.DateDebut >= reservationNew.DateFin)
                {
                    ModelState.AddModelError("", "The start date (DateDebut) must be earlier than the end date (DateFin).");
                    return View(reservationNew);
                }
                // Assign the passed IdChambre to the reservation
                reservationNew.IdChambre = idChambre;

                // Validate that the room exists
                var room = await _context.ChambreNews.FindAsync(idChambre);
                if (room == null)
                {
                    ModelState.AddModelError("", "The selected room does not exist.");
                    return View(reservationNew);
                }

                // Set the logged-in user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                reservationNew.UserId = userId;

                // Set the reservation date to the current date and time
                reservationNew.DateReservation = DateTime.Now;

                // Save the reservation
                _context.Add(reservationNew);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the reservation list
     
                
            
            }


            // Re-populate the room details if validation failed
            var roomDetails = await _context.ChambreNews
                .Where(r => r.IdChambre == idChambre)
                .Select(r => new { r.DescriptionChambre, r.PhotoUrl })
                .FirstOrDefaultAsync();

            if (roomDetails != null)
            {
                ViewData["RoomDetails"] = roomDetails;
            }


            // Return the view with validation errors
            return View(reservationNew);
        }


        // GET: ReservationNew/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationNew = await _context.ReservationNews.FindAsync(id);
            if (reservationNew == null)
            {
                return NotFound();
            }
            ViewData["IdChambre"] = new SelectList(_context.ChambreNews, "IdChambre", "IdChambre", reservationNew.IdChambre);
            return View(reservationNew);
        }

        // POST: ReservationNew/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReservation,IdChambre,DateDebut,DateFin,DateReservation,StatutReservation,TypeReservation,UserId")] ReservationNew reservationNew)
        {
            if (id != reservationNew.IdReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationNew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationNewExists(reservationNew.IdReservation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdChambre"] = new SelectList(_context.ChambreNews, "IdChambre", "IdChambre", reservationNew.IdChambre);
            return View(reservationNew);
        }

        // GET: ReservationNew/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservationNew = await _context.ReservationNews
                .Include(r => r.IdChambreNavigation)
                .FirstOrDefaultAsync(m => m.IdReservation == id);
            if (reservationNew == null)
            {
                return NotFound();
            }

            return View(reservationNew);
        }

        // POST: ReservationNew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationNew = await _context.ReservationNews.FindAsync(id);
            if (reservationNew != null)
            {
                _context.ReservationNews.Remove(reservationNew);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationNewExists(int id)
        {
            return _context.ReservationNews.Any(e => e.IdReservation == id);
        }
    }
}
