using Finansista.Data;
using Finansista.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Finansista.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userMenager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userMenager)
        {
            _context = context;
            _userMenager = userMenager;
            _logger = logger;
        }
        public IActionResult Index() 
        {
        return View();
        }
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {

            IdentityUser user = _userMenager.FindByNameAsync(User.Identity.Name).Result;
           
            var applicationDbContext = _context.Transaction
                .Include(t => t.Balance).
                Include(e => e.Balance.user)
                .Where(e => e.Balance.userId == user.Id);
            return View(await applicationDbContext.ToListAsync());
            
          
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}