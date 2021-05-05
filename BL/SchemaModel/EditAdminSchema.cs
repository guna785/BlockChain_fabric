using GSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class EditAdminSchema
    {
        [GSchema("Id", "ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("uname", "User Name", "string", true, getHtmlClass = "col-md-6")]
        public string uname { get; set; }
        [GSchema("zone", "Zone", "string", true, getEnumVal = "zone", getHtmlClass = "col -md-6", getfieldHtmlClass = "select2 selectfield")]
        public string zone { get; set; }
        [GSchema("name", "Name", "string", true, getHtmlClass = "col-md-6")]
        public string name { get; set; }
        [GSchema("password", "Password", "password", false, getHtmlClass = "col-md-6")]
        public string password { get; set; }
        [GSchema("address", "Address", "string", true, getHtmlClass = "col-md-12")]
        public string address { get; set; }
        [GSchema("email", "Email", "email", true, getHtmlClass = "col-md-6")]
        public string email { get; set; }

        [GSchema("phone", "Phone", "number", true, getHtmlClass = "col-md-6")]
        public string phone { get; set; }
        [GSchema("status", "Status", "string", true, getEnumVal = "status", getHtmlClass = "col-md-6", getfieldHtmlClass = "select2 selectfield")]
        public string status { get; set; }
        [GSchema("gst", "GST No", "string", true, getHtmlClass = "col-md-6")]
        public string gst { get; set; }
        [GSchema("photo", "Photo", "file", false, getHtmlClass = "col-md-6")]
        public byte[] photo { get; set; }
        [GSchema("remarks", "Remarks", "string", false, getHtmlClass = "col-md-6")]
        public string remarks { get; set; }
    }
}
