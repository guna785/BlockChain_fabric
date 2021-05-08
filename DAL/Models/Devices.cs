using DAL.SharedModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [BsonCollection("Devices")]
    public class Devices:CommonModel
    {
        public string name { get; set; }
        public string mac { get; set; }
        public string userId { get; set; }
    }
}
