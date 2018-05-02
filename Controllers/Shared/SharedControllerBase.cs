using ContainerManagementSystem.CommonEnum;
using ContainerManagementSystem.DAL;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.Country;
using ContainerManagementSystem.Models.ShipmentRoute;
using ContainerManagementSystem.Models.ShipmentRouteDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainerManagementSystem.Controllers.Shared
{
    public class SharedControllerBase : Controller
    {
        protected CMSContext context = new CMSContext();

        public JsonResult GetRouteScheduleString(Guid route_id, Guid? method_id)
        {
            string route_schedule_string = string.Empty;
            List<Country> countries = context.Countries.ToList();
            List<City> cities = context.Cities.ToList();
            List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == route_id).OrderBy(x => x.ShipmentRouteDetailIndex).ToList();

            route_schedule_string += "<ul>";

            int additional_days = 0;
            List<string> visited_countries = new List<string>();
            List<string> visited_cities = new List<string>();
            List<string> visited_dates = new List<string>();

            for (int i = 0; i < route_details.Count;i++ )
            {
                additional_days += route_details[i].ShipmentouteDetailDayDuration;

                string country_name = countries.Where(x => x.CountryId == route_details[i].CountryId).FirstOrDefault().CountryName;
                string city_name = cities.Where(x => x.CityId == route_details[i].CityId).FirstOrDefault().CityName;

                visited_countries.Add(country_name);
                visited_cities.Add(city_name);
                visited_dates.Add(DateTime.Now.AddDays(additional_days).ToString("dd MMM yyyy - hh:mm"));

                route_schedule_string += "<li>" + country_name + " | " + city_name + " at " + DateTime.Now.AddDays(additional_days).ToString("dd MMM yyyy - hh:mm") + "</li>";
            }
            route_schedule_string += "</ul>";

            return Json(new {DepartureDate = DateTime.Now.ToString("dd MMM yyyy - hh:mm"), ArrivalDate = DateTime.Now.AddDays(additional_days).ToString("dd MMM yyyy - hh:mm"), TotalTravelDays = additional_days
                            , VisitedCountries = visited_countries , VisitedCities = visited_cities, VisitedDates = visited_dates
                            , strValue = route_schedule_string}, JsonRequestBehavior.AllowGet);
        }

        public string GetCityOptionsByCounmtryId(Guid country_id)
        {
            string options = string.Empty;

            List<City> cities = new List<City>();

            cities = context.Cities.Where(x => x.CountryId == country_id).OrderBy(x => x.CityName).ToList();

            foreach (City city in cities)
            {
                options += "<option value=\"" + city.CityId.ToString() + "\">" + city.CityName + "</option>";
            }

            return options;
        }

        public string GetRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }

        public string GetRoutesByCountryCity(Guid sender_country_id, Guid sender_city_id, Guid destination_country_id, Guid destination_city_id)
        {
            string options = string.Empty;

            List<ShipmentRoute> routes = new List<ShipmentRoute>();

            routes = context.ShipmentRoutes.Where(x => x.SenderCountryId == sender_country_id
                                                    && x.SenderCityId == sender_city_id
                                                    && x.DestinationCountryId == destination_country_id
                                                    && x.DestinationCityId == destination_city_id).ToList();

            foreach (ShipmentRoute route in routes)
            {
                options += "<option value=\"" + route.ShipmentRouteId.ToString() + "\">" + route.ShipmentRouteName + "</option>";
            }

            return options;
        }

        public int GetCurrentLoadForTheCurrentCity(Guid city_id)
        {
            int total_load = 0;

            List<ShipmentStatus> received_statuses = new List<ShipmentStatus>() { ShipmentStatus.Arrived, ShipmentStatus.Delivered};

            total_load = (from a in context.Shipments.ToList()
                          join s in context.ShipmentRoutes.ToList()
                            on a.ShipmentRouteId equals s.ShipmentRouteId
                          join d in context.ShipmentRouteDetails.ToList()
                            on s.ShipmentRouteId equals d.ShipmentRouteId
                          where a.ShipmentRouteIndex == d.ShipmentRouteDetailIndex && received_statuses.Contains(a.ShipmentStatus) && d.CityId == city_id
                          select a
                          ).Count();

            return total_load;
        }
    }
}