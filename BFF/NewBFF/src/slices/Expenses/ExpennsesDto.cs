using System.Collections;

namespace NewBFF.Slices.Expenses;
public class ExpensesDto
{
    public int UserId { get; set; }
    public double TotalVask { get; set; }
    public double TotalTank { get; set; }
    public double TotalExpenses { get; set; }
    public required IEnumerable<ExpenseBreakDto> ExpenseBreakdown { get; set; }
}

public class ExpenseBreakDto
{
    public string Category { get; set; } = string.Empty;
    public double Amount { get; set; }

}