using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.SharedModel
{
    public abstract class CommonModel
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
