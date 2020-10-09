using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF_CityProvince.Models;

namespace XF_CityProvince.Data
{
    interface ICityRepository
    {
        Task<List<City>> GetCities();
        Task<City> GetCity(int ID);
        Task<List<City>> GetCitiesByProvince(int ProvinceID);
        Task AddCity(City cityToAdd);
        Task UpdateCity(City cityToUpdate);
        Task DeleteCity(City cityToDelete);
    }
}
