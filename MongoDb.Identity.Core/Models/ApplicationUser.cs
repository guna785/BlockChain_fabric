using Microsoft.AspNetCore.Identity;
using MongoDb.Identity.Core.Identity.Stores;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core.Models
{
    public class ApplicationUser : IdentityUser<ObjectId>, IIdentityUserRole
    {
        [PersonalData]
        public string Name { get; set; }
        public bool IsAdmin { get; set; }

        public string device { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public virtual List<string> Roles { get; set; }

        public ApplicationUser()
        {
            Roles = new List<string>();
        }

        public virtual void AddRole(string role)
        {
            Roles.Add(role);
        }

        public virtual void RemoveRole(string role)
        {
            Roles.Remove(role);
        }
    }
}
