using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expence_Tracker.Models
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Amount { get; set; }
        [Column(TypeName = "Nvarchar(100)")]
        public string? Note { get; set; }

        public DateTime Date { get; set; }=DateTime.Now;
    }
}
