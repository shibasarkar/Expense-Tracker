using Expence_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.EJ2.Charts;
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
            ViewBag.TotalExpense= totalExpense.ToString("C0");

            //Total balance
            int totalBalance = totalIncome - totalExpense;
            CultureInfo culture= CultureInfo.CreateSpecificCulture("en-IN");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", totalBalance);

            //Doughnut Chart - Expense by Category
            ViewBag.DoughnutChartData = selectedTransactions
                .Where(l => l.Category.Type == "Expense")
                .GroupBy(j => j.CategoryId)
                .Select(k => new
                {
                    categoryWithIcon=k.First().Category.Icon+" "+k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formatedAmount = k.Sum(j => j.Amount).ToString("C0"),

                })
                .OrderByDescending(s=>s.amount)
                .ToList();

            //Spline Chart - Incoem vs Expence
            //Income
            List<SplineChartData> incomeSummery = selectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(s => new SplineChartData()
                {
                    Day=s.First().Date.ToString("dd-MMM"),
                    Income=s.Sum(l=>l.Amount)
                })
                .ToList();
            //Expence
            List<SplineChartData> expenceSummery = selectedTransactions
                .Where(i => i.Category.Type == "Expence")
                .GroupBy(j => j.Date)
                .Select(s => new SplineChartData()
                {
                    Day = s.First().Date.ToString("dd-MMM"),
                    Expence = s.Sum(l => l.Amount)
                })
                .ToList();
            //Combine Income & Expence
            string[] last7days=Enumerable.Range(0, 7)
                .Select(s=>startDate.AddDays(s).ToString("dd-MMM"))
                .ToArray();
            ViewBag.SplineChartData = from days in last7days
                                      join income in incomeSummery on days equals income.Day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expence in expenceSummery on days equals expence.Day into dayExpenceJoined
                                      from expence in dayExpenceJoined.DefaultIfEmpty()
                                      select new
                                      {
                                          day = days,
                                          income = income == null ? 0 : income.Income,
                                          expence = expence== null ? 0 : expence.Expence
                                      };
            //Recent Transaction
            ViewBag.RecentTransaction=await _context.Transactions
                .Include(i=>i.Category)
                .OrderByDescending(d=>d.Date)
                .Take(5)
                .ToListAsync();
            return View();
        }
    }

    public class SplineChartData
    {
        public string Day { get; set; }
        public int Income { get; set; }
        public int Expence { get; set; }
    }
}
