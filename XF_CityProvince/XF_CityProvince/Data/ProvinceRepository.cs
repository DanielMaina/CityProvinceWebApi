using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XF_CityProvince.Models;
using XF_CityProvince.Utilities;

namespace XF_CityProvince.Data
{
    public class ProvinceRepository : IProvinceRepository
    {
        HttpClient client = new HttpClient();

        public ProvinceRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Province>> GetProvinces()
        {
            var response = await client.GetAsync("api/provinces");
            if (response.IsSuccessStatusCode)
            {
                List<Province> provinces = await response.Content.ReadAsAsync<List<Province>>();
                return provinces;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }

        }

        public async Task<Province> GetProvince(int ID)
        {
            var response = await client.GetAsync($"api/provinces/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Province province = await response.Content.ReadAsAsync<Province>();
                return province;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddProvince(Province provinceToAdd)
        {
            var response = await client.PostAsJsonAsync("api/provinces", provinceToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateProvince(Province provinceToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/provinces/{provinceToUpdate.ID}", provinceToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteProvince(Province provinceToDelete)
        {
            var response = await client.DeleteAsync($"api/provinces/{provinceToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }

        }
    }
}
