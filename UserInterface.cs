using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBankClasses;
public class UserInterface
{
    private readonly ILogger<UserInterface> _logger;

    public UserInterface(ILogger<UserInterface> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("App starting...");

        Console.Write("Enter the name of the file you wish to read from: ");
        var fileName = Console.ReadLine() ?? "";
        Console.Write("Enter the name of the bank you wish to load file into: ");
        var bankName = Console.ReadLine() ?? "";

        try
        {
            fileName = "DodgyTransactions2015.csv";
            bankName = "TechSwitch Bank";

            DataFile dataFile = new(fileName,bankName,_logger);
            dataFile.LoadFileToBank();
            _logger.LogInformation($"File {fileName} loaded to Bank {bankName}");

            while (true)
            {
                Console.WriteLine("[1] List for the user");
                Console.WriteLine("[2] List All");
                Console.WriteLine("[3] Exit");
                Console.WriteLine("Enter the number of the action you would like to take:");
                if (int.TryParse(Console.ReadLine(), out var actionNumber))
                {
                    if (actionNumber == 1)
                    {
                        Console.Write("Enter the person's name you wish to see balance for : ");
                        var ownerName = Console.ReadLine() ?? "";
                        ownerName = "Laura B";
                        dataFile.List(ownerName);
                        _logger.LogInformation($"List for user name {ownerName}.");
                        continue;
                    }
                    else if (actionNumber == 2)
                    {
                        dataFile.ListAll();
                        _logger.LogInformation($"List for all users.");
                        continue;
                    }
                    else if(actionNumber ==3)
                        break;
                    else
                    {
                        //Console.WriteLine("Sorry, that wasn't one of the options.");
                        _logger.LogWarning($"User entered int which was outside from the int options given.");
                    }
                }
                else
                {
                    _logger.LogWarning($"User entered string instead of int set of options.");
                    //Console.WriteLine("Sorry, I didn't understand your response.");
                }
                break;
            }
        }
        catch (FileNotFoundException)
        {
            _logger.LogError($"File {fileName} was not found.");
            //Console.WriteLine("Sorry, that file was not found.");
        }        
    }
}