using ContainerManagementSystem.CommonEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.ShipmentHistory
{
    [Table("ShipmentHistory")]
    public class ShipmentHistory
    {
        public Guid ShipmentHistoryId {get;set;}
		public Guid ShipmentId {get;set;}
        public ShipmentStatus ShipmentStatus { get; set; }
		public Guid DispatchedCountry {get;set;}
		public Guid DispatchedCity {get;set;}
		public Guid? ReceivedCountry {get;set;}
		public Guid? ReceivedCity {get;set;}
		public DateTime UpdatedDate {get;set;}
    }
}