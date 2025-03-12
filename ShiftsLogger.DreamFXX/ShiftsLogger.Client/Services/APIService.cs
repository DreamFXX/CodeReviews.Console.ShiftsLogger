namespace ShiftsLogger.Client.Services;

public class APIService
{
    private readonly HttpClient _httpClient;
    public APIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


}


