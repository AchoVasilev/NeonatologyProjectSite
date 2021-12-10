namespace Data.Seeding
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models.Dto;

    using Data.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Identity;
    using static global::Common.GlobalConstants;

    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
            var data = serviceProvider.GetRequiredService<NeonatologyDbContext>();

            data.Database.Migrate();

            await SeedCitiesAsync(data);
            await SeedAdministratorAsync(serviceProvider);
            await SeedDoctorAsync(data, serviceProvider);
            await SeedPatientRoleAsync(serviceProvider, PatientRoleName);

            return app;
        }

        private static async Task SeedCitiesAsync(NeonatologyDbContext data)
        {
            if (data.Cities.Any())
            {
                return;
            }

            var citiesJson = File.ReadAllText("Datasets/Cities.json");
            var citiesDto = JsonConvert.DeserializeObject<CityDto[]>(citiesJson);
            var cities = new List<City>();

            foreach (var cityDto in citiesDto)
            {
                var city = new City()
                {
                    Name = cityDto.Name,
                    ZipCode = cityDto.ZipCode
                };

                cities.Add(city);
            }

            await data.Cities.AddRangeAsync(cities);
            await data.SaveChangesAsync();
        }

        //private static async Task SeedSpecializations(NeonatologyDbContext data)
        //{
        //    if(data.Specializations.Any())
        //    {
        //        return;
        //    }

        //    var specializations = new List<SpecializationDto>();
        //    specializations.Add(new SpecializationDto
        //    {
        //        Name = "Неонатология",
        //        Description = "Подспециалност на детските болести, която се занимава с медицински грижи за новородените, по-специално болните или недоносените."
        //    });

        //    specializations.Add(new SpecializationDto
        //    {
        //        Name = "Детски болести",
        //        Description = "Дял от медицината, който се занимава с проследяване на физическото и нервно-психическото развитие на детския организъм, диагностика и лечения на детски заболявания."
        //    });

        //    foreach (var specializationDto in specializations)
        //    {
        //        var specialization = new Specialization()
        //        {
        //            Name = specializationDto.Name,
        //            Description = specializationDto.Description
        //        };

        //        await data.AddAsync(specialization);
        //    }

        //    await data.SaveChangesAsync();
        //}

        private static async Task SeedDoctorAsync(NeonatologyDbContext data, IServiceProvider serviceProvider)
        {
            if (data.Doctors.Any())
            {
                return;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var identityRole = new ApplicationRole()
            {
                Name = DoctorRoleName
            };

            await roleManager.CreateAsync(identityRole);

            var doctor = new ApplicationUser()
            {
                Email = DoctorEmail,
                UserName = DoctorEmail,
                EmailConfirmed = true,
                Doctor = new Doctor
                {
                    FirstName = DoctorFirstName,
                    LastName = DoctorLastName,
                    PhoneNumber = DoctorPhone,
                    Address = Address,
                    Age = DoctorAge,
                    Biography = Biography,
                    CityId = 582,
                    YearsOfExperience = YearsOfExperience
                }
            };

            var image = new Image()
            {
                Doctor = doctor.Doctor,
                RemoteImageUrl = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1639079041/264612402_286215640113581_1185377196531358262_n_putyw3.jpg"
            };

            doctor.Doctor.Images.Add(image);

            await userManager.CreateAsync(doctor, DoctorPassword);
            await userManager.AddToRoleAsync(doctor, identityRole.Name);
        }

        private static async Task SeedAdministratorAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (await roleManager.RoleExistsAsync(AdministratorRoleName))
            {
                return;
            }

            var identityRole = new ApplicationRole()
            {
                Name = AdministratorRoleName
            };

            await roleManager.CreateAsync(identityRole);

            const string adminEmail = AdministratorEmail;
            const string adminPassword = AdministratorPassword;

            var adminUser = new ApplicationUser()
            {
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, adminPassword);
            await userManager.AddToRoleAsync(adminUser, identityRole.Name);
        }

        private static async Task SeedPatientRoleAsync(IServiceProvider serviceProvider, string roleName)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
