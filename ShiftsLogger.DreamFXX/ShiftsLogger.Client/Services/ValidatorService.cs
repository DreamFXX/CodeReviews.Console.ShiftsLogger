namespace ShiftsLogger.Client.Services;

public class ValidatorService
{
    public string ValidateString(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }
        return input.Trim();
    }
}
