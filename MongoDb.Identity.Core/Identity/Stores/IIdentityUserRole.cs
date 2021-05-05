using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb.Identity.Core.Identity.Stores
{
    public interface IIdentityUserRole
    {
        List<string> Roles { get; set; }

        void AddRole(string role);

        void RemoveRole(string role);
    }
}
