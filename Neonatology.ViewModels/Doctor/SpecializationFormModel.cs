namespace Neonatology.ViewModels.Doctor;

using Common.Mapping;
using Data.Models;

public class SpecializationFormModel : IMapFrom<Specialization>
{
    public string Name { get; set; }

    public string Description { get; set; }
}