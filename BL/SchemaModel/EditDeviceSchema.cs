using GSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SchemaModel
{
    public class EditDeviceSchema
    {
        [GSchema("Id", "ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
        [GSchema("mac", "Mac Address", "string", true, getHtmlClass = "col-md-12")]
        public string mac { get; set; }
        [GSchema("userId", "Assigned To", "string", true, getEnumVal = "user", getHtmlClass = "col-md-12")]
        public string userId { get; set; }
    }
}
