using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.Country;
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
    public class ShipmentRouteController : SharedControllerBase
    {
        public ActionResult ShipmentRouteList()
        {
            List<ShipmentRoute> shipment_routes = context.ShipmentRoutes.ToList();
            List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.OrderBy(x => x.ShipmentRouteDetailIndex).ToList();
            List<ShipmentRouteDetail> route_details_perShipment = new List<ShipmentRouteDetail>();

            List<Country> countries = context.Countries.ToList();
            List<City> cities = context.Cities.ToList();

            List<ShipmentRouteViewModel> view_models = new List<ShipmentRouteViewModel>();
            ShipmentRouteViewModel view_model_single = new ShipmentRouteViewModel();

            for (int i = 0; i < shipment_routes.Count;i++ )
            {
                view_model_single = new ShipmentRouteViewModel() {
                    ShipmentRouteId = shipment_routes[i].ShipmentRouteId,
                    ShipmentRouteName = shipment_routes[i].ShipmentRouteName,
                    
                    SenderCountryId = shipment_routes[i].SenderCountryId,
                    SenderCountryName = countries.Where(x => x.CountryId == shipment_routes[i].SenderCountryId).FirstOrDefault().CountryName,

                    SenderCityId = shipment_routes[i].SenderCityId,
                    SenderCityName = cities.Where(x => x.CityId == shipment_routes[i].SenderCityId).FirstOrDefault().CityName,

                    DestinationCountryId = shipment_routes[i].DestinationCountryId,
                    DestinationCountryName = countries.Where(x => x.CountryId == shipment_routes[i].DestinationCountryId).FirstOrDefault().CountryName,

                    DestinationCityId = shipment_routes[i].DestinationCityId,
                    DestinationCityName = cities.Where(x => x.CityId == shipment_routes[i].DestinationCityId).FirstOrDefault().CityName
                };

                route_details_perShipment = new List<ShipmentRouteDetail>();
                route_details_perShipment = route_details.Where(x => x.ShipmentRouteId == shipment_routes[i].ShipmentRouteId).ToList();

                for(int j=0;j<route_details_perShipment.Count;j++)
                {
                    view_model_single.RouteDetails.Add(new ShipmentRouteDetailViewModel()
                    {
                        ShipmentRouteDetailId = route_details_perShipment[j].ShipmentRouteDetailId,
                        ShipmentRouteId = route_details_perShipment[j].ShipmentRouteId,
                        ShipmentouteDetailDayDuration = route_details_perShipment[j].ShipmentouteDetailDayDuration,
                        ShipmentRouteDetailIndex = route_details_perShipment[j].ShipmentRouteDetailIndex,
                        CountryId = route_details_perShipment[j].CountryId,
                        CountryName = countries.Where(x => x.CountryId == route_details_perShipment[j].CountryId).FirstOrDefault().CountryName,
                        CityId = route_details_perShipment[j].CityId,
                        CityName = cities.Where(x => x.CityId == route_details_perShipment[j].CityId).FirstOrDefault().CityName
                    });
                }

                view_models.Add(view_model_single);
            }

            ViewData.Model = view_models;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        private void BindDropDownValues()
        {
            List<Country> countries = new List<Country>();
            List<City> cities = new List<City>();

            countries = context.Countries.ToList();
            cities = context.Cities.ToList();

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
        }

        public ActionResult ShipmentRouteDetailSingle(Guid route_detail_id, int index)
        {
            ShipmentRouteDetailViewModel view_model = new ShipmentRouteDetailViewModel();
            List<Country> countries = context.Countries.ToList();
            List<City> cities = context.Cities.ToList();
            ShipmentRouteDetail route_detail = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteDetailId == route_detail_id).FirstOrDefault();

            BindDropDownValues();

            if (route_detail != null)
            {
                view_model = new ShipmentRouteDetailViewModel()
                {
                    ShipmentRouteDetailId = route_detail.ShipmentRouteDetailId,
                    ShipmentRouteId = route_detail.ShipmentRouteId,
                    ShipmentouteDetailDayDuration = route_detail.ShipmentouteDetailDayDuration,
                    ShipmentRouteDetailIndex = route_detail.ShipmentRouteDetailIndex,
                    CountryId = route_detail.CountryId,
                    CountryName = countries.Where(x => x.CountryId == route_detail.CountryId).FirstOrDefault().CountryName,
                    CityId = route_detail.CityId,
                    CityName = cities.Where(x => x.CityId == route_detail.CityId).FirstOrDefault().CityName
                };
            }
            else
            {
                view_model.ShipmentRouteDetailIndex = index;
            }

            ViewData.Model = view_model;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult ShipmentRouteDetails(Guid shipment_route_id)
        {
            ShipmentRoute shipment_route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == shipment_route_id).FirstOrDefault();
            List<ShipmentRouteDetail> route_details = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteId == shipment_route_id).OrderBy(x => x.ShipmentRouteDetailIndex).ToList();

            List<Country> countries = context.Countries.ToList();
            List<City> cities = context.Cities.ToList();

            ShipmentRouteViewModel view_model = new ShipmentRouteViewModel();

            BindDropDownValues();

            if (shipment_route != null)
            {
                view_model = new ShipmentRouteViewModel() {
                    ShipmentRouteId = shipment_route.ShipmentRouteId,
                    ShipmentRouteName = shipment_route.ShipmentRouteName,
                    
                    SenderCountryId = shipment_route.SenderCountryId,
                    SenderCountryName = countries.Where(x => x.CountryId == shipment_route.SenderCountryId).FirstOrDefault().CountryName,

                    SenderCityId = shipment_route.SenderCityId,
                    SenderCityName = cities.Where(x => x.CityId == shipment_route.SenderCityId).FirstOrDefault().CityName,

                    DestinationCountryId = shipment_route.DestinationCountryId,
                    DestinationCountryName = countries.Where(x => x.CountryId == shipment_route.DestinationCountryId).FirstOrDefault().CountryName,

                    DestinationCityId = shipment_route.DestinationCityId,
                    DestinationCityName = cities.Where(x => x.CityId == shipment_route.DestinationCityId).FirstOrDefault().CityName
                };

                for(int j=0;j<route_details.Count;j++)
                {
                    view_model.RouteDetails.Add(new ShipmentRouteDetailViewModel()
                    {
                        ShipmentRouteDetailId = route_details[j].ShipmentRouteDetailId,
                        ShipmentRouteId = route_details[j].ShipmentRouteId,
                        ShipmentouteDetailDayDuration = route_details[j].ShipmentouteDetailDayDuration,
                        ShipmentRouteDetailIndex = route_details[j].ShipmentRouteDetailIndex,
                        CountryId = route_details[j].CountryId,
                        CountryName = countries.Where(x => x.CountryId == route_details[j].CountryId).FirstOrDefault().CountryName,
                        CityId = route_details[j].CityId,
                        CityName = cities.Where(x => x.CityId == route_details[j].CityId).FirstOrDefault().CityName
                    });
                }
            }
            else
            {

            }

            ViewData.Model = view_model;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public List<string> ValidateRouteDetails(ShipmentRouteViewModel viewModel)
        {
            List<string> error_messages = new List<string>();
            int start_index = 0;
            int end_index = viewModel.RouteDetails.Count - 1;

            if (end_index < 1)
                error_messages.Add("Please specify route details");

            else
            {
                ShipmentRouteDetailViewModel first_detail = viewModel.RouteDetails.OrderBy(x => x.ShipmentRouteDetailIndex).FirstOrDefault();
                ShipmentRouteDetailViewModel last_detail = viewModel.RouteDetails.OrderByDescending(x => x.ShipmentRouteDetailIndex).FirstOrDefault();

                if (context.ShipmentRoutes.Where(x => x.ShipmentRouteId != viewModel.ShipmentRouteId && x.ShipmentRouteName.ToLower() == viewModel.ShipmentRouteName.ToLower()).Any())
                    error_messages.Add("Route with the same name already exists");

                if(viewModel.SenderCountryId != first_detail.CountryId || viewModel.SenderCityId != first_detail.CityId)
                    error_messages.Add("First route must be equal to sender");

                if (viewModel.DestinationCountryId != last_detail.CountryId || viewModel.DestinationCityId != last_detail.CityId)
                    error_messages.Add("Last route must be equal to destination");

                if(viewModel.RouteDetails.Where(x => x.ShipmentRouteDetailIndex != 0 && x.ShipmentRouteDetailIndex != end_index
                                                    && x.CityId == viewModel.SenderCityId).Any())
                {
                    error_messages.Add("Departure point cannot be in the middle of the route");
                }

                if (viewModel.RouteDetails.Where(x => x.ShipmentRouteDetailIndex != 0 && x.ShipmentRouteDetailIndex != end_index
                                                    && x.CityId == viewModel.DestinationCityId).Any())
                {
                    error_messages.Add("Destination point cannot be in the middle of the route");
                }

                foreach(ShipmentRouteDetailViewModel vm in viewModel.RouteDetails)
                {
                    if(vm.ShipmentouteDetailDayDuration < 1 && vm.ShipmentRouteDetailIndex != 0)
                    {
                        error_messages.Add(vm.ShipmentRouteDetailIndex.ToString() + " | Duration cannot be zero or negative");
                    }

                    if (vm.ShipmentouteDetailDayDuration != 0 && vm.ShipmentRouteDetailIndex == 0)
                    {
                        error_messages.Add(vm.ShipmentRouteDetailIndex.ToString() + " | First point duration must be zero");
                    }
                }
            }

            return error_messages;
        }

        public JsonResult ShipmentRouteCreateUpdate(ShipmentRouteViewModel viewModel)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();

            ajaxResponse.ErrorMessages = ValidateRouteDetails(viewModel);

            if (!ajaxResponse.ErrorMessages.Any())
            {
                ShipmentRoute found_route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == viewModel.ShipmentRouteId).FirstOrDefault();

                if (found_route != null)
                {
                    UpdateRoute(viewModel);
                }
                else
                {
                    CreateRoute(viewModel);
                }

                context.SaveChanges();
                ajaxResponse.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
            }
            else
            {
                ajaxResponse.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
            }

            return Json(ajaxResponse);
        }

        private void CreateRoute(ShipmentRouteViewModel viewModel)
        {
            viewModel.ShipmentRouteId = Guid.NewGuid();

            ShipmentRoute route = new ShipmentRoute() {
                ShipmentRouteId = viewModel.ShipmentRouteId,
                ShipmentRouteName = viewModel.ShipmentRouteName,
                SenderCityId = viewModel.SenderCityId,
                SenderCountryId = viewModel.SenderCountryId,
                DestinationCityId = viewModel.DestinationCityId,
                DestinationCountryId = viewModel.DestinationCountryId
            };

            context.ShipmentRoutes.Add(route);

            for(int i = 0;i<viewModel.RouteDetails.Count;i++)
            {
                Guid route_detail_id = viewModel.RouteDetails[i].ShipmentRouteDetailId;
                ShipmentRouteDetail routeDetail = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteDetailId == route_detail_id).FirstOrDefault();

                if(routeDetail != null)
                {
                    routeDetail.ShipmentRouteId = viewModel.ShipmentRouteId;
                    routeDetail.ShipmentRouteDetailId = viewModel.RouteDetails[i].ShipmentRouteDetailId;
                    routeDetail.ShipmentRouteDetailIndex = viewModel.RouteDetails[i].ShipmentRouteDetailIndex;
                    routeDetail.ShipmentouteDetailDayDuration = viewModel.RouteDetails[i].ShipmentouteDetailDayDuration;
                    routeDetail.CountryId = viewModel.RouteDetails[i].CountryId;
                    routeDetail.CityId = viewModel.RouteDetails[i].CityId;
                }
                else
                {
                    routeDetail = new ShipmentRouteDetail();

                    routeDetail.ShipmentRouteId = viewModel.ShipmentRouteId;
                    routeDetail.ShipmentRouteDetailId = Guid.NewGuid();
                    routeDetail.ShipmentRouteDetailIndex = viewModel.RouteDetails[i].ShipmentRouteDetailIndex;
                    routeDetail.ShipmentouteDetailDayDuration = viewModel.RouteDetails[i].ShipmentouteDetailDayDuration;
                    routeDetail.CountryId = viewModel.RouteDetails[i].CountryId;
                    routeDetail.CityId = viewModel.RouteDetails[i].CityId;

                    context.ShipmentRouteDetails.Add(routeDetail);
                }
            }
        }

        private void UpdateRoute(ShipmentRouteViewModel viewModel)
        {
            ShipmentRoute route = context.ShipmentRoutes.Where(x => x.ShipmentRouteId == viewModel.ShipmentRouteId).FirstOrDefault();

            route.ShipmentRouteId = viewModel.ShipmentRouteId;
            route.ShipmentRouteName = viewModel.ShipmentRouteName;
            route.SenderCityId = viewModel.SenderCityId;
            route.SenderCountryId = viewModel.SenderCountryId;
            route.DestinationCityId = viewModel.DestinationCityId;
            route.DestinationCountryId = viewModel.DestinationCountryId;

            for (int i = 0; i < viewModel.RouteDetails.Count; i++)
            {
                Guid route_detail_id = viewModel.RouteDetails[i].ShipmentRouteDetailId;
                ShipmentRouteDetail routeDetail = context.ShipmentRouteDetails.Where(x => x.ShipmentRouteDetailId == route_detail_id).FirstOrDefault();

                if (routeDetail != null)
                {
                    routeDetail.ShipmentRouteId = viewModel.ShipmentRouteId;
                    routeDetail.ShipmentRouteDetailId = viewModel.RouteDetails[i].ShipmentRouteDetailId;
                    routeDetail.ShipmentRouteDetailIndex = viewModel.RouteDetails[i].ShipmentRouteDetailIndex;
                    routeDetail.ShipmentouteDetailDayDuration = viewModel.RouteDetails[i].ShipmentouteDetailDayDuration;
                    routeDetail.CountryId = viewModel.RouteDetails[i].CountryId;
                    routeDetail.CityId = viewModel.RouteDetails[i].CityId;
                }
                else
                {
                    routeDetail = new ShipmentRouteDetail();

                    routeDetail.ShipmentRouteId = viewModel.ShipmentRouteId;
                    routeDetail.ShipmentRouteDetailId = Guid.NewGuid();
                    routeDetail.ShipmentRouteDetailIndex = viewModel.RouteDetails[i].ShipmentRouteDetailIndex;
                    routeDetail.ShipmentouteDetailDayDuration = viewModel.RouteDetails[i].ShipmentouteDetailDayDuration;
                    routeDetail.CountryId = viewModel.RouteDetails[i].CountryId;
                    routeDetail.CityId = viewModel.RouteDetails[i].CityId;

                    context.ShipmentRouteDetails.Add(routeDetail);
                }
            }
        }
    }
}
