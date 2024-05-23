using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expence_Tracker.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        [Range(1,int.MaxValue ,ErrorMessage ="Select a Category ")]
        public int CategoryId { get; set; }
        public Category ?Category { get; set; }
        [Range(100, int.MaxValue, ErrorMessage = "Amount can't be less than $100 ")]
        public int Amount { get; set; }
        [Column(TypeName = "Nvarchar(100)")]
        public string? Note { get; set; }
        public DateTime Date { get; set; }=DateTime.Now;
        [NotMapped]
        public string? CategoryWithIcon 
        {
            get { return Category == null ? "" : $"{Category.Icon} {Category.Title}"; }
        }
        [NotMapped]
        public string? FormateAmount
        {
            get 
            {
                return (Category == null || Category.Type == "Expense") ? "-"+Amount.ToString("C0"):"+"+Amount.ToString("C0");
            }
        }
    }
}
