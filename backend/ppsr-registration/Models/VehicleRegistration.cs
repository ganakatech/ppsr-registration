namespace PpsrRegistration.Models;
public class VehicleRegistration
{
    public int Id { get; set; }
    public string GrantorFirstName { get; set; } = string.Empty;
    public string? GrantorMiddleNames { get; set; }
    public string GrantorLastName { get; set; } = string.Empty;
    public string VIN { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string SPG_ACN { get; set; } = string.Empty;
    public string SPG_OrgName { get; set; } = string.Empty;
}


