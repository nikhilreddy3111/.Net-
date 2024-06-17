using Dapper;
using EmployeeManagement.DapperContent;
using EmployeeManagement.Modals;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDapperContent _dapperContent;
        public HomeController(ILogger<HomeController> logger, IDapperContent dapperContent)
        {
            _logger = logger;
            _dapperContent = dapperContent;
        }

        [Route("~/api/Home")]
        [Route("~/")]
        [HttpGet(nameof(Index))]
        public IActionResult Index()
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
