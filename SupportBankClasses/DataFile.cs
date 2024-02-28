using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Logging;


namespace SupportBank.SupportBankClasses;

public class DataFile
{
    public required string FileName {get;init;}
    public required string BankName {get;init;}

    private  List<string> _lines = new List<string>();
    private  Bank _bank;
    private readonly ILogger<UserInterface> _logger;

    [SetsRequiredMembers]
    public DataFile(string fileName, string bankName, ILogger<UserInterface> logger)
    {
        FileName = fileName;
        BankName = bankName;
        //_lines = new(File.ReadAllLines(FileName));
        _bank = new Bank(logger){BankName=bankName};
        _logger = logger;
    }

    public void LoadFileToBank()
    {
        
        int lineNum = 0;
        foreach(string line in _lines)
        {
            if(lineNum>0)   //Ignore Header row
            {
                //Create New Account for each line if it does not exist and add a transaction
                var arrLine = line.Split(",");
                DateTime transactionDate = DateTime.Now;
                Decimal transactionAmt = 0;
                try
                {
                    transactionDate = DateTime.Parse(arrLine[0]);
                }
                catch(FormatException exception)
                {
                    _logger.LogError($"Error while reading Date from line {line}");                    
                    _logger.LogDebug(exception.StackTrace);
                    //throw;
                }
                var fromAccount = arrLine[1];
                var toAccount = arrLine[2];
                var comment = arrLine[3];
                try
                {
                    transactionAmt = Decimal.Parse(arrLine[4]);
                }
                catch(FormatException)
                {
                    _logger.LogError($"Error while reading Amount from line {line}");
                }

                if(!_bank.IfAccountExistsForOwner(fromAccount))
                {
                    _bank.CreateNewAccount(fromAccount);
                }
                if(!_bank.IfAccountExistsForOwner(toAccount))
                {
                    _bank.CreateNewAccount(toAccount);
                }
                _bank.GetAccount(fromAccount).AddTransaction(transactionDate,fromAccount,toAccount,comment, transactionAmt);
                _bank.GetAccount(toAccount).AddTransaction(transactionDate,fromAccount,toAccount,comment, transactionAmt);
                //Console.WriteLine(line);
            }        
            lineNum++;
        }   
               
    }

    // private void LoadFile()
    // {
    //     using var fileReader = new StreamReader(FileName);
    //         using var csvReader = new CsvReader(fileReader, CultureInfo.InvariantCulture);
    //         var transactions = csvReader.GetRecords<Transaction>();
    // }
    public void List(string ownerName)
    {
        _bank.GetAccount(ownerName).AccountSummary();
    }

    public void ListAll()
    {
        _bank.BankSummary();
    }
}