using ShiftsLogger.Client.Services;
using Spectre.Console;

var shiftsLoggerService = new ShiftsLoggerService();
try
{
    await shiftsLoggerService.Run();
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine($"[red]Critical error: {ex.Message}[/]");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}


