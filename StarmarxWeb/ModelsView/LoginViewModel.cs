using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarmarxWeb.ModelsView
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}