using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContainerManagementSystem.Models.User;

namespace ContainerManagementSystem.Controllers
{
    public class CustomerController : SharedControllerBase
    {
        //
        // GET: /User/

        public ActionResult CustomerList()
        {
            List<User> customers = context.Users.Where(x => x.UserType == CommonEnum.UserType.Customer).ToList();

            ViewData.Model = customers;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult EditCustomer(Guid user_id)
        {
            User user = context.Users.ToList().Where(x => x.UserId == user_id && x.UserType == CommonEnum.UserType.Customer).FirstOrDefault();

            if (user == null)
                user = new Models.User.User();

            ViewData.Model = user;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        private List<string> ValidateCustomerForDelete(Guid user_id)
        {
            List<string> error_messages = new List<string>();

            if(context.Shipments.Where(x => x.CustomerId == user_id).Any())
            {
                error_messages.Add("The customer cannot be deleted because he/she has shipments in the system");
            }

            return error_messages;
        }

        public JsonResult DeleteCustomer(Guid user_id)
        {
            AjaxResponse returnModel = new AjaxResponse();

            returnModel.ErrorMessages = ValidateCustomerForDelete(user_id);

            if (!returnModel.ErrorMessages.Any())
            {
                User found_customer = context.Users.Where(x => x.UserId == user_id && x.UserType == CommonEnum.UserType.Customer).FirstOrDefault();

                if (found_customer != null)
                {
                    context.Users.Remove(found_customer);
                }

                returnModel.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
            }
            else
            {
                returnModel.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
            }

            return Json(returnModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CustomerCreateUpdate(User user)
        {
            User found_customer = context.Users.Where(x => x.UserId == user.UserId && x.UserType == CommonEnum.UserType.Customer).FirstOrDefault();
            AjaxResponse ajax_response = new AjaxResponse();

            if(found_customer != null)
            {
                if (!context.Users.Where(x => x.Email.ToLower() == user.Email.ToLower() && x.UserId != user.UserId).Any())
                { 
                    found_customer.FirstName = user.FirstName;
                    found_customer.LastName = user.LastName;
                    found_customer.Email = user.Email;
                    found_customer.Password = user.Password;

                    ajax_response.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
                }
                else
                {
                    ajax_response.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
                    ajax_response.ErrorMessages.Add("User with the same email exists");
                }
        }
            else
            {
                if(!context.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).Any())
                {
                    found_customer = new User();

                    found_customer.UserId = Guid.NewGuid();
                    found_customer.FirstName = user.FirstName;
                    found_customer.LastName = user.LastName;
                    found_customer.Email = user.Email;
                    found_customer.Password = user.Password;
                    found_customer.UserType = CommonEnum.UserType.Customer;

                    context.Users.Add(found_customer);

                    ajax_response.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
                }
                else
                {
                    ajax_response.ErrorMessages.Add("User with the same email exists");
                    ajax_response.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
                }
            }
            
            ajax_response.ItemId = found_customer.UserId;
            context.SaveChanges();

            return Json(ajax_response, JsonRequestBehavior.AllowGet);
        }
    }
}
