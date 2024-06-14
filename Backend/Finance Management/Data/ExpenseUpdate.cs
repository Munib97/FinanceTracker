namespace Finance_Management.Data
{
    public record ExpenseUpdate(int? ExpenseId, string? Name, decimal? Amount, DateTime? DateSpent, int? CategoryId);

}
