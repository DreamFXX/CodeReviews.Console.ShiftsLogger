using ShiftsLogger.Client.Models;
using ShiftsLogger.Client.Services;
using Spectre.Console;

namespace ShiftsLogger.Client.UI;
public class UserInput
{
    private readonly ValidatorService _validatorService;
    public UserInput(ValidatorService validatorService)
    {
        _validatorService = validatorService;
    }

    public EmployeeDto GetEmployeeName()
    {
        AnsiConsole.Markup("[bold]Enter employee name:[/] ");
        EmployeeDto employeeDto = new();
        employeeDto.EmployeeName = _validatorService.ValidateString(Console.ReadLine());
        if (string.IsNullOrWhiteSpace(employeeDto.EmployeeName))
        {
            AnsiConsole.MarkupLine("[yellow]Employee name is required.[/]");
            return GetEmployeeName();
        }
        return employeeDto;
    }

    public int GetShiftId()
    {
        AnsiConsole.Markup("[bold]Enter shift ID:[/] ");
        string input = Console.ReadLine() ?? "";
        if (int.TryParse(input, out int shiftId) && shiftId > 0)
        {
            return shiftId;
        }
        
        AnsiConsole.MarkupLine("[yellow]Invalid shift ID. Please enter a positive number.[/]");
        return GetShiftId();
    }
}
