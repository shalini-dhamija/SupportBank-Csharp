using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBankClasses;

public class Bank
{
    public required string BankName {get;init;}
    private List<Account> _bankAccounts = new List<Account>();
    private readonly ILogger<UserInterface> _logger;

    public Bank(ILogger<UserInterface> logger)
    {
        _logger = logger;
    }
   
    public void CreateNewAccount(string accOwnerName)
    {
        var newAccount = new Account(accOwnerName.Trim(),_logger);
        _bankAccounts.Add(newAccount);
        _logger.LogInformation($"Account with name {accOwnerName} has been opened.");
    }

    public bool IfAccountExistsForOwner(string ownerName)
    {
        bool accExists = false;
        if(_bankAccounts.Any(acc => acc.OwnerName == ownerName.Trim()))
            accExists = true;

        return accExists;
    }

    public Account GetAccount(string accOwnerName)
    {
        return _bankAccounts.First(acc => acc.OwnerName == accOwnerName);
    }

    public void BankSummary()
    {
        Console.WriteLine($"*******Summary*******");
        foreach (var acc in _bankAccounts)
        {
            var owedAmt = acc.GetAllTransactions().Where(trans => trans.FromAccount == acc.OwnerName).Sum(trans => trans.Amount);
            var owesAmt = acc.GetAllTransactions().Where(trans => trans.ToAccount == acc.OwnerName).Sum(trans => trans.Amount);
            
            Console.WriteLine($"{acc.OwnerName} - Owed: {owedAmt}  Owes: {owesAmt}");
        }        
    }
}