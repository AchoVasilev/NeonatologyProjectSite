namespace Services.PatientService
{
    using System.Threading.Tasks;

    public interface IPatientService
    {
        public Task<string> GetPatientIdByUserId(string userId);
    }
}
