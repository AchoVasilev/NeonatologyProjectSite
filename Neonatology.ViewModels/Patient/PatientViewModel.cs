namespace Neonatology.ViewModels.Patient;

using System.ComponentModel.DataAnnotations;
using Common.Constants;
using Common.Mapping;
using Data.Models;
using static Common.Constants.GlobalConstants.AccountConstants;

public class PatientViewModel : IMapFrom<Patient>
{
    public string Id { get; set; }

    [Display(Name = Name)]
    public string FirstName { get; set; }

    [Display(Name = FamilyName)]
    public string LastName { get; set; }

    [Display(Name = GlobalConstants.AccountConstants.Phone)]
    public string Phone { get; set; }

    public bool HasPaid { get; set; }
}