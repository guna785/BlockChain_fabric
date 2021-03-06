using BL.service;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDb.Identity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class DeleteController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IGenericBL<Logs> _log;
        private readonly IGenericBL<Devices> _device;

        public DeleteController(UserManager<ApplicationUser> user, IGenericBL<Devices> device, IGenericBL<Logs> log)
        {
            _user = user;
            _log = log;
            _device = device;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDevice([FromBody] string Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }
            await _device.DeleteByIdAsync(Id);
            return Ok("Your Record has been deleted");
        }
    }
}
