using DAL.SharedModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [BsonCollection("Logs")]
    public class Logs : CommonModel
    {
        public string events { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public ObjectId userId { get; set; }
    }
}
