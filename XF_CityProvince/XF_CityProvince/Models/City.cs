using System;
using System.Collections.Generic;
using System.Text;

namespace XF_CityProvince.Models
{
    public class City
    {
        public int ID { get; set; }

        public string Summary=> Name + ", " + Province?.Code;

        public string PopSummary => "Population: " + Population + " as of " + CensusYear;

        public string Name { get; set; }

        public string Population { get; set; }

        public string CensusYear { get; set; }

        public Byte[] RowVersion { get; set; }

        public int ProvinceID { get; set; }

        public virtual Province Province { get; set; }
    }
}
