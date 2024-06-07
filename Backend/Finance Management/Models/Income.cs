using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Security;

namespace Finance_Management.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public DateTime DateReceived { get; set; }
        [ForeignKey("IdentityUser")]
        public string UserId { get; set; }
        public IdentityUser User{ get; set; }

    }
}
