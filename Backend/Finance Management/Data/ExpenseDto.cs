using Finance_Management.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Finance_Management.Data
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateSpent { get; set; }
        public int CategoryId { get; set; }
    }
}
