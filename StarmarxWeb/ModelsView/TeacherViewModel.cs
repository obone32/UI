using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StarmarxWeb.ModelsView
{
    public class TeacherViewModel
    {
        [Required(ErrorMessage = "Required")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Class { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Division { get; set; }
        public int? InstituteID { get; set; }
        public int? RoleID { get; set; }
        public int? GroupID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Required")]
        public string EmailID { get; set; }
        public int TeacherID { get; set; }
        public string WalletID { get; set; }

        public int IsActive { get; set; }

    }
}