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

    public GetShiftStartDetails()
    {
        var shift = new WorkerNameDto();

        return null;
    }


}
