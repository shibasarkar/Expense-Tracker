using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expence_Tracker.Models;

namespace Expence_Tracker.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        #region Details
        // GET: Transactions/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Transactions == null)
        //    {
        //        return NotFound();
        //    }

        //    var transactions = await _context.Transactions
        //        .Include(t => t.Category)
        //        .FirstOrDefaultAsync(m => m.TransactionId == id);
        //    if (transactions == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(transactions);
        //}
        #endregion
        // GET: Transactions/CreateOrEdit
        public IActionResult CreateOrEdit(int id)
        {
            PopulateCategory();
            //ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            if(id == 0)
            {
                return View(new Transactions());
            }
            else
            {
                return View(_context.Transactions.Find(id));
            }
        }

        // POST: Transactions/CreateOrEdit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("TransactionId,CategoryId,Amount,Note,Date")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                if(transactions.TransactionId == 0)
                {
                    _context.Add(transactions);
                }
                else
                {
                    _context.Update(transactions);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", transactions.CategoryId);
            return View(transactions);
        }

        #region Update
        // GET: Transactions/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Transactions == null)
        //    {
        //        return NotFound();
        //    }

        //    var transactions = await _context.Transactions.FindAsync(id);
        //    if (transactions == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", transactions.CategoryId);
        //    return View(transactions);
        //}

        //// POST: Transactions/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TransactionId,CategoryId,Amount,Note,Date")] Transactions transactions)
        //{
        //    if (id != transactions.TransactionId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(transactions);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TransactionsExists(transactions.TransactionId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", transactions.CategoryId);
        //    return View(transactions);
        //}
        #endregion
       
        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
            }
            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions != null)
            {
                _context.Transactions.Remove(transactions);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public void PopulateCategory()
        {
            var categories = _context.Category.ToList();
            Category defaultCategory=new Category() {CategoryId=0, Title = "Choose a Category" };
            categories.Insert(0, defaultCategory);
            ViewBag.Categories = categories;
        }

        //private bool TransactionsExists(int id)
        //{
        //  return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        //}
    }
}
