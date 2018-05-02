using ContainerManagementSystem.CommonEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.ShipmentHistory
{
    public class ShipmentHistoryViewModel
    {
        public Guid ShipmentId { get; set; }
        public string ShipmentNo { get; set; }
        public string ShipmentStatus { get; set; }

        public Guid ShipmentCurrentCityId { get; set; }
        public string ShipmentCurrentCity { get; set; }

        public Guid ShipmentNextCityId { get; set; }
        public string ShipmentNextCity { get; set; }

        public ShippingMethod? ShippingMethod { get; set; }


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
    }
}