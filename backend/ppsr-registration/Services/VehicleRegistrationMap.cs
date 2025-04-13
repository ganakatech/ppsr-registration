using CsvHelper.Configuration;
using PpsrRegistration.Models;

namespace PpsrRegistration.Services;

public sealed class VehicleRegistrationMap : ClassMap<VehicleRegistration>
{
    public VehicleRegistrationMap()
    {
        Map(m => m.GrantorFirstName).Name("Grantor First Name");
        Map(m => m.GrantorMiddleNames).Name("Grantor Middle Names");
        Map(m => m.GrantorLastName).Name("Grantor Last Name");
        Map(m => m.VIN).Name("VIN");
        Map(m => m.StartDate).Name("Registration start date");
        Map(m => m.Duration).Name("Registration duration");
        Map(m => m.SPG_ACN).Name("SPG ACN");
        Map(m => m.SPG_OrgName).Name("SPG Organization Name");
    }
}
