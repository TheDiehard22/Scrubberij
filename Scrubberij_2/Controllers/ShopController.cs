using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scrubberij_2.Data;
using Microsoft.EntityFrameworkCore;
using Scrubberij_2.Models.ShopViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scrubberij_2.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var allCars = _context.Cars.Include(i => i.Fotos);

            return View(allCars);
        }

        public IActionResult ViewSingleCar(int? id)
        {
            ViewBag.Title = "auto";

            if (!CarExists(id))
            {
                return NotFound();
            }

            var vm = getVM(id);

            return View("View", vm);
        }

        [HttpPost]
        public IActionResult ViewSingleCar(int? id,
            [Bind("Id", "CarId", "Voornaam", "Achternaam", "Email", "Tekst", "Datum")] Comment comment)
        {
            comment.CarId = id.Value;

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Comments.Add(comment);
                    _context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View("View", getVM(comment.CarId));
        }

        public bool CarExists(int? id)
        {
            var carExists = _context.Cars.Any();

            return carExists;
        }

        public ShopIndex getVM(int? id)
        {
            var vm = new ShopIndex();

            vm.Car = _context.Cars
                .Include(c => c.Comments)
                .Where(c => c.Id == id)
                .SingleOrDefault();

            vm.Comments = _context.Comments
                .Where(i => i.CarId == id)
                .ToList();

            return vm;
        }
    }
}
