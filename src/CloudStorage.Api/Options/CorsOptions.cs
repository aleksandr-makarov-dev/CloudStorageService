namespace CloudStorage.Api.Options;

public sealed class CorsOptions
{
    public const string SectionName = nameof(CorsOptions);

    public string[] AllowedOrigins { get; init; } = [];
    public string[] AllowedHeaders { get; init; } = [];
    public string[] AllowedMethods { get; init; } = [];
    public bool AllowCredentials { get; init; }
}