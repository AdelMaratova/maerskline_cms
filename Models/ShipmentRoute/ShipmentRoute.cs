using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.ShipmentRoute
{
    [Table("ShipmentRoutes")]
    public class ShipmentRoute
    {
        public Guid ShipmentRouteId { get; set; }
        public string ShipmentRouteName { get; set; }
		public Guid SenderCountryId {get;set;}
		public Guid SenderCityId {get;set;}
		public Guid DestinationCountryId {get;set;}
		public Guid DestinationCityId {get;set;}
    }
}