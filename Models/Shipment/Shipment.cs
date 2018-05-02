using ContainerManagementSystem.CommonEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.Shipment
{
    [Table("Shipments")]
    public class Shipment
    {
        public Guid ShipmentId { get; set; }
		public string ShipmentNo {get;set;}
        public ShipmentStatus ShipmentStatus { get; set; }
		public Guid ShipmentCurrentCityId {get;set;}
        public Guid ShippingMethod { get; set; }
        public Guid? CustomerId { get; set; }
		public Guid SenderCountryId {get;set;}
		public Guid SenderCityId {get;set;}
		public Guid DestinationCountryId {get;set;}
		public Guid DestinationCityId {get;set;}
		public Guid ShipmentRouteId {get;set;}
		public int ShipmentRouteIndex {get;set;}
    }
}