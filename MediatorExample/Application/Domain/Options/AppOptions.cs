namespace MediatorExample.Application.Domain.Options;

public class AppOptions
{
    public static string SectionKey = "MediatorExample";
    
    /// <summary>
    /// Message for funny things -> Example for dependency injected options
    /// </summary>
    public string FunnyMessage { get; set; } = null!;

}