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
        AnsiConsole.Markup("[bold]Zadejte jméno zaměstnance:[/] ");
        EmployeeDto employeeDto = new();
        employeeDto.EmployeeName = _validatorService.ValidateString(Console.ReadLine());
        if (string.IsNullOrWhiteSpace(employeeDto.EmployeeName))
        {
            AnsiConsole.MarkupLine("[yellow]Jméno zaměstnance je povinné.[/]");
            return GetEmployeeName();
        }
        return employeeDto;
    }

    public int GetShiftId()
    {
        AnsiConsole.Markup("[bold]Zadejte ID směny:[/] ");
        string input = Console.ReadLine() ?? "";
        if (int.TryParse(input, out int shiftId) && shiftId > 0)
        {
            return shiftId;
        }
        
        AnsiConsole.MarkupLine("[yellow]Neplatné ID směny. Zadejte prosím kladné číslo.[/]");
        return GetShiftId();
    }
}
