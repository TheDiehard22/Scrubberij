using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Scrubberij_2.Data;
using Scrubberij_2.Models.ShopViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Scrubberij_2.Controllers
{
    [Authorize]
    public class ManageShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;

        public ManageShopController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /ManageShop/
        public IActionResult Index()
        {
            var allCars = _context.Cars.ToList();

            return View(allCars);
        }

        // GET: /ManageShop/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(int id,
            [Bind("Merk", "Type", "Bouwjaar", "Prijs", "Kilometerstand", "Brandstof", "Transmissie", "ApkVerloopDatum", "Fotos", "BTW", "ExtraInformatie")] Car car,
            List<IFormFile> files)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "uploads");
            var allPhotos = new List<CarImage>();

            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
                        {
                            file.CopyTo(stream);

                            var image = new CarImage
                            {
                                URL = Path.Combine(filePath, file.FileName),
                                IsFirst = true
                            };

                            //Add Photos to CarImage collection
                            allPhotos.Add(image);
                        }

                    }
                }

                //Add all the fotos the fotos collection
                car.Fotos = allPhotos;

                if (ModelState.IsValid)
                {
                    _context.Cars.Add(car);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(car);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var singleCar = _context.Cars.Where(a => a.Id == id).Single();

            return View(singleCar);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> UpdateCar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carToUpdate = _context.Cars.SingleOrDefault(a => a.Id == id);

            if (await TryUpdateModelAsync<Car>(
                carToUpdate,
                "",
                s => s.Merk, s => s.Type, s => s.Bouwjaar, s => s.Prijs, s => s.Kilometerstand, s => s.Brandstof, s => s.Transmissie, s => s.ApkVerloopDatum, s => s.BTW, s => s.ExtraInformatie))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //Come back here when the model state is not valid
            return View(carToUpdate);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var carToRemove = _context.Cars.AsNoTracking().SingleOrDefault(i => i.Id == id);

            if (carToRemove == null)
            {
                return NotFound();
            }

            try
            {
                _context.Cars.Remove(carToRemove);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePath = Path.Combine(_environment.WebRootPath, "uploads");


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(filePath, formFile.FileName), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count(), size, filePath });
        }
    }
}
