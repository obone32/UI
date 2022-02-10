using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StarmarxWeb.Models
{
    public class EmailModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
    }
}