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

            if (data.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                data.Database.Migrate();
            }

            await SeedCitiesAsync(data);
            await SeedAdministratorAsync(serviceProvider);
            await SeedDoctorAsync(data, serviceProvider);
            await SeedPatientRoleAsync(serviceProvider, PatientRoleName);
            await SeedSpecializations(data);
            await SeedOfferedServices(data);
            await SeedAppointmentCause(data);
            await SeedNotificationTypes(data);
            await SeedGaleryImages(data);

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

        private static async Task SeedSpecializations(NeonatologyDbContext data)
        {
            if (data.Specializations.Any())
            {
                return;
            }

            var specializations = new List<SpecializationDto>();
            specializations.Add(new SpecializationDto
            {
                Name = DoctorConstants.NeonatologySpecializationName,
                Description = DoctorConstants.NeonatologySpecializationDescription,
            });

            specializations.Add(new SpecializationDto
            {
                Name = DoctorConstants.ChildSicknessName,
                Description = DoctorConstants.ChildSicknessDescription
            });

            var doctor = await data.Doctors.FirstAsync();
            foreach (var specializationDto in specializations)
            {
                var specialization = new Specialization()
                {
                    Name = specializationDto.Name,
                    Description = specializationDto.Description,
                    DoctorId = doctor.Id,
                    Doctor = doctor
                };

                await data.AddAsync(specialization);
            }

            await data.SaveChangesAsync();
        }

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
                Name = DoctorConstants.DoctorRoleName
            };

            await roleManager.CreateAsync(identityRole);

            var plevenCity = await data.Cities.Where(x => x.Name == DoctorConstants.PlevenCityName).FirstOrDefaultAsync();
            var gabrovoCity = await data.Cities.Where(x => x.Name == DoctorConstants.GabrovoCityName).FirstOrDefaultAsync();

            var addresses = new List<Address>
            {
                new Address
                {
                    CityId = plevenCity.Id,
                    StreetName = DoctorConstants.PlevenAddress,
                },
                new Address
                {
                    CityId = gabrovoCity.Id,
                    StreetName = DoctorConstants.GabrovoAddress,
                }
            };

            var doctor = new ApplicationUser()
            {
                Email = DoctorConstants.DoctorEmail,
                UserName = DoctorConstants.DoctorEmail,
                EmailConfirmed = true,
                Doctor = new Doctor
                {
                    FirstName = DoctorConstants.DoctorFirstName,
                    LastName = DoctorConstants.DoctorLastName,
                    PhoneNumber = DoctorConstants.DoctorPhone,
                    Age = DoctorConstants.DoctorAge,
                    Biography = DoctorConstants.Biography,
                    YearsOfExperience = DoctorConstants.YearsOfExperience,
                    Email = DoctorConstants.DoctorEmail,
                    Addresses = addresses
                }
            };

            await userManager.CreateAsync(doctor, DoctorConstants.DoctorPassword);
            await userManager.AddToRoleAsync(doctor, identityRole.Name);

            doctor.Doctor.UserId = doctor.Id;

            doctor.Image = new Image()
            {
                UserId = doctor.Id,
                Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1639079041/264612402_286215640113581_1185377196531358262_n_putyw3.jpg",
                RemoteImageUrl = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1639079041/264612402_286215640113581_1185377196531358262_n_putyw3.jpg",
                Extension = ".jpg"
            };

            await data.SaveChangesAsync();
        }

        private static async Task SeedOfferedServices(NeonatologyDbContext data)
        {
            if (data.OfferedServices.Any())
            {
                return;
            }

            await data.OfferedServices.AddRangeAsync(new[]
            {
                new OfferedService() {Name = "Първичен преглед", Price = 60.00m},
                new OfferedService() {Name = "Вторичен преглед в рамките на едно заболяване", Price = 30.00m},
                new OfferedService() {Name = "Консултация по документи", Price = 30.00m},
                new OfferedService() {Name = "Онлайн консултация", Price = 30.00m},
                new OfferedService() {Name = "Домашно посещение в рамките на гр. Плевен", Price = 80.00m},
                new OfferedService() {Name = "Вземане на венозна кръв и поставяне на абокат", Price = 20.00m},
                new OfferedService() {Name = "Поставяне на мускулна инжекция", Price = 10.00m},
                new OfferedService() {Name = "Измерване на АН (кръвно налягане)", Price = 05.00m},
                new OfferedService() {Name = "Вземане на периферен секрет за микробиологично изследване", Price = 10.00m},
                new OfferedService() {Name = "Стомашна промивка", Price = 25.00m},
                new OfferedService() {Name = "Клизма", Price = 15.00m},
                new OfferedService() {Name = "Инхалация на медикамент", Price = 10.00m},
                new OfferedService() {Name = "Първична обработка на рана и поставяне на тетанус", Price = 25.00m}
            });

            await data.SaveChangesAsync();
        }

        private static async Task SeedAppointmentCause(NeonatologyDbContext data)
        {
            if (await data.AppointmentCauses.AnyAsync())
            {
                return;
            }

            var appointmentCauses = new List<AppointmentCause>
            {
                new AppointmentCause() {Name = "Първичен преглед"},
                new AppointmentCause() {Name = "Вторичен преглед"},
                new AppointmentCause() {Name = "Детско здравеопазване"},
                new AppointmentCause() {Name = "Свободен прием"},
            };

            await data.AddRangeAsync(appointmentCauses);
            await data.SaveChangesAsync();
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

            if (await userManager.IsInRoleAsync(adminUser, identityRole.Name))
            {
                return;
            }

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

        private static async Task SeedNotificationTypes(NeonatologyDbContext data)
        {
            if (await data.NotificationTypes.AnyAsync())
            {
                return;
            }

            var notificationTypes = new List<NotificationType>
            {
                new NotificationType() {Name = "Message"},
                new NotificationType() {Name = "Banned"},
                new NotificationType() {Name = "Paid"},
                new NotificationType() {Name = "Feedback"},
            };

            await data.NotificationTypes.AddRangeAsync(notificationTypes);
            await data.SaveChangesAsync();
        }

        private static async Task SeedGaleryImages(NeonatologyDbContext data)
        {
            if (data.Images.Count() > 2)
            {
                return;
            }

            var images = new List<Image>
            {
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085032/pediamed/IMG-fd628ff6ab68f73643fd7b0cf41868ab-V.jpg.jpg",
                    Name = "IMG-fd628ff6ab68f73643fd7b0cf41868ab-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085031/pediamed/IMG-f1c897a19a277fb5f6b3adc1c1668e8e-V.jpg.jpg",
                    Name = "IMG-f1c897a19a277fb5f6b3adc1c1668e8e-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085031/pediamed/IMG-e415d77485530c8a65dd356e3fe820e9-V.jpg.jpg",
                    Name = "IMG-e415d77485530c8a65dd356e3fe820e9-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085030/pediamed/IMG-b93856f2c12b1dcaa36a80c8b886cd30-V.jpg.jpg",
                    Name = "IMG-b93856f2c12b1dcaa36a80c8b886cd30-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085030/pediamed/IMG-ab0a5b033ee8cf53def9e937f6d2baf6-V.jpg.jpg",
                    Name = "IMG-ab0a5b033ee8cf53def9e937f6d2baf6-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085029/pediamed/IMG-4817c65b81006eea5c2815837337f8ed-V.jpg.jpg",
                    Name = "IMG-4817c65b81006eea5c2815837337f8ed-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085029/pediamed/IMG-73d4aaf2edbfbd86e3bc8b6b44c1dfa2-V.jpg.jpg" ,
                    Name = "IMG-73d4aaf2edbfbd86e3bc8b6b44c1dfa2-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085028/pediamed/IMG-9add7b0ba788ced6c9f6186f01dce2ba-V.jpg.jpg" ,
                    Name = "IMG-9add7b0ba788ced6c9f6186f01dce2ba-V.jpg.jpg",
                    Extension = "jpg"
                },
                new Image()
                {
                    Url = "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1640085027/pediamed/IMG-3d4d5a079b4f712fd53ce9ad097faf24-V.jpg.jpg",
                    Name = "IMG-3d4d5a079b4f712fd53ce9ad097faf24-V.jpg.jpg",
                    Extension = "jpg"
                },
            };

            await data.Images.AddRangeAsync(images);
            await data.SaveChangesAsync();
        }
    }
}
