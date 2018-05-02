using ContainerManagementSystem.CommonEnum;
using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.Country;
using ContainerManagementSystem.Models.Shipment;
using ContainerManagementSystem.Models.ShipmentHistory;
using ContainerManagementSystem.Models.ShipmentRoute;
using ContainerManagementSystem.Models.ShipmentRouteDetail;
using ContainerManagementSystem.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainerManagementSystem.Controllers
{
    public class ShipmentHistoryController : SharedControllerBase
    {
        //
        // GET: /ShipmentHistorty/

        public ActionResult ViewShipmentTracking()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult GetTrackingDetails(string shipment_no)
        {
            List<ShipmentViewModel> viewModel = new List<ShipmentViewModel>();
            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentNo.ToLower() == shipment_no.ToLower()).FirstOrDefault();

            if (found_shipment != null)
            {
                List<Country> countries = new List<Country>();
                List<City> cities = new List<City>();

                cities = context.Cities.ToList();
                countries = context.Countries.ToList();

                List<ShipmentHistory> histories = context.ShipmentHistory.Where(x => x.ShipmentId == found_shipment.ShipmentId).OrderByDescending(x => x.UpdatedDate).ToList();

                for(int i=0;i<histories.Count;i++)
                {
                    viewModel.Add(new ShipmentViewModel() {
                        ShipmentId = histories[i].ShipmentId,
                        ShipmentNo = found_shipment.ShipmentNo,
                        ShipmentStatus = histories[i].ShipmentStatus,
                        SenderCountryId = histories[i].DispatchedCountry,
                        SenderCountry = countries.Where(x => x.CountryId == histories[i].DispatchedCountry).FirstOrDefault().CountryName,
                        SenderCityId = histories[i].DispatchedCity,
                        SenderCity = cities.Where(x => x.CityId == histories[i].DispatchedCity).FirstOrDefault().CityName,
                        DestinationCountryId = histories[i].ReceivedCountry.GetValueOrDefault(),
                        DestinationCountry = histories[i].ReceivedCountry == null ? string.Empty : countries.Where(x => x.CountryId == histories[i].ReceivedCountry.GetValueOrDefault()).FirstOrDefault().CountryName,
                        DestinationCityId = histories[i].ReceivedCity.GetValueOrDefault(),
                        DestinationCity = histories[i].ReceivedCity == null ? string.Empty : cities.Where(x => x.CityId == histories[i].ReceivedCity.GetValueOrDefault()).FirstOrDefault().CityName,
                        ShippingDate = histories[i].UpdatedDate
                    });
                }
            }

            ViewData.Model = viewModel;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public JsonResult CheckShipmentExistance(string shipment_no)
        {
            AjaxResponse returnModel = new AjaxResponse();
            bool isFound = context.Shipments.Where(x => x.ShipmentNo.ToLower() == shipment_no.ToLower()).Any();

            if(isFound)
            {
                returnModel.ReturnStatus = AjaxReturnStatus.Success;
            }
            else
            {
                returnModel.ReturnStatus = AjaxReturnStatus.Error;
            }

            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        private bool ValidateCityCapacityForRecieve(Guid city_id)
        {
            bool isValid = false;

            City found_city = context.Cities.Where(x => x.CityId == city_id).FirstOrDefault();

            if (found_city != null)
            {
                if (found_city.YardCapacity > GetCurrentLoadForTheCurrentCity(city_id))
                    isValid = true;
            }

            return isValid;
        }

        public JsonResult ReceiveShipment(Guid shipment_id)
        {
            AjaxResponse returnModel = new AjaxResponse();

            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if (found_shipment != null)
            {
                ShipmentRoute route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == found_shipment.ShipmentRouteId).FirstOrDefault();
                List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == route.ShipmentRouteId).ToList();

                ShipmentRouteDetail previous_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex).FirstOrDefault();

                found_shipment.ShipmentRouteIndex ++;

                ShipmentRouteDetail current_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex).FirstOrDefault();

                if(ValidateCityCapacityForRecieve(current_route.CityId))
                {
                    if (found_shipment.ShipmentRouteIndex == route_details.Count() - 1)
                    {
                        found_shipment.ShipmentStatus = ShipmentStatus.Delivered;
                    }
                    else
                    {
                        found_shipment.ShipmentStatus = ShipmentStatus.Arrived;
                    }

                    found_shipment.ShipmentCurrentCityId = current_route.CityId;

                    ShipmentHistory history = new ShipmentHistory()
                    {
                        ShipmentHistoryId = Guid.NewGuid(),
                        ShipmentId = found_shipment.ShipmentId,
                        ShipmentStatus = found_shipment.ShipmentStatus,
                        DispatchedCountry = previous_route.CountryId,
                        DispatchedCity = previous_route.CityId,
                        ReceivedCountry = current_route.CountryId,
                        ReceivedCity = current_route.CityId,
                        UpdatedDate = DateTime.Now
                    };

                    context.ShipmentHistory.Add(history);

                    context.SaveChanges();

                    returnModel.ReturnStatus = AjaxReturnStatus.Success;
                }
                else
                {
                    returnModel.ReturnStatus = AjaxReturnStatus.Error;
                    returnModel.ErrorMessages.Add("The city yard is full, please dispatch other shipments before receiving this one.");
                }
            }

            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        private List<string> ValidateShipmentForDispatching(Guid shipment_id)
        {
            List<string> error_messages = new List<string>();

            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if(found_shipment != null)
            {
                List<ShipmentStatus> active_statuses = new List<ShipmentStatus>()
                {
                    ShipmentStatus.Arrived,
                    ShipmentStatus.Delivered,
                    ShipmentStatus.Dispatched
                };

                if(context.Shipments.Where(x => x.ShipmentId != found_shipment.ShipmentId
                                            && x.ShippingMethod == found_shipment.ShippingMethod
                                            && active_statuses.Contains(x.ShipmentStatus)).Any())
                {
                    error_messages.Add("The current shipment vessel is busy, please free it before use");
                }
            }

            return error_messages;
        }

        public JsonResult DispatchItem (Guid shipment_id)
        {
            AjaxResponse returnModel = new AjaxResponse();

            returnModel.ErrorMessages = ValidateShipmentForDispatching(shipment_id);

            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if (found_shipment != null && !returnModel.ErrorMessages.Any())
            {
                found_shipment.ShipmentStatus = ShipmentStatus.Dispatched;

                ShipmentRoute route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == found_shipment.ShipmentRouteId).FirstOrDefault();
                List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == route.ShipmentRouteId).ToList();

                ShipmentRouteDetail previous_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex).FirstOrDefault();

                ShipmentRouteDetail next_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex + 1).FirstOrDefault();

                found_shipment.ShipmentStatus = ShipmentStatus.Dispatched;

                ShipmentHistory history = new ShipmentHistory()
                {
                    ShipmentHistoryId = Guid.NewGuid(),
                    ShipmentId = found_shipment.ShipmentId,
                    ShipmentStatus = found_shipment.ShipmentStatus,
                    DispatchedCountry = previous_route.CountryId,
                    DispatchedCity = previous_route.CityId,
                    ReceivedCountry = next_route.CountryId,
                    ReceivedCity = next_route.CityId,
                    UpdatedDate = DateTime.Now
                };

                context.ShipmentHistory.Add(history);

                context.SaveChanges();

                returnModel.ReturnStatus = AjaxReturnStatus.Success;
            }
            else
            {
                returnModel.ReturnStatus = AjaxReturnStatus.Error;
            }

            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendOutForDeliveryShipment(Guid shipment_id)
        {
            AjaxResponse returnModel = new AjaxResponse();

            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if (found_shipment != null)
            {
                ShipmentRoute route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == found_shipment.ShipmentRouteId).FirstOrDefault();
                List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == route.ShipmentRouteId).ToList();

                ShipmentRouteDetail previous_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex).FirstOrDefault();

                //found_shipment.ShipmentRouteIndex++;

                ShipmentRouteDetail current_route = route_details.Where(x => x.ShipmentRouteDetailIndex == found_shipment.ShipmentRouteIndex).FirstOrDefault();

                found_shipment.ShipmentStatus = ShipmentStatus.OutForDelivery;

                found_shipment.ShipmentCurrentCityId = current_route.CityId;

                ShipmentHistory history = new ShipmentHistory()
                {
                    ShipmentHistoryId = Guid.NewGuid(),
                    ShipmentId = found_shipment.ShipmentId,
                    ShipmentStatus = found_shipment.ShipmentStatus,
                    DispatchedCountry = previous_route.CountryId,
                    DispatchedCity = previous_route.CityId,
                    ReceivedCountry = current_route.CountryId,
                    ReceivedCity = current_route.CityId,
                    UpdatedDate = DateTime.Now
                };

                context.ShipmentHistory.Add(history);

                context.SaveChanges();

                returnModel.ReturnStatus = AjaxReturnStatus.Success;
            }

            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewShipmentHistory()
        {
            List<ShipmentViewModel> viewModel = new List<ShipmentViewModel>();
            List<Shipment> shipments = new List<Shipment>();
            List<Country> countries = new List<Country>();
            List<City> cities = new List<City>();

            shipments = context.Shipments.ToList();
            countries = context.Countries.ToList();
            cities = context.Cities.ToList();

            foreach (Shipment shipment in shipments)
            {
                viewModel.Add(new ShipmentViewModel()
                {
                    ShipmentId = shipment.ShipmentId,
                    ShipmentNo = shipment.ShipmentNo,
                    ShipmentStatus = shipment.ShipmentStatus,

                    ShipmentCurrentCityId = shipment.ShipmentCurrentCityId,
                    ShipmentCurrentCity = cities.Where(x => x.CityId == shipment.ShipmentCurrentCityId).FirstOrDefault().CityName,

                    ShippingMethod = shipment.ShippingMethod,

                    SenderCountryId = shipment.SenderCountryId,
                    SenderCountry = countries.Where(x => x.CountryId == shipment.SenderCountryId).FirstOrDefault().CountryName,

                    SenderCityId = shipment.SenderCityId,
                    SenderCity = cities.Where(x => x.CityId == shipment.SenderCityId).FirstOrDefault().CityName,

                    DestinationCountryId = shipment.SenderCountryId,
                    DestinationCountry = countries.Where(x => x.CountryId == shipment.DestinationCountryId).FirstOrDefault().CountryName,

                    DestinationCityId = shipment.SenderCityId,
                    DestinationCity = cities.Where(x => x.CityId == shipment.DestinationCityId).FirstOrDefault().CityName,

                    ShipmentRouteId = shipment.ShipmentRouteId,
                    ShipmentRouteIndex = shipment.ShipmentRouteIndex
                });
            }

            ViewData.Model = viewModel;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult ShipmentUpdate(Guid shipment_id)
        {
            Shipment shipment = new Shipment();
            ShipmentViewModel viewModel = new ShipmentViewModel();
            BindDropDownValues();

            shipment = context.Shipments.ToList().Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if (shipment != null)
            {
                viewModel = BindValues(shipment);
            }
            else
                viewModel = new ShipmentViewModel();

            ViewData.Model = viewModel;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        private ShipmentViewModel BindValues(Shipment shipment)
        {
            ShipmentViewModel viewModel = new ShipmentViewModel();

            List<Country> countries = new List<Country>();
            List<City> cities = new List<City>();
            ShipmentRoute route = new ShipmentRoute();
            List<ShipmentRouteDetail> route_details = new List<ShipmentRouteDetail>();
            ShipmentRouteDetail current_route_point = new ShipmentRouteDetail();
            ShipmentRouteDetail next_route_point = new ShipmentRouteDetail();

            countries = context.Countries.ToList();
            cities = context.Cities.ToList();
            route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == shipment.ShipmentRouteId).FirstOrDefault();
            route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == shipment.ShipmentRouteId).ToList();

            if(shipment.ShipmentRouteIndex == 0)
            {
                current_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == shipment.ShipmentRouteIndex).FirstOrDefault();
                next_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == (shipment.ShipmentRouteIndex + 1)).FirstOrDefault();

                viewModel.ShipmentNextCityId = next_route_point.CityId;
                viewModel.ShipmentNextCity = cities.Where(x => x.CityId == next_route_point.CityId).FirstOrDefault().CityName;
            }
            else if (shipment.ShipmentRouteIndex == (route_details.Count() - 1))
            {
                current_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == shipment.ShipmentRouteIndex).FirstOrDefault();
                //next_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == (shipment.ShipmentRouteIndex + 1)).FirstOrDefault();
            }
            else
            {
                current_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == shipment.ShipmentRouteIndex).FirstOrDefault();
                next_route_point = route_details.Where(x => x.ShipmentRouteDetailIndex == (shipment.ShipmentRouteIndex + 1)).FirstOrDefault();

                viewModel.ShipmentNextCityId = next_route_point.CityId;
                viewModel.ShipmentNextCity = cities.Where(x => x.CityId == next_route_point.CityId).FirstOrDefault().CityName;

                //    public Guid ShipmentNextCityId { get; set; }
                //public string ShipmentNextCity { get; set; }   
            }

            viewModel.ShipmentId = shipment.ShipmentId;
            viewModel.ShipmentNo = shipment.ShipmentNo;
            viewModel.ShipmentStatus = shipment.ShipmentStatus;
            viewModel.ShipmentCurrentCityId = shipment.ShipmentCurrentCityId;

            viewModel.SenderCountryId = shipment.SenderCountryId;
            viewModel.SenderCountry = countries.Where(x => x.CountryId == shipment.SenderCountryId).FirstOrDefault().CountryName;
            viewModel.SenderCityId = shipment.SenderCityId;
            viewModel.SenderCity = cities.Where(x => x.CityId == shipment.SenderCityId).FirstOrDefault().CityName;
            viewModel.DestinationCountryId = shipment.DestinationCountryId;
            viewModel.DestinationCountry = countries.Where(x => x.CountryId == shipment.DestinationCountryId).FirstOrDefault().CountryName;
            viewModel.DestinationCityId = shipment.DestinationCityId;
            viewModel.DestinationCity = cities.Where(x => x.CityId == shipment.DestinationCityId).FirstOrDefault().CityName;

            viewModel.ShipmentCurrentCity = cities.Where(x => x.CityId == shipment.ShipmentCurrentCityId).FirstOrDefault().CityName;
            viewModel.ShippingMethod = shipment.ShippingMethod;
            viewModel.ShipmentRouteId = shipment.ShipmentRouteId;
            viewModel.ShipmentRouteIndex = shipment.ShipmentRouteIndex;

            return viewModel;
        }

        private void BindDropDownValues()
        {
            List<Country> countries = new List<Country>();
            List<City> cities = new List<City>();
            List<ShipmentRoute> routes = new List<ShipmentRoute>();

            countries = context.Countries.ToList();
            cities = context.Cities.ToList();
            //routes = context.ShipmentRoutes.ToList();

            List<SelectListItem> listItems = new List<SelectListItem>();
            SelectListItem listItem = new SelectListItem();


            listItems = new List<SelectListItem>();
            foreach (Country country in countries)
            {
                listItem = new SelectListItem();

                listItem.Text = country.CountryName;
                listItem.Value = country.CountryId.ToString();

                listItems.Add(listItem);
            }

            ViewData["CountryList"] = listItems;


            listItems = new List<SelectListItem>();
            foreach (City city in cities)
            {
                listItem = new SelectListItem();

                listItem.Text = city.CityName;
                listItem.Value = city.CityId.ToString();

                listItems.Add(listItem);
            }

            ViewData["CityList"] = listItems;


            listItems = new List<SelectListItem>();
            foreach (ShipmentRoute route in routes)
            {
                listItem = new SelectListItem();

                listItem.Text = route.ShipmentRouteName;
                listItem.Value = route.ShipmentRouteId.ToString();

                listItems.Add(listItem);
            }

            ViewData["RouteList"] = listItems;
        }
    }
}
