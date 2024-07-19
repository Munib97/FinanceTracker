namespace Finance_Management.Data
{
    public class CombinedSpendingsDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string CategoryName { get; set; }
    }
}
