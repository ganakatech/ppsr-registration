namespace PpsrRegistration.Services;

public class BatchSummary
{
    public int Submitted { get; set; }
    public int Invalid { get; set; }
    public int Processed { get; set; }
    public int Updated { get; set; }
    public int Added { get; set; }
}