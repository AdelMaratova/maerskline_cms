using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.Country
{
    [Table("Countries")]
    public class Country
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
    }
}