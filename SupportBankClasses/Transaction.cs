
using System.Diagnostics.CodeAnalysis;
namespace SupportBank.SupportBankClasses;

public class Transaction
{
    public required decimal Amount { get; init;}
    public required string FromAccount {get; init;}
    public required string ToAccount {get; init;}
    public required DateTime Date { get; init; }
    public required string Comment { get; init; }

    [SetsRequiredMembers]
    public Transaction(DateTime date, string fromAccount, string toAccount, string comment, decimal amount)
    {
        Amount = amount;
        FromAccount = fromAccount;
        ToAccount = toAccount;
        Date = date;
        Comment = comment;
    }
}
