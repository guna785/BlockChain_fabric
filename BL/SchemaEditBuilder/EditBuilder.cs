using BL.SchemaModel;
using BL.service;
using DAL.DALService;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.SchemaEditBuilder
{
    public class EditBuilder
    {

       
        private IGenericBL<email> _emails;

        public EditBuilder( IGenericBL<email> emails)
        {
            _emails = emails;
        }
        public async Task<T> ReturnObjectData<T>(string id)
        {
            var obj = typeof(T).Name;
           

            if (obj.Equals("EmailSchema"))
            {
                var obdata = _emails.AsQueryable().FirstOrDefault() == null ? new email() : _emails.AsQueryable().FirstOrDefault();
                return (T)Convert.ChangeType(new EmailSchema()
                {
                    emailaddress = obdata.emailaddress,
                    name = obdata.name,
                    password = "",
                    smtpport = obdata.smtpport,
                    smtpserver = obdata.smtpserver,
                    Id = obdata.Id.ToString()

                }, typeof(T));
            }

            return (T)Convert.ChangeType(null, typeof(T));
        }
    }
}
