using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Models
{
    public class RecordData
    {
        public string temp { get; set; }
        public string humidity { get; set; }
        public bool doorSts { get; set; }
        public string lat { get; set; }
        public string lang { get; set; }
        public string mac { get; set; }
    }
}
