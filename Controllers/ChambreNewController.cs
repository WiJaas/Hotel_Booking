using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_Booking.Models;
using Microsoft.AspNetCore.Hosting;

namespace Hotel_Booking.Controllers
{
    public class ChambreNewController : Controller
    {
        private readonly HotelBookingContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ChambreNewController(HotelBookingContext context ,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: ChambreNew
        public async Task<IActionResult> Index()
        {
            // Retrieve all ChambreNew records from the database
            var chambres = await _context.ChambreNews
                                 .OrderBy(c => c.TarifParNuit) // Replace "Capacite" with the property you want to sort by
                                 .ToListAsync();

            // Pass the list of ChambreNew to the view
            return View(chambres);
        }

   
        // GET: ChambreNew/Create
        public IActionResult Create()
        {
            ViewBag.StatusOptions = new List<string> { "Available", "Unavailable" };

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Capacite,DescriptionChambre,StatutChambre,TarifParNuit,TypeChambre")] ChambreNew chambre, IFormFile photoUrl)
        {
            if (ModelState.IsValid)
            {
                if (photoUrl != null && photoUrl.Length > 0)
                {
                    // Vérifier l'extension du fichier pour la validité
                    var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(photoUrl.FileName).ToLower();

                    if (!validExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("PhotoUrl", "Type de fichier invalide. Seuls les fichiers image (.jpg, .jpeg, .png, .gif) sont autorisés.");
                        return View(chambre); // Retourner avec le message d'erreur
                    }

                    // Définir le chemin du fichier et sauvegarder l'image
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath,"images");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + photoUrl.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photoUrl.CopyToAsync(stream);
                    }

                    // Enregistrer le chemin du fichier dans l'objet chambre
                    chambre.PhotoUrl = $"/images/{uniqueFileName}";
                }

                _context.Add(chambre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate status options if the form submission fails
            ViewBag.StatusOptions = new List<string> { "Available", "Unavailable" };
            return View(chambre); // Si l'état du modèle est invalide, retourner à la vue avec les erreurs de validation
        }

        // GET: ChambreNew/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chambreNew = await _context.ChambreNews.FindAsync(id);
            if (chambreNew == null)
            {
                return NotFound();
            }
            ViewBag.StatusOptions = new List<string> { "Available", "Unavailable" };

            return View(chambreNew);
        }

        // POST: ChambreNew/Edit/5
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdChambre,Capacite,DescriptionChambre,StatutChambre,TarifParNuit,TypeChambre,PhotoUrl")] ChambreNew chambreNew)
        {
            if (id != chambreNew.IdChambre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chambreNew);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChambreNewExists(chambreNew.IdChambre))
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
            // Repopulate the status list in case the form is redisplayed due to an error
            ViewBag.StatusOptions = new List<string> { "Available", "Unavailable" };
            return View(chambreNew);
        }

        // GET: ChambreNew/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chambreNew = await _context.ChambreNews
                .FirstOrDefaultAsync(m => m.IdChambre == id);
            if (chambreNew == null)
            {
                return NotFound();
            }

            return View(chambreNew);
        }

        // POST: ChambreNew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chambreNew = await _context.ChambreNews.FindAsync(id);
            if (chambreNew != null)
            {
                _context.ChambreNews.Remove(chambreNew);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChambreNewExists(int id)
        {
            return _context.ChambreNews.Any(e => e.IdChambre == id);
        }

        // GET: Chambres/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var chambre = await _context.ChambreNews.FindAsync(id);

            if (chambre == null)
            {
                return NotFound();
            }
            return View("ChambreDetails", chambre); // Specify the view name
        }



        // Action to display available rooms for booking
        public async Task<IActionResult> AvailableRooms()
        {
            // Get all rooms where the status is 'Available'
            var availableRooms = await _context.ChambreNews
                .Where(c => c.StatutChambre == "Available") // Ensure you only show available rooms
                .OrderBy(c => c.TarifParNuit) // Replace "Capacite" with the property you want to sort by
                                 .ToListAsync(); ;

            // Return the AvailableRooms view with the list of available rooms
            return View("Listings", availableRooms);
        }












    }
}
