using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.CommonEnum
{
    public enum ShippingMethod
    {
        Plane,
        Ship,
        Vehicle
    }

    public enum AjaxReturnStatus
    {
        Success,
        Error
    }

    public enum ShipmentStatus
    { 
        New,
        Dispatched,
        Arrived,
        Delivered,
        OutForDelivery,
        Pending,
        Cancelled
    }

    public enum UserType
    { 
        Admin = 1,
        Agent = 2,
        Customer = 3
    }

    public class CommonEnum
    {
        
    }
}