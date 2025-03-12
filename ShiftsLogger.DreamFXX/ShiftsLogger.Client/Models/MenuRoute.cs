namespace ShiftsLogger.Client.Models;

public class MenuRoute
{
    public string Route { get; set; } = string.Empty;
    public Func<Task> Action { get; set; }


    public MenuRoute(string route, Func<Task> action)
    {
        Route = route;
        Action = action;
    }
}
