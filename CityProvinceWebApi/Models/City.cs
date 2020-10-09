using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityProvinceWebApi.Models
{
    public class City
    {
        public int ID { get; set; }

        [Display(Name = "City")]
        public string Summary
        {
            get
            {
                return Name + ", " + Province?.Code;
            }
        }

        [Display(Name = "City Name")]
        [Required(ErrorMessage = "You cannot leave the name of the city blank.")]
        [StringLength(255, ErrorMessage = "City name cannot be more than 255 characters long.")]
        public string Name { get; set; }

        [StringLength(8, ErrorMessage = "Population too large.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Please enter the population as an integer.")]
        public string Population { get; set; }

        [Display(Name = "Census Year")]
        [StringLength(4)]
        [RegularExpression("^\\d{4}$", ErrorMessage = "Please enter 4 digits for the census year.")]
        public string CensusYear { get; set; }

        [Timestamp]
        public Byte[] RowVersion { get; set; }

        [Display(Name = "Province")]
        public int ProvinceID { get; set; }

        public virtual Province Province { get; set; }
    }
}
