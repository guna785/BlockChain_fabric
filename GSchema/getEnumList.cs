using DAL.DALrepo;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSchema
{
    public static class getEnumList
    {
        public async static Task<string> getEnumRecords(string val, string zone = "")
        {


            if (val.Equals("status"))
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>() { "Active", "InActive" });
            }
            //if (val.Equals("area"))
            //{
            //    GenericRepository<area> rep = new GenericRepository<area>();
            //    var ls = new List<string>();
            //    ls.Add("Not Linked");
            //    var iot = zone == "root" ? rep.AsQueryable().Where(x => x.status != "Deleted").GroupBy(x => new { x.name }).Select(x => x.Key.name).ToList() : rep.AsQueryable().Where(x => x.status != "Deleted" && x.zone == zone).Select(x => x.name).ToList();
            //    ls.AddRange(iot);
            //    return Newtonsoft.Json.JsonConvert.SerializeObject(ls);
            //}

            if (val.Contains("month"))
            {
                var ls = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames);
                return Newtonsoft.Json.JsonConvert.SerializeObject(ls);
            }
            if (val.Contains("year"))
            {
                var ls = new List<string>();
                int y = 2017;
                for (int i = 0; i <= DateTime.Now.Year; i++)
                {
                    ls.Add(y.ToString());
                    y++;
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(ls);
            }

            return "";
        }

        public async static Task<string> getVlidationMessage(string val)
        {
            var msg = new
            {
                required = val + " is Required Property",
                pattern = "Correct format of " + val

            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(msg);
        }
    }
}
