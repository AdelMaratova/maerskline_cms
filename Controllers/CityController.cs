using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models;
using ContainerManagementSystem.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainerManagementSystem.Controllers
{
    public class CityController : SharedControllerBase
    {
        //
        // GET: /City/

        public ActionResult CreateUpdateCities(Guid country_id)
        {
            List<City> cities_for_coutnry = context.Cities.ToList().Where(x => x.CountryId == country_id).ToList();

            ViewData.Model = cities_for_coutnry;
            ViewData["country_id"] = country_id.ToString();

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        [HttpPost]
        public JsonResult CitiesCreateUpdate(List<City> cities)
        {
            AjaxResponse returnResponse = new AjaxResponse();

            if (cities != null && cities.Any())
            {
                foreach (City city in cities)
                {
                    City cityFound = context.Cities.ToList().Where(x => x.CityId == city.CityId).FirstOrDefault();

                    if (cityFound != null)
                    {
                        cityFound.CityName = city.CityName;
                    }
                    else
                    {
                        cityFound = new City();
                        cityFound.CityName = city.CityName;
                        cityFound.CountryId = city.CountryId;
                        cityFound.CityId = city.CityId;

                        context.Cities.Add(cityFound);
                    }
                }

                context.SaveChanges();
            }

            return Json(returnResponse);
        }
    }
}
