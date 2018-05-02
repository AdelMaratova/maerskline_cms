using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.Country;
using ContainerManagementSystem.Models.Shipment;
using ContainerManagementSystem.Models.ShipmentRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContainerManagementSystem.Models.System;
using ContainerManagementSystem.CommonEnum;
using ContainerManagementSystem.Models.ShippingChannel;
using ContainerManagementSystem.Models.User;

namespace ContainerManagementSystem.Controllers
{
    public class ShipmentController : SharedControllerBase
    {
        private List<string> ValidateShipmentForSave(Shipment shipment)
        {
            List<string> error_messages = new List<string>();

            if (shipment.CustomerId == null || shipment.CustomerId == Guid.Empty)
                error_messages.Add("Please select a customer");

            if (shipment.SenderCountryId == null || shipment.SenderCountryId == Guid.Empty)
                error_messages.Add("Please select departure country");

            if (shipment.SenderCityId == null || shipment.SenderCityId == Guid.Empty)
                error_messages.Add("Please select departure city");

            if (shipment.DestinationCityId == null || shipment.DestinationCityId == Guid.Empty)
                error_messages.Add("Please select destination city");

            if (shipment.DestinationCountryId == null || shipment.DestinationCountryId == Guid.Empty)
                error_messages.Add("Please select destination country");

            if (shipment.ShippingMethod == null || shipment.ShippingMethod == Guid.Empty)
                error_messages.Add("Please select shipping method");

            if (shipment.ShipmentRouteId == null || shipment.ShipmentRouteId == Guid.Empty)
                error_messages.Add("Please select shipment route");

            return error_messages;
        }

        public JsonResult RejectShipment(Guid shipment_id)
        {
            AjaxResponse returnResponse = new AjaxResponse();


            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            //Update
            if (found_shipment != null)
            {
                found_shipment.ShipmentStatus = ShipmentStatus.Cancelled;
            }
            
            context.SaveChanges();

            returnResponse.ReturnStatus = AjaxReturnStatus.Success;
            returnResponse.ItemId = found_shipment.ShipmentId;

            return Json(returnResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ApproveShipment(Guid shipment_id)
        {
            AjaxResponse returnResponse = new AjaxResponse();


            Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            //Update
            if (found_shipment != null)
            {
                found_shipment.ShipmentStatus = ShipmentStatus.New;
            }

            context.SaveChanges();

            returnResponse.ReturnStatus = AjaxReturnStatus.Success;
            returnResponse.ItemId = found_shipment.ShipmentId;

            return Json(returnResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShipmentRequestCreateUpdate(Shipment shipment)
        {
            AjaxResponse returnResponse = new AjaxResponse();

            returnResponse.ErrorMessages = ValidateShipmentForSave(shipment);

            if (!returnResponse.ErrorMessages.Any())
            {
                Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment.ShipmentId).FirstOrDefault();

                //Update
                if (found_shipment != null)
                {
                    found_shipment.ShipmentId = shipment.ShipmentId;
                    found_shipment.ShipmentNo = shipment.ShipmentNo;
                    found_shipment.ShipmentStatus = shipment.ShipmentStatus;
                    found_shipment.ShipmentCurrentCityId = shipment.ShipmentCurrentCityId;
                    found_shipment.ShippingMethod = shipment.ShippingMethod;
                    found_shipment.SenderCountryId = shipment.SenderCountryId;
                    found_shipment.SenderCityId = shipment.SenderCityId;
                    found_shipment.DestinationCountryId = shipment.DestinationCountryId;
                    found_shipment.DestinationCityId = shipment.DestinationCityId;
                    found_shipment.ShipmentRouteId = shipment.ShipmentRouteId;
                    found_shipment.ShipmentRouteIndex = shipment.ShipmentRouteIndex;
                }
                //Create
                else
                {
                    found_shipment = new Shipment();

                    found_shipment.ShipmentId = Guid.NewGuid();
                    found_shipment.ShipmentNo = GetRandomString(10);
                    found_shipment.ShipmentStatus = ShipmentStatus.Pending;
                    found_shipment.ShipmentCurrentCityId = shipment.SenderCityId;
                    found_shipment.ShippingMethod = shipment.ShippingMethod;
                    found_shipment.SenderCountryId = shipment.SenderCountryId;
                    found_shipment.SenderCityId = shipment.SenderCityId;
                    found_shipment.DestinationCountryId = shipment.DestinationCountryId;
                    found_shipment.DestinationCityId = shipment.DestinationCityId;
                    found_shipment.ShipmentRouteId = shipment.ShipmentRouteId;
                    found_shipment.ShipmentRouteIndex = 0;

                    context.Shipments.Add(found_shipment);
                }

                context.SaveChanges();

                returnResponse.ReturnStatus = AjaxReturnStatus.Success;
                returnResponse.ItemId = found_shipment.ShipmentId;
            }
            else
            {
                returnResponse.ReturnStatus = AjaxReturnStatus.Error;
            }

            return Json(returnResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShipmentCreateUpdate(Shipment shipment)
        {
            AjaxResponse returnResponse = new AjaxResponse();

            returnResponse.ErrorMessages = ValidateShipmentForSave(shipment);

            if(!returnResponse.ErrorMessages.Any())
            {
                Shipment found_shipment = context.Shipments.Where(x => x.ShipmentId == shipment.ShipmentId).FirstOrDefault();

                //Update
                if (found_shipment != null)
                {
                    found_shipment.ShipmentId = shipment.ShipmentId;
                    found_shipment.ShipmentNo = shipment.ShipmentNo;
                    found_shipment.ShipmentStatus = shipment.ShipmentStatus;
                    found_shipment.ShipmentCurrentCityId = shipment.ShipmentCurrentCityId;
                    found_shipment.ShippingMethod = shipment.ShippingMethod;
                    found_shipment.SenderCountryId = shipment.SenderCountryId;
                    found_shipment.SenderCityId = shipment.SenderCityId;
                    found_shipment.DestinationCountryId = shipment.DestinationCountryId;
                    found_shipment.DestinationCityId = shipment.DestinationCityId;
                    found_shipment.ShipmentRouteId = shipment.ShipmentRouteId;
                    found_shipment.ShipmentRouteIndex = shipment.ShipmentRouteIndex;
                }
                //Create
                else
                {
                    found_shipment = new Shipment();

                    found_shipment.ShipmentId = Guid.NewGuid();
                    found_shipment.ShipmentNo = GetRandomString(10);
                    found_shipment.ShipmentStatus = ShipmentStatus.New;
                    found_shipment.ShipmentCurrentCityId = shipment.SenderCityId;
                    found_shipment.ShippingMethod = shipment.ShippingMethod;
                    found_shipment.SenderCountryId = shipment.SenderCountryId;
                    found_shipment.SenderCityId = shipment.SenderCityId;
                    found_shipment.DestinationCountryId = shipment.DestinationCountryId;
                    found_shipment.DestinationCityId = shipment.DestinationCityId;
                    found_shipment.ShipmentRouteId = shipment.ShipmentRouteId;
                    found_shipment.ShipmentRouteIndex = 0;

                    context.Shipments.Add(found_shipment);
                }

                context.SaveChanges();

                returnResponse.ReturnStatus = AjaxReturnStatus.Success;
            }
            else
            {
                returnResponse.ReturnStatus = AjaxReturnStatus.Error;
            }

            return Json(returnResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShipmentList()
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

        public ActionResult ShipmentRequest(Guid shipment_id)
        {
            Shipment shipment = new Shipment();
            ShipmentViewModel viewModel = new ShipmentViewModel();
            BindDropDownValues();

            shipment.CustomerId = MySession.CurrentSession.UserId;

            //shipment = context.Shipments.ToList().Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            //if (shipment != null)
            //{
            //    viewModel = BindValues(shipment);
            //}
            //else
            //    viewModel = new ShipmentViewModel();

            ViewData.Model = viewModel;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult ShipmentDetails(Guid shipment_id)
        {
            Shipment shipment = new Shipment();
            ShipmentViewModel viewModel = new ShipmentViewModel();
            BindDropDownValues();

            shipment = context.Shipments.ToList().Where(x => x.ShipmentId == shipment_id).FirstOrDefault();

            if(shipment != null)
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

            countries = context.Countries.ToList();
            cities = context.Cities.ToList();

            viewModel.ShipmentId = shipment.ShipmentId;
            viewModel.ShipmentNo = shipment.ShipmentNo;
            viewModel.ShipmentStatus = shipment.ShipmentStatus;
            viewModel.ShipmentCurrentCityId = shipment.ShipmentCurrentCityId;
            viewModel.ShipmentCurrentCity = cities.Where(x => x.CityId == shipment.ShipmentCurrentCityId).FirstOrDefault().CityName;
            viewModel.ShippingMethod = shipment.ShippingMethod;
            viewModel.SenderCountryId = shipment.SenderCountryId;
            viewModel.SenderCountry = countries.Where(x => x.CountryId == shipment.SenderCountryId).FirstOrDefault().CountryName;
            viewModel.SenderCityId = shipment.SenderCityId;
            viewModel.SenderCity = cities.Where(x => x.CityId == shipment.SenderCityId).FirstOrDefault().CityName;
            viewModel.DestinationCountryId = shipment.DestinationCountryId;
            viewModel.DestinationCountry = countries.Where(x => x.CountryId == shipment.DestinationCountryId).FirstOrDefault().CountryName;
            viewModel.DestinationCityId = shipment.DestinationCityId;
            viewModel.DestinationCity = cities.Where(x => x.CityId == shipment.DestinationCityId).FirstOrDefault().CityName;
            viewModel.ShipmentRouteId = shipment.ShipmentRouteId;
            viewModel.ShipmentRouteIndex = shipment.ShipmentRouteIndex;
            viewModel.CustomerId = shipment.CustomerId;

            return viewModel;
        }

        private void BindDropDownValues()
        {
            List<Country> countries = new List<Country>();
            List<City> cities = new List<City>();
            List<ShipmentRoute> routes = new List<ShipmentRoute>();
            List<ShippingChannel> channels = new List<ShippingChannel>();
            List<User> customers = new List<User>();

            countries = context.Countries.ToList();
            cities = context.Cities.ToList();
            //routes = context.ShipmentRoutes.ToList();
            channels = context.ShippingChannels.ToList();
            customers = context.Users.Where(x => x.UserType == UserType.Customer).ToList();

            List<SelectListItem> listItems = new List<SelectListItem>();
            SelectListItem listItem = new SelectListItem();


            listItems = new List<SelectListItem>();
            foreach (User customer in customers)
            {
                listItem = new SelectListItem();

                listItem.Text = customer.FirstName + " | " + customer.LastName;
                listItem.Value = customer.UserId.ToString();

                listItems.Add(listItem);
            }

            ViewData["Customers"] = listItems;


            listItems = new List<SelectListItem>();
            foreach (ShippingChannel channel in channels)
            {
                listItem = new SelectListItem();

                listItem.Text = channel.ShippingChannelName;
                listItem.Value = channel.ShippingChannelId.ToString();

                listItems.Add(listItem);
            }

            ViewData["ShippingChannels"] = listItems;


            listItems = new List<SelectListItem>();
            foreach(Country country in countries)
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
