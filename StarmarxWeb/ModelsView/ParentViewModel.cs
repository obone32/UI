using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarmarxWeb.ModelsView
{
    public class ParentViewModel
    {
        public int ParentID { get; set; }
        [Required(ErrorMessage = "Required")]
        public int StudentID { get; set; }
        [Required(ErrorMessage = "Required")]
        public string ParentGuardianName { get; set; }
        public string WalletID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Required")]
        public string EmailID { get; set; }

        public int IsActive { get; set; }
    }
}