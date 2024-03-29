﻿namespace Neonatology.Services.CityService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Neonatology.Common.Models;
using ViewModels.Address;
using ViewModels.City;

public interface ICityService : ITransientService
{
    Task<ICollection<CityFormModel>> GetAllCities();

    Task<int> GetCityIdByName(string cityName);

    Task<CityFormModel> GetCityById(int id);

    Task<OperationResult> CityExists(int id);

    Task<ICollection<AddressFormModel>> GetDoctorAddressesByDoctorId(string doctorId);
}