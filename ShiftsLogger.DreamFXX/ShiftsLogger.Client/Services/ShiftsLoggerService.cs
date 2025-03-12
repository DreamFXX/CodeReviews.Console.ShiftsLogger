using ShiftsLogger.Client.UI;

namespace ShiftsLogger.Client.Services;

public class ShiftsLoggerService
{
    private readonly HttpClient _httpClient;
    private readonly APIService _apiService;
    private readonly ValidatorService _validatorService;
    private readonly UserInput _userInput;

    public ShiftsLoggerService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7299") };
        _apiService = new APIService(_httpClient);
        _validatorService = new ValidatorService();
        _userInput = new UserInput();
    }

}
