using DAL.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    [BsonCollection("KeyMaintainer")]
    public class KeyMaintainer:CommonModel
    {
        public string uname { get; set; }
        public string keysettings { get; set; }
        public string certficate { get; set; }
    }
}
