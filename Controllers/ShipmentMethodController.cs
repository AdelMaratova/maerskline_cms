using ContainerManagementSystem.CommonEnum;
using ContainerManagementSystem.Controllers.Shared;
using ContainerManagementSystem.Models.ShippingChannel;
using ContainerManagementSystem.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContainerManagementSystem.Controllers
{
    public class ShipmentMethodController : SharedControllerBase
    {
        public ActionResult ShipmentMethodList()
        {
            List<ShippingChannel> shipment_methods = context.ShippingChannels.ToList();

            ViewData.Model = shipment_methods;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        public ActionResult EditShipmentMethod(Guid shipping_channel_id)
        {
            ShippingChannel shipping_channel = context.ShippingChannels.Where(x => x.ShippingChannelId == shipping_channel_id).FirstOrDefault();

            if (shipping_channel == null)
                shipping_channel = new ShippingChannel();

            BindDropDownValues();

            ViewData.Model = shipping_channel;

            if (Request.IsAjaxRequest())
                return PartialView();

            return View();
        }

        private void BindDropDownValues()
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            SelectListItem listItem = new SelectListItem();

            listItems = new List<SelectListItem>();
            foreach (ShippingMethod method in Enum.GetValues(typeof(ShippingMethod)))
            {
                listItem = new SelectListItem();

                listItem.Text = method.ToString();
                listItem.Value = method.ToString();

                listItems.Add(listItem);
            }

            ViewData["ShippingMethod"] = listItems;
        }

        public JsonResult ShippingMethodCreateUpdate(ShippingChannel shipping_channel)
        {
            AjaxResponse ajax_response = new AjaxResponse();

            ShippingChannel found_channel = context.ShippingChannels.Where(x => x.ShippingChannelId == shipping_channel.ShippingChannelId).FirstOrDefault();

            if (found_channel != null)
            {
                found_channel.ShippingChannelId = shipping_channel.ShippingChannelId;
                found_channel.ShippingMethod = shipping_channel.ShippingMethod;
                found_channel.ShippingChannelName = shipping_channel.ShippingChannelName;
                found_channel.ShippingChannelsCapacity = shipping_channel.ShippingChannelsCapacity;
            }
            else
            {
                found_channel = new ShippingChannel() {
                    ShippingChannelId = shipping_channel.ShippingChannelId,
                    ShippingMethod = shipping_channel.ShippingMethod,
                    ShippingChannelName = shipping_channel.ShippingChannelName,
                    ShippingChannelsCapacity = shipping_channel.ShippingChannelsCapacity
                };

                context.ShippingChannels.Add(found_channel);
            }

            context.SaveChanges();

            ajax_response.ReturnStatus = AjaxReturnStatus.Success;
            ajax_response.ItemId = found_channel.ShippingChannelId;

            return Json(ajax_response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShippingMethodDelete(Guid shipping_channel_id)
        {
            AjaxResponse ajax_response = new AjaxResponse();

            if(!context.Shipments.Where(x => x.ShippingMethod == shipping_channel_id).Any())
            {
                ShippingChannel channel = context.ShippingChannels.Where(x => x.ShippingChannelId == shipping_channel_id).FirstOrDefault();
                context.ShippingChannels.Remove(channel);
                context.SaveChanges();

                ajax_response.ReturnStatus = AjaxReturnStatus.Success;
            }
            else
            {
                ajax_response.ReturnStatus = AjaxReturnStatus.Error;
                ajax_response.ErrorMessages.Add("Cannot delete the shipping method as it is in use");
            }

            return Json(ajax_response, JsonRequestBehavior.AllowGet);
        }
    }
}
