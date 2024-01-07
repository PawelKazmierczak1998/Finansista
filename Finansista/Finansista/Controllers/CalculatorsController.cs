using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finansista.Data;
using Finansista.Models;

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
            return View(new Calculators());
        }

        [HttpPost]
        public IActionResult Oblicz(Calculators model)
        {
            model.Wynik = model.KwotaNetto;
            return View("Index", model);
        }



    }
}
