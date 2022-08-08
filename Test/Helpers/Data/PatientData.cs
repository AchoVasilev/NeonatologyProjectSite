namespace Test.Helpers.Data;

using System.Threading.Tasks;
using global::Data;
using global::Data.Models;

public class PatientData
{
    public static async Task OnePatient(NeonatologyDbContext dataMock)
    {
        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Evlogi",
            LastName = "Manev",
            Phone = "098787862",
            UserId = "patpat"
        };
        
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();
    }
}