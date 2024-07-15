namespace Finance_Management.Data
{
    public class ExpenseGetDTO
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateSpent { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
