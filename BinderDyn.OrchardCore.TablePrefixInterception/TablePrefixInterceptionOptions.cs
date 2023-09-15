namespace BinderDyn.OrchardCore.TablePrefixInterception;

public class TablePrefixInterceptionOptions
{
    public string[] TableNamesToPrefix { get; set; } = Array.Empty<string>();
}