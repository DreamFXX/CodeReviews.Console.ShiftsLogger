namespace ShiftsLogger.Client.Services;

public class ValidatorService
{
    public string? ValidateString(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }
        return input;
    }
}
