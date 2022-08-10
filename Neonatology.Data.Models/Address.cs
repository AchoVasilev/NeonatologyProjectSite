namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using static Common.DataConstants.Constants;

public class Address : BaseModel<int>
{
    [ForeignKey(nameof(City))]
    public int CityId { get; set; }

    public virtual City City { get; set; }

    [MaxLength(AddressMaxLength)]
    public string StreetName { get; set; }

    [ForeignKey(nameof(Doctor))]
    public string DoctorId { get; set; }

    public virtual Doctor Doctor { get; set; }

    [ForeignKey(nameof(Patient))]
    public string PatientId { get; set; }

    public virtual Patient Patient { get; set; }
}