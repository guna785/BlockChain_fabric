using DAL.SharedModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    [BsonCollection("otp")]
    public class otp : CommonModel
    {
        public string otpvalue { get; set; }
        public ObjectId userId { get; set; }
        public string zone { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
    }
}
