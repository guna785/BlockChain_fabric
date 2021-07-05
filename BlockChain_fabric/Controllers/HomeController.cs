using BL.Models;
using BL.SawtoothClient;
using BL.SchemaEditBuilder;
using BL.SchemaModel;
using BlockChain_fabric.Models;
using GSchema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDb.Identity.Core.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly ILogger<HomeController> _logger;
        private readonly EditBuilder _builder;
        private readonly GSgenerator _gSgenerator;
        private readonly ClientKeyManager _clientKey;
        public HomeController(ILogger<HomeController> logger, EditBuilder builder, GSgenerator gSgenerator, ClientKeyManager clientKey,
            UserManager<ApplicationUser> user)
        {
            _logger = logger;
            _builder = builder;
            _gSgenerator = gSgenerator;
            _clientKey = clientKey;
            _user = user;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            return View();
        }
        public IActionResult Records()
        {
            var dlist = new List<MRecord>();
            ViewBag.mRecord = dlist;
            ViewBag.user = HttpContext.User.IsInRole("Admin") ? "" : _user.Users.Where(x => x.Id == ObjectId.Parse(HttpContext.User.Identity.Name)).FirstOrDefault().UserName;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Records(RecordSearch record)
        {
            ViewBag.user = HttpContext.User.IsInRole("Admin") ? "" : _user.Users.Where(x => x.Id == ObjectId.Parse(HttpContext.User.Identity.Name)).FirstOrDefault().UserName;

            var dlist = new List<MRecord>();
            if (!ModelState.IsValid)
            {
                ViewBag.mRecord = dlist;
            }
            else
            {
                ClientAccess client = new ClientAccess();
                var res = client.GetMedicalRecord(record.data);                
                ViewBag.mRecord = await _clientKey.SwatoothRetriveData(res,record.data);
            }
            return View();
        }
        public IActionResult Devices()
        {
            return View();
        }
        
        public IActionResult Logs()
        {
            return View();
        }
        private string schema;
        public async Task<IActionResult> PopUpModelShow(string ID)
        {

            if (ID.Contains("AddDevice"))
            {
                schema = await _gSgenerator.GenerateSchema<DeviceSchema>("");
                ViewBag.modalTitle = "AddDevice";
            }
            else if (ID.Contains("EditDevice"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditDeviceSchema>(objId);

                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await _gSgenerator.GenerateSchema<EditDeviceSchema>("");
                ViewBag.modalTitle = "EditDevice";
            }

            ViewBag.schema = schema;

            return View();
        }        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
