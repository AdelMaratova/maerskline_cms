using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models.System;
using ContainerManagementSystem.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainerManagementSystem.Controllers
{
    public class LoginController : SharedControllerBase
    {
        #region "Views"

        public ActionResult LoginPage()
        {
            if (MySession.CurrentSession != null)
            {
                MySession.CurrentSession.UserId = Guid.Empty;
                MySession.CurrentSession.LoginDate = new DateTime(1990, 1, 1);
            }
            
            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult RegisterUser()
        {
            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        #endregion 

        #region "Actions"

        [HttpPost]
        public JsonResult LoginUser(User input_user)
        {
            AjaxResponse returnResponse = new AjaxResponse();

            input_user = context.Users.ToList().Where(x => x.Email.ToLower() == input_user.Email.ToLower() && x.Password == input_user.Password).FirstOrDefault();

            if(input_user != null)
            {
                MySession.CurrentSession = new MySession();
                returnResponse.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;

                MySession.CurrentSession.UserId = input_user.UserId;
                MySession.CurrentSession.LoginDate = DateTime.Now;
                MySession.CurrentSession.UserType = input_user.UserType;

            }
            else
            {
                input_user = new Models.User.User();
                returnResponse.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
                returnResponse.ErrorMessages.Add("Invalid user credentials");
            }

            return Json(new { returnResponse = returnResponse, UserType = input_user.UserType.ToString()});
        }

        #endregion
    }
}
