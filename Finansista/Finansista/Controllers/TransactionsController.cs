using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Finansista.Data;
using Finansista.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Finansista.Data.Enum;

namespace Finansista.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userMenager;
        public TransactionsController(ApplicationDbContext context, UserManager<IdentityUser> userMenager)
        {
            _context = context;
            _userMenager = userMenager;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            IdentityUser user = _userMenager.FindByNameAsync(User.Identity.Name).Result;
            var applicationDbContext = _context.Transaction
                .Include(t => t.Balance).
                Include(e => e.Balance.user)
                .Where(e => e.Balance.userId == user.Id )
                ;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            IdentityUser user = _userMenager.FindByNameAsync(User.Identity.Name).Result;
            ViewData["balanceId"] = new SelectList(_context.Balance, "Id", "accountName");

            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,amount,description,balanceId,TransactionType")] Transaction transaction)
        {
            IdentityUser user = _userMenager.FindByNameAsync(User.Identity.Name).Result;
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                UpdateDataBase(transaction);
                await _context.SaveChangesAsync();              
                return RedirectToAction(nameof(Index));
            }
            ViewData["balanceId"] = new SelectList(_context.Balance, "Id", "accountName", transaction.balanceId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["balanceId"] = new SelectList(_context.Balance, "Id", "accountName", transaction.balanceId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,amount,description,balanceId,TransactionType")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["balanceId"] = new SelectList(_context.Balance, "Id", "accountName", transaction.balanceId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transaction == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Balance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transaction == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transaction'  is null.");
            }
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                _context.Transaction.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transaction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private void UpdateDataBase(Transaction transaction)
        {
           
            decimal currentBalance = _context.Transaction.Where(e=> e.balanceId==transaction.balanceId).Sum(t => t.TransactionType == TransactionType.Wpływy ? t.amount : -t.amount);
            var balance = _context.Balance
                .Where(e => e.Id == transaction.balanceId)
                .FirstOrDefault();


            if (transaction.TransactionType == TransactionType.Wpływy)
            {
                currentBalance += transaction.amount;
            }
            else
            {
                currentBalance -= transaction.amount;
            }

            balance.accountBalance = currentBalance;
            _context.Update(balance);
        }
    }
}
