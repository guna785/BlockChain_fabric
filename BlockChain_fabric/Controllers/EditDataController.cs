using BL.SchemaModel;
using BL.service;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Identity.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class EditDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IGenericBL<Logs> _log;
        private readonly IGenericBL<Devices> _device;

        public EditDataController(UserManager<ApplicationUser> user, IGenericBL<Devices> device, IGenericBL<Logs> log)
        {
            _user = user;
            _log = log;
            _device = device;
        }
        [HttpPost]
        public async Task<IActionResult> EditDevice([FromBody] EditDeviceSchema schema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            var res = await _device.ReplaceOneAsync(new Devices()
            {
                Id = ObjectId.Parse(schema.Id),
                CreatedAt = DateTime.Now,
                mac = schema.mac,
                name = schema.name,
                userId = schema.userId
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
    }
}
