using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Models
{
    public class LogModel
    {
        public string events { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string userId { get; set; }
        public DateTime createdAt { get; set; }
    }
}
