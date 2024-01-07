using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finansista.Data;
using Finansista.Models;
using NuGet.ContentModel;

namespace Finansista.Controllers
{
    public class CalculatorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CalculatorsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (ModelState.IsValid)
            {
                return View(new Calculators());
            }
           else { return NotFound(); }
        }

        [HttpPost]
        public IActionResult Oblicz(Calculators model)
        {
            if (!ModelState.IsValid)
            {   

                return View("Index", model);
            }

            model.Wynik = model.KwotaNetto;
            return View("Index", model);

        }
          

    }
}
