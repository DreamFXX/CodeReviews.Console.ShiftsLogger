using ShiftsLogger.Client.Services;
using Spectre.Console;

var shiftsLoggerService = new ShiftsLoggerService();
try
{
    await shiftsLoggerService.Run();
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine($"[red]Kritická chyba: {ex.Message}[/]");
    Console.WriteLine("Stiskněte libovolnou klávesu pro ukončení...");
    Console.ReadKey();
}


