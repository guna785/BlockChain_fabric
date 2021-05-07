using DAL.SharedModel;
using MerkleTree;
using MongoDB.Bson;
using Sawtooth.BlockChain;
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
        public Blockchain chain { get; set; }
        public DemoMerkleTree mercleTree { get; set; }
    }
}
