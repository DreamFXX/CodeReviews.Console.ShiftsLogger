namespace ShiftsLogger.Client.Models;

public class MenuRoute(string route, Func<Task> action)
{
    public string Route { get; set; } = route;
    public Func<Task> Action { get; set; } = action;
    
    public override string ToString()
    {
        return Route;
    }
}
