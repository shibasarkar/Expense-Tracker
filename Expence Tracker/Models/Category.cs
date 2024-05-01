using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expence_Tracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Column(TypeName = "Nvarchar(50)")]
        public string Title { get; set; }
        [Column(TypeName = "Nvarchar(50)")]
        public string Icon { get; set; } = "";
        [Column(TypeName = "Nvarchar(50)")]
        public string Type { get; set; } = "Expense";
        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return  Icon+" "+ Title;
            }
        }
    }
}
