using ShiftsLogger.Client.Services;

var shiftsLoggerService = new ShiftsLoggerService();
try
{
    await shiftsLoggerService.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Critical error: {ex.Message}");
}


