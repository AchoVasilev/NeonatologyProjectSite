namespace Test.Helpers;

using System.Threading.Tasks;
using Data;
using Data.Models;

public static class DoctorData
{
    public static async Task OneDoctor(NeonatologyDbContext dataMock)
    {
        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862",
            UserId = "docdoc"
        };
        
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.SaveChangesAsync();
    }
}