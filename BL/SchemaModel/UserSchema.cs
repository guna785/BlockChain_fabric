using GSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class UserSchema
    {

        [GSchema("name", "Name", "string", true, getHtmlClass = "col-md-6")]
        public string name { get; set; }
        [GSchema("email", "Email", "email", true, getHtmlClass = "col-md-6")]
        public string email { get; set; }
        [GSchema("phone", "Phone", "number", true, getHtmlClass = "col-md-6")]
        public string phone { get; set; }
        [GSchema("type", "Role", "string", true, getEnumVal = "role", getHtmlClass = "col-md-6", getfieldHtmlClass = "select2 selectfield")]
        public string type { get; set; }
        [GSchema("uname", "User Name", "string", true, getHtmlClass = "col-md-6")]
        public string uname { get; set; }
        [GSchema("password", "Password", "password", true, getHtmlClass = "col-md-6")]
        public string password { get; set; }

        [GSchema("zone", "Zone", "string", true, getEnumVal = "zone", getHtmlClass = "col-md-6", getfieldHtmlClass = "select2 selectfield")]
        public string zone { get; set; }
        [GSchema("address", "Address", "textarea", true, getHtmlClass = "col-md-6")]
        public string address { get; set; }

        [GSchema("area", "Area", "string", true, getEnumVal = "area", getHtmlClass = "col-md-6", getfieldHtmlClass = "select2 selectfield")]
        public string area { get; set; }
        [GSchema("remarks", "Remarks", "textarea", true, getHtmlClass = "col-md-6")]
        public string remarks { get; set; }
    }
}
