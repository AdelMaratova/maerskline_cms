using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ContainerManagementSystem.CommonEnum;

namespace ContainerManagementSystem.Models.ShippingChannel
{
    [Table("ShippingChannels")]
    public class ShippingChannel
    {
        public Guid ShippingChannelId { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
		public string ShippingChannelName {get;set;}
		public int ShippingChannelsCapacity {get;set;}
    }
}