using GSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.SchemaModel
{
    public class EmailSchema
    {
        [GSchema("Id", "ID", "hidden", true)]
        public string Id { get; set; }
        [GSchema("name", "Name", "string", true, getHtmlClass = "col-md-12")]
        public string name { get; set; }
        [GSchema("smtpserver", "SMTP Server", "string", true, getHtmlClass = "col-md-12")]
        public string smtpserver { get; set; }
        [GSchema("smtpport", "SMTP Port", "string", true, getHtmlClass = "col-md-12")]
        public string smtpport { get; set; }
        [GSchema("zone", "Zone", "string", true, getEnumVal = "zone", getHtmlClass = "col -md-12", getfieldHtmlClass = "select2 selectfield")]
        public string zone { get; set; }
        [GSchema("emailaddress", "Email Address", "string", true, getHtmlClass = "col-md-12")]
        public string emailaddress { get; set; }
        [GSchema("password", "Password", "password", true, getHtmlClass = "col-md-12")]
        public string password { get; set; }

    }
}
