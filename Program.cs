using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SupportBank.SupportBankClasses;

var servicesProvider = new ServiceCollection()
    .AddTransient<UserInterface>()
    .AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    })
    .BuildServiceProvider();

var userInterface = servicesProvider.GetRequiredService<UserInterface>();
userInterface.Run();



