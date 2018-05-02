using ContainerManagementSystem.CommonEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.Shipment
{
    public class ShipmentViewModel
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNo { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }

        public Guid ShipmentCurrentCityId { get; set; }
        public string ShipmentCurrentCity { get; set; }

        public Guid ShipmentNextCityId { get; set; }
        public string ShipmentNextCity { get; set; }

        public Guid? ShippingMethod { get; set; }


        public Guid SenderCountryId { get; set; }
        public string SenderCountry { get; set; }


        public Guid SenderCityId { get; set; }
        public string SenderCity { get; set; }


        public Guid DestinationCountryId { get; set; }
        public string DestinationCountry { get; set; }


        public Guid DestinationCityId { get; set; }
        public string DestinationCity { get; set; }


        public Guid ShipmentRouteId { get; set; }
        public int ShipmentRouteIndex { get; set; }

        public DateTime ShippingDate { get; set; }

        public Guid? CustomerId { get; set; }
    }
}