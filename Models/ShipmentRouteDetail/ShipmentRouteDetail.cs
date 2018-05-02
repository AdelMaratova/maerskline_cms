using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.ShipmentRouteDetail
{
    [Table("ShipmentRouteDetails")]
    public class ShipmentRouteDetail
    {
        public Guid ShipmentRouteDetailId {get;set;}
		public int ShipmentRouteDetailIndex {get;set;}
        public Guid ShipmentRouteId { get; set; }
        public int ShipmentouteDetailDayDuration { get; set; }
        public Guid CountryId { get; set; }
		public Guid CityId {get;set;}
    }
}