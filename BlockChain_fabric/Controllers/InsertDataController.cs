using BL.Models;
using BL.SawtoothClient;
using BL.SchemaModel;
using BL.service;
using BlockChain_fabric.Models;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Identity.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class InsertDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IGenericBL<Logs> _log;
        private readonly IGenericBL<Devices> _device;
        private readonly ClientKeyManager _clientKey;
        public InsertDataController(UserManager<ApplicationUser> user, IGenericBL<Devices> device, IGenericBL<Logs> log, ClientKeyManager clientKey)
        {
            _user = user;
            _log = log;
            _device = device;
            _clientKey = clientKey;
        }
        [HttpPost]
        public async Task<IActionResult> InsertDevice([FromBody] DeviceSchema schema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            var res = await _device.InsertOneAsync(new Devices()
            {
                CreatedAt = DateTime.Now,
                mac = schema.mac,
                name = schema.name,
                userId =schema.userId
            });
            if (res)
            {
                var usr = _user.Users.Where(x => x.UserName == schema.userId).FirstOrDefault();
                await _log.InsertOneAsync(
                    new Logs()
                    {
                        userId = usr.Id,
                        message = "Device " + schema.name + " is Added Successfully",
                        events = "Event",
                        subject = "Insertion",
                        CreatedAt = DateTime.Now
                    });
                return Ok(new { status = "Device " + schema.name + " is Added Successfully" });
            }
            return BadRequest("Invalid Request");

        }

        /// <summary>
        ///           /InsertData/PostData
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostData([FromBody] RecordData data)
        {
            var div = _device.AsQueryable().Where(x => x.mac == data.mac).FirstOrDefault();

            ClientAccess access = new ClientAccess();
            var res = access.postMedicalRecord(div.userId, "set", Convert.ToBase64String(Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(data))));
            if (res != null)
            {
                 var r= await  _clientKey.SwatoothKeyStore(res.encoderSettings, res.obj);
                return Ok("Success");
            }

            return BadRequest("Invalid Request");
        }
    }
}
