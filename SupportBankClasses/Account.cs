using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBankClasses;

public class Account
{
    private static double _numberInit = 100;
    public required double AccNumber { get; init; }
    public required string OwnerName { get; init; }
    private readonly List<Transaction> _accTransactions = [];
    private readonly ILogger<UserInterface> _logger;

    [SetsRequiredMembers]
    public Account(string ownerName, ILogger<UserInterface> logger)
    {
        AccNumber = _numberInit + 1;
        OwnerName = ownerName;  
        _logger = logger;      
    }

    public void AddTransaction(DateTime date, string fromAccount, string toAccount, string comment, decimal amount)
    {
          Transaction trans = new Transaction(date,fromAccount,toAccount,comment,amount);
        _accTransactions.Add(trans);
    }

    // public void DebitAccount(decimal amount, string fromAccount, string toAccount, DateTime date, string comment)
    // {
    //     Transaction trans = new Transaction(-amount,TransactionType.Debit, fromAccount, toAccount, date,comment);
    //     _accTransactions.Add(trans);
    // }

    public List<Transaction> GetAllTransactions()
    {
        return _accTransactions;
        // foreach(var trans in _accTransactions)
        // {
        //     Console.WriteLine(string.Join(",",trans));
        // }
    }

    public void AccountSummary()
    {
        var owedToAcc = _accTransactions
                        .Where(trans => trans.FromAccount == this.OwnerName)
                        .GroupBy(trans => trans.ToAccount)
                        .Select(group => (owedFrom : group.Key, amount: group.Sum(trans => trans.Amount)));

        var oweToOthers = _accTransactions
                        .Where(trans => trans.ToAccount == this.OwnerName)
                        .GroupBy(trans => trans.FromAccount)
                        .Select(group => (owes : group.Key, amount: group.Sum(trans => trans.Amount)));

        Console.WriteLine($"*******{this.OwnerName} owed *******");
        if(owedToAcc.Count()!=0)
            Console.WriteLine($"{this.OwnerName} owed nothing.");
        foreach (var item in owedToAcc)
        {
            Console.WriteLine($"{item.owedFrom} : {item.amount}");
        }

        Console.WriteLine($"*******{this.OwnerName} owes *******");
        if(oweToOthers.Count()!=0)
            Console.WriteLine($"{this.OwnerName} owes to no one.");
        
        foreach (var item in oweToOthers)
        {
            Console.WriteLine($"{item.owes} : {item.amount}");
        }
    }

}