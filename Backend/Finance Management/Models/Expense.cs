using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finance_Management.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        public decimal Amount { get; set; }
        public string Name { get; set; }
        public DateTime DateSpent { get; set; }
       
        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser User{ get; set; }
        [ForeignKey("Category")]

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
