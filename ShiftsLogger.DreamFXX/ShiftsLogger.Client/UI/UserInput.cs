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

    public EmployeeDto GetDetailsForStart()
    {
        AnsiConsole.Markup("[bold]Enter name of the employee:[/]");
        EmployeeDto? employeeDto = new();
        employeeDto.EmployeeName = _validatorService.ValidateString(Console.ReadLine());
        if (employeeDto.EmployeeName == null)
        {
            AnsiConsole.WriteLine("[yellow]Worker name is required.[/]");
            return GetDetailsForStart();
        }
        return employeeDto;
    }

    public EmployeeDto GetDetailsForEnd()
    {
        AnsiConsole.Markup("[bold]Enter name of the employee:[/]");
        EmployeeDto? employeeDto = new EmployeeDto();
        employeeDto.EmployeeName = _validatorService.ValidateString(Console.ReadLine());
        if (employeeDto.EmployeeName == null)
        {
            AnsiConsole.WriteLine("[yellow]Worker name is required.[/]");
            return GetDetailsForEnd();
        }
        return employeeDto;
    }
}
