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
    public class CityRepository : ICityRepository
    {
        HttpClient client = new HttpClient();
        public CityRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<City>> GetCities()
        {
            var response = await client.GetAsync("api/cities");
            if (response.IsSuccessStatusCode)
            {
                List<City> Cities = await response.Content.ReadAsAsync<List<City>>();
                return Cities;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task<List<City>> GetCitiesByProvince(int ProvinceID)
        {
            var response = await client.GetAsync($"api/cities/byProvince/{ProvinceID}");
            if (response.IsSuccessStatusCode)
            {
                List<City> Cities = await response.Content.ReadAsAsync<List<City>>();
                return Cities;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task<City> GetCity(int ID)
        {
            var response = await client.GetAsync($"api/cities/{ID}");
            if (response.IsSuccessStatusCode)
            {
                City City = await response.Content.ReadAsAsync<City>();
                return City;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddCity(City cityToAdd)
        {
            var response = await client.PostAsJsonAsync("api/cities", cityToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateCity(City cityToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/cities/{cityToUpdate.ID}", cityToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteCity(City cityToDelete)
        {
            var response = await client.DeleteAsync($"api/cities/{cityToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
