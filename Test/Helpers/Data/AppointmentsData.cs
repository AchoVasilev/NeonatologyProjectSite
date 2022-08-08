namespace Test.Helpers.Data;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using global::Data;
using global::Data.Models;

public static class AppointmentsData
{
    public static async Task GetAppointments(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DateTime = DateTime.UtcNow.AddDays(2)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DateTime = DateTime.UtcNow.AddDays(3)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task GetTakenAppointments(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                AddressId = 1,
                DateTime = DateTime.Now.AddDays(1)
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                IsDeleted = isDeleted,
                AddressId = 1,
                DateTime = DateTime.Now.AddDays(2)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                AddressId = 1,
                DateTime = DateTime.Now.AddDays(3)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task GetAppointmentsWithOneDeleted(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                AddressId = 1
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                IsDeleted = true,
                AddressId = 1
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                AddressId = 1
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task GetAppointmentsWithThreeDifferentDates(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DateTime = DateTime.UtcNow.AddDays(2)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DateTime = DateTime.UtcNow.AddDays(3)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task GetAppointmentsWithOneNegativeDay(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DateTime = DateTime.UtcNow.AddDays(2)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DateTime = DateTime.UtcNow.AddDays(-3)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task GetAppointmentsWithAddress(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                AddressId = 1,
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                AddressId = 1
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                AddressId = 1
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task AppointmentsWithDoctorId(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow.AddDays(10),
                IsDeleted = isDeleted
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow.AddDays(10)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow.AddDays(10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task AppointmentsWithDoctorIdForToday(NeonatologyDbContext dataMock)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                Id = 1,
                AppointmentCauseId = 1,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                Id = 2,
                AppointmentCauseId = 2,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                Id = 3,
                AppointmentCauseId = 3,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task AppointmentsWithDoctorIdWithNegativeDays(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DoctorId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10),
                IsDeleted = isDeleted
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DoctorId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DoctorId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task AppointmentsWithPatientId(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                Id = 1,
                AppointmentCauseId = 1,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(10),
                IsDeleted = isDeleted
            },

            new Appointment
            {
                Id = 2,
                AppointmentCauseId = 2,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(10)
            },

            new Appointment
            {
                Id = 3,
                AppointmentCauseId = 3,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }

    public static async Task AppointmentsWithPatientIdWithNegativeDays(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10),
                IsDeleted = isDeleted
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10)
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                PatientId = "pat",
                DateTime = DateTime.UtcNow.AddDays(-10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();
    }
}