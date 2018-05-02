using ContainerManagementSystem.CommonEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContainerManagementSystem.Models.System
{
    public class AjaxResponse
    {
        public AjaxReturnStatus ReturnStatus { get; set; }

        private string _StrReturnStatus;
        public string StrReturnStatus
        {
            get
            {
                _StrReturnStatus = ReturnStatus.ToString();
                return _StrReturnStatus;
            }
            set
            {
                value = ReturnStatus.ToString();
            }
        }

        private List<string> _ErrorMessages;
        public List<string> ErrorMessages
        {
            get
            {
                if (_ErrorMessages == null)
                    _ErrorMessages = new List<string>();

                return _ErrorMessages;
            }
            set
            {
                _ErrorMessages = value == null ? new List<string>() : value;
            }
        }

        public Guid ItemId { get; set; }
    }
}