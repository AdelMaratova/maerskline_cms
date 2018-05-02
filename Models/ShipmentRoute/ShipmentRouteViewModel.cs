using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.ShipmentRoute
{
    public class ShipmentRouteViewModel
    {
        public Guid ShipmentRouteId { get; set; }
        public string ShipmentRouteName { get; set; }
        public Guid SenderCountryId { get; set; }
        public string SenderCountryName { get; set; }
        public Guid SenderCityId { get; set; }
        public string SenderCityName { get; set; }
        public Guid DestinationCountryId { get; set; }
        public string DestinationCountryName { get; set; }
        public Guid DestinationCityId { get; set; }
        public string DestinationCityName { get; set; }

        private List<ShipmentRouteDetailViewModel> _RouteDetails { get; set; }
        public List<ShipmentRouteDetailViewModel> RouteDetails 
        {
            get
            {
                if (_RouteDetails == null)
                    _RouteDetails = new List<ShipmentRouteDetailViewModel>();

                return _RouteDetails;
            }
            set
            {
                _RouteDetails = value == null ? new List<ShipmentRouteDetailViewModel>() : _RouteDetails;
            }
        }
    }

    public class ShipmentRouteDetailViewModel
    {
        public Guid ShipmentRouteDetailId {get;set;}
		public int ShipmentRouteDetailIndex {get;set;}
        public Guid ShipmentRouteId { get; set; }
        public int ShipmentouteDetailDayDuration { get; set; }
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }
		public Guid CityId {get;set;}
        public string CityName { get; set; }
    }
}