using ShiftsLogger.Client.Dtos;
using ShiftsLogger.Client.Services;

namespace ShiftsLogger.Client.UI;
public class UserInput
{
    private readonly ValidatorService _validatorService;
    public UserInput(ValidatorService validatorService)
    {
        _validatorService = validatorService;
    }

    public WorkerNameDto GetDetailsForStart()
    {
        var shiftStart = new WorkerNameDto();

        shiftStart.WorkerName = _validatorService.ValidateString(Console.ReadLine());
        if (shiftStart.WorkerName == null)
        {
            Console.WriteLine("Worker name is required. Please enter a valid worker name!");
            return GetDetailsForStart();
        }
        return shiftStart;
    }
}
