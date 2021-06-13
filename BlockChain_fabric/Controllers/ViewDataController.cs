using BL.DataTableModel;
using BL.Extentions;
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
using System.Threading.Tasks;

namespace BlockChain_fabric.Controllers
{
    public class ViewDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IGenericBL<Logs> _log;
        private readonly IGenericBL<Devices> _device;

        public ViewDataController(UserManager<ApplicationUser> user, IGenericBL<Devices> device, IGenericBL<Logs> log)
        {
            _user = user;
            _log = log;
            _device = device;
        }
        [HttpPost]
        public async Task<IActionResult> LoadLogs([FromBody] DtParameters parameters)
        {
            var searchBy = parameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (parameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = "CreatedAt";
                orderAscendingDirection = parameters.Order[0].Dir.ToString().ToLower() != "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CreatedAt";
                orderAscendingDirection = false;
            }

            var result = HttpContext.User.IsInRole("Admin") ? _log.AsQueryable() : _log.AsQueryable().Where(x => x.userId == ObjectId.Parse(HttpContext.User.Identity.Name));

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.events != null && r.events.ToUpper().Contains(searchBy.ToUpper()) ||
                                          r.message != null && r.message.ToUpper().Contains(searchBy.ToUpper())
                                         );
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = HttpContext.User.IsInRole("Admin") ? _log.AsQueryable() : _log.AsQueryable().Where(x => x.userId == ObjectId.Parse(HttpContext.User.Identity.Name));
            var u = _user.Users.AsEnumerable();

            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = parameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(parameters.Start)
                    .Take(parameters.Length)
                    .AsEnumerable().Select(x => new LogModel()
                    {
                        userId = u.SingleOrDefault(s => s.Id == x.userId).UserName,
                        message = x.message,
                        events = x.events,
                        createdAt = x.Id.CreationTime
                    }).ToList()
            });
        }
        [HttpPost]
        public IActionResult LoadUserList([FromBody] DtParameters parameters)
        {
            var searchBy = parameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (parameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = "CreatedAt";
                orderAscendingDirection = parameters.Order[0].Dir.ToString().ToLower() != "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CreatedAt";
                orderAscendingDirection = false;
            }

            var result = _user.Users.Where(x => x.IsAdmin == false);

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Email != null && r.Email.ToUpper().Contains(searchBy.ToUpper())
                                          );
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = _user.Users.Where(x => x.IsAdmin == false);

            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = parameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(parameters.Start)
                    .Take(parameters.Length)
                    .AsEnumerable().Select(x => new ApplicationUser()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        UserName = x.UserName,
                        gender = x.gender,
                        age = x.age,
                        createdAt = x.Id.CreationTime
                    }).ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadDevices([FromBody] DtParameters parameters)
        {
            var searchBy = parameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (parameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = "CreatedAt";
                orderAscendingDirection = parameters.Order[0].Dir.ToString().ToLower() != "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CreatedAt";
                orderAscendingDirection = false;
            }

            var result =  _device.AsQueryable() ;

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                          r.userId != null && r.userId.ToUpper().Contains(searchBy.ToUpper())
                                         );
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = _device.AsQueryable();

            var totalResultsCount = cntdb.Count();

            return Json(new
            {
                draw = parameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                    .Skip(parameters.Start)
                    .Take(parameters.Length).ToList()
            });
        }
        [HttpPost]
        public async Task<IActionResult> LoadMedicalData([FromBody] DtParameters parameters)
        {
            var searchBy = parameters.Search?.Value;
            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (parameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = "CreatedAt";
                orderAscendingDirection = parameters.Order[0].Dir.ToString().ToLower() != "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "CreatedAt";
                orderAscendingDirection = false;
            }

            var result = _device.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.name != null && r.name.ToUpper().Contains(searchBy.ToUpper()) ||
                                          r.userId != null && r.userId.ToUpper().Contains(searchBy.ToUpper())
                                         );
            }

            result = orderAscendingDirection ? result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var cntdb = _device.AsQueryable();

            var totalResultsCount = cntdb.Count();
            return Json(new
            {
                draw = parameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                   .Skip(parameters.Start)
                   .Take(parameters.Length).ToList()
            });
        }
    }
}
