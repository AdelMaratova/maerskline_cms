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
    public class UserController : SharedControllerBase
    {
        //
        // GET: /User/

        public ActionResult AgentList()
        {
            List<User> agents = context.Users.Where(x => x.UserType == CommonEnum.UserType.Agent).ToList();

            ViewData.Model = agents;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult EditUser(Guid user_id)
        {
            User user = context.Users.ToList().Where(x => x.UserId == user_id).FirstOrDefault();

            if (user == null)
                user = new Models.User.User();

            ViewData.Model = user;

            if (Request.IsAjaxRequest())
                return PartialView("/Views/User/MyProfile.cshtml");

            return View("/Views/User/MyProfile.cshtml");
        }

        public ActionResult MyProfile()
        {
            Guid current_user_id = MySession.CurrentSession.UserId;

            User current_user = context.Users.ToList().Where(x => x.UserId == current_user_id).FirstOrDefault();

            ViewData.Model = current_user;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public JsonResult Deleteuser(Guid user_id)
        {
            AjaxResponse returnModel = new AjaxResponse();

            User found_user = context.Users.Where(x => x.UserId == user_id).FirstOrDefault();

            if (found_user != null)
            {
                context.Users.Remove(found_user);
            }

            return Json(returnModel);
        }

        public JsonResult GetUserInfo()
        {
            Guid current_user_id = Guid.Empty;
            if(MySession.CurrentSession != null)
            {
                current_user_id = MySession.CurrentSession.UserId;
            }
            else
            {
                current_user_id = Guid.Empty;
            }

            User current_user = context.Users.ToList().Where(x => x.UserId == current_user_id).FirstOrDefault();

            return Json(current_user, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UserCreateUpdate(User user)
        {
            User found_user = context.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            AjaxResponse res = new AjaxResponse();


            if(found_user != null)
            {
                if (!context.Users.Where(x => x.Email.ToLower() == user.Email.ToLower() && x.UserId != user.UserId).Any())
                {
                    found_user.FirstName = user.FirstName;
                    found_user.LastName = user.LastName;
                    found_user.Email = user.Email;
                    found_user.Password = user.Password;
                    res.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
                }
                else
                {
                    res.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
                    res.ErrorMessages.Add("User with the same email exists");
                }
                
            }
            else
            {
                if(!context.Users.Where(x => x.Email.ToLower() == user.Email.ToLower()).Any())
                {
                    found_user = new User();

                    found_user.UserId = Guid.NewGuid();
                    found_user.FirstName = user.FirstName;
                    found_user.LastName = user.LastName;
                    found_user.Email = user.Email;
                    found_user.Password = user.Password;
                    found_user.UserType = CommonEnum.UserType.Agent;

                    context.Users.Add(found_user);

                    res.ReturnStatus = CommonEnum.AjaxReturnStatus.Success;
                }
                else
                {
                    res.ErrorMessages.Add("User with same email exists");
                    res.ReturnStatus = CommonEnum.AjaxReturnStatus.Error;
                }
            }

            context.SaveChanges();

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
