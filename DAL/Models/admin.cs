using DAL.SharedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    [BsonCollection("admin")]
    public class admin : CommonModel
    {
        public string uname { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string type { get; set; }
        public string phone { get; set; }
        public string status { get; set; }
        public string pan { get; set; }
        public byte[] photo { get; set; }
        public string remarks { get; set; }
    }
}
