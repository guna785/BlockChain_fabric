using DAL.SharedModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [BsonCollection("UserCondifetialInfo")]
    public class UserCondifetialInfo:CommonModel
    {
        public ObjectId userId { get; set; }
        public string deviceMac { get; set; }
        public bool doorSts  { get; set; }
    }
}
