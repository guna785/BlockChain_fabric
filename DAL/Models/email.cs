using DAL.SharedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    [BsonCollection("email")]
    public class email : CommonModel
    {
        public string name { get; set; }
        public string smtpserver { get; set; }
        public string smtpport { get; set; }
        public string emailaddress { get; set; }
        public string password { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
    }
}
