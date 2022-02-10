using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarmarxWeb.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int CustomerID { get; set; }
        public int CustomerType { get; set; }
        public int EmailVerify { get; set; }
        public int MobileVerify { get; set; }
        public bool IsActive { get; set; }
        public int Active { get; set; }
    }
}