namespace Finance_Management.Data
{
    public record ExpenseUpdateDTO(int? ExpenseId, string? Name, decimal? Amount, DateTime? DateSpent, int? CategoryId);

}
