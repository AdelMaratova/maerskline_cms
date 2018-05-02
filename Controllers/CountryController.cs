using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContainerManagementSystem.Models.Country;
using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models.System;

namespace ContainerManagementSystem.Controllers
{
    public class CountryController : SharedControllerBase
    {
        //
        // GET: /Country/

        public ActionResult CountryList()
        {
            List<Country> countries = new List<Country>();

            countries = context.Countries.ToList();

            ViewData.Model = countries;
            
            if (Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        public ActionResult CountryDetails(Guid country_id)
        {
            Country country = context.Countries.ToList().Where(x => x.CountryId == country_id).FirstOrDefault();

            if (country == null)
                country = new Country();

            ViewData.Model = country;

            if (Request.IsAjaxRequest())
                return PartialView();
            return View();
        }

        [HttpPost]
        public JsonResult CountriesCreateUpdate(List<Country> countries)
        {
            AjaxResponse returnResponse = new AjaxResponse();

            if(countries != null && countries.Any())
            {
                foreach(Country country in countries)
                {
                    Country countryFound = context.Countries.ToList().Where(x => x.CountryId == country.CountryId).FirstOrDefault();

                    if(countryFound != null)
                    {
                        countryFound.CountryName = country.CountryName;
                    }
                    else
                    {
                        countryFound = new Country();
                        countryFound.CountryName = country.CountryName;
                        countryFound.CountryId = country.CountryId;

                        context.Countries.Add(countryFound);
                    }
                }

                context.SaveChanges();
            }

            return Json(returnResponse);
        }
    }
}
