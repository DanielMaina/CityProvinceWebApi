using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF_CityProvince.Models;

namespace XF_CityProvince.Data
{
    interface IProvinceRepository
    {
        Task<List<Province>> GetProvinces();
        Task<Province> GetProvince(int ID);
        Task AddProvince(Province ProvinceToAdd);
        Task UpdateProvince(Province ProvinceToUpdate);
        Task DeleteProvince(Province ProvinceToDelete);
    }
}
