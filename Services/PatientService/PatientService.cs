namespace Services.PatientService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Data;

    using Microsoft.EntityFrameworkCore;

    public class PatientService : IPatientService
    {
        private readonly NeonatologyDbContext data;

        public PatientService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task<string> GetPatientIdByUserId(string userId)
           => await this.data.Patients
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
    }
}
