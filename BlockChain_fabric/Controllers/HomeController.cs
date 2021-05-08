using BL.SchemaEditBuilder;
using BL.SchemaModel;
using BlockChain_fabric.Models;
using GSchema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EditBuilder _builder;
        public HomeController(ILogger<HomeController> logger, EditBuilder builder)
        {
            _logger = logger;
            _builder = builder;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
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
                schema = await GSgenerator.GenerateSchema<DeviceSchema>("");
                ViewBag.modalTitle = "AddDevice";
            }
            else if (ID.Contains("EditDevice"))
            {
                var objId = ID.Split('-')[1];
                var data = await _builder.ReturnObjectData<EditDeviceSchema>(objId);

                ViewBag.val = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                schema = await GSgenerator.GenerateSchema<EditDeviceSchema>("");
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
