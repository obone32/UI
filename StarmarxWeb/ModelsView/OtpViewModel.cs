using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarmarxWeb.ModelsView
{
    public class OtpViewModel
    {
        public int UserID { get; set; }
        [Required(ErrorMessage = "Required")]
        public int OTP { get; set; }
        public string Receiver { get; set; }
        public string Status { get; set; }
        public int Type { get; set; }
    }
}