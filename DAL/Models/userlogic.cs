using DAL.SharedModel;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    [BsonCollection("userlogic")]
    public class userlogic : CommonModel
    {
        public ObjectId uid { get; set; }
        public byte[] usrToken { get; set; }
    }
}
