using Expence_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
namespace Expence_Tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context; 
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            TempData["title"] = "Side Bar";

            //Last 7 days Transactions
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;
            List<Transactions> selectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= startDate && y.Date <= endDate )
                .ToListAsync();

            //Total Income
            int totalIncome = selectedTransactions
                .Where(x => x.Category.Type == "Income")
                .Sum(y => y.Amount);
            ViewBag.TotalIncome = totalIncome.ToString("c0");

            //Total Expense
            int totalExpense = selectedTransactions
                .Where(x => x.Category.Type == "Expense")
                .Sum(y => y.Amount);
            ViewBag.TotalExpense= totalExpense.ToString("c0");

            //Total balance
            int totalBalance = totalIncome - totalExpense;
            CultureInfo culture= CultureInfo.CreateSpecificCulture("en-IN");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", totalBalance);
            return View();
        }
        public ActionResult Details()
        {
            string name;

            if (TempData.ContainsKey("title"))
                name = TempData["title"].ToString();
            return View();
        }
    }
}
