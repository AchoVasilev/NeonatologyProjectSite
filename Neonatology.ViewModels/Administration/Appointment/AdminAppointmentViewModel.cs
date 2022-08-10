namespace Neonatology.ViewModels.Administration.Appointment;

using System;
using Common.Mapping;
using Data.Models;

public class AdminAppointmentViewModel : IMapFrom<Appointment>
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public DateTime End { get; set; }

    public string PatientFirstName { get; set; }

    public string PatientLastName { get; set; }

    public string PatientPhone { get; set; }

    public string ChildFirstName { get; set; }

    public string AppointmentCauseName { get; set; }

    public string DoctorName { get; set; }

    public bool IsRated { get; set; }

    public int RatingNumber { get; set; }

    public string RatingComment { get; set; }
}