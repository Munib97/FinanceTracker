using System.ComponentModel.DataAnnotations;

namespace Finance_Management.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
