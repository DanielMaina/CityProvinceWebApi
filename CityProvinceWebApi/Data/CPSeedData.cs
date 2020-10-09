using CityProvinceWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityProvinceWebApi.Data
{
    public static class CPSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CityProvinceContext(
                serviceProvider.GetRequiredService<DbContextOptions<CityProvinceContext>>()))
            {
                // Look for any Doctors.  Since we can't have patients without Doctors.
                if (!context.Provinces.Any())
                {
                    context.Provinces.AddRange(
                     new Province
                     {
                         Code = "ON",
                         Name = "Ontario"
                     }, new Province
                     {
                         Code = "PE",
                         Name = "Prince Edward Island"
                     }, new Province
                     {
                         Code = "NB",
                         Name = "New Brunswick"
                     }, new Province
                     {
                         Code = "BC",
                         Name = "British Columbia"
                     }, new Province
                     {
                         Code = "NL",
                         Name = "Newfoundland and Labrador"
                     }, new Province
                     {
                         Code = "SK",
                         Name = "Saskatchewan"
                     }, new Province
                     {
                         Code = "AB",
                         Name = "Alberta"
                     }, new Province
                     {
                         Code = "NS",
                         Name = "Nova Scotia"
                     }, new Province
                     {
                         Code = "MB",
                         Name = "Manitoba"
                     }, new Province
                     {
                         Code = "QC",
                         Name = "Quebec"
                     }, new Province
                     {
                         Code = "NT",
                         Name = "Northwest Territories"
                     }, new Province
                     {
                         Code = "YT",
                         Name = "Yukon"
                     }, new Province
                     {
                         Code = "NU",
                         Name = "Nunavut"
                     }

                );
                    context.SaveChanges();
                }
                if (!context.Cities.Any())
                {
                    context.Cities.AddRange(
                    new City
                    {
                        Name = "Toronto",
                        Population = "2503281",
                        CensusYear = "2006",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "ON").ID
                    },
                    new City
                    {
                        Name = "Halifax",
                        Population = "372679",
                        CensusYear = "2005",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "NS").ID
                    },
                    new City
                    {
                        Name = "Calgary",
                        Population = "988193",
                        CensusYear = "2007",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "AB").ID
                    },
                    new City
                    {
                        Name = "Winnipeg",
                        Population = "653431",
                        CensusYear = "2006",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "MB").ID
                    },
                    new City
                    {
                        Name = "St. Catharines",
                        Population = "131989",
                        CensusYear = "2008",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "ON").ID
                    }, new City
                    {
                        Name = "Toronto",
                        Population = "11",
                        CensusYear = "2008",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "PE").ID
                    },
                    new City
                    {
                        Name = "Stratford",
                        Population = "7083",
                        CensusYear = "2006",
                        ProvinceID = context.Provinces.FirstOrDefault(p => p.Code == "ON").ID
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
