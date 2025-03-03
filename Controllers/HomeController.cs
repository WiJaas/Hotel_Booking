using Hotel_Booking.Data;
using Hotel_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel_Booking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HotelBookingContext _context;

        public HomeController(ILogger<HomeController> logger, HotelBookingContext context)
        {
            _logger = logger;
            _context = context;

        }
        // GET: Home
        public async Task<IActionResult> Index()
        {
            // Retrieve all ChambreNew records from the database
            var chambres = await _context.ChambreNews.ToListAsync();

            // Pass the list of ChambreNew to the view
            return View(chambres);
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult Contact()
        {
            return View();

        }

        public IActionResult Privacy()
        {
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
