using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steady_Management.Domain
{
    public class City
    {
        private int cityId;
        private string cityName;
        private int province;
        private string country;

        public City(int cityId, string cityName, int province, string country)
        {
            this.cityId = cityId;
            this.cityName = cityName;
            this.province = province;
            this.country = country;
        }

        public int CityId { get => cityId; set => cityId = value; }
        public string CityName { get => cityName; set => cityName = value; }
        public int Province { get => province; set => province = value; }
        public string Country { get => country; set => country = value; }
    }
}
