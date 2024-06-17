using EmployeeManagement.DapperContent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IDapperContent _dapperContent;

        public RolesController(RoleManager<IdentityRole> roleManager,IDapperContent dapperContent)
        {
            _roleManager = roleManager;
            _dapperContent = dapperContent;
        }

        [Route("~/api/Roles")]
        [HttpGet(nameof(Index))]
        public async Task<IActionResult> Index()
        {
            string query = "SELECT * FROM AspNetRoles";
            var roles = await Task.FromResult(_dapperContent.GetAll<IdentityRole>(query, null, commandType: CommandType.Text));
            return View(roles);
        }

        [HttpGet(nameof(Create))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole roleModel)
        {
            if (!await _roleManager.RoleExistsAsync(roleModel.Name))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));
            }
            return RedirectToAction("Index");
        }
    }
}
