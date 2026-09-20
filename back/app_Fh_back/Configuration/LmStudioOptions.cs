namespace app_Fh_back.Configuration;

public class LmStudioOptions
{
    public const string SectionName = "LmStudio";

    public string BaseUrl { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;
}