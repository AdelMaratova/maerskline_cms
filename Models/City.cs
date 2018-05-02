using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models
{
    [Table("Cities")]
    public class City
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public Guid CountryId { get; set; }
        public int YardCapacity { get; set; }
    }
}